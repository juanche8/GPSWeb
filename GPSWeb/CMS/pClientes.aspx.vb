'Pagina para dar de alta o modificar clientes
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Partial Public Class pClientes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'busco los datos del cliente si esta editando o muestro el formulario para agregar uno nuevo
        Try
            lblerror.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then

                    'cargo elc ombo de tipo de clientes
                    ddlTipoCliente.DataSource = clsParametros.TipoClienteList()
                    ddlTipoCliente.DataBind()

                  
                    'es una edicion cargo los datos del cliente
                    If Request.Params("cli_id") IsNot Nothing Then
                        hdncli_id.Value = Request.Params("cli_id").ToString()
                        Dim cliente As Cliente = clsCliente.SelectById(CInt(Request.Params("cli_id").ToString()))

                        'datos generales
                        txtApellido.Text = cliente.cli_apellido
                        txtNombre.Text = cliente.cli_nombre
                        txtDni.Text = cliente.cli_DNI
                        If cliente.cli_fecha_nacimiento IsNot Nothing Then txtFechaNac.Text = cliente.cli_fecha_nacimiento
                        txtFormaPago.Text = cliente.cli_forma_pago
                        txtContrasenia.Text = cliente.cli_contrasenia
                        txtDireccion.Text = cliente.cli_direccion
                        txtCP.Text = cliente.cli_CP
                        txtMail.Text = cliente.cli_email
                        txtTelefono.Text = cliente.cli_telefono
                        ddlTipoCliente.SelectedValue = cliente.tipo_cli_id
                        rdnActivo.SelectedValue = cliente.cli_activo
                        'rdnEnviaSMS.SelectedValue = cliente.cli_enviar_sms
                      

                        If cliente.cli_fecha_nacimiento IsNot Nothing Then txtFechaNac.Text = cliente.cli_fecha_nacimiento.Value.ToString("dd/MM/yyyy")

                        'datos tipo Cliente
                        'If cliente.Clientes_Particulares.Count > 0 Then
                        'txtApellidoParticular.Text = cliente.Clientes_Particulares.FirstOrDefault().part_apellido
                        'txtNombreParticular.Text = cliente.Clientes_Particulares.FirstOrDefault().part_nombre
                        'txtDireccionParticular.Text = cliente.Clientes_Particulares.FirstOrDefault().part_domicilio
                        'txtTel1Particular.Text = cliente.Clientes_Particulares.FirstOrDefault().part_telefono_1
                        'txtTel2Particular.Text = cliente.Clientes_Particulares.FirstOrDefault().part_telefono_2
                        ' txtTel3Particular.Text = cliente.Clientes_Particulares.FirstOrDefault().part_telefono_3
                        'End If

                        If cliente.Clientes_Empresas.Count > 0 Then
                            txtDireccionEmpresa.Text = cliente.Clientes_Empresas.FirstOrDefault().empr_Domicilio
                            txtCUIT.Text = cliente.Clientes_Empresas.FirstOrDefault().empr_CUIT
                            txtCPEmpresa.Text = cliente.Clientes_Empresas.FirstOrDefault().empr_CP
                            txtDireccionEmpresa.Text = cliente.Clientes_Empresas.FirstOrDefault().empr_Domicilio
                            txtRazonSocial.Text = cliente.Clientes_Empresas.FirstOrDefault().empr_razon_social
                            txtTel1Empresa.Text = cliente.Clientes_Empresas.FirstOrDefault().empr_telefono_1
                            txtTel2Empresa.Text = cliente.Clientes_Empresas.FirstOrDefault().empr_telefono_2


                        End If

                        'datos contacto
                        If cliente.Contactos_Clientes.Count > 0 Then
                            hdnCont1.Value = cliente.Contactos_Clientes(0).cont_id.ToString()
                            txtApellidoContacto1.Text = cliente.Contactos_Clientes(0).cont_apellido
                            txtNombreContacto1.Text = cliente.Contactos_Clientes(0).cont_nombre
                            txtTel1Contacto.Text = cliente.Contactos_Clientes(0).cont_telefono_1

                            If cliente.Contactos_Clientes.Count > 1 Then
                                hdnCont2.Value = cliente.Contactos_Clientes(1).cont_id.ToString()
                                txtApellidoContacto2.Text = cliente.Contactos_Clientes(1).cont_apellido
                                txtNombreContacto2.Text = cliente.Contactos_Clientes(1).cont_nombre
                                txtTelContacto2.Text = cliente.Contactos_Clientes(1).cont_telefono_1
                            End If

                            If cliente.Contactos_Clientes.Count >= 2 Then
                                hdnCont3.Value = cliente.Contactos_Clientes(2).cont_id.ToString()
                                txtApellidoContacto3.Text = cliente.Contactos_Clientes(2).cont_apellido
                                txtNombreContacto3.Text = cliente.Contactos_Clientes(2).cont_nombre
                                txtTelContacto3.Text = cliente.Contactos_Clientes(2).cont_telefono_1
                            End If
                        End If

                        'ejecuto la accion del combo de tipo de cliente
                        ddlTipoCliente_SelectedIndexChanged(sender, e)

                        'verifico los sensores que tiene asignados para marcarlos

                    End If

                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception
            lblerror.Text = "Ocurrio un error al cargar los Datos. " + ex.Message
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            'grabo los datos del cliente
            Dim insertEmpresa, insertParticular, insertContacto As Boolean
            insertEmpresa = True
            insertParticular = True
            insertContacto = True
            Dim cliente As Cliente = New Cliente()
            Dim clienteParticular As Clientes_Particulares = New Clientes_Particulares()
            Dim clienteEmpresa As Clientes_Empresas = New Clientes_Empresas()
            Dim clienteContacto As Contactos_Clientes = New Contactos_Clientes()

            Dim cli_id As Integer = 0
            If hdncli_id.Value <> "0" Then
                cliente = clsCliente.SelectById(CInt(hdncli_id.Value))
                cli_id = cliente.cli_id
            End If



            'verifico que no exista el mail
            If clsCliente.Existe(txtMail.Text, cli_id) IsNot Nothing Then
                lblerror.Text = "El e-mail ingresado ya Existe en el sistema."
            Else
                'datos generales
                cliente.cli_apellido = txtApellido.Text
                cliente.cli_nombre = txtNombre.Text
                cliente.cli_DNI = txtDni.Text

                If txtFechaNac.Text <> "" Then cliente.cli_fecha_nacimiento = CDate(txtFechaNac.Text)
                cliente.cli_forma_pago = txtFormaPago.Text
                cliente.cli_contrasenia = txtContrasenia.Text
                cliente.cli_direccion = txtDireccion.Text
                cliente.cli_CP = txtCP.Text
                cliente.cli_email = txtMail.Text
                cliente.cli_telefono = txtTelefono.Text
                cliente.tipo_cli_id = CInt(ddlTipoCliente.SelectedValue)
                cliente.cli_activo = CBool(rdnActivo.SelectedValue)
                '  cliente.cli_enviar_sms = CBool(rdnEnviaSMS.SelectedValue)
                'datos tipo Cliente

                If ddlTipoCliente.SelectedItem.Text = "Particular" Then
                    If cliente.Clientes_Particulares.Count > 0 Then
                        clienteParticular = cliente.Clientes_Particulares.FirstOrDefault()
                        insertParticular = False
                    End If

                    clienteParticular.part_apellido = txtApellido.Text
                    clienteParticular.part_nombre = txtNombre.Text
                    clienteParticular.part_domicilio = txtDireccion.Text
                    clienteParticular.part_telefono_1 = txtTelefono.Text
                    clienteParticular.part_telefono_2 = ""
                    clienteParticular.part_telefono_3 = ""

                Else
                    If cliente.Clientes_Empresas.Count > 0 Then
                        clienteEmpresa = cliente.Clientes_Empresas.FirstOrDefault()
                        insertEmpresa = False
                    End If

                    clienteEmpresa.empr_Domicilio = txtDireccionEmpresa.Text
                    clienteEmpresa.empr_CUIT = txtCUIT.Text
                    clienteEmpresa.empr_CP = txtCPEmpresa.Text
                    clienteEmpresa.empr_Domicilio = txtDireccionEmpresa.Text
                    clienteEmpresa.empr_razon_social = txtRazonSocial.Text
                    clienteEmpresa.empr_telefono_1 = txtTel1Empresa.Text
                    clienteEmpresa.empr_telefono_2 = txtTel2Empresa.Text
                End If



                'datos contacto
                Dim contacto1 As New Contactos_Clientes()
                contacto1.cont_apellido = txtApellidoContacto1.Text
                contacto1.cont_nombre = txtNombreContacto1.Text
                contacto1.cont_telefono_1 = txtTel1Contacto.Text
                contacto1.cont_telefono_2 = ""

                Dim contacto2 As New Contactos_Clientes()
                contacto2.cont_apellido = txtApellidoContacto2.Text
                contacto2.cont_nombre = txtNombreContacto2.Text
                contacto2.cont_telefono_1 = txtTelContacto2.Text
                contacto2.cont_telefono_2 = ""

                Dim contacto3 As New Contactos_Clientes()
                contacto3.cont_apellido = txtApellidoContacto3.Text
                contacto3.cont_nombre = txtNombreContacto3.Text
                contacto3.cont_telefono_1 = txtTelContacto3.Text
                contacto3.cont_telefono_2 = ""

                If hdncli_id.Value <> "0" Then
                    clsCliente.Update(cliente)

                    If ddlTipoCliente.SelectedItem.Text = "Particular" Then
                        If insertParticular Then
                            clienteParticular.cli_id = cliente.cli_id
                            clsCliente.InsertParticular(clienteParticular)
                        Else
                            clsCliente.UpdateParticular(clienteParticular)
                        End If
                    Else
                        If insertEmpresa Then
                            clienteEmpresa.cli_id = cliente.cli_id
                            clsCliente.InsertEmpresa(clienteEmpresa)
                        Else
                            clsCliente.UpdateEmpresa(clienteEmpresa)
                        End If
                    End If

                    If hdnCont1.Value <> "0" Then
                        contacto1.cont_id = CInt(hdnCont1.Value)
                        clsCliente.UpdateContacto(contacto1)
                    Else
                        If contacto1.cont_nombre <> "" Or contacto1.cont_apellido <> "" Or contacto1.cont_telefono_1 <> "" Then
                            contacto1.cli_id = cliente.cli_id
                            clsCliente.InsertContacto(contacto1)
                        End If

                    End If

                    If hdnCont2.Value <> "0" Then
                        contacto2.cont_id = CInt(hdnCont2.Value)
                        clsCliente.UpdateContacto(contacto2)
                    Else

                        If contacto2.cont_nombre <> "" Or contacto2.cont_apellido <> "" Or contacto2.cont_telefono_1 <> "" Then
                            contacto2.cli_id = cliente.cli_id
                            clsCliente.InsertContacto(contacto2)
                        End If
                    End If

                    If hdnCont3.Value <> "0" Then
                        contacto3.cont_id = CInt(hdnCont3.Value)
                        clsCliente.UpdateContacto(contacto3)
                    Else

                        If contacto3.cont_nombre <> "" Or contacto3.cont_apellido <> "" Or contacto3.cont_telefono_1 <> "" Then
                            contacto3.cli_id = cliente.cli_id
                            clsCliente.InsertContacto(contacto3)
                        End If

                    End If


                Else
                    cliente.cli_fecha_creacion = DateTime.Now
                    cliente.cli_borrado = False
                    clsCliente.Insert(cliente)

                    If ddlTipoCliente.SelectedItem.Text = "Particular" Then
                        clienteParticular.cli_id = cliente.cli_id
                        clsCliente.InsertParticular(clienteParticular)
                    Else
                        clienteEmpresa.cli_id = cliente.cli_id
                        clsCliente.InsertEmpresa(clienteEmpresa)
                    End If

                    contacto1.cli_id = cliente.cli_id
                    contacto2.cli_id = cliente.cli_id
                    contacto3.cli_id = cliente.cli_id
                    clsCliente.InsertContacto(contacto1)
                    clsCliente.InsertContacto(contacto2)
                    clsCliente.InsertContacto(contacto3)

                End If

                'sensores

              

                'retorno al listado
                Response.Redirect("~/CMS/pAdminClientes.aspx", False)
            End If



        Catch ex As Exception
            lblerror.Text = "Ocurrio un error al Grabar los Datos. " + ex.Message
        End Try
    End Sub

    Protected Sub ddlTipoCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoCliente.SelectedIndexChanged
        ' si el tipo de cliente no es empresa muestro datos de particular
        If ddlTipoCliente.SelectedItem.Text = "Particular" Then
            'PanelParticular.Visible = True
            PanelEmpresa.Visible = False
        Else
            ' PanelParticular.Visible = False
            PanelEmpresa.Visible = True
        End If

        'UpdatePanel2.Update()
    End Sub

    

    
End Class