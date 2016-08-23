'pagina para editar las alertas configuradas
Imports GPS.Business
Imports GPS.Data
Partial Public Class pAlarmaEdit
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then

                    hdncli_id.Value = Session("Cliente").ToString()
                    hdnveh_id.Value = Request.Params("veh_id").ToString()

                    'busco datos del movil
                    Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))

                    lblMovil.Text = _movil.veh_descripcion + "-" + _movil.veh_patente
                    'cargo las alarmas de exceso de velocidad definidas

                    datagridAlarmas.DataSource = clsCategoriaAlarma.ListExcVelocidad()
                    datagridAlarmas.DataBind()


                End If

            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMA VELOCIDAD EDIT- " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error recuperando las configuraciones, contacte al administrador."
        End Try
    End Sub



    Protected Sub grid_DataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then



            Dim alarma As Alarmas_Velocidad = DirectCast(e.Item.DataItem, Alarmas_Velocidad)
            Dim veh_id As Integer = CInt(hdnveh_id.Value)

            Dim velocidad As Alertas_Velocidad_Configuradas = clsAlarma.SelectAlarmaByVelocidad(veh_id, alarma.vel_tipo_via)

            If velocidad IsNot Nothing Then
                DirectCast(e.Item.FindControl("txtValor"), TextBox).Text = velocidad.ale_valor_maximo
                DirectCast(e.Item.FindControl("chkAvisarme"), CheckBox).Checked = True
                DirectCast(e.Item.FindControl("chkMail"), CheckBox).Checked = velocidad.ale_enviar_mail
            End If

        End If
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        'guardo la configuracion elegida por el usuario
        Try
            Dim selecciono = False
            Dim seleccionoMovil = False


            'elimino la configuracion que puedan tener el movil

            clsAlarma.DeleteByMovil(CInt(hdnveh_id.Value))


            For Each row As DataGridItem In datagridAlarmas.Items



                If DirectCast(row.FindControl("chkAvisarme"), CheckBox).Checked Then

                    'verifico los vehiculos, inserto una alerta por cada uno

                          Dim alertaUsuario As Alertas_Velocidad_Configuradas = New Alertas_Velocidad_Configuradas()

                    alertaUsuario.veh_id = CInt(hdnveh_id.Value)
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
            'retorno
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)

        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMA VELOCIDAD EDIT - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error grabando las configuraciones, contacte al administrador."
        End Try

    End Sub

  
End Class