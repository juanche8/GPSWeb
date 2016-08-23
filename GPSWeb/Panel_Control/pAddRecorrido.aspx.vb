Imports GPS.Business
Imports GPS.Data
Imports System.Linq
Imports System.Net
Imports System.Xml
Imports System.IO
Partial Public Class pAddRecorrido
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    If Request.Params("rec_id") IsNot Nothing Then
                        hdnrec_id.Value = Request.Params("rec_id").ToString()

                        Dim recorrido As Recorridos = clsRecorrido.SelectById(CInt(hdnrec_id.Value))
                        txtNombre.Text = recorrido.rec_nombre
                        txtDireccionDestino.Text = recorrido.rec_destino
                        txtDireccionOrigen.Text = recorrido.rec_origen
                        For Each puntos As Recorridos_Puntos In recorrido.Recorridos_Puntos
                            hdnRuta.Value += "|" + puntos.rec_latitud + "," + puntos.rec_longitud
                        Next

                    End If

                    hdncli_id.Value = Session("Cliente").ToString()

                End If

            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("RECORRIDOS - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos del Recorrido.Contacte al administrador."
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            Dim _recorrido = New Recorridos()
            Dim dirOrigen As String = txtDireccionOrigen.Text
            Dim dirDestino As String = txtDireccionDestino.Text
            Dim sError As String = ""
            Dim puntos As String()
            Dim coordena As String()

            If hdnRuta.Value <> "0" And hdnRuta.Value <> "" Then
                

                'grabo el recorrido
                'verifico si es edicion
                If hdnrec_id.Value <> "0" Then
                     

                    'verifico que no tenga otro recorrido con las mismas direcciones
                    Dim recor As Recorridos = clsRecorrido.SelectByDireccion(hdnDir1.Value.ToLower, hdnDir2.Value.ToLower(), CInt(hdnrec_id.Value))

                    If recor Is Nothing Then
                        _recorrido = clsRecorrido.SelectById(hdnrec_id.Value)

                        'borro los puntos y la vuelvo a grabar
                        clsRecorrido.DeleteRecorrido_Puntos(_recorrido)
                        _recorrido.rec_nombre = txtNombre.Text
                        _recorrido.cli_id = CInt(hdncli_id.Value)
                        _recorrido.rec_destino = hdnDir1.Value
                        _recorrido.rec_origen = hdnDir2.Value

                        clsRecorrido.UpdateRecorrido(_recorrido)

                        'recupero los puntos que eligio para marcar la ruta

                        puntos = hdnRuta.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            coordena = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Recorridos_Puntos()

                                punto.rec_id = _recorrido.rec_id
                                punto.rec_latitud = coordena(0).ToString()
                                punto.rec_longitud = coordena(1).ToString()
                               

                                clsRecorrido.InsertPunto(punto)
                            End If

                        Next
                    Else
                        lblError.Text = "Ya existe un Recorrido con el mismo Origen y Destino cargado en el sistema. Verifique."
                        Exit Sub
                    End If

                Else
                    Dim recor As Recorridos = clsRecorrido.SelectByDireccion(hdnDir1.Value.ToLower, hdnDir2.Value.ToLower(), 0)
                    If recor Is Nothing Then
                        'busco las direcciones para el caso de que marco los puntos en el mapa

                        puntos = hdnRuta.Value.Split("|")
                        If txtDireccionDestino.Text = "" Then
                            'primer punto
                            coordena = puntos(1).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                clsGoogle.getdireccion(coordena(0).ToString(), coordena(1).ToString(), dirOrigen, sError)
                            End If
                        End If
                      
                        If txtDireccionOrigen.Text = "" Then
                            'ultimo punto
                            coordena = puntos(puntos.Length - 1).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                clsGoogle.getdireccion(coordena(0).ToString(), coordena(1).ToString(), dirOrigen, sError)
                            End If
                        End If
                      

                        'recorrido nuevo
                        _recorrido.rec_nombre = txtNombre.Text
                        _recorrido.rec_destino = hdnDir1.Value
                        _recorrido.rec_origen = hdnDir2.Value
                        Dim _direccion As GPSDataOSM.Direccione
                      
                        _recorrido.cli_id = CInt(hdncli_id.Value)
                        clsRecorrido.Insert(_recorrido)
                        'recupero los puntos que eligio para marcar la ruta
                        puntos = hdnRuta.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            coordena = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Recorridos_Puntos()

                                punto.rec_id = _recorrido.rec_id
                                punto.rec_latitud = coordena(0).ToString()
                                punto.rec_longitud = coordena(1).ToString()
                               

                                clsRecorrido.InsertPunto(punto)
                            End If

                        Next
                    Else
                        lblError.Text = "Ya existe un Recorrido con el mismo Origen y Destino cargado en el sistema. Verifique."
                        Exit Sub
                    End If
                End If

            Else
                'si tengo un punto de origen y otro de destino tengo que ver como obetener los puntos intermedios, o como obtengo al ruta que tengo q ue controlar despues
                If txtDireccionOrigen.Text <> "" And txtDireccionDestino.Text <> "" Then
                    'verifico que no tenga otro recorrido con las mismas direcciones
                    Dim recor As Recorridos = clsRecorrido.SelectByDireccion(hdnDir1.Value.ToLower, txtDireccionDestino.Text.ToLower(), CInt(hdnrec_id.Value))

                    If recor Is Nothing Then

                        If hdnrec_id.Value = "0" Then
                            'nuevo recorrido
                            _recorrido = New Recorridos()

                            _recorrido.rec_nombre = txtNombre.Text
                            _recorrido.cli_id = CInt(hdncli_id.Value)
                            _recorrido.rec_origen = hdnDir1.Value
                            _recorrido.rec_destino = hdnDir2.Value

                            clsRecorrido.Insert(_recorrido)


                        Else
                            'si existe elimino los puntos y los creo de nuevo por si modifico algo
                            _recorrido = clsRecorrido.SelectById(hdnrec_id.Value)

                            'borro los puntos y la vuelvo a grabar

                            _recorrido.rec_nombre = txtNombre.Text
                            _recorrido.cli_id = CInt(hdncli_id.Value)
                            _recorrido.rec_origen = hdnDir1.Value
                            _recorrido.rec_destino = hdnDir2.Value
                            clsRecorrido.UpdateRecorrido(_recorrido)
                            clsRecorrido.DeleteRecorrido_Puntos(_recorrido)

                        End If

                        'vuelvo a crear los puntos
                        If Not getRoute(txtDireccionOrigen.Text, txtDireccionDestino.Text, _recorrido.rec_id) Then
                            lblError.Text = "Las Direcciónes ingresadas no se  encontraron en el sistema de Geoposicionamiento. Ingrese Calle, Localidad,Provincia."
                            Exit Sub
                        End If
                    End If
                Else
                    lblError.Text = "Debe Ingresar la Dirección de Origen y Destino. O marcar el recorrido en el mapa."
                    Exit Sub
                End If
                End If

            If sError <> "" Then
                lblError.Text = sError
                Exit Sub
            End If


            'retorno al listado
            Response.Redirect("~/Panel_Control/pAdminZonaRecorrido.aspx?tab=2", False)

          

        Catch ex As Exception
            Funciones.WriteToEventLog("RECORRIDOS - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error grabando los datos. Contacte al administrador."
        End Try
    End Sub

    Private Function getRoute(ByVal origen As String, ByVal destino As String, ByVal rec_id As Integer) As Boolean

        Dim request As WebRequest = WebRequest.Create(" http://maps.googleapis.com/maps/api/directions/xml?origin=" + origen + "&destination=" + destino + "&sensor=false")
        request.Method = "GET"
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
        Dim result1 As String = sr.ReadToEnd()
        'cargo el resultado como xml y busco las coordenadas

        Dim xmlRespuesta As XmlDocument = New XmlDocument()
        xmlRespuesta.LoadXml(result1)

        If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then
            'recupero la ruta
            'tengo que leer los nodos steps y sacar los puntos

            For Each nodo As XmlNode In xmlRespuesta.SelectNodes("//leg/step")
                Dim punto = New Recorridos_Puntos()
                punto.rec_id = rec_id
                punto.rec_latitud = nodo.SelectSingleNode("start_location/lat").InnerText
                punto.rec_longitud = nodo.SelectSingleNode("start_location/lng").InnerText
        clsRecorrido.InsertPunto(punto)

      

            Next

            Return True

        Else
            Return False
        End If

    End Function
End Class