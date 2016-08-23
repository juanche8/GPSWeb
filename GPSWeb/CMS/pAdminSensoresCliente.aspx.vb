'pagina para configurar alertas de sensores para un movil
Imports GPS.Business
Imports GPS.Data
Public Class pAdminSensores
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'busco los sensores creados, y los sensores que tieen asignados este cliente
        Try
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then

                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()

                        Dim movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
                        lblCliente.Text = movil.veh_descripcion + "-" + movil.veh_patente
                    End If
                  
                    'recupero los sensores configurados para este cliente                
                    datagridSensores.DataSource = clsCategoriaAlarma.ListSensores()
                    datagridSensores.DataBind()

                End If

            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception

            lblError.Text = "Ocurrio un error recuperando las configuraciones." + ex.Message + " - " + ex.StackTrace
        End Try
    End Sub

    'marco los sensores configurados para este cliente
    Protected Sub datagrid_itemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim _sensor As Sensores = clsCategoriaAlarma.SensorByMovil(CInt(hdnveh_id.Value), DirectCast(e.Item.DataItem, Sensores).sen_id)

            If _sensor IsNot Nothing Then
                DirectCast(e.Item.FindControl("chkAvisarme"), CheckBox).Checked = True
                DirectCast(e.Item.FindControl("chkSMS"), CheckBox).Checked = _sensor.Sensores_Moviles.FirstOrDefault().enviar_sms
                
            End If
        End If
    End Sub


    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try

            'eliminio las configuraciones anteriores para el movil - sensor
            'recorro la grilla y guardo la nueva configuracion
            clsCategoriaAlarma.DeleteSensorByMovil(CInt(hdnveh_id.Value))
            For Each row As DataGridItem In datagridSensores.Items


                If DirectCast(row.FindControl("chkAvisarme"), CheckBox).Checked Then
                    'verifico los vehiculos, inserto una alerta por cada uno

                    Dim _sensorCliente As Sensores_Moviles = New Sensores_Moviles()

                    _sensorCliente.veh_id = CInt(hdnveh_id.Value)
                    _sensorCliente.sen_id = CInt(datagridSensores.DataKeys(row.ItemIndex).ToString())
                    _sensorCliente.enviar_sms = DirectCast(row.FindControl("chkSMS"), CheckBox).Checked

                    clsCategoriaAlarma.InsertSensorMovil(_sensorCliente)

                End If

            Next
            'retorno
            Response.Redirect("~/CMS/pAdminVehiculos.aspx", False)

        Catch ex As Exception

            lblError.Text = "Ocurrio un error grabando las configuraciones. " + ex.Message + " - " + ex.StackTrace
        End Try
    End Sub
End Class