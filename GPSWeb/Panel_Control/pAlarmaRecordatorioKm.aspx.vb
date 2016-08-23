'Pagina para editar un recordatorio por Kms
Imports GPS.Business
Imports GPS.Data
Partial Public Class pAlarmaRecordatorioKm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    If Request.Params("reck_id") IsNot Nothing Then
                        Dim valores As String() = Request.Params("reck_id").ToString().Split("|")
                        hdnveh_id.Value = valores(1)
                        hdnreck_id.Value = valores(0)
                        Dim _movil = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
                        lblMovil.Text = _movil.veh_descripcion & "-" & _movil.veh_patente

                        Dim recordatorio As Alertas_Recordatorios_Por_Km = clsAlarma.AlertaRecordatorioKmSelect(CInt(hdnreck_id.Value), CInt(hdnveh_id.Value))
                        txtDescripcion.Text = recordatorio.reck_descripcion

                        txtPrimerReporte.Text = recordatorio.reck_kms_primer_alarma
                        txtFrecuencia.Text = recordatorio.reck_ocurrencia_cada_km
                        chkMail.Checked = recordatorio.reck_notificar_mail


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

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            'edito la alarma

            Dim recodatoriokm As Alertas_Recordatorios_Por_Km = clsAlarma.AlertaRecordatorioKmSelect(CInt(hdnreck_id.Value), CInt(hdnveh_id.Value))
            recodatoriokm.reck_descripcion = txtDescripcion.Text

            recodatoriokm.reck_notificar_mail = chkMail.Checked
            recodatoriokm.reck_notificar_sms = False
            recodatoriokm.reck_kms_primer_alarma = txtPrimerReporte.Text
            recodatoriokm.reck_ocurrencia_cada_km = txtFrecuencia.Text
            recodatoriokm.reck_kilm_proxima_alarma = recodatoriokm.reck_kms_primer_alarma + recodatoriokm.reck_ocurrencia_cada_km

            clsAlarma.UpdateAlertaRecKm(recodatoriokm)

            'retorno al listado
            Response.Redirect("~/Panel_Control/pAdminAlarmas.aspx?tab=tabs-2", False)

        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS RECORDATORIO FECHA - Edicion " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Grabando los datos. Contacte al administrador."
        End Try
    End Sub

   
End Class