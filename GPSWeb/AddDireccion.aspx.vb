Imports GPS.Business
Imports GPS.Data
Partial Public Class AddDireccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim duracion As Long = DateDiff(DateInterval.Second, CDate("2014-09-08 10:21:23.650"), CDate("2014-09-08 10:22:07.447"))

        Dim _duracion As Double = (CDate("2014-09-08 10:21:23.650") - CDate("2014-09-08 10:22:07.447")).TotalSeconds
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        'busco la direccion contra el servicio de openstreet
        Dim direccion As String = ""
        Dim lerror As String = ""
        'clsOpenStreet.getdireccion(txtLatitud.Text, txtLongitud.Text, lerror)

        lblDireccion.Text = direccion
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        'grabo el registro de direccion en la tabla
    End Sub
End Class