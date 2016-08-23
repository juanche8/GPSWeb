Imports Microsoft.VisualBasic
Imports System.Web.Routing

Public Interface IRoutablePage
    ' Properties
    Property RequestContext() As RequestContext
End Interface

Public Class RoutablePage
    Inherits Page
    Implements IRoutablePage

    ' Fields
    Protected _requestContext As RequestContext

    ' Methods
    Public Function ActionLink(ByVal url As String) As String
        Return Me.ActionLink(url, url)
    End Function

    Protected Function ActionLink(ByVal url As String, ByVal [text] As String) As String
        Return String.Format("<a href=""{0}"">{1}</a>", url, [text])
    End Function

    Public Function GetVirtualPath(ByVal values As Object) As String
        Return Me.GetVirtualPath(Nothing, values)
    End Function

    Public Function GetVirtualPath(ByVal routeName As String, ByVal values As Object) As String
        If (Me.RequestContext Is Nothing) Then
            Return Nothing
        End If
        Return RouteTable.Routes.GetVirtualPath(Me.RequestContext, routeName, New RouteValueDictionary(values)).VirtualPath
    End Function

    Protected Function RouteValue(ByVal key As String) As Object
        Return Me.RequestContext.RouteData.Values.Item(key)
    End Function

    ' Properties
    Public ReadOnly Property BaseUrl() As String
        Get
            Return (MyBase.Request.Url.GetLeftPart(1) & VirtualPathUtility.ToAbsolute("~/"))
        End Get
    End Property

    Public Property RequestContext() As RequestContext Implements IRoutablePage.RequestContext
        Set(ByVal value As RequestContext)
            _requestContext = value
        End Set
        Get
            Return _requestContext
        End Get
    End Property

End Class
