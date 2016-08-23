Imports System.Web
Imports System.Web.Services

Public Class refresh_session
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

      

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property


    Public Class MantenSesionHandler
        Implements IRequiresSessionState

        Dim cliente As Object
        'aquí el código del manejador, que esencialmente estará vacío.

    End Class

End Class