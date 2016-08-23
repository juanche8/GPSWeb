Public Partial Class SiteCMS
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub linkSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles linkSalir.Click
        Session.Remove("Admin")
        Response.Redirect("~/login_admin.aspx", False)
    End Sub
End Class