'Pagina para crear marcadores
Imports GPS.Business
Imports GPS.Data
Imports System.Net
Imports System.Xml
Imports System.IO
Partial Public Class pAdminMarcadores
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'cargo el combo de tipo de vhiculos

        ddlTipoMarcador.DataSource = clsMarcador.ListTiposMarcador()
        ddlTipoMarcador.DataBind()
        'muestro el listado de clientes
        Dim listClientes As List(Of Cliente) = clsCliente.List
        Dim item As ListItem
        For Each cliente As Cliente In listClientes
            item = New ListItem(cliente.cli_apellido + " " + cliente.cli_nombre, cliente.cli_id.ToString)
            lstClientes.Items.Add(item)
        Next

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblError.Text = ""

            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then
                    'cliente cero = admin
                    hdncli_id.Value = "0"
                    'muestro los marcadores ya creados en la grilla y en el mapa
                    datagridMarcadores.DataSource = clsMarcador.ListGenericos
                    datagridMarcadores.DataBind()

                
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception
            lblError.Text = "Error Cargando los datos - " + ex.Message
        End Try
       

    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            'guardo el marcador asociado al cliente
            Dim latitud As String = lat.Value
            Dim longitud As String = lng.Value

            'verifico si eligio un punto en el mapa, sino llamo a la api de google para obtener la lat y lng en base a la direccion
            If latitud = "-35" And longitud = "-64" Then
                getLatLng(direccion.Text, latitud, longitud)
            End If

            Dim marcador As Marcadores_Generico
            'verifico que no se duplique el marcador
            'lo busco por direccion
            marcador = clsMarcador.SelectGenericoByLatLng(latitud, longitud)

            If marcador IsNot Nothing Then
                If marcador.marc_id <> CInt(hdnmarc_id.Value) Then
                    lblError.Text = "Ya existe un Marcador cargado en el sistema para la misma ubicación elegida."
                End If
            Else
                'lo busco por la direccion por si no coinciden las latitudes
                marcador = clsMarcador.SelectGenericoByDirecc(direccion.Text)
                If marcador IsNot Nothing Then
                    If marcador.marc_id <> CInt(hdnmarc_id.Value) Then
                        lblError.Text = "Ya existe un Marcador cargado en el sistema para la misma ubicación elegida."
                    End If
                End If
            End If

            marcador = New Marcadores_Generico()
            'tengo que ver si es una edicion
            If hdnmarc_id.Value <> "0" Then marcador = clsMarcador.SelectGenaricoById(CInt(hdnmarc_id.Value))

            marcador.marc_latitud = latitud
            marcador.marc_longitud = longitud
            marcador.marc_nombre = txtNombreMarcador.Text
            marcador.tipo_marc_id = CInt(ddlTipoMarcador.SelectedValue)
            marcador.marc_direccion = direccion.Text
            marcador.marc_imagen = ""
            marcador.marc_mostrar_a_todos = chkTodos.Checked

            'upload de la imagen a la carpeta marcadores
            If FileUpload1.HasFile Then
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~") + "\images\Marcadores\" + Path.GetFileName(FileUpload1.FileName))
                marcador.marc_imagen = "Marcadores/" + Path.GetFileName(FileUpload1.FileName)
            End If
            If hdnmarc_id.Value <> "0" Then
                clsMarcador.UpdateGenerico(marcador)
                clsMarcador.DeleteCliente(marcador)
            Else
                clsMarcador.InsertGenerico(marcador)
            End If

            'agrego los clientes que eligio
            If chkTodos.Checked = False Then
                'Dim lclientes As List(Of Clientes) = clsCliente.ListActivos()

                ' For Each cliente As Clientes In lclientes

                'Dim clienteMarca As New Marcadores_GenericosXClientes
                ' clienteMarca.cli_id = cliente.cli_id
                'clienteMarca.marc_id = marcador.marc_id

                'lsMarcador.InsertCliente(clienteMarca)

                ' Next

                ' Else
                For Each item As ListItem In lstClientes.Items
                    If item.Selected Then
                        Dim cliente As New Marcadores_GenericosXClientes
                        cliente.cli_id = item.Value
                        cliente.marc_id = marcador.marc_id

                        clsMarcador.InsertCliente(cliente)
                    End If
                Next
            End If

            txtNombreMarcador.Text = ""
            direccion.Text = ""
            ddlTipoMarcador.SelectedIndex = -1
            hdnmarc_id.Value = "0"

            'actualizo la grilla
            datagridMarcadores.DataSource = clsMarcador.ListGenericos()
            datagridMarcadores.DataBind()

            'refresco el mapa
            ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "initialize();", True)

        Catch ex As Exception
            lblError.Text = "Error Guardando los datos - " + ex.Message
        End Try


    End Sub

    Protected Sub datagridMarcadores_itemcommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        If e.CommandName = "Borrar" Then
            clsMarcador.DeleteGenerico(e.CommandArgument)
            'actualizo la grilla
            datagridMarcadores.DataSource = clsMarcador.ListGenericos
            datagridMarcadores.DataBind()
            'refresco el mapa
            ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "initialize();", True)

        End If

        If e.CommandName = "Editar" Then
            'cargo los datos del marcador
            Dim marca As Marcadores_Generico = clsMarcador.SelectGenaricoById(CInt(e.CommandArgument.ToString))
            hdnmarc_id.Value = marca.marc_id.ToString

            lat.Value = marca.marc_latitud
            lng.Value = marca.marc_longitud
            txtNombreMarcador.Text = marca.marc_nombre
            ddlTipoMarcador.SelectedValue = marca.tipo_marc_id.ToString
            direccion.Text = marca.marc_direccion
            chkTodos.Checked = marca.marc_mostrar_a_todos

            'cargo los clientes elegidos
            For Each cliente As Marcadores_GenericosXClientes In marca.Marcadores_GenericosXClientes
                lstClientes.Items.FindByValue(cliente.cli_id).Selected = True
            Next
            hdnmarc_id.Value = marca.marc_id.ToString()
            'zoom en el mapa con este marcador - hago un set center y un zoom
            ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "marcaZoom('" + marca.marc_latitud + "','" + marca.marc_longitud + "');", True)

        End If

        If e.CommandName = "Ver" Then
            'zoom en el mapa con este marcador - hago un set center y un zoom
            Dim marca As Marcadores_Generico = clsMarcador.SelectGenaricoById(CInt(e.CommandArgument.ToString))
            ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "marcaZoom('" + marca.marc_latitud + "','" + marca.marc_longitud + "');", True)

        End If
    End Sub

    Private Sub getLatLng(ByVal direccion As String, ByRef latitud As String, ByRef longitud As String)

        Dim request As WebRequest = WebRequest.Create("http://maps.google.com/maps/api/geocode/xml?address=" + direccion + "&sensor=true")
        request.Method = "GET"
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim sr As StreamReader = New StreamReader(Response.GetResponseStream())
        Dim result1 As String = sr.ReadToEnd()
        'cargo el resultado como xml y busco las coordenadas

        Dim xmlRespuesta As XmlDocument = New XmlDocument()
        xmlRespuesta.LoadXml(result1)

        If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then
            latitud = xmlRespuesta.SelectSingleNode("//geometry/location/lat").InnerText
            longitud = xmlRespuesta.SelectSingleNode("//geometry/location/lng").InnerText
        Else
            lblError.Text = "La Dirección ingresada no se pudo encontrar en el sistema de Geoposicionamiento. Verifique"
        End If

    End Sub

   
End Class