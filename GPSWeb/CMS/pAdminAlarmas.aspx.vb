'Pagina para mostrar las alertas que se generan para todos los clientes.
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Partial Public Class pAdminAlarmas1
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Dim categ As List(Of Categorias_Alarmas) = clsCategoriaAlarma.List()
        'ddlCategoria.DataSource = categ
        'ddlCategoria.DataBind()

        Dim cboitem As ListItem = New ListItem("-- Todas --", "0")
        'ddlCategoria.Items.Insert(0, cboitem)

        cboitem = New ListItem("-- Todos --", "0")
        ddlMovil.Items.Insert(0, cboitem)

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'cargo el listado de alarmas reportadas, todas las del dia
        Dim veh_id As Integer = 0
        Try
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then

                    'verifico si llega por parametro el codigo de cliente y filtro sus alertas
                    If Request.Params("cli_id") IsNot Nothing Then
                        cargarClientes(CInt(Request.Params("cli_id").ToString))
                        ddlCliente.SelectedValue = CInt(Request.Params("cli_id").ToString)
                        ddlCliente_SelectedIndexChanged(sender, e)
                    Else
                        cargarClientes(0)
                    End If

                    'verifico si filtra desde el mapa por codigo del vehiculo
                    If Request.Params("veh_id") IsNot Nothing Then
                        Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(Request.Params("veh_id").ToString()))
                        cargarClientes(_movil.cli_id)
                        ddlCliente.SelectedValue = CInt(_movil.cli_id)
                        ddlCliente_SelectedIndexChanged(sender, e)
                        ddlMovil.SelectedValue = Request.Params("veh_id").ToString()
                    End If

                    'por defecto rango de fechas en un mes 
                    txtfechaDesde.Text = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy")
                    txtfechaHasta.Text = DateTime.Now.Date.ToString("dd/MM/yyyy")

                    'muestro todas las alarmas 
                    datagridAlertas.DataSource = GetAlarmas()
                    datagridAlertas.DataBind()
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If

        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos. Error: " + ex.Message + " - " + ex.StackTrace
        End Try
    End Sub


    Protected Sub Alarmas_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        datagridAlertas.PageIndex = e.NewPageIndex
        datagridAlertas.DataSource = GetAlarmas()
        datagridAlertas.DataBind()

    End Sub

    Protected Function GetAlarmas() As DataTable
        'verifico opciones de filtro
        Dim fechaDesde As Nullable(Of Date)
        Dim fechaHasta As Nullable(Of Date)
        Dim veh_id As Integer = 0
        If ddlMovil.SelectedValue <> "" Then veh_id = CInt(ddlMovil.SelectedValue)

        If txtfechaDesde.Text <> "" Then fechaDesde = DateTime.Parse(txtfechaDesde.Text + " 00:00:00")
        If txtfechaHasta.Text <> "" Then fechaHasta = DateTime.Parse(txtfechaHasta.Text + " 23:59:59")

        'busco primero alarmas de categorias pre definidas, despues alarmas de categorias propias del cliente

        Dim dt As DataTable
        dt = New DataTable()
        dt.Columns.Add("Alarma")
        dt.Columns.Add("Categoria")
        dt.Columns.Add("Patente")
        dt.Columns.Add("Fecha", GetType(Date))
        dt.Columns.Add("Hora", GetType(TimeSpan))
        dt.Columns.Add("Ubicacion")
        dt.Columns.Add("Valor")
        dt.Columns.Add("alar_id", GetType(Integer))
        dt.Columns.Add("Cliente")

        Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertas(ddlCliente.SelectedValue, veh_id, fechaDesde, fechaHasta, "")

        If alarmas.Count > 0 Then

            For Each d As Alarmas In alarmas
                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.alar_nombre
                dr(1) = d.alar_Categoria

                dr(2) = d.veh_patente
                dr(3) = d.alar_fecha.Date
                dr(4) = TimeSpan.Parse(d.alar_fecha.ToString("hh:mm:ss"))
                dr(5) = d.alar_nombre_via + "," + d.alar_Localidad + "," + d.alar_Provincia
                dr(6) = d.alar_valor

                dr(7) = d.alar_id
                dr(8) = clsVehiculo.Seleccionar(d.veh_id.Value).Cliente.cli_nombre

                dt.Rows.Add(dr)
            Next
        End If

        Return dt

    End Function

    Protected Sub ddlCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        'cargo lso vehiculos del cliente
        ddlMovil.DataSource = clsVehiculo.List(ddlCliente.SelectedValue)
        ddlMovil.DataBind()

        ddlMovil.Items.Insert(0, New ListItem("-- Todos --", "0"))
    End Sub

    Protected Sub btBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btBuscar.Click
        'busco con el filtro aplicado
        datagridAlertas.DataSource = GetAlarmas()
        datagridAlertas.DataBind()
    End Sub

    Protected Sub SortRecords(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        Dim sortExpression As String = e.SortExpression
        Dim direction As String = String.Empty


        If e.SortDirection = SortDirection.Ascending Then

            e.SortDirection = SortDirection.Descending
            direction = " DESC"
        Else
            e.SortDirection = SortDirection.Ascending
            direction = " ASC"
        End If

    End Sub

    Private Sub cargarClientes(ByVal cli_id As Integer)

        Dim cboitem As ListItem
        Dim lclientes As List(Of Cliente) = clsCliente.ListActivos

        For Each cliente As Cliente In lclientes
            cboitem = New ListItem(cliente.cli_nombre + " " + cliente.cli_apellido, cliente.cli_id.ToString)
            ddlCliente.Items.Add(cboitem)
        Next

        cboitem = New ListItem("-- Todos --", "0")
        If cli_id = 0 Then
            ddlCliente.Items.Insert(0, cboitem)
        Else
            ddlCliente.Items.Add(cboitem)
        End If
    End Sub
End Class