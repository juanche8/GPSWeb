'Pagina para crear marcadores
Imports GPS.Business
Imports GPS.Data
Imports System.Net
Imports System.Xml
Imports System.IO

Partial Public Class pMarcadores
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'cargo el combo de tipo de vhiculos

        ddlTipoMarcador.DataSource = clsMarcador.ListTiposMarcador()
        ddlTipoMarcador.DataBind()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then

                    Dim cli_id As Integer = DirectCast(Session("Cliente"), Integer)
                    hdncli_id.Value = cli_id.ToString()
                    'muestro los marcadores ya creados en la grilla y en el mapa
                    datagridMarcadores.DataSource = clsMarcador.List(cli_id)
                    datagridMarcadores.DataBind()

                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos. Contacte con el Administrador."
            Funciones.WriteToEventLog("MARCADORES - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            'guardo el marcador asociado al cliente
            Dim latitud As String = lat.Value
            Dim longitud As String = lng.Value
            Dim sError As String = ""

            'verifico si eligio un punto en el mapa, sino llamo a la api de google para obtener la lat y lng en base a la direccion
            If latitud = "-35" And longitud = "-64" Then
                clsGoogle.getLatLng(latitud, longitud, direccion.Text, sError)

                If sError <> "" Then
                    lblError.Text = sError
                    Exit Sub
                End If
            End If

            Dim marcador As Marcadores
            'verifico que no se duplique el marcador
            'lo busco por direccion
            marcador = clsMarcador.SelectByLatLng(CInt(hdncli_id.Value), latitud, longitud)

            If marcador IsNot Nothing Then
                If marcador.marc_id <> CInt(hdnmarc_id.Value) Then
                    lblError.Text = "Ya existe un Marcador cargado en el sistema para la misma ubicación elegida."
                    Exit Sub
                End If
            Else
                'lo busco por la direccion por si no coinciden las latitudes
                marcador = clsMarcador.SelectByDirecc(direccion.Text)
                If marcador IsNot Nothing Then
                    If marcador.marc_id <> CInt(hdnmarc_id.Value) Then
                        lblError.Text = "Ya existe un Marcador cargado en el sistema para la misma ubicación elegida."
                        Exit Sub
                    End If
                End If
            End If

                marcador = New Marcadores()
                'tengo que ver si es una edicion
                If hdnmarc_id.Value <> "0" Then marcador = clsMarcador.SelectById(CInt(hdnmarc_id.Value))

                marcador.cli_id = CInt(hdncli_id.Value)
                marcador.marc_latitud = latitud
                marcador.marc_longitud = longitud
                marcador.marc_nombre = txtNombreMarcador.Text
                marcador.tipo_marc_id = CInt(ddlTipoMarcador.SelectedValue)
                marcador.marc_direccion = direccion.Text

                If hdnmarc_id.Value <> "0" Then
                    clsMarcador.Update(marcador)
                Else
                    clsMarcador.Insert(marcador)
                End If

                txtNombreMarcador.Text = ""
                direccion.Text = ""
                ddlTipoMarcador.SelectedIndex = -1
                hdnmarc_id.Value = "0"
                'actualizo la grilla
                datagridMarcadores.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                datagridMarcadores.DataBind()

                'refresco el mapa
                ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "initialize();", True)
                'hago zoom en el que creo
                ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "marcaZoom('" + marcador.marc_latitud + "','" + marcador.marc_longitud + "');", True)


        Catch ex As Exception
            lblError.Text = "Ocurrio un error grabando los datos. Contacte con el Administrador."
            Funciones.WriteToEventLog("MARCADORES - " + ex.Message + " - " + ex.StackTrace)
        End Try
       

    End Sub

    Protected Sub datagridMarcadores_itemcommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try
            If e.CommandName = "Borrar" Then
                clsMarcador.Delete(e.CommandArgument)
                'actualizo la grilla
                datagridMarcadores.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                datagridMarcadores.DataBind()
                'muestro de nuevo los marcadores
                'zoom en el mapa con este marcador - hago un set center y un zoom
                ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "initialize();", True)

            End If

            If e.CommandName = "Editar" Then
                'cargo los datos del marcador
                Dim marca As Marcadores = clsMarcador.SelectById(CInt(e.CommandArgument.ToString))
                hdnmarc_id.Value = marca.marc_id.ToString
                lat.Value = marca.marc_latitud
                lng.Value = marca.marc_longitud
                txtNombreMarcador.Text = marca.marc_nombre
                ddlTipoMarcador.SelectedValue = marca.tipo_marc_id.ToString
                direccion.Text = marca.marc_direccion
                'zoom en el mapa con este marcador - hago un set center y un zoom
                ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "marcaZoom('" + marca.marc_latitud + "','" + marca.marc_longitud + "');", True)

            End If

            If e.CommandName = "Ver" Then
                'zoom en el mapa con este marcador - hago un set center y un zoom
                Dim marca As Marcadores = clsMarcador.SelectById(CInt(e.CommandArgument.ToString))
                ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "marcaZoom('" + marca.marc_latitud + "','" + marca.marc_longitud + "');", True)
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("MAPA - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error modificando los datos. Contacte con el Administrador."
        End Try
       
    End Sub

End Class

'http://www.masquewordpress.com/como-usar-google-maps-geocoder-api-v3/