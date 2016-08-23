'pagina para configurar alertas genericas por parte de los clientes
Imports GPS.Business
Imports GPS.Data
Partial Public Class pAlarmaAdd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                 
                    hdncli_id.Value = Session("Cliente").ToString()

                    'verifico si tiene grupos y cargo el combo
                    Dim _grupos As List(Of Grupos) = clsGrupo.Search(CInt(hdncli_id.Value))
                    PanelGrupo.Visible = False
                    If _grupos.Count > 0 Then

                        ddlgrupo.DataSource = _grupos
                        ddlgrupo.DataBind()

                        ddlgrupo.Items.Insert(0, New ListItem("--Seleccione--", 0))

                        PanelGrupo.Visible = True
                    End If
                    'cargo los vehiculos activos

                    PanelMoviles.Visible = True
                    cargarMoviles()

                    'cargo las alarmas de exceso de velocidad definidas
                   
                    datagridAlarmas.DataSource = clsCategoriaAlarma.ListExcVelocidad()
                    datagridAlarmas.DataBind()

                  
                End If

            Else
                'no esta logeado
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "redirect", "parent.iraLogin();", True)
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMA VELOCIDAD ADD - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error recuperando las configuraciones, contacte al administrador."
        End Try
    End Sub



    Protected Sub grid_DataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        'If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then



        '    Dim alarma As Alarmas_Velocidad = DirectCast(e.Item.DataItem, Alarmas_Velocidad)
        '    Dim veh_id As Integer = 0
        '    'busco el primer auto chequeado
        '    For Each check As ListItem In chkMoviles.Items

        '        If check.Selected Then
        '            veh_id = CInt(check.Value)
        '            Exit For
        '        End If
        '    Next

        '    Dim velocidad As Alertas_Velocidad_Configuradas = clsAlarma.SelectAlarmaByVelocidad(veh_id, alarma.vel_tipo_via)

        '    If velocidad IsNot Nothing Then
        '        DirectCast(e.Item.FindControl("txtValor"), TextBox).Text = velocidad.ale_valor_maximo
        '        DirectCast(e.Item.FindControl("chkAvisarme"), CheckBox).Checked = True
        '        DirectCast(e.Item.FindControl("chkMail"), CheckBox).Checked = velocidad.ale_enviar_mail
        '    End If

        'End If
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        'guardo la configuracion elegida por el usuario
        Try
            Dim selecciono = False
            Dim seleccionoMovil = False
            Dim strMoviles As String = ""
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("chkMoviles"), CheckBox)

                If rdnMovil.Checked Then
                    If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                    strMoviles = strMoviles + DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                End If

            Next
            If strMoviles = "" Then
                lblError.Text = "Seleccione el o los Moviles para configurar la alarmar."
                Exit Sub
            End If

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

            'elimino la configuracion que puedan tener el cleinte para todos los moviles y lo vuelvo a creas

            clsAlarma.DeleteByCliente(CInt(hdncli_id.Value))


            For Each row As DataGridItem In datagridAlarmas.Items



                If DirectCast(row.FindControl("chkAvisarme"), CheckBox).Checked Then

                    'verifico los vehiculos, inserto una alerta por cada uno

                    For Each row1 As DataListItem In DataListVehiculos.Items
                        Dim check As CheckBox = DirectCast(row1.FindControl("chkMoviles"), CheckBox)

                        If check.Checked Then
                            Dim alertaUsuario As Alertas_Velocidad_Configuradas = New Alertas_Velocidad_Configuradas()

                            alertaUsuario.veh_id = CInt(DataListVehiculos.DataKeys(row1.ItemIndex).ToString())
                            alertaUsuario.ale_fecha_creacion = DateTime.Now
                            alertaUsuario.vel_id = CInt(datagridAlarmas.DataKeys(row.ItemIndex).ToString())


                            '  If DirectCast(row.FindControl("chkSMS"), CheckBox).Checked Then
                            'alertaUsuario.ale_enviar_SMS = True
                            'Else
                            alertaUsuario.ale_enviar_SMS = False
                            'End If

                            If DirectCast(row.FindControl("chkMail"), CheckBox).Checked Then
                                alertaUsuario.ale_enviar_mail = True
                            Else
                                alertaUsuario.ale_enviar_mail = False
                            End If

                            alertaUsuario.ale_valor_maximo = DirectCast(row.FindControl("txtValor"), TextBox).Text

                            clsAlarma.InsertAlarmaConfigurada(alertaUsuario)

                        End If
                    Next


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

        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(Session("Cliente").ToString())
      


        If vehiculos.Count > 0 Then
            DataListVehiculos.DataSource = vehiculos
            DataListVehiculos.DataBind()
        Else
            lblError.Text = "No posee Moviles Activos para definir Alarmas. Contacte al Administrador."
        End If
        
    End Sub

 
    Protected Sub ddlgrupo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlgrupo.SelectedIndexChanged
        Try
            'cargo lso moviles del grupo
            'si selecciono ver todos los moviles los cargo a todos
            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivosGrupo(CInt(ddlgrupo.SelectedValue), "")
             If vehiculos.Count > 0 Then
                DataListVehiculos.DataSource = vehiculos
                DataListVehiculos.DataBind()
            Else
                lblError.Text = "No posee Moviles Activos para definir Alarmas. Contacte al Administrador."
            End If

            PanelMoviles.Visible = True
            chkTodos.Checked = False
        
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub chkTodos_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodos.CheckedChanged
        'muestro todos los auots del cliente
        If chkTodos.Checked Then
            cargarMoviles()
            ddlgrupo.SelectedValue = "0"
            PanelMoviles.Visible = True
            For Each item As DataListItem In DataListVehiculos.Items

                Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
                movil.Checked = True
            Next
        Else
            For Each item As DataListItem In DataListVehiculos.Items

                Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
                movil.Checked = False
            Next
        End If

    End Sub

    Protected Sub LinkDestildar_Click(sender As Object, e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = False
        Next
    End Sub

    Protected Sub LinkTildar_Click(sender As Object, e As EventArgs) Handles LinkTildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = True
        Next
    End Sub
End Class