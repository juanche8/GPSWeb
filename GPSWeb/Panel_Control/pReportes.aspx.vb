'Pagina para generar los reprotes del sistema
Imports GPS.Business
Imports GPS.Data
Imports System.IO

Partial Public Class pReportes1
    Inherits System.Web.UI.Page
    Dim totalKm As Decimal = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    Dim cli_id As Integer = Session("Cliente").ToString()
                    hdncli_id.Value = cli_id.ToString()

                    'verifico si viene de recorridos del mapa
                    If Request.Params("recorridos") IsNot Nothing Then

                        PanelRutinaCompleto.Visible = True
                        PanelRutinaKmsRecorridos.Visible = False
                        PanelRutinaAlarmas.Visible = False

                        Dim hoy As String = "false"

                        If Request.Params("hoy") IsNot Nothing Then hoy = Request.Params("hoy").ToString()

                        hdnFechaDesde.Value = Request.Params("fechaDesde").ToString()
                        'si el dia hasta es menor que la fecha actual pongo la hora en 23:59:59
                      
                            hdnFechaHasta.Value = Request.Params("fechaHasta").ToString()


                        hdnmoviles.Value = Request.Params("moviles").ToString()
                        hdnCampos.Value = ""

                        If hdnFechaDesde.Value = "" Or hdnFechaDesde.Value = " 00:00" Then
                            lblFechas.Text = " (" & String.Format("{0:dd/MM/yyyy 00:00}", DateTime.Now) & " - "
                            hdnFechaDesde.Value = String.Format("{0:yyyyMMdd}", DateTime.Now) & " 00:00"
                        Else
                            If hoy = "true" Then
                                lblFechas.Text = " (" & String.Format("{0:dd/MM/yyyy}", DateTime.Now) & Request.Params("fechaDesde").ToString() & " - "
                                hdnFechaDesde.Value = String.Format("{0:yyyyMMdd}", DateTime.Now) & Request.Params("fechaDesde").ToString()
                            Else
                                hdnFechaDesde.Value = String.Format("{0:yyyyMMdd HH:mm:ss}", CDate(Request.Params("fechaDesde").ToString()))
                                lblFechas.Text = " (" & String.Format("{0:dd/MM/yyyy HH:mm:ss}", CDate(Request.Params("fechaDesde").ToString())) & " - "
                            End If
                            
                        End If

                        If hdnFechaHasta.Value = "" Or hdnFechaHasta.Value = " 23:59:59" Or hdnFechaHasta.Value = " 23:59" Then
                            lblFechas.Text = lblFechas.Text & String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now) & ")"
                            hdnFechaHasta.Value = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now)
                        Else
                            If hoy = "true" Then
                                hdnFechaHasta.Value = String.Format("{0:yyyyMMdd}", DateTime.Now) & Request.Params("fechaHasta").ToString() & ":59"
                                lblFechas.Text = lblFechas.Text & String.Format("{0:dd/MM/yyyy}", DateTime.Now) & Request.Params("fechaHasta").ToString() & ")"
                            Else
                                If CDate(Request.Params("fechaHasta").ToString()) > DateTime.Now And Request.Params("origen") Is Nothing Then
                                    hdnFechaHasta.Value = CDate(Request.Params("fechaHasta").ToString()).ToString("yyyyMMdd") & " 23:59:59"
                                    lblFechas.Text = lblFechas.Text & String.Format("{0:dd/MM/yyyy 23:59}", CDate(Request.Params("fechaHasta").ToString())) & ")"
                                Else
                                    hdnFechaHasta.Value = CDate(Request.Params("fechaHasta").ToString()).ToString("yyyyMMdd HH:mm:59")
                                    lblFechas.Text = lblFechas.Text & String.Format("{0:dd/MM/yyyy HH:mm:ss}", CDate(Request.Params("fechaHasta").ToString())) & ")"
                                End If
                            End If
                          
                        End If


                        gridRutinaCompleto.DataSource = GetRecorridos(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value, 1, 30, hdnOrden.Value)
                        gridRutinaCompleto.DataBind()

                        'vengo del mapa desabilito la opcion de recorrido personalizado
                        LiteralDisabled.Text = "<script type='text/javascript'> $(function () {$('#tabs').tabs('disable', 1);});</script>"

                    Else
                        ' LiteralDisabled.Text = "<script type='text/javascript'> $(function () { $('#tabs').tabs('disable' , 1);});</script>"
                    End If

                    'verifico si quiere ver solo reportes de un vehiculo
                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()
                        PanelCustomizado.Visible = False
                        PanelMoviles.Visible = False
                        PanelPatente.Visible = True
                        lblPatente.Text = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value)).veh_patente

                        btnReporteRutinaCompleto_Click(sender, e)
                    Else
                        'busco los vehiculos del usuario
                        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)

                        DataListVehiculos1.DataSource = vehiculos
                        DataListVehiculos1.DataBind()

                        DataListVehiculos.DataSource = vehiculos
                        DataListVehiculos.DataBind()

                    End If


                End If
            Else
                'no esta logeado
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "redirigir", " parent.iraLogin();", True)
            End If



        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador. " & ex.ToString()
        End Try
    End Sub

  


    Protected Sub btnReporteRutinaCompleto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReporteRutinaCompleto.Click
        Try
            'reporte diario solo para el dia de hoy- completo
            Dim strMoviles As String = ""
            '  Dim horaDesde As String = "00:00:00"
            '  Dim horaHasta As String = "23:59:59"

            Dim FechaDesde As String = DateTime.Now.ToString("yyyyMMdd") & " 00:00:00"
            Dim FechaHasta As String = DateTime.Now.ToString("yyyyMMdd HH:mm:ss")

            If hdnFechaDesde.Value <> "" Then FechaDesde = hdnFechaDesde.Value
            If hdnFechaHasta.Value <> "" Then FechaHasta = hdnFechaHasta.Value

            '  If rdnFrecuencia.SelectedValue = 2 Then
            'asigno al rango de fechas una semana
            'FechaDesde = DateTime.Now.AddDays(-7).ToString("yyyyMMdd")
            ' End If

            'verifico si eligio algun vehiculo, sino tengo que marcar todos

            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                For Each row As DataListItem In DataListVehiculos1.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                        strMoviles = strMoviles + DataListVehiculos1.DataKeys(row.ItemIndex).ToString()
                    End If

                Next

            End If


            If strMoviles = "" Then
                lblError.Text = "Seleccione el o los Moviles para ver el Reporte."
            Else
                'consulto los datos para los moviles elegidos y voy armando la salida
                PanelRutinaCompleto.Visible = True
                PanelRutinaKmsRecorridos.Visible = False
                PanelRutinaAlarmas.Visible = False

                hdnFechaDesde.Value = FechaDesde
                hdnFechaHasta.Value = FechaHasta
                hdnmoviles.Value = strMoviles
                hdnCampos.Value = ""



                gridRutinaCompleto.DataSource = GetRecorridos(FechaDesde, FechaHasta, strMoviles, 1, 30, hdnOrden.Value)
                gridRutinaCompleto.DataBind()



            End If

            hdnTab.Value = "#tabs-1"
        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el Reporte de Rutina. Contacte al Administrador. " + ex.ToString()
        End Try
    End Sub

    

    Protected Function GetRecorridos(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal moviles As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal order As String) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim recordCount As Integer = 0
        Dim _moviles As String() = Split(moviles, "|")

        dt.Columns.Add("patente")
        dt.Columns.Add("fecha")
        dt.Columns.Add("hora")
        dt.Columns.Add("nombre_via")
        dt.Columns.Add("localidad")
        dt.Columns.Add("provincia")
        dt.Columns.Add("velocidad", GetType(Decimal))
        dt.Columns.Add("total_kilometros")
        dt.Columns.Add("cant_alarmas")
        dt.Columns.Add("encendido")
        dt.Columns.Add("ocupado")
        dt.Columns.Add("RPM", GetType(Integer))
        dt.Columns.Add("Temp", GetType(Decimal))
        dt.Columns.Add("BAT", GetType(Decimal))
        dt.Columns.Add("EVENTO")

        For i = 0 To _moviles.Length - 1
            If _moviles(i) <> "" Then

                recordCount = recordCount + clsReporte.RecorridosRutinaTotal(fecha_desde, fecha_hasta, _moviles(i))
                Dim _recorridos As List(Of vMonitoreo) = clsReporte.RecorridosRutina(fecha_desde, fecha_hasta, _moviles(i), pageIndex, pageSize, order)
                Dim patente As String = clsVehiculo.Seleccionar(CInt(_moviles(i))).veh_patente

                'tengo que ponerle cero a los kms recorridos del primer registro del dia
                'como estoy paginando verifico si la cantidad de registros es menor al tamaño de la pagina

                If recordCount > 0 Then
                    If recordCount <= pageSize Then _recorridos(_recorridos.Count - 1).KMS_RECORRIDOS = 0

                    '  Si la cantidad de registros es menor o igual a la cantidad de paginas * la pagina actual
                    If recordCount <= (pageSize * pageIndex) Then _recorridos(_recorridos.Count - 1).KMS_RECORRIDOS = 0
                End If




                For Each d As vMonitoreo In _recorridos

                    Dim dr As DataRow = dt.NewRow()
                    dr(0) = patente
                    dr(1) = d.FECHA.ToString("dd/MM/yyyy")

                    dr(2) = d.FECHA.ToString("HH:mm:ss")
                    dr(3) = d.NOMBRE_VIA
                    dr(4) = d.LOCALIDAD
                    dr(5) = d.PROVINCIA
                    dr(6) = d.VELOCIDAD

                    dr(7) = String.Format("{0:###0.000}", d.KMS_RECORRIDOS)
                    dr(8) = clsReporte.CantidadAlarmas(d.FECHA, _moviles(i))

                    'verifico si el movil tiene el sensor sino lo muestro vacio
                    If clsAlarma.GetSensoresConfigurados(4, _moviles(i)).Count > 0 Then  'encendido
                        dr(9) = d.ENCENDIDO
                    Else
                        dr(9) = ""
                    End If

                    If clsAlarma.GetSensoresConfigurados(7, _moviles(i)).Count > 0 Then  'ocupado
                        dr(10) = d.OCUPADO
                    Else
                        dr(10) = ""
                    End If

                    If clsAlarma.GetSensoresConfigurados(18, _moviles(i)).Count > 0 Then  'RPM
                        dr(11) = d.RPM
                    Else
                        dr(11) = 0
                    End If

                    If clsAlarma.GetSensoresConfigurados(17, _moviles(i)).Count > 0 Then  'Temperatura
                        dr(12) = d.TEMP
                    Else
                        dr(12) = 0
                    End If

                    dr(13) = d.BATERIA

                    If d.CODIGO_EVENTO IsNot Nothing Then
                        Dim _evento As Evento = clsEventoTrama.SelectByCodigo(d.CODIGO_EVENTO)
                        If _evento IsNot Nothing Then
                            dr(14) = _evento.eve_nombre
                        End If
                    End If

                    dt.Rows.Add(dr)
                Next
            End If
        Next
        Me.PopulatePager(recordCount, pageIndex)

        Return dt




    End Function

    Protected Function GetRecorridosExcel(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal moviles As String) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim recordCount As Integer = 0
        Dim _moviles As String() = Split(moviles, "|")


        dt.Columns.Add("patente")
        dt.Columns.Add("fecha")
        dt.Columns.Add("hora")
        dt.Columns.Add("nombre_via")
        dt.Columns.Add("localidad")
        dt.Columns.Add("provincia")
        dt.Columns.Add("velocidad", GetType(Decimal))
        dt.Columns.Add("total_kilometros")
        dt.Columns.Add("cant_alarmas")
        dt.Columns.Add("encendido")
        dt.Columns.Add("ocupado")
        dt.Columns.Add("RPM", GetType(Integer))
        dt.Columns.Add("Temp", GetType(Decimal))
        dt.Columns.Add("Bateria", GetType(Decimal))
        dt.Columns.Add("Evento")

        For i = 0 To _moviles.Length - 1
            If _moviles(i) <> "" Then

                recordCount = recordCount + clsReporte.RecorridosRutinaTotal(fecha_desde, fecha_hasta, _moviles(i))
                Dim PageCount As Integer = 1

                If recordCount > 500 Then
                    PageCount = recordCount / 500
                End If
                'si el resto de la division no es cero le sumo uno mas para traer todos
                Dim resto As Double = recordCount Mod 500   ' Returns 0.

                If resto > 0 Then PageCount = PageCount + 1
                'voy a buscar los recorridos por pagina y los junto

                Dim j As Integer = 1
                Do While (j <= PageCount)
                    Dim _recorridos As List(Of vMonitoreo) = clsReporte.RecorridosRutina(fecha_desde, fecha_hasta, _moviles(i), j, 500, hdnOrden.Value)
                    Dim patente As String = clsVehiculo.Seleccionar(CInt(_moviles(i))).veh_patente
                    For Each d As vMonitoreo In _recorridos

                        Dim dr As DataRow = dt.NewRow()
                        dr(0) = patente
                        dr(1) = d.FECHA.ToString("dd/MM/yyyy")

                        dr(2) = d.FECHA.ToString("HH:mm:ss")
                        dr(3) = d.NOMBRE_VIA
                        dr(4) = d.LOCALIDAD
                        dr(5) = d.PROVINCIA
                        dr(6) = d.VELOCIDAD

                        dr(7) = String.Format("{0:###0.000}", d.KMS_RECORRIDOS)
                        dr(8) = clsReporte.CantidadAlarmas(d.FECHA, _moviles(i))

                        'verifico si el movil tiene el sensor sino lo muestro vacio
                        If clsAlarma.GetSensoresConfigurados(4, _moviles(i)).Count > 0 Then  'encendido
                            dr(9) = d.ENCENDIDO
                        Else
                            dr(9) = ""
                        End If

                        If clsAlarma.GetSensoresConfigurados(7, _moviles(i)).Count > 0 Then  'ocupado
                            dr(10) = d.OCUPADO
                        Else
                            dr(10) = ""
                        End If

                        If clsAlarma.GetSensoresConfigurados(18, _moviles(i)).Count > 0 Then  'RPM
                            dr(11) = d.RPM
                        Else
                            dr(11) = 0
                        End If

                        If clsAlarma.GetSensoresConfigurados(17, _moviles(i)).Count > 0 Then  'Temperatura
                            dr(12) = d.TEMP
                        Else
                            dr(12) = 0
                        End If


                        dr(13) = d.BATERIA

                        If d.CODIGO_EVENTO IsNot Nothing Then
                            Dim _evento As Evento = clsEventoTrama.SelectByCodigo(d.CODIGO_EVENTO)
                            If _evento IsNot Nothing Then
                                dr(14) = _evento.eve_nombre
                            End If
                        End If
                       

                        dt.Rows.Add(dr)
                    Next
                    j = j + 1
                Loop

            End If
        Next


        'muestro columnas seleccionadas
        '  If hdnCampos.Value <> "" Then
        'Dim _campos As String() = Split(hdnCampos.Value, ",")

        'For i = 0 To _campos.Length - 1
        'If _campos(i) <> "" Then dt.Columns(CInt(_campos(i))).ColumnMapping = MappingType.Hidden
        ' Next
        ' End If


        Return dt




    End Function


    Private Sub PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer)
        Dim dblPageCount As Double = CType((CType(recordCount, Decimal) / 30), Double)
        Dim pageCount As Integer = CType(Math.Ceiling(dblPageCount), Integer)
        Dim pages As New List(Of ListItem)
        If (pageCount > 0) Then
            pages.Add(New ListItem("<<", "1", (currentPage > 1)))
            Dim i As Integer = 1
            Do While (i <= pageCount)
                pages.Add(New ListItem(i.ToString, i.ToString, (i <> currentPage)))
                i = (i + 1)
            Loop
            pages.Add(New ListItem(">>", pageCount.ToString, (currentPage < pageCount)))
        End If
        rptPager.DataSource = pages
        rptPager.DataBind()
    End Sub



    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)

        hdnPagina.Value = pageIndex.ToString
        gridRutinaCompleto.DataSource = GetRecorridos(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value, pageIndex, 30, hdnOrden.Value)
        gridRutinaCompleto.DataBind()

        If hdnCampos.Value <> "" Then
            Dim _campos As String() = Split(hdnCampos.Value, ",")

            'primero oculto todo
            gridRutinaCompleto.Columns(0).Visible = False
            gridRutinaCompleto.Columns(1).Visible = False
            gridRutinaCompleto.Columns(2).Visible = False
            gridRutinaCompleto.Columns(3).Visible = False
            gridRutinaCompleto.Columns(4).Visible = False
            gridRutinaCompleto.Columns(5).Visible = False
            gridRutinaCompleto.Columns(6).Visible = False
            gridRutinaCompleto.Columns(7).Visible = False
            gridRutinaCompleto.Columns(8).Visible = False
            gridRutinaCompleto.Columns(9).Visible = False
            gridRutinaCompleto.Columns(10).Visible = False
            gridRutinaCompleto.Columns(11).Visible = False
            gridRutinaCompleto.Columns(12).Visible = False
            gridRutinaCompleto.Columns(13).Visible = False
            'despues muestro
            For i = 0 To _campos.Length - 1
                If _campos(i) <> "" Then gridRutinaCompleto.Columns(_campos(i)).Visible = True
            Next
        End If

    End Sub


    Protected Function GetAlarmas(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal moviles As String) As DataTable
        Dim dt As DataTable = New DataTable()

        Dim _moviles As String() = Split(moviles, "|")

        dt.Columns.Add("patente")
        dt.Columns.Add("fecha")
        dt.Columns.Add("hora")
        dt.Columns.Add("categoria")
        dt.Columns.Add("alarma")
        dt.Columns.Add("valor")
        dt.Columns.Add("direccion")
        dt.Columns.Add("localidad")
        dt.Columns.Add("provincia")
        dt.Columns.Add("duracion")

        For i = 0 To _moviles.Length - 1
            Dim _alarmas As List(Of Alarmas) = clsReporte.AlarmasRutina(fecha_desde, fecha_hasta, _moviles(i))
            Dim patente As String = clsVehiculo.Seleccionar(CInt(_moviles(i))).veh_patente
            For Each d As Alarmas In _alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = patente
                dr(1) = d.alar_fecha.ToString("dd/MM/yyyy")

                dr(2) = d.alar_fecha.ToString("HH:mm:ss")
                dr(3) = d.alar_Categoria
                dr(4) = d.alar_nombre
                dr(5) = d.alar_valor
                dr(6) = d.alar_nombre_via

                dr(7) = d.alar_Localidad
                dr(8) = d.alar_Provincia

                'duracion
                If d.alar_duracion <> 0 Then
                    Dim hor As Integer
                    Dim min As Integer
                    Dim seg As Integer

                    'si la duracion es mas de 24hs voy a mostrar +24hs
                    If d.alar_duracion > 86400 Then
                        dr(9) = "+24 hs"
                    Else
                        hor = Math.Floor(d.alar_duracion.Value / 3600)
                        min = Math.Floor((d.alar_duracion.Value - hor * 3600) / 60)
                        seg = d.alar_duracion - (hor * 3600 + min * 60)

                        If hor > 0 Then
                            dr(9) = Trim(hor) + " h " + Trim(min) + " m " + Trim(seg) + " s"
                        Else
                            dr(9) = Trim(min) + " m " + Trim(seg) + " s"
                        End If
                    End If

                Else
                    dr(9) = ""
                End If

                dt.Rows.Add(dr)
            Next
        Next

        Return dt

    End Function

    Protected Function GetKmsRecorridos(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal moviles As String) As DataTable
        totalKm = 0
        Dim cant As Integer = 0
        Dim dt As DataTable = New DataTable()

        Dim _moviles As String() = Split(moviles, "|")

        dt.Columns.Add("patente")
        dt.Columns.Add("fecha")
        dt.Columns.Add("kms", Type.GetType("System.Decimal"))

        For i = 0 To _moviles.Length - 1

            Dim _recorridos As List(Of Kms) = clsReporte.KmsRecorridos(fecha_desde, fecha_hasta, _moviles(i))
            Dim patente As String = clsVehiculo.Seleccionar(CInt(_moviles(i))).veh_patente
            For j = 0 To _recorridos.Count - 1
                Dim dr As DataRow = dt.NewRow()
                dr(0) = patente
                dr(1) = CDate(_recorridos(j).FECHA).ToString("dd/MM/yyyy")

                dr(2) = String.Format("{0:###0.000}", _recorridos(j).KMS_RECORRIDOS)

                dt.Rows.Add(dr)
            Next
            
        Next


        Return dt

    End Function

    Protected Sub btnReporteRutinaKms_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReporteRutinaKms.Click
        Try
            'reporte diario solo para el dia de hoy- completo
             Dim strMoviles As String = ""
            '  Dim horaDesde As String = "00:00:00"
            '  Dim horaHasta As String = "23:59:59"
            '
            Dim FechaDesde As String = DateTime.Now.ToString("yyyyMMdd") & " 00:00:00"
            Dim FechaHasta As String = DateTime.Now.ToString("yyyyMMdd") & " 23:59:59"


            If hdnFechaDesde.Value <> "" Then FechaDesde = hdnFechaDesde.Value
            If hdnFechaHasta.Value <> "" Then FechaHasta = hdnFechaHasta.Value


            ' If rdnFrecuencia.SelectedValue = 2 Then
            'asigno al rango de fechas una semana
            'FechaDesde = DateTime.Now.AddDays(-7).ToString("yyyyMMdd")
            ' End If

            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                'verifico si eligio algun vehiculo, sino tengo que marcar todos

                For Each row As DataListItem In DataListVehiculos1.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                        strMoviles = strMoviles + DataListVehiculos1.DataKeys(row.ItemIndex).ToString()
                    End If

                Next
            End If


            If strMoviles = "" Then
                lblError.Text = "Seleccione el o los Moviles para ver el Reporte."
            Else
                PanelRutinaCompleto.Visible = False
                PanelRutinaKmsRecorridos.Visible = True
                PanelRutinaAlarmas.Visible = False

                hdnFechaDesde.Value = FechaDesde
                hdnFechaHasta.Value = FechaHasta
                hdnmoviles.Value = strMoviles


                GridKmsRecorridos.DataSource = GetKmsRecorridos(FechaDesde, FechaHasta, strMoviles)
                GridKmsRecorridos.DataBind()

            End If

            hdnTab.Value = "#tabs-1"
        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el Reporte de Kms Recorridos. Contacte al Administrador."
            hdnTab.Value = "#tabs-1"
        End Try
    End Sub

    Protected Sub btnReporteAlertas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReporteAlertas.Click
        Try
            'reporte diario solo para el dia de hoy- completo
            Dim strMoviles As String = ""
            'Dim horaDesde As String = "00:00:00"
            ' Dim horaHasta As String = "23:59:59"

            Dim FechaDesde As String = DateTime.Now.ToString("yyyyMMdd") & " 00:00:00"
            Dim FechaHasta As String = DateTime.Now.ToString("yyyyMMdd") & " 23:59:59"

            If hdnFechaDesde.Value <> "" Then FechaDesde = hdnFechaDesde.Value
            If hdnFechaHasta.Value <> "" Then FechaHasta = hdnFechaHasta.Value

            ' If rdnFrecuencia.SelectedValue = 2 Then
            'asigno al rango de fechas una semana
            'FechaDesde = DateTime.Now.AddDays(-7).ToString("yyyyMMd")

            ' End If

            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                'verifico si eligio algun vehiculo, sino tengo que marcar todos
                For Each row As DataListItem In DataListVehiculos1.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    If rdnMovil.Checked Then
                        If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                        strMoviles = strMoviles + DataListVehiculos1.DataKeys(row.ItemIndex).ToString()
                    End If

                Next

            End If

            If strMoviles = "" Then
                lblError.Text = "Seleccione el o los Moviles para ver el Reporte."
            Else
                'cargo el reporte de rutina de acuerdo a la opcion elegida

                PanelRutinaCompleto.Visible = False
                PanelRutinaKmsRecorridos.Visible = False
                PanelRutinaAlarmas.Visible = True

                hdnFechaDesde.Value = FechaDesde
                hdnFechaHasta.Value = FechaHasta
                hdnmoviles.Value = strMoviles


                GridAlarmasRutina.DataSource = GetAlarmas(FechaDesde, FechaHasta, strMoviles)
                GridAlarmasRutina.DataBind()
            End If

            hdnTab.Value = "#tabs-1"
        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el Reporte de Alertas. Contacte al Administrador."
        End Try
    End Sub

    Protected Sub KmsRecorridos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            totalKm += DataBinder.Eval(e.Row.DataItem, "kms")
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Text = "TOTAL KMS"
            e.Row.Cells(2).Text = totalKm.ToString()
        End If

    End Sub

  
  

    Protected Sub GridKmsRecorridos_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
       
        GridKmsRecorridos.PageIndex = e.NewPageIndex

        GridKmsRecorridos.DataSource = GetKmsRecorridos(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)
        GridKmsRecorridos.DataBind()
    End Sub

    Protected Sub gridRutinaCompleto_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gridRutinaCompleto.PageIndex = e.NewPageIndex


        ' gridRutinaCompleto.DataSource = GetRecorridos(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)
        ' gridRutinaCompleto.DataBind()
        '
    
        If hdnCampos.Value <> "" Then
            Dim _campos As String() = Split(hdnCampos.Value, ",")

            'primero oculto todo
            gridRutinaCompleto.Columns(0).Visible = False
            gridRutinaCompleto.Columns(1).Visible = False
            gridRutinaCompleto.Columns(2).Visible = False
            gridRutinaCompleto.Columns(3).Visible = False
            gridRutinaCompleto.Columns(4).Visible = False
            gridRutinaCompleto.Columns(5).Visible = False
            gridRutinaCompleto.Columns(6).Visible = False
            gridRutinaCompleto.Columns(7).Visible = False
            gridRutinaCompleto.Columns(8).Visible = False
            gridRutinaCompleto.Columns(9).Visible = False
            gridRutinaCompleto.Columns(10).Visible = False
            gridRutinaCompleto.Columns(11).Visible = False
            gridRutinaCompleto.Columns(12).Visible = False
            'despues muestro
            For i = 0 To _campos.Length - 1
                If _campos(i) <> "" Then gridRutinaCompleto.Columns(_campos(i)).Visible = True
            Next
        End If
    End Sub

    Protected Sub GridAlarmasRutina_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridAlarmasRutina.PageIndex = e.NewPageIndex

        GridAlarmasRutina.DataSource = GetAlarmas(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)
        GridAlarmasRutina.DataBind()
    End Sub

    Protected Sub LinkTildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkTildar.Click
        'marco todos los check de moviles

        For Each item As DataListItem In DataListVehiculos1.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = True
        Next
    End Sub

    Protected Sub LinkDestildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos1.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = False
        Next
    End Sub

    
  
    Protected Sub btRecExport_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btRecExport.Click
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim pagina As Page = New Page
        Dim grilla As New GridView
        grilla.EnableViewState = False
        grilla.AllowPaging = False
        grilla.DataSource = GetRecorridosExcel(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)
        grilla.DataBind()

    

        'grilla.Columns(0).Visible = False
        Dim form = New HtmlForm
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        pagina.Controls.Add(form)
        form.Controls.Add(grilla)
        pagina.RenderControl(htw)
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Rastreo_Urbano_Recorridos.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
    End Sub

    Protected Sub btKmsExport_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btKmsExport.Click
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim pagina As Page = New Page
        Dim grilla As New GridView
        grilla.EnableViewState = False
        grilla.AllowPaging = False
        grilla.DataSource = GetKmsRecorridos(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)
        grilla.DataBind()
        'grilla.Columns(0).Visible = False
        Dim form = New HtmlForm
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        pagina.Controls.Add(form)
        form.Controls.Add(grilla)
        pagina.RenderControl(htw)
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Rastreo Urbano Kms Recorridos.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
    End Sub

    Protected Sub btAlarmasExport_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btAlarmasExport.Click
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim pagina As Page = New Page
        Dim grilla As New GridView
        grilla.EnableViewState = False
        grilla.AllowPaging = False
        grilla.DataSource = GetAlarmas(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)
        grilla.DataBind()
        'grilla.Columns(0).Visible = False
        Dim form = New HtmlForm
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        pagina.Controls.Add(form)
        form.Controls.Add(grilla)
        pagina.RenderControl(htw)
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Rastreo Urbano Alarmas Reportadas.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
    End Sub

    

    Protected Sub Vehiculo_itemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataListVehiculos1.ItemDataBound

        Dim rdnMovil As CheckBox = DirectCast(e.Item.FindControl("rdnMovil"), CheckBox)

        If hdnmoviles.Value <> "" Then
            If hdnmoviles.Value.Contains(DataListVehiculos1.DataKeys(e.Item.ItemIndex).ToString()) Then
                rdnMovil.Checked = True
            End If
        End If
       

    End Sub

    Protected Sub btnListadoCustomAll_Click(sender As Object, e As EventArgs) Handles btnListadoCustomAll.Click
        Try
            'Reporte con campos dinamicos, voy a evaluar que campos mostrar
            Dim sSelect As String = ""
            Dim veh_id As String = "0"
            Dim sRowNumber = ""
            Dim sHoraDesde As String = "00:00:00"
            Dim sHoraHasta As String = "23:59:59"

            'fecha por default un mes
            Dim sFechaDesde As String = DateTime.Now.ToString("yyyyMMdd") + " " + sHoraDesde
            Dim sFechaHasta As String = DateTime.Now.AddDays(7).ToString("yyyyMMdd") + " " + sHoraHasta
            'campos a mostrar

            'verifico si ingreso fechas, sino ingreso se las pido
            'verifico rango de una semana
            If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                If (CDate(txtFechaHasta.Text) - CDate(txtFechaDesde.Text)).TotalDays > 7 Then
                    hdnTab.Value = "#tabs-2"
                    lblError.Text = "El rango de fechas no puede superar los 7 días."
                    Return
                End If


            End If
            'agrego este campo siempre porque tengo un grupo 
            If chkPatente.Checked Then
                sRowNumber = sRowNumber + "0"
            End If


            If chkFecha.Checked Then
                sRowNumber = sRowNumber + ",1"
            End If

            If chkHora.Checked Then
                sRowNumber = sRowNumber + ",2"
            End If

            If chkDireccion.Checked Then
                sRowNumber = sRowNumber + ",3"
            End If

            If chkLocalidad.Checked Then
                sRowNumber = sRowNumber + ",4"
            End If

            If chkProvincia.Checked Then
                sRowNumber = sRowNumber + ",5"
            End If

            If chkVelocidad.Checked Then
                sRowNumber = sRowNumber + ",6"
            End If

            If ChkKms.Checked Then
                sRowNumber = sRowNumber + ",7"
            End If

            If ChkAlertas.Checked Then
                sRowNumber = sRowNumber + ",8"
            End If

            If chkEncendido.Checked Then
                sRowNumber = sRowNumber + ",9"
            End If

            If chkOcupado.Checked Then
                sRowNumber = sRowNumber + ",10"
            End If

            If chkRPM.Checked Then
                sRowNumber = sRowNumber + ",11"
            End If

            If chkTemp.Checked Then
                sRowNumber = sRowNumber + ",12"
            End If

            If chkbateria.Checked Then
                sRowNumber = sRowNumber + ",13"
            End If
            If chkEvento.Checked Then
                sRowNumber = sRowNumber + ",14"
            End If

            If ddlhoraDesde.SelectedValue <> "" Then
                sHoraDesde = ddlhoraDesde.SelectedValue
            Else
                sHoraDesde = "00"
            End If

            If ddlMinDesde.SelectedValue <> "" Then
                sHoraDesde += ":" & ddlMinDesde.SelectedValue
            Else
                sHoraDesde += ":00"
            End If


            If ddlHoraHasta.SelectedValue <> "" Then
                sHoraHasta = ddlHoraHasta.SelectedValue
            Else
                sHoraHasta = "23"
            End If

            If ddlMinHasta.SelectedValue <> "" Then
                sHoraHasta += ":" & ddlMinHasta.SelectedValue
            Else
                sHoraHasta += ":59:59"
            End If

            gridRutinaCompleto.DataSource = Nothing
            gridRutinaCompleto.DataBind()
            'todos los campos visibles
            For i = 0 To 12
                gridRutinaCompleto.Columns(i).Visible = True
            Next


            'verifico si viene con el parametro de un solo vehiculo
            If hdnveh_id.Value <> "" Then

                veh_id = hdnveh_id.Value
                'verifico si elgio filtro de fecha para todos los moviles
                If txtFechaDesde.Text <> "" Then
                    sFechaDesde = CDate(txtFechaDesde.Text).ToString("yyyyMMdd") + " " + sHoraDesde
                End If

                If txtFechaHasta.Text <> "" Then
                    sFechaHasta = CDate(txtFechaHasta.Text).ToString("yyyyMMdd") + " " + sHoraHasta
                End If

                hdnFechaDesde.Value = sFechaDesde
                hdnFechaHasta.Value = sFechaHasta
                hdnmoviles.Value = veh_id
                hdnCampos.Value = sRowNumber


                PanelRutinaCompleto.Visible = True
                PanelRutinaKmsRecorridos.Visible = False
                PanelRutinaAlarmas.Visible = False
                'tengo que verificar que columnas mostrar si eligio solo algunos campos


                gridRutinaCompleto.DataSource = GetRecorridos(sFechaDesde, sFechaHasta, veh_id, 1, 30, hdnOrden.Value)
                gridRutinaCompleto.DataBind()


            Else
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As RadioButton = DirectCast(row.FindControl("rdnMovil"), RadioButton)
                    If rdnMovil.Checked Then
                        veh_id = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    End If
                Next


                If veh_id = "0" Then
                    lblError.Text = "Seleccione el Movil para ver el Reporte."
                Else
                    'verifico si elgio filtro de fecha para todos los moviles
                    If txtFechaDesde.Text <> "" Then
                        sFechaDesde = CDate(txtFechaDesde.Text).ToString("yyyyMMdd") + " " + sHoraDesde
                    End If


                    If txtFechaHasta.Text <> "" Then
                        sFechaHasta = CDate(txtFechaHasta.Text).ToString("yyyyMMdd") + " " + sHoraHasta
                    End If

                    hdnFechaDesde.Value = sFechaDesde
                    hdnFechaHasta.Value = sFechaHasta
                    hdnmoviles.Value = veh_id
                    hdnCampos.Value = sRowNumber


                    PanelRutinaCompleto.Visible = True
                    PanelRutinaKmsRecorridos.Visible = False
                    PanelRutinaAlarmas.Visible = False
                    'tengo que verificar que columnas mostrar si eligio solo algunos campos


                    gridRutinaCompleto.DataSource = GetRecorridos(sFechaDesde, sFechaHasta, veh_id, 1, 30, hdnOrden.Value)
                    gridRutinaCompleto.DataBind()

                    If hdnCampos.Value <> "" Then
                        Dim _campos As String() = Split(hdnCampos.Value, ",")

                        'primero oculto todo
                        gridRutinaCompleto.Columns(0).Visible = False
                        gridRutinaCompleto.Columns(1).Visible = False
                        gridRutinaCompleto.Columns(2).Visible = False
                        gridRutinaCompleto.Columns(3).Visible = False
                        gridRutinaCompleto.Columns(4).Visible = False
                        gridRutinaCompleto.Columns(5).Visible = False
                        gridRutinaCompleto.Columns(6).Visible = False
                        gridRutinaCompleto.Columns(7).Visible = False
                        gridRutinaCompleto.Columns(8).Visible = False
                        gridRutinaCompleto.Columns(9).Visible = False
                        gridRutinaCompleto.Columns(10).Visible = False
                        gridRutinaCompleto.Columns(11).Visible = False
                        gridRutinaCompleto.Columns(12).Visible = False
                        gridRutinaCompleto.Columns(13).Visible = False
                        gridRutinaCompleto.Columns(14).Visible = False

                        'despues muestro
                        For i = 0 To _campos.Length - 1
                            If _campos(i) <> "" Then gridRutinaCompleto.Columns(_campos(i)).Visible = True
                        Next
                    End If
                End If

            End If
            hdnTab.Value = "#tabs-2"

        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES CUSTOM ALL -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el reporte customizado. Contacte al Administrador."
            hdnTab.Value = "#tabs-2"
        End Try
    End Sub

    Protected Sub btnKmsCustom_Click(sender As Object, e As EventArgs) Handles btnKmsCustom.Click
        Try
            'Reporte con campos dinamicos, voy a evaluar que campos mostrar
            Dim sSelect As String = ""
            Dim veh_id As String = "0"
            Dim sRowNumber = ""
            Dim sHoraDesde As String = "00:00:00"
            Dim sHoraHasta As String = "23:59:59"

            'fecha por default un mes
            Dim sFechaDesde As String = DateTime.Now.ToString("yyyyMMdd") + " " + sHoraDesde
            Dim sFechaHasta As String = DateTime.Now.AddDays(7).ToString("yyyyMMdd") + " " + sHoraHasta
            'campos a mostrar

            'verifico si ingreso fechas, sino ingreso se las pido
            'verifico rango de una semana
            If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                If (CDate(txtFechaDesde.Text) - CDate(txtFechaHasta.Text)).TotalDays > 7 Then
                    hdnTab.Value = "#tabs-2"
                    lblError.Text = "El rango de fechas no puede superar los 7 días."
                    Return
                End If
            End If
          
            If ddlhoraDesde.SelectedValue <> "" Then
                sHoraDesde = ddlhoraDesde.SelectedValue
            Else
                sHoraDesde = "00"
            End If

            If ddlMinDesde.SelectedValue <> "" Then
                sHoraDesde += ":" & ddlMinDesde.SelectedValue
            Else
                sHoraDesde += ":00"
            End If


            If ddlHoraHasta.SelectedValue <> "" Then
                sHoraHasta = ddlHoraHasta.SelectedValue
            Else
                sHoraHasta = "23"
            End If

            If ddlMinHasta.SelectedValue <> "" Then
                sHoraHasta += ":" & ddlMinHasta.SelectedValue
            Else
                sHoraHasta += ":59:59"
            End If


            'verifico si viene con el parametro de un solo vehiculo
            If hdnveh_id.Value <> "" Then

                veh_id = hdnveh_id.Value

               
                'verifico si elgio filtro de fecha para todos los moviles
                If txtFechaDesde.Text <> "" Then
                    sFechaDesde = CDate(txtFechaDesde.Text).ToString("yyyyMMdd") + " " + sHoraDesde
                End If

                If txtFechaHasta.Text <> "" Then
                    sFechaHasta = CDate(txtFechaHasta.Text).ToString("yyyyMMdd") + " " + sHoraHasta
                End If

                hdnFechaDesde.Value = sFechaDesde
                hdnFechaHasta.Value = sFechaHasta
                hdnmoviles.Value = veh_id
                hdnCampos.Value = sRowNumber


                PanelRutinaCompleto.Visible = False
                PanelRutinaKmsRecorridos.Visible = True
                PanelRutinaAlarmas.Visible = False

                GridKmsRecorridos.DataSource = GetKmsRecorridos(sFechaDesde, sFechaHasta, veh_id)
                GridKmsRecorridos.DataBind()


            Else
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As RadioButton = DirectCast(row.FindControl("rdnMovil"), RadioButton)
                    If rdnMovil.Checked Then
                        veh_id = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    End If
                Next


                If veh_id = "0" Then
                    lblError.Text = "Seleccione el Movil para ver el Reporte."
                Else
                    'verifico si elgio filtro de fecha para todos los moviles
                    If txtFechaDesde.Text <> "" Then
                        sFechaDesde = CDate(txtFechaDesde.Text).ToString("yyyyMMdd") + " " + sHoraDesde
                    End If


                    If txtFechaHasta.Text <> "" Then
                        sFechaHasta = CDate(txtFechaHasta.Text).ToString("yyyyMMdd") + " " + sHoraHasta
                    End If

                    hdnFechaDesde.Value = sFechaDesde
                    hdnFechaHasta.Value = sFechaHasta
                    hdnmoviles.Value = veh_id
                    hdnCampos.Value = sRowNumber


                    PanelRutinaCompleto.Visible = False
                    PanelRutinaKmsRecorridos.Visible = True
                    PanelRutinaAlarmas.Visible = False

                    GridKmsRecorridos.DataSource = GetKmsRecorridos(sFechaDesde, sFechaHasta, veh_id)
                    GridKmsRecorridos.DataBind()
                End If

            End If
            hdnTab.Value = "#tabs-2"

        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES CUSTOM KMS -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el reporte customizado. Contacte al Administrador."
        End Try
    End Sub

    Protected Sub btnAlertasCustom_Click(sender As Object, e As EventArgs) Handles btnAlertasCustom.Click
        Try
            'Reporte con campos dinamicos, voy a evaluar que campos mostrar
            Dim sSelect As String = ""
            Dim veh_id As String = "0"
            Dim sRowNumber = ""
            Dim sHoraDesde As String = "00:00:00"
            Dim sHoraHasta As String = "23:59:59"

            'fecha por default un mes
            Dim sFechaDesde As String = DateTime.Now.ToString("yyyyMMdd") + " " + sHoraDesde
            Dim sFechaHasta As String = DateTime.Now.AddDays(7).ToString("yyyyMMdd") + " " + sHoraHasta
            'campos a mostrar

            'verifico si ingreso fechas, sino ingreso se las pido
            'verifico rango de una semana
            If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                If (CDate(txtFechaDesde.Text) - CDate(txtFechaHasta.Text)).TotalDays > 7 Then
                    hdnTab.Value = "#tabs-2"
                    lblError.Text = "El rango de fechas no puede superar los 7 días."
                    Return
                End If
            End If

            'verifico si viene con el parametro de un solo vehiculo
            If hdnveh_id.Value <> "" Then

                veh_id = hdnveh_id.Value

                If ddlhoraDesde.SelectedValue <> "" Then
                    sHoraDesde = ddlhoraDesde.SelectedValue
                Else
                    sHoraDesde = "00"
                End If

                If ddlMinDesde.SelectedValue <> "" Then
                    sHoraDesde += ":" & ddlMinDesde.SelectedValue
                Else
                    sHoraDesde += ":00"
                End If


                If ddlHoraHasta.SelectedValue <> "" Then
                    sHoraHasta = ddlHoraHasta.SelectedValue
                Else
                    sHoraHasta = "23"
                End If

                If ddlMinHasta.SelectedValue <> "" Then
                    sHoraHasta += ":" & ddlMinHasta.SelectedValue
                Else
                    sHoraHasta += ":59:59"
                End If

                'verifico si elgio filtro de fecha para todos los moviles
                If txtFechaDesde.Text <> "" Then
                    sFechaDesde = CDate(txtFechaDesde.Text).ToString("yyyyMMdd") + " " + sHoraDesde
                End If

                If txtFechaHasta.Text <> "" Then
                    sFechaHasta = CDate(txtFechaHasta.Text).ToString("yyyyMMdd") + " " + sHoraHasta
                End If

                hdnFechaDesde.Value = sFechaDesde
                hdnFechaHasta.Value = sFechaHasta
                hdnmoviles.Value = veh_id
                hdnCampos.Value = sRowNumber

                'alarmas
                PanelRutinaCompleto.Visible = False
                PanelRutinaKmsRecorridos.Visible = False
                PanelRutinaAlarmas.Visible = True

                GridAlarmasRutina.DataSource = GetAlarmas(sFechaDesde, sFechaHasta, veh_id)
                GridAlarmasRutina.DataBind()
            Else
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As RadioButton = DirectCast(row.FindControl("rdnMovil"), RadioButton)
                    If rdnMovil.Checked Then
                        veh_id = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    End If
                Next

                If veh_id = "0" Then
                    lblError.Text = "Seleccione el Movil para ver el Reporte."
                Else
                    'verifico si elgio filtro de fecha para todos los moviles
                    If txtFechaDesde.Text <> "" Then
                        sFechaDesde = CDate(txtFechaDesde.Text).ToString("yyyyMMdd") + " " + sHoraDesde
                    End If

                    If txtFechaHasta.Text <> "" Then
                        sFechaHasta = CDate(txtFechaHasta.Text).ToString("yyyyMMdd") + " " + sHoraHasta
                    End If

                    hdnFechaDesde.Value = sFechaDesde
                    hdnFechaHasta.Value = sFechaHasta
                    hdnmoviles.Value = veh_id
                    hdnCampos.Value = sRowNumber

                    'alarmas
                    PanelRutinaCompleto.Visible = False
                    PanelRutinaKmsRecorridos.Visible = False
                    PanelRutinaAlarmas.Visible = True

                    GridAlarmasRutina.DataSource = GetAlarmas(sFechaDesde, sFechaHasta, veh_id)
                    GridAlarmasRutina.DataBind()
                End If

            End If
            hdnTab.Value = "#tabs-2"

        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES CUSTOM KMS -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el reporte customizado. Contacte al Administrador."
        End Try
    End Sub

    Protected Sub gridRutinaCompleto_SortRecords(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        Dim sortExpression As String = e.SortExpression

        Dim direction As String = String.Empty

        If SortDirection = SortDirection.Ascending Then

            SortDirection = SortDirection.Descending
            direction = " DESC"

        Else

            SortDirection = SortDirection.Ascending
            direction = " ASC"

        End If

        'tengo que ordenar contra la consulta de la base de datos ya que tambien la pagina
        hdnOrden.Value = sortExpression & direction
        Dim table As DataTable = GetRecorridos(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value, 1, 30, hdnOrden.Value)

        table.DefaultView.Sort = sortExpression & direction
        gridRutinaCompleto.DataSource = table
        gridRutinaCompleto.DataBind()

    End Sub

    Protected Sub gridKms_SortRecords(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        Dim sortExpression As String = e.SortExpression

        Dim direction As String = String.Empty

        If SortDirection = SortDirection.Ascending Then

            SortDirection = SortDirection.Descending
            direction = " DESC"

        Else

            SortDirection = SortDirection.Ascending
            direction = " ASC"

        End If

        Dim table As DataTable = GetKmsRecorridos(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)

        table.DefaultView.Sort = sortExpression & direction
        GridKmsRecorridos.DataSource = table
        GridKmsRecorridos.DataBind()

    End Sub


    Protected Sub gridAlarmas_SortRecords(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        Dim sortExpression As String = e.SortExpression

        Dim direction As String = String.Empty

        If SortDirection = SortDirection.Ascending Then

            SortDirection = SortDirection.Descending
            direction = " DESC"

        Else

            SortDirection = SortDirection.Ascending
            direction = " ASC"

        End If


        Dim table As DataTable = GetAlarmas(hdnFechaDesde.Value, hdnFechaHasta.Value, hdnmoviles.Value)

        table.DefaultView.Sort = sortExpression & direction
        GridAlarmasRutina.DataSource = table
        GridAlarmasRutina.DataBind()

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

    'http://www.canalvisualbasic.net/foro/visual-basic-6-0/problemas-al-crear-documento-excel-12550/
    'http://mattberseth.com/blog/2007/04/export_gridview_to_excel_1.html

    Protected Sub btnEstadisticas_Click(sender As Object, e As EventArgs) Handles btnEstadisticas.Click
        'voy a estadisticas con la fecha y autos elegidos
        Try
            Dim FechaDesde As String = DateTime.Now.ToString("dd/MM/yyyy") & " 00:00:00"
            Dim FechaHasta As String = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")

            If hdnFechaDesde.Value <> "" Then FechaDesde = hdnFechaDesde.Value.Substring(6, 2) & "/" & hdnFechaDesde.Value.Substring(4, 2) & "/" & hdnFechaDesde.Value.Substring(0, 4) & " " & hdnFechaDesde.Value.Substring(9, 5) 'yyyyMMdd HH:mm:ss'
            If hdnFechaHasta.Value <> "" Then FechaHasta = hdnFechaHasta.Value.Substring(6, 2) & "/" & hdnFechaHasta.Value.Substring(4, 2) & "/" & hdnFechaHasta.Value.Substring(0, 4) & " " & hdnFechaHasta.Value.Substring(9, 5)

           
         
            Dim strMoviles As String = ""


            'verifico si eligio algun vehiculo, sino tengo que marcar todos

            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                For Each row As DataListItem In DataListVehiculos1.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                        strMoviles = strMoviles + DataListVehiculos1.DataKeys(row.ItemIndex).ToString()
                    End If

                Next

            End If

          
            If strMoviles = "" Then
                lblError.Text = "Seleccione el o los Moviles para ver el Reporte."
            Else
                Response.Redirect("pEstaditicas.aspx?origen=reporte&fechadesde=" & FechaDesde & "&fechahasta=" & FechaHasta & "&moviles=" & strMoviles, False)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnEstadisticasCustom_Click(sender As Object, e As EventArgs) Handles btnEstadisticasCustom.Click
        Try
            Dim sHoraDesde As String = "00:00:00"
            Dim sHoraHasta As String = "23:59:59"

            'fecha por default un mes
            Dim FechaDesde As String = DateTime.Now.ToString("dd/MM/yyyy") + " " + sHoraDesde
            Dim FechaHasta As String = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy") + " " + sHoraHasta
            Dim strMoviles As String = ""

            If hdnFechaDesde.Value <> "" Then FechaDesde = hdnFechaDesde.Value.Substring(6, 2) & "/" & hdnFechaDesde.Value.Substring(4, 2) & "/" & hdnFechaDesde.Value.Substring(0, 4) & " " & hdnFechaDesde.Value.Substring(9, 5) 'yyyyMMdd HH:mm:ss'
            If hdnFechaHasta.Value <> "" Then FechaHasta = hdnFechaHasta.Value.Substring(6, 2) & "/" & hdnFechaHasta.Value.Substring(4, 2) & "/" & hdnFechaHasta.Value.Substring(0, 4) & " " & hdnFechaHasta.Value.Substring(9, 5)


            'campos a mostrar

            'verifico si ingreso fechas, sino ingreso se las pido
            'verifico rango de una semana
            If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                If (CDate(txtFechaDesde.Text) - CDate(txtFechaHasta.Text)).TotalDays > 7 Then
                    hdnTab.Value = "#tabs-2"
                    lblError.Text = "El rango de fechas no puede superar los 7 días."
                    Return
                End If
            End If


            'verifico si viene con el parametro de un solo vehiculo
            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As RadioButton = DirectCast(row.FindControl("rdnMovil"), RadioButton)
                    If rdnMovil.Checked Then
                        strMoviles = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    End If
                Next
            End If

           


            If ddlhoraDesde.SelectedValue <> "" Then
                sHoraDesde = ddlhoraDesde.SelectedValue
            Else
                sHoraDesde = "00"
            End If

            If ddlMinDesde.SelectedValue <> "" Then
                sHoraDesde += ":" & ddlMinDesde.SelectedValue
            Else
                sHoraDesde += ":00"
            End If


            If ddlHoraHasta.SelectedValue <> "" Then
                sHoraHasta = ddlHoraHasta.SelectedValue
            Else
                sHoraHasta = "23"
            End If

            If ddlMinHasta.SelectedValue <> "" Then
                sHoraHasta += ":" & ddlMinHasta.SelectedValue
            Else
                sHoraHasta += ":59:59"
            End If

            'verifico si elgio filtro de fecha para todos los moviles
            If txtFechaDesde.Text <> "" Then
                FechaDesde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy") + " " + sHoraDesde
            End If

            If txtFechaHasta.Text <> "" Then
                FechaHasta = CDate(txtFechaHasta.Text).ToString("dd/MM/yyyy") + " " + sHoraHasta
            End If


            If strMoviles = "" Then
                lblError.Text = "Seleccione el o los Moviles para ver el Reporte."
            Else
                Response.Redirect("pEstaditicas.aspx?origen=reporte&fechadesde=" & FechaDesde & "&fechahasta=" & FechaHasta & "&moviles=" & strMoviles, False)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class