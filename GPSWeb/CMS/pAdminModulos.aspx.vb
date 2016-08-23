'Pagina para agregar o eliminar los Modulos que se instalan en los autos
Imports GPS.Business
Imports GPS.Data
Partial Public Class pAdminModulos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then
                    lbltotal.Text = clsModulo.List().Count.ToString()
                    FillData()
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos - " + ex.Message
        End Try
    End Sub

    Private Sub FillData()
        'cargo el listado de los modulos datos de alta

        Dim nro_modulo = 0

        If txtNroMod.Text <> "" Then nro_modulo = CInt(txtNroMod.Text)

        Dim _modulos As List(Of Modulo) = clsModulo.Search(nro_modulo)


        gridviewModulos.DataSource = _modulos
        gridviewModulos.DataBind()
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Try
            'guardo el modulo
            'verifico que no exista otro igual

            If txtModulo.Text = "" Then
                lblError.Text = "Ingrese el Nro de Módulo."
                Exit Sub
            End If

            ' If Not IsNumeric(txtModulo.Text) Then
            'lblError.Text = "El Nro de Módulo debe ser un valor numerico Entero."
            'Exit Sub
            'End If

            If accion.Value = "alta" Then
                Dim modulo As Modulo
                modulo = clsModulo.Selecionar(txtModulo.Text)

                If modulo IsNot Nothing Then
                    If modulo.mod_numero = txtModulo.Text Then
                        lblError.Text = "Existe un Modulo con el mismo número cargado en el sistema."
                        Exit Sub
                    End If
                End If
                modulo = New Modulo()
                modulo.mod_en_uso = False
                modulo.mod_numero = txtModulo.Text
                modulo.mod_nro_cel = txtCelular.Text
                modulo.mod_version_trama = ddlTrama.SelectedValue

                clsModulo.Insert(modulo)

            Else
                Dim modulo As Modulo
                modulo = clsModulo.Selecionar(txtModulo.Text)

                If modulo IsNot Nothing Then
                    If modulo.mod_numero = txtModulo.Text And hdnmod_id.Value <> modulo.mod_id.ToString() Then
                        lblError.Text = "Existe un Modulo con el mismo número cargado en el sistema."
                        Exit Sub
                    End If
                End If

                modulo = clsModulo.SelecionarById(CInt(hdnmod_id.Value))
                ' tengo que modificar el auto que tenia asociado este modulo
                Dim _movil As Vehiculo = clsVehiculo.Seleccionar(modulo.mod_numero.ToString())

                If _movil IsNot Nothing Then
                    _movil.mod_id = txtModulo.Text

                    clsVehiculo.Update(_movil)
                End If

                modulo.mod_numero = txtModulo.Text
                modulo.mod_nro_cel = txtCelular.Text
                modulo.mod_version_trama = ddlTrama.SelectedValue
                clsModulo.Update(modulo)
                txtModulo.Text = ""
                txtCelular.Text = ""
                hdnmod_id.Value = "0"
                accion.Value = "alta"
            End If

            'cargo el listado de los modulos datos de alta
            gridviewModulos.DataSource = clsModulo.List()
            gridviewModulos.DataBind()
        Catch ex As Exception
            lblError.Text = "Ocurrio un error grabando los datos - " + ex.Message
        End Try
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

    Protected Sub grid_rowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "Borrar" Then
            'verifico si esta en uso y no lo dejo borrar
            Dim modulo As Modulo = clsModulo.SelecionarById(e.CommandArgument)
            If modulo.mod_en_uso Then
                lblError.Text = "El Modulo esta asociado a un Vehiculo, debe primero eliminar esta relación."
                Exit Sub
            End If
            clsModulo.Delete(e.CommandArgument)
            'actualizo la grilla
            gridviewModulos.DataSource = clsModulo.List()
            gridviewModulos.DataBind()
        End If
        If e.CommandName = "Editar" Then
            Dim modulo As Modulo = clsModulo.SelecionarById(e.CommandArgument)
            lblTitulo.Text = "Modificar Modulo"
            txtCelular.Text = modulo.mod_nro_cel
            txtModulo.Text = modulo.mod_numero
            If modulo.mod_version_trama IsNot Nothing Then
                ddlTrama.SelectedValue = modulo.mod_version_trama
            End If

            accion.Value = "editar"
            hdnmod_id.Value = modulo.mod_id.ToString()
            mpeEditar.Show()
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        FillData()
    End Sub


End Class