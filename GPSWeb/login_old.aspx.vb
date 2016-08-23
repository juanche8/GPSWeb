Imports GPS.Business
Imports GPS.Data
Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Login_Click(ByVal sender As Object, ByVal e As EventArgs)
        'verifico que sea un usuario registrado
        Dim cliente As Cliente = clsCliente.Login(Login1.UserName, Login1.Password)

        If cliente IsNot Nothing Then
            'verifico si no esta borrado
            If cliente.cli_borrado Or cliente.cli_activo = False Then
                Dim lerror As Label = DirectCast(Login1.FindControl("lblError"), Label)
                lerror.Text = "Su Usuario no esta activo en el sistema. Contacte al administrador."
                lerror.Visible = True
            Else
                'voy al panel de control del cliente
                Session.Add("Cliente", cliente.cli_id)
                Dim url = Request.UrlReferrer.AbsolutePath
              
                Response.Redirect("Panel_Control/pMapa.aspx", False)


            End If

        Else
        Dim lerror As Label = DirectCast(Login1.FindControl("lblError"), Label)
        lerror.Visible = True
        End If
    End Sub
End Class