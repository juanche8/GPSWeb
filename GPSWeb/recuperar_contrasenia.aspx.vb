Imports GPS.Business
Imports GPS.Data

Public Class recuperar_contrasenia
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnRecuperar_Click(sender As Object, e As EventArgs) Handles btnRecuperar.Click
        Try
            'busco el mail ingresado en los registrados como clientes
            'envio por mail los datos de ingreso
            Dim cliente As Cliente = clsCliente.SearchByMail(mail.Value)
            If cliente IsNot Nothing Then

                Dim mensaje As String = "Estimado Cliente: <br/> Le reenviamos sus datos de registro en nuestro sistema:<br/> Usuario: " & cliente.cli_email & "<br/> Contraseña: " & cliente.cli_contrasenia & "<br/><br/>" & "Saludos Cordiales. El equipo de Rastreo Web."
                Funciones.Send_Mail_Cliente("Datos Registro en Rastreo Web", mensaje, mail.Value)
            Else

                lblError.Text = "El e-mail ingresado no existe en nuestro sistema."
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class