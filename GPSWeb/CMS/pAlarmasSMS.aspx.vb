'pagina para configurar si se envia o no SMS ante alertas
Imports GPS.Business
Imports GPS.Data
Public Class pAlarmasSMS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'buscolos tipos de alarmas y la configuracion guardada
        Try
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then

                    ddlCliente.DataSource = clsCliente.ListActivos()
                    ddlCliente.DataBind()
                    ddlCliente.Items.Insert(0, New ListItem("Seleccione Cliente", 0))
                    ddlMoviles.Items.Insert(0, New ListItem("Seleccione Vehiculo", 0))
                End If

            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception

            lblError.Text = "Ocurrio un error recuperando las configuraciones." + ex.Message + " - " + ex.StackTrace
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try

            'recorro la grilla y guardo la nueva configuracion

            For Each row As DataGridItem In datagridAlarmas.Items

                clsAlarma.UpdateAlertaEnvioSMS(CInt(ddlMoviles.SelectedValue), CInt(datagridAlarmas.DataKeys(row.ItemIndex).ToString()), DirectCast(row.FindControl("chkEnviar"), CheckBox).Checked)
            Next
            'retorno
            Response.Redirect("~/CMS/pAdminAlarmas.aspx", False)

        Catch ex As Exception

            lblError.Text = "Ocurrio un error grabando las configuraciones. " + ex.Message + " - " + ex.StackTrace
        End Try
    End Sub

    Private Sub cargarMoviles()

        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(CInt(ddlCliente.SelectedValue))
        ddlMoviles.Items.Clear()
        If vehiculos.Count > 0 Then
            Dim cboitem As ListItem

            For Each movil As Vehiculo In vehiculos
                cboitem = New ListItem(movil.veh_descripcion + " - " + movil.veh_patente, movil.veh_id.ToString)

                ddlMoviles.Items.Add(cboitem)
            Next
            ddlMoviles.Items.Insert(0, New ListItem("Seleccione Vehiculo", 0))
        Else
            lblError.Text = "El Cliente posee Moviles Activos para definir Alarmas."
        End If

    End Sub

    Protected Sub datagrid_itemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim _sensor As Sensores_Moviles = clsCategoriaAlarma.GetSensorAsignado(CInt(ddlMoviles.SelectedValue), DirectCast(e.Item.DataItem, Sensores).sen_id)

            If _sensor IsNot Nothing Then
                If _sensor.enviar_sms Then
                    DirectCast(e.Item.FindControl("chkEnviar"), CheckBox).Checked = True

                End If
            End If
        End If
    End Sub

    Protected Sub ddlMoviles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMoviles.SelectedIndexChanged
        Dim _sensores As List(Of Sensores) = clsCategoriaAlarma.ListSensoresByMovil(CInt(ddlMoviles.SelectedValue))
        datagridAlarmas.DataSource = _sensores
        datagridAlarmas.DataBind()


        If _sensores.Count = 0 Then
            lblError.Text = "Este Movil no tiene asignado sensores."
        End If
    End Sub

    Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        cargarMoviles()
    End Sub
End Class