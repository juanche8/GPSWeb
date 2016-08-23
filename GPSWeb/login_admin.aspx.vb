Imports GPS.Business
Imports GPS.Data
Partial Public Class login_admin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Login_Click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        'verifico y voy al modulo de administracion
        Dim administrador As Administradores = clsCliente.LoginAdmin(username.Value, password.Value)

        If administrador IsNot Nothing Then
            'voy al panel de control del cliente
            Session.Add("Admin", administrador)
            Response.Redirect("~/CMS/pAdminMapa.aspx", False)
        Else

            lblError.Text = "Usuario o contraseña incorrectos."
        End If
    End Sub
End Class