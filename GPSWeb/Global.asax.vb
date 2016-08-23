Imports System.Web.SessionState
Imports System.Web.Routing
Imports System.Collections.Generic


Public Class Global_asax
    Inherits System.Web.HttpApplication

  Private Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.Add("home", New Route("Home", New WebFormRouteHandler("~/index.aspx")))
        routes.Add("productoweb", New Route("Rastreo-Web", New WebFormRouteHandler("~/productos.aspx")))
        routes.Add("modulos", New Route("Equipos-GPS", New WebFormRouteHandler("~/prodmodulos.aspx")))
        routes.Add("servicios", New Route("Servicios", New WebFormRouteHandler("~/servicios.aspx")))
        routes.Add("soluciones", New Route("Soluciones-Empresas", New WebFormRouteHandler("~/Soluciones_empresas.aspx")))
        routes.Add("logistica", New Route("Soluciones-Logistica", New WebFormRouteHandler("~/logistica.aspx")))
        routes.Add("contacto", New Route("Contacto", New WebFormRouteHandler("~/Contacto.aspx")))
        routes.Add("quienes", New Route("Quienes-Somos", New WebFormRouteHandler("~/quienes_somos.aspx")))
        routes.Add("login", New Route("Acceso-Clientes", New WebFormRouteHandler("~/login.aspx")))
    End Sub

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        RegisterRoutes(RouteTable.Routes)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la sesión
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

End Class

