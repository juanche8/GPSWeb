'Pagina para editar un recordatorio por fecha
Imports GPS.Business
Imports GPS.Data

Partial Public Class pAlarmaRecordatorioFecha
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    If Request.Params("recf_id") IsNot Nothing Then
                        Dim valores As String() = Request.Params("recf_id").ToString().Split("|")
                        hdnveh_id.Value = valores(1)
                        hdnrecf_id.Value = valores(0)

                        Dim _movil = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
                        lblMovil.Text = _movil.veh_descripcion & "-" & _movil.veh_patente

                        Dim recordatorio As Alertas_Recordatorios_Por_Fechas = clsAlarma.AlertaRecordatorioFechaSelect(CInt(hdnrecf_id.Value), CInt(hdnveh_id.Value))
                        txtDescripcion.Text = recordatorio.recf_descripcion
                        rdnFrecuencia.SelectedValue = recordatorio.recf_periocidad
                        If recordatorio.recf_periocidad = "anio" Then
                            PanelDia.Visible = False
                            PanelAnio.Visible = True
                            PanelMes.Visible = False
                            Dim fecha() As String = recordatorio.recf_proxima_ocurrencia.ToString().Split("/")
                            txtDiaMes.Text = fecha(0)
                            ddlMes.SelectedValue = fecha(1)

                        End If

                        If recordatorio.recf_periocidad = "mes" Then
                            PanelDia.Visible = False
                            PanelAnio.Visible = False
                            PanelMes.Visible = True
                            txtDiaMes.Text = recordatorio.recf_proxima_ocurrencia.Day.ToString()
                        End If

                        If recordatorio.recf_periocidad = "dia" Then
                            PanelDia.Visible = True
                            PanelAnio.Visible = False
                            PanelMes.Visible = False
                            txtFecha.Text = String.Format("{0:dd/MM/yyyy}", recordatorio.recf_proxima_ocurrencia)

                            'saco la hora y los minutos
                            ddlhora.SelectedValue = String.Format("{0:HH}", recordatorio.recf_proxima_ocurrencia)
                            ddlmin.SelectedValue = String.Format("{0:mm}", recordatorio.recf_proxima_ocurrencia)
                        End If

                        'chkSms.Checked = recordatorio.recf_notificar_sms
                        chkMail.Checked = recordatorio.recf_notificar_mail
                    End If
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS RECORDATORIO FECHA - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos. Contacte al administrador."
        End Try
    End Sub

    Protected Sub rdnFrecuencia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdnFrecuencia.SelectedIndexChanged
        'cuando cambia la seleccion muestro u oculto los paneles
        If rdnFrecuencia.SelectedValue = "dia" Then
            PanelDia.Visible = True
            PanelAnio.Visible = False
            PanelMes.Visible = False
        End If
        If rdnFrecuencia.SelectedValue = "mes" Then
            PanelDia.Visible = False
            PanelAnio.Visible = False
            PanelMes.Visible = True
        End If
        If rdnFrecuencia.SelectedValue = "anio" Then
            PanelDia.Visible = False
            PanelAnio.Visible = True
            PanelMes.Visible = False
        End If
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            'edito la alarma
            
            Dim recodatorioFecha As Alertas_Recordatorios_Por_Fechas = clsAlarma.AlertaRecordatorioFechaSelect(CInt(hdnrecf_id.Value), CInt(hdnveh_id.Value))
            If txtDescripcion.Text = "" Then
                lblError.Text = "Para Configurar un Recordatorio para Fecha ingrese una Descripción para el mismo."
                Exit Sub
            End If
            recodatorioFecha.recf_descripcion = txtDescripcion.Text
            If rdnFrecuencia.SelectedValue = "dia" Then
                If txtFecha.Text = "" Then
                    lblError.Text = "Para Configurar un Recordatorio para Fecha para una Fecha especifica, ingrese la Fecha."
                    Exit Sub
                Else
                    'fecha mayor a la actual
                    If DateTime.Parse(txtFecha.Text + " " + ddlhora.SelectedValue + ":" + ddlmin.SelectedValue + ":00") < DateTime.Now Then
                        lblError.Text = "La Fecha ingresada debe ser mayor a la actual."
                        Exit Sub
                    End If
                    recodatorioFecha.recf_periocidad = "dia"
                    recodatorioFecha.recf_proxima_ocurrencia = CDate(txtFecha.Text + " " + ddlhora.SelectedValue + ":" + ddlmin.SelectedValue + ":00")
                End If
            End If
            If rdnFrecuencia.SelectedValue = "mes" Then
                If txtDiaMes.Text = "" Then
                    lblError.Text = "Para Configurar un Recordatorio para Fecha para Todos los Meses, ingrese el Día del Mes."
                    Exit Sub
                End If
                recodatorioFecha.recf_periocidad = "mes"
                Dim fecha As DateTime = CDate(txtDiaMes.Text + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())

                If (fecha < DateTime.Now) Then
                    recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + DateTime.Now.AddMonths(1).Month.ToString() + "/" + DateTime.Now.Year.ToString())

                Else
                    recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())

                End If
            End If
            If rdnFrecuencia.SelectedValue = "anio" Then
                If txtDiaMes.Text = "" Then
                    lblError.Text = "Para Configurar un Recordatorio para Fecha para Todos los Años, ingrese el Día del Mes."
                    Exit Sub
                End If
                recodatorioFecha.recf_periocidad = "anio"
                Dim fecha As DateTime = CDate(txtDiaMes.Text + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())

                'si el mes es menor al mes actual le tengo que poner el proximo año
                If (fecha < DateTime.Now) Then
                    recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + ddlMes.SelectedValue + "/" + DateTime.Now.AddYears(1).Year.ToString())

                Else
                    recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + ddlMes.SelectedValue + "/" + DateTime.Now.Year.ToString())

                End If
            End If

            recodatorioFecha.recf_notificar_sms = False
            recodatorioFecha.recf_notificar_mail = chkMail.Checked
            clsAlarma.UpdateAlertaRecFecha(recodatorioFecha)

            'retorno al listado
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)
        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS RECORDATORIO FECHA - Edicion " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Grabando los datos. Contacte al administrador."
        End Try
    End Sub

   
End Class