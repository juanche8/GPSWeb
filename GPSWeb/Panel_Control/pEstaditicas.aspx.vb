Imports GPS.Business
Imports GPS.Data
Public Class pEstaditicas
    Inherits System.Web.UI.Page
    Public _dataVelocidad As String = ""
    Dim CantMoviles As Integer = 0
    Dim strMoviles As String = ""

    Public HoraDesde As String = "00:00"
    Public HoraHasta As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '" Velocidad", "Paradas y tiempos" ( aca iria cantidad de tiempo detenido y en movimiento y el %, y la duracion de cada parada), "kms recorridos", "Alarmas y sensores"
        Try
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    Dim cli_id As Integer = Session("Cliente").ToString()
                    hdncli_id.Value = cli_id.ToString()

                    'verifico si quiere ver solo reportes de un vehiculo
                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()

                        PanelMoviles.Visible = False
                        PanelPatente.Visible = True
                        lblPatente.Text = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value)).veh_patente

                        'seteo para el dia de hoy por dia
                        rdnPeriodo.SelectedValue = "1"

                        txtFechaHasta.Text = DateTime.Now.ToString("dd/MM/yyyy")
                        txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy")

                        ddlhoraDesde.SelectedValue = "00"
                        ddlMinDesde.SelectedValue = "00"

                        ddlHoraHasta.SelectedValue = DateTime.Now.ToString("HH")
                        ddlMinHasta.SelectedValue = "59"

                        HoraHasta = DateTime.Now.ToString("HH:mm:ss")
                        hdnOrigen.Value = "dia"
                        CantMoviles = 1
                        strMoviles = hdnveh_id.Value
                        btnVerEstadisticas_Click(sender, e)
                    Else

                        'verifico si viene de Reportes
                        If Request.Params("origen") IsNot Nothing Then

                            'busco los vehiculos del usuario
                            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)

                            DataListVehiculos.DataSource = vehiculos
                            DataListVehiculos.DataBind()


                            txtFechaDesde.Text = DateTime.Parse(Request.Params("fechadesde")).ToString("dd/MM/yyyy")
                            txtFechaHasta.Text = DateTime.Parse(Request.Params("fechahasta")).ToString("dd/MM/yyyy")

                            ddlhoraDesde.SelectedValue = DateTime.Parse(Request.Params("fechadesde")).ToString("HH")
                            ddlMinDesde.SelectedValue = DateTime.Parse(Request.Params("fechadesde")).ToString("mm")

                            ddlHoraHasta.SelectedValue = DateTime.Parse(Request.Params("fechahasta")).ToString("HH")
                            ddlMinHasta.SelectedValue = DateTime.Parse(Request.Params("fechahasta")).ToString("mm")

                            HoraDesde = DateTime.Parse(Request.Params("fechadesde")).ToString("HH:mm:ss")
                            HoraHasta = DateTime.Parse(Request.Params("fechahasta")).ToString("HH:mm:ss")

                            hdnOrigen.Value = "reporte"
                            Dim _moviles As String() = Split(Request.Params("moviles"), "|")

                            For i = 0 To _moviles.Length - 1
                                For Each row As DataListItem In DataListVehiculos.Items
                                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                                    If DataListVehiculos.DataKeys(row.ItemIndex).ToString() = _moviles(i) Then rdnMovil.Checked = True

                                Next
                            Next

                            btnVerEstadisticas_Click(sender, e)
                        Else
                            'busco los vehiculos del usuario
                            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)

                            DataListVehiculos.DataSource = vehiculos
                            DataListVehiculos.DataBind()
                        End If
                      


                    End If


                End If
            Else
                'no esta logeado
                '  ScriptManager.RegisterStartupScript(UpdatePanel4, UpdatePanel4.GetType(), "redirigir", " parent.iraLogin();", True)
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If



        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador."
        End Try
    End Sub

    Protected Sub btnVerEstadisticas_Click(sender As Object, e As EventArgs) Handles btnVerEstadisticas.Click
        Try

            lblError.Text = ""
            'veoq ue moviles eligio
           
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        If strMoviles.Length > 0 Then strMoviles = strMoviles + ","
                        strMoviles = strMoviles + DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                        CantMoviles += 1
                    End If

                Next

            If CantMoviles = 0 Then
                If hdnveh_id.Value <> "0" Then
                    strMoviles = hdnveh_id.Value
                    CantMoviles = 1
                End If
            End If

            'si eligio consultar por meses no le muestro el boton de reporte
            If rdnPeriodo.SelectedValue = 2 Or rdnPeriodo.SelectedValue = 3 Then
                ButtonReporte.Visible = False
            End If

            'solo armo la hora si no vengo de otra parte como reportes o estadisticas del dia
            If hdnOrigen.Value = "" Then
                If ddlhoraDesde.SelectedValue <> "" Then
                    HoraDesde = ddlhoraDesde.SelectedValue
                Else
                    HoraDesde = "00"
                End If

                If ddlMinDesde.SelectedValue <> "" Then
                    HoraDesde += ":" & ddlMinDesde.SelectedValue
                Else
                    HoraDesde += ":00"
                End If

                If HoraHasta = "" Then
                    If ddlHoraHasta.SelectedValue <> "" Then
                        HoraHasta = ddlHoraHasta.SelectedValue
                    Else
                        'si el dia es menor al actual la hora es hasta las 23
                        If CDate(txtFechaHasta.Text) < DateTime.Now Then
                            HoraHasta = "23"
                        Else
                            HoraHasta = DateTime.Now.ToString("HH")
                        End If

                    End If

                    If ddlMinHasta.SelectedValue <> "" Then
                        HoraHasta += ":" & ddlMinHasta.SelectedValue & ":59"
                    Else
                        'si el dia es menor al actual la hora es hasta las 23
                        If CDate(txtFechaHasta.Text) < DateTime.Now Then
                            HoraHasta += ":59:59"
                        Else
                            HoraHasta += ":" & DateTime.Now.ToString("mm")
                        End If

                    End If
                End If
            End If



            'verifico que eligio fecha menor o igual a la actual

            If CDate(txtFechaHasta.Text & " " & HoraHasta) > DateTime.Now Then
                lblError.Text = "La Fecha Hasta de la consulta debe ser igual o menor a la fecha actual."
                hdnOrigen.Value = ""
                Return
            End If
            Dim rangoDias As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays

            If rdnPeriodo.SelectedValue = "1" Then
                If rangoDias > 7 Then
                    lblError.Text = "El rango de fechas no puede superar los 7 días para la consulta agrupada por Días."
                    Return

                End If
            Else
                If rdnPeriodo.SelectedValue = "3" Then
                    If rangoDias < 60 Then
                        lblError.Text = "El rango de fechas debe ser de por lo menos Dos Meses para la consulta agrupada por Mes."
                        Return
                    End If
                End If

            End If

            If rangoDias > 180 Then

                lblError.Text = "El rango de fechas no puede superar los 6 meses."
                Return
            End If

            If strMoviles = "" Then
                lblError.Text = "Seleccione el o los Moviles para ver el Reporte."
            Else


                If CantMoviles = 1 Then
                    hdnveh_id.Value = strMoviles
                Else
                    hdnveh_id.Value = "0"
                End If


                If CantMoviles = 1 Then hdnveh_id.Value = strMoviles
                Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:100%;border-collapse:collapse;"">" & _
                                     " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

                'velocidad por dias
                If rdnPeriodo.SelectedValue = "1" Then


                    EstadisticasVelocPorDia(strTabla, CantMoviles)
                    EstadisticasParadasDia(CantMoviles)
                    EstadisticasParadasDiaEncendido(CantMoviles)
                    EstadisticasKmsPorDia(CantMoviles)
                    EstadisticasIndicadoresporDia(strMoviles, CantMoviles)
                    EstadisticasSensoresDia(CantMoviles)
                Else

                    If rdnPeriodo.SelectedValue = "2" Then
                        EstadisticasVelocPorSemana(strTabla)
                        EstadisticasParadasSemana(strMoviles, CantMoviles)
                        EstadisticasParadasSemanalEncendido(CantMoviles)
                        EstadisticasKmsPorSemana(CantMoviles)
                        EstadisticasIndicadoresporSemana(strMoviles, CantMoviles)
                        EstadisticasSensoreSemana(CantMoviles)
                    Else
                        If rdnPeriodo.SelectedValue = "3" Then
                            EstadisticasVelocPorMes(strTabla)
                            EstadisticasParadasMes(CantMoviles)
                            EstadisticasParadasMensualEncendido(CantMoviles)
                            EstadisticasKmsPorMes(CantMoviles)
                            EstadisticasIndicadoresporMes(strMoviles, CantMoviles)
                            EstadisticasSensoresMes(CantMoviles)
                        End If
                    End If

                End If

                EstadisticasResumenParadas(strMoviles, CantMoviles)

                'Cosulta de Cantidad de Horas
                'tiempos detenido y motor apagado
                EstadisticasResumenTiempos(CantMoviles)

                ' alarmas
                EstadisticasAlarmas()

                'indicadores RPM yTEMP general
                EstadisticasIndicadores(strMoviles)

                'sensores general
                EstadisticasSensores()

                hdnOrigen.Value = ""
                '  PanelEstaditicas.Visible = True
                '  ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "redirigir", getCharLine(), True)
            End If
        Catch ex As Exception
            lblError.Text = "Error generando los Datos.-" & ex.ToString
        End Try
    End Sub

    Protected Sub Vehiculo_itemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataListVehiculos.ItemDataBound

        Dim rdnMovil As CheckBox = DirectCast(e.Item.FindControl("rdnMovil"), CheckBox)

        If hdnmoviles.Value <> "" Then
            If hdnmoviles.Value.Contains(DataListVehiculos.DataKeys(e.Item.ItemIndex).ToString()) Then
                rdnMovil.Checked = True
            End If
        End If


    End Sub

    Protected Sub LinkDestildar_Click(sender As Object, e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = False
        Next
    End Sub

    Protected Sub LinkTildar_Click(sender As Object, e As EventArgs) Handles LinkTildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = True
        Next
    End Sub

    Private Function getCharLine(ByVal data As String) As String
        Dim texto As String = " google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
         " google.setOnLoadCallback(LineVelocidadChart); " & _
            " function LineVelocidadChart() {" & _
          "var data = google.visualization.arrayToDataTable([ ['Período'," & data & _
           "   var options = {" & _
" title:  'Velociades Máximas'" & _
           "   };" & _
          "    var chart = new google.visualization.LineChart(document.getElementById('barCharVelocidad'));" & _
           "   chart.draw(data, options);" & _
       "   } "




        Return texto
    End Function

    Private Function getComboChart(ByVal data As String, ByVal series As String) As String
        Dim texto As String = "   google.load(""visualization"", ""1"", {packages:[""corechart""]});" & _
"google.setOnLoadCallback(drawVisualization);" & _
"function drawVisualization() {" & _
  "var data = google.visualization.arrayToDataTable([ " & _
  data & _
 "  var options = {" & _
" title:  'Cantidad de Paradas'," & _
  "  vAxis: {title: ""Cantidad""}," & _
  "  hAxis: {title: ""Movil""}," & _
  " width: 600," & _
"        height: 500," & _
"isStacked: true," & _
  "   seriesType: ""bars""," & _
 "    series: {" & series & ": {type: ""line""}}" & _
 " };" & _
  " var chart = new google.visualization.ComboChart(document.getElementById('ComboCharParadas'));" & _
  " chart.draw(data, options); }"

        Return texto

    End Function

    Private Function getComboChartIndicadores(ByVal data As String, ByVal series As String, ByVal titulo As String, ByVal div As String) As String
        Dim texto As String = "   google.load(""visualization"", ""1"", {packages:[""corechart""]});" & _
"google.setOnLoadCallback(drawVisualization);" & _
"function drawVisualization() {" & _
  "var data = google.visualization.arrayToDataTable([ " & _
  data & _
 "  var options = {" & _
 " bars: 'horizontal'," & _
" title:  '" & titulo & "'," & _
" width: 600," & _
"        height: 500," & _
  "  vAxis: {title: ""Cantidad""}," & _
  "  hAxis: {title: ""Movil""}," & _
  "   seriesType: ""bars""," & _
 "    series: {" & series & ": {type: ""line""}}" & _
 " };" & _
  " var chart = new google.visualization.BarChart(document.getElementById('" & div & "'));" & _
  " chart.draw(data, options); }"

        Return texto

    End Function


    Private Function getComboChartSensores(ByVal data As String, ByVal series As String, ByVal titulo As String, ByVal div As String) As String
        Dim texto As String = "   google.load(""visualization"", ""1"", {packages:[""corechart""]});" & _
"google.setOnLoadCallback(drawVisualization);" & _
"function drawVisualization() {" & _
  "var data = google.visualization.arrayToDataTable([ " & _
  data & _
 "  var options = {" &
" title:  '" & titulo & "'," & _
" width: 750," & _
"        height: 500," & _
  "  vAxis: {title: ""Cantidad""}," & _
  "  hAxis: {title: ""Movil"", maxTextLines: ""auto""}," & _
  "   seriesType: ""bars""," & _
 "    series: {" & series & ": {type: ""line""}}" & _
 " };" & _
  " var chart = new google.visualization.ComboChart(document.getElementById('" & div & "'));" & _
  " chart.draw(data, options); }"

        Return texto

    End Function

    Private Function getBarChartApagado(ByVal data As String) As String


        Dim texto As String = "google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
        " google.setOnLoadCallback(BarParadasChart);" & _
" function BarParadasChart() { " & _
     "var data = google.visualization.arrayToDataTable(" & data & _
"     var options = {" & _
"title: 'Cantidad Veces Motor Apagado'," & _
" width: 600," & _
"        height: 500," & _
          "       hAxis: { title: 'Periodo', titleTextStyle: { color: 'gold'} }, " & _
          "vAxis: { title: 'Cantidad', titleTextStyle: { color: 'green'}}" & _
            " };" & _
            " var chart = new google.visualization.ColumnChart(document.getElementById('ComboCharParadasDetallado'));" & _
            " chart.draw(data, options);" & _
         "}"

        Return texto

        ' ['Patente', '12/12/2014','13/12/2014' ],
        '['MSE234', 10,20],
        '['AAA596', 5,6],
        '['ZZZ222',8,30],

    End Function

    Private Function getBarChartEncendido(ByVal data As String) As String


        Dim texto As String = "google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
        " google.setOnLoadCallback(BarParadasChart);" &
" function BarParadasChart() { " & _
     "var data = google.visualization.arrayToDataTable(" & data & _
"     var options = {" & _
"title: 'Cantidad Veces Motor Encendido a 0 km/h'," & _
" width: 600," & _
"        height: 500," & _
          "       hAxis: { title: 'Período', titleTextStyle: { color: 'gold'} }, " & _
          "vAxis: { title: 'Cant Apagado', titleTextStyle: { color: 'green'}}" & _
            " };" & _
            " var chart = new google.visualization.ColumnChart(document.getElementById('ComboCharEncendidoDetallado'));" & _
            " chart.draw(data, options);" & _
         "}"

        Return texto

    End Function

    Private Function getCharBar(ByVal data As String) As String

        Dim texto As String = "google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
         " google.setOnLoadCallback(BarVelocidadChart);" & _
 " function BarVelocidadChart() { " & _
      "var data = google.visualization.arrayToDataTable([ ['Periodo'," & data & _
 "     var options = {" & _
"title: 'Velociades Máximas'," & _
" width: 650," & _
"        height: 500," & _
           "       hAxis: { title: 'Período', titleTextStyle: { color: 'red'} }, " & _
           "vAxis: { title: 'Velocidad', titleTextStyle: { color: 'green'}}" & _
             " };" & _
             " var chart = new google.visualization.ColumnChart(document.getElementById('barCharVelocidad'));" & _
             " chart.draw(data, options);" & _
          "}"
        Return texto
    End Function

    Private Function getPieChar(titulo As String, data As String, div As String)
        Dim texto As String = " google.load(""visualization"", ""1"", {packages:[""corechart""]}); " & _
    "  google.setOnLoadCallback(drawChart);" & _
    "  function drawChart() {" & _
    "    var data = google.visualization.arrayToDataTable([" & _
     "     ['Patente', 'Cantidad']," & _
    data & _
       " ]);" & _
       " var options = {" & _
"title:  '" & titulo & "'" & _
      "  };" & _
       " var chart = new google.visualization.PieChart(document.getElementById('" & div & "'));" & _
      "  chart.draw(data, options);" & _
     " }"

        Return texto
    End Function

    Private Function getCharBarKms(ByVal data As String) As String

        Dim texto As String = "google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
         " google.setOnLoadCallback(BarKmsChart);" & _
 " function BarKmsChart() { " & _
      "var data = google.visualization.arrayToDataTable([ ['Periodo'," & data & _
 "     var options = {" & _
"title: 'Kms Recorridos'," & _
" width: 600," & _
"        height: 500," & _
           "       hAxis: { title: 'Período', titleTextStyle: { color: 'blue'} }, " & _
           "vAxis: { title: 'Kms', titleTextStyle: { color: 'green'}}" & _
             " };" & _
             " var chart = new google.visualization.ColumnChart(document.getElementById('CharKmsRecorridos'));" & _
             " chart.draw(data, options);" & _
          "}"
        Return texto
    End Function

    Private Function getPieChartKms(data As String)
        Dim texto As String = " google.load(""visualization"", ""1"", {packages:[""corechart""]}); " & _
    "  google.setOnLoadCallback(drawChartKms);" & _
    "  function drawChartKms() {" & _
    "    var data = google.visualization.arrayToDataTable([" & _
     "     ['Fecha', 'Kms']," & _
    data & _
       " var options = {" & _
"title:  'Kms Recorridos'," & _
" width: 600," & _
"        height: 500" & _
      "  };" & _
       " var chart = new google.visualization.PieChart(document.getElementById('CharKmsRecorridos'));" & _
      "  chart.draw(data, options);" & _
     " }"

        Return texto
    End Function

    Private Function getCharLinePorc(ByVal data As String) As String
        Dim texto As String = " google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
         " google.setOnLoadCallback(LineApagadoChart); " & _
            " function LineApagadoChart() {" & _
          "var data = google.visualization.arrayToDataTable([ ['Período'," & data & _
           "   var options = {" & _
" title:  'Proporción Motor Apagado'," & _
" width: 600," & _
"        height: 500" & _
           "   };" & _
          "    var chart = new google.visualization.LineChart(document.getElementById('LineCharParadasPorce'));" & _
           "   chart.draw(data, options);" & _
       "   } "


        Return texto
    End Function

    Private Function getCharLinePorcEncendido(ByVal data As String) As String
        Dim texto As String = " google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
         " google.setOnLoadCallback(LineApagadoChart); " & _
            " function LineApagadoChart() {" & _
          "var data = google.visualization.arrayToDataTable([ ['Período'," & data & _
           "   var options = {" & _
" title:  'Proporción Motor Encendido a 0 km/h'," & _
" width: 600," & _
"        height: 500" & _
           "   };" & _
          "    var chart = new google.visualization.LineChart(document.getElementById('LineCharEncendidoPorce'));" & _
           "   chart.draw(data, options);" & _
       "   } "


        Return texto
    End Function

    Private Function getCharBarTiempo(ByVal data As String) As String

        Dim texto As String = "google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
         " google.setOnLoadCallback(BarTiempoChart);" & _
 " function BarTiempoChart() { " & _
      "var data = google.visualization.arrayToDataTable([ " & data & _
         " ]);" & _
 "     var options = {" & _
"title: 'Resumen de Tiempos'," & _
" width: 600," & _
"        height: 500," & _
           "       hAxis: { title: 'Patente', titleTextStyle: { color: 'blue'} }, " & _
           "vAxis: { title: 'Horas', titleTextStyle: { color: 'green'}}" & _
             " };" & _
             " var chart = new google.visualization.ColumnChart(document.getElementById('CharTiemposHs'));" & _
             " chart.draw(data, options);" & _
          "}"
        Return texto
    End Function

    Private Function getPieChartTiempo(data As String)
        Dim texto As String = " google.load(""visualization"", ""1"", {packages:[""corechart""]}); " & _
    "  google.setOnLoadCallback(drawChartTiempo);" & _
    "  function drawChartTiempo() {" & _
    "    var data = google.visualization.arrayToDataTable([" & _
       data & _
  " ]);" & _
       " var options = {" & _
"title:  'Resumen de Tiempos'," & _
" width: 600," & _
"        height: 500" & _
      "  };" & _
       " var chart = new google.visualization.PieChart(document.getElementById('CharTiemposHs'));" & _
      "  chart.draw(data, options);" & _
     " }"

        Return texto
    End Function

    Private Function getCharBarAlarmas(ByVal data As String) As String

        Dim texto As String = "google.load('visualization', '1.1', { packages: ['corechart'] }); " & _
         " google.setOnLoadCallback(BarAlarmaChart);" & _
 " function BarAlarmaChart() { " & _
      "var data = google.visualization.arrayToDataTable([" & data & _
 "     var options = {" & _
"title: 'Alarmas Reportadas'," & _
" width: 750," & _
"        height: 500," & _
           "       hAxis: { title: 'Alarma', titleTextStyle: { color: 'blue'} }, " & _
           "vAxis: { title: 'Cantidad', titleTextStyle: { color: 'green'}}" & _
             " };" & _
             " var chart = new google.visualization.ColumnChart(document.getElementById('CharAlarmas'));" & _
             " chart.draw(data, options);" & _
          "}"
        Return texto
    End Function

    Private Sub EstadisticasParadasMes(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:100%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Detallado Motor Apagado</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        Dim strTablaPorc As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:100%;border-collapse:collapse;"">" & _
                               " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Proporción de Motor Apagado por Fecha</th></tr>" & _
                               "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        ltGrillaParadaPorc.Text = ""
        ltGrillaParadaDetallado.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia

        Dim _apagado As New Apagado
        Dim charDataProporcion As String = ""
        Dim _Semanas As New List(Of Semanas)
        Dim _dias As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _rango As Integer = Math.Ceiling(_dias / 30)


        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(getCantDiasMes(CDate(txtFechaDesde.Text).Month) - 1).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(getCantDiasMes(CDate(_semana.fecha_desde).Month)).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next


        Dim charData As String = " [['Movil'"
        Dim _totales As New List(Of Apagado)
        Dim _totalesSemanas As New List(Of TotalesSemanas)

        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
            If rdnMovil.Checked Then

                If charDataProporcion.Length > 0 Then charDataProporcion += ","
                charDataProporcion += "'" & patente.Text.ToUpper & "'"

                If charData.Length > 0 Then charData += ","
                charData += "'" & patente.Text.ToUpper & "'"

                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & " %</th>"

                'lo calculo una vez por movil
                Dim _total As New Apagado
                _total.Cantidad = clsReporte.TotalApagadoDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), veh_id)
                _total.veh_id = veh_id
                _totales.Add(_total)
            End If
        Next

        charData += "],"

      
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        For Each fechas As Semanas In _Semanas
            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            charData += "["
            charData += "'" & mes & "',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then

                
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id, 1)

                    If _apagado Is Nothing Then
                        charData += "0," '

                    Else
                        charData += _apagado.Cantidad.ToString() & ","
                    End If

              
            End If
            Next
            charData += "],"
        Next


        charDataProporcion += "],"
        'total general
        Dim totalGeneral As Integer
        For Each total In _totales
            totalGeneral += total.Cantidad
        Next

        'total por fila
        strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

        If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL</th>"
        strTabla += "</tr>"

        ' ['2004', 1000, 400],
        For Each fechas As Semanas In _Semanas

            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            strTabla += "<tr><td align=""left"">" & mes & "</td>"

            Dim totalApagado As Integer = 0
            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    '   _apagado = clsReporte.CantidadApagadoMes(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id, 1)

                    If _apagado Is Nothing Then
                        strTabla += "<td align=""center""></td>"

                    Else
                        Dim porcentaje As Decimal = 0

                        If _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad)

                        totalfila += _apagado.Cantidad

                        strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", _apagado.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                    End If
                End If

            Next

            If totalGeneral > 0 Then totalPorcFila = ((totalfila * 100) / totalGeneral)
            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"

            strTabla += " </tr>"

            'totales por semana
            Dim _totalSemana As New TotalesSemanas
            _totalSemana.fecha_desde = fechas.fecha_desde
            _totalSemana.fecha_hasta = fechas.fecha_hasta
            _totalSemana.total = totalfila
            _totalesSemanas.Add(_totalSemana)
        Next


        charData += "]);"

        'TABLA PROPORCIONES
        For Each fechas As Semanas In _Semanas

            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            strTablaPorc += "<tr><td align=""left"">" & mes & "</td>"

            charDataProporcion += "["
            charDataProporcion += "'" & mes & "',"

            Dim totalApagado As Integer = 0
            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then


                    _apagado = clsReporte.CantidadApagadoMes(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    If _apagado Is Nothing Then
                        charDataProporcion += "0," '

                        strTablaPorc += "<td align=""center""></td>"

                    Else

                        Dim porcentaje As Decimal = 0

                        If _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total)

                        totalPorcFila += porcentaje

                        If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", porcentaje) & "</td>"
                        charDataProporcion += porcentaje.ToString().Replace(",", ".") & ","

                    End If
                End If

            Next

            If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", totalPorcFila) & "</td>"
            strTablaPorc += " </tr>"
            charDataProporcion += "],"
        Next


        charDataProporcion += "]);"

        'total general

        strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
        For Each total In _totales

            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", total.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        Next

        'total general pro fila
        strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        strTabla += "</tr>"


        strTabla += "</tbody></table>"
        strTablaPorc += "</tbody></table>"
        If CantMoviles > 1 Then ltGrillaParadaPorc.Text = strTablaPorc

        ltGrillaParadaDetallado.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoApagado", " <script> " & getBarChartApagado(charData) & " </script>")
        If CantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoLineApagado", " <script> " & getCharLinePorc(charDataProporcion) & " </script>")
        Else
            hdnOcultarProporcion.Value = "1"
        End If


    End Sub

    Private Sub EstadisticasParadasSemana(strMoviles As String, CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Detallado Motor Apagado</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        Dim strTablaPorc As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                               " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Proporción de Motor Apagado por Fecha</th></tr>" & _
                               "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        ltGrillaParadaPorc.Text = ""
        ltGrillaParadaDetallado.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 7)

        Dim _apagado As New Apagado

        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(6).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(7).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next

        Dim charDataProporcion As String = ""
        Dim charData As String = " [['Movil'"

        Dim _totales As New List(Of Apagado)
        Dim _totalesSemanas As New List(Of TotalesSemanas)

        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
            If rdnMovil.Checked Then

                If charDataProporcion.Length > 0 Then charDataProporcion += ","
                charDataProporcion += "'" & patente.Text.ToUpper & "'"

                If charData.Length > 0 Then charData += ","
                charData += "'" & patente.Text.ToUpper & "'"

                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & " %</th>"
                'lo calculo una vez por movil
                Dim _total As New Apagado
                _total.Cantidad = clsReporte.TotalApagadoDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), veh_id)
                _total.veh_id = veh_id
                _totales.Add(_total)

            End If
        Next

        charData += "],"

     
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        For Each fechas As Semanas In _Semanas

            charData += "["
            charData += "'" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "',"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id, 1)

                    If _apagado Is Nothing Then
                        charData += "0," '

                    Else
                        charData += _apagado.Cantidad.ToString() & ","
                    End If


                End If
            Next
            charData += "],"
        Next


        charDataProporcion += "],"

        'total general
        Dim totalGeneral As Integer
        For Each total In _totales
            totalGeneral += total.Cantidad
        Next

        'total por fila
        strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"
        strTabla += "</tr>"

        If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL %</th>"
        strTablaPorc += "</tr>"

        ' ['2004', 1000, 400],
        For Each fechas As Semanas In _Semanas

            strTabla += "<tr><td align=""left"">Del " & CDate(fechas.fecha_desde).ToString("dd/MM") & " al " & CDate(fechas.fecha_hasta).ToString("dd/MM") & "</td>"

            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0


            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id, 1)

                    If _apagado Is Nothing Then
                        strTabla += "<td align=""center""></td>"

                    Else
                        Dim porcentaje As Decimal = 0
                        If _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad)

                        totalfila += _apagado.Cantidad
                        'el total dela uto sobre el total de todos los autos para esa fecha
                        strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", _apagado.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                    End If
                End If

            Next

            If totalGeneral > 0 Then totalPorcFila = ((totalfila * 100) / totalGeneral)
            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"
            strTabla += " </tr>"
            'totales por semana
            Dim _totalSemana As New TotalesSemanas
            _totalSemana.fecha_desde = fechas.fecha_desde
            _totalSemana.fecha_hasta = fechas.fecha_hasta
            _totalSemana.total = totalfila
            _totalesSemanas.Add(_totalSemana)
        Next


        charData += "]);"


        'TABLA PROPORCION
        For Each fechas As Semanas In _Semanas

            strTablaPorc += "<tr><td align=""left"">Del " & CDate(fechas.fecha_desde).ToString("dd/MM") & " al " & CDate(fechas.fecha_hasta).ToString("dd/MM") & "</td>"
            charDataProporcion += "["
            charDataProporcion += "'" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "',"
            Dim totalApagado As Integer = 0
            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    _apagado = clsReporte.CantidadApagadoSemana(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    If _apagado Is Nothing Then
                        charDataProporcion += "0," '

                        strTabla += "<td align=""center""></td>"

                    Else
                        Dim porcentaje As Decimal = 0

                        If _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total)

                        'el total del a uto sobre el total de todos los autos para esa semana
                        totalPorcFila += porcentaje
                        If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", porcentaje) & "</td>"
                        charDataProporcion += porcentaje.ToString().Replace(",", ".") & ","

                    End If
                End If

            Next

            If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", totalPorcFila) & "</td>"

            strTablaPorc += " </tr>"
            charDataProporcion += "],"
        Next

        charDataProporcion += "]);"



        'total general

        strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
        For Each total In _totales
            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", total.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        Next

        'total general pro fila
        strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        strTabla += "</tr>"


        strTabla += "</tbody></table>"
        strTablaPorc += "</tbody></table>"
        If CantMoviles > 1 Then ltGrillaParadaPorc.Text = strTablaPorc

        ltGrillaParadaDetallado.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoApagado", " <script> " & getBarChartApagado(charData) & " </script>")
        If CantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoLineApagado", " <script>" & getCharLinePorc(charDataProporcion) & " </script>")
        Else
            hdnOcultarProporcion.Value = "1"
        End If


    End Sub

    Private Sub EstadisticasParadasDia(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""4"" align=""center"">Detallado Motor Apagado</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        Dim strTablaPorc As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                               " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Proporción de Motor Apagado por Fecha</th></tr>" & _
                               "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        ltGrillaParadaPorc.Text = ""
        ltGrillaParadaDetallado.Text = ""
        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _dias As New List(Of String)
        Dim _rango As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _apagado As New Apagado
        Dim charDataProporcion As String = ""
        Dim totalGeneral As Integer = 0
        Dim charData As String = " [['Movil'"
        Dim hora As Integer = DateTime.Now.Hour
        Dim horaD As Integer = DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).Hour

        Dim horas As Double = diferenciaHora(txtFechaDesde.Text & " " & HoraDesde, txtFechaHasta.Text & " " & HoraHasta)

        hora = Math.Floor(horas)

        
        'si es el mismo dia pero solo algunas horas el rango va a ser menor a cero
        If _rango = 0 Then
            If txtFechaHasta.Text = txtFechaDesde.Text Then
                _rango = 1
            Else
                'si son distintos dias pero el rango de horas es menor a 12 tambien va a dar menor a cero
                If (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays > 0 Then
                    _rango = 1
                End If
            End If
        End If

        'PARA UN DIA UN AUTO agrupo por horas.
        If CantMoviles = 1 And _rango = 1 Then
            'si no es el dia de hoy el que consulta uso la hora hasta las 24

            If txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy") Then
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            Else
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            End If


        Else
            _dias.Add(txtFechaDesde.Text)
            For i As Integer = 1 To _rango - 1
                _dias.Add(CDate(txtFechaDesde.Text).AddDays(i).ToString("dd/MM/yyyy"))
            Next
        End If

        Dim _totales As New List(Of Apagado)
        Dim _totalesSemanas As New List(Of TotalesSemanas)

        If hdnveh_id.Value <> "0" Then

            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))

            If charData.Length > 0 Then charData += ","
            charData += "'" & _movil.veh_patente.ToUpper & "'"
                charData += " ],"

            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & _movil.veh_patente.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"


            'lo calculo una vez por movil
            Dim _total As New Apagado
            _total.Cantidad = clsReporte.TotalApagadoDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), _movil.veh_id)
            _total.veh_id = CInt(hdnveh_id.Value)
            _totales.Add(_total)
            Dim posi As Integer = 0

            For Each fecha As String In _dias

                charData += "["


                If _rango = 1 Then
                    charData += "'" & CDate(fecha).ToString("HH:mm") & "',"
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fecha).ToString("yyyyMMdd HH:mm"), CDate(fecha).ToString("yyyyMMdd HH:59:59"), _movil.veh_id, posi)
                Else
                    charData += "'" & CDate(fecha).ToString("dd/MM/yyyy") & "',"
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", _movil.veh_id, posi)
                End If

                If _apagado Is Nothing Then
                    charData += "0" '

                Else
                    charData += _apagado.Cantidad.ToString()
                End If
                charData += "],"

                If _apagado.Fecha IsNot Nothing Then posi += 1
            Next



            strTabla += "</tr>"

            posi = 0
            For Each fecha As String In _dias

                If _rango = 1 Then
                    strTabla += "<tr><td align=""left"">" & CDate(fecha).ToString("HH:mm") & "</td>"
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fecha).ToString("yyyyMMdd HH:mm"), CDate(fecha).ToString("yyyyMMdd HH:59:59"), _movil.veh_id, posi)
                Else
                    strTabla += "<tr><td align=""left"">" & fecha & "</td>"
                    _apagado = clsReporte.CantidadApagadoDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", _movil.veh_id, posi)
                End If


                Dim totalApagado As Integer = 0
                Dim totalfila As Integer = 0
                Dim totalPorcFila As Decimal = 0

                If _apagado Is Nothing Then

                    strTabla += "<td align=""center""></td>"
                Else

                    Dim porcentaje As Decimal = 0

                    If _totales.Where(Function(v) v.veh_id = _movil.veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totales.Where(Function(v) v.veh_id = _movil.veh_id).FirstOrDefault().Cantidad)

                    totalfila += _apagado.Cantidad

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", _apagado.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                End If


                strTabla += " </tr>"

                'totales por Dia
                Dim _totalSemana As New TotalesSemanas
                _totalSemana.fecha_desde = fecha
                _totalSemana.total = totalfila
                _totalesSemanas.Add(_totalSemana)

                posi += 1
            Next


            charData += "]);"

        Else
            'primero los autos
            ' ['Year', 'Sales', 'Expenses'],
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then

                    If charDataProporcion.Length > 0 Then charDataProporcion += ","
                    charDataProporcion += "'" & patente.Text.ToUpper & "'"

                    If charData.Length > 0 Then charData += ","
                    charData += "'" & patente.Text.ToUpper & "'"

                    strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                    If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & " %</th>"

                    'lo calculo una vez por movil
                    Dim _total As New Apagado
                    _total.Cantidad = clsReporte.TotalApagadoDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), veh_id)
                    _total.veh_id = veh_id
                    _totales.Add(_total)


                End If
            Next

            charData += " ],"
            Dim posi As Integer = 0
            For Each fecha As String In _dias
                charData += "["
                charData += "'" & fecha & "',"
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    If rdnMovil.Checked Then

                        _apagado = clsReporte.CantidadApagadoDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", veh_id, posi)


                        If _apagado Is Nothing Then
                            charData += "0" & "," '

                        Else
                            charData += _apagado.Cantidad.ToString() & ","
                        End If

                End If
                Next
                charData += "],"
                posi += 1
            Next

            charDataProporcion += "],"
            'total general

            For Each total In _totales
                totalGeneral += total.Cantidad
            Next

            'total por fila
            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

            If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL</th>"


            strTabla += "</tr>"

            posi = 0
            For Each fecha As String In _dias

                strTabla += "<tr><td align=""left"">" & fecha & "</td>"

                Dim totalApagado As Integer = 0
                Dim totalfila As Integer = 0
                Dim totalPorcFila As Decimal = 0

                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    If rdnMovil.Checked Then

                        _apagado = clsReporte.CantidadApagadoDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59", veh_id, posi)

                        If _apagado Is Nothing Then

                            strTabla += "<td align=""center""></td>"
                        Else

                            Dim porcentaje As Decimal = 0

                            If _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad)

                            totalfila += _apagado.Cantidad

                            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", _apagado.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                        End If
                    End If

                    posi += 1
                Next
                If totalGeneral > 0 Then totalPorcFila = ((totalfila * 100) / totalGeneral)
                strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"

                strTabla += " </tr>"

                'totales por Dia
                Dim _totalSemana As New TotalesSemanas
                _totalSemana.fecha_desde = fecha
                _totalSemana.total = totalfila
                _totalesSemanas.Add(_totalSemana)
            Next


            charData += "]);"
            If CantMoviles > 1 And _rango > 1 Then
                'TABLA PROPORCION
                posi = 0
                For Each fecha As String In _dias

                    strTablaPorc += "<tr><td align=""left"">" & fecha & "</td>"
                    charDataProporcion += "["
                    charDataProporcion += "'" & fecha & "',"
                    Dim totalApagado As Integer = 0
                    Dim totalfila As Integer = 0
                    Dim totalPorcFila As Decimal = 0

                    For Each row As DataListItem In DataListVehiculos.Items
                        Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                        Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                        If rdnMovil.Checked Then


                            _apagado = clsReporte.CantidadApagadoDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59", veh_id, posi)

                            If _apagado Is Nothing Then
                                charDataProporcion += "0," '

                                totalPorcFila += "<td align=""center""></td>"

                            Else

                                Dim porcentaje As Decimal = 0

                                If _totalesSemanas.Where(Function(v) v.fecha_desde = fecha).FirstOrDefault().total > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totalesSemanas.Where(Function(v) v.fecha_desde = fecha).FirstOrDefault().total)

                                totalPorcFila += porcentaje
                                If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", porcentaje) & "</td>"
                                charDataProporcion += porcentaje.ToString().Replace(",", ".") & ","

                            End If
                        End If
                        posi += 1
                    Next

                    If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", totalPorcFila) & "</td>"
                    strTablaPorc += " </tr>"
                    charDataProporcion += "],"
                Next


                charDataProporcion += "]);"
            End If

            'total general
            strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
            For Each total In _totales
                strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", total.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
            Next

            'total general pro fila
            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
            strTabla += "</tr>"


                End If


                strTabla += "</tbody></table>"
                strTablaPorc += "</tbody></table>"

                ltGrillaParadaDetallado.Text = strTabla
                PanelEstaditicas.Visible = True
                'si tengo un solo movil muestro grafico de lineas

                ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoApagado", " <script> " & getBarChartApagado(charData) & " </script>")
                If CantMoviles > 1 And _rango > 1 Then
                    ltGrillaParadaPorc.Text = strTablaPorc
                    ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoLineApagado", " <script> " & getCharLinePorc(charDataProporcion) & " </script>")

                Else
                    hdnOcultarProporcion.Value = "1"

                End If


    End Sub

    '** PARADAS CON MOTOR ENCENDIDO Y VELOCIADAD = 0
    Private Sub EstadisticasParadasDiaEncendido(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Detallado Motor Encendido a 0 km/h</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        Dim strTablaPorc As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                               " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Proporción de Motor Encendido a 0 km/h por Fecha</th></tr>" & _
                               "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        ltGrillaEncendidoPorc.Text = ""
        ltGrillaEncendidoDetallado.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _dias As New List(Of String)
        Dim _rango As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _apagado As New Apagado

        Dim charDataProporcion As String = ""
        Dim totalGeneral As Integer = 0
        Dim charData As String = " [['Movil'"
        Dim hora As Integer = DateTime.Now.Hour
        Dim horaD As Integer = DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).Hour

        Dim horas As Double = diferenciaHora(txtFechaDesde.Text & " " & HoraDesde, txtFechaHasta.Text & " " & HoraHasta)

        hora = Math.Floor(horas)

        'si es el mismo dia pero solo algunas horas el rango va a ser menor a cero
        If _rango = 0 Then
            If txtFechaHasta.Text = txtFechaDesde.Text Then
                _rango = 1
            Else
                'si son distintos dias pero el rango de horas es menor a 12 tambien va a dar menor a cero
                If (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays > 0 Then
                    _rango = 1
                End If
            End If
        End If

        'PARA UN DIA UN AUTO agrupo por horas.
        If CantMoviles = 1 And _rango = 1 Then
            'si no es el dia de hoy el que consulta uso la hora hasta las 24

            If txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy") Then
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            Else
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            End If


        Else
            _dias.Add(txtFechaDesde.Text)
            For i As Integer = 1 To _rango - 1
                _dias.Add(CDate(txtFechaDesde.Text).AddDays(i).ToString("dd/MM/yyyy"))
            Next
        End If

        Dim _totales As New List(Of Apagado)
        Dim _totalesSemanas As New List(Of TotalesSemanas)

        If hdnveh_id.Value <> "0" Then

            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))

            If charData.Length > 0 Then charData += ","
            charData += "'" & _movil.veh_patente.ToUpper & "'"
            charData += " ],"

            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & _movil.veh_patente.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"


            'lo calculo una vez por movil
            Dim _total As New Apagado
            _total.Cantidad = clsReporte.TotalEncendidoCeroDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), _movil.veh_id)
            _total.veh_id = CInt(hdnveh_id.Value)
            _totales.Add(_total)

         

            For Each fecha As String In _dias
                charData += "["


                If _rango = 1 Then
                    charData += "'" & CDate(fecha).ToString("HH:mm") & "',"
                    _apagado = clsReporte.CantidadEncendidoCeroDia(CDate(fecha).ToString("yyyyMMdd HH:mm"), CDate(fecha).ToString("yyyyMMdd HH:59:59"), _movil.veh_id)
                Else
                    charData += "'" & CDate(fecha).ToString("dd/MM/yyyy") & "',"
                    _apagado = clsReporte.CantidadEncendidoCeroDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", _movil.veh_id)
                End If

                If _apagado Is Nothing Then
                    charData += "0" '

                Else
                    charData += _apagado.Cantidad.ToString()
                End If
                charData += "],"
            Next



            strTabla += "</tr>"


            For Each fecha As String In _dias

                If _rango = 1 Then
                    strTabla += "<tr><td align=""left"">" & CDate(fecha).ToString("HH:mm") & "</td>"
                Else
                    strTabla += "<tr><td align=""left"">" & CDate(fecha).ToString("dd/MM/yyyy") & "</td>"
                End If


                Dim totalApagado As Integer = 0
                Dim totalfila As Integer = 0
                Dim totalPorcFila As Decimal = 0

                If _rango = 1 Then
                    _apagado = clsReporte.CantidadEncendidoCeroDia(CDate(fecha).ToString("yyyyMMdd HH:mm"), CDate(fecha).ToString("yyyyMMdd HH:59:59"), _movil.veh_id)
                Else
                    _apagado = clsReporte.CantidadEncendidoCeroDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", _movil.veh_id)
                End If

                If _apagado Is Nothing Then

                    strTabla += "<td align=""center""></td>"
                Else

                    Dim porcentaje As Decimal = 0

                    If _totales.Where(Function(v) v.veh_id = _movil.veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totales.Where(Function(v) v.veh_id = _movil.veh_id).FirstOrDefault().Cantidad)

                    totalfila += _apagado.Cantidad

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", _apagado.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                End If


                strTabla += " </tr>"

                'totales por Dia
                Dim _totalSemana As New TotalesSemanas
                _totalSemana.fecha_desde = fecha
                _totalSemana.total = totalfila
                _totalesSemanas.Add(_totalSemana)
            Next


            charData += "]);"
            '**********************
            'MAS DE UN DIA Y UN AUTO
        Else
            'primero los autos
            ' ['Year', 'Sales', 'Expenses'],

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then

                    If charDataProporcion.Length > 0 Then charDataProporcion += ","
                    charDataProporcion += "'" & patente.Text.ToUpper & "'"

                    If charData.Length > 0 Then charData += ","
                    charData += "'" & patente.Text.ToUpper & "'"

                    strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                    If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & " %</th>"

                    'lo calculo una vez por movil
                    Dim _total As New Apagado
                    _total.Cantidad = clsReporte.TotalEncendidoCeroDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), veh_id)
                    _total.veh_id = veh_id
                    _totales.Add(_total)

                End If
            Next

            charData += "],"

            For Each fecha As String In _dias
                charData += "["
                charData += "'" & fecha & "',"

                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    If rdnMovil.Checked Then

                        _apagado = clsReporte.CantidadEncendidoCeroDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59", veh_id)

                        If _apagado Is Nothing Then
                            charData += "0," '

                        Else
                            charData += _apagado.Cantidad.ToString() & ","
                        End If

                End If
                Next

                charData += "],"
            Next



            charDataProporcion += "],"
            'total general

            For Each total In _totales
                totalGeneral += total.Cantidad
            Next

            'total por fila
            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

            If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL</th>"
            strTabla += "</tr>"

            For Each fecha As String In _dias

                strTabla += "<tr><td align=""left"">" & fecha & "</td>"
                Dim totalApagado As Integer = 0
                Dim totalfila As Integer = 0
                Dim totalPorcFila As Decimal = 0

                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    If rdnMovil.Checked Then
                        _apagado = clsReporte.CantidadEncendidoCeroDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59", veh_id)

                        If _apagado Is Nothing Then

                            strTabla += "<td align=""center""></td>"
                        Else
                            Dim porcentaje As Decimal = 0

                            If _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad)

                            totalfila += _apagado.Cantidad
                            totalPorcFila += porcentaje

                            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", _apagado.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                        End If
                    End If

                Next

                strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"

                strTabla += " </tr>"

                'totales por Dia
                Dim _totalSemana As New TotalesSemanas
                _totalSemana.fecha_desde = fecha
                _totalSemana.total = totalfila
                _totalesSemanas.Add(_totalSemana)
            Next


            charData += "]);"

            'TABLA PROPORCION
            For Each fecha As String In _dias

                strTablaPorc += "<tr><td align=""left"">" & fecha & "</td>"
                charDataProporcion += "["
                charDataProporcion += "'" & fecha & "',"
                Dim totalApagado As Integer = 0
                Dim totalfila As Integer = 0
                Dim totalPorcFila As Decimal = 0

                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    If rdnMovil.Checked Then


                        _apagado = clsReporte.CantidadEncendidoCeroDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59", veh_id)

                        If _apagado Is Nothing Then
                            charDataProporcion += "0," '

                            strTablaPorc += "<td align=""center"">0.00</td>"

                        Else
                            Dim porcentaje As Decimal = 0

                            If _totalesSemanas.Where(Function(v) v.fecha_desde = fecha).FirstOrDefault().total > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totalesSemanas.Where(Function(v) v.fecha_desde = fecha).FirstOrDefault().total)

                            totalPorcFila += porcentaje
                            If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", porcentaje) & "</td>"
                            charDataProporcion += porcentaje.ToString().Replace(",", ".") & ","

                        End If
                    End If
                Next

                If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", totalPorcFila) & "</td>"
                strTablaPorc += " </tr>"
                charDataProporcion += "],"
            Next


            charDataProporcion += "]);"

            'total general

            strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
            For Each total In _totales
                totalGeneral += total.Cantidad
                strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", total.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
            Next

            'total general pro fila
            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
            strTabla += "</tr>"
        End If



        strTabla += "</tbody></table>"
        strTablaPorc += "</tbody></table>"


        ltGrillaEncendidoDetallado.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoEncendido", " <script> " & getBarChartEncendido(charData) & " </script>")
        If CantMoviles > 1 And _rango > 1 Then
            ltGrillaEncendidoPorc.Text = strTablaPorc
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoLineEncendido", " <script> " & getCharLinePorcEncendido(charDataProporcion) & " </script>")
        Else
            hdnOcultarProporcion.Value = "1"
        End If




    End Sub


    Private Sub EstadisticasParadasSemanalEncendido(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Detallado Motor Encendido a 0 km/h</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        Dim strTablaPorc As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                               " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Proporción de Motor Encendido a 0 km/h por Fecha</th></tr>" & _
                               "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"
        ltGrillaEncendidoPorc.Text = ""
        ltGrillaEncendidoPorc.Text = ""
        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 7)
        Dim _apagado As New Apagado

        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(6).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(7).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next

        Dim charDataProporcion As String = ""
        Dim charData As String = " [['Movil'"
     
        Dim _totales As New List(Of Apagado)
        Dim _totalesSemanas As New List(Of TotalesSemanas)
        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
            If rdnMovil.Checked Then

                If charDataProporcion.Length > 0 Then charDataProporcion += ","
                charDataProporcion += "'" & patente.Text.ToUpper & "'"

                If charData.Length > 0 Then charData += ","
                charData += "'" & patente.Text.ToUpper & "'"


                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & " %</th>"

                'lo calculo una vez por movil
                Dim _total As New Apagado
                _total.Cantidad = clsReporte.TotalEncendidoCeroDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), veh_id)
                _total.veh_id = veh_id
                _totales.Add(_total)

            End If
        Next

        charData += "],"
      
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        Dim cant As Integer = 0
        For Each fechas As Semanas In _Semanas

            charData += "["
            charData += "'" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "',"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    cant = 0
                    cant = clsReporte.TotalEncendidoCeroDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)
                    charData += cant.ToString() & ","
               
            End If
            Next
            charData += "],"
        Next


        charDataProporcion += "],"

        'total por fila
        strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

        If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL</th>"


        strTabla += "</tr>"

        For Each fechas As Semanas In _Semanas

            strTabla += "<tr><td align=""left"">Del " & CDate(fechas.fecha_desde).ToString("dd/MM") & " al " & CDate(fechas.fecha_hasta).ToString("dd/MM") & "</td>"
            Dim totalApagado As Integer = 0
            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then

                    cant = 0
                    cant = clsReporte.TotalEncendidoCeroDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    If _apagado Is Nothing Then
                        strTabla += "<td align=""center""></td>"

                    Else
                        Dim porcentaje As Decimal = 0
                        If _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((cant * 100) / _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad)

                        totalfila += cant
                        'el total dela uto sobre el total de todos los autos para esa fecha
                        totalPorcFila += porcentaje

                        strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", cant) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                    End If
                End If

            Next

            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"

            strTabla += " </tr>"


            'totales por semana
            Dim _totalSemana As New TotalesSemanas
            _totalSemana.fecha_desde = fechas.fecha_desde
            _totalSemana.fecha_hasta = fechas.fecha_hasta
            _totalSemana.total = totalfila
            _totalesSemanas.Add(_totalSemana)
        Next


        charData += "]);"

        'TABLA PROPORCION
        For Each fechas As Semanas In _Semanas
            strTablaPorc += "<tr><td align=""left"">Del " & CDate(fechas.fecha_desde).ToString("dd/MM") & " al " & CDate(fechas.fecha_hasta).ToString("dd/MM") & "</td>"
            charDataProporcion += "["
            charDataProporcion += "'" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "',"

            Dim totalApagado As Integer = 0
            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    _apagado = clsReporte.CantidadEncendidoSemanal(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    If _apagado Is Nothing Then
                        charDataProporcion += "0," '
                        strTabla += "<td align=""center""></td>"
                    Else
                        Dim porcentaje As Decimal = 0

                        If _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total)

                        'el total del a uto sobre el total de todos los autos para esa semana
                        totalPorcFila += porcentaje
                        If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", porcentaje) & "</td>"
                        charDataProporcion += porcentaje.ToString().Replace(",", ".") & ","

                    End If
                End If
            Next

            If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", totalPorcFila) & "</td>"
            strTablaPorc += " </tr>"
            charDataProporcion += "],"
        Next


        charDataProporcion += "]);"

        'total general
        Dim totalGeneral As Integer
        strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
        For Each total In _totales
            totalGeneral += total.Cantidad
            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", total.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        Next

        'total general pro fila
        strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        strTabla += "</tr>"


        strTabla += "</tbody></table>"
        strTablaPorc += "</tbody></table>"
        If CantMoviles > 1 Then ltGrillaEncendidoPorc.Text = strTablaPorc

        ltGrillaEncendidoDetallado.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoEncendido", " <script> " & getBarChartEncendido(charData) & " </script>")
        If CantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoLineEncendido", " <script> " & getCharLinePorcEncendido(charDataProporcion) & " </script>")
        Else
            hdnOcultarProporcion.Value = "1"
        End If


    End Sub

    Private Sub EstadisticasParadasMensualEncendido(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Detallado Motor Encendido a 0 km/h</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        Dim strTablaPorc As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                               " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Proporción de Motor Encendido a 0 km/h por Fecha</th></tr>" & _
                               "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"

        ltGrillaEncendidoPorc.Text = ""
        ltGrillaEncendidoPorc.Text = ""
        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _Semanas As New List(Of Semanas)
         Dim _apagado As New Apagado

        Dim _dias As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _rango As Integer = Math.Ceiling(_dias / 30)


        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(getCantDiasMes(CDate(txtFechaDesde.Text).Month) - 1).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(getCantDiasMes(CDate(_semana.fecha_desde).Month)).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next

        Dim charDataProporcion As String = ""
        Dim charData As String = " [['Movil'"
        Dim _totales As New List(Of Apagado)
        Dim _totalesSemanas As New List(Of TotalesSemanas)

        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
            If rdnMovil.Checked Then
                If charDataProporcion.Length > 0 Then charDataProporcion += ","
                charDataProporcion += "'" & patente.Text.ToUpper & "'"

                If charData.Length > 0 Then charData += ","
                charData += "'" & patente.Text.ToUpper & "'"

                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & " %</th>"
                'lo calculo una vez por movil
                Dim _total As New Apagado
                _total.Cantidad = clsReporte.TotalEncendidoCeroDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:59"), veh_id)
                _total.veh_id = veh_id
                _totales.Add(_total)

            End If
        Next

        charData += " ],"

      
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        Dim cant As Integer = 0
        For Each fechas As Semanas In _Semanas
            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            charData += "["
            charData += "'" & mes & "',"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then

                    cant = 0
                    cant = clsReporte.TotalEncendidoCeroDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    charData += cant.ToString() & ","

                End If
            Next
            charData += "],"
        Next


        charDataProporcion += "],"

        'total por fila
        strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

        If CantMoviles > 1 Then strTablaPorc += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL</th>"
        strTabla += "</tr>"

        For Each fechas As Semanas In _Semanas

            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            strTabla += "<tr><td align=""left"">" & mes & "</td>"

            Dim totalApagado As Integer = 0
            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then

                    cant = 0
                    cant = clsReporte.TotalEncendidoCeroDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    Dim porcentaje As Decimal = 0
                        If _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad > 0 Then porcentaje = ((cant * 100) / _totales.Where(Function(v) v.veh_id = veh_id).FirstOrDefault().Cantidad)

                        totalfila += cant
                        'el total dela uto sobre el total de todos los autos para esa fecha
                        totalPorcFila += porcentaje

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", cant) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                End If

            Next

            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"

            strTabla += " </tr>"
            'totales por semana
            Dim _totalSemana As New TotalesSemanas
            _totalSemana.fecha_desde = fechas.fecha_desde
            _totalSemana.fecha_hasta = fechas.fecha_hasta
            _totalSemana.total = totalfila
            _totalesSemanas.Add(_totalSemana)
        Next


        charData += "]);"

        'TABLA PROPORCION
        For Each fechas As Semanas In _Semanas
            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            strTablaPorc += "<tr><td align=""left"">" & mes & "</td>"

            charDataProporcion += "["
            charDataProporcion += "'" & mes & "',"

            Dim totalApagado As Integer = 0
            Dim totalfila As Integer = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    _apagado = clsReporte.CantidadEncendidoMes(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    If _apagado Is Nothing Then
                        charDataProporcion += "0," '

                        strTabla += "<td align=""center""></td>"

                    Else

                        Dim porcentaje As Decimal = 0

                        If _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total > 0 Then porcentaje = ((_apagado.Cantidad * 100) / _totalesSemanas.Where(Function(v) v.fecha_desde = fechas.fecha_desde And v.fecha_hasta = fechas.fecha_hasta).FirstOrDefault().total)

                        'el total del a uto sobre el total de todos los autos para esa semana
                        totalPorcFila += porcentaje
                        If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", porcentaje) & "</td>"
                        charDataProporcion += porcentaje.ToString().Replace(",", ".") & ","

                    End If
                End If
            Next

            If CantMoviles > 1 Then strTablaPorc += "<td align=""center"">" & String.Format("{0:###0.00}", totalPorcFila) & "</td>"
            strTablaPorc += " </tr>"
            charDataProporcion += "],"
        Next


        charDataProporcion += "]);"

        'total general
        Dim totalGeneral As Integer
        strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
        For Each total In _totales
            totalGeneral += total.Cantidad
            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", total.Cantidad) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        Next

        'total general pro fila
        strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        strTabla += "</tr>"


        strTabla += "</tbody></table>"
        strTablaPorc += "</tbody></table>"
        If CantMoviles > 1 Then ltGrillaEncendidoPorc.Text = strTablaPorc

        ltGrillaEncendidoDetallado.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoEncendido", " <script> " & getBarChartEncendido(charData) & " </script>")
        If CantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoLineEncendido", " <script> " & getCharLinePorcEncendido(charDataProporcion) & " </script>")
        Else
            hdnOcultarProporcion.Value = "1"
        End If


    End Sub
    '*********

    Private Sub EstadisticasResumenParadas(strMoviles As String, cantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""4"" align=""center"">Cantidad de Paradas</th></tr><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:20%;"">" & txtFechaDesde.Text & " - " & txtFechaHasta.Text & "</th><th scope=""col"" style=""font-weight:normal;width:20%;"">Cant. Paradas(0km/h)</th><th scope=""col"" style=""font-weight:normal;width:20%;"">Cant.Motor Apagado</th>"
        If cantMoviles > 1 Then strTabla += "<th scope=""col"" style=""font-weight:normal;width:20%;"" align=""center"">Total</th>"
        strTabla += "</tr>"

        ltGrillaPorcentajeParadas.Text = ""
        LiteralDivs.Text = ""
        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _dias As New List(Of String)
        Dim _paradas As New List(Of Paradas)

        Dim totalParadas As Integer = 0
        Dim totalApagado As Integer = 0

        Dim charData As String = ""
        'primero los autos
        charData += "['Patente', 'Paradas', 'Apagado'],"


        _paradas = clsReporte.CantidadDeParadas(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm:ss"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:ss"), strMoviles)

        If _paradas IsNot Nothing Then

            If _paradas.Count = 0 Then
                strTabla += "<tr><td align=""center"" colspan=""4"">Sin Datos</td></tr>"
                charData += "['Sin Datos', 0, 0],"
            Else
                For Each _parada In _paradas
                    ' ['2004', 1000, 400],
                    charData += "['" & _parada.PATENTE & "'," & _parada.CantParadas & "," & _parada.CantMotorApagado & "],"

                    strTabla += "<tr><td align=""center"">" & _parada.PATENTE & "</td><td align=""center"">" & _parada.CantParadas & "</td><td align=""center"">" & _parada.CantMotorApagado & "</td>"
                    If cantMoviles > 1 Then strTabla += "<td align=""center"">" & (_parada.CantMotorApagado + _parada.CantParadas) & "</td>"
                    strTabla += "</tr>"

                    totalParadas += _parada.CantParadas
                    totalApagado += _parada.CantMotorApagado
                Next

                'total general
                If cantMoviles > 1 Then
                    strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td><td align=""center"">" & totalParadas & "</td><td align=""center"">" & totalApagado & "</td>"
                    strTabla += "<td align=""center"">" & (totalParadas + totalApagado) & "</td>"
                    strTabla += "</tr>"
                End If


            End If
        End If

        charData += "]);"

        strTabla += "</tbody></table>"

        ltGrillaParadas.Text = strTabla


        If _paradas.Count > 1 Then
            'muestro tabla de porcentaje y grafico de torta
            strTabla = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:100%;height:auto;border-collapse:collapse;"">" & _
                                 " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""4"" align=""center"">Comparación entre Móviles</th></tr><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">" & txtFechaDesde.Text & " - " & txtFechaHasta.Text & "</th><th scope=""col"" style=""font-weight:normal;width:20%;"">Cant. Paradas(0km/h)</th><th scope=""col"" style=""font-weight:normal;width:20%;"">Cant.Motor Apagado</th>"
            'If cantMoviles > 1 Then strTabla += "<th scope=""col"" style=""font-weight:normal;width:20%;"" align=""center"">Total</th>"
            strTabla += "</tr>"



            Dim porcentajeParadas As Decimal = 0
            Dim porcentajeApagado As Decimal = 0
            Dim Total_porcentajeParadas As Decimal = 0
            Dim Total_porcentajeApagado As Decimal = 0
            Dim charDataPie1 As String = ""
            Dim charDataPie2 As String = ""


            For Each _parada In _paradas
                ' ['2004', 1000, 400],
                If totalParadas <> 0 Then porcentajeParadas = ((_parada.CantParadas * 100) / totalParadas)
                If totalApagado <> 0 Then porcentajeApagado = (_parada.CantMotorApagado * 100 / totalApagado)

                Total_porcentajeParadas += porcentajeParadas
                Total_porcentajeApagado += porcentajeApagado

                charDataPie1 += "['" & _parada.PATENTE & "'," & String.Format("{0:##0.00}", porcentajeParadas).Replace(",", ".") & "],"
                charDataPie2 += "['" & _parada.PATENTE & "'," & String.Format("{0:##0.00}", porcentajeApagado).Replace(",", ".") & "],"

                strTabla += "<tr><td align=""center"">" & _parada.PATENTE & "</td><td align=""center"">" & String.Format("{0:##0.00}", porcentajeParadas) & "%</td><td align=""center"">" & String.Format("{0:##0.00}", porcentajeApagado) & "%</td><td align=""center"">" & String.Format("{0:##0.00}", (porcentajeParadas + porcentajeApagado)) & "%</td></tr>"


            Next

            'total general
            ' If cantMoviles > 1 Then strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td><td align=""center"">" & String.Format("{0:##0.00}", Total_porcentajeParadas) & "%</td><td align=""center"">" & String.Format("{0:##0.00}", Total_porcentajeApagado) & "%</td><td align=""center""></td></tr>"

            strTabla += "</tbody></table>"

            ltGrillaPorcentajeParadas.Text = strTabla

            If Total_porcentajeParadas <> 0 Then
                LiteralDivs.Text = "<div id=""TortaCantParadas"" style=""width: 500px; height: 400px;float:left;""></div>"

                ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoTorta1", " <script> " & getPieChar("Cantidad de Paradas", charDataPie1, "TortaCantParadas") & " </script>")

            End If
            If Total_porcentajeApagado <> 0 Then
                LiteralDivs.Text += "<div id=""ToratMotorApagado"" style=""width: 500px; height: 400px;float:left;""></div>"
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoTorta2", " <script> " & getPieChar("Motor Apagado", charDataPie2, "ToratMotorApagado") & " </script>")

            End If

        End If
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficobarras", " <script> " & getComboChart(charData, 2) & " </script>")


    End Sub

    Private Sub EstadisticasVelocPorDia(strTabla As String, cantMoviles As Integer)

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _dias As New List(Of String)
        Dim _rango As Integer = (DateTime.Parse(txtFechaHasta.Text & " " & HoraHasta) - DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _velocidades As New Velocidades
        Dim charData As String = ""
        Dim horaD As Integer = 0
        Dim hora As Integer = DateTime.Now.Hour

        Dim horas As Double = diferenciaHora(txtFechaDesde.Text & " " & HoraDesde, txtFechaHasta.Text & " " & HoraHasta)

        hora = Math.Floor(horas)
        'si es el mismo dia pero solo algunas horas el rango va a ser menor a cero
        If _rango = 0 Then
            If txtFechaHasta.Text = txtFechaDesde.Text Then
                _rango = 1
            Else
                'si son distintos dias pero el rango de horas es menor a 12 tambien va a dar menor a cero
                If (DateTime.Parse(txtFechaHasta.Text & " " & HoraHasta) - DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde)).TotalDays > 0 Then
                    _rango = 1
                End If
            End If
        End If
      

        'PARA UN DIA UN AUTO agrupo por horas.
        If cantMoviles = 1 And _rango = 1 Then
            'si no es el dia de hoy el que consulta uso la hora hasta las 24

            If txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy") Then
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            Else
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            End If


        Else
            _dias.Add(txtFechaDesde.Text)
            For i As Integer = 1 To _rango - 1
                _dias.Add(DateTime.Parse(txtFechaDesde.Text).AddDays(i).ToString("dd/MM/yyyy"))
            Next
        End If


        If hdnveh_id.Value <> "0" Then

            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
            If charData.Length > 0 Then charData += ","
            charData += "'" & _movil.veh_patente.ToUpper & "'"

            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & _movil.veh_patente.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Max.</td><td align=""center"" style=""font-weight:normal;width:50%;"">Prom.</td></tr></table></th>"


            strTabla += "</tr>"
            charData += " ],"

            For Each fecha As String In _dias
                charData += "["
                If _rango = 1 Then
                    strTabla += "<tr><td align=""left"">" & CDate(fecha).ToString("HH:mm") & "</td>"
                    charData += "'" & DateTime.Parse(fecha).ToString("HH:mm") & "',"
                    _velocidades = clsReporte.VelocidadMaxMovilDia(DateTime.Parse(fecha).ToString("yyyyMMdd HH:mm"), DateTime.Parse(fecha).ToString("yyyyMMdd HH:59:59"), hdnveh_id.Value)
                Else
                    strTabla += "<tr><td align=""left"">" & fecha & "</td>"
                    charData += "'" & fecha & "',"
                    _velocidades = clsReporte.VelocidadMaxMovilDia(DateTime.Parse(fecha).ToString("yyyyMMdd") & " 00:00", DateTime.Parse(fecha).ToString("yyyyMMdd") & " 23:59:59", hdnveh_id.Value)
                End If

                If _velocidades Is Nothing Then
                    charData += "0," 'velocidades maximas por auto

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">0</td><td align=""center"" style=""font-weight:normal;width:50%;"">0</td></tr></table></td>"

                Else
                    charData += _velocidades.VELOCIDAD_MAX.ToString().Replace(",", ".") & "," 'velocidades maximas por auto

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_MAX) & " Kms/h</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_PROM) & " Kms/h</td></tr></table></td>"

                End If

                strTabla += " </tr>"
                charData += "],"
            Next

        Else

            'primero los autos
            ' ['Year', 'Sales', 'Expenses'],
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                If rdnMovil.Checked Then

                    If charData.Length > 0 Then charData += ","
                    charData += "'" & patente.Text.ToUpper & "'"

                    strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Max.</td><td align=""center"" style=""font-weight:normal;width:50%;"">Prom.</td></tr></table></th>"

                End If
            Next

            strTabla += "</tr>"
            charData += " ],"
            ' ['2004', 1000, 400],
            For Each fecha As String In _dias

                strTabla += "<tr><td align=""left"">" & fecha & "</td>"

                charData += "["
                charData += "'" & fecha & "',"
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    If rdnMovil.Checked Then

                        _velocidades = clsReporte.VelocidadMaxMovilDia(DateTime.Parse(fecha).ToString("yyyyMMdd") & " 00:00", DateTime.Parse(fecha).ToString("yyyyMMdd") & " 23:59", veh_id)

                        If _velocidades Is Nothing Then
                            charData += "0," 'velocidades maximas por auto

                            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">0</td><td align=""center"" style=""font-weight:normal;width:50%;"">0</td></tr></table></td>"
                        Else
                            charData += _velocidades.VELOCIDAD_MAX.ToString().Replace(",", ".") & "," 'velocidades maximas por auto

                            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_MAX) & " Kms/h</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_PROM) & " Kms/h</td></tr></table></td>"

                        End If
                    End If
                Next
                strTabla += " </tr>"
                charData += "],"
            Next

        End If

        charData += "]);"

        strTabla += "</tbody></table>"

        ltGrillaVelocidad.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas


        If cantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharBar(charData) & " </script>")
        Else
            If _rango > 1 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharLine(charData) & " </script>")
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharBar(charData) & " </script>")
            End If

        End If

    End Sub


    Private Sub EstadisticasVelocPorSemana(strTabla As String)

        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 7)
        Dim _velocidades As New Velocidades
        Dim cantMoviles As Integer = 0

        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(6).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(7).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next


        Dim charData As String = ""
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            If rdnMovil.Checked Then
                cantMoviles += 1
                If charData.Length > 0 Then charData += ","
                charData += "'" & patente.Text.ToUpper & "'"

                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:30%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"">Max.</td><td align=""center"">Prom.</td></tr></table></th>"

            End If
        Next

        strTabla += "</tr>"
        charData += " ],"
        ' ['2004', 1000, 400],
        For Each fechas As Semanas In _Semanas

            strTabla += "<tr><td align=""left"">Del " & CDate(fechas.fecha_desde).ToString("dd/MM") & " al " & CDate(fechas.fecha_hasta).ToString("dd/MM") & "</td>"

            charData += "["
            charData += "'" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()

                If rdnMovil.Checked Then

                    _velocidades = clsReporte.VelocidadMaxMovilSemana(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59", veh_id)

                    If _velocidades Is Nothing Then
                        charData += "0," 'velocidades maximas por auto

                        strTabla += "<td align=""center""></td>"

                    Else
                        charData += _velocidades.VELOCIDAD_MAX.ToString().Replace(",", ".") & "," 'velocidades maximas por auto

                        strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_MAX) & " Kms/h</td><td align=""center"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_PROM) & " Kms/h</td></tr></table></td>"

                    End If
                End If
            Next
            strTabla += " </tr>"
            charData += "],"
        Next


        charData += "]);"

        strTabla += "</tbody></table>"

        ltGrillaVelocidad.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas


        If cantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharBar(charData) & " </script>")
        Else
            If _rango > 1 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharLine(charData) & " </script>")
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharBar(charData) & " </script>")
            End If

        End If

    End Sub

    Private Sub EstadisticasVelocPorMes(strTabla As String)

        Dim _Semanas As New List(Of Semanas)
        Dim _dias As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _rango As Integer = Math.Ceiling(_dias / 30)


        Dim _velocidades As New Velocidades
        Dim cantMoviles As Integer = 0

        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(getCantDiasMes(CDate(txtFechaDesde.Text).Month) - 1).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(getCantDiasMes(CDate(_semana.fecha_desde).Month)).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next


        Dim charData As String = ""
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            If rdnMovil.Checked Then
                cantMoviles += 1
                If charData.Length > 0 Then charData += ","
                charData += "'" & patente.Text.ToUpper & "'"

                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:30%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"">Max.</td><td align=""center"">Prom.</td></tr></table></th>"

            End If

        Next
        strTabla += "</tr>"
        charData += " ],"
        ' ['2004', 1000, 400],
        For Each fechas As Semanas In _Semanas

            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            strTabla += "<tr><td align=""left"">" & mes & "</td>"

            charData += "["
            charData += "'" & mes & "',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()

                If rdnMovil.Checked Then

                    _velocidades = clsReporte.VelocidadMaxMovilMes(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59", veh_id)

                    If _velocidades Is Nothing Then
                        charData += "0," 'velocidades maximas por auto

                        strTabla += "<td align=""center""></td>"

                    Else
                        charData += _velocidades.VELOCIDAD_MAX.ToString().Replace(",", ".") & "," 'velocidades maximas por auto

                        strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_MAX) & " Kms/h</td><td align=""center"">" & String.Format("{0:###0.00}", _velocidades.VELOCIDAD_PROM) & " Kms/h</td></tr></table></td>"

                    End If
                End If
            Next
            strTabla += " </tr>"
            charData += "],"
        Next


        charData += "]);"

        strTabla += "</tbody></table>"

        ltGrillaVelocidad.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        If cantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharBar(charData) & " </script>")
        Else
            If _rango > 1 Then
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharLine(charData) & " </script>")
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "grafico", " <script> " & getCharBar(charData) & " </script>")
            End If

        End If


    End Sub

    Private Sub EstadisticasKmsPorDia(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Kms Recorridos por Fecha</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"


        ltGrillaKmsRecorridos.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _dias As New List(Of String)
        Dim _rango As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _kms As New Kms
        Dim charData As String = ""
        Dim _totales As New List(Of Kms)

        'si es el mismo dia pero solo algunas horas el rango va a menor a cero
        If txtFechaHasta.Text = txtFechaDesde.Text Then _rango = 1
        Dim hora As Integer = DateTime.Now.Hour
        Dim horaD As Integer = DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).Hour


        Dim horas As Double = diferenciaHora(txtFechaDesde.Text & " " & HoraDesde, txtFechaHasta.Text & " " & HoraHasta)

        hora = Math.Floor(horas)

        'si es el mismo dia pero solo algunas horas el rango va a ser menor a cero
        If _rango = 0 Then
            If txtFechaHasta.Text = txtFechaDesde.Text Then
                _rango = 1
            Else
                'si son distintos dias pero el rango de horas es menor a 12 tambien va a dar menor a cero
                If (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays > 0 Then
                    _rango = 1
                End If
            End If
        End If

        'PARA UN DIA UN AUTO agrupo por horas.
        If CantMoviles = 1 And _rango = 1 Then
            'si no es el dia de hoy el que consulta uso la hora hasta las 24

            If txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy") Then
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            Else
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            End If


        Else
            _dias.Add(txtFechaDesde.Text)
            For i As Integer = 1 To _rango - 1
                _dias.Add(CDate(txtFechaDesde.Text).AddDays(i).ToString("dd/MM/yyyy"))
            Next
        End If

        'primero los autos
        ' ['Periodo', 'Patentes', 'Expenses'],
        If hdnveh_id.Value <> "0" Then
            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))

            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & _movil.veh_patente.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

            'lo calculo una vez por movil
            Dim _total As New Kms
            _total.KMS_RECORRIDOS = clsReporte.TotalKmsDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:59"), _movil.veh_id)
            _total.ID_MODULO = _movil.veh_id
            _totales.Add(_total)


            charData += "'" & _movil.veh_patente.ToUpper & "'"
            ' ['2004', 1000, 400],
            charData += " ],"
            For Each fecha As String In _dias
                charData += "["
                If _rango = 1 Then
                    strTabla += "<tr><td align=""left"">" & CDate(fecha).ToString("HH:mm") & "</td>"
                    charData += "'" & CDate(fecha).ToString("HH:mm") & "',"
                    _kms = clsReporte.KmsRecorridosDia(CDate(fecha).ToString("yyyyMMdd HH:mm"), CDate(fecha).ToString("yyyyMMdd HH:59:59"), _movil.veh_id)

                Else
                    strTabla += "<tr><td align=""left"">" & fecha & "</td>"
                    charData += "'" & fecha & "',"
                    _kms = clsReporte.KmsRecorridosDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", _movil.veh_id)
                End If

                Dim totalKms As Decimal = 0
                Dim totalfila As Decimal = 0
                Dim totalPorcFila As Decimal = 0


                If _kms Is Nothing Then
                    charData += "0," '
                    strTabla += "<td align=""center""></td>"
                Else
                    charData += _kms.KMS_RECORRIDOS.ToString().Replace(",", ".") & ","
                    Dim porcentaje As Decimal = 0

                    If _totales.Where(Function(v) v.ID_MODULO = _movil.veh_id).FirstOrDefault().KMS_RECORRIDOS > 0 Then porcentaje = ((_kms.KMS_RECORRIDOS * 100) / _totales.Where(Function(v) v.ID_MODULO = _movil.veh_id).FirstOrDefault().KMS_RECORRIDOS)

                    totalfila += _kms.KMS_RECORRIDOS

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _kms.KMS_RECORRIDOS) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                End If

                strTabla += " </tr>"

                charData += "],"
            Next

           

            charData += "]);"

        Else
            '*************MAS DE UN AUTO Y DIA
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    If CantMoviles > 1 Then
                        If charData.Length > 0 Then charData += ","
                        charData += "'" & patente.Text.ToUpper & "'"
                    End If

                    strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                    'lo calculo una vez por movil
                    Dim _total As New Kms
                    _total.KMS_RECORRIDOS = clsReporte.TotalKmsDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), veh_id)
                    _total.ID_MODULO = veh_id
                    _totales.Add(_total)
                End If
            Next

            'total general
            Dim totalGeneral As Integer

            For Each total In _totales
                totalGeneral += total.KMS_RECORRIDOS
              Next

            'total por fila
            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th></tr>"

            charData += " ],"
            ' ['2004', 1000, 400],

            For Each fecha As String In _dias

                strTabla += "<tr><td align=""left"">" & fecha & "</td>"

                charData += "["
                charData += "'" & fecha & "',"
                Dim totalKms As Decimal = 0
                Dim totalfila As Decimal = 0
                Dim totalPorcFila As Decimal = 0

                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    If rdnMovil.Checked Then
                        _kms = clsReporte.KmsRecorridosDia(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59", veh_id)

                        If _kms Is Nothing Then
                            charData += "0," '
                            strTabla += "<td align=""center""></td>"
                        Else
                            charData += _kms.KMS_RECORRIDOS.ToString().Replace(",", ".") & ","
                            Dim porcentaje As Decimal = 0

                            If _totales.Where(Function(v) v.ID_MODULO = veh_id).FirstOrDefault().KMS_RECORRIDOS > 0 Then porcentaje = ((_kms.KMS_RECORRIDOS * 100) / _totales.Where(Function(v) v.ID_MODULO = veh_id).FirstOrDefault().KMS_RECORRIDOS)

                            totalfila += _kms.KMS_RECORRIDOS


                            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _kms.KMS_RECORRIDOS) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                        End If
                    End If
                Next

                'Porcentaje por fila
                totalPorcFila = 0
                If (totalGeneral > 0) Then totalPorcFila = (totalfila * 100) / totalGeneral


                strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"
                strTabla += " </tr>"

                charData += "],"
            Next

            charData += "]);"

            'pinto el total general
            strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
            For Each total In _totales
                strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", total.KMS_RECORRIDOS) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
            Next

            'total general pro fila
            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
            strTabla += "</tr>"


        End If
        strTabla += "</tbody></table>"


        ltGrillaKmsRecorridos.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        'If CantMoviles > 1 Then
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoKms", " <script> " & getCharBarKms(charData) & " </script>")
        'Else
        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoKms", " <script> " & getPieChartKms(charData) & " </script>")
        ' End If



    End Sub

    Private Sub EstadisticasKmsPorSemana(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Kms Recorridos por Fecha</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"


        ltGrillaKmsRecorridos.Text = ""

        Dim _kms As New Kms

        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 7)


        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(6).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(7).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next

        Dim charData As String = ""
        Dim _totales As New List(Of Kms)
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
            If rdnMovil.Checked Then

                If CantMoviles > 1 Then
                    If charData.Length > 0 Then charData += ","
                    charData += "'" & patente.Text.ToUpper & "'"
                End If

                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                'lo calculo una vez por movil
                Dim _total As New Kms
                _total.KMS_RECORRIDOS = clsReporte.TotalKmsDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm"), veh_id)
                _total.ID_MODULO = veh_id
                _totales.Add(_total)
            End If
        Next

        'total por fila
        strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

        Dim totalGeneral As Integer
        For Each total In _totales
            totalGeneral += total.KMS_RECORRIDOS
         Next

        strTabla += "</tr>"
        If CantMoviles > 1 Then charData += " ],"
        ' ['2004', 1000, 400],

        Dim _Totalkms As Decimal = 0
        For Each fechas As Semanas In _Semanas

            strTabla += "<tr><td align=""left"">Del " & CDate(fechas.fecha_desde).ToString("dd/MM") & " al " & CDate(fechas.fecha_hasta).ToString("dd/MM") & "</td>"
            charData += "["
            charData += "'" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "',"

            Dim totalKms As Decimal = 0
            Dim totalfila As Decimal = 0
            Dim totalPorcFila As Decimal = 0

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then

                    _Totalkms = 0
                    _Totalkms = clsReporte.TotalKmsDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59", veh_id)

                    If _kms Is Nothing Then
                        charData += "0," '
                        strTabla += "<td align=""center""></td>"
                    Else
                        charData += _Totalkms.ToString().Replace(",", ".") & ","
                        Dim porcentaje As Decimal = 0

                        If _totales.Where(Function(v) v.ID_MODULO = veh_id).FirstOrDefault().KMS_RECORRIDOS > 0 Then porcentaje = ((_Totalkms * 100) / _totales.Where(Function(v) v.ID_MODULO = veh_id).FirstOrDefault().KMS_RECORRIDOS)

                        totalfila += _Totalkms
                       
                        strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _Totalkms) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                    End If
                End If
            Next

            'Porcentaje por fila
            totalPorcFila = 0
            If (totalGeneral > 0) Then totalPorcFila = (totalfila * 100) / totalGeneral

            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"
            strTabla += " </tr>"

            charData += "],"
        Next

        charData += "]);"

        'total general pro fila
         strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
        For Each total In _totales
             strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0.00}", total.KMS_RECORRIDOS) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        Next

        'total general

        strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0.00}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td></tr></tbody></table>"


        ltGrillaKmsRecorridos.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        If CantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoKms", " <script> " & getCharBarKms(charData) & " </script>")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoKms", " <script> " & getPieChartKms(charData) & " </script>")
        End If



    End Sub

    Private Sub EstadisticasKmsPorMes(CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Kms Recorridos por Fecha</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th>"


        ltGrillaKmsRecorridos.Text = ""

        Dim _kms As New KmsMensuales

        Dim _Semanas As New List(Of Semanas)
        Dim _dias As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _rango As Integer = Math.Ceiling(_dias / 30)


        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(getCantDiasMes(CDate(txtFechaDesde.Text).Month) - 1).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(getCantDiasMes(CDate(_semana.fecha_desde).Month)).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next


        Dim charData As String = ""
        Dim _totales As New List(Of Kms)
        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
            If rdnMovil.Checked Then

                If CantMoviles > 1 Then
                    If charData.Length > 0 Then charData += ","
                    charData += "'" & patente.Text.ToUpper & "'"
                End If

                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"

                'lo calculo una vez por movil
                Dim _total As New Kms
                _total.KMS_RECORRIDOS = clsReporte.TotalKmsDia(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:59"), veh_id)
                _total.ID_MODULO = veh_id
                _totales.Add(_total)
            End If
        Next

        'total por fila
        strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">TOTAL<table style=""width:100%;""  rules=""all""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">Cant.</td><td align=""center"" style=""font-weight:normal;width:50%;"">%</td></tr></table></th>"


        Dim totalGeneral As Integer
          For Each total In _totales
            totalGeneral += total.KMS_RECORRIDOS
           Next

        strTabla += "</tr>"
        If CantMoviles > 1 Then charData += " ],"
        ' ['2004', 1000, 400],


        For Each fechas As Semanas In _Semanas

            Dim mes As String = GetMonth(CDate(fechas.fecha_hasta).Month)
            strTabla += "<tr><td align=""left"">" & mes & "</td>"
            charData += "["
            charData += "'" & mes & "',"

            Dim totalKms As Decimal = 0
            Dim totalfila As Decimal = 0
            Dim totalPorcFila As Decimal = 0
            Dim _Totalkms As Decimal = 0
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    _Totalkms = 0
                    totalfila = 0
                    totalPorcFila = 0

                    _Totalkms = clsReporte.TotalKmsDia(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id)

                    If _kms Is Nothing Then
                        charData += "0," '
                        strTabla += "<td align=""center""></td>"
                    Else
                        charData += _Totalkms.ToString().Replace(",", ".") & ","
                        Dim porcentaje As Decimal = 0

                        If _totales.Where(Function(v) v.ID_MODULO = veh_id).FirstOrDefault().KMS_RECORRIDOS > 0 Then porcentaje = ((_Totalkms * 100) / _totales.Where(Function(v) v.ID_MODULO = veh_id).FirstOrDefault().KMS_RECORRIDOS)

                        totalfila += _Totalkms
                      
                        strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", _Totalkms) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", porcentaje) & "%</td></tr></table></td>"

                    End If
                End If
            Next

            'Porcentaje por fila
            totalPorcFila = 0
            If (totalGeneral > 0) Then totalPorcFila = (totalfila * 100) / totalGeneral

            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalfila) & "</td><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:###0.00}", totalPorcFila) & "%</td></tr></table></td>"
            strTabla += " </tr>"

            charData += "],"
        Next

        charData += "]);"

        'total general

        strTabla += "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><td align=""center"">TOTAL</td>"
        For Each total In _totales

            strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0.00}", total.KMS_RECORRIDOS) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        Next

        'total general pro fila
        strTabla += " <td align=""left""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & String.Format("{0:##0.00}", totalGeneral) & "</td><td align=""center"" style=""font-weight:normal;width:50%;""></td></tr></table></td>"
        strTabla += "</tr>"

        strTabla += "</tbody></table>"


        ltGrillaKmsRecorridos.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        If CantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoKms", " <script> " & getCharBarKms(charData) & " </script>")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoKms", " <script> " & getPieChartKms(charData) & " </script>")
        End If



    End Sub

    Private Sub EstadisticasResumenTiempos(CantMoviles As Integer)



        'Tiempo Detenido Motor encendido: cantidad en horas velocidad = 0 Encendido = 1
        'Tiempo en Movimiento: cantidad horas velocidad > 0 Encendido = 1
        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:95%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Resumen de Tiempos</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;""></th>"

        ltGrillaTiempoHs.Text = ""

        Dim charData As String = ""
        'primero los autos


        charData += "['Patente', 'Tiempo Detenido Encendido', 'Tiempo Detenido Total','Tiempo Movimiento', 'Tiempo Motor Apagado'],"
        'primero los autos
        ' ['Patente', 1000, 400],
        Dim _resumenTiempos As New Tiempos()
       
        If hdnveh_id.Value <> "0" Then
            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
           
            Dim TiempoDetenidoEncendido As Long = 0
            Dim TiempoMovimiento As Long = 0
            Dim TiempoMotorApagado As Long = 0
            Dim TiempoTotalDetenido As Long = 0
            strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & _movil.veh_patente.ToUpper & "</th>"
            'armo el data para el chart
            'un movil para un dia tengo que mostrar un grafico  
            Dim _rango As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays

           
            charData = "['TIEMPO', 'Horas'],"

            _resumenTiempos = clsReporte.ResumenTiempos(CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy") & " " & HoraDesde, CDate(txtFechaHasta.Text).ToString("dd/MM/yyyy") & " " & HoraHasta, _movil.veh_id)
            TiempoMovimiento = _resumenTiempos.Movimiento
            TiempoMotorApagado = _resumenTiempos.Apagado
            TiempoDetenidoEncendido = _resumenTiempos.Detenido_Encendido

            TiempoTotalDetenido = TiempoMotorApagado + TiempoDetenidoEncendido

            charData += "['Tiempo Detenido Encendido', "
            ' charData += "['" & _movil.veh_patente.ToUpper() & "',"
            Dim horas As String = ""
            Dim horDetenidoEncendido As Integer
            Dim minDetenidoEncendido As Integer
            Dim segDetenidoEncendido As Integer

            Dim horMovimiento As Integer
            Dim minMovimiento As Integer
            Dim segMovimiento As Integer

            Dim horTotalDetenido As Integer
            Dim minTotalDetenido As Integer
            Dim segTotalDetenido As Integer

            Dim horMotorApagado As Integer
            Dim minMotorApagado As Integer
            Dim segMotorApagado As Integer

            horDetenidoEncendido = Math.Floor(TiempoDetenidoEncendido / 3600)
            minDetenidoEncendido = Math.Floor((TiempoDetenidoEncendido - horDetenidoEncendido * 3600) / 60)
            segDetenidoEncendido = TiempoDetenidoEncendido - (horDetenidoEncendido * 3600 + minDetenidoEncendido * 60)

            Dim dec As Decimal = 0

            Dim ts As TimeSpan = New TimeSpan(0, horDetenidoEncendido, minDetenidoEncendido, segDetenidoEncendido)
            dec = Convert.ToDecimal(ts.TotalHours)
           
             charData += String.Format("{0:##0.00}", dec).Replace(",", ".") & " ],"

            strTabla += "</tr>"

            strTabla += "<tr><td align=""left"">Tiempo Detenido Motor encendido</td>"
            If horDetenidoEncendido > 0 Then
                horas = Trim(horDetenidoEncendido) + " h " + Trim(minDetenidoEncendido) + " m " + Trim(segDetenidoEncendido) + " s"
            Else
                horas = Trim(minDetenidoEncendido) + " m " + Trim(segDetenidoEncendido) + " s"
            End If

            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"

            horas = ""
            '----------------------------------------------------

            charData += "['Tiempo Movimiento', "
            horMovimiento = Math.Floor(TiempoMovimiento / 3600)
            minMovimiento = Math.Floor((TiempoMovimiento - horMovimiento * 3600) / 60)
            segMovimiento = TiempoMovimiento - (horMovimiento * 3600 + minMovimiento * 60)

            ts = New TimeSpan(0, horMovimiento, minMovimiento, segMovimiento)
            dec = Convert.ToDecimal(ts.TotalHours)

            charData += String.Format("{0:##0.00}", dec).Replace(",", ".") & " ],"


            If horMovimiento > 0 Then
                horas = Trim(horMovimiento) + " h " + Trim(minMovimiento) + " m " + Trim(segMovimiento) + " s"
            Else
                horas = Trim(minMovimiento) + " m " + Trim(segMovimiento) + " s"
            End If

            strTabla += "</tr>"

            strTabla += "</tr><tr><td align=""left"">Tiempo en Movimiento</td>"
            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"

            '----------------------------------------
            horas = ""
            'charData += "['Tiempo Detenido', "
            horTotalDetenido = Math.Floor(TiempoTotalDetenido / 3600)
            minTotalDetenido = Math.Floor((TiempoTotalDetenido - horTotalDetenido * 3600) / 60)
            segTotalDetenido = TiempoTotalDetenido - (horTotalDetenido * 3600 + minTotalDetenido * 60)

            ' If _rango > 1 Then
            'If horTotalDetenido > 0 Then
            'charData += Trim(horTotalDetenido & "." & minTotalDetenido) & "]"
            ' Else
            '   charData += Trim("0." & minTotalDetenido) & "]"
            ' End If
            ' End If


            strTabla += "</tr>"
            strTabla += "<tr><td align=""left"">Tiempo Total Detenido</td>"
            If horTotalDetenido > 0 Then
                horas = Trim(horTotalDetenido) + " h " + Trim(minTotalDetenido) + " m " + Trim(segTotalDetenido) + " s"
            Else
                horas = Trim(minTotalDetenido) + " m " + Trim(segTotalDetenido) + " s"
            End If

            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"
            '------------------------------------------------

            charData += "['Tiempo Motor Apagado', "
            horas = ""
            horMotorApagado = Math.Floor(TiempoMotorApagado / 3600)
            minMotorApagado = Math.Floor((TiempoMotorApagado - horMotorApagado * 3600) / 60)
            segMotorApagado = TiempoMotorApagado - (horMotorApagado * 3600 + minMotorApagado * 60)

            ts = New TimeSpan(0, horMotorApagado, minMotorApagado, segMotorApagado)
            dec = Convert.ToDecimal(ts.TotalHours)
            charData += String.Format("{0:##0.00}", dec).Replace(",", ".") & " ],"
            'charData += " ],"
            strTabla += "</tr><tr><td align=""left"">Tiempo Motor Apagado</td>"

            TiempoMovimiento = 0
            horas = ""
            If horMotorApagado > 0 Then
                horas = Trim(horMotorApagado) + " h " + Trim(minMotorApagado) + " m " + Trim(segMotorApagado) + " s"
            Else
                horas = Trim(minMotorApagado) + " m " + Trim(segMotorApagado) + " s"
            End If

            strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"

        Else
            'MAS DE UN AUTO UN DIA
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    Dim TiempoDetenidoEncendido As Long = 0
                    Dim TiempoMovimiento As Long = 0
                    Dim TiempoMotorApagado As Long = 0
                    Dim TiempoTotalDetenido As Long = 0

                    strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "</th>"
                    'armo el data para el chart

                    _resumenTiempos = clsReporte.ResumenTiempos(CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy") & " " & HoraDesde, CDate(txtFechaHasta.Text).ToString("dd/MM/yyyy") & " " & HoraHasta, veh_id)
                    TiempoMovimiento = _resumenTiempos.Movimiento
                    TiempoMotorApagado = _resumenTiempos.Apagado
                    TiempoDetenidoEncendido = _resumenTiempos.Detenido_Encendido
                    TiempoTotalDetenido = TiempoMotorApagado + TiempoDetenidoEncendido
                    charData += "['" & patente.Text & "',"

                    Dim hor As Integer
                    Dim min As Integer
                    Dim seg As Integer
                    Dim dec As Decimal
                    hor = Math.Floor(TiempoDetenidoEncendido / 3600)
                    min = Math.Floor((TiempoDetenidoEncendido - hor * 3600) / 60)
                    seg = TiempoDetenidoEncendido - (hor * 3600 + min * 60)

                    Dim ts As TimeSpan = New TimeSpan(0, hor, min, seg)
                    dec = Convert.ToDecimal(ts.TotalHours)
                    charData += String.Format("{0:##0.00}", dec).Replace(",", ".") & ","


                    hor = Math.Floor(TiempoTotalDetenido / 3600)
                    min = Math.Floor((TiempoTotalDetenido - hor * 3600) / 60)
                    seg = TiempoTotalDetenido - (hor * 3600 + min * 60)

                    ts = New TimeSpan(0, hor, min, seg)
                    dec = Convert.ToDecimal(ts.TotalHours)

                    charData += String.Format("{0:##0.00}", dec).Replace(",", ".") & ","

                    hor = Math.Floor(TiempoMovimiento / 3600)
                    min = Math.Floor((TiempoMovimiento - hor * 3600) / 60)
                    seg = TiempoMovimiento - (hor * 3600 + min * 60)

                    ts = New TimeSpan(0, hor, min, seg)
                    dec = Convert.ToDecimal(ts.TotalHours)
                    charData += String.Format("{0:##0.00}", dec).Replace(",", ".") & ","

                    hor = Math.Floor(TiempoMotorApagado / 3600)
                    min = Math.Floor((TiempoMotorApagado - hor * 3600) / 60)
                    seg = TiempoMotorApagado - (hor * 3600 + min * 60)

                    ts = New TimeSpan(0, hor, min, seg)
                    dec = Convert.ToDecimal(ts.TotalHours)
                    charData += String.Format("{0:##0.00}", dec).Replace(",", ".") & ","

                    charData += " ],"
                End If
            Next

            strTabla += "</tr>"

            strTabla += "<tr><td align=""left"">Tiempo Detenido Motor encendido</td>"


            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    Dim horas As String = ""
                    Dim TiempoDetenidoEncendido As Long = 0

                    _resumenTiempos = clsReporte.ResumenTiempos(CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy") & " " & HoraDesde, CDate(txtFechaHasta.Text).ToString("dd/MM/yyyy") & " " & HoraHasta, veh_id)

                    TiempoDetenidoEncendido = _resumenTiempos.Detenido_Encendido


                    Dim hor As Integer
                    Dim min As Integer
                    Dim seg As Integer

                    hor = Math.Floor(TiempoDetenidoEncendido / 3600)
                    min = Math.Floor((TiempoDetenidoEncendido - hor * 3600) / 60)
                    seg = TiempoDetenidoEncendido - (hor * 3600 + min * 60)

                    If hor > 0 Then
                        horas = Trim(hor) + " h " + Trim(min) + " m " + Trim(seg) + " s"
                    Else
                        horas = Trim(min) + " m " + Trim(seg) + " s"
                    End If

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"

                End If
            Next

            strTabla += "</tr><tr><td align=""left"">Tiempo Motor Apagado</td>"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    Dim TiempoApagado As Long = 0
                    Dim horas As String = ""
                    _resumenTiempos = clsReporte.ResumenTiempos(CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy") & " " & HoraDesde, CDate(txtFechaHasta.Text).ToString("dd/MM/yyyy") & " " & HoraHasta, veh_id)

                    TiempoApagado = _resumenTiempos.Apagado

                    Dim hor As Integer
                    Dim min As Integer
                    Dim seg As Integer

                    'si la duracion es mas de 24hs voy a mostrar +24hs

                    hor = Math.Floor(TiempoApagado / 3600)
                    min = Math.Floor((TiempoApagado - hor * 3600) / 60)
                    seg = TiempoApagado - (hor * 3600 + min * 60)

                    If hor > 0 Then
                        horas = Trim(hor) + " h " + Trim(min) + " m " + Trim(seg) + " s"
                    Else
                        horas = Trim(min) + " m " + Trim(seg) + " s"
                    End If

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"

                End If

            Next
            strTabla += "</tr>"

            strTabla += "</tr><tr><td align=""left"">Tiempo en Movimiento</td>"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    Dim TiempoMovimiento As Long = 0
                    Dim horas As String = ""
                    _resumenTiempos = clsReporte.ResumenTiempos(CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy") & " " & HoraDesde, CDate(txtFechaHasta.Text).ToString("dd/MM/yyyy") & " " & HoraHasta, veh_id)
                    TiempoMovimiento = _resumenTiempos.Movimiento

                    Dim hor As Integer
                    Dim min As Integer
                    Dim seg As Integer

                    hor = Math.Floor(TiempoMovimiento / 3600)
                    min = Math.Floor((TiempoMovimiento - hor * 3600) / 60)
                    seg = TiempoMovimiento - (hor * 3600 + min * 60)

                    If hor > 0 Then
                        horas = Trim(hor) + " h " + Trim(min) + " m " + Trim(seg) + " s"
                    Else
                        horas = Trim(min) + " m " + Trim(seg) + " s"
                    End If

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"

                End If

            Next
            strTabla += "</tr>"

            strTabla += "<tr><td align=""left"">Tiempo Total Detenido</td>"

            'tiempo total Detenido es la suma de Tiempo detenido motor encendido + Tiempo detenido motor apagado
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                If rdnMovil.Checked Then
                    Dim TiempoMovimiento As Long = 0
                    Dim horas As String = ""
                    _resumenTiempos = clsReporte.ResumenTiempos(CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy") & " " & HoraDesde, CDate(txtFechaHasta.Text).ToString("dd/MM/yyyy") & " " & HoraHasta, veh_id)

                    TiempoMovimiento = _resumenTiempos.Apagado + _resumenTiempos.Detenido_Encendido
                    Dim hor As Integer
                    Dim min As Integer
                    Dim seg As Integer

                    hor = Math.Floor(TiempoMovimiento / 3600)
                    min = Math.Floor((TiempoMovimiento - hor * 3600) / 60)
                    seg = TiempoMovimiento - (hor * 3600 + min * 60)

                    If hor > 0 Then
                        horas = Trim(hor) + " h " + Trim(min) + " m " + Trim(seg) + " s"
                    Else
                        horas = Trim(min) + " m " + Trim(seg) + " s"
                    End If

                    strTabla += "<td align=""center""><table style=""width:100%;"" rules=""cols""><tr><td align=""center"" style=""font-weight:normal;width:50%;"">" & horas & "</td></tr></table></td>"

                End If

            Next
        End If

        strTabla += "</tr>"

        strTabla += "</tbody></table>"

        ltGrillaTiempoHs.Text = strTabla
        PanelEstaditicas.Visible = True
        'si tengo un solo movil muestro grafico de lineas

        If CantMoviles > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoTiempo", " <script> " & getCharBarTiempo(charData) & " </script>")
        Else

            ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoTiempo", " <script> " & getPieChartTiempo(charData) & " </script>")
        End If



    End Sub

    Private Sub EstadisticasAlarmas()

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:11px;width:90%;border-collapse:collapse;"">" & _
                                " <tbody><tr style=""font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:100%;"" colspan=""100%"" align=""center"">Alarmas Reportadas</th></tr>" & _
                                "<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Alarma</th>"

        Dim charData As String = ""
        ltGrillaAlarmas.Text = ""

        'primero los autos
        ' ['Year', 'Sales', 'Expenses'],
        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
            Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
            Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
            If rdnMovil.Checked Then
                strTabla += "<th scope=""col"" align=""center"" style=""font-weight:normal;width:20%;"">" & patente.Text.ToUpper & "</th>"
            End If
        Next

        strTabla += "</tr>"
        '  Inactividad
        'Comienzo de Actividad Diaria

        '  Uso Móvil Fuera Horario
        'Exceso de kms Recorridos
        'Excesos de Velocidad

        'Entrada/Salida Zonas
        'Entrada/Salida Direcciones
        'Desvio Recorridos

        'APARTE ' Sensores

        charData += " ['Alarmas',"
        'Auto1','Auto12','Auto11','Auto3'
        Dim cantAlarma As Integer = 0
        If hdnveh_id.Value <> "0" Then
            Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
            charData += "'" & _movil.veh_patente.ToUpper & "'],"
            'Inactividad

            strTabla += "<tr><td align=""left"">Inactividad </td>"
            charData += "['Inactividad',"

            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 10)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Comienzo de Actividad Diaria</td>"
            charData += "['Comienzo de Actividad Diaria',"


            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 8)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            'Uso Móvil Fuera Horario
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Uso Móvil Fuera Horario</td>"
            charData += "['Uso Móvil Fuera Horario',"
            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 7)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            'Exceso de kms Recorridos
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Exceso de kms Recorridos</td>"
            charData += "['Exceso de kms Recorridos',"

            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 9)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"


            'Excesos de Velocidad
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Exceso de kms Recorridos</td>"
            charData += "['Exceso de Velocidad',"
            cantAlarma = 0

            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 1)

            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            'Entrada/Salida Zonas
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Entrada/Salida Zonas</td>"
            charData += "['Entrada/Salida Zonas',"
            cantAlarma = 0

            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 5)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            'Entrada/Salida Direcciones
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Entrada/Salida Direcciones</td>"
            charData += "['Entrada/Salida Direcciones',"

            cantAlarma = 0
            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 3)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            'Entrada/Salida Direcciones
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Desvio Recorridos</td>"
            charData += "['Desvio Recorridos',"

            cantAlarma = 0

            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 4)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            'Sensores
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Sensores</td>"
            charData += "['Sensores',"

            cantAlarma = 0

            cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), _movil.veh_id, 2)
            charData += cantAlarma.ToString() & "],"
            strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td></tr>"

            charData += "]);"
            strTabla += "</tbody></table>"

        Else '*************** MAS DE UN AUTO UN DIA

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)

                If rdnMovil.Checked Then
                    charData += "'" & patente.Text & "',"
                End If
            Next
            charData += "],"
            'Inactividad

            strTabla += "<tr><td align=""left"">Inactividad </td>"
            charData += "['Inactividad',"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then

                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 10)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next

            charData += "],"
            strTabla += " </tr>"
            'Comienzo de Actividad Diaria
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Comienzo de Actividad Diaria</td>"
            charData += "['Comienzo de Actividad Diaria',"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 8)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next


            charData += "],"
            strTabla += " </tr>"

            'Uso Móvil Fuera Horario
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Uso Móvil Fuera Horario</td>"
            charData += "['Uso Móvil Fuera Horario',"

            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 7)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next


            charData += "],"
            strTabla += " </tr>"

            'Exceso de kms Recorridos
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Exceso de kms Recorridos</td>"
            charData += "['Exceso de kms Recorridos',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 9)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next
            charData += "],"
            strTabla += " </tr>"


            'Excesos de Velocidad
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Exceso de kms Recorridos</td>"
            charData += "['Exceso de Velocidad',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 1)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next
            charData += "],"
            strTabla += " </tr>"


            'Entrada/Salida Zonas
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Entrada/Salida Zonas</td>"
            charData += "['Entrada/Salida Zonas',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 5)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next
            charData += "],"
            strTabla += " </tr>"

            'Entrada/Salida Direcciones
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Entrada/Salida Direcciones</td>"
            charData += "['Entrada/Salida Direcciones',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 3)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next
            charData += "],"
            strTabla += " </tr>"


            'Entrada/Salida Direcciones
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Desvio Recorridos</td>"
            charData += "['Desvio Recorridos',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 4)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next
            charData += "],"
            strTabla += " </tr>"


            'Sensores
            cantAlarma = 0
            strTabla += "<tr><td align=""left"">Sensores</td>"
            charData += "['Sensores',"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                cantAlarma = 0
                If rdnMovil.Checked Then
                    cantAlarma = clsReporte.CantidadAlarmasByVehiculo(CDate(txtFechaDesde.Text & " " & HoraDesde), CDate(txtFechaHasta.Text & " " & HoraHasta), veh_id, 2)
                    charData += cantAlarma.ToString() & ","
                    strTabla += "<td align=""center"">" & cantAlarma.ToString() & "</td>"
                End If
            Next
            charData += "],]);"

            strTabla += "</tr></tbody></table>"

        End If





        ltGrillaAlarmas.Text = strTabla
        PanelEstaditicas.Visible = True
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficoAlarmas", " <script> " & getCharBarAlarmas(charData) & " </script>")



    End Sub


    'INDICADORES

    Private Sub EstadisticasIndicadores(strMoviles As String)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""4"" align=""center"">TEMP</th><th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">RPM</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">BATERIA</th></tr>" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;""></th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"</tr>"
        ltGrillaIndicadores.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia

        Dim _indicadores As New List(Of Indicadores)
        Dim cantMoviles As Integer = 0


        Dim charDataTemp As String = ""
        Dim charDataRPM As String = ""
        Dim charDataBat As String = ""
        'primero los autos
        charDataTemp += "['Patente', 'Max', 'Min','Prom'],"
        charDataRPM += "['Patente', 'Max', 'Min','Prom'],"
        charDataBat += "['Patente', 'Max', 'Min','Prom'],"

        _indicadores = clsReporte.IndicadoresGeneral(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm:ss"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:59"), strMoviles)

        If _indicadores IsNot Nothing Then

            If _indicadores.Count = 0 Then
                strTabla += "<tr><td align=""center"" colspan=""4"">Sin Datos</td></tr>"
            Else
                For Each indicador In _indicadores

                    charDataTemp += "['" & indicador.Patente & "'," & indicador.Temp_Max & "," & indicador.Temp_Min & "," & indicador.Temp_Prom.ToString().Replace(",", ".") & "],"
                    charDataRPM += "['" & indicador.Patente & "'," & indicador.RPM_Max & "," & indicador.RPM_Min & "," & indicador.RPM_Prom.ToString().Replace(",", ".") & "],"
                    charDataBat += "['" & indicador.Patente & "'," & indicador.Bat_Max & "," & indicador.Bat_Min & "," & indicador.Bat_Prom.ToString().Replace(",", ".") & "],"

                    strTabla += "<tr>"
                    strTabla += "<td align=""center"">" & indicador.Patente & "</td>"
                    strTabla += "<td align=""center"">" & indicador.Temp_Max & "</td><td align=""center"">" & indicador.Temp_Min & "</td><td align=""center"">" & indicador.Temp_Prom & "</td>"
                    strTabla += "<td align=""center"">" & indicador.RPM_Max & "</td><td align=""center"">" & indicador.RPM_Min & "</td><td align=""center"">" & indicador.RPM_Prom & "</td>"
                    strTabla += "<td align=""center"">" & indicador.Bat_Max & "</td><td align=""center"">" & indicador.Bat_Min & "</td><td align=""center"">" & indicador.Bat_Prom & "</td>"

                    strTabla += "</tr>"

                Next
            End If
        End If

        charDataTemp += "]);"
        charDataRPM += "]);"
        charDataBat += "]);"


        strTabla += "</tbody></table>"

        ltGrillaIndicadores.Text = strTabla

        'un grafico para cada indicador

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficobarrasTemp", " <script> " & getComboChartIndicadores(charDataTemp, 3, "Temperatura", "ChartBarraTemp") & " </script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficobarrasRPM", " <script> " & getComboChartIndicadores(charDataRPM, 3, "RPM", "ChartBarraRPM") & " </script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficobarrasBat", " <script> " & getComboChartIndicadores(charDataBat, 3, "Bateria", "ChartBarraBat") & " </script>")

    End Sub

    'indicadores detallado

    Private Sub EstadisticasIndicadoresporDia(strMoviles As String, CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""5"" align=""center"">TEMP</th><th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">RPM</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">BATERIA</th></tr>" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;""></th><th scope=""col"" style=""font-weight:normal;width:10%;""></th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"</tr>"
        ltGrillaIndicadoresDetalaldo.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia

        Dim _dias As New List(Of String)
        Dim _rango As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        Dim _kms As New Kms
        'si es el mismo dia pero solo algunas horas el rango va a menor a cero
        If txtFechaHasta.Text = txtFechaDesde.Text Then _rango = 1
        Dim hora As Integer = DateTime.Now.Hour
       
        Dim horas As Double = diferenciaHora(txtFechaDesde.Text & " " & HoraDesde, txtFechaHasta.Text & " " & HoraHasta)

        hora = Math.Floor(horas)

        'si es el mismo dia pero solo algunas horas el rango va a ser menor a cero
        If _rango = 0 Then
            If txtFechaHasta.Text = txtFechaDesde.Text Then
                _rango = 1
            Else
                'si son distintos dias pero el rango de horas es menor a 12 tambien va a dar menor a cero
                If (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays > 0 Then
                    _rango = 1
                End If
            End If
        End If

        'PARA UN DIA UN AUTO agrupo por horas.
        If CantMoviles = 1 And _rango = 1 Then
            'si no es el dia de hoy el que consulta uso la hora hasta las 24

            If txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy") Then
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora 'DateTime.Now.Hour
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            Else
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            End If


        Else
            _dias.Add(txtFechaDesde.Text)
            For i As Integer = 1 To _rango - 1
                _dias.Add(CDate(txtFechaDesde.Text).AddDays(i).ToString("dd/MM/yyyy"))
            Next
        End If


        Dim _indicadores As New List(Of Indicadores)
        If hdnveh_id.Value <> "0" Then

            For Each fecha As String In _dias

                If _rango = 1 Then
                    strTabla += "<tr><td align=""center"" rowspan=""" & (CantMoviles + 1) & """>" & CDate(fecha).ToString("HH:mm") & "</td></tr>"
                    _indicadores = clsReporte.IndicadoresGeneral(CDate(fecha).ToString("yyyyMMdd HH:mm"), CDate(fecha).ToString("yyyyMMdd HH:59:59"), strMoviles)
                Else
                    strTabla += "<tr><td align=""center"" rowspan=""" & (CantMoviles + 1) & """>" & fecha & "</td></tr>"
                    _indicadores = clsReporte.IndicadoresGeneral(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", strMoviles)
                End If


                If _indicadores IsNot Nothing Then

                    If _indicadores.Count = 0 Then
                        strTabla += "<tr><td align=""center"" colspan=""4"">Sin Datos</td></tr>"
                    Else
                        For Each indicador In _indicadores
                            strTabla += "<tr>"
                            strTabla += "<td align=""center"">" & indicador.Patente & "</td>"
                            strTabla += "<td align=""center"">" & indicador.Temp_Max & "</td><td align=""center"">" & indicador.Temp_Min & "</td><td align=""center"">" & indicador.Temp_Prom & "</td>"
                            strTabla += "<td align=""center"">" & indicador.RPM_Max & "</td><td align=""center"">" & indicador.RPM_Min & "</td><td align=""center"">" & indicador.RPM_Prom & "</td>"
                            strTabla += "<td align=""center"">" & indicador.Bat_Max & "</td><td align=""center"">" & indicador.Bat_Min & "</td><td align=""center"">" & indicador.Bat_Prom & "</td>"
                            strTabla += "</tr>"

                        Next
                    End If
                End If

            Next

        Else
            '********MAS DE UNA AUTO Y UN DIA
            For Each fecha As String In _dias
                strTabla += "<tr><td align=""center"" rowspan=""" & (CantMoviles + 1) & """>" & fecha & "</td></tr>"
                _indicadores = clsReporte.IndicadoresGeneral(CDate(fecha).ToString("yyyyMMdd") & " 00:00", CDate(fecha).ToString("yyyyMMdd") & " 23:59:59", strMoviles)

                If _indicadores IsNot Nothing Then

                    If _indicadores.Count = 0 Then
                        strTabla += "<tr><td align=""center"" colspan=""4"">Sin Datos</td></tr>"
                    Else
                        For Each indicador In _indicadores
                            strTabla += "<tr>"
                            strTabla += "<td align=""center"">" & indicador.Patente & "</td>"
                            strTabla += "<td align=""center"">" & indicador.Temp_Max & "</td><td align=""center"">" & indicador.Temp_Min & "</td><td align=""center"">" & indicador.Temp_Prom & "</td>"
                            strTabla += "<td align=""center"">" & indicador.RPM_Max & "</td><td align=""center"">" & indicador.RPM_Min & "</td><td align=""center"">" & indicador.RPM_Prom & "</td>"
                            strTabla += "<td align=""center"">" & indicador.Bat_Max & "</td><td align=""center"">" & indicador.Bat_Min & "</td><td align=""center"">" & indicador.Bat_Prom & "</td>"
                            strTabla += "</tr>"

                        Next
                    End If
                End If

            Next
        End If



        strTabla += "</tbody></table>"

        ltGrillaIndicadoresDetalaldo.Text = strTabla

    End Sub

    Private Sub EstadisticasIndicadoresporSemana(strMoviles As String, CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""5"" align=""center"">TEMP</th><th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">RPM</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">BATERIA</th></tr>" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;""></th><th scope=""col"" style=""font-weight:normal;width:10%;""></th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"</tr>"
        ltGrillaIndicadoresDetalaldo.Text = ""

        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 7)

        Dim _apagado As New Apagado

        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(6).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(7).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next

        Dim _indicadores As New List(Of Indicadores)
        For Each fechas As Semanas In _Semanas
            strTabla += "<tr><td align=""center"" rowspan=""" & (CantMoviles + 1) & """>" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "</td></tr>"
            _indicadores = clsReporte.IndicadoresGeneral(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", strMoviles)

            If _indicadores IsNot Nothing Then

                If _indicadores.Count = 0 Then
                    strTabla += "<tr><td align=""center"" colspan=""4"">Sin Datos</td></tr>"
                Else
                    For Each indicador In _indicadores
                        strTabla += "<tr>"
                        strTabla += "<td align=""center"">" & indicador.Patente & "</td>"
                        strTabla += "<td align=""center"">" & indicador.Temp_Max & "</td><td align=""center"">" & indicador.Temp_Min & "</td><td align=""center"">" & indicador.Temp_Prom & "</td>"
                        strTabla += "<td align=""center"">" & indicador.RPM_Max & "</td><td align=""center"">" & indicador.RPM_Min & "</td><td align=""center"">" & indicador.RPM_Prom & "</td>"
                        strTabla += "<td align=""center"">" & indicador.Bat_Max & "</td><td align=""center"">" & indicador.Bat_Min & "</td><td align=""center"">" & indicador.Bat_Prom & "</td>"
                        strTabla += "</tr>"

                    Next
                End If
            End If

        Next

        strTabla += "</tbody></table>"

        ltGrillaIndicadoresDetalaldo.Text = strTabla

    End Sub

    Private Sub EstadisticasIndicadoresporMes(strMoviles As String, CantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""5"" align=""center"">TEMP</th><th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">RPM</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:30%;"" colspan=""3"" align=""center"">BATERIA</th></tr>" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;""></th><th scope=""col"" style=""font-weight:normal;width:10%;""></th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"<th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Max.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Min.</th><th scope=""col"" style=""font-weight:normal;width:10%;"" align=""center"">Prom.</th>" & _
"</tr>"
        ltGrillaIndicadoresDetalaldo.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia
        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 30)

        Dim _apagado As New Apagado

        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(30).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(30).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next

        Dim _indicadores As New List(Of Indicadores)
        For Each fechas As Semanas In _Semanas
            strTabla += "<tr><td align=""center"" rowspan=""" & (CantMoviles + 1) & """>" & GetMonth(CDate(fechas.fecha_hasta).Month) & "</td></tr>"
            _indicadores = clsReporte.IndicadoresGeneral(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", strMoviles)

            If _indicadores IsNot Nothing Then

                If _indicadores.Count = 0 Then
                    strTabla += "<tr><td align=""center"" colspan=""4"">Sin Datos</td></tr>"
                Else
                    For Each indicador In _indicadores
                        strTabla += "<tr>"
                        strTabla += "<td align=""center"">" & indicador.Patente & "</td>"
                        strTabla += "<td align=""center"">" & indicador.Temp_Max & "</td><td align=""center"">" & indicador.Temp_Min & "</td><td align=""center"">" & indicador.Temp_Prom & "</td>"
                        strTabla += "<td align=""center"">" & indicador.RPM_Max & "</td><td align=""center"">" & indicador.RPM_Min & "</td><td align=""center"">" & indicador.RPM_Prom & "</td>"
                        strTabla += "<td align=""center"">" & indicador.Bat_Max & "</td><td align=""center"">" & indicador.Bat_Min & "</td><td align=""center"">" & indicador.Bat_Prom & "</td>"
                        strTabla += "</tr>"

                    Next
                End If
            End If

        Next

        strTabla += "</tbody></table>"

        ltGrillaIndicadoresDetalaldo.Text = strTabla

    End Sub

    'Sensores General
    Private Sub EstadisticasSensores()

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;""></th>"


        'busco los sensores que tenga en el sistema
        Dim _sensores As List(Of Sensores) = clsCategoriaAlarma.ListSensoresAlarma()
        Dim charData As String = ""
        Dim cantMoviles As Integer = 0
        'primero los autos
        charData += "['Patente',"
        For Each _sensor In _sensores
            strTabla += "<th scope=""col"" style=""font-weight:normal;width:20%;"" align=""center"">" & _sensor.sen_nombre & "</th>"
            charData += "'" & _sensor.sen_nombre & "',"
        Next

        charData += " ],"
        strTabla += "</tr>"
        ltGrillaSensores.Text = ""
        'busco para cada auto la cantidad por cada sensor para la fecha elegida
        If hdnveh_id.Value <> "0" Then
            Dim _moviles As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
            strTabla += "<tr>"
            charData += "['" & _moviles.veh_patente.ToUpper() & "',"
            strTabla += "<td align=""center"">" & _moviles.veh_patente.ToUpper() & "</td>"
            For Each _sensor In _sensores
                Dim cantidad As Integer = clsReporte.SensoresGeneral(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm:ss"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:ss"), _moviles.veh_id, _sensor.sen_id)
                charData += cantidad & ","
                strTabla += "<td align=""center"">" & cantidad & "</td>"
            Next
            charData += "],"
            strTabla += "</tr>"

        Else
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                If rdnMovil.Checked Then
                    cantMoviles += 1
                    strTabla += "<tr>"
                    charData += "['" & patente.Text & "',"
                    strTabla += "<td align=""center"">" & patente.Text & "</td>"
                    For Each _sensor In _sensores
                        Dim cantidad As Integer = clsReporte.SensoresGeneral(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm:ss"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:ss"), veh_id, _sensor.sen_id)
                        charData += cantidad & ","
                        strTabla += "<td align=""center"">" & cantidad & "</td>"
                    Next
                    charData += "],"
                    strTabla += "</tr>"

                End If
            Next
        End If


        charData += "]);"


        strTabla += "</tbody></table>"

        ltGrillaSensores.Text = strTabla



        ClientScript.RegisterClientScriptBlock(Me.GetType(), "graficobarrasSensores", " <script> " & getComboChartSensores(charData, _sensores.Count, "Sensores", "CharSensores") & " </script>")

    End Sub

    'Sensores detallado
    Private Sub EstadisticasSensoresDia(cantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th><th scope=""col"" style=""font-weight:normal;width:10%;""></th>"


        'busco los sensores que tenga en el sistema
        Dim _sensores As List(Of Sensores) = clsCategoriaAlarma.ListSensoresAlarma()

        'primero los autos

        For Each _sensor In _sensores
            strTabla += "<th scope=""col"" style=""font-weight:normal;width:20%;"" align=""center"">" & _sensor.sen_nombre & "</th>"
        Next

        strTabla += "</tr>"
        ltGrillaSensoresDetallado.Text = ""

        'armo los datos del grafico
        'tengo que buscar las fechas del periodo y buscar las velocidades maximas para cada movil elegido para ese dia

        Dim _dias As New List(Of String)
        Dim _rango As Integer = (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays
        'si es el mismo dia pero solo algunas horas el rango va a menor a cero
        If txtFechaHasta.Text = txtFechaDesde.Text Then _rango = 1
        Dim hora As Integer = DateTime.Now.Hour
        Dim horas As Double = diferenciaHora(txtFechaDesde.Text & " " & HoraDesde, txtFechaHasta.Text & " " & HoraHasta)

        hora = Math.Floor(horas)

        'si es el mismo dia pero solo algunas horas el rango va a ser menor a cero
        If _rango = 0 Then
            If txtFechaHasta.Text = txtFechaDesde.Text Then
                _rango = 1
            Else
                'si son distintos dias pero el rango de horas es menor a 12 tambien va a dar menor a cero
                If (CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays > 0 Then
                    _rango = 1
                End If
            End If
        End If

        'PARA UN DIA UN AUTO agrupo por horas.
        If cantMoviles = 1 And _rango = 1 Then
            'si no es el dia de hoy el que consulta uso la hora hasta las 24

            If txtFechaDesde.Text = DateTime.Now.ToString("dd/MM/yyyy") Then
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            Else
                _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).ToString("dd/MM/yyyy HH:00"))
                For i As Integer = 1 To hora
                    _dias.Add(DateTime.Parse(txtFechaDesde.Text & " " & HoraDesde).AddHours(i).ToString("dd/MM/yyyy HH:00"))
                Next
            End If


        Else
            _dias.Add(txtFechaDesde.Text)
            For i As Integer = 1 To _rango - 1
                _dias.Add(CDate(txtFechaDesde.Text).AddDays(i).ToString("dd/MM/yyyy"))
            Next
        End If


        'busco para cada auto la cantidad por cada sensor para la fecha elegida
        If hdnveh_id.Value <> "0" Then

            Dim _moviles As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
            For Each fecha As String In _dias
                If _rango = 1 Then
                    strTabla += "<tr><td align=""center"" rowspan=""" & (cantMoviles + 1) & """>" & CDate(fecha).ToString("HH:mm") & "</td></tr>"
                Else
                    strTabla += "<tr><td align=""center"" rowspan=""" & (cantMoviles + 1) & """>" & fecha & "</td></tr>"
                End If

                strTabla += "<tr>"

                strTabla += "<td align=""center"">" & _moviles.veh_patente.ToUpper() & "</td>"
                For Each _sensor In _sensores
                    Dim cantidad As Integer = 0
                    If _rango = 1 Then
                        cantidad = clsReporte.SensoresGeneral(CDate(fecha).ToString("yyyyMMdd HH:mm"), CDate(fecha).ToString("yyyyMMdd HH:59:59"), _moviles.veh_id, _sensor.sen_id)
                    Else

                        cantidad = clsReporte.SensoresGeneral(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm:ss"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:ss"), _moviles.veh_id, _sensor.sen_id)

                    End If
                    strTabla += "<td align=""center"">" & cantidad & "</td>"
                Next

                strTabla += "</tr>"


            Next
        Else
            For Each fecha As String In _dias
                strTabla += "<tr><td align=""center"" rowspan=""" & (cantMoviles + 1) & """>" & fecha & "</td></tr>"
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                    Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                    If rdnMovil.Checked Then

                        strTabla += "<tr>"

                        strTabla += "<td align=""center"">" & patente.Text & "</td>"
                        For Each _sensor In _sensores
                            Dim cantidad As Integer = clsReporte.SensoresGeneral(CDate(txtFechaDesde.Text & " " & HoraDesde).ToString("yyyyMMdd HH:mm:ss"), CDate(txtFechaHasta.Text & " " & HoraHasta).ToString("yyyyMMdd HH:mm:ss"), veh_id, _sensor.sen_id)

                            strTabla += "<td align=""center"">" & cantidad & "</td>"
                        Next

                        strTabla += "</tr>"

                    End If
                Next
            Next
        End If



        strTabla += "</tbody></table>"

        ltGrillaSensoresDetallado.Text = strTabla


    End Sub


    Private Sub EstadisticasSensoreSemana(cantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th><th scope=""col"" style=""font-weight:normal;width:10%;""></th>"


        'busco los sensores que tenga en el sistema
        Dim _sensores As List(Of Sensores) = clsCategoriaAlarma.ListSensoresAlarma()
        'primero los autos

        For Each _sensor In _sensores
            strTabla += "<th scope=""col"" style=""font-weight:normal;width:20%;"" align=""center"">" & _sensor.sen_nombre & "</th>"
        Next

        strTabla += "</tr>"
        ltGrillaSensoresDetallado.Text = ""

        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 7)

        Dim _apagado As New Apagado

        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(6).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(7).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next
        'busco para cada auto la cantidad por cada sensor para la fecha elegida

        For Each fechas As Semanas In _Semanas
            strTabla += "<tr><td align=""center"" rowspan=""" & (cantMoviles + 1) & """>" & CDate(fechas.fecha_desde).ToString("dd-MM") & "/" & CDate(fechas.fecha_hasta).ToString("dd-MM") & "</td></tr>"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                If rdnMovil.Checked Then

                    strTabla += "<tr>"

                    strTabla += "<td align=""center"">" & patente.Text & "</td>"
                    For Each _sensor In _sensores
                        Dim cantidad As Integer = clsReporte.SensoresGeneral(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id, _sensor.sen_id)

                        strTabla += "<td align=""center"">" & cantidad & "</td>"
                    Next

                    strTabla += "</tr>"

                End If
            Next
        Next

        strTabla += "</tbody></table>"

        ltGrillaSensoresDetallado.Text = strTabla


    End Sub

    Private Sub EstadisticasSensoresMes(cantMoviles As Integer)

        Dim strTabla As String = "<table cellspacing=""0"" cellpadding=""4"" align=""Center"" border=""1""  style=""color:Black;background-color:White;border-color:#999999;border-width:1px;border-style:Solid;font-size:10px;width:95%;border-collapse:collapse;"">" & _
                                   " <tbody><tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;"">" & _
"<tr style=""color:White;background-color:#343535;font-size:10pt;font-weight:normal;""><th scope=""col"" style=""font-weight:normal;width:10%;"">Periodo</th><th scope=""col"" style=""font-weight:normal;width:10%;""></th>"


        'busco los sensores que tenga en el sistema
        Dim _sensores As List(Of Sensores) = clsCategoriaAlarma.ListSensoresAlarma()
        'primero los autos

        For Each _sensor In _sensores
            strTabla += "<th scope=""col"" style=""font-weight:normal;width:20%;"" align=""center"">" & _sensor.sen_nombre & "</th>"
        Next

        strTabla += "</tr>"
        ltGrillaSensoresDetallado.Text = ""

        Dim _Semanas As New List(Of Semanas)
        Dim _rango As Integer = Math.Ceiling((CDate(txtFechaHasta.Text & " " & HoraHasta) - CDate(txtFechaDesde.Text & " " & HoraDesde)).TotalDays / 30)

        Dim _apagado As New Apagado

        Dim _semana As New Semanas
        _semana.fecha_desde = CDate(txtFechaDesde.Text).ToString("dd/MM/yyyy")
        _semana.fecha_hasta = CDate(txtFechaDesde.Text).AddDays(30).ToString("dd/MM/yyyy")

        _Semanas.Add(_semana)
        For i As Integer = 1 To _rango - 1
            _semana = New Semanas
            _semana.fecha_desde = CDate(_Semanas(i - 1).fecha_hasta).AddDays(1).ToString("dd/MM/yyyy")
            _semana.fecha_hasta = CDate(_Semanas(i - 1).fecha_hasta).AddDays(30).ToString("dd/MM/yyyy")

            _Semanas.Add(_semana)
        Next
        'busco para cada auto la cantidad por cada sensor para la fecha elegida

        For Each fechas As Semanas In _Semanas
            strTabla += "<tr><td align=""center"" rowspan=""" & (cantMoviles + 1) & """>" & GetMonth(CDate(fechas.fecha_hasta).Month) & "</td></tr>"
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                Dim veh_id As Integer = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                Dim patente As Label = DirectCast(row.FindControl("lblPatente"), Label)
                If rdnMovil.Checked Then

                    strTabla += "<tr>"

                    strTabla += "<td align=""center"">" & patente.Text & "</td>"
                    For Each _sensor In _sensores
                        Dim cantidad As Integer = clsReporte.SensoresGeneral(CDate(fechas.fecha_desde).ToString("yyyyMMdd") & " 00:00", CDate(fechas.fecha_hasta).ToString("yyyyMMdd") & " 23:59:59", veh_id, _sensor.sen_id)

                        strTabla += "<td align=""center"">" & cantidad & "</td>"
                    Next

                    strTabla += "</tr>"

                End If
            Next
        Next

        strTabla += "</tbody></table>"

        ltGrillaSensoresDetallado.Text = strTabla


    End Sub

    Private Shared Function diferenciaHora(fecha_desde As String, fecha_hasta As String) As Double
        Dim inicio As DateTime = DateTime.Parse(fecha_desde)

        'EJECUCIÓN de un proceso

        Dim final As DateTime = DateTime.Parse(fecha_hasta)
        Dim duracion As TimeSpan = final - inicio
        Dim segundosTotales As Double = duracion.TotalHours

        Return segundosTotales

    End Function

    Private Function GetMonth(nroMes As String) As String
        Select Case nroMes
            Case "1"
                Return "Enero"
            Case "2"
                Return "Febrero"
            Case "3"
                Return "Marzo"
            Case "4"
                Return "Abril"
            Case "5"
                Return "Mayo"
            Case "6"
                Return "Junio"
            Case "7"
                Return "Julio"
            Case "8"
                Return "Agosto"
            Case "9"
                Return "Septiembre"
            Case "10"
                Return "Octubre"
            Case "11"
                Return "Noviembre"
            Case "12"
                Return "Diciembre"
        End Select
    End Function

    Private Function getCantDiasMes(mes As String) As Integer
        Dim cant As Integer = 31

        Select Case mes
            Case "4"
                cant = 30
            Case "6"
                cant = 30
            Case "9"
                cant = 30
            Case "11"
                cant = 30
            Case "2"
                cant = 28

        End Select
        Return cant
    End Function

    Protected Sub ButtonReporte_Click(sender As Object, e As EventArgs) Handles ButtonReporte.Click
        'voy a la pagina de reportes, le paso los autos y la fecha que selecciono

        For Each row As DataListItem In DataListVehiculos.Items
            Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

            If rdnMovil.Checked Then
                If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                strMoviles = strMoviles + DataListVehiculos.DataKeys(row.ItemIndex).ToString()

            End If

        Next

        'si esta consultado desde el flotante asigno al parametro el movil preseleccionado
        If strMoviles = "" Then strMoviles = hdnveh_id.Value

        If hdnOrigen.Value = "" Then
            If ddlhoraDesde.SelectedValue <> "" Then
                HoraDesde = ddlhoraDesde.SelectedValue
            Else
                HoraDesde = "00"
            End If

            If ddlMinDesde.SelectedValue <> "" Then
                HoraDesde += ":" & ddlMinDesde.SelectedValue
            Else
                HoraDesde += ":00"
            End If

            If HoraHasta = "" Then
                If ddlHoraHasta.SelectedValue <> "" Then
                    HoraHasta = ddlHoraHasta.SelectedValue
                Else
                    'si el dia es menor al actual la hora es hasta las 23
                    If CDate(txtFechaHasta.Text) < DateTime.Now Then
                        HoraHasta = "23"
                    Else
                        HoraHasta = DateTime.Now.ToString("HH")
                    End If

                End If

                If ddlMinHasta.SelectedValue <> "" Then
                    HoraHasta += ":" & ddlMinHasta.SelectedValue & ":59"
                Else
                    'si el dia es menor al actual la hora es hasta las 23
                    If CDate(txtFechaHasta.Text) < DateTime.Now Then
                        HoraHasta += ":59:59"
                    Else
                        HoraHasta += ":" & DateTime.Now.ToString("mm")
                    End If

                End If
            End If
        End If




        Response.Redirect("pReportes.aspx?origen=estadisticas&recorridos=1&fechaDesde=" & txtFechaDesde.Text & " " & HoraDesde & "&fechaHasta=" & txtFechaHasta.Text & " " & HoraHasta & "&moviles=" & strMoviles, False)

    End Sub

    Protected Sub rdnPeriodo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdnPeriodo.SelectedIndexChanged
        If rdnPeriodo.SelectedValue = "2" Or rdnPeriodo.SelectedValue = 3 Then
            ButtonReporte.Visible = False
        Else
            ButtonReporte.Visible = True
        End If
    End Sub
End Class



Public Class Semanas

    Public Property fecha_desde() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property fecha_hasta() As String
        Get
            Return m_fecha_hasta
        End Get
        Set(value As String)
            m_fecha_hasta = value
        End Set
    End Property
    Private m_fecha_hasta As String

End Class

Public Class TotalesSemanas

    Public Property fecha_desde() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property fecha_hasta() As String
        Get
            Return m_fecha_hasta
        End Get
        Set(value As String)
            m_fecha_hasta = value
        End Set
    End Property
    Private m_fecha_hasta As String

    Public Property total() As Decimal
        Get
            Return m_total
        End Get
        Set(value As Decimal)
            m_total = value
        End Set
    End Property
    Private m_total As Decimal

End Class