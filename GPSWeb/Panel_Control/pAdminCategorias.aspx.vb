'Pagina que para que los clientes creen sus propias categorias de alarmas
Imports GPS.Business
Imports GPS.Data
Imports System.Linq

Partial Public Class pAdminCategorias
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'verifico si es un update o nuevo
        Try
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    hdncli_id.Value = DirectCast(Session("Cliente"), Integer).ToString()
                    If Request.Params("cat_usu_id") IsNot Nothing Then
                        lbltitulo.Text = "Modificar Categoria de Alerta"
                        hdncat_usu_id.Value = Request.Params("cat_usu_id").ToString()

                        'cargo los datos del movil
                        'Dim categoria As Categorias_Alarmas_Clientes = clsCategoriaAlarma.SelectById(CInt(hdncat_usu_id.Value))

                        'txtNombre.Text = categoria.cat_usu_descripcion
                        'txtValor.Text = categoria.cat_usu_valor_defecto
                        'rdnUnidad.SelectedValue = categoria.cat_usu_unidadmedida

                    End If
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If

        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos, contacte al administrador."
            Funciones.WriteToEventLog("Admin CATEGORIAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            'Dim categoria As Categorias_Alarmas_Clientes = New Categorias_Alarmas_Clientes()

            'categoria.cat_usu_descripcion = txtNombre.Text
            'categoria.cat_usu_unidadmedida = rdnUnidad.SelectedValue
            'categoria.cat_usu_valor_defecto = txtValor.Text
            'categoria.cli_id = CInt(hdncli_id.Value)

            'If hdncat_usu_id.Value = "0" Then
            '    'nuevo               
            '    categoria.cli_id = CInt(hdncli_id.Value)
            '    clsCategoriaAlarma.Insert(categoria)
            'Else
            '    'update
            '    categoria.cat_usu_id = CInt(hdncat_usu_id.Value)
            '    clsCategoriaAlarma.Update(categoria)
            'End If

            'vuelvo al listado
            Response.Redirect("~/Panel_Control/pCategoriasCliente.aspx", False)
        Catch ex As Exception
            lblError.Text = "Ocurrio un error Grabando los datos, contacte al administrador."
            Funciones.WriteToEventLog("Admin CATEGORIAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub
End Class