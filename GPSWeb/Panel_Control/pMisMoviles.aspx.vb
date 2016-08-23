'Pagina que muestra al cliente los vehiculos agregados al sistema
Imports GPS.Business
Imports GPS.Data
Imports System.Linq

Partial Public Class pMisMoviles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'muestro los moviles del cliente
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                'enter por defecto
                DirectCast(Page.Master, SiteMaster).DefaultButton = btnBuscar.UniqueID

                If Not IsPostBack Then
                    'busco los vehiculos del usuario

                    hdncli_id.Value = Session("Cliente").ToString()

                    gridMoviles.DataSource = GetData()
                    gridMoviles.DataBind()

                End If
            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("MIS MOVILES - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al administrador. " & ex.ToString
        End Try
    End Sub

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
        gridMoviles.DataSource = table

        gridMoviles.DataBind()

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

    Protected Sub gridMoviles_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gridMoviles.PageIndex = e.NewPageIndex
        gridMoviles.DataSource = GetData()
        gridMoviles.DataBind()

    End Sub

    Private Function GetData() As DataTable

        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.List(CInt(hdncli_id.Value), txtPantente.Text, txtConductor.Text, txtMarca.Text, txtModelo.Text)
        Dim dt As New DataTable()
        dt.Columns.Add("veh_id", GetType(Integer))
        dt.Columns.Add("Nombre")
        dt.Columns.Add("Patente")
        dt.Columns.Add("Activo")
        dt.Columns.Add("ubicacion") '10  
        dt.Columns.Add("velocidad")
        dt.Columns.Add("temp")
        dt.Columns.Add("encendido")
        dt.Columns.Add("libre")
        dt.Columns.Add("fecha", GetType(DateTime))
        dt.Columns.Add("rpm")
        dt.Columns.Add("estado_corriente")
        dt.Columns.Add("kms")
       
        'busco el listado de categorias de alarmas
        Dim CatAlarmasVelo As List(Of Alarmas_Velocidad) = clsCategoriaAlarma.ListExcVelocidad()
        ' Dim catAlarmasPropias As List(Of Categorias_Alarmas_Clientes) = clsCategoriaAlarma.ListByCliente(CInt(hdncli_id.Value))

        For Each movil As Vehiculo In vehiculos
            Dim dr As DataRow = dt.NewRow()
            dr(0) = movil.veh_id
            dr(1) = movil.veh_descripcion
            dr(2) = movil.veh_patente
            If movil.veh_activo Then
                dr(3) = "Si"
            Else
                dr(3) = "No"
            End If

            'busco al ubicaciona ctual del movil
            Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocation(movil.mod_id)
            If ubicacion IsNot Nothing Then
                dr(4) = ubicacion.NOMBRE_VIA & "," & ubicacion.LOCALIDAD & "," & ubicacion.PROVINCIA
                dr(5) = String.Format("{0:###,0}", ubicacion.VELOCIDAD)
                dr(6) = ubicacion.TEMP
                dr(7) = IIf(ubicacion.ENCENDIDO, "Si", "No")
                If ubicacion.OCUPADO Then
                    dr(8) = "Ocupado"
                Else
                    dr(8) = "Libre"
                End If

                dr(9) = ubicacion.FECHA
                dr(10) = ubicacion.RPM
            Else
                dr(4) = "Sin Datos"
                dr(5) = "0"
                dr(6) = "0"
                dr(7) = ""
                dr(8) = ""
                dr(9) = DateTime.Now
                dr(10) = "0"
            End If

            'la cantidad de alarmas la tengo que mostrar por categoria
            'Dim total As Integer = 0

            'total = 0
            ' total = clsAlarma.CantidadByVehiculo(movil.veh_id, 1) 'velocidad
            ' dr(10) = total

            'total = clsAlarma.CantidadByVehiculo(movil.veh_id, 6) 'recordatorios
            'dr(11) = total

            'total = clsAlarma.CantidadByVehiculo(movil.veh_id, 2) 'sensores
            'dr(12) = total
            'total = clsAlarma.CantidadByVehiculo(movil.veh_id, 4) 'recorridos
            'dr(13) = total
            'total = clsAlarma.CantidadByVehiculo(movil.veh_id, 5) 'zonas
            'dr(14) = total
            'total = clsAlarma.CantidadByVehiculo(movil.veh_id, 3) 'direcciones
            'dr(15) = total
            'cantidad de alarmas propias
            ' total = 0
            ' For Each categoriaPropia As Categorias_Alarmas_Clientes In catAlarmasPropias
            'total += clsAlarma.CantidadByVehiculo(movil.veh_id, categoriaPropia.cat_usu_id, 0)
            'Next
            'verifico el estado actual del movil si esta activo o se le corto la corriente
            dr(11) = clsVehiculo.GetEstado(movil.veh_id)
            dr(12) = clsVehiculo.KmsAcumuladosByVehiculo(movil.veh_id) + movil.veh_kilometros.Value + movil.veh_kilometros_acumulados

            dt.Rows.Add(dr)
        Next

        Return dt

    End Function

    Private Function GetDatos(ByVal veh_id As Integer) As DataTable

        Dim vehiculos As Vehiculo = clsVehiculo.Seleccionar(CInt(veh_id))
        Dim dt As New DataTable()
        dt.Columns.Add("veh_id", GetType(Integer))

        dt.Columns.Add("Conductor")
        dt.Columns.Add("Tipo_Movil")

        dt.Columns.Add("Tipo_Uso")
        dt.Columns.Add("Marca")
        dt.Columns.Add("Modelo")
        dt.Columns.Add("Color")


        'busco el listado de categorias de alarmas

        Dim dr As DataRow = dt.NewRow()
        dr(0) = vehiculos.veh_id

        dr(1) = vehiculos.veh_nombre_conductor
        dr(2) = vehiculos.Tipos_Vehiculos.veh_tipo_detalle
        dr(3) = vehiculos.Tipos_Usos_Moviles.tipo_uso_descripcion
        dr(4) = vehiculos.veh_marca
        dr(5) = vehiculos.veh_modelo
        dr(6) = vehiculos.veh_color

        dt.Rows.Add(dr)


        Return dt

    End Function


    Protected Sub grid_rowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)


        If e.CommandName = "CortarCorriente" Then
            'corto la corriente del movil
            'solo si la velocidad actual es menor a 20kms/h
            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(e.CommandArgument))
            Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocation(_movil.veh_id)

            If ubicacion IsNot Nothing Then
                If ubicacion.VELOCIDAD <= 20 Then
                    Dim _corte As Cortes_Corriente = New Cortes_Corriente()
                    _corte.cort_admin = "Cliente Id " & Session("Cliente").ToString()
                    _corte.cort_fecha = DateTime.Now
                    _corte.cort_hecho = False
                    _corte.cort_tipo = "CCC"
                    _corte.modulo_num = _movil.mod_id
                    _corte.veh_id = CInt(e.CommandArgument)
                    clsVehiculo.CortarCorriente(_corte)
                Else
                    'notifico
                    lblError.Text = "La velocidad del Movil no es menor a 20 km/h no puede cortar la corriente."
                End If
            Else
                lblError.Text = "El Movil no tiene reporte de Ubicación. No se puede ejecutar la acción."
            End If
        End If

        If e.CommandName = "Restablecer" Then
            'corto la corriente del movil

            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(e.CommandArgument))
            Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocation(_movil.veh_id)

            If ubicacion IsNot Nothing Then

                Dim _corte As Cortes_Corriente = New Cortes_Corriente()
                _corte.cort_admin = "Cliente Id " & Session("Cliente").ToString()
                _corte.cort_fecha = DateTime.Now
                _corte.cort_hecho = False
                _corte.cort_tipo = "RCC"
                _corte.veh_id = CInt(e.CommandArgument)
                _corte.modulo_num = _movil.mod_id
                clsVehiculo.CortarCorriente(_corte)

            Else
                lblError.Text = "El Movil no tiene reporte de Ubicación. No se puede ejecutar la acción."
            End If
        End If

        gridMoviles.DataSource = GetData()
        gridMoviles.DataBind()
    End Sub

    Protected Sub grid_DataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If
        Dim boton As ImageButton = DirectCast(e.Row.FindControl("imagebuttonCorriente"), ImageButton)
        'verifico que estado tiene el movil para mostrar el boton de cortar o restablecer la corriente
        Dim estado As String = DirectCast(DirectCast(DirectCast(DirectCast(e.Row, System.Web.UI.WebControls.GridViewRow).DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).ItemArray(11)

        If estado = "CCC" Then 'esta cortada
            boton.ImageUrl = "~/images/electricidad.jpg"
            boton.CommandName = "Restablecer"
            boton.ToolTip = "Restablecer Electricidad"
            boton.OnClientClick = "return confirm('Esta Seguro de Restablecer la Corriente del Vehiculo Seleccionado?');"
        Else
            'no esta cortado
            boton.ImageUrl = "~/images/electricidad_no.jpg"
            boton.CommandName = "CortarCorriente"
            boton.ToolTip = "Cortar Electricidad"
            boton.OnClientClick = "return confirm('Esta Seguro de Cortar la Corriente del Vehiculo Seleccionado?');"

        End If

        Dim gvDatos As GridView = DirectCast(e.Row.FindControl("gridDatos"), GridView)

        gvDatos.DataSource = GetDatos(CInt(DirectCast(DirectCast(DirectCast(DirectCast(e.Row, System.Web.UI.WebControls.GridViewRow).DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).ItemArray(0)))
        gvDatos.DataBind()

    End Sub
  
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            'filtro el listado
            gridMoviles.DataSource = GetData()
            gridMoviles.DataBind()
        Catch ex As Exception
            Funciones.WriteToEventLog("MIS MOVILES - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al administrador."
        End Try
        
    End Sub
End Class