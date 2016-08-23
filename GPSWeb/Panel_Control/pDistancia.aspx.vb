'Pagina para determinar los tiempos de espera de llegada de un movil o distancias entre dos puntos
Imports GPS.Business
Imports GPS.Data
Partial Public Class pDistancia
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Session.Remove("Filtro")
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    'verifico si va a filtrar por un solo vehiculo
                    Dim cli_id As Integer = DirectCast(Session("Cliente"), Integer)
                    hdncli_id.Value = cli_id.ToString()

                    'cargo el combo de marcadores
                    Dim item = New ListItem("- Seleccione Marcador -", "0")
                    ddlMarcaDestino.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                    ddlMarcaDestino.DataBind()

                    ddlMarcaDestino.Items.Insert(0, item)

                    'cargo el combo de marcadores
                    ddlMarcaOrigen.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                    ddlMarcaOrigen.DataBind()
                    ddlMarcaOrigen.Items.Insert(0, item)

                    'busco los vehiculos del usuario
                    Dim _vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)

                    ddlVehiculoOrigen.DataSource = _vehiculos
                    ddlVehiculoOrigen.DataBind()
                    ddlVehiculoOrigen.Items.Insert(0, New ListItem("- Seleccione Vehiculo -", "0"))

                    ddlVehiculoDestino.DataSource = _vehiculos
                    ddlVehiculoDestino.DataBind()
                    ddlVehiculoDestino.Items.Insert(0, New ListItem("- Seleccione Vehiculo -", "0"))

                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("DISTANCIA - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador."
        End Try
    End Sub

    

    Protected Sub ddlMarcaOrigen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarcaOrigen.SelectedIndexChanged
        'pongo la direccion del marcador en el text Origen
        Dim _marcador As Marcadores = clsMarcador.SelectById(CInt(ddlMarcaOrigen.SelectedValue))
        txtDesde.Text = _marcador.marc_direccion

    End Sub

    Protected Sub ddlMarcaDestino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarcaDestino.SelectedIndexChanged
        'pongo la direccion del marcador en el text Destino
        Dim _marcador As Marcadores = clsMarcador.SelectById(CInt(ddlMarcaDestino.SelectedValue))
        txtHasta.Text = _marcador.marc_direccion
    End Sub

    Protected Sub ddlVehiculoOrigen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVehiculoOrigen.SelectedIndexChanged
        'pongo la direccion del marcador en el text Origen
        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocation(CInt(ddlVehiculoOrigen.SelectedValue))
        If ubicacion IsNot Nothing Then
            txtDesde.Text = ubicacion.NOMBRE_VIA + " " + ubicacion.LOCALIDAD + " " + ubicacion.PROVINCIA
        Else

            txtDesde.Text = "Movil sin reportes de ubicación."
        End If

    End Sub

    Protected Sub ddlVehiculoDestino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVehiculoDestino.SelectedIndexChanged
        'pongo la direccion del marcador en el text destino
        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocation(CInt(ddlVehiculoDestino.SelectedValue))
        If ubicacion IsNot Nothing Then
            txtHasta.Text = ubicacion.NOMBRE_VIA + " " + ubicacion.LOCALIDAD + " " + ubicacion.PROVINCIA
        Else
            txtHasta.Text = "Movil sin reportes de ubicación."
        End If
    End Sub
End Class

'ejemplos http://scipion.es/tag/google-maps-api-3/