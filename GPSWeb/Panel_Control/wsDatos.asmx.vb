Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports GPS.Business
Imports GPS.Data

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la siguiente línea.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class wsDatos
    Inherits System.Web.Services.WebService


    <WebMethod()> _
    Public Function GetVehiculosUbicacion(ByVal cli_id As Integer) As List(Of VehiculoItem)
        Dim Items As New List(Of VehiculoItem)
        Try
            'recuepro la ultima ubicacion de los vehiculos del cliente logeado.

            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)
            Dim icono As String = ""
            For Each movil As Vehiculo In vehiculos

                'busco la ultima ubicacion del movil
                Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationValidByVehiculo(movil.veh_id)

                If ubicacion IsNot Nothing Then
                    'segun el tipo de vehiculo asigno el icono
                    'y verifico tambien la orientacion
                    ' 1 norte, 2 sur, 3 este, 4 oeste, 5 NE, 6 N0, 7 SE, 8 SO
                    Dim orientacion As String = ""
                    icono = iconoMovil(movil.veh_tipo_id, ubicacion.ORIENTACION, orientacion, ubicacion.ESTADO)

                    'verifico si hay un marcador cerca de la ubicacion
                    'marcadores creados por el cliente o marcadores creados por el administrador
                    Dim smarcador As String = ""

                    If ubicacion.LATITUD <> "" And ubicacion.LONGITUD <> "" Then
                        Dim marcador_cliente As Marcadores = clsMarcador.SelectByLatLng(movil.cli_id, ubicacion.LATITUD.Substring(0, 7), ubicacion.LONGITUD.Substring(0, 7))
                        Dim marcador As Marcadores_Generico = clsMarcador.SelectGenericoByLatLng(movil.cli_id, ubicacion.LATITUD.Substring(0, 7), ubicacion.LONGITUD.Substring(0, 7))

                        If marcador_cliente IsNot Nothing Then smarcador = marcador_cliente.marc_nombre

                        If marcador IsNot Nothing Then smarcador += " " + marcador_cliente.marc_nombre
                    End If

                    Items.Add(New VehiculoItem With {.nombre = movil.veh_descripcion, _
                                                    .id = movil.mod_id, _
                                                    .conductor = movil.veh_nombre_conductor, _
                                                    .modulo = movil.mod_id, _
                                                    .patente = movil.veh_patente, _
                                                     .ubicacion = ubicacion.NOMBRE_VIA + "," + ubicacion.LOCALIDAD, _
                                                    .velocidad = String.Format("{0:###,00}", ubicacion.VELOCIDAD), _
                                                    .foto = movil.veh_imagen, _
                                                      .icono = icono, _
                                                      .orientacion = orientacion, _
                                                     .hora = ubicacion.FECHA.ToString("HH:mm:ss"), _
                                                     .fecha = ubicacion.FECHA.ToString("dd/MM/yyyy"), _
                                                     .marcador = smarcador, _
                                                    .lng = ubicacion.LONGITUD, _
                                                    .lat = ubicacion.LATITUD})
                End If
            Next

        Catch ex As Exception

        End Try
        Return Items
    End Function


    <WebMethod()> _
    Public Function GetVehiculosUbicacionGrupo(ByVal grup_id As Integer) As List(Of VehiculoItem)
        Dim Items As New List(Of VehiculoItem)
        Try
            'recuepro la ultima ubicacion de los vehiculos del cliente logeado.

            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivosGrupo(grup_id, "")
            Dim icono As String = ""
            For Each movil As Vehiculo In vehiculos

                'busco la ultima ubicacion del movil
                Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationValidByVehiculo(movil.veh_id)

                If ubicacion IsNot Nothing Then
                    'segun el tipo de vehiculo asigno el icono
                    'y verifico tambien la orientacion
                    ' 1 norte, 2 sur, 3 este, 4 oeste, 5 NE, 6 N0, 7 SE, 8 SO
                    Dim orientacion As String = ""
                    icono = iconoMovil(movil.veh_tipo_id, ubicacion.ORIENTACION, orientacion, ubicacion.ESTADO)

                    'verifico si hay un marcador cerca de la ubicacion
                    'marcadores creados por el cliente o marcadores creados por el administrador
                    Dim marcador_cliente As Marcadores = clsMarcador.SelectByLatLng(movil.cli_id, ubicacion.LATITUD.Substring(0, 7), ubicacion.LONGITUD.Substring(0, 7))
                    Dim marcador As Marcadores_Generico = clsMarcador.SelectGenericoByLatLng(movil.cli_id, ubicacion.LATITUD.Substring(0, 7), ubicacion.LONGITUD.Substring(0, 7))

                    Dim smarcador As String = ""

                    If marcador_cliente IsNot Nothing Then smarcador = marcador_cliente.marc_nombre

                    If marcador IsNot Nothing Then smarcador += " " + marcador_cliente.marc_nombre

                    Items.Add(New VehiculoItem With {.nombre = movil.veh_descripcion, _
                                                    .id = movil.mod_id, _
                                                    .conductor = movil.veh_nombre_conductor, _
                                                    .modulo = movil.mod_id, _
                                                    .patente = movil.veh_patente, _
                                                     .ubicacion = ubicacion.NOMBRE_VIA + "," + ubicacion.LOCALIDAD, _
                                                    .velocidad = String.Format("{0:###,00}", ubicacion.VELOCIDAD), _
                                                    .foto = movil.veh_imagen, _
                                                      .icono = icono, _
                                                      .orientacion = orientacion, _
                                                     .hora = ubicacion.FECHA.ToString("HH:mm:ss"), _
                                                     .fecha = ubicacion.FECHA.ToString("dd/MM/yyyy"), _
                                                     .marcador = smarcador, _
                                                    .lng = ubicacion.LONGITUD, _
                                                    .lat = ubicacion.LATITUD})
                End If
            Next

        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetUbicacion(ByVal veh_id As Integer) As List(Of VehiculoItem)
        Dim Items As New List(Of VehiculoItem)
        Try
            'recuepro la ultima ubicacion de los vehiculos del cliente logeado.

            Dim movil As Vehiculo = clsVehiculo.Seleccionar(veh_id)
            Dim icono As String = ""
            Dim orientacion As String = ""
            'busco la ultima ubicacion del movil
            Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationValidByVehiculo(movil.veh_id)

            If ubicacion IsNot Nothing Then
                'segun el tipo de vehiculo asigno el icono
                icono = iconoMovil(movil.veh_tipo_id, ubicacion.ORIENTACION, orientacion, ubicacion.ESTADO)

                Items.Add(New VehiculoItem With {.nombre = movil.veh_descripcion, _
                                                .id = movil.mod_id, _
                                                .patente = movil.veh_patente, _
                                                .ubicacion = ubicacion.NOMBRE_VIA + "," + ubicacion.LOCALIDAD, _
                                                .velocidad = String.Format("{0:###,00}", ubicacion.VELOCIDAD), _
                                                .foto = movil.veh_imagen, _
                                                  .icono = icono, _
                                                 .hora = ubicacion.FECHA.ToString("HH:mm:ss"), _
                                                 .fecha = ubicacion.FECHA.ToString("dd/MM/yyyy"), _
                                                .lng = ubicacion.LONGITUD, _
                                                .orientacion = orientacion, _
                                                .lat = ubicacion.LATITUD})
            End If


        Catch ex As Exception

        End Try
        Return Items
    End Function

    Private Function iconoMovil(ByVal tipo_movil As Integer, ByVal orientacion As String, ByRef ori As String, ByVal estado As String) As String
        Dim icono As String = ""

        Dim _tipo As Tipos_Vehiculos = clsParametros.TipoVehiculoSelect(tipo_movil)
        Dim imagen As String = _tipo.veh_tipo_icono.Substring(0, _tipo.veh_tipo_icono.LastIndexOf("."))
        ' 1 norte, 2 sur, 3 este, 4 oeste, 5 NE, 6 N0, 7 SE, 8 SO
        Select Case orientacion
            Case "1"
                icono = imagen & "-n.png"
                If estado = "1" Then icono = imagen & "_norte_v.png"
                If estado = "2" Then icono = imagen & "-n.png"
                If estado = "3" Then icono = imagen & "_norte_a.png"
                ori = "N"
            Case "2"
                icono = imagen & "-s.png"
                If estado = "1" Then icono = imagen & "_sur_v.png"
                If estado = "2" Then icono = imagen & "-s.png"
                If estado = "3" Then icono = imagen & "_sur_a.png"
                ori = "S"
            Case "3"
                icono = imagen & "-e.png"
                If estado = "1" Then icono = imagen & "_este_v.png"
                If estado = "2" Then icono = imagen & "-e.png"
                If estado = "3" Then icono = imagen & "_este_a.png"
                ori = "E"
            Case "4"
                icono = imagen & "-o.png"
                If estado = "1" Then icono = imagen & "_oeste_v.png"
                If estado = "2" Then icono = imagen & "-o.png"
                If estado = "3" Then icono = imagen & "_oeste_a.png"
                ori = "O"
            Case "5"
                icono = imagen & "-ne.png"
                If estado = "1" Then icono = imagen & "_noreste_v.png"
                If estado = "2" Then icono = imagen & "-ne.png"
                If estado = "3" Then icono = imagen & "_noreste_a.png"
                ori = "NE"
            Case "6"
                icono = imagen & "-no.png"
                If estado = "1" Then icono = imagen & "_noroeste_v.png"
                If estado = "2" Then icono = imagen & "-no.png"
                If estado = "3" Then icono = imagen & "_noroeste_a.png"
                ori = "NO"
            Case "7"
                icono = imagen & "-se.png"
                If estado = "1" Then icono = imagen & "_sureste_v.png"
                If estado = "2" Then icono = imagen & "-se.png"
                If estado = "3" Then icono = imagen & "_sureste_a.png"
                ori = "SE"
            Case "8"
                icono = imagen & "-so.png"
                If estado = "1" Then icono = imagen & "_suroeste_v.png"
                If estado = "2" Then icono = imagen & "-so.png"
                If estado = "3" Then icono = imagen & "_suroeste_a.png"
                ori = "SO"
            Case Else
                icono = _tipo.veh_tipo_icono
                ori = ""
        End Select



        Return icono
    End Function

    <WebMethod()> _
    Public Function GetVehiculos(ByVal cli_id As Integer) As List(Of VehiculoItem)
        Dim Items As New List(Of VehiculoItem)
        Try
            'recupero todos los vehiculos del cliente

            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.List(cli_id)
            Dim icono As String = ""
            For Each movil As Vehiculo In vehiculos

                Items.Add(New VehiculoItem With {.nombre = movil.veh_descripcion, _
                                                .id = movil.veh_id, _
                                                .patente = movil.veh_patente, _
                                                .ubicacion = "", _
                                                .velocidad = "", _
                                                .foto = "", _
                                                  .icono = icono, _
                                                 .hora = "", _
                                                 .fecha = "", _
                                                .lng = "", _
                                                .lat = ""})
            Next


        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetVehiculosActivos(ByVal cli_id As Integer) As List(Of VehiculoItem)
        Dim Items As New List(Of VehiculoItem)
        Try
            'recupero todos los vehiculos del cliente

            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)
            Dim icono As String = ""
            For Each movil As Vehiculo In vehiculos

                Items.Add(New VehiculoItem With {.nombre = movil.veh_descripcion, _
                                                .id = movil.veh_id, _
                                                .patente = movil.veh_patente, _
                                                .ubicacion = "", _
                                                .velocidad = "", _
                                                .foto = "", _
                                                  .icono = icono, _
                                                 .hora = "", _
                                                 .fecha = "", _
                                                .lng = "", _
                                                .lat = ""})
            Next


        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetVehiculosActivosTop(ByVal cli_id As Integer) As List(Of VehiculoItem)
        Dim Items As New List(Of VehiculoItem)
        Try
            'recupero todos los vehiculos del cliente

            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivosTop(cli_id) 'para seguimientos solo 9
            Dim icono As String = ""
            For Each movil As Vehiculo In vehiculos

                Items.Add(New VehiculoItem With {.nombre = movil.veh_descripcion, _
                                                .id = movil.veh_id, _
                                                .patente = movil.veh_patente, _
                                                .ubicacion = "", _
                                                .velocidad = "", _
                                                .foto = "", _
                                                  .icono = icono, _
                                                 .hora = "", _
                                                 .fecha = "", _
                                                .lng = "", _
                                                .lat = ""})
            Next


        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetAlarmas(ByVal cli_id As Integer) As List(Of AlarmaItem)
        Dim Items As New List(Of AlarmaItem)
        Try


            ' Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertasLast(cli_id)
            'busco las ultimas alarmas de los ultimos 10 min
            Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertasNoVista(cli_id, DateTime.Now.AddMinutes(-10), DateTime.Now)

            Dim icono As String = ""
            For Each alerta As Alarmas In alarmas

                Items.Add(New AlarmaItem With {.id = alerta.alar_id, _
                                                .alarma = alerta.alar_nombre, _
                                                .categoria = alerta.alar_Categoria, _
                                                .conductor = alerta.veh_conductor, _
                                               .ubicacion = alerta.alar_nombre_via, _
                                                .velocidad = alerta.alar_valor, _
                                                .patente = alerta.veh_patente, _
                                               .lat = alerta.alar_lat + "," + alerta.alar_lng, _
                                               .lng = alerta.alar_lng, _
                                                .fecha = alerta.alar_fecha.ToString("dd/MM/yyyy"), _
                                                .hora = alerta.alar_fecha.ToString("HH:mm:ss")})
            Next


        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetDataAlarma(ByVal veh_id As Integer, ByVal subcat_id As Integer) As List(Of AlarmaConfiguradaItem)
        Dim Items As New List(Of AlarmaConfiguradaItem)
        Try
            'recupero todos los vehiculos del cliente

            Dim movil As Vehiculo = clsVehiculo.Seleccionar(veh_id)

            Dim alertaConfig As Alertas_Velocidad_Configuradas = clsAlarma.SelectByMovil(veh_id, subcat_id)

            Dim alerta As Alarmas_Velocidad = clsCategoriaAlarma.SelectAlarmaVelocidad(subcat_id)
            'verifico si tengo condifurada esta alarma
            If alertaConfig IsNot Nothing Then
                Items.Add(New AlarmaConfiguradaItem With {.ale_id = alertaConfig.ale_id.ToString(), _
                                           .alarma = alertaConfig.Alarmas_Velocidad.vel_descripcion, _
                                           .patente = movil.veh_patente + "-" + movil.veh_descripcion, _
                                           .valor = alertaConfig.ale_valor_maximo, _
                                           .enviarMail = alertaConfig.ale_enviar_mail.ToString(), _
                                           .enviarSMS = alertaConfig.ale_enviar_SMS.ToString(), _
                                            .configurada = True, _
                                           .veh_id = movil.veh_id.ToString()})

            Else
                Items.Add(New AlarmaConfiguradaItem With {.ale_id = 0, _
                                           .alarma = alerta.vel_descripcion, _
                                           .patente = movil.veh_patente + "-" + movil.veh_descripcion, _
                                           .valor = alerta.vel_valor_por_defecto, _
                                           .enviarMail = "false", _
                                           .enviarSMS = "false", _
                                           .configurada = False, _
                                           .veh_id = movil.veh_id.ToString()})

            End If




        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetAlarmasPanico() As List(Of AlarmaItem)
        Dim Items As New List(Of AlarmaItem)
        Try
            'recupero todos los vehiculos del cliente

            Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertasPanico()

            Dim icono As String = ""
            For Each alerta As Alarmas In alarmas
                'busco el cliente a traves del movil
                Dim movil As Vehiculo = clsVehiculo.Seleccionar(alerta.veh_id.Value)
                Items.Add(New AlarmaItem With {.id = alerta.alar_id, _
                                               .cliente = movil.Cliente.cli_apellido + " " + movil.Cliente.cli_nombre, _
                                                .ubicacion = alerta.alar_nombre_via + "," + alerta.alar_Localidad + "," + alerta.alar_Provincia, _
                                                .patente = alerta.veh_patente, _
                                                .alarma = alerta.alar_nombre, _
                                                .fecha = alerta.alar_fecha.ToString("dd/MM/yyyy"), _
                                                .hora = alerta.alar_fecha.ToString("HH:mm:ss")})
            Next


        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function visarAlarma(ByVal alar_id As Integer) As Boolean

        Try
            'marco la alarma como ya vista para que no aparesca en el pop up

            clsAlarma.visarAlarma(alar_id)


        Catch ex As Exception

        End Try
        Return True
    End Function

    <WebMethod()> _
    Public Function ocultarAlarma(ByVal alar_id As Integer) As Boolean

        Try
            'marco la alarma como ya vista para que no aparesca en el pop up

            clsAlarma.ocultarAlarma(alar_id)


        Catch ex As Exception

        End Try
        Return True
    End Function

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

    <WebMethod(EnableSession:=True)> _
    Public Function activaDesacAlertaR(ByVal id As Integer, ByVal estado As String, ByVal veh_id As Integer) As Boolean

        Try
            'marco la alarma como activa o desactiva
            clsAlarma.AlertaRecorridoActDesact(id, CBool(estado))

            Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)

            page.Session.Remove("AlarmasRecorridoList")
            gAlarmasRecorridos(veh_id)
        Catch ex As Exception

        End Try
        Return True
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function activaDesacAlertaD(ByVal id As Integer, ByVal estado As String, ByVal veh_id As Integer) As Boolean

        Try
            'marco la alarma como activa o desactiva
            clsAlarma.AlertaDireccionActDesact(id, CBool(estado))

            System.Web.HttpContext.Current.Session.Remove("AlarmasDireccList")

            gAlarmasDirecciones(veh_id)
        Catch ex As Exception

        End Try
        Return True
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function activaDesacAlertaZ(ByVal id As Integer, ByVal estado As String, ByVal veh_id As Integer) As Boolean

        Try
            'marco la alarma como activa o desactiva
            clsAlarma.AlertaZonaActDesact(id, estado)

            System.Web.HttpContext.Current.Session.Remove("AlarmasZonaList")
            gAlarmasZonas(veh_id)
        Catch ex As Exception

        End Try
        Return True
    End Function

    <WebMethod()> _
    Public Function visarAlarmaPanico(ByVal alar_id As Integer) As Boolean

        Try
            'marco la alarma como ya vista para que no aparesca en el pop up

            clsAlarma.visarAlarmaAdmin(alar_id)


        Catch ex As Exception

        End Try
        Return True
    End Function

    <WebMethod()> _
    Public Function GetRecorridos(ByVal cli_id As Integer, ByVal veh_id As Integer, ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal hora_desde As String, ByVal hora_hasta As String) As List(Of VehiculoItem)
        Dim Items As New List(Of VehiculoItem)
        Try


            If hora_desde = ":" Then hora_desde = "00:00:00"
            If hora_hasta = ":" Then hora_hasta = "23:59:59"
            If hora_hasta = "00:00" Then hora_hasta = "23:59:59"

            If fecha_desde = "" Then
                fecha_desde = DateTime.Now.ToString("yyyyMMdd")
            Else
                fecha_desde = CDate(fecha_desde).ToString("yyyyMMdd")
            End If


            If fecha_hasta = "" Then
                fecha_hasta = DateTime.Now.ToString("yyyyMMdd")
            Else
                fecha_hasta = CDate(fecha_hasta).ToString("yyyyMMdd")
            End If


            Dim cant As Integer = 0
            Dim movil As Vehiculo = clsVehiculo.Seleccionar(veh_id)
            Dim icono_marca As String = ""
            Dim _nombre_alarma As String = ""
            Dim recordCount As Integer = clsReporte.RecorridosRutinaTotal(fecha_desde + " " + hora_desde, fecha_hasta + " " + hora_hasta, veh_id)
            Dim PageCount As Integer = 1
            If recordCount > 1000 Then
                PageCount = recordCount / 1000
            End If
            Dim resto As Integer = 0
            resto = recordCount Mod 1000
            If resto > 0 Then PageCount = PageCount + 1
            'voy a buscar los recorridos por pagina y los junto

            Dim j As Integer = 1
            Do While (j <= PageCount)
                Dim recorridos As List(Of vMonitoreo) = clsReporte.RecorridosRutinaMapa(fecha_desde + " " + hora_desde, fecha_hasta + " " + hora_hasta, veh_id, j, 1000, " FECHA DESC ")
                For Each r As vMonitoreo In recorridos
                    'verifico si en vMonitoreos punto se produjo una alarma
                    _nombre_alarma = ""
                    icono_marca = ""
                    Dim estado As String = ""

                    If r.ESTADO IsNot Nothing Then estado = r.ESTADO
                    Dim _alarma As Alarmas = clsVehiculo.GetAlarma(veh_id, r.LATITUD, r.LONGITUD)

                    ' para el primer recorrido voy a poner un icono verde y para el ultimo un icono rojo para decir donde empezo y termino
                    If cant = 0 Then
                        icono_marca = "../images/flechas_recorridos/marcador_puntollegada.png"
                    Else
                        If cant = recordCount - 1 Then
                            icono_marca = "../images/flechas_recorridos/marcador_puntopartida.png"
                        Else
                            If _alarma IsNot Nothing Then
                                icono_marca = "../images/flechas_recorridos/recorrido_alerta.png"
                                _nombre_alarma = _alarma.alar_nombre
                            Else
                                icono_marca = GetOrientacion(r.ORIENTACION.ToString(), estado)
                            End If
                        End If

                    End If


                    Items.Add(New VehiculoItem With {.id = r.ID_MODULO, _
                                                     .patente = movil.veh_patente, _
                                                     .nombre = movil.veh_descripcion, _
                                                  .ubicacion = r.NOMBRE_VIA + "," + r.LOCALIDAD, _
                                                  .velocidad = String.Format("{0:###,00}", r.VELOCIDAD), _
                                                   .hora = r.FECHA.ToString("HH:mm:ss"), _
                                                   .fecha = r.FECHA.ToString("dd/MM/yyyy"), _
                                                     .alarma = _nombre_alarma, _
                                                  .lng = r.LONGITUD, _
                                                     .icono = icono_marca, _
                                                  .lat = r.LATITUD})
                    cant = cant + 1
                Next
                j = j + 1
            Loop

        Catch ex As Exception
            Items.Add(New VehiculoItem With {.id = 0, _
                                                     .patente = "", _
                                                     .nombre = ex.Message, _
                                                  .ubicacion = ex.StackTrace, _
                                                  .velocidad = "", _
                                                   .hora = "", _
                                                   .fecha = "", _
                                                     .alarma = "", _
                                                  .lng = "", _
                                                     .icono = "", _
                                                  .lat = ""})
        End Try



        Return Items
    End Function

    Private Function GetOrientacion(ByVal orientacion As String, ByVal estado As String) As String
        Dim icono As String = ""


        '1 -En Movimiento - Verde
        '2 -Motor Apagado - rojo
        '3-Detenido en okm - amarillo

        ' 1 norte, 2 sur, 3 este, 4 oeste, 5 NE, 6 N0, 7 SE, 8 SO
        Select Case orientacion
            Case "1"
                icono = "../images/flechas_recorridos/recorrido_n.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_norte_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_norte_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_norte_a.png"

            Case "2"
                icono = "../images/flechas_recorridos/recorrido_s.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_sur_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_sur_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_sur_a.png"
            Case "3"
                icono = "../images/flechas_recorridos/recorrido_e.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_este_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_este_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_este_a.png"
            Case "4"
                icono = "../images/flechas_recorridos/recorrido_o.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_oeste_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_este_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_este_a.png"
            Case "5"
                icono = "../images/flechas_recorridos/recorrido_ne.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_noreste_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_noreste_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_noreste_a.png"
            Case "6"
                icono = "../images/flechas_recorridos/recorrido_no.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_noroeste_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_noroeste_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_noroeste_a.png"
            Case "7"
                icono = "../images/flechas_recorridos/recorrido_se.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_sureste_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_sureste_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_sureste_a.png"

            Case "8"
                icono = "../images/flechas_recorridos/recorrido_so.png"
                If estado = "1" Then icono = "../images/flechas_recorridos/flecha_suroeste_v.png"
                If estado = "2" Then icono = "../images/flechas_recorridos/flecha_suroeste_r.png"
                If estado = "3" Then icono = "../images/flechas_recorridos/flecha_suroeste_a.png"

            Case Else
                icono = "../images/flechas_recorridos/recorrido.png"

        End Select



        Return icono
    End Function

    <WebMethod()> _
    Public Function GetArea(ByVal zon_id As Integer) As List(Of RecorridoItem)

        Dim Items As New List(Of RecorridoItem)

        Dim puntos As List(Of Zonas_Puntos) = clsZona.ZonaPuntosList(zon_id)

        For Each r As Zonas_Puntos In puntos
            Items.Add(New RecorridoItem With {.id = r.zon_punto_id, _
                                          .lng = r.zon_longitud, _
                                          .lat = r.zon_latitud})

        Next

        Return Items
    End Function

    <WebMethod()> _
    Public Function GetRecorrido(ByVal rec_id As Integer) As List(Of RecorridoItem)

        Dim Items As New List(Of RecorridoItem)

        Dim recorridos As List(Of Recorridos_Puntos) = clsRecorrido.RecorridoPuntosList(rec_id)

        For Each r As Recorridos_Puntos In recorridos
            Items.Add(New RecorridoItem With {.id = r.rec_punto_id, _
                                          .lng = r.rec_longitud, _
                                          .lat = r.rec_latitud})

        Next

        Return Items
    End Function

    <WebMethod()> _
    Public Function GetMarcadores(ByVal cli_id As Integer) As List(Of MarcadorItem)
        Dim Items As New List(Of MarcadorItem)
        Try


            'recuepro la ultima ubicacion de los vehiculos del cliente logeado.

            Dim marcadores As List(Of Marcadores) = clsMarcador.List(cli_id)
            Dim icono As String = ""



            For Each marca As Marcadores In marcadores



                'segun el tipo de marcador es el icono

                icono = marca.Tipos_Marcadores.tipo_marc_imagen

                Items.Add(New MarcadorItem With {.id = marca.marc_id, _
                                                  .icono = icono, _
                                                .lng = marca.marc_longitud, _
                                                .lat = marca.marc_latitud, _
                                                .nombre = marca.marc_nombre, _
                                                .direccion = marca.marc_direccion})

            Next
            'busco los marcadores genericos asociados a este cliente
            'Dim marcadoresGenericos As List(Of Marcadores_Genericos) = clsMarcador.ListGenericosByCliente(cli_id)

            'For Each marca As Marcadores_Genericos In marcadoresGenericos

            'If marca.marc_imagen = "" Then
            'icono = marca.Tipos_Marcadores.tipo_marc_imagen
            'Else
            'icono = marca.marc_imagen
            'End If



            'Items.Add(New MarcadorItem With {.id = marca.marc_id, _
            '  .icono = icono, _
            '.lng = marca.marc_longitud, _
            '.lat = marca.marc_latitud, _
            '.nombre = marca.marc_nombre, _
            '.direccion = marca.marc_direccion})

            'Next
        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetMarcadoresGenericosCliente(ByVal cli_id As Integer) As List(Of MarcadorItem)
        Dim Items As New List(Of MarcadorItem)
        Try

            Dim icono As String = ""
            Dim marcadoresGenericos As List(Of Marcadores_Generico) = clsMarcador.ListGenericosByCliente(cli_id)

            For Each marca As Marcadores_Generico In marcadoresGenericos

                If marca.marc_imagen = "" Then
                    icono = marca.Tipos_Marcadores.tipo_marc_imagen
                Else
                    icono = marca.marc_imagen
                End If



                Items.Add(New MarcadorItem With {.id = marca.marc_id, _
                                                  .icono = icono, _
                                                .lng = marca.marc_longitud, _
                                                .lat = marca.marc_latitud, _
                                                .nombre = marca.marc_nombre, _
                                                .direccion = marca.marc_direccion})

            Next
        Catch ex As Exception

        End Try
        Return Items
    End Function


    <WebMethod()> _
    Public Function GetMarcadoresGenericos() As List(Of MarcadorItem)
        Dim Items As New List(Of MarcadorItem)
        Try


            'recuepro la ultima ubicacion de los vehiculos del cliente logeado.

            Dim marcadores As List(Of Marcadores_Generico) = clsMarcador.ListGenericos()
            Dim icono As String = ""
            For Each marca As Marcadores_Generico In marcadores

                'segun el tipo de marcador es el icono

                If marca.marc_imagen = "" Then
                    icono = marca.Tipos_Marcadores.tipo_marc_imagen
                Else
                    icono = marca.marc_imagen
                End If

                Items.Add(New MarcadorItem With {.id = marca.marc_id, _
                                                  .icono = icono, _
                                                .lng = marca.marc_longitud, _
                                                .lat = marca.marc_latitud, _
                                                .nombre = marca.marc_nombre, _
                                                .direccion = marca.marc_direccion})

            Next
        Catch ex As Exception

        End Try
        Return Items
    End Function

    <WebMethod()> _
    Public Function GetDireccion(ByVal lat As String, ByVal lng As String) As String
        Dim direccion As String = ""
        Dim sError As String = ""
        Try
            'recupero la direccion en base a las coordenadas

            clsGoogle.getdireccion(lat, lng, direccion, sError)

        Catch ex As Exception
            Return sError
        End Try
        Return direccion
    End Function


    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasExcesos(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()

            Dim alarmas As List(Of Alamas_Kms_Excedidos) = clsAlarma.AlertaExcesoKmsByMovil(CInt(veh_id))
            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Detalle")
            dt.Columns.Add("Tipo")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("veh_id")
            dt.Columns.Add("kms")
            For Each d As Alamas_Kms_Excedidos In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.alak_id
                dr(1) = d.Vehiculo.veh_patente
                dr(2) = d.alak_descripcion

                If d.alak_frecuencia = "1" Then dr(3) = "Diaria"
                If d.alak_frecuencia = "2" Then dr(3) = "Semanal"
                If d.alak_frecuencia = "3" Then dr(3) = "Mensual"

                If d.alak_enviar_mail Then
                    dr(4) = "Si"
                Else
                    dr(4) = "No"
                End If


                dr(5) = "No"

                dr(6) = d.alak_id.ToString() + "|" + veh_id.ToString() + "|" + "Excesos"

                dr(7) = d.alak_cant_km.ToString()

                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasExcesoList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasFueraHorario(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()

            Dim alarmas As List(Of Alarmas_Fuera_Horario) = clsAlarma.AlertaFueraHoraByMovil(CInt(veh_id))
            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Detalle")
            dt.Columns.Add("Tipo")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("veh_id")

            For Each d As Alarmas_Fuera_Horario In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.alah_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.alah_descripcion

                If d.alah_enviar_mail.Value Then
                    dr(4) = "Si"
                Else
                    dr(4) = "No"
                End If


                dr(5) = "No"

                dr(6) = d.alah_id.ToString() + "|" + veh_id.ToString() + "|" + "horario"

                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasHorarioList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasInicioActividad(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()

            Dim alarmas As List(Of Alarma_Inicio_Actividad) = clsAlarma.AlertaInicioActividadByMovil(CInt(veh_id))
            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Detalle")
            dt.Columns.Add("Horario")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("veh_id")

            For Each d As Alarma_Inicio_Actividad In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.alaric_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.alaric_descripcion
                dr(3) = d.alar_horainicio
                If d.alar_enviar_mail Then
                    dr(4) = "Si"
                Else
                    dr(4) = "No"
                End If


                dr(5) = "No"

                dr(6) = d.alaric_id.ToString() + "|" + veh_id.ToString() + "|" + "inicio"

                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasInicioList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasInactividad(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()

            Dim alarmas As List(Of Alarmas_Inactividad) = clsAlarma.AlertaInactividadByMovil(CInt(veh_id))
            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Detalle")
            dt.Columns.Add("Tiempo")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("veh_id")

            For Each d As Alarmas_Inactividad In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.alari_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.alari_descripcion
                dr(3) = d.alari_tiempo_minimo
                If d.alari_enviar_mail Then
                    dr(4) = "Si"
                Else
                    dr(4) = "No"
                End If


                dr(5) = "No"

                dr(6) = d.alari_id.ToString() + "|" + veh_id.ToString() + "|" + "inactivo"

                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasInactividadList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasZonas(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()

            Dim alarmas As List(Of Alertas_Zonas) = clsAlarma.AlertaZonaByMovil(CInt(veh_id))
            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Detalle")
            dt.Columns.Add("Tipo")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("veh_id")
            dt.Columns.Add("zon_activa")
            For Each d As Alertas_Zonas In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.azon_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.Zonas.zon_nombre
                If d.azon_tipo = "1" Then
                    dr(3) = "Llegada"
                Else
                    dr(3) = "Salida"
                End If
                If d.azon_enviar_mail Then
                    dr(4) = "Si"
                Else
                    dr(4) = "No"
                End If

                If d.azon_enviar_sms Then
                    dr(5) = "Si"
                Else
                    dr(5) = "No"
                End If
                dr(6) = d.azon_id.ToString() + "|" + veh_id.ToString() + "|" + "zona"


                If d.azon_activa Then
                    dr(7) = "Si"
                Else
                    dr(7) = "No"
                End If

                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasZonaList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasDirecciones(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()

            Dim alarmas As List(Of Alertas_Direcciones) = clsAlarma.AlertaDireccionByMovil(CInt(veh_id))


            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Direccion")
            dt.Columns.Add("Tipo")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("dir_id")
            dt.Columns.Add("dir_activa")

            For Each d As Alertas_Direcciones In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.dir_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.Direcciones.dir_direccion

                If d.adir_tipo = "1" Then
                    dr(3) = "Llegada"
                Else
                    dr(3) = "Salida"
                End If
                If d.adir_enviar_mail Then
                    dr(4) = "Si"
                Else
                    dr(4) = "No"
                End If

                If d.adir_enviar_sms Then
                    dr(5) = "Si"
                Else
                    dr(5) = "No"
                End If
                dr(6) = d.adir_id.ToString() + "|" + veh_id.ToString() + "|" + "dir"

                If d.adir_activa Then
                    dr(7) = "Si"
                Else
                    dr(7) = "No"
                End If
                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasDireccList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasRecorridos(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()
            Dim alarmas As List(Of Alertas_Recorridos) = clsAlarma.AlertaRecorridoByMovil(CInt(veh_id))

            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Descripcion")
            dt.Columns.Add("Origen")
            dt.Columns.Add("Destino")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("rec_id")
            dt.Columns.Add("rec_activa")

            For Each d As Alertas_Recorridos In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.arec_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.Recorridos.rec_nombre
                dr(3) = d.Recorridos.rec_origen
                dr(4) = d.Recorridos.rec_destino

                If d.arec_enviar_mail Then
                    dr(5) = "Si"
                Else
                    dr(5) = "No"
                End If

                If d.arec_enviar_sms Then
                    dr(6) = "Si"
                Else
                    dr(6) = "No"
                End If
                dr(7) = d.arec_id.ToString() + "|" + veh_id.ToString() + "|" + "rec"

                If d.arec_activa Then
                    dr(8) = "Si"
                Else
                    dr(8) = "No"
                End If
                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasRecorridoList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasRecordatorioFecha(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()
            Dim alarmas As List(Of Alertas_Recordatorios_Por_Fechas) = clsAlarma.AlertaRecordatorioFechaByMovil(CInt(veh_id))

            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Descripcion")
            dt.Columns.Add("Perciocidad")
            dt.Columns.Add("Ocurrencia")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("recf_id")

            For Each d As Alertas_Recordatorios_Por_Fechas In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.recf_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.recf_descripcion
                If d.recf_periocidad = "anio" Then dr(3) = "Todos los Años"
                If d.recf_periocidad = "mes" Then dr(3) = "Todos los Meses"
                If d.recf_periocidad = "dia" Then dr(3) = "Fecha Especif"

                dr(4) = d.recf_proxima_ocurrencia

                If d.recf_notificar_mail Then
                    dr(5) = "Si"
                Else
                    dr(5) = "No"
                End If

                If d.recf_notificar_sms Then
                    dr(6) = "Si"
                Else
                    dr(6) = "No"
                End If
                dr(7) = d.recf_id.ToString() + "|" + veh_id.ToString() + "|" + "recf"
                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasRecordatorioFechaList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function gAlarmasRecordatorioKm(ByVal veh_id As String) As String
        Try
            Dim dt As DataTable = New DataTable()
            Dim alarmas As List(Of Alertas_Recordatorios_Por_Km) = clsAlarma.AlertaRecordatorioKmByMovil(CInt(veh_id))

            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Patente")
            dt.Columns.Add("Descripcion")
            dt.Columns.Add("Kms")
            dt.Columns.Add("Ocurrencia")
            dt.Columns.Add("Notificar_Mail")
            dt.Columns.Add("Notificar_SMS")
            dt.Columns.Add("reck_id")

            For Each d As Alertas_Recordatorios_Por_Km In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.reck_id
                dr(1) = d.Vehiculo.veh_patente

                dr(2) = d.reck_descripcion
                dr(3) = d.reck_kilm_proxima_alarma
                dr(4) = d.reck_ocurrencia_cada_km

                If d.reck_notificar_mail Then
                    dr(5) = "Si"
                Else
                    dr(5) = "No"
                End If

                If d.reck_notificar_sms Then
                    dr(6) = "Si"
                Else
                    dr(6) = "No"
                End If
                dr(7) = d.reck_id.ToString() + "|" + veh_id.ToString() + "|" + "reck"
                dt.Rows.Add(dr)
            Next

            System.Web.HttpContext.Current.Session.Add("AlarmasRecordatorioKmList", dt)

        Catch ex As Exception

        End Try

        Return "hecho"

    End Function

    Public Class VehiculoItem
        Public id As String
        Public nombre As String
        Public patente As String
        Public ubicacion As String
        Public velocidad As String
        Public foto As String
        Public lng As String
        Public lat As String
        Public hora As String
        Public fecha As String
        Public icono As String
        Public modulo As String
        Public conductor As String
        Public orientacion As String
        Public estado_reporte As String
        Public marcador As String
        Public alarma As String
    End Class

    Public Class MarcadorItem
        Public id As String
        Public lng As String
        Public lat As String
        Public icono As String
        Public nombre As String
        Public direccion As String

    End Class

    Public Class RecorridoItem
        Public id As String
        Public lng As String
        Public lat As String

    End Class

    Public Class AlarmaItem
        Public id As String
        Public patente As String
        Public alarma As String
        Public fecha As String
        Public hora As String
        Public cliente As String
        Public ubicacion As String
        Public categoria As String
        Public conductor As String
        Public velocidad As String
        Public lng As String
        Public lat As String

    End Class

    Public Class AlarmaConfiguradaItem
        Public ale_id As String
        Public patente As String
        Public valor As String
        Public enviarMail As String
        Public enviarSMS As String
        Public veh_id As String
        Public alarma As String
        Public configurada As Boolean
    End Class

    Private Function recordCount() As Integer
        Throw New NotImplementedException
    End Function

End Class