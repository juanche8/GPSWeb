Imports GPS.Business
Imports GPS.Data
Partial Public Class pAdminZona
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'cargo las zonas que tiene el cliente creadas

            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                hdncli_id.Value = Session("Cliente").ToString()
               
                'verifico si me llega el parametro de la alarma que hay que borrar.
                If Request.Params("zon_id") IsNot Nothing Then
                    EliminarZona(CInt(Request.Params("zon_id").ToString()))
                End If

                If Request.Params("rec_id") IsNot Nothing Then
                    EliminarRec(CInt(Request.Params("rec_id").ToString()))
                End If

                If Request.Params("dir_id") IsNot Nothing Then
                    EliminarDir(CInt(Request.Params("dir_id").ToString()))
                End If

                If Request.Params("tab") IsNot Nothing Then
                    hdnTab.Value = "#tabs-" & Request.Params("tab").ToString()
                End If
            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ZONAS/RECORRIDOS - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos. Contacte al administrador."
        End Try


    End Sub

    Protected Sub EliminarZona(ByVal zon_id As Integer)
        'verifico si esta zona esta unida a alguna alarma y le digo que no la puede borrar
        Dim zona As Zonas = clsZona.SelectById(zon_id)

        If zona.Alertas_Zonas.Count > 0 Then
            lblError.Text = "No se puede eliminar la Zona ya que esta asociada a una Alerta. Debe eliminar primero la Alerta configurada. "
            Return
        Else
            clsZona.DeleteZona(zon_id)
          
        End If

        Response.Redirect("~/Panel_Control/pAdminZonaRecorrido.aspx", False)


    End Sub


    Protected Sub EliminarRec(ByVal rec_id As Integer)
        'verifico si esta zona esta unida a alguna alarma y le digo que no la puede borrar
        Dim recorrido As Recorridos = clsRecorrido.SelectById(rec_id)

        If recorrido.Alertas_Recorridos.Count > 0 Then
            lblError.Text = "No se puede eliminar el Recorrido ya que esta asociada a una Alerta. Debe eliminar primero la Alerta configurada. "
            Return
        Else
            clsRecorrido.DeleteRecorrido(rec_id)

        End If

        Response.Redirect("~/Panel_Control/pAdminZonaRecorrido.aspx?tab=3", False)


    End Sub

    Protected Sub EliminarDir(ByVal dir_id As Integer)
        'verifico si esta zona esta unida a alguna alarma y le digo que no la puede borrar
        Dim direccion As Direcciones = clsDireccion.SelectById(dir_id)

        If direccion.Alertas_Direcciones.Count > 0 Then
            lblError.Text = "No se puede eliminar la Dirección ya que esta asociada a una Alerta. Debe eliminar primero la Alerta configurada. "
            Return
        Else
            clsDireccion.DeleteDireccion(dir_id)

        End If

        Response.Redirect("~/Panel_Control/pAdminZonaRecorrido.aspx?tab=3", False)


    End Sub
    'ZONAS
    Public Sub JQGridRecorrrido_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression
        JQGridRecorrrido.DataSource = clsRecorrido.ListByCliente(CInt(hdncli_id.Value))
        JQGridRecorrrido.DataBind()

    End Sub


    '
    Public Sub JQGridZona_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        JQGridZona.DataSource = clsZona.ListByCliente(CInt(hdncli_id.Value))
        JQGridZona.DataBind()


    End Sub

    Public Sub JQGridDireccion_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        JQGridDireccion.DataSource = clsDireccion.ListByCliente(CInt(hdncli_id.Value))
        JQGridDireccion.DataBind()


    End Sub

End Class