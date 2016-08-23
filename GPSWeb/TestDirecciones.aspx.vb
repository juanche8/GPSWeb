Imports GPS.Business
Imports GPS.Data
Imports GPSDataOSM

Public Class TestDirecciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            clsFunciones.Send_Mail_Alarma_Clientes("Prueba", "nattyco@gmail.com")
            lblResultado.Text = "Mail enviado"

        Catch ex As Exception
            lblResultado.Text = "Error enviando mail. " & ex.ToString()
        End Try

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sError = ""
        Dim direccion As GPSDataOSM.Direccione = clsOpenStreet.getdireccion(txtLatitud.Text, txtLongitud.Text, sError)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sError = ""
        Dim direccion As String = ""
        ' clsGoogle.getdireccion(txtLatitud.Text, txtLongitud.Text, sError)

        Dim kms_recorridos As Double = Distance(Decimal.Parse("-34.6018519"), Decimal.Parse("-58.4158012166667"), Decimal.Parse("-34.6017956333333"), Decimal.Parse("-58.4158492666667"))

        ' Dim _vehiculos As List(Of Alarmas_Inactividad) = clsAlarma.AlertaInactividadList()
        'For Each _alerta As Alarmas_Inactividad In _vehiculos
        'Dim fecha_actual As DateTime = DateTime.Now

        'Dim _registro As vMonitoreos = clsVehiculo.searchLastLocation(_alerta.veh_id)
        'If (_registro.VELOCIDAD < 20) Then
        'Dim _movil As Vehiculos = clsVehiculo.Seleccionar(_alerta.veh_id)
        ' clsAlarma.VerificarInactividad(_registro, _movil, _alerta)

        ' clsAlarma.FinInactividad(_movil, _alerta, _registro)

        'End If

        'Next





    End Sub

    'alarma que se disparo a las 17:51 de entrada a la zona Acoyte y rivadavia se disparo mal
    'probe con los puntos y la distancia al poligono me da 500 metros
    'el umbral es 300
    'despues 17:59 disparo de nuevo la alarma de entrada a acoyte y rivadavia
    'ahi si esta dentro del umbral
    'pero no se porque la disparo si no disparo antes la salida
    ''disparo tambien la salida de direccion de ambrosetti 801-899
    'pero no disparo la llegada
    ' no me esta tirando el recorrido del modulo 10003 desde las 17 a las 18
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'calcular zona
        Dim _registro As vMonitoreo = clsVehiculo.SeleccionarPosicion(268598)

        ' Dim _movil As Vehiculos = clsVehiculo.Seleccionar(13)

        ' Dim _previusRegistro As vMonitoreos = clsVehiculo.SeleccionarPosicion(252225)

        'Dim alerta As Alertas_Zonas = clsAlarma.SelectAlertaZonaById(34) ' Sale zona parque centenario

        'clsAlarma.VerificarZona(_registro, _previusRegistro, _movil, alerta)

        'alerta = clsAlarma.SelectAlertaZonaById(36) ' Entra zona parque centenario

        'clsAlarma.VerificarZona(_registro, _previusRegistro, _movil, alerta)

        'Dim alerta As Alertas_Zonas = clsAlarma.SelectAlertaZonaById(35) ' Entra zona Acoyte y Rivadavia

        'clsAlarma.VerificarZona(_registro, _previusRegistro, _movil, alerta)

        ' Dim alertaR As Alertas_Recorridos = clsAlarma.AlertaRecorridoSelect(19) 'salida parque centenario

        ' clsAlarma.VerificarRecorrido(_registro, _previusRegistro, _movil, alertaR)

        ' Dim _recorridos As List(Of vMonitoreos) = clsReporte.RecorridosRutinaOld("20150330 21:10:00", "20150330 21:13:10", "10002")

        'Dim _recorridos As List(Of vMonitoreos) = clsReporte.RecorridosRutinaOld("02/10/2014 17:34:20", "02/10/2014 17:34:50", "10003")
        'tengo inactividad el 7/10 entre las 10:16:04

        'For Each _ubicacion As vMonitoreos In _recorridos
        ' clsAlarma.verificarAlertas(_ubicacion.Codigo)
        ' Next

    End Sub

    Private Function Distance(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double) As Double
        Dim distancia As Double = 0
        Dim deg2radMultiplier As Double = Math.PI / 180
        lat1 = lat1 * deg2radMultiplier
        lon1 = lon1 * deg2radMultiplier
        lat2 = lat2 * deg2radMultiplier
        lon2 = lon2 * deg2radMultiplier

        Dim radius As Double = 6378.137 ' earth mean radius defined by WGS84
        Dim dlon As Double = lon2 - lon1
        distancia = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(dlon)) * radius


        Return distancia
    End Function

End Class