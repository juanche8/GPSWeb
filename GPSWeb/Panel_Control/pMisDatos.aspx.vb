'Pagina donde el cliente puede cambiar sus datos de registro
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Partial Public Class pMisDatos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    'Busco los datos del cliente

                    hdncli_id.Value = Session("Cliente").ToString()

                    Dim cliente As Cliente = clsCliente.SelectById(CInt(hdncli_id.Value))
                    txtContrasenia.Text = cliente.cli_contrasenia
                    txtApellido.Text = cliente.cli_apellido
                    txtDni.Text = cliente.cli_DNI
                    txtCP.Text = cliente.cli_CP
                    If cliente.cli_fecha_nacimiento IsNot Nothing Then txtFechaNac.Text = cliente.cli_fecha_nacimiento
                    txtDireccion.Text = cliente.cli_direccion
                    txtMail.Text = cliente.cli_email
                    txtNombre.Text = cliente.cli_nombre
                    txtTelefono.Text = cliente.cli_telefono
                    ' rdnEnviaSMS.SelectedValue = cliente.cli_enviar_sms

                End If

            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos, contacte al administrador."
            Funciones.WriteToEventLog("MIS DATOS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            'grabo los datos del cliente
            Dim cliente As Cliente = clsCliente.SelectById(hdncli_id.Value)

            cliente.cli_contrasenia = txtContrasenia.Text
            cliente.cli_apellido = txtApellido.Text
            cliente.cli_DNI = txtDni.Text
            cliente.cli_CP = txtCP.Text
            If txtFechaNac.Text <> "" Then cliente.cli_fecha_nacimiento = txtFechaNac.Text
            cliente.cli_direccion = txtDireccion.Text
            cliente.cli_email = txtMail.Text
            cliente.cli_nombre = txtNombre.Text
            cliente.cli_telefono = txtTelefono.Text
          
            clsCliente.Update(cliente)
         
            lblError.Text = "Sus datos se actualizarón con Exito."

        Catch ex As Exception
            lblError.Text = "Ocurrio un error grabando los datos, contacte al administrador."
            Funciones.WriteToEventLog("MIS DATOS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub
End Class