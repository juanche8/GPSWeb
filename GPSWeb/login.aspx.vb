Imports GPS.Business
Imports GPS.Data

Public Class WebForm2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        'verifico que sea un usuario registrado
        Dim cliente As Cliente = clsCliente.Login(username.Value, password.Value)

        If cliente IsNot Nothing Then
            'verifico si no esta borrado
            If cliente.cli_borrado Or cliente.cli_activo = False Then

                lblError.Text = "Su Usuario no esta activo en el sistema. Contacte al administrador."

            Else
                'voy al panel de control del cliente
                Session.Add("Cliente", cliente.cli_id)
                Dim url = Request.UrlReferrer.AbsolutePath

                Response.Redirect("Panel_Control/pMapa.aspx", False)


            End If

        Else

            lblError.Text = "Usuario o contraseña incorrectos."
        End If
    End Sub
End Class