Public Partial Class testModulos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtFecha.Text = DateTime.Now.ToString()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        'tomo los datos y los inserto en la tabla modulos de la base Posicionamientos

    End Sub
End Class