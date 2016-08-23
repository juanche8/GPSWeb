Imports GPS.Business
Imports GPS.Data
Public Class pAdminGrupo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'cargo las zonas que tiene el cliente creadas
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    hdncli_id.Value = Session("Cliente").ToString()

                    'cargo la grilla de grupos
                    gridGrupos.DataSource = clsGrupo.Search(hdncli_id.Value)
                    gridGrupos.DataBind()
                End If
            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("Admin Grupo - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos. Contacte al administrador."
        End Try
    End Sub

    Public Sub JQGridGrupo_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        '  JQGridGrupo.DataSource = clsGrupo.Search(hdncli_id.Value)
        '  JQGridGrupo.DataBind()

    End Sub

    
    Private Function GetData() As DataTable

        Dim grupos As List(Of Grupos) = clsGrupo.Search(hdncli_id.Value)
        Dim dt As New DataTable()
        dt.Columns.Add("grup_id", GetType(Integer))
        dt.Columns.Add("grup_nombre")
       
        For Each group As Grupos In grupos
            Dim dr As DataRow = dt.NewRow()
            dr(0) = group.grup_id
            dr(1) = group.grup_nombre

            dt.Rows.Add(dr)
        Next

        Return dt

    End Function

    Protected Sub SortRecords(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        Dim sortExpression As String = e.SortExpression

        Dim direction As String = String.Empty

        If SortDirection = SortDirection.Ascending Then

            SortDirection = SortDirection.Descending
            direction = " DESC"
        Else

            SortDirection = SortDirection.Ascending
            direction = " ASC"
        End If

        Dim table As DataTable = Me.GetData()

        table.DefaultView.Sort = sortExpression & direction
        gridGrupos.DataSource = table
        gridGrupos.DataBind()

    End Sub



    Public Property SortDirection() As SortDirection
        Get
            If ViewState("SortDirection") Is Nothing Then
                ViewState("SortDirection") = SortDirection.Ascending
            End If

            Return DirectCast(ViewState("SortDirection"), SortDirection)
        End Get

        Set(ByVal value As SortDirection)
            ViewState("SortDirection") = value
        End Set

    End Property

    Protected Sub gridGrupo_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gridGrupos.PageIndex = e.NewPageIndex
        gridGrupos.DataSource = GetData()
        gridGrupos.DataBind()

    End Sub

    Protected Sub grid_rowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)


        If e.CommandName = "Borrar" Then
            clsGrupo.Delete(e.CommandArgument)
        End If

        gridGrupos.DataSource = GetData()
        gridGrupos.DataBind()
    End Sub

End Class