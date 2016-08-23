Public Partial Class SiteMaster
    Inherits System.Web.UI.MasterPage

    Public _defaultButton As String = ""
    'propiedad para definir el default button que ejecuta al hacer ENTER    
    Public Property DefaultButton() As String
        Get
            Return _defaultButton
        End Get
        Set(ByVal value As String)
            form1.DefaultButton = value
        End Set
    End Property


    ' Protected Sub linkSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles linkSalir.Click
    '    'logout
    '  Session.Remove("Cliente")
    ' Response.Redirect("~/login.aspx", False)
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'si esta logeado pongo el nombre del usuario
        If Session("Cliente") IsNot Nothing Then
            cliente.Value = CInt(Session("Cliente").ToString)
        End If
    End Sub


End Class