'Pagina que para administrar los moviles de los clientes
Imports GPS.Business
Imports GPS.Data
Imports System.Linq
Partial Public Class pAdminVehiculos
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'cargo el combo de tipo de vhiculos

        ddlCliente.DataSource = clsCliente.List()
        ddlCliente.DataBind()

        Dim cboitem As ListItem = New ListItem("-- Todos --", "0")
        ddlCliente.Items.Insert(0, cboitem)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'muestro los moviles del cliente
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then
                    'busco los vehiculos
                    'verifico si viene prefiltrado por cliente
                    If Request.Params("cli_id") IsNot Nothing Then
                        ddlCliente.SelectedValue = Request.Params("cli_id").ToString()
                        gridviewMoviles.DataSource = GetData()
                        gridviewMoviles.DataBind()


                    Else
                        gridviewMoviles.DataSource = GetData()
                        gridviewMoviles.DataBind()
                    End If
                   

                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception
            lblError.Text = "ERROR " + ex.Message
        End Try
    End Sub

    Protected Sub grid_rowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        'verifico si puedo borrar el movil, sino lo desactivo
        'si tiene un modulo asociado lo tengo que volver a poner activo
        If e.CommandName = "Borrar" Then

            Dim movil As Vehiculo = clsVehiculo.Seleccionar(CInt(e.CommandArgument))
            Dim modulo As Modulo = clsModulo.Selecionar(movil.mod_id)

            If modulo IsNot Nothing Then
                modulo.mod_en_uso = False
                clsModulo.Update(modulo)
            End If
            clsVehiculo.Borrar(e.CommandArgument)
            'actualizo la grilla

        End If

        If e.CommandName = "CortarCorriente" Then
            'corto la corriente del movil
            'solo si la velocidad actual es menor a 20kms/h
            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(e.CommandArgument))
            Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(_movil.veh_id)

            If ubicacion IsNot Nothing Then
                If ubicacion.VELOCIDAD <= 20 Then
                    Dim _corte As Cortes_Corriente = New Cortes_Corriente()
                    _corte.cort_admin = DirectCast(Session("Admin"), Administradores).adm_nombre
                    _corte.cort_fecha = DateTime.Now
                    _corte.cort_hecho = False
                    _corte.cort_tipo = "CCC"
                    _corte.veh_id = CInt(e.CommandArgument)
                    _corte.modulo_num = _movil.mod_id
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
            Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(_movil.veh_id)

            If ubicacion IsNot Nothing Then

                Dim _corte As Cortes_Corriente = New Cortes_Corriente()
                _corte.cort_admin = DirectCast(Session("Admin"), Administradores).adm_nombre
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

        gridviewMoviles.DataSource = GetData()
        gridviewMoviles.DataBind()
    End Sub

    Protected Sub grid_DataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If
          Dim boton As ImageButton = DirectCast(e.Row.FindControl("imagebuttonCorriente"), ImageButton)
           'verifico que estado tiene el movil para mostrar el boton de cortar o restablecer la corriente
        Dim estado As String = DirectCast(DirectCast(DirectCast(DirectCast(e.Row, System.Web.UI.WebControls.GridViewRow).DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).ItemArray(10)
        Dim sensor As String = DirectCast(DirectCast(DirectCast(DirectCast(e.Row, System.Web.UI.WebControls.GridViewRow).DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).ItemArray(11)

        If estado = "E" Then 'esta cortada
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

        If sensor = "No" Then
            e.Row.Cells(13).Enabled = False
        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Response.Redirect("~/CMS/pVehiculos.aspx?cli_id=" + ddlCliente.SelectedValue, False)
    End Sub

    ' al cargar la grilla muestro el boton de cortar o de restaurar la corriente

    Private Function GetData() As DataTable

        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.List(ddlCliente.SelectedValue, txtPatente.Text, txtConductor.Text, "", "")

        Dim dt As New DataTable()
        dt.Columns.Add("veh_id", GetType(Integer))
        dt.Columns.Add("Nombre")
        dt.Columns.Add("Patente")
        dt.Columns.Add("Conductor")
        dt.Columns.Add("Tipo_Movil")
        dt.Columns.Add("Modulo")
        dt.Columns.Add("Alarmas")
        dt.Columns.Add("Activo")
        dt.Columns.Add("Cliente")
        dt.Columns.Add("Tipo_Uso")
        dt.Columns.Add("estado_corriente")
        dt.Columns.Add("sensor")

        For Each movil As Vehiculo In vehiculos
            Dim dr As DataRow = dt.NewRow()
            dr(0) = movil.veh_id
            dr(1) = movil.veh_descripcion
            dr(2) = movil.veh_patente
            dr(3) = movil.veh_nombre_conductor
            dr(4) = movil.Tipos_Vehiculos.veh_tipo_detalle
            dr(5) = movil.mod_id.ToString()
            dr(6) = clsAlarma.CantidadByVehiculo(movil.veh_id)

            If movil.veh_activo Then
                dr(7) = "Si"
            Else
                dr(7) = "No"
            End If
            dr(8) = movil.Cliente.cli_apellido + " " + movil.Cliente.cli_nombre
            dr(9) = movil.Tipos_Usos_Moviles.tipo_uso_descripcion
            'verifico el estado actual del movil si esta activo o se le corto la corriente
            dr(10) = clsVehiculo.GetEstado(movil.veh_id)
            If movil.veh_modulo_sensor Then
                dr(11) = "Si"
            Else
                dr(11) = "No"
            End If
            dt.Rows.Add(dr)
        Next

        Return dt

    End Function

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        gridviewMoviles.DataSource = GetData()
        gridviewMoviles.DataBind()
    End Sub
End Class