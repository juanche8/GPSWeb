'pagina para configurar alertas de sensores por parte de los clientes
Imports GPS.Business
Imports GPS.Data
Partial Public Class pAlarmaSensorAdd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then

                 
                    hdncli_id.Value = Session("Cliente").ToString()
                    'cargo los vehiculos activos
                    cargarMoviles()
                    
                End If

            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMA SENSOR ADD - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error recuperando las configuraciones, contacte al administrador."
        End Try
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        'guardo la configuracion elegida por el usuario
        Try
            Dim selecciono = False
            Dim moviles As List(Of Vehiculo) = clsVehiculo.ListActivos(CInt(hdncli_id.Value))
            'valido que haya seleccionado alguna alarma para configurar sino le aviso
            For Each row As DataGridItem In datagridAlarmas.Items
                If DirectCast(row.FindControl("chkAvisarme"), CheckBox).Checked Then
                    selecciono = True
                End If
            Next

            If Not selecciono Then
                lblError.Text = "Seleccione las Alarmas que quiere configurar marcando la opción 'Avisarme'."
                Exit Sub
            End If
            'eliminio las configuraciones anteriores para el movil - categoria
            'recorro la grilla y guardo la nueva configuracion

            'elimino la configuracion que puedan tener los moviles seleccionados

          
            clsAlarma.DeleteSensorByMovil(CInt(ddlMoviles.SelectedValue))
             
            For Each row As DataGridItem In datagridAlarmas.Items


                If DirectCast(row.FindControl("chkAvisarme"), CheckBox).Checked Then

                    'verifico los vehiculos, inserto una alerta por cada uno


                    Dim alertaUsuario As Sensores_Configurados = New Sensores_Configurados()

                    alertaUsuario.veh_id = CInt(ddlMoviles.SelectedValue)
                    alertaUsuario.sen_id = CInt(datagridAlarmas.DataKeys(row.ItemIndex).ToString())
                 
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


    Private Sub cargarMoviles()

        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(CInt(Session("Cliente").ToString()))

        If vehiculos.Count > 0 Then
            Dim cboitem As ListItem

            For Each movil As Vehiculo In vehiculos
                cboitem = New ListItem(movil.veh_descripcion + " - " + movil.veh_patente, movil.veh_id.ToString)

                ddlMoviles.Items.Add(cboitem)
            Next
            ddlMoviles.Items.Insert(0, New ListItem("Seleccione Vehiculo", 0))
        Else
            lblError.Text = "No posee Moviles Activos para definir Alarmas. Contacte al Administrador."
        End If

    End Sub

    
    Protected Sub ddlMoviles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMoviles.SelectedIndexChanged
        Dim _sensores As List(Of Sensores) = clsCategoriaAlarma.ListSensoresByMovil(CInt(ddlMoviles.SelectedValue))
        datagridAlarmas.DataSource = _sensores
        datagridAlarmas.DataBind()


        If _sensores.Count = 0 Then
            lblError.Text = "Este Movil no tiene asignado sensores. Contacte al Administrador."
        Else
            PanelTildar.Visible = True
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


