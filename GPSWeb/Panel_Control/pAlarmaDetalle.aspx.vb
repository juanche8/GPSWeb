'pagina para ver el detalle de una alarma reportada
Imports GPS.Business
Imports GPS.Data
Public Class pAlarmaDetalle
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'cargo el detalle de la alarma
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    If Request.Params("alar_id") IsNot Nothing Then
                        Dim _alarma As Alarmas = clsAlarma.SelectAlarmaById(CInt(Request.Params("alar_id")))
                        lblAlarma.Text = _alarma.alar_nombre
                        lblConductor.Text = _alarma.veh_conductor
                        lblFecha.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", _alarma.alar_fecha)
                        lblPatente.Text = _alarma.veh_patente
                        lblubicacion.Text = _alarma.alar_nombre_via & "," & _alarma.alar_Localidad & "," & _alarma.alar_Provincia
                        lblVelocidad.Text = _alarma.alar_valor & "kms/h"

                        lat.Value = _alarma.alar_lat
                        lng.Value = _alarma.alar_lng

                        'verifico el tipo y el codigo de la alarma
                        If _alarma.alar_tipo = "5" Or _alarma.alar_tipo = "51" Then

                            Dim _zona As Alertas_Zonas = clsAlarma.AlertaZonaSelectById(_alarma.alar_codigo_config)
                            hdnzon_id.Value = _zona.zon_id

                        End If

                        If _alarma.alar_tipo = "3" Or _alarma.alar_tipo = "31" Then

                            Dim _direcc As Alertas_Direcciones = clsAlarma.AlertaDireccionSelect(_alarma.alar_codigo_config)
                            latDir.Value = _direcc.Direcciones.dir_latitud
                            lngDir.Value = _direcc.Direcciones.dir_longitud

                        End If

                        If _alarma.alar_tipo = "6" Or _alarma.alar_tipo = "61" Then

                            Dim _direcc As Alertas_Recorridos = clsAlarma.AlertaRecorridoSelect(_alarma.alar_codigo_config)
                            hdnrec_id.Value = _direcc.rec_id.ToString()

                        End If

                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class