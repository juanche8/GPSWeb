'Pagina para generar los reprotes del sistema
Imports GPS.Business
Imports GPS.Data
Imports Microsoft.Reporting.WebForms
Partial Public Class pReportes
    Inherits System.Web.UI.Page
    '
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    Dim cli_id As Integer = DirectCast(Session("Cliente"), Integer)
                    hdncli_id.Value = cli_id.ToString()
                    'verifico si quiere ver solo reportes de un vehiculo
                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()
                        PanelCustomizado.Visible = False
                        PanelMoviles.Visible = False
                        PanelPatente.Visible = True
                        lblPatente.Text = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value)).veh_patente
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
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador."
        End Try
    End Sub


    'REPORTE CUSTOMIZADO
    Protected Sub btnReporteCustom_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReporteCustom.Click
        Try
            'Reporte con campos dinamicos, voy a evaluar que campos mostrar
            Dim sSelect As String = ""
            Dim sWhere As String = " WHERE cli_id = " + hdncli_id.Value
            Dim veh_id As String = "0"
            Dim sRowNumber = ""
            Dim sHoraDesde As String = "00:00:00"
            Dim sHoraHasta As String = "23:59:59"

            'fecha por default un mes
            Dim sFechaDesde As String = DateTime.Now.AddMonths(-1).ToString("dd-MM-yyyy")
            Dim sFechaHasta As String = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy")
            'campos a mostrar

            'agrego este campo siempre porque tengo un grupo 
            If chkPatente.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","
                sSelect = sSelect + "veh_patente"
                sRowNumber = sRowNumber + "a"
            End If
           

            If chkFecha.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","

                sSelect = sSelect + "Fecha"
                sRowNumber = sRowNumber + "b"

            End If

            If chkHora.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","

                sSelect = sSelect + "Hora"
                sRowNumber = sRowNumber + "c"

            End If



            If chkDireccion.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","
                sSelect = sSelect + "Nombre_via"
                sRowNumber = sRowNumber + "d"
            End If

            If chkLocalidad.Checked Then

                If sSelect.Length > 0 Then sSelect = sSelect + ","
                sSelect = sSelect + "Localidad"
                sRowNumber = sRowNumber + "e"

            End If

            If chkProvincia.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","

                sSelect = sSelect + "Provincia"
                sRowNumber = sRowNumber + "f"
            End If

            If chkVelocidad.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","

                sSelect = sSelect + "Velocidad"
                sRowNumber = sRowNumber + "g"

            End If

            If ChkKms.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","

                sSelect = sSelect + "Kms_Recorridos"
                sRowNumber = sRowNumber + "h"

            End If

            If ChkAlertas.Checked Then
                If sSelect.Length > 0 Then sSelect = sSelect + ","

                sSelect = sSelect + "  (SELECT alar_nombre FROM Alarmas WHERE (veh_id = Vehiculos.veh_id) AND (alar_fecha = vMonitoreos.FECHA) AND (alar_hora = CONVERT(time,vMonitoreos.FECHA,108))) AS alertas "
                sRowNumber = sRowNumber + "i"

            End If

            If sSelect = "" Then
                sSelect = " * , (SELECT alar_nombre FROM Alarmas WHERE (veh_id = Vehiculos.veh_id) AND (alar_fecha = vMonitoreos.FECHA) AND (alar_hora = CONVERT(time,vMonitoreos.FECHA,108))) AS alertas "
                sRowNumber = ", 'abcdefghi'" + " AS RowNum"
            Else
                'verifico si elgio la patente
                If Not chkPatente.Checked Then
                    If sSelect.Length > 0 Then sSelect = sSelect + ","
                    sSelect = sSelect + "veh_patente"
                    sRowNumber = sRowNumber + "a"
                End If

                sRowNumber = "," + "'" + sRowNumber + "' AS RowNum"
            End If

            'verifico si viene con el parametro de un solo vehiculo
            If hdnveh_id.Value <> "" Then

                veh_id = hdnveh_id.Value
                'verifico si elgio filtro de fecha para todos los moviles
                If txtFechaDesde.Text <> "" Then
                    sWhere = sWhere + " AND Fecha >= '" + txtFechaDesde.Text + " " + ddlhoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + "'"
                    sFechaDesde = txtFechaDesde.Text
                End If

                If txtFechaHasta.Text <> "" Then
                    sFechaHasta = txtFechaHasta.Text
                    sWhere = sWhere + " AND Fecha <= '" + txtFechaHasta.Text + " " + ddlHoraHasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + "'"
                End If

                'sHoraDesde = ddlhoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue
                'sWhere = sWhere + " AND Hora >= '" + sHoraDesde + "'"

                'sHoraHasta = ddlHoraHasta.SelectedValue + ":" + ddlMinHasta.SelectedValue
                'sWhere = sWhere + " AND Hora <= '" + sHoraHasta + "'"

            Else

            'Condicion de filtro
            If Not chkTodos.Checked Then

                'busco en el datalist de vehiculos si marco alguno
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As RadioButton = DirectCast(row.FindControl("rdnMovil"), RadioButton)
                    If rdnMovil.Checked Then
                        veh_id = DataListVehiculos.DataKeys(row.ItemIndex).ToString()
                        'verifico si eligio fecha y hora
                        Dim txtFechaDesde As TextBox = DirectCast(row.FindControl("txtFechaDesde"), TextBox)
                        Dim txtFechaHasta As TextBox = DirectCast(row.FindControl("txtFechaHasta"), TextBox)
                            Dim ddlHoraDesde As DropDownList = DirectCast(row.FindControl("ddlhoraDesde"), DropDownList)
                            Dim ddlHoraHasta As DropDownList = DirectCast(row.FindControl("ddlHoraHasta"), DropDownList)
                            Dim ddlMinDesde As DropDownList = DirectCast(row.FindControl("ddlMinDesde"), DropDownList)
                            Dim ddlMinHasta As DropDownList = DirectCast(row.FindControl("ddlMinHasta"), DropDownList)

                        If txtFechaDesde.Text <> "" Then
                            sFechaDesde = txtFechaDesde.Text
                                sWhere = sWhere + " AND Fecha >= '" + txtFechaDesde.Text + " " + ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + "'"
                        End If


                        If txtFechaHasta.Text <> "" Then
                            sFechaHasta = txtFechaHasta.Text
                                sWhere = sWhere + " AND Fecha <= '" + txtFechaHasta.Text + " " + ddlHoraHasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + "'"
                        End If

                            'sHoraDesde = ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue
                            'sWhere = sWhere + " AND Hora >= '" + sHoraDesde + "'"

                            'sHoraHasta = ddlHoraHasta.SelectedValue + ":" + ddlMinHasta.SelectedValue
                            'sWhere = sWhere + " AND Hora <= '" + sHoraHasta + "'"

                        End If
                Next

                If Not veh_id = "" Then
                    sWhere = sWhere + " AND veh_id = " + veh_id
                End If
            Else
                'verifico si elgio filtro de fecha para todos los moviles
                If txtFechaDesde.Text <> "" Then
                    sWhere = sWhere + " AND Fecha >= '" + txtFechaDesde.Text + "'"
                        sFechaDesde = txtFechaDesde.Text
                    Else
                        sWhere = sWhere + " AND Fecha >= '" + sFechaDesde + "'"
                    End If


                If txtFechaHasta.Text <> "" Then
                    sFechaHasta = txtFechaHasta.Text
                        sWhere = sWhere + " AND Fecha <= '" + txtFechaHasta.Text + " " + ddlhoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + "'"
                    Else
                        sWhere = sWhere + " AND Fecha <= '" + sFechaHasta + " " + ddlHoraHasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + "'"
                    End If

                    'sHoraDesde = ddlhoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue
                    'sWhere = sWhere + " AND Hora >= '" + sHoraDesde + "'"

                    'sHoraHasta = ddlHoraHasta.SelectedValue + ":" + ddlMinHasta.SelectedValue
                    'sWhere = sWhere + " AND Hora <= '" + sHoraHasta + "'"



            End If
            End If



            'Le indicamos al Control que la invocacion del reporte sera de modo remoto
            ReportViewer1.ProcessingMode = ProcessingMode.Remote
            ' ReportViewer1.ProcessingMode = ProcessingMode.Local
            'Le indicamos la URL donde se encuentra hospedado Reporting Services

            Dim reportserver As String = System.Configuration.ConfigurationManager.AppSettings("ReportServer")

            ReportViewer1.ServerReport.ReportServerUrl = New Uri(reportserver)

            'verifico si eligio reporte completo o solo kms recorridos
            If rdnTipoReporte.SelectedValue = 1 Then
                ReportViewer1.ServerReport.ReportPath = "/rptCustomizadoListadoCompleto" '
                Dim parametros As New List(Of ReportParameter)

                ''Creo cada uno de los parametros: 1-id de parametro, 2-Valor, 3-Visible*/

                parametros.Add(New ReportParameter("Select", sSelect + sRowNumber, False))
                parametros.Add(New ReportParameter("Where", sWhere, False))
                'Paso los parametros al reporte*/                 
                ReportViewer1.ServerReport.SetParameters(parametros)
            Else
                If rdnTipoReporte.SelectedValue = 2 Then
                    ReportViewer1.ServerReport.ReportPath = "/rptCustomizadoKms"
                    Dim parametros As New List(Of ReportParameter)
                    parametros.Add(New ReportParameter("fechadesde", sFechaDesde + " " + sHoraDesde, False))
                    parametros.Add(New ReportParameter("fechahasta", sFechaHasta + " " + sHoraHasta, False))
                    parametros.Add(New ReportParameter("veh_id", veh_id, False))

                    ReportViewer1.ServerReport.SetParameters(parametros)
                Else
                    'alarmas

                    ReportViewer1.ServerReport.ReportPath = "/rptAlarmasRutina"
                    Dim parametros As New List(Of ReportParameter)
                    parametros.Add(New ReportParameter("FechaDesde", sFechaDesde + " " + sHoraDesde, False))
                    parametros.Add(New ReportParameter("FechaHasta", sFechaHasta + " " + sHoraHasta, False))
                    parametros.Add(New ReportParameter("HoraDesde", sHoraDesde, False))
                    parametros.Add(New ReportParameter("HoraHasta", sHoraHasta, False))

                    'verifico si eligio todos los vehiculos
                    If veh_id = "0" Then
                        veh_id = ""
                        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(hdncli_id.Value)
                        For Each movil As Vehiculo In vehiculos

                            If veh_id.Length > 0 Then veh_id = veh_id + "|"
                            veh_id = veh_id + movil.veh_id.ToString()

                        Next
                    End If

                    parametros.Add(New ReportParameter("veh_id", veh_id, False))

                    ReportViewer1.ServerReport.SetParameters(parametros)
                End If


            End If

            ReportViewer1.ServerReport.Refresh()
            ReportViewer1.ZoomPercent = 90
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ' Dim deviceInfo As String = ""
            ' deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat> <PageWidth>21cm</PageWidth>  <PageHeight>29.7cm</PageHeight> <MarginTop>0.5cm</MarginTop>  <MarginLeft>1.1cm</MarginLeft> <MarginRight>0.5cm</MarginRight> <MarginBottom>0.5cm</MarginBottom></DeviceInfo>"

            '  helper.ExportPdf(ReportViewer1, deviceInfo)

        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el reporte customizado. Contacte al Administrador."
        End Try

    End Sub

   
    Protected Sub datalist_ItemDataBound(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim strFunction As String = "SetUniqueRadioButton('" + DataListVehiculos.ClientID + "_ctl00_rdnMovil',this)"
            Dim rdnmovil As RadioButton = DirectCast(e.Item.FindControl("rdnMovil"), RadioButton)
            rdnmovil.Attributes.Add("onclick", strFunction)
        End If

    End Sub


    'REPORTE RUTINA
    Protected Sub btnReporteRutinaKms_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReporteRutinaKms.Click
        Try
            'reporte diario solo para el dia de hoy- completo
            Dim strNombreReporte = "/rptRutinaDiarioKmsRecorridos"
            Dim strMoviles As String = ""
            Dim horaDesde As String = "00:00:00"
            Dim horaHasta As String = "23:59:59"

            Dim FechaDesde As String = DateTime.Now.ToString("dd-MM-yyyy")
            Dim FechaHasta As String = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy")

            If rdnFrecuencia.SelectedValue = 2 Then
                'asigno al rango de fechas una semana
                FechaDesde = DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy")
                strNombreReporte = "/rptRutinaKmsRecorridos"
            End If

            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                'verifico si eligio algun vehiculo, sino tengo que marcar todos
                If Not chkTodos1.Checked Then
                    'busco en el datalist de vehiculos si marco alguno
                    For Each row As DataListItem In DataListVehiculos1.Items
                        Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                        If rdnMovil.Checked Then
                            If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                            If rdnMovil.Checked Then strMoviles = strMoviles + DataListVehiculos1.DataKeys(row.ItemIndex).ToString()
                        End If

                    Next


                    If strMoviles.Length = 0 Then
                        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(hdncli_id.Value)
                        For Each movil As Vehiculo In vehiculos

                            If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                            strMoviles = strMoviles + movil.veh_id.ToString()

                        Next
                    End If
                Else
                    Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(hdncli_id.Value)
                    For Each movil As Vehiculo In vehiculos

                        If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                        strMoviles = strMoviles + movil.veh_id.ToString()

                    Next
                End If

                End If
                'cargo el reporte de rutina de acuerdo a la opcion elegida
                'Le indicamos al Control que la invocacion del reporte sera de modo remoto
                ReportViewer1.ProcessingMode = ProcessingMode.Remote
                ' ReportViewer1.ProcessingMode = ProcessingMode.Local

                Dim reportserver As String = System.Configuration.ConfigurationManager.AppSettings("ReportServer")
                'Le indicamos la URL donde se encuentra hospedado Reporting Services
                ReportViewer1.ServerReport.ReportServerUrl = New Uri(reportserver)

                'Le indicamos la carpeta y el Reporte que deseamos Ver

                ReportViewer1.ServerReport.ReportPath = strNombreReporte
                ReportViewer1.ZoomPercent = 75

                Dim parametros As New List(Of ReportParameter)

                'Creo cada uno de los parametros: 1-id de parametro, 2-Valor, 3-Visible*/
            parametros.Add(New ReportParameter("fechadesde", FechaDesde + " " + horaDesde, False))
            parametros.Add(New ReportParameter("fechahasta", FechaHasta + " " + horaHasta, False))
                parametros.Add(New ReportParameter("veh_id", strMoviles, False))

                'Paso los parametros al reporte*/    
                ReportViewer1.ServerReport.SetParameters(parametros)
                ReportViewer1.ServerReport.Refresh()


        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el Reporte de Kms Recorridos. Contacte al Administrador."
        End Try

    End Sub

    Protected Sub btnReporteRutinaCompleto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReporteRutinaCompleto.Click
        Try
            'reporte diario solo para el dia de hoy- completo
            Dim strMoviles As String = ""
               Dim horaDesde As String = "00:00:00"
            Dim horaHasta As String = "23:59:59"

            Dim FechaDesde As String = DateTime.Now.ToString("dd-MM-yyyy")
            Dim FechaHasta As String = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy")

            If rdnFrecuencia.SelectedValue = 2 Then
                'asigno al rango de fechas una semana
                FechaDesde = DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy")
            End If

            'verifico si eligio algun vehiculo, sino tengo que marcar todos

            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                If Not chkTodos1.Checked Then
                    'busco en el datalist de vehiculos si marco alguno
                    For Each row As DataListItem In DataListVehiculos1.Items
                        Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                        If rdnMovil.Checked Then
                            If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                            If rdnMovil.Checked Then strMoviles = strMoviles + DataListVehiculos1.DataKeys(row.ItemIndex).ToString()
                        End If

                    Next


                    If strMoviles.Length = 0 Then
                        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(hdncli_id.Value)
                        For Each movil As Vehiculo In vehiculos

                            If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                            strMoviles = strMoviles + movil.veh_id.ToString()

                        Next
                    End If
                Else
                    Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(hdncli_id.Value)
                    For Each movil As Vehiculo In vehiculos

                        If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                        strMoviles = strMoviles + movil.veh_id.ToString()

                    Next
                End If
            End If


            'cargo el reporte de rutina de acuerdo a la opcion elegida
            'Le indicamos al Control que la invocacion del reporte sera de modo remoto
            ReportViewer1.ProcessingMode = ProcessingMode.Remote
            ' ReportViewer1.ProcessingMode = ProcessingMode.Local


            Dim reportserver As String = System.Configuration.ConfigurationManager.AppSettings("ReportServer")
            'Le indicamos la URL donde se encuentra hospedado Reporting Services
            ReportViewer1.ServerReport.ReportServerUrl = New Uri(reportserver)

            'Le indicamos la carpeta y el Reporte que deseamos Ver

            ReportViewer1.ServerReport.ReportPath = "/rptRutinaCompleto"


            Dim parametros As New List(Of ReportParameter)

            'Creo cada uno de los parametros: 1-id de parametro, 2-Valor, 3-Visible*/
            parametros.Add(New ReportParameter("fechadesde", FechaDesde + " " + horaDesde, False))
            parametros.Add(New ReportParameter("fechahasta", FechaHasta + " " + horaHasta, False))
          parametros.Add(New ReportParameter("veh_id", strMoviles, False))

            'Paso los parametros al reporte*/    
            ReportViewer1.ServerReport.SetParameters(parametros)
            ReportViewer1.ServerReport.Refresh()


        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el Reporte de Rutina. Contacte al Administrador."
        End Try

    End Sub

    Protected Sub rdnTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdnTipoReporte.SelectedIndexChanged

        ReportViewer1.Reset()
        'desabilito el panel de campos si eligio reporte customizado total kms
        If rdnTipoReporte.SelectedValue = 2 Or rdnTipoReporte.SelectedValue = 3 Then
            PanelCampos.Visible = False
        Else
            PanelCampos.Visible = True
        End If
    End Sub

    Protected Sub btnReporteAlertas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReporteAlertas.Click
        'ejecuto el reporte de alarmas
        Try
            ReportViewer1.Reset()
            'reporte diario solo para el dia de hoy- completo
            Dim strNombreReporte = "/rptAlarmasRutina"
            Dim strMoviles As String = ""
            Dim horaDesde As String = "00:00:00"
            Dim horaHasta As String = "23:59:59"

            Dim FechaDesde As String = DateTime.Now.ToString("dd-MM-yyyy")
            Dim FechaHasta As String = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy")

            If rdnFrecuencia.SelectedValue = 2 Then
                'asigno al rango de fechas una semana
                FechaDesde = DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy")

            End If

            If hdnveh_id.Value <> "" Then
                strMoviles = hdnveh_id.Value
            Else
                'verifico si eligio algun vehiculo, sino tengo que marcar todos
                If Not chkTodos1.Checked Then
                    'busco en el datalist de vehiculos si marco alguno
                    For Each row As DataListItem In DataListVehiculos1.Items
                        Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                        If rdnMovil.Checked Then
                            If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                            If rdnMovil.Checked Then strMoviles = strMoviles + DataListVehiculos1.DataKeys(row.ItemIndex).ToString()
                        End If

                    Next


                    If strMoviles.Length = 0 Then
                        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(hdncli_id.Value)
                        For Each movil As Vehiculo In vehiculos

                            If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                            strMoviles = strMoviles + movil.veh_id.ToString()

                        Next
                    End If
                Else
                    Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(hdncli_id.Value)
                    For Each movil As Vehiculo In vehiculos

                        If strMoviles.Length > 0 Then strMoviles = strMoviles + "|"
                        strMoviles = strMoviles + movil.veh_id.ToString()

                    Next
                End If
            End If

            'cargo el reporte de rutina de acuerdo a la opcion elegida
            'Le indicamos al Control que la invocacion del reporte sera de modo remoto
            ReportViewer1.ProcessingMode = ProcessingMode.Remote
            ' ReportViewer1.ProcessingMode = ProcessingMode.Local


            Dim reportserver As String = System.Configuration.ConfigurationManager.AppSettings("ReportServer")
            'Le indicamos la URL donde se encuentra hospedado Reporting Services
            ReportViewer1.ServerReport.ReportServerUrl = New Uri(reportserver)

            'Le indicamos la carpeta y el Reporte que deseamos Ver

            ReportViewer1.ServerReport.ReportPath = "/rptAlarmasRutina"

            Dim parametros As New List(Of ReportParameter)

            'Creo cada uno de los parametros: 1-id de parametro, 2-Valor, 3-Visible*/
            parametros.Add(New ReportParameter("FechaDesde", FechaDesde, False))
            parametros.Add(New ReportParameter("FechaHasta", FechaHasta, False))
            parametros.Add(New ReportParameter("HoraDesde", horaDesde, False))
            parametros.Add(New ReportParameter("HoraHasta", horaHasta, False))
            parametros.Add(New ReportParameter("veh_id", strMoviles, False))

            'Paso los parametros al reporte*/    
            ReportViewer1.ServerReport.SetParameters(parametros)
            ReportViewer1.ServerReport.Refresh()


        Catch ex As Exception
            Funciones.WriteToEventLog(" REPORTES -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error generando el Reporte de Alertas. Contacte al Administrador."
        End Try
    End Sub

    Protected Sub DesmarcarTodo(ByVal sender As Object, ByVal e As EventArgs)
        chkTodos1.Checked = False
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        If PanelCustom.Visible = False Then
            PanelCustom.Visible = True
            PanelRutina.Visible = False
        Else
            PanelCustom.Visible = False
            PanelRutina.Visible = True
        End If

    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton2.Click
        If PanelRutina.Visible = False Then
            PanelRutina.Visible = True
            PanelCustom.Visible = False
        Else
            PanelRutina.Visible = False
            PanelCustom.Visible = True
        End If
    End Sub
End Class

'Api de codificacion geografica de google
'https://developers.google.com/maps/documentation/geocoding/?hl=es#ReverseGeocoding