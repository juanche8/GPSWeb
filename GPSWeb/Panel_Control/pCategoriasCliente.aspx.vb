'Pagina que para que los clientes creen sus propias categorias de alarmas
Imports GPS.Business
Imports GPS.Data
Imports System.Linq

Partial Public Class pCategoriasCliente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'muestro los moviles del cliente
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    'busco los vehiculos del usuario
                    'Dim cli_id As Integer = DirectCast(Session("Cliente"), Integer)
                    'Dim categorias As List(Of Categorias_Alarmas_Clientes) = clsCategoriaAlarma.ListByCliente(cli_id)

                    'GridViewCategorias.DataSource = categorias
                    'GridViewCategorias.DataBind()
                    'hdncli_id.Value = cli_id.ToString()
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("CATEGORIAS CLIENTES - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al adminsitrador."
        End Try
    End Sub

    Protected Sub GridView_RowCommand(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Borrar" Then
                'solo si no tiene alertas asociadas
                '  clsCategoriaAlarma.Delete(CInt(e.CommandArgument))
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("CATEGORIAS CLIENTES - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error eliminando los datos. Contacte al adminsitrador."
        End Try
       
    End Sub
End Class