Imports GPS.Business
Imports GPS.Data
Partial Public Class pHistorial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'analizar si hay que acortar el rango de fecha a determinada cantidad de días por la info puede ser mucha
        'tengo que traer las posiciones en un rango de fechas y marcar los puntos en el mapa
        ' los unos y creo markes con el icono de google y una ventana de info que digala fecha, hora y ubicacion en ese momento.

        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    'verifico si va a filtrar por un solo vehiculo
                    Dim cli_id As Integer = DirectCast(Session("Cliente"), Integer)
                    hdncli_id.Value = cli_id.ToString()
                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()
                        'PanelCustomizado.Visible = False
                        PanelMoviles.Visible = False
                        Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
                        lblMovil.Text = "Vehiculo Seleccionado" + _movil.veh_descripcion + "-" + _movil.veh_patente
                    Else
                        'busco los vehiculos del usuario
                        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)
                        DataListVehiculos.DataSource = vehiculos
                        DataListVehiculos.DataBind()

                    End If

                    'por defecto una semana
                    txtfechadesde.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy")
                    txtfechahasta.Text = DateTime.Now.ToString("dd/MM/yyyy")
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("RECORRIDOS - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador."
        End Try
    End Sub

End Class
'unir puntos
'http://www.forosdelweb.com/f13/dibujar-ruta-para-varios-marcadores-google-maps-1007865/
'animar movil
'http://econym.org.uk/gmap/example_cartrip.htm