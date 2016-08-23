'Pagina para mostrar las alertas que se generan para los vehiculos del cliente.
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Services
Imports System.Web.Services.Protocols

Partial Public Class pAlarmas
    Inherits System.Web.UI.Page
    'por defecto rango de fechas en un mes 
    Public fechaDesde As DateTime = DateTime.Now.AddMonths(-1) 'Nullable(Of Date)
    Public fechaHasta As DateTime = DateTime.Now
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'cargo el listado de categorias
        'cargo el listado de alarmas por defecto la primer categoria
        Dim veh_id As Integer = 0
        Try
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    'verifico si me llega por parametro el id del movil para ver al alertas de ese vehiculo
                    'solo alertas del dia
                    hdncli_id.Value = Session("Cliente").ToString
                    '   Session.Remove("Filtro")
                    If Request.Params("veh_id") IsNot Nothing Then

                        veh_id = CInt(Request.Params("veh_id").ToString())
                        hdnveh_id.Value = Request.Params("veh_id").ToString()
                        PanelPatente.Visible = True
                        lblPatente.Text = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value)).veh_patente
                    End If

                    'verifico si me llega el parametro de la alarma que hay que borrar.
                    If Request.Params("alar_id") IsNot Nothing Then
                        EliminarAlarma(Request.Params("alar_id").ToString())
                    End If

                    CargarGrilla()

                    'verifico el tab que tengo que dejar marcados
                    If Request.Params("tab") IsNot Nothing Then
                        hdnTab.Value = Request.Params("tab").ToString()
                    End If

                End If



            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If


        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador. - " + ex.Message
            Funciones.WriteToEventLog("ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Public Sub JQGridAlarmas_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        JQGridAlarmas.DataSource = GetAlarmas(hdnveh_id.Value)
        JQGridAlarmas.DataBind()



    End Sub

    Protected Function GetAlarmas(ByVal veh_id As Integer) As DataTable
        Dim dt As DataTable = New DataTable()

        If Session("Filtro") IsNot Nothing Then

            Dim filtro As String() = Session("Filtro").ToString().Split("|")
            fechaDesde = DateTime.Parse(filtro(0))
            fechaHasta = DateTime.Parse(filtro(1))



        End If

        Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertasVisibles(CInt(hdncli_id.Value), veh_id, fechaDesde, fechaHasta, "")



        dt.Columns.Add("Alarma")
        dt.Columns.Add("Categoria")
        dt.Columns.Add("Patente")
        dt.Columns.Add("Fecha", GetType(DateTime))
        dt.Columns.Add("Hora")
        dt.Columns.Add("Ubicacion")
        dt.Columns.Add("Valor", GetType(Integer))
        dt.Columns.Add("alar_id", GetType(Integer))
        dt.Columns.Add("Conductor")
        dt.Columns.Add("Limite")
        dt.Columns.Add("Mail_Enviado")
        dt.Columns.Add("SMS_Enviado")
        dt.Columns.Add("latLng")
        dt.Columns.Add("duracion")

        For Each d As Alarmas In alarmas

            Dim dr As DataRow = dt.NewRow()
            dr(0) = d.alar_nombre
            dr(1) = d.alar_Categoria

            dr(2) = d.veh_patente
            dr(3) = d.alar_fecha
            dr(4) = d.alar_fecha.ToString("HH:mm:ss")
            dr(5) = d.alar_nombre_via + "," + d.alar_Localidad + "," + d.alar_Provincia
            dr(6) = d.alar_valor

            dr(7) = d.alar_id
            dr(8) = d.veh_conductor
            dr(9) = d.alar_limite
            If (d.mail_enviado IsNot Nothing) Then
                dr(10) = d.mail_enviado.Value.ToString("dd/MM/yyyy hh:mm:ss")
            Else
                dr(10) = "No enviado"
            End If

            If (d.sms_enviado IsNot Nothing) Then
                dr(11) = d.sms_enviado.Value.ToString("dd/MM/yyyy hh:mm:ss")
            Else
                dr(11) = "No enviado"
            End If

            dr(12) = d.alar_lat & "," & d.alar_lng & "," & d.alar_tipo.Value.ToString & "," + d.alar_codigo_config.Value.ToString
            'duracion
            If d.alar_duracion <> 0 Then
                 Dim hor As Integer
                Dim min As Integer
                Dim seg As Integer

                'si la duracion es mas de 24hs voy a mostrar +24hs
                If d.alar_duracion > 86400 Then
                    dr(13) = "+24 hs"
                Else
                    hor = Math.Floor(d.alar_duracion.Value / 3600)
                    min = Math.Floor((d.alar_duracion.Value - hor * 3600) / 60)
                    seg = d.alar_duracion - (hor * 3600 + min * 60)

                    If hor > 0 Then
                        dr(13) = Trim(hor) + " h " + Trim(min) + " m " + Trim(seg) + " s"
                    Else
                        dr(13) = Trim(min) + " m " + Trim(seg) + " s"
                    End If
                End If
               
            Else

                dr(13) = ""
            End If


                dt.Rows.Add(dr)
        Next

        Session.Add("AlarmasList", dt)
        Session.Timeout = 1

        Return dt

    End Function

    'le asigno al link de mostrar mapa las coordenadas donde tiene que poner el punto
    Protected Sub datagridAlertas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If
        Dim rowview As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Dim alarma As Alarmas = clsAlarma.SelectAlarmaById(CInt(rowview(7).ToString()))
        ' Dim linkMapa As ImageButton = DirectCast(e.Row.FindControl("imagebuttonMapa"), ImageButton)

        ' linkMapa.OnClientClick = "alert('aca mostramos el mapa);" '"viewMapa('-35', '-64');"


    End Sub

    'Protected Function GetAlarmasCliente(ByVal cat_id As Integer, ByVal veh_id As Integer) As DataTable
    '    hdncat_id.Value = cat_id
    '    hdnveh_id.Value = veh_id

    '    Dim dt As DataTable = New DataTable()

    '    Dim fechaDesde As Nullable(Of Date)
    '    Dim fechaHasta As Nullable(Of Date)

    '    If txtfechaDesde.Text <> "" Then fechaDesde = Date.Parse(txtfechaDesde.Text)
    '    If txtfechaHasta.Text <> "" Then fechaHasta = Date.Parse(txtfechaHasta.Text)

    '    Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertasCliente(CInt(hdncli_id.Value), cat_id, veh_id, fechaDesde, fechaHasta)

    '    dt.Columns.Add("Alarma")
    '    dt.Columns.Add("Categoria")
    '    dt.Columns.Add("Patente")
    '    dt.Columns.Add("Fecha", GetType(Date))
    '    dt.Columns.Add("Hora", GetType(TimeSpan))
    '    dt.Columns.Add("Ubicacion")
    '    dt.Columns.Add("Valor")
    '    dt.Columns.Add("alar_id", GetType(Integer))

    '    For Each d As Alarmas In alarmas

    '        Dim dr As DataRow = dt.NewRow()
    '        dr(0) = d.alar_nombre
    '        dr(1) = d.alar_Categoria

    '        dr(2) = d.veh_patente
    '        dr(3) = d.alar_fecha
    '        dr(4) = d.alar_hora
    '        dr(5) = d.alar_nombre_via + "," + d.alar_Localidad + "," + d.alar_Provincia
    '        dr(6) = d.alar_valor

    '        dr(7) = d.alar_id

    '        dt.Rows.Add(dr)
    '    Next

    '    Return dt

    'End Function
  

    Protected Sub btBuscar_Click(sender As Object, e As EventArgs) Handles btBuscar.Click

        '  pongo el filtro en session para poder filtrar la grilla despues
        If txtfechaDesde.Text <> "" Then
            fechaDesde = DateTime.Parse(txtfechaDesde.Text + " 00:00:00")
        End If

        If txtfechaHasta.Text <> "" Then
            fechaHasta = DateTime.Parse(txtfechaHasta.Text + " 23:59:00")
        End If

        Session.Add("Filtro", fechaDesde.ToString() + "|" + fechaHasta.ToString())

        ' cargo al grilla filtrando por las fecha ingresadas
        JQGridAlarmas.DataSource = GetAlarmas(hdnveh_id.Value)
        JQGridAlarmas.DataBind()
    End Sub

    Protected Sub linkOcultar_Click(sender As Object, e As EventArgs) Handles linkOcultar.Click
        'oculto todas las alarmas que esta viendo
        For Each row As DataRow In GetAlarmas(hdnveh_id.Value).Rows
            clsAlarma.ocultarAlarma(row("alar_id"))
        Next

        JQGridAlarmas.DataSource = GetAlarmas(hdnveh_id.Value)
        JQGridAlarmas.DataBind()
    End Sub

    'Exceso
    Public Sub JQGridAlarmasExceso_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasExcesoList") IsNot Nothing Then
            JQGridAlarmasExceso.DataSource = DirectCast(Session("AlarmasExcesoList"), DataTable)
            JQGridAlarmasExceso.DataBind()
        End If

    End Sub

    'Inactividad
    Public Sub JQGridAlarmasInactividad_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasInactividadList") IsNot Nothing Then
            JQGridAlarmasInactividad.DataSource = DirectCast(Session("AlarmasInactividadList"), DataTable)
            JQGridAlarmasInactividad.DataBind()
        End If

    End Sub

    'Inicio Actividad
    Public Sub JQGridAlarmasInicio_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasInicioList") IsNot Nothing Then
            JQGridAlarmasInicio.DataSource = DirectCast(Session("AlarmasInicioList"), DataTable)
            JQGridAlarmasInicio.DataBind()
        End If

    End Sub

    'Horario
    Public Sub JQGridAlarmasHorario_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasHorarioList") IsNot Nothing Then
            JQGridFueraHorario.DataSource = DirectCast(Session("AlarmasHorarioList"), DataTable)
            JQGridFueraHorario.DataBind()
        End If

    End Sub

    'ZONAS
    Public Sub JQGridAlarmasZona_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasZonaList") IsNot Nothing Then
            JQGridAlarmasZona.DataSource = DirectCast(Session("AlarmasZonaList"), DataTable)
            JQGridAlarmasZona.DataBind()
        End If

    End Sub

    Protected Sub JQGridAlarmasZona_CellBinding(sender As Object, e As Trirand.Web.UI.WebControls.JQGridCellBindEventArgs)
        If e.RowValues(7).ToString() = "Si" Then
            If e.ColumnIndex = 8 Then
                Dim valores As String() = e.RowValues(6).ToString().Split("|")
                e.CellHtml = "<a title='Desactivar Alarma' onclick='activaDesacAlertaZ(" + valores(1) + "," + e.RowValues(0).ToString() + ",0);' href='#' >Desactivar</a>"

            End If
        Else
            If e.ColumnIndex = 8 Then
                Dim valores As String() = e.RowValues(6).ToString().Split("|")
                e.CellHtml = "<a title='Activar Alarma' onclick='activaDesacAlertaZ(" + valores(1) + "," + e.RowValues(0).ToString() + ",1);' href='#' >Activar</a>"

            End If
        End If


    End Sub


    'DIRECCIONES
    Public Sub JQGridAlarmasDirecc_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasDireccList") IsNot Nothing Then
            JQGridDireccion.DataSource = DirectCast(Session("AlarmasDireccList"), DataTable)
            JQGridDireccion.DataBind()
        End If

    End Sub

    'Recordatorio Fechas

    Public Sub JQGridAlarmasRecFecha_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasRecordatorioFechaList") IsNot Nothing Then
            JQGridRecordatorioFecha.DataSource = DirectCast(Session("AlarmasRecordatorioFechaList"), DataTable)
            JQGridRecordatorioFecha.DataBind()
        End If

    End Sub
    'Recordatorio Kms
    Public Sub JQGridAlarmasRecKm_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasRecordatorioKmList") IsNot Nothing Then
            JQGridRecordatorioKm.DataSource = DirectCast(Session("AlarmasRecordatorioKmList"), DataTable)
            JQGridRecordatorioKm.DataBind()
        End If

    End Sub
    'RECORRIDOS
    Public Sub JQGridAlarmasRecorrido_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        If Session("AlarmasRecorridoList") IsNot Nothing Then
            JQGridAlarmaRecorrido.DataSource = DirectCast(Session("AlarmasRecorridoList"), DataTable)
            JQGridAlarmaRecorrido.DataBind()
        End If

    End Sub

    Protected Sub JQGridAlarmasRecorrido_CellBinding(sender As Object, e As Trirand.Web.UI.WebControls.JQGridCellBindEventArgs)
        If e.RowValues(8).ToString() = "Si" Then
            If e.ColumnIndex = 10 Then
                Dim valores As String() = e.RowValues(7).ToString().Split("|")
                e.CellHtml = "<a title='Desactivar Alarma' onclick='activaDesacAlertaR(" + valores(1) + "," + e.RowValues(0).ToString() + ",0);' href='#' >Desactivar</a>"

            End If
        Else
            If e.ColumnIndex = 10 Then
                Dim valores As String() = e.RowValues(7).ToString().Split("|")
                e.CellHtml = "<a title='Activar Alarma' onclick='activaDesacAlertaR(" + valores(1) + "," + e.RowValues(0).ToString() + ",1);' href='#' >Activar</a>"

            End If
        End If


    End Sub

    Protected Sub JQGridAlarmasDirecc_CellBinding(sender As Object, e As Trirand.Web.UI.WebControls.JQGridCellBindEventArgs)
        If e.RowValues(7).ToString() = "Si" Then
            If e.ColumnIndex = 7 Then
                Dim valores As String() = e.RowValues(6).ToString().Split("|")
                e.CellHtml = "<a title='Desactivar Alarma' onclick='activaDesacAlertaD(" + valores(1) + "," + valores(0) + ",0);' href='#' >Desactivar</a>"

            End If
        Else
            If e.ColumnIndex = 7 Then
                Dim valores As String() = e.RowValues(6).ToString().Split("|")
                e.CellHtml = "<a title='Activar Alarma' onclick='activaDesacAlertaD(" + valores(1) + "," + valores(0).ToString() + ",1);' href='#' >Activar</a>"

            End If
        End If


    End Sub



    Private Sub CargarGrilla()
        GridViewAlarmas.DataSource = clsVehiculo.ListActivos(CInt(hdncli_id.Value))
        GridViewAlarmas.DataBind()
    End Sub

    Private Sub EliminarAlarma(ByVal alar_id As String)
        Try
            Dim valores As String() = alar_id.Split("|")
            Select Case valores(2)
                Case "zona"
                    clsAlarma.DeleteAlertas_Zona_Vehiculos(valores(0), valores(1))
                Case "dir"
                    clsAlarma.DeleteAlertas_Direcciones_Vehiculos(valores(0), valores(1))
                Case "rec"
                    'verifico si recibo accion para activar o desactivar en lugar de borrar
                    clsAlarma.DeleteAlertas_Recorrido_Vehiculos(valores(0), valores(1))
                Case "recf"
                    clsAlarma.DeleteAlertas_Recordatorio_Fecha(valores(0))
                Case "reck"
                    clsAlarma.DeleteAlertas_Recordatorio_Km(valores(0))
                Case "horario"
                    clsAlarma.DeleteAlertas_Fuera_Horario(valores(0))
                Case "actividad"
                    clsAlarma.DeleteAlertas_InicioActividad(valores(0))
                Case "inicio"
                    clsAlarma.DeleteAlertas_InicioActividad(valores(0))
                Case "inactivo"
                    clsAlarma.DeleteAlertas_Inactividad(valores(0))
                Case "Excesos"
                    clsAlarma.DeleteAlertas_ExcesoKms(valores(0))
            End Select


        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - Eliminar alarmas de Zona, direccion o recorrido " + ex.Message + " - " + ex.StackTrace)
        End Try
        Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)

    End Sub

    'a medida que lleno la grilla verifico si tengo configurada la alarma para el movil y pongo el check de yes o no
    Protected Sub GridViewAlarmas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If
        Dim movil As Vehiculo = DirectCast(e.Row.DataItem, Vehiculo)
        'verifico si tiene configurada las alertas y muestro la imagen de check verde o rojo
        Dim _alertas As List(Of Alertas_Velocidad_Configuradas) = clsAlarma.ListByMovil(movil.veh_id)
        DirectCast(e.Row.FindControl("lblVelocidad"), Label).Text = _alertas.Count.ToString()
        'verifico si configuro alarmas de desvio de recorridos y direcciones y zonas
        Dim _recorridos As List(Of Alertas_Recorridos) = clsAlarma.AlertaRecorridoByMovil(movil.veh_id)
        If _recorridos.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantRecorrido"), Label).Text = _recorridos.Count().ToString()
        End If

        Dim _zonas As List(Of Alertas_Zonas) = clsAlarma.AlertaZonaByMovil(movil.veh_id)
        If _zonas.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantZona"), Label).Text = _zonas.Count().ToString()
        End If

        Dim _direcciones As List(Of Alertas_Direcciones) = clsAlarma.AlertaDireccionByMovil(movil.veh_id)
        If _direcciones.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantDireccion"), Label).Text = _direcciones.Count().ToString()
        End If

        Dim _recordatorioFecha As List(Of Alertas_Recordatorios_Por_Fechas) = clsAlarma.AlertaRecordatorioFechaByMovil(movil.veh_id)
        Dim _recordatorioKm As List(Of Alertas_Recordatorios_Por_Km) = clsAlarma.AlertaRecordatorioKmByMovil(movil.veh_id)

        If _recordatorioFecha.Count() > 0 Or _recordatorioKm.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantRecordatorio"), Label).Text = (_recordatorioFecha.Count() + _recordatorioKm.Count()).ToString()
        End If

        'alarmas de sensores
        Dim _sensores As List(Of Sensores_Configurados) = clsAlarma.AlertaSensoresByMovil(movil.veh_id)

        If _sensores.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblSensor"), Label).Text = _sensores.Count().ToString()
        End If

        'fuera horario

        Dim _fueraHora As List(Of Alarmas_Fuera_Horario) = clsAlarma.AlertaFueraHoraByMovil(movil.veh_id)

        If _fueraHora.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantFueraHora"), Label).Text = _fueraHora.Count().ToString()
        End If

        'inicio Actividad

        Dim _inici As List(Of Alarma_Inicio_Actividad) = clsAlarma.AlertaInicioActividadByMovil(movil.veh_id)

        If _inici.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantInicio"), Label).Text = _inici.Count().ToString()
        End If

        'Inactividad

        Dim _inactivo As List(Of Alarmas_Inactividad) = clsAlarma.AlertaInactividadByMovil(movil.veh_id)

        If _inactivo.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantInactivo"), Label).Text = _inactivo.Count().ToString()
        End If

        'kms excedidos
        Dim _kms As List(Of Alamas_Kms_Excedidos) = clsAlarma.AlertaExcesoKmsByMovil(movil.veh_id)

        If _kms.Count() > 0 Then
            DirectCast(e.Row.FindControl("lblCantKms"), Label).Text = _kms.Count().ToString()
        End If

    End Sub

    <WebMethod()> _
    Public Function filtrarAlarma1(ByVal alar_id As String, ByVal alar2 As String) As Boolean
        Try
            ' If desde <> "" Then
            ' Dim fechaDesde As DateTime = DateTime.Parse(desde + " 00:00:00")
            ' End If

            ' If hasta <> "" Then
            ' fechaHasta = DateTime.Parse(fechaH + " 23:59:00")
            ' End If


            'cargo al grilla filtrando por las fecha ingresadas
            '  JQGridAlarmas.DataSource = GetAlarmas(hdnveh_id.Value, fechaDesde, fechaHasta)
            ' JQGridAlarmas.DataBind()
        Catch ex As Exception

        End Try


        Return True
    End Function
End Class
'mapa en pop up
'http://www.femgeek.co.uk/popup-google-map-using-fancybox/