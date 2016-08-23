Imports GPS.Business
Imports GPS.Data

Partial Public Class pAddZona
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    'si esta editando
                    If Request.Params("zon_id") IsNot Nothing Then
                        hdnzon_id.Value = Request.Params("zon_id").ToString()

                        Dim zona As Zonas = clsZona.SelectById(CInt(hdnzon_id.Value))
                        txtNombre.Text = zona.zon_nombre
                        For Each puntos As Zonas_Puntos In zona.Zonas_Puntos
                            hdnZona.Value += "|" + puntos.zon_latitud + "," + puntos.zon_longitud
                        Next

                    End If

                    hdncli_id.Value = Session("Cliente").ToString()
                  

                End If
            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("ZONAS - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos. Contacte al administrador."
        End Try
        

        
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Try
            Dim zona = New Zonas()

            If hdnZona.Value <> "" Then
                
                'verifico si es edicion
                If hdnzon_id.Value <> "0" Then
                    zona = clsZona.SelectById(hdnzon_id.Value)

                    'borro los puntos y la vuelvo a grabar
                    clsZona.DeleteZona_Puntos(zona)
                    zona.zon_nombre = txtNombre.Text
                    zona.cli_id = CInt(hdncli_id.Value)
                    clsZona.UpdateZona(zona)

                    'recupero los puntos que eligio para marcar la ruta
                    Dim puntos As String() = hdnZona.Value.Split("|")
                    For i As Integer = 0 To puntos.Length - 1
                        Dim coordena As String() = puntos(i).Split(",")

                        If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                            Dim punto = New Zonas_Puntos()

                            punto.zon_id = zona.zon_id
                            punto.zon_latitud = coordena(0).ToString()
                            punto.zon_longitud = coordena(1).ToString()

                            clsZona.InsertPunto(punto)
                        End If

                    Next

                Else
                    'zona nueva
                    zona.zon_nombre = txtNombre.Text
                    zona.cli_id = CInt(hdncli_id.Value)
                    clsZona.Insert(zona)
                    'recupero los puntos que eligio para marcar la ruta
                    Dim puntos As String() = hdnZona.Value.Split("|")
                    For i As Integer = 0 To puntos.Length - 1
                        Dim coordena As String() = puntos(i).Split(",")

                        If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                            Dim punto = New Zonas_Puntos()

                            punto.zon_id = zona.zon_id
                            punto.zon_latitud = coordena(0).ToString()
                            punto.zon_longitud = coordena(1).ToString()

                            clsZona.InsertPunto(punto)
                        End If

                    Next

                End If


                'retorno al listado
                Response.Redirect("~/Panel_Control/pAdminZonaRecorrido.aspx?tab=1", False)


            Else
                'no marco los puntos muestro error
                lblError.Text = "Debe seleccionar el Area en el Mapa."
            End If

          
        Catch ex As Exception
            Funciones.WriteToEventLog("ZONA - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error grabando los datos. Contacte al administrador."
        End Try
        

    End Sub
End Class