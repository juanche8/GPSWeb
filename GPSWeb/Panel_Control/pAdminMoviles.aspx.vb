'Pagina que permite hacer altas y modificaciones de los vehiculos del cliente
Imports GPS.Business
Imports GPS.Data
Imports System.IO

Partial Public Class pAdminMoviles
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'cargo el combo de tipo de vhiculos

        ddlTipoMovil.DataSource = clsVehiculo.ListTipoVehiculo
        ddlTipoMovil.DataBind()

        ddlTipoUso.DataSource = clsVehiculo.ListTipoUso
        ddlTipoUso.DataBind()

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'verifico si es un update o nuevo
        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    hdncli_id.Value = Session("Cliente").ToString()
                    If Request.Params("veh_id") IsNot Nothing Then
                        lbltitulo.Text = "Modificar Vehiculo "
                        hdnveh_id.Value = Request.Params("veh_id").ToString()

                        'cargo los datos del movil
                        Dim movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))

                        txtConductor.Text = movil.veh_nombre_conductor
                        txtNombre.Text = movil.veh_descripcion
                        txtPatente.Text = movil.veh_patente
                        ddlTipoMovil.SelectedValue = movil.veh_tipo_id.ToString()
                        txtMarca.Text = movil.veh_marca
                        txtModelo.Text = movil.veh_modelo
                        txtColor.Text = movil.veh_color
                        txtKilometros.Text = movil.veh_kilometros
                        ddlTipoUso.SelectedValue = movil.tip_uso_id
                        'imagen
                        If movil.veh_imagen <> "" Then imgMovil.ImageUrl = "~/fotos/" + movil.veh_imagen


                    End If
                End If
            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If

        Catch ex As Exception
            Funciones.WriteToEventLog("Admin MOVILES - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos del movil, contacte al administrador."
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            Dim movil As Vehiculo
           
            'valido que no cargue un movil duplicado por su patente
            movil = clsVehiculo.SeleccionarPatente(txtPatente.Text)
            'verifico si la patente ya existe
            If movil IsNot Nothing Then
                If movil.veh_patente = txtPatente.Text And movil.veh_id <> CInt(hdnveh_id.Value) Then
                    lblError.Text = "Ya existe un movil con la misma Patente cargado en el sistema."
                    Exit Sub
                End If
            End If

            movil = New Vehiculo()
            If hdnveh_id.Value = "0" Then
                'nuevo
                movil.mod_id = 0
                movil.veh_activo = False
                movil.cli_id = CInt(hdncli_id.Value)
                movil.veh_nombre_conductor = txtConductor.Text
                movil.veh_descripcion = txtNombre.Text
                movil.veh_patente = txtPatente.Text
                movil.veh_tipo_id = CInt(ddlTipoMovil.SelectedValue)
                movil.veh_marca = txtMarca.Text
                movil.veh_modelo = txtModelo.Text
                movil.veh_color = txtColor.Text
                movil.tip_uso_id = CInt(ddlTipoUso.SelectedValue)

                If txtKilometros.Text <> "" Then
                    movil.veh_kilometros = CInt(txtKilometros.Text)
                Else
                    movil.veh_kilometros = 0
                End If


                'cargo la imagen
                If FileUpload1.HasFile Then
                    movil.veh_imagen = FileUpload1.FileName
                    FileUpload1.PostedFile.SaveAs(Server.MapPath("~") + "\fotos\" + Path.GetFileName(FileUpload1.FileName))
                Else
                    movil.veh_imagen = ""
                End If

                clsVehiculo.Insert(movil)
            Else
                'update
                movil = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))

                If FileUpload1.HasFile Then
                    movil.veh_imagen = FileUpload1.FileName
                    FileUpload1.PostedFile.SaveAs(Server.MapPath("~") + "\fotos\" + Path.GetFileName(FileUpload1.FileName))
                End If

                movil.veh_nombre_conductor = txtConductor.Text
                movil.veh_descripcion = txtNombre.Text
                movil.veh_patente = txtPatente.Text
                movil.veh_tipo_id = CInt(ddlTipoMovil.SelectedValue)
                movil.veh_marca = txtMarca.Text
                movil.veh_modelo = txtModelo.Text
                movil.veh_color = txtColor.Text
                movil.tip_uso_id = CInt(ddlTipoUso.SelectedValue)
                If txtKilometros.Text <> "" Then
                    movil.veh_kilometros = CInt(txtKilometros.Text)
                Else
                    movil.veh_kilometros = 0
                End If
                clsVehiculo.Update(movil)
            End If

            lblError.Text = "Los datos se actulizaron con exito."
        Catch ex As Exception
            Funciones.WriteToEventLog("Admin MOVILES - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error grabando los datos, contacte al administrador."
        End Try
    End Sub
End Class