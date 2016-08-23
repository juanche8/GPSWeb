'pagina para configurar alertas de sensores para un movil
Imports GPS.Business
Imports GPS.Data
Partial Public Class pAlarmaSensorEdit
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then

                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()

                        Dim movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
                        lblMovil.Text = movil.veh_descripcion + "-" + movil.veh_patente
                    End If
                    hdncli_id.Value = Session("Cliente").ToString()
                   
                    'recupero los sensores configurados para este cliente                
                    datagridAlarmas.DataSource = clsCategoriaAlarma.ListSensoresByMovil(CInt(hdnveh_id.Value))
                    datagridAlarmas.DataBind()

                End If

            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMA SENSOR EDIT - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error recuperando las configuraciones, contacte al administrador."
        End Try
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        'guardo la configuracion elegida por el usuario
        Try
           
            'eliminio las configuraciones anteriores para el movil - sensor
            'recorro la grilla y guardo la nueva configuracion
            For Each row As DataGridItem In datagridAlarmas.Items

                clsAlarma.DeleteSensorByMovil(CInt(datagridAlarmas.DataKeys(row.ItemIndex).ToString()), CInt(hdnveh_id.Value))
                If DirectCast(row.FindControl("chkAvisarme"), CheckBox).Checked Then
                    'verifico los vehiculos, inserto una alerta por cada uno

                    Dim alertaUsuario As Sensores_Configurados = New Sensores_Configurados()

                    alertaUsuario.veh_id = CInt(hdnveh_id.Value)
                    alertaUsuario.sen_id = CInt(datagridAlarmas.DataKeys(row.ItemIndex).ToString())
                    ' If DirectCast(row.FindControl("chkSMS"), CheckBox).Checked Then
                    'alertaUsuario.sen_enviar_sms = True
                    'Else
                    ' alertaUsuario.sen_enviar_sms = False
                    'End If

                    If DirectCast(row.FindControl("chkMail"), CheckBox).Checked Then
                        alertaUsuario.sen_enviar_mail = True
                    Else
                        alertaUsuario.sen_enviar_mail = False
                    End If

                    clsAlarma.InsertAlarmaSensor(alertaUsuario)

                End If

            Next
            'retorno
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)

        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMA ADD - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error grabando las configuraciones, contacte al administrador."
        End Try

    End Sub

    'marco los sensores configurados para este movil
    Protected Sub datagrid_itemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim _sensor As Sensores = DirectCast(e.Item.DataItem, Sensores)
            Dim _alarma As Sensores_Configurados = clsAlarma.AlertaSensorSelect(_sensor.sen_id, CInt(hdnveh_id.Value))

            If _alarma IsNot Nothing Then
                DirectCast(e.Item.FindControl("chkAvisarme"), CheckBox).Checked = True

                If _alarma.sen_enviar_mail Then DirectCast(e.Item.FindControl("chkMail"), CheckBox).Checked = True
                ' If _alarma.sen_enviar_sms Then DirectCast(e.Item.FindControl("chkMail"), CheckBox).Checked = True
            End If
        End If
    End Sub

    Protected Sub LinkTildar_Click(sender As Object, e As EventArgs) Handles LinkTildar.Click
        For Each item As DataGridItem In datagridAlarmas.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkAvisarme"), CheckBox)
            movil.Checked = True
        Next
    End Sub

    Protected Sub LinkDestildar_Click(sender As Object, e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataGridItem In datagridAlarmas.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkAvisarme"), CheckBox)
            movil.Checked = False
        Next

    End Sub
End Class