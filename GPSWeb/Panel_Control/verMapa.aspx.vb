Imports GPS.Business
Imports GPS.Data
Partial Public Class verMapa
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lat.Value = Request.Params("lat").ToString()
            lng.Value = Request.Params("lng").ToString()

            'verifico el tipo y el codigo de la alarma
            If Request.Params("tipo").ToString() = "5" Or Request.Params("tipo").ToString() = "51" Then

                Dim _alarma As Alertas_Zonas = clsAlarma.AlertaZonaSelectById(CInt(Request.Params("codigo").ToString()))
                hdnzon_id.Value = _alarma.zon_id

            End If

            If Request.Params("tipo").ToString() = "3" Or Request.Params("tipo").ToString() = "31" Then

                Dim _direcc As Alertas_Direcciones = clsAlarma.AlertaDireccionSelect(CInt(Request.Params("codigo").ToString()))
                latDir.Value = _direcc.Direcciones.dir_latitud
                lngDir.Value = _direcc.Direcciones.dir_longitud

            End If

            If Request.Params("tipo").ToString() = "6" Or Request.Params("tipo").ToString() = "61" Then

                Dim _direcc As Alertas_Recorridos = clsAlarma.AlertaRecorridoSelect(CInt(Request.Params("codigo").ToString()))
                hdnrec_id.Value = _direcc.rec_id.ToString()

            End If
        End If
    End Sub

End Class