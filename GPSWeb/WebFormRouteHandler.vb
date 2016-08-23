Imports Microsoft.VisualBasic
Imports System.Web.Routing
Imports System.Security
Imports System.Web.Compilation
Public Class WebFormRouteHandler
    Implements IRouteHandler

    Public VirtualPath As String
    Public CheckPhysicalUrlAccess As Boolean

    ' Methods
    Public Sub New(ByVal virtualPath As String)
        Me.New(virtualPath, True)
    End Sub

    Public Sub New(ByVal virtualPath As String, ByVal checkPhysicalUrlAccess As Boolean)
        Me.VirtualPath = virtualPath
        Me.CheckPhysicalUrlAccess = checkPhysicalUrlAccess
    End Sub

    Public Function GetHttpHandler(ByVal requestContext As RequestContext) As IHttpHandler Implements IRouteHandler.GetHttpHandler

        If (Me.CheckPhysicalUrlAccess AndAlso Not UrlAuthorizationModule.CheckUrlAccessForPrincipal(Me.VirtualPath, requestContext.HttpContext.User, requestContext.HttpContext.Request.HttpMethod)) Then
            Throw New SecurityException
        End If

        Dim page As IHttpHandler = TryCast(BuildManager.CreateInstanceFromVirtualPath(Me.VirtualPath, GetType(Page)), IHttpHandler)

        If (Not page Is Nothing) Then
            Dim routablePage As IRoutablePage = TryCast(page, IRoutablePage)
            If (Not routablePage Is Nothing) Then
                routablePage.RequestContext = requestContext
            End If
        End If

        Return page

    End Function

End Class

