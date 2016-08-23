'Pagina que permite hacer altas y modificaciones de los vehiculos del cliente
Imports GPS.Business
Imports GPS.Data
Imports System.IO
Partial Public Class pVehiculos
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
       
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'verifico si es un update o nuevo
        Try
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then

                    'cargo el combo de tipo de vhiculos
                    ddlTipoMovil.DataSource = clsVehiculo.ListTipoVehiculo
                    ddlTipoMovil.DataBind()

                    ddlCliente.DataSource = clsCliente.List()
                    ddlCliente.DataBind()

                    ddlTipoUso.DataSource = clsVehiculo.ListTipoUso
                    ddlTipoUso.DataBind()

                    ddlModulo.DataSource = clsModulo.ListDisponibles()
                    ddlModulo.DataBind()
                    Dim cboitem As ListItem
                    cboitem = New ListItem("Seleccionar Modulo", 0)
                    ddlModulo.Items.Insert(0, cboitem)

                    If Request.Params("veh_id") IsNot Nothing Then
                        lbltitulo.Text = "Modificar Vehiculo"
                        hdnveh_id.Value = Request.Params("veh_id").ToString()

                        'cargo los datos del movil
                        Dim movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
                        'agrego el modulo del movil
                        Dim modulo As Modulo = clsModulo.Selecionar(movil.mod_id)

                        If modulo IsNot Nothing Then
                            cboitem = New ListItem(modulo.mod_numero.ToString(), modulo.mod_numero)
                            ddlModulo.Items.Add(cboitem)
                            ddlModulo.SelectedValue = modulo.mod_id.ToString()
                        End If

                        txtConductor.Text = movil.veh_nombre_conductor
                        txtNombre.Text = movil.veh_descripcion
                        txtPatente.Text = movil.veh_patente
                        ddlTipoMovil.SelectedValue = movil.veh_tipo_id.ToString()
                        txtMarca.Text = movil.veh_marca
                        txtModelo.Text = movil.veh_modelo
                        ddlModulo.SelectedValue = movil.mod_id
                        rdnActivo.SelectedValue = movil.veh_activo.ToString()
                        ddlCliente.SelectedValue = movil.cli_id.ToString()
                        txtColor.Text = movil.veh_color
                        txtKilometros.Text = movil.veh_kilometros
                        ddlTipoUso.SelectedValue = movil.tip_uso_id.ToString()
                        rdnModuloSensor.SelectedValue = movil.veh_modulo_sensor
                    Else
                        'verifico si va a agregar un vehiculo de un cliente en particular y marco el combo
                        If Request.Params("cli_id") IsNot Nothing Then
                            If Request.Params("cli_id").ToString() <> "0" Then
                                ddlCliente.SelectedValue = Request.Params("cli_id").ToString()
                            End If
                        End If
                    End If
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If

        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos - " + ex.Message
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            Dim movil As Vehiculo = New Vehiculo()

            If hdnveh_id.Value <> "0" Then movil = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
            Dim vehiculo As Vehiculo = clsVehiculo.SeleccionarPatente(txtPatente.Text)
            'verifico si la patente ya existe
            If vehiculo IsNot Nothing Then
                If vehiculo.veh_patente = txtPatente.Text And vehiculo.veh_id <> movil.veh_id Then
                    lblError.Text = "Ya existe un movil con la misma Patente cargado en el sistema."
                    Exit Sub
                End If
            End If

            'si esta editando marco el modulo asociado como libre si quiere cambiar de modulo
            Dim modulo As Modulo = clsModulo.Selecionar(movil.mod_id)

            If modulo IsNot Nothing Then
                If movil.mod_id <> ddlModulo.SelectedValue Then
                    modulo.mod_en_uso = False
                    clsModulo.Update(modulo)
                End If
            End If
          
            movil.veh_nombre_conductor = txtConductor.Text
            movil.veh_descripcion = txtNombre.Text
            movil.veh_patente = txtPatente.Text.ToUpper()
            movil.veh_tipo_id = CInt(ddlTipoMovil.SelectedValue)
            movil.veh_marca = txtMarca.Text
            movil.veh_modelo = txtModelo.Text
            movil.mod_id = ddlModulo.SelectedValue
            movil.veh_activo = rdnActivo.SelectedValue
            movil.cli_id = CInt(ddlCliente.SelectedValue)
            movil.veh_color = txtColor.Text
            movil.tip_uso_id = CInt(ddlTipoUso.SelectedValue)
            movil.veh_modulo_sensor = CBool(rdnModuloSensor.SelectedValue)


            If txtKilometros.Text <> "" Then
                movil.veh_kilometros = CInt(txtKilometros.Text)
            Else
                movil.veh_kilometros = 0
            End If


            movil.veh_nombre_conductor = txtConductor.Text

            If hdnveh_id.Value = "0" Then
                'nuevo
                movil.veh_imagen = ""
                clsVehiculo.Insert(movil)
            Else
                'update
                clsVehiculo.Update(movil)
            End If

            'marco el modulo asociado como en uso
            modulo = clsModulo.Selecionar(ddlModulo.SelectedValue)
            modulo.mod_en_uso = True
            clsModulo.Update(modulo)

            'vuelvo al listado
            Response.Redirect("~/CMS/pAdminVehiculos.aspx", False)
        Catch ex As Exception
            lblError.Text = "Ocurrio un error Grabando los datos - " + ex.Message
        End Try
    End Sub
End Class