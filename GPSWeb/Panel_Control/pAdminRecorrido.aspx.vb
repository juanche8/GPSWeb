Imports GPS.Business
Imports GPS.Data
Partial Public Class pAdminRecorrido
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'cargo las zonas que tiene el cliente creadas

            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    hdncli_id.Value = Session("Cliente").ToString()
                    gridRecorrido.DataSource = clsRecorrido.ListByCliente(CInt(hdncli_id.Value))
                    gridRecorrido.DataBind()
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("Admin Recorridos - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos. Contacte al administrador."
        End Try


    End Sub

    Protected Sub grid_rowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        'verifico si puedo borrar el movil, sino lo desactivo
        'si tiene un modulo asociado lo tengo que volver a poner activo
        If e.CommandName = "Borrar" Then
            'verifico si esta zona esta unida a alguna alarma y le digo que no la puede borrar
            Dim recorrido As Recorridos = clsRecorrido.SelectById(CInt(e.CommandArgument))

            If recorrido.Alertas_Recorridos.Count > 0 Then
                lblError.Text = "No se puede eliminar el Recorrido ya que esta asociada a una Alerta. Debe eliminar primero la Alerta configurada. "
            Else
                clsRecorrido.DeleteRecorrido(e.CommandArgument)
                gridRecorrido.DataSource = clsRecorrido.ListByCliente(CInt(hdncli_id.Value))
                gridRecorrido.DataBind()
            End If
        End If


    End Sub

    Protected Sub SortRecords(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        Dim sortExpression As String = e.SortExpression
        Dim direction As String = String.Empty


        If e.SortDirection = SortDirection.Ascending Then

            e.SortDirection = SortDirection.Descending
            direction = " DESC"
        Else
            e.SortDirection = SortDirection.Ascending
            direction = " ASC"
        End If

    End Sub

End Class