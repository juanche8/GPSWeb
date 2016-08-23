'Pagina para configurar las alertas de llegada o salida de direcciones
Imports GPS.Business
Imports GPS.Data
Imports System.Net
Imports System.Xml
Imports System.IO
Partial Public Class pAddDireccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim masOpciones As Boolean = False
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then

                    hdncli_id.Value = Session("Cliente").ToString()

                    'cargo el combo de marcadores
                    ddlMarcador.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                    ddlMarcador.DataBind()

                    Dim item As ListItem = New ListItem("- Seleccione Marcador -", "0")
                    ddlMarcador.Items.Insert(0, item)

                    'verifico si esta editando la alerta, busco los datos ya guardados
                    If Request.Params("dir_id") IsNot Nothing Then

                        hdndir_id.Value = Request.Params("dir_id").ToString()

                        Dim direccion As Direcciones = clsDireccion.SelectById(CInt(hdndir_id.Value))

                        txtDireccion.Text = direccion.dir_direccion
                        lat.Value = direccion.dir_latitud
                        lng.Value = direccion.dir_longitud
                        txtNombre.Text = direccion.dir_nombre
                       
                    End If
                End If
            Else
                'no esta logeado
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "redirigir", " parent.iraLogin();", True)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("DIRECCION GENERICA - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos de la Alerta.Contacte al administrador."
        End Try
    End Sub


    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try


            'guardo esta definicion de la alerta

            Dim latitud As String = lat.Value
            Dim longitud As String = lng.Value
            Dim dir As String = txtDireccion.Text
            Dim sError As String = ""
            'verifico si eligio un punto en el mapa, sino llamo a la api de google para obtener la lat y lng en base a la direccion
            If latitud = "-34.604" And longitud = "-58.382" Then
                'si la direccion esta vacio tiro alerta

                If (Not dir.Contains("Avenida 9 de Julio, Buenos Aires")) Then
                    clsGoogle.getLatLng(latitud, longitud, dir, sError)
                Else
                    lblError.Text = "Ingrese la Dirección o seleccione el punto en el mapa"
                    Exit Sub
                End If

            Else
                'si eligio un punto recupero la direccion
                clsGoogle.getdireccion(latitud, longitud, dir, sError)
            End If

          

            Dim direccion = New Direcciones()
            'si la alerta no existe la doy de alta

            'verifico si es edicion 
            If hdndir_id.Value <> "0" Then
                direccion = clsDireccion.SelectById(CInt(hdndir_id.Value))
                'verifico que no exista otra direccion igual
                Dim _dir As Direcciones = clsDireccion.SelectByLtLg(latitud, longitud, direccion.dir_id)
                If _dir Is Nothing Then
                    direccion = clsDireccion.SelectById(CInt(hdndir_id.Value))
                    direccion.dir_direccion = txtDireccion.Text
                    direccion.dir_latitud = latitud
                    direccion.dir_longitud = longitud
                    direccion.dir_nombre = txtNombre.Text


                    clsDireccion.UpdateDireccion(direccion)
                Else
                    lblError.Text = "Ya existe una Dirección con la misma latitud-longitud en el sistema. Verifique."
                    Exit Sub
                End If

            Else

                'verifico que no exista otra direccion igual
                Dim _dir As Direcciones = clsDireccion.SelectByLtLg(latitud, longitud, 0)
                If _dir Is Nothing Then
                    direccion.dir_direccion = dir
                    direccion.dir_latitud = latitud
                    direccion.dir_longitud = longitud
                    direccion.dir_nombre = txtNombre.Text
                    direccion.cli_id = CInt(hdncli_id.Value)
                    clsDireccion.Insert(direccion)
                Else
                    lblError.Text = "Ya existe una Dirección con la misma latitud-longitud en el sistema. Verifique."
                    Exit Sub
                End If
              
            End If



            'retorno al listado
            Response.Redirect("~/Panel_Control/pAdminZonaRecorrido.aspx?tab=3", False)


        Catch ex As Exception
            Funciones.WriteToEventLog("DIRECCION GENERICA- " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Grabando los datos de la Alerta.Contacte al administrador."
        End Try

    End Sub


    Protected Sub ddlMarcador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarcador.SelectedIndexChanged
        Try
            Dim sError As String = ""
            Dim latitud As String = ""
            Dim longitud As String = ""
            'tomo la direccion del marcador

            If ddlMarcador.SelectedValue <> "0" Then
                Dim _marcador As Marcadores = clsMarcador.SelectById(CInt(ddlMarcador.SelectedValue))
                txtDireccion.Text = _marcador.marc_direccion

                lat.Value = _marcador.marc_latitud
                lng.Value = _marcador.marc_longitud

                'pinto la direccion
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " codeAddress();", True)

            End If
         
           

        Catch ex As Exception
            Funciones.WriteToEventLog("DIRECCION GENERICA MARCADOR - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error obteniendo los datos.Contacte al administrador."
        End Try
        
    End Sub
End Class