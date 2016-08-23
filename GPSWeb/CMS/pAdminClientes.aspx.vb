'Pagina para administrar clientes
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

Partial Public Class pAdminClientes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'busco los clientes
        Try
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then

                    GridClientes.DataSource = clsCliente.ListActivos()
                    GridClientes.DataBind()
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos - " + ex.Message
        End Try
    End Sub

    
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            GridClientes.DataSource = clsCliente.List(txtNombre.Text, txtMail.Text)
            GridClientes.DataBind()
        Catch ex As Exception
            lblError.Text = "Ocurrio un error filtrando los datos - " + ex.Message
        End Try
       
    End Sub

    Protected Sub grid_rowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "Borrar" Then
            clsCliente.Borrar(e.CommandArgument)
            'actualizo la grilla
            GridClientes.DataSource = clsCliente.ListActivos()
            GridClientes.DataBind()
        End If
    End Sub

    Protected Sub gvCliente_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If
        'cargo la grilla de empresa, particular y contacto
        Dim cliente As Cliente = DirectCast(e.Row.DataItem, Cliente)

        Dim gvEmpresa As GridView = DirectCast(e.Row.FindControl("gvEmpresa"), GridView)
        ' Dim gvParticular As GridView = DirectCast(e.Row.FindControl("gvParticular"), GridView)
        Dim gvContacto As GridView = DirectCast(e.Row.FindControl("gvContacto"), GridView)

        'si es particular no muestro los datos particulares
        If cliente.Tipos_Clientes.tipo_cli_nombre = "Particular" Then
            Dim lblTitulo As Label = DirectCast(e.Row.FindControl("lblTitulo"), Label)
            lblTitulo.Visible = False
        End If

        gvEmpresa.DataSource = cliente.Clientes_Empresas
        gvEmpresa.DataBind()

        'gvParticular.DataSource = cliente.Clientes_Particulares
        'gvParticular.DataBind()

        gvContacto.DataSource = cliente.Contactos_Clientes
        gvContacto.DataBind()


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
'grilla maestro detalle
'http://ltuttini.blogspot.com.ar/2012/04/aspnetgridview-anidados-maestro-detalle.html