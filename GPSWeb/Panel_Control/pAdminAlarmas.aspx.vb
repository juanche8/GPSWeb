Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Partial Public Class pAdminAlarmas2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Session.Remove("Filtro")
            'cargo el listado de vehiculos y por cada uno tengo que buscar si tienen configurada la alarma
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then

                    hdncli_id.Value = Session("Cliente").ToString()
                    CargarGrilla()
                End If
                'verifico si me llega el parametro de la alarma que hay que borrar.
                If Request.Params("alar_id") IsNot Nothing Then
                    EliminarAlarma(Request.Params("alar_id").ToString())
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
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
            If e.ColumnIndex = 9 Then
                Dim valores As String() = e.RowValues(6).ToString().Split("|")
                e.CellHtml = "<a title='Desactivar Alarma' onclick='activaDesacAlertaZ(" + valores(1) + "," + e.RowValues(0).ToString() + ",0);' href='#' >Desactivar</a>"

            End If
        Else
            If e.ColumnIndex = 9 Then
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
            If e.ColumnIndex = 8 Then
                Dim valores As String() = e.RowValues(6).ToString().Split("|")
                e.CellHtml = "<a title='Desactivar Alarma' onclick='activaDesacAlertaD(" + valores(1) + "," + valores(0) + ",0);' href='#' >Desactivar</a>"

            End If
        Else
            If e.ColumnIndex = 8 Then
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


            End Select


        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - Eliminar alarmas de Zona, direccion o recorrido " + ex.Message + " - " + ex.StackTrace)
        End Try
        Response.Redirect("~/Panel_Control/pAdminAlarmas.aspx", False)

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

    End Sub

    'Protected Sub btnVelocidad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVelocidad.Click
    '    Try
    '        'verifico si eligio Deshabiltiarla, entonces la elimino
    '        If chkBorrar.Checked Then
    '            clsAlarma.DeleteByMovil(CInt(hdnMovil.Value), CInt(hdnsubcat_id.Value))
    '        Else
    '            'elimino la configuracion anterior para volver a crearla
    '            clsAlarma.DeleteByMovil(CInt(hdnMovil.Value), CInt(hdnsubcat_id.Value))
    '            'guardo la alarma de exceso de velocidad

    '            Dim alertaUsuario As Alertas_Velocidad_Configuradas = New Alertas_Velocidad_Configuradas()

    '            alertaUsuario.veh_id = CInt(hdnMovil.Value)
    '            alertaUsuario.ale_fecha_creacion = DateTime.Now
    '            alertaUsuario.vel_id = CInt(hdnsubcat_id.Value)

    '            If chkSMS.Checked Then
    '                alertaUsuario.ale_enviar_SMS = True
    '            Else
    '                alertaUsuario.ale_enviar_SMS = False
    '            End If

    '            If chkMail.Checked Then
    '                alertaUsuario.ale_enviar_mail = True
    '            Else
    '                alertaUsuario.ale_enviar_mail = False
    '            End If

    '            alertaUsuario.ale_valor_maximo = txtValor.Text

    '            clsAlarma.InsertAlarmaConfigurada(alertaUsuario)
    '        End If

    '        CargarGrilla()
    '    Catch ex As Exception
    '        Funciones.WriteToEventLog("ADD ALARMAS VELOCIDAD - " + ex.Message + " - " + ex.StackTrace)
    '    End Try
    'End Sub

   
 
     
End Class