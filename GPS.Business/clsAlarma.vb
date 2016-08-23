' Esta clase administra todo los referido a las alarmas
' Alta, baja y modificacion de alertas configuradas para un movil
'Control de alarmas segun el reporte de ubicacion de un movil

Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data
Imports System.Drawing
Imports System.Math

'clases auxiliares
Public Class DistanciaPuntos
    Private m_distancia As Double
    Private m_idPunto As Int32

    Public Property Distancia() As Double
        Get
            Return m_distancia
        End Get
        Set(ByVal value As Double)
            m_distancia = value
        End Set
    End Property

    Public Property idPunto() As String
        Get
            Return m_idPunto
        End Get
        Set(ByVal value As String)
            m_idPunto = value
        End Set
    End Property


End Class

Public Class Vertice
    Private m_verticex As Double
    Private m_verticey As Double

    Public Property verticeX() As Double
        Get
            Return m_verticex
        End Get
        Set(ByVal value As Double)
            m_verticex = value
        End Set
    End Property

    Public Property verticeY() As Double
        Get
            Return m_verticey
        End Get
        Set(ByVal value As Double)
            m_verticey = value
        End Set
    End Property


End Class

Public Class AlarmarPorCategoria
    Private m_total As Int32


    Public Property Total() As Int32
        Get
            Return m_total
        End Get
        Set(ByVal value As Int32)
            m_total = value
        End Set
    End Property

    Private m_Categoria As String
    Public Property Categoria() As String
        Get
            Return m_Categoria
        End Get
        Set(ByVal value As String)
            m_Categoria = value
        End Set
    End Property


End Class

Public Class clsAlarma


    Public Shared points As New List(Of RouteGeoFence.Point)()

    ''' <summary>
    ''' Retorna los datos de una alarma reportada buscando por su id
    ''' </summary>
    ''' <param name="alar_id"></param>
    ''' <returns>Alarmas object</returns>
    ''' <remarks></remarks>
    Public Shared Function SelectAlarmaById(ByVal alar_id As Integer) As Alarmas
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarmas.Where(Function(d) d.alar_id = alar_id).FirstOrDefault()

    End Function

    ''' <summary>
    ''' Retorna la lista de sensores configurados para un auto buscando por la posicion asignada
    ''' </summary>
    ''' <param name="posicion"></param>
    ''' <param name="veh_id"></param>
    ''' <returns>list of sensores configurados</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSensoresConfigurados(ByVal posicion As Integer, ByVal veh_id As Integer) As List(Of Sensores_Configurados)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Sensores_Configurados _
       Where alarma.Sensores.sen_posicion = posicion And alarma.veh_id = veh_id
       Select alarma).ToList()

    End Function

    ''' <summary>
    ''' Retorna la lista de alarmas reportadas que el cliente no ha visto para un rango de fechas
    ''' </summary>
    ''' <param name="cli_id"></param>
    ''' <param name="fecha_desde"></param>
    ''' <param name="fecha_hasta"></param>
    ''' <returns>list of object Alarmas</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAlertasNoVista(ByVal cli_id As Integer, ByVal fecha_desde As Date, ByVal fecha_hasta As Date) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
        Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
                    Where alarma.alar_vista = False And (vehiculo.cli_id = cli_id) _
                    And (alarma.alar_fecha >= fecha_desde) And (alarma.alar_fecha <= fecha_hasta) _
                    Select alarma _
                    Order By alarma.alar_id Descending).ToList()

        Return alarmas

    End Function

    ''' <summary>
    ''' Retorna todas las alarmas reportadas por los moviles de un cliente en un rango de fechas
    ''' </summary>
    ''' <param name="cli_id"></param>
    ''' <param name="fecha_desde"></param>
    ''' <param name="fecha_hasta"></param>
    ''' <returns>list of object Alarmas</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAlertasAll(ByVal cli_id As Integer, ByVal fecha_desde As Date, ByVal fecha_hasta As Date) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
        Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
                    Where (vehiculo.cli_id = cli_id Or cli_id = 0) _
                    And (alarma.alar_fecha >= fecha_desde) And (alarma.alar_fecha <= fecha_hasta) _
                    Select alarma _
                    Order By alarma.alar_id Descending).ToList()

        Return alarmas

    End Function

    ''' <summary>
    ''' Retorna los datos de la configuracion de una alerta de velocidad
    ''' </summary>
    ''' <param name="ale_id"></param>
    ''' <returns>Alertas_Velocidad_Configuradas object</returns>
    ''' <remarks></remarks>
    Public Shared Function SelectAlertas_Velocidad_ConfiguradasById(ByVal ale_id As Integer) As Alertas_Velocidad_Configuradas
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Velocidad_Configuradas.Where(Function(d) d.ale_id = ale_id).FirstOrDefault()

    End Function

    ''' <summary>
    ''' Retorna los datos de la configuracion de una alerta de entrada o salida de direcciones
    ''' </summary>
    ''' <param name="dir_id"></param>
    ''' <returns>Alertas_Direcciones object</returns>
    ''' <remarks></remarks>

    Public Shared Function SelectAlertaDireccionById(ByVal dir_id As Integer) As Alertas_Direcciones
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Direcciones.Where(Function(d) d.adir_id = dir_id).FirstOrDefault()

    End Function

    ''' <summary>
    ''' Retorna los datos de la configuracion de una alerta de desvio de zonas
    ''' </summary>
    ''' <param name="zon_id"></param>
    ''' <returns>Alertas_Zonas object</returns>
    ''' <remarks></remarks>
    Public Shared Function SelectAlertaZonaById(ByVal zon_id As Integer) As Alertas_Zonas
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Zonas.Where(Function(d) d.azon_id = zon_id).FirstOrDefault()

    End Function


    ''' <summary>
    ''' Retorna los datos de una alarma reportada de exceso de velocidad
    ''' </summary>
    ''' <param name="veh_id"></param>
    ''' <param name="velocidad"></param>
    ''' <param name="ale_id"></param>
    ''' <returns>list of Alarmas</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAlertasReportadasVelocidad(ByVal veh_id As Integer, ByVal velocidad As Decimal, ByVal ale_id As Integer) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And (CDec(alarma.alar_valor) = velocidad)
                    Select alarma _
                    Order By alarma.alar_id Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasReportadasMes(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal fecha As String) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And alarma.alar_duracion = 0 _
                    And (alarma.alar_fecha.Month = CDate(fecha).Month)
                    Select alarma _
                    Order By alarma.alar_id Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasReportadasSemana(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal fecha As String) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = DB.ExecuteQuery(Of Alarmas)(" Select * FROM Alarmas  WHERE (DATEPART(week, alar_fecha) = DATEPART(week, '" & fecha & "')) AND (alar_codigo_config = " & ale_id & ") AND (veh_id = " & veh_id & ") AND (alar_tipo = " & tipo & ")").ToList()

        Return alarmas

    End Function


    Public Shared Function GetAlertasReportadasFecha(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal fecha As String) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And alarma.alar_duracion = 0 _
                    And (alarma.alar_fecha >= CDate(fecha & " 00:00") And alarma.alar_fecha <= CDate(fecha & " 23:59"))
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasReportadasFecha(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal fecha As String, ByVal valor As Decimal) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And alarma.alar_duracion = 0 _
                    And alarma.alar_valor = valor _
                    And (alarma.alar_fecha >= CDate(fecha & " 00:00") And alarma.alar_fecha <= CDate(fecha & " 23:59"))
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function

    'verifico si hay una alarma igual disparada en una fecha anterior
    'esto solo va a aplicar para las tramas que vienen de memoria
    Public Shared Function GetAlertasReportadas(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal fecha As String) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And alarma.alar_duracion = 0 And alarma.alar_fecha >= CDate(fecha)
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasReportadas(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And alarma.alar_duracion = 0
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()
        Return alarmas

    End Function

    Public Shared Function GetAlertasReportadasPunto(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal fecha As DateTime) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And alarma.alar_duracion = 0 And alarma.alar_fecha >= fecha
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasReportadasPunto(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And alarma.alar_duracion = 0
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function
    'busco si ya reporte la alarma y la duracion esta en cero, para no volver a grabarla

    'verificacion para tramas en memoria
    Public Shared Function GetAlertasReportadasD(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal fecha As DateTime) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where alarma.veh_id = veh_id _
                    And alarma.alar_codigo_config = ale_id And alarma.alar_tipo = tipo And alarma.alar_duracion = 0 And alarma.alar_fecha >= fecha
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function



    Public Shared Function GetAlertasReportadas(ByVal veh_id As Integer, ByVal ale_id As Integer, ByVal tipo As Integer, ByVal valor As Decimal) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.veh_id = veh_id) _
                    And (alarma.alar_codigo_config = ale_id) And alarma.alar_tipo = tipo And (CDec(alarma.alar_valor) = valor)
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertas(ByVal cli_id As Integer, ByVal veh_id As Integer, ByVal fecha_desde As Date, ByVal fecha_hasta As Date, ByVal patente As String) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
        Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
                    Where (vehiculo.cli_id = cli_id Or cli_id = 0) And (vehiculo.veh_id = veh_id Or veh_id = 0) _
                    And (alarma.alar_fecha >= fecha_desde) And (alarma.alar_fecha <= fecha_hasta) And (alarma.veh_patente = patente Or patente = "") _
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasVisibles(ByVal cli_id As Integer, ByVal veh_id As Integer, ByVal fecha_desde As Date, ByVal fecha_hasta As Date, ByVal patente As String) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()

        DB.CommandTimeout = 60
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
        Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
                    Where (vehiculo.cli_id = cli_id Or cli_id = 0) And (vehiculo.veh_id = veh_id Or veh_id = 0) _
                    And (alarma.alar_fecha >= fecha_desde) And (alarma.alar_fecha <= fecha_hasta) And (alarma.veh_patente = patente Or patente = "") And alarma.alar_mostrar = True _
                    Select alarma _
                    Order By alarma.alar_fecha Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasPanico() As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
                    Where (alarma.alar_tipo = 2) And alarma.alar_vista_admin = False _
                    Select alarma _
                    Order By alarma.alar_id Descending).ToList()

        Return alarmas

    End Function

    Public Shared Function GetAlertasNoVistas(ByVal cli_id As Integer) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
        Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
                    Where vehiculo.cli_id = cli_id And alarma.alar_vista = False _
                    Select alarma _
                    Order By alarma.alar_id Descending).Take(10).ToList()
        Return alarmas


    End Function


    Public Shared Function GetAlertasLast(ByVal cli_id As Integer) As List(Of Alarmas)
        Dim DB As New DataClassesGPSDataContext()
        Dim alarmas = New List(Of Alarmas)

        alarmas = (From alarma In DB.Alarmas _
        Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
                    Where vehiculo.cli_id = cli_id And alarma.alar_vista = False _
                    Select alarma _
                    Order By alarma.alar_id Descending).Take(5).ToList()

        Return alarmas

    End Function

    Public Shared Function ocultarAlarma(ByVal ala_id As Integer) As Boolean
        Using DB As New DataClassesGPSDataContext()
            Dim oOriginal As Alarmas = DB.Alarmas.Where(Function(d) d.alar_id = ala_id).FirstOrDefault()

            oOriginal.alar_mostrar = False
            DB.SubmitChanges()
            Return True
        End Using
    End Function
    'busco la alarma de este tipo anterior que tenga duracion en cero porque todavia no finalizo
    Public Shared Function SearchAlarmaSensorAnterior(ByVal veh_id As Integer, ByVal tipo As Integer, ByVal sen_id As Integer) As Alarmas
        Dim DB As New DataClassesGPSDataContext()
        DB.CommandTimeout = 120
        Return DB.Alarmas.Where(Function(d) d.veh_id = veh_id And d.alar_tipo = tipo And d.alar_duracion = 0 And d.alar_codigo_config = sen_id).OrderByDescending(Function(d) d.alar_fecha).FirstOrDefault()

    End Function


    Public Shared Function SearchAnterior(ByVal veh_id As Integer, ByVal tipo As Integer) As Alarmas
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Alarmas.Where(Function(d) d.veh_id = veh_id And d.alar_tipo = tipo And d.alar_duracion = 0).OrderByDescending(Function(d) d.alar_id).FirstOrDefault()

    End Function

    Public Shared Function SearchAnterior(ByVal veh_id As Integer, ByVal tipo As Integer, ByVal codigo_config As Integer, ByVal fecha As DateTime) As Alarmas
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Alarmas.Where(Function(d) d.veh_id = veh_id And d.alar_tipo = tipo And d.alar_codigo_config = codigo_config And d.alar_duracion = 0 And d.alar_fecha < fecha).OrderByDescending(Function(d) d.alar_fecha).FirstOrDefault()

    End Function

    ''' <summary>
    ''' Setea una alrma reportada como vista por el cliente
    ''' </summary>
    ''' <param name="ala_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function visarAlarma(ByVal ala_id As Integer) As Boolean
        Using DB As New DataClassesGPSDataContext()

            Dim oOriginal As Alarmas = DB.Alarmas.Where(Function(d) d.alar_id = ala_id).FirstOrDefault()

            oOriginal.alar_vista = True
            DB.SubmitChanges()
            Return True
        End Using
    End Function

    ''' <summary>
    ''' Marca uan alarma reportada como vista por el administrador
    ''' </summary>
    ''' <param name="ala_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function visarAlarmaAdmin(ByVal ala_id As Integer) As Boolean
        Using DB As New DataClassesGPSDataContext()
            Dim oOriginal As Alarmas = DB.Alarmas.Where(Function(d) d.alar_id = ala_id).FirstOrDefault()

            oOriginal.alar_vista_admin = True
            DB.SubmitChanges()
            Return True
        End Using
    End Function

    Public Shared Function CantidadByVehiculo(ByVal veh_id As Integer, ByVal tipo As Integer) As Integer
        Using DB As New DataClassesGPSDataContext()

            Return DB.Alarmas.Where(Function(d) d.veh_id = veh_id And d.alar_tipo = tipo).ToList().Count
        End Using

    End Function


    Public Shared Function CantidadByVehiculo(ByVal veh_id As Integer) As Integer
        Using DB As New DataClassesGPSDataContext()
            Return DB.Alarmas.Where(Function(d) d.veh_id = veh_id).ToList().Count
        End Using

    End Function

    Public Shared Function CantidadByCategoria(ByVal veh_id As Integer) As List(Of AlarmarPorCategoria)
        Dim DB As New DataClassesGPSDataContext()
        'Return DB.Alarmas.Where(Function(d) d.veh_id = veh_id).ToList().Count
        Dim result = (From a In DB.Alarmas Where a.veh_id = veh_id _
            Group By SubjectId = a.alar_Categoria + a.alar_nombre Into SIds = Group _
            Select _
            Categoria = SubjectId, _
            Total = SIds.Count()).ToList

        Dim c As New List(Of AlarmarPorCategoria)

        For Each al In result
            Dim item = New AlarmarPorCategoria()

            item.Categoria = al.Categoria
            item.Total = al.Total
            c.Add(item)

        Next

        Return c

    End Function



    Public Shared Function AlertaInactividadDiasList(ByVal alah_id As Integer) As List(Of Alarmas_Inactividad_Dias)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarmas_Inactividad_Dias.Where(Function(d) d.alari_id = alah_id).ToList()

    End Function

    Public Shared Function AlertaInicioActividadDiasList(ByVal alah_id As Integer) As List(Of Alarmas_Inicio_Actividad_Dias)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarmas_Inicio_Actividad_Dias.Where(Function(d) d.alaric_id = alah_id).ToList()

    End Function

    Public Shared Function AlertaFueraHorarioDiasList(ByVal alah_id As Integer) As List(Of Alarmas_Fuera_Horario_Dias)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarmas_Fuera_Horario_Dias.Where(Function(d) d.alah_id = alah_id).ToList()

    End Function

    Public Shared Function AlertaKmsExcedidosSelect(ByVal alak_id As Integer) As Alamas_Kms_Excedidos
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alamas_Kms_Excedidos.Where(Function(d) d.alak_id = alak_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaFueraHorarioSelect(ByVal alah_id As Integer) As Alarmas_Fuera_Horario
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarmas_Fuera_Horario.Where(Function(d) d.alah_id = alah_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaInicioActividadSelect(ByVal alah_id As Integer) As Alarma_Inicio_Actividad
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarma_Inicio_Actividad.Where(Function(d) d.alaric_id = alah_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaInactividadSelect(ByVal alah_id As Integer) As Alarmas_Inactividad
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarmas_Inactividad.Where(Function(d) d.alari_id = alah_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaDireccionSelect(ByVal dir_id As Integer) As Alertas_Direcciones
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Direcciones.Where(Function(d) d.adir_id = dir_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaDireccionSearch(ByVal veh_id As Integer, ByVal lat As String, ByVal lng As String, ByVal tipo As Integer) As Alertas_Direcciones
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Direcciones _
       Where alarma.Direcciones.dir_latitud = lat And alarma.Direcciones.dir_longitud = lng And alarma.veh_id = veh_id And alarma.adir_tipo = tipo _
       Select alarma).FirstOrDefault()

    End Function


    Public Shared Function AlertaZonaSelect(ByVal zon_id As Integer) As Alertas_Zonas
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Zonas.Where(Function(d) d.zon_id = zon_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaZonaSelectById(ByVal azon_id As Integer) As Alertas_Zonas
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Zonas.Where(Function(d) d.azon_id = azon_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaZonaSelect(ByVal zon_id As Integer, ByVal veh_id As Integer) As Alertas_Zonas
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Zonas _
      Where alarma.azon_id = zon_id And alarma.veh_id = veh_id _
      Select alarma).FirstOrDefault()


    End Function

    Public Shared Function AlertaSensoresByMovil(ByVal veh_id As Integer) As List(Of Sensores_Configurados)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Sensores_Configurados _
      Where alarma.veh_id = veh_id _
      Select alarma).ToList()

    End Function

    Public Shared Function DeleteRegistroInactividadMovil(ByVal veh_id As Integer, ByVal alar_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As List(Of Inactividad_Moviles) = DB.Inactividad_Moviles.Where(Function(b) b.veh_id = veh_id And b.alar_id = alar_id).ToList()

                If original IsNot Nothing Then
                    DB.Inactividad_Moviles.DeleteAllOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function DeleteRegistroUsoMovil(ByVal veh_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As List(Of Uso_Moviles) = DB.Uso_Moviles.Where(Function(b) b.veh_id = veh_id).ToList()

                If original IsNot Nothing Then
                    DB.Uso_Moviles.DeleteAllOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Shared Function SelectRegistroInactividadMovil(ByVal veh_id As Integer, ByVal alar_id As Integer) As Inactividad_Moviles
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Inactividad_Moviles _
       Where alarma.veh_id = veh_id And alarma.alar_id = alar_id
       Select alarma).FirstOrDefault()

    End Function

    Public Shared Function SelectRegistroUsoMovil(ByVal veh_id As Integer) As Uso_Moviles
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Uso_Moviles _
       Where alarma.veh_id = veh_id
       Select alarma).FirstOrDefault()

    End Function

    Public Shared Function SelectAlertaSensores(ByVal posicion As Integer, ByVal veh_id As Integer) As List(Of Sensores_Configurados)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Sensores_Configurados _
       Where alarma.Sensores.sen_posicion = posicion And alarma.veh_id = veh_id And alarma.Sensores.sen_alarma = True
       Select alarma).ToList()

    End Function

    Public Shared Function SelectSensoresByMovil(ByVal veh_id As Integer) As List(Of Sensores_Moviles)
        Dim DB As New DataClassesGPSDataContext()
        Dim _sensores As List(Of Sensores_Moviles)

        _sensores = (From sensor In DB.Sensores_Moviles _
       Where sensor.veh_id = veh_id And sensor.Sensores.sen_binario = True _
       Select sensor).OrderBy(Function(s) s.Sensores.sen_posicion).ToList()

        Return _sensores

    End Function

    Public Shared Function SelectSensor(ByVal sen_id As Integer) As Sensores
        Dim DB As New DataClassesGPSDataContext()
        Return (From sensor In DB.Sensores _
        Where sensor.sen_id = sen_id
        Select sensor).FirstOrDefault


    End Function

    Public Shared Function AlertaDireccionByMovil(ByVal veh_id As Integer) As List(Of Alertas_Direcciones)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Direcciones _
      Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
      Where vehiculo.veh_id = veh_id And alarma.adir_activa = True _
      Select alarma).ToList()

    End Function

    Public Shared Function AlertaDireccionByMovil(ByVal veh_id As Integer, ByVal fecha As DateTime) As List(Of Alertas_Direcciones)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Direcciones _
       Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
       Where vehiculo.veh_id = veh_id And ((alarma.adir_fecha_desde <= fecha Or alarma.adir_fecha_desde Is Nothing) And (alarma.adir_fecha_hasta >= fecha Or alarma.adir_fecha_hasta Is Nothing)) _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaDireccionByMovilFrecuencia(ByVal veh_id As Integer, ByVal fecha As DateTime) As List(Of Alertas_Direcciones)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Direcciones _
     Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
     Join frecuencia In DB.Alertas_Direcciones_Frecuencia On alarma.adir_id Equals frecuencia.dir_id _
     Where vehiculo.veh_id = veh_id And frecuencia.dir_dia_semana = fecha.DayOfWeek.GetHashCode() And ((frecuencia.dir_frec_hora_desde <= fecha.TimeOfDay Or frecuencia.dir_frec_hora_desde Is Nothing) And (frecuencia.dir_frec_hora_hasta >= fecha.TimeOfDay Or frecuencia.dir_frec_hora_hasta Is Nothing)) _
     Select alarma).ToList()

    End Function

    Public Shared Function AlertaZonaActivaByMovil(ByVal veh_id As Integer) As List(Of Alertas_Zonas)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Zonas _
        Where alarma.veh_id = veh_id And alarma.azon_activa = True _
       Select alarma).ToList()


    End Function


    Public Shared Function AlertaZonaByMovil(ByVal veh_id As Integer) As List(Of Alertas_Zonas)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Zonas _
        Where alarma.veh_id = veh_id _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaRecorridoSelect(ByVal rec_id As Integer) As Alertas_Recorridos
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Recorridos.Where(Function(d) d.arec_id = rec_id).FirstOrDefault()

    End Function

    Public Shared Function AlertaRecorridoByMovil(ByVal veh_id As Integer) As List(Of Alertas_Recorridos)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recorridos _
       Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
       Where vehiculo.veh_id = veh_id _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaFueraHoraByMovil(ByVal veh_id As Integer) As List(Of Alarmas_Fuera_Horario)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alarmas_Fuera_Horario _
       Where alarma.veh_id = veh_id _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaInicioActividadByMovil(ByVal veh_id As Integer) As List(Of Alarma_Inicio_Actividad)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alarma_Inicio_Actividad _
       Where alarma.veh_id = veh_id _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaExcesoKmsByMovil(ByVal veh_id As Integer) As List(Of Alamas_Kms_Excedidos)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alamas_Kms_Excedidos _
       Where alarma.veh_id = veh_id _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaInactividadByMovil(ByVal veh_id As Integer) As List(Of Alarmas_Inactividad)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alarmas_Inactividad _
       Where alarma.veh_id = veh_id _
       Select alarma).ToList()


    End Function

    Public Shared Function AlertaInactividadList() As List(Of Alarmas_Inactividad)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alarmas_Inactividad _
      Select alarma).ToList()

    End Function

    Public Shared Function AlertaRecorridoActivoByMovil(ByVal veh_id As Integer) As List(Of Alertas_Recorridos)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recorridos _
       Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
       Where vehiculo.veh_id = veh_id And alarma.arec_activa = True _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaRecordatorioFechaByMovil(ByVal veh_id As Integer) As List(Of Alertas_Recordatorios_Por_Fechas)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recordatorios_Por_Fechas _
       Where alarma.veh_id = veh_id _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaRecordatorioFecha(ByVal fecha1 As Date, ByVal fecha2 As Date) As List(Of Alertas_Recordatorios_Por_Fechas)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recordatorios_Por_Fechas _
      Where alarma.recf_proxima_ocurrencia >= fecha1 And alarma.recf_proxima_ocurrencia <= fecha2 _
      Select alarma).ToList()

    End Function

    Public Shared Function AlertaRecordatorioKmByMovil(ByVal veh_id As Integer) As List(Of Alertas_Recordatorios_Por_Km)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recordatorios_Por_Kms _
       Where alarma.veh_id = veh_id _
       Select alarma).ToList()

    End Function

    Public Shared Function ListAlertaRecordatorioKm() As List(Of Alertas_Recordatorios_Por_Km)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recordatorios_Por_Kms _
       Select alarma).ToList()

    End Function

    Public Shared Function AlertaSensorSelect(ByVal sen_id As Integer, ByVal veh_id As Integer) As Sensores_Configurados
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Sensores_Configurados _
      Where alarma.sen_id = sen_id And alarma.veh_id = veh_id _
      Select alarma).FirstOrDefault()

    End Function

    Public Shared Function AlertaRecordatorioFechaSelect(ByVal recf_id As Integer, ByVal veh_id As Integer) As Alertas_Recordatorios_Por_Fechas

        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recordatorios_Por_Fechas _
       Where alarma.recf_id = recf_id And alarma.veh_id = veh_id _
       Select alarma).FirstOrDefault()

    End Function

    Public Shared Function AlertaRecordatorioKmSelect(ByVal reck_id As Integer, ByVal veh_id As Integer) As Alertas_Recordatorios_Por_Km

        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recordatorios_Por_Kms _
      Where alarma.reck_id = reck_id And alarma.veh_id = veh_id _
       Select alarma).FirstOrDefault()

    End Function

    Public Shared Function UpdateAlarma(ByVal alarma As Alarmas) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alarmas = DB.Alarmas.Where(Function(d) d.alar_id = alarma.alar_id).FirstOrDefault()


                oOriginal.alar_fecha = alarma.alar_fecha
                oOriginal.alar_nombre_via = alarma.alar_nombre_via

                oOriginal.alar_Provincia = alarma.alar_Provincia
                oOriginal.alar_Localidad = alarma.alar_Localidad
                oOriginal.alar_lat = alarma.alar_lat
                oOriginal.alar_lng = alarma.alar_lng
                oOriginal.alar_codigo_config = alarma.alar_codigo_config
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function UpdateAlertaEnvioSMS(ByVal veh_id As Integer, ByVal sen_id As Integer, ByVal alart_envia_sms As Boolean) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Sensores_Moviles = DB.Sensores_Moviles.Where(Function(d) d.sen_id = sen_id And d.veh_id = veh_id).FirstOrDefault()

                oOriginal.enviar_sms = alart_envia_sms

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaConfigurada(ByVal alerta As Alertas_Velocidad_Configuradas) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Velocidad_Configuradas = DB.Alertas_Velocidad_Configuradas.Where(Function(d) d.ale_id = alerta.ale_id).FirstOrDefault()

                oOriginal.ale_enviar_mail = alerta.ale_enviar_mail
                oOriginal.ale_enviar_SMS = alerta.ale_enviar_SMS
                oOriginal.ale_valor_maximo = alerta.ale_valor_maximo


                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function UpdateAlertaDireccion(ByVal direccion As Alertas_Direcciones) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()

                Dim oOriginal As Alertas_Direcciones = DB.Alertas_Direcciones.Where(Function(d) d.adir_id = direccion.adir_id).FirstOrDefault()

                oOriginal.adir_enviar_mail = direccion.adir_enviar_mail
                oOriginal.adir_enviar_sms = direccion.adir_enviar_sms
                oOriginal.adir_tipo = direccion.adir_tipo
                oOriginal.adir_fecha_desde = direccion.adir_fecha_desde
                oOriginal.adir_fecha_hasta = direccion.adir_fecha_hasta
                oOriginal.adir_umbral_desvio = direccion.adir_umbral_desvio
                oOriginal.dir_id = direccion.dir_id
                oOriginal.adir_activa = direccion.adir_activa

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaRecorrido(ByVal recorrido As Alertas_Recorridos) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Recorridos = DB.Alertas_Recorridos.Where(Function(d) d.arec_id = recorrido.arec_id).FirstOrDefault()


                oOriginal.arec_enviar_mail = recorrido.arec_enviar_mail
                oOriginal.arec_enviar_sms = recorrido.arec_enviar_sms

                oOriginal.arec_fecha_desde = recorrido.arec_fecha_desde
                oOriginal.arec_fecha_hasta = recorrido.arec_fecha_hasta
                oOriginal.arec_umbral_desvio = recorrido.arec_umbral_desvio
                oOriginal.arec_activa = recorrido.arec_activa
                oOriginal.rec_id = recorrido.rec_id
                oOriginal.arec_no_deseado = recorrido.arec_no_deseado
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaExcesoKms(ByVal alarma As Alamas_Kms_Excedidos) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alamas_Kms_Excedidos = DB.Alamas_Kms_Excedidos.Where(Function(d) d.alak_id = alarma.alak_id).FirstOrDefault()
                oOriginal.alak_descripcion = alarma.alak_descripcion
                oOriginal.alak_cant_km = alarma.alak_cant_km
                oOriginal.alak_enviar_mail = alarma.alak_enviar_mail
                oOriginal.alak_frecuencia = alarma.alak_frecuencia
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaFueraHorario(ByVal alarma As Alarmas_Fuera_Horario) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alarmas_Fuera_Horario = DB.Alarmas_Fuera_Horario.Where(Function(d) d.alah_id = alarma.alah_id).FirstOrDefault()


                oOriginal.alah_descripcion = alarma.alah_descripcion
                oOriginal.alah_enviar_mail = alarma.alah_enviar_mail

                oOriginal.alah_fechadesde = alarma.alah_fechadesde
                oOriginal.alah_fechahasta = alarma.alah_fechahasta
                oOriginal.alah_horadesde = alarma.alah_horadesde
                oOriginal.alah_horahasta = alarma.alah_horahasta
                oOriginal.alah_tiempo_minimo = alarma.alah_tiempo_minimo
                oOriginal.alah_velocidad_minima = alarma.alah_velocidad_minima
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaInactividad(ByVal alarma As Alarmas_Inactividad) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alarmas_Inactividad = DB.Alarmas_Inactividad.Where(Function(d) d.alari_id = alarma.alari_id).FirstOrDefault()


                oOriginal.alari_descripcion = alarma.alari_descripcion

                oOriginal.alari_fechadesde = alarma.alari_fechadesde
                oOriginal.alari_fechahasta = alarma.alari_fechahasta
                oOriginal.alari_horadesde = alarma.alari_horadesde
                oOriginal.alari_horahasta = alarma.alari_horahasta
                oOriginal.alari_tiempo_minimo = alarma.alari_tiempo_minimo
                oOriginal.alah_velocidad_minima = alarma.alah_velocidad_minima
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaInicioActividad(ByVal alarma As Alarma_Inicio_Actividad) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alarma_Inicio_Actividad = DB.Alarma_Inicio_Actividad.Where(Function(d) d.alaric_id = alarma.alaric_id).FirstOrDefault()


                oOriginal.alaric_descripcion = alarma.alaric_descripcion
                oOriginal.alar_enviar_mail = alarma.alar_enviar_mail
                oOriginal.alaric_inicio_anteshorario = alarma.alaric_inicio_anteshorario
                oOriginal.alar_horainicio = alarma.alar_horainicio
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlarmaDuracion(ByVal alar_id As Integer, ByVal alar_duracion As Long) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alarmas = DB.Alarmas.Where(Function(d) d.alar_id = alar_id).FirstOrDefault()

                oOriginal.alar_duracion = alar_duracion

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function AlertaRecorridoActDesact(ByVal rec_id As Integer, ByVal estado As Boolean) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Recorridos = DB.Alertas_Recorridos.Where(Function(d) d.arec_id = rec_id).FirstOrDefault()

                oOriginal.arec_activa = estado

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function AlertaDireccionActDesact(ByVal dir_id As Integer, ByVal estado As Boolean) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Direcciones = DB.Alertas_Direcciones.Where(Function(d) d.adir_id = dir_id).FirstOrDefault()

                oOriginal.adir_activa = estado

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaZona(ByVal zona As Alertas_Zonas) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Zonas = DB.Alertas_Zonas.Where(Function(d) d.azon_id = zona.azon_id).FirstOrDefault()

                oOriginal.zon_id = zona.zon_id
                oOriginal.azon_tipo = zona.azon_tipo
                oOriginal.azon_enviar_mail = zona.azon_enviar_mail
                oOriginal.azon_enviar_sms = zona.azon_enviar_sms
                oOriginal.azon_fecha_desde = zona.azon_fecha_desde
                oOriginal.azon_fecha_hasta = zona.azon_fecha_hasta
                oOriginal.azon_umbral_desvio = zona.azon_umbral_desvio
                oOriginal.azon_activa = zona.azon_activa


                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function AlertaZonaActDesact(ByVal zon_id As Integer, ByVal estado As Boolean) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Zonas = DB.Alertas_Zonas.Where(Function(d) d.azon_id = zon_id).FirstOrDefault()

                oOriginal.azon_activa = estado


                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaRecFecha(ByVal rec As Alertas_Recordatorios_Por_Fechas) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Recordatorios_Por_Fechas = DB.Alertas_Recordatorios_Por_Fechas.Where(Function(d) d.recf_id = rec.recf_id).FirstOrDefault()

                oOriginal.recf_descripcion = rec.recf_descripcion
                oOriginal.recf_notificar_mail = rec.recf_notificar_mail
                oOriginal.recf_notificar_sms = rec.recf_notificar_mail
                oOriginal.recf_periocidad = rec.recf_periocidad
                oOriginal.recf_proxima_ocurrencia = rec.recf_proxima_ocurrencia

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateAlertaRecKm(ByVal rec As Alertas_Recordatorios_Por_Km) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Recordatorios_Por_Km = DB.Alertas_Recordatorios_Por_Kms.Where(Function(d) d.reck_id = rec.reck_id).FirstOrDefault()

                oOriginal.reck_descripcion = rec.reck_descripcion
                oOriginal.reck_kms_primer_alarma = rec.reck_kms_primer_alarma
                oOriginal.reck_kilm_proxima_alarma = rec.reck_kilm_proxima_alarma
                oOriginal.reck_notificar_mail = rec.reck_notificar_mail
                oOriginal.reck_notificar_sms = rec.reck_notificar_sms
                oOriginal.reck_ocurrencia_cada_km = rec.reck_ocurrencia_cada_km

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlarmaConfigurada(ByVal alarma As Alertas_Velocidad_Configuradas) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Velocidad_Configuradas.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlarmaSensor(ByVal alarma As Sensores_Configurados) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Sensores_Configurados.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaRecorridoPunto(ByVal punto As Alertas_Recorrido_Puntos_Visitar) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Recorrido_Puntos_Visitar.InsertOnSubmit(punto)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaRecorrido(ByVal alarma As Alertas_Recorridos) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Recorridos.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaFueraHorarios(ByVal alarma As Alarmas_Fuera_Horario) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alarmas_Fuera_Horario.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaExcesoKms(ByVal alarma As Alamas_Kms_Excedidos) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alamas_Kms_Excedidos.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaInactividad(ByVal alarma As Alarmas_Inactividad) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alarmas_Inactividad.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaInicioActividad(ByVal alarma As Alarma_Inicio_Actividad) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alarma_Inicio_Actividad.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaFueraHorarioDia(ByVal alarma As Alarmas_Fuera_Horario_Dias) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alarmas_Fuera_Horario_Dias.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaInactividadDia(ByVal alarma As Alarmas_Inactividad_Dias) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alarmas_Inactividad_Dias.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaInicioActividadDia(ByVal alarma As Alarmas_Inicio_Actividad_Dias) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alarmas_Inicio_Actividad_Dias.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaRecordatorioKm(ByVal alarma As Alertas_Recordatorios_Por_Km) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Recordatorios_Por_Kms.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaRecordatorioFecha(ByVal alarma As Alertas_Recordatorios_Por_Fechas) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Recordatorios_Por_Fechas.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaDireccion(ByVal direccion As Alertas_Direcciones) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Direcciones.InsertOnSubmit(direccion)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlertaZona(ByVal zona As Alertas_Zonas) As [Boolean]

        Try
            Dim DB As New DataClassesGPSDataContext()
            DB.Alertas_Zonas.InsertOnSubmit(zona)
            DB.SubmitChanges()
            Return True


        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function InsertAlertaDireccionFrecuencia(ByVal frecuencia As Alertas_Direcciones_Frecuencia) As [Boolean]

        Try
            Dim DB As New DataClassesGPSDataContext()
            DB.Alertas_Direcciones_Frecuencia.InsertOnSubmit(frecuencia)
            DB.SubmitChanges()
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function InsertAlertaZonaFrecuencia(ByVal frecuencia As Alertas_Zonas_Frecuencias) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Zonas_Frecuencias.InsertOnSubmit(frecuencia)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function InsertAlertaRecorridoFrecuencia(ByVal frecuencia As Alertas_Recorridos_Frecuencias) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alertas_Recorridos_Frecuencias.InsertOnSubmit(frecuencia)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function InsertInactividadMovil(ByVal alarma As Inactividad_Moviles) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Inactividad_Moviles.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function InsertUsoMovil(ByVal alarma As Uso_Moviles) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Uso_Moviles.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertAlarma(ByVal alarma As Alarmas) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Alarmas.InsertOnSubmit(alarma)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function SelectByMovil(ByVal veh_id As Integer, ByVal vel_id As Integer) As Alertas_Velocidad_Configuradas
        Using DB As New DataClassesGPSDataContext()
            Return DB.Alertas_Velocidad_Configuradas.Where(Function(s) s.veh_id = veh_id And s.vel_id = vel_id).FirstOrDefault()
        End Using
    End Function

    Public Shared Function ListByMovil(ByVal veh_id As Integer) As List(Of Alertas_Velocidad_Configuradas)
        Using DB As New DataClassesGPSDataContext()
            Return DB.Alertas_Velocidad_Configuradas.Where(Function(s) s.veh_id = veh_id).ToList()
        End Using
    End Function

    Public Shared Function SelectByMovil(ByVal veh_id As Integer) As List(Of Alertas_Velocidad_Configuradas)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Velocidad_Configuradas.Where(Function(s) s.veh_id = veh_id).ToList()

    End Function

    Public Shared Function SelectByVelocidad(ByVal veh_id As Integer, ByVal velocidad As Integer, ByVal tipo_via As Integer) As List(Of Alertas_Velocidad_Configuradas)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Velocidad_Configuradas.Where(Function(s) s.veh_id = veh_id And s.Alarmas_Velocidad.vel_tipo_via = tipo_via And s.ale_valor_maximo <= velocidad).ToList()

    End Function


    Public Shared Function SelectAlarmaByVelocidad(ByVal veh_id As Integer, ByVal tipo_via As Integer) As Alertas_Velocidad_Configuradas
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alertas_Velocidad_Configuradas.Where(Function(s) s.veh_id = veh_id And s.Alarmas_Velocidad.vel_tipo_via = tipo_via).FirstOrDefault()

    End Function

    Public Shared Function AlertaRecorridoList(ByVal cli_id As Integer) As List(Of Alertas_Recorridos)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Recorridos _
       Join vehiculo In DB.Vehiculos On alarma.veh_id Equals vehiculo.veh_id _
       Where vehiculo.cli_id = cli_id _
       Select alarma).ToList()

    End Function



    Public Shared Function AlertaDireccionList(ByVal cli_id As Integer) As List(Of Alertas_Direcciones)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Direcciones _
        Where alarma.Vehiculo.cli_id = cli_id _
      Select alarma).ToList()

    End Function

    Public Shared Function AlertaZonaList(ByVal cli_id As Integer) As List(Of Alertas_Zonas)
        Dim DB As New DataClassesGPSDataContext()
        Return (From alarma In DB.Alertas_Zonas _
     Where alarma.Vehiculo.cli_id = cli_id _
     Select alarma).ToList()


    End Function

    Public Shared Function DeleteUsoMovil(ByVal veh_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As Uso_Moviles = DB.Uso_Moviles.Where(Function(b) b.veh_id = veh_id).SingleOrDefault()

                If original IsNot Nothing Then
                    DB.Uso_Moviles.DeleteOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteByMovil(ByVal veh_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As List(Of Alertas_Velocidad_Configuradas) = DB.Alertas_Velocidad_Configuradas.Where(Function(b) b.veh_id = veh_id).ToList()

                If original IsNot Nothing Then
                    DB.Alertas_Velocidad_Configuradas.DeleteAllOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteByMovil(ByVal veh_id As Integer, ByVal subcat_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As Alertas_Velocidad_Configuradas = DB.Alertas_Velocidad_Configuradas.Where(Function(b) b.veh_id = veh_id And b.vel_id = subcat_id).SingleOrDefault()

                If original IsNot Nothing Then
                    DB.Alertas_Velocidad_Configuradas.DeleteOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteByCliente(ByVal cli_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As List(Of Alertas_Velocidad_Configuradas) = DB.Alertas_Velocidad_Configuradas.Where(Function(b) b.Vehiculo.cli_id = cli_id).ToList()

                If original.Count > 0 Then
                    DB.Alertas_Velocidad_Configuradas.DeleteAllOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteSensorByMovil(ByVal veh_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As Sensores_Configurados = DB.Sensores_Configurados.Where(Function(b) b.veh_id = veh_id).SingleOrDefault()

                If original IsNot Nothing Then
                    DB.Sensores_Configurados.DeleteOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteSensorByMovil(ByVal sen_id As Integer, ByVal veh_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As Sensores_Configurados = DB.Sensores_Configurados.Where(Function(b) b.veh_id = veh_id And b.sen_id = sen_id).SingleOrDefault()

                If original IsNot Nothing Then
                    DB.Sensores_Configurados.DeleteOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteById(ByVal ale_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim original As Alertas_Velocidad_Configuradas = DB.Alertas_Velocidad_Configuradas.Where(Function(b) b.ale_id = ale_id).SingleOrDefault()

                If original IsNot Nothing Then
                    DB.Alertas_Velocidad_Configuradas.DeleteOnSubmit(original)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlarmaReportadaByTrama(ByVal veh_id As Integer, ByVal alar_nombre_via As String, ByVal fecha As DateTime) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alarmas As List(Of Alarmas) = DB.Alarmas.Where(Function(b) b.alar_fecha = fecha And b.veh_id = veh_id And b.alar_nombre_via = alar_nombre_via).ToList

                For Each alarma As Alarmas In alarmas
                    DB.Alarmas.DeleteOnSubmit(alarma)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlerta_Recorrido(ByVal arec_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Recorridos = DB.Alertas_Recorridos.SingleOrDefault(Function(d) d.arec_id = arec_id)

                If oOriginal IsNot Nothing Then
                    DB.Alertas_Recorridos_Frecuencias.DeleteAllOnSubmit(oOriginal.Alertas_Recorridos_Frecuencias)
                    DB.Alertas_Recorrido_Puntos_Visitar.DeleteAllOnSubmit(oOriginal.Alertas_Recorrido_Puntos_Visitar)
                    DB.Alertas_Recorridos.DeleteOnSubmit(oOriginal)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function DeleteAlerta_Direccion(ByVal adir_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Direcciones = DB.Alertas_Direcciones.SingleOrDefault(Function(d) d.adir_id = adir_id)

                If oOriginal IsNot Nothing Then
                    DB.Alertas_Direcciones_Frecuencia.DeleteAllOnSubmit(oOriginal.Alertas_Direcciones_Frecuencia)
                    DB.Alertas_Direcciones.DeleteOnSubmit(oOriginal)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlerta_Zona(ByVal azon_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Alertas_Zonas = DB.Alertas_Zonas.SingleOrDefault(Function(d) d.azon_id = azon_id)

                If oOriginal IsNot Nothing Then
                    DB.Alertas_Zonas_Frecuencias.DeleteAllOnSubmit(oOriginal.Alertas_Zonas_Frecuencias)
                    DB.Alertas_Zonas.DeleteOnSubmit(oOriginal)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_FueraHorario_Dias(ByVal alarma As Alarmas_Fuera_Horario) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                For Each frecuencia As Alarmas_Fuera_Horario_Dias In alarma.Alarmas_Fuera_Horario_Dias
                    Dim alerta As Alarmas_Fuera_Horario_Dias = DB.Alarmas_Fuera_Horario_Dias.SingleOrDefault(Function(d) d.alahd_id = frecuencia.alahd_id)
                    DB.Alarmas_Fuera_Horario_Dias.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Inactividad_Dias(ByVal alarma As Alarmas_Inactividad) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                For Each frecuencia As Alarmas_Inactividad_Dias In alarma.Alarmas_Inactividad_Dias
                    Dim alerta As Alarmas_Inactividad_Dias = DB.Alarmas_Inactividad_Dias.SingleOrDefault(Function(d) d.alarid_id = frecuencia.alarid_id)
                    DB.Alarmas_Inactividad_Dias.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_InicioActividad_Dias(ByVal alarma As Alarma_Inicio_Actividad) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                For Each frecuencia As Alarmas_Inicio_Actividad_Dias In alarma.Alarmas_Inicio_Actividad_Dias
                    Dim alerta As Alarmas_Inicio_Actividad_Dias = DB.Alarmas_Inicio_Actividad_Dias.SingleOrDefault(Function(d) d.alaricd_id = frecuencia.alaricd_id)
                    DB.Alarmas_Inicio_Actividad_Dias.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Direcciones_Frecuencia(ByVal direccion As Alertas_Direcciones) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                For Each frecuencia As Alertas_Direcciones_Frecuencia In direccion.Alertas_Direcciones_Frecuencia
                    Dim alerta As Alertas_Direcciones_Frecuencia = DB.Alertas_Direcciones_Frecuencia.SingleOrDefault(Function(d) d.dir_frec_id = frecuencia.dir_frec_id)
                    DB.Alertas_Direcciones_Frecuencia.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Direcciones_Vehiculos(ByVal adir_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alertas_Direcciones = DB.Alertas_Direcciones.SingleOrDefault(Function(d) d.adir_id = adir_id)
                DB.Alertas_Direcciones_Frecuencia.DeleteAllOnSubmit(alerta.Alertas_Direcciones_Frecuencia)
                DB.Alertas_Direcciones.DeleteOnSubmit(alerta)
                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Direcciones_Vehiculos(ByVal adir_id As Integer, ByVal veh_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alertas_Direcciones = DB.Alertas_Direcciones.SingleOrDefault(Function(d) d.adir_id = adir_id And d.veh_id = veh_id)
                DB.Alertas_Direcciones_Frecuencia.DeleteAllOnSubmit(alerta.Alertas_Direcciones_Frecuencia)
                DB.Alertas_Direcciones.DeleteOnSubmit(alerta)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Recorrido_Puntos(ByVal recorrido As Alertas_Recorridos) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                For Each frecuencia As Alertas_Recorrido_Puntos_Visitar In recorrido.Alertas_Recorrido_Puntos_Visitar
                    Dim alerta As Alertas_Recorrido_Puntos_Visitar = DB.Alertas_Recorrido_Puntos_Visitar.SingleOrDefault(Function(d) d.rec_punto_id = frecuencia.rec_punto_id And d.arec_id = frecuencia.arec_id)
                    DB.Alertas_Recorrido_Puntos_Visitar.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Recorrido_Frecuencia(ByVal recorrido As Alertas_Recorridos) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                For Each frecuencia As Alertas_Recorridos_Frecuencias In recorrido.Alertas_Recorridos_Frecuencias
                    Dim alerta As Alertas_Recorridos_Frecuencias = DB.Alertas_Recorridos_Frecuencias.SingleOrDefault(Function(d) d.rec_frec_id = frecuencia.rec_frec_id)
                    DB.Alertas_Recorridos_Frecuencias.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Shared Function DeleteAlertas_Zona_Frecuencia(ByVal zona As Alertas_Zonas) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                For Each frecuencia As Alertas_Zonas_Frecuencias In zona.Alertas_Zonas_Frecuencias
                    Dim alerta As Alertas_Zonas_Frecuencias = DB.Alertas_Zonas_Frecuencias.SingleOrDefault(Function(d) d.zon_frec_id = frecuencia.zon_frec_id)

                    DB.Alertas_Zonas_Frecuencias.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                Next

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Zona_Vehiculos(ByVal zon_id As Integer, ByVal veh_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alertas_Zonas = DB.Alertas_Zonas.SingleOrDefault(Function(d) d.veh_id = veh_id And d.azon_id = zon_id)
                DB.Alertas_Zonas_Frecuencias.DeleteAllOnSubmit(alerta.Alertas_Zonas_Frecuencias)
                DB.Alertas_Zonas.DeleteOnSubmit(alerta)
                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Shared Function DeleteAlertas_Recorrido_Vehiculos(ByVal rec_id As Integer, ByVal veh_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alertas_Recorridos = DB.Alertas_Recorridos.SingleOrDefault(Function(d) d.arec_id = rec_id And d.veh_id = veh_id)
                DB.Alertas_Recorridos_Frecuencias.DeleteAllOnSubmit(alerta.Alertas_Recorridos_Frecuencias)
                DB.Alertas_Recorrido_Puntos_Visitar.DeleteAllOnSubmit(alerta.Alertas_Recorrido_Puntos_Visitar)
                DB.Alertas_Recorridos.DeleteOnSubmit(alerta)
                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Recordatorio_Km(ByVal rec_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alertas_Recordatorios_Por_Km = DB.Alertas_Recordatorios_Por_Kms.SingleOrDefault(Function(d) d.reck_id = rec_id)

                DB.Alertas_Recordatorios_Por_Kms.DeleteOnSubmit(alerta)
                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Recordatorio_Fecha(ByVal rec_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alertas_Recordatorios_Por_Fechas = DB.Alertas_Recordatorios_Por_Fechas.SingleOrDefault(Function(d) d.recf_id = rec_id)

                DB.Alertas_Recordatorios_Por_Fechas.DeleteOnSubmit(alerta)
                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_ExcesoKms(ByVal alah_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alamas_Kms_Excedidos = DB.Alamas_Kms_Excedidos.SingleOrDefault(Function(d) d.alak_id = alah_id)

                DB.Alamas_Kms_Excedidos.DeleteOnSubmit(alerta)
                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Fuera_Horario(ByVal alah_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alarmas_Fuera_Horario = DB.Alarmas_Fuera_Horario.SingleOrDefault(Function(d) d.alah_id = alah_id)

                If alerta IsNot Nothing Then
                    DB.Alarmas_Fuera_Horario_Dias.DeleteAllOnSubmit(alerta.Alarmas_Fuera_Horario_Dias)
                    DB.Alarmas_Fuera_Horario.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteAlertas_Inactividad(ByVal alah_id As Integer) As Boolean
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alarmas_Inactividad = DB.Alarmas_Inactividad.SingleOrDefault(Function(d) d.alari_id = alah_id)

                If alerta IsNot Nothing Then
                    DB.Alarmas_Inactividad_Dias.DeleteAllOnSubmit(alerta.Alarmas_Inactividad_Dias)
                    DB.Alarmas_Inactividad.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function DeleteAlertas_InicioActividad(ByVal alaic_id As Integer) As Boolean

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim alerta As Alarma_Inicio_Actividad = DB.Alarma_Inicio_Actividad.SingleOrDefault(Function(d) d.alaric_id = alaic_id)
                If alerta IsNot Nothing Then
                    DB.Alarmas_Inicio_Actividad_Dias.DeleteAllOnSubmit(alerta.Alarmas_Inicio_Actividad_Dias)
                    DB.Alarma_Inicio_Actividad.DeleteOnSubmit(alerta)
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function





    'proceso que controla si un movil esta fuera de los rangos establecidos para una alerta
    ' Public Shared Sub verificarAlertas(ByVal idRegistro As Integer, ByVal idRegistroAnt As Integer)
    Public Shared Sub verificarAlertas(ByVal idRegistro As Integer, ByVal tramaMemoria As Boolean)


        'la alarma tiene un tipo
        '1 - exceso de velocidad
        ' 2 - sensores
        '3 - direcciones entrada
        '31 direccion salida
        '4 - sale recorrido
        ' 41 entra en recorrido
        ' 5- zonas entrada
        '51 - zona salida
        '6 - recordatorios

         ' busco el movil para saber si tiene alarmas configuradas
        '---------------------------------------------------------------------
        'EL MOVIL PUEDE TENER ALARMAS DE VELOCIDAD, SENSORES, Y RECORDATORIOS
        ' ENTRADA Y SALIDA DE DIRECCION, DESVIOS DE RECORRIDOS  Y DE ZONAS
        ' LOS SENSORES SOLO SI TIENE CONTRATADO ESTE SERVICIO
        '------------------------------------------------------------------------------
        Dim _registro As vMonitoreo = clsVehiculo.SeleccionarPosicion(idRegistro)

        Dim _movil As Vehiculo = clsVehiculo.Seleccionar(_registro.ID_MODULO)

        ' Dim _previusRegistro As vMonitoreos = clsVehiculo.searchPreviusLocationM(_registro.ID_MODULO, idRegistro) ' ' 

        Dim _previusRegistro As vMonitoreo = clsVehiculo.searchPreviusLocation(_registro.ID_MODULO, idRegistro, String.Format("{0:yyyyMMdd HH:mm:ss}", _registro.FECHA))

        'Dim _previusRegistro As vMonitoreos = clsVehiculo.SeleccionarPosicion(idRegistroAnt)

        If _movil IsNot Nothing Then

            Dim envia_sms As Boolean = False

            'alarmas de exceso de velocidad
            ControlarAlarmasdeVelocidad(_movil, _registro, tramaMemoria)


            'Alarmas de sensores - Ya las controla el socket en cada trama que entra

            ' ControlarAlarmasdeSensores(_movil, _registro, tramaMemoria)


            'ALARMA INICIO ACTIVIDAD DIARIA
            '*******************************************
            ControlarAlarmaInicioActividadDiario(_movil, _registro, tramaMemoria)

            'ALAMAS KMS ACUMULADOS
            '******************************************
            ControlarAlarmaExcesosKms(_movil, _registro, tramaMemoria)


            'USO FUERA DE HORARIO
            '**************************************
            ControlarAlarmaUsoFueraHorario(_movil, _registro, tramaMemoria)

            Dim fecha_actual As DateTime = DateTime.Now


            'Alarmas de desvio de zonas, direccion, recorrido
            ' Entrada y salida de direcciones, tengo que controlar el registro anterior de este movil y ver si esta en la misma ubicacion o no
            ' Tengo que ver si estaba en esa direccion antes y ahora ya no, entonces es una salida
            ' Entrada tengo que ver si estaba en otra ubicacion antes y ahora en esta entonces es una entrada
            'Ver si conviene usar para esto un calculo de distancia entre la ubicacion actual y la coordenada de la zona

            '****
            'ZONAS
            ControlarAlarmaEntradaSalidaZona(_movil, _registro, _previusRegistro, tramaMemoria)

            '*************
            'RECORRIDOS

            ControlarAlarmaDesvioRecorridos(_movil, _registro, _previusRegistro, tramaMemoria)

            '************************
            'DIRECCIONES
            'aca tengo que ver la distancia entre el punto actual y la direccion para saber si llega o sale

            '****
            ControlarAlarmaEntradaSalidaDirecciones(_movil, _registro, _previusRegistro, tramaMemoria)

        End If


    End Sub

    Private Shared Sub ControlarAlarmasdeVelocidad(ByVal _movil As Vehiculo, ByVal _registro As vMonitoreo, ByVal tramaMemoria As Boolean)

        'Alarmas de excesos de velocidad
        'voy a buscar las alertas que cumplan con el valor de la velocidad actual asi no las traigo a todas


        Dim Alertas As List(Of Alertas_Velocidad_Configuradas) = SelectByVelocidad(_movil.veh_id, _registro.VELOCIDAD, _registro.TIPO_VIA)

        'si el movil esta en una velocidad dentro de lo normal, verifico si tengo una alarma de exceso anterior y grabo un registro indicando que volvio a la velocidad
        ' y calculo el tiempo que estuvo en exceso

        If Alertas.Count = 0 Then
            Dim _alerta As Alertas_Velocidad_Configuradas = SelectAlarmaByVelocidad(_movil.veh_id, _registro.TIPO_VIA)

            If _alerta IsNot Nothing Then
                Dim _alarmaAnterior As Alarmas = SearchAlarmaSensorAnterior(_movil.veh_id, 1, _alerta.ale_id)

                If _alarmaAnterior IsNot Nothing Then

                    'verifico si no reprote una alarma igual antes

                    If GetAlertasReportadas(_movil.veh_id, _alerta.ale_id, 11).Count = 0 Then
                        Dim _alarma = New Alarmas()
                        _alarma.alar_fecha = _registro.FECHA
                        _alarma.alar_Categoria = _alerta.Alarmas_Velocidad.vel_descripcion
                        _alarma.alar_Localidad = _registro.LOCALIDAD
                        _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                        _alarma.alar_Provincia = _registro.PROVINCIA
                        _alarma.alar_valor = _registro.VELOCIDAD
                        _alarma.veh_id = _movil.veh_id
                        _alarma.veh_patente = _movil.veh_patente
                        _alarma.alar_nombre = "Retorno a Velocidad permitida."
                        _alarma.alar_lat = _registro.LATITUD
                        _alarma.alar_lng = _registro.LONGITUD
                        _alarma.alar_tipo = 11
                        _alarma.alar_vista = False
                        _alarma.alar_vista_admin = False
                        _alarma.veh_conductor = _movil.veh_nombre_conductor
                        _alarma.alar_limite = _alerta.ale_valor_maximo
                        _alarma.alar_mostrar = True
                        _alarma.alar_codigo_config = _alerta.ale_id

                        'actualizo la duracion en exceso de velocidad en minutos
                        Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                        _alarma.alar_duracion = duracion

                        If _alerta.ale_enviar_mail And Not tramaMemoria Then
                            clsFunciones.Send_Mail_Alarma_Clientes("Su movil patente " + _alerta.Vehiculo.veh_patente + ", retorno a la velocidad maxima permitida. Ubicacion actual: " + _registro.NOMBRE_VIA + "," + _registro.LOCALIDAD + "," + _registro.PROVINCIA + ". <br/> El día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm") + ". <br/> El movil desplaza a " + _registro.VELOCIDAD.ToString() + " Kms/h.", _movil.Cliente.cli_email)
                            _alarma.mail_enviado = DateTime.Now
                        End If

                        InsertAlarma(_alarma)
                        If duracion = 0 Then duracion = 1
                        UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                    End If

                End If
            End If

        Else
            For Each _alerta As Alertas_Velocidad_Configuradas In Alertas
                'verifico el tipo de via donde esta el movil y si excedio el valor para este grupo de alertas
                'solo registro la alarma si no registre una igual antes
                'verifico si excedi la velocidad
                If _registro.VELOCIDAD >= _alerta.ale_valor_maximo Then

                    If GetAlertasReportadas(_movil.veh_id, _alerta.ale_id, 1).Count = 0 Then
                        Dim _alarma = New Alarmas()
                        _alarma.alar_fecha = _registro.FECHA
                        _alarma.alar_Categoria = _alerta.Alarmas_Velocidad.vel_descripcion
                        _alarma.alar_Localidad = _registro.LOCALIDAD
                        _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                        _alarma.alar_Provincia = _registro.PROVINCIA
                        _alarma.alar_valor = _registro.VELOCIDAD
                        _alarma.veh_id = _movil.veh_id
                        _alarma.veh_patente = _movil.veh_patente
                        _alarma.alar_nombre = _alerta.Alarmas_Velocidad.vel_descripcion
                        _alarma.alar_lat = _registro.LATITUD
                        _alarma.alar_lng = _registro.LONGITUD
                        _alarma.alar_tipo = 1
                        _alarma.alar_vista = False
                        _alarma.alar_vista_admin = False
                        _alarma.veh_conductor = _movil.veh_nombre_conductor
                        _alarma.alar_limite = _alerta.ale_valor_maximo
                        _alarma.alar_mostrar = True
                        _alarma.alar_codigo_config = _alerta.ale_id
                        _alarma.alar_duracion = 0

                        'verifico si envio mail o sms

                        If _alerta.ale_enviar_mail And Not tramaMemoria Then
                            clsFunciones.Send_Mail_Alarma_Clientes("Su movil patente " + _alerta.Vehiculo.veh_patente + ", excedio la velocidad maxima permitida en la ubicacion: " + _registro.NOMBRE_VIA + "," + _registro.LOCALIDAD + "," + _registro.PROVINCIA + ". <br/> El día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm") + ". <br/> El movil se desplazaba a " + _registro.VELOCIDAD.ToString() + " Kms/h.", _movil.Cliente.cli_email)
                            _alarma.mail_enviado = DateTime.Now
                        End If

                        InsertAlarma(_alarma)

                    End If

                End If

            Next
        End If

    End Sub

    Public Shared Sub ControlarAlarmasdeSensores(ByVal _movil As Vehiculo, ByVal _registro As vMonitoreo, ByVal tramaMemoria As Boolean)
        'Verifico si el cliente tiene el modulo con sensores
        Dim sensor As Sensores
        If _movil.veh_modulo_sensor Then
            'busco los sensores que tiene configurados el cliente
            Dim _sensores As List(Of Sensores_Moviles) = SelectSensoresByMovil(_movil.veh_id)

            If _sensores.Count > 0 Then
                'veo que sensores me llegan
                If _registro.SENSORES <> "" Then

                    'busco las alarmas de Sensores_Clientes configuradas para este sensor y el movil que estoy evaluando
                    For Each _sensorConfigurado As Sensores_Moviles In _sensores

                        sensor = SelectSensor(_sensorConfigurado.sen_id)
                        'Solo traigo los sensores marcados como binarios, y verifico si disparan alarmas
                        If sensor.sen_alarma = True Then
                            'para el caso del sendor de encendido y ocupacion tengo que guardar tambien un true o false en algun campo

                            If _registro.SENSORES(sensor.sen_posicion - 1) = "1" Then
                                'verifico si tengo que reportar alerta por este sensor, traigo solo sensores que reportan alertas
                                Dim AlertasSensores As List(Of Sensores_Configurados) = SelectAlertaSensores(sensor.sen_posicion, _movil.veh_id)

                                For Each _alerta As Sensores_Configurados In AlertasSensores


                                    If GetAlertasReportadas(_movil.veh_id, _alerta.sen_id, 2).Count = 0 Then
                                        Dim _alarmaSensor = New Alarmas()
                                        _alarmaSensor.alar_fecha = _registro.FECHA
                                        _alarmaSensor.alar_Categoria = "Sensores"
                                        _alarmaSensor.alar_Localidad = _registro.LOCALIDAD
                                        _alarmaSensor.alar_nombre_via = _registro.NOMBRE_VIA
                                        _alarmaSensor.alar_Provincia = _registro.PROVINCIA
                                        _alarmaSensor.alar_valor = _registro.VELOCIDAD
                                        _alarmaSensor.veh_id = _movil.veh_id
                                        _alarmaSensor.veh_patente = _movil.veh_patente
                                        _alarmaSensor.alar_nombre = _alerta.Sensores.sen_nombre
                                        _alarmaSensor.alar_lat = _registro.LATITUD
                                        _alarmaSensor.alar_lng = _registro.LONGITUD
                                        _alarmaSensor.alar_tipo = 2
                                        _alarmaSensor.alar_vista = False
                                        _alarmaSensor.alar_vista_admin = False
                                        _alarmaSensor.veh_conductor = _movil.veh_nombre_conductor
                                        _alarmaSensor.alar_mostrar = True
                                        _alarmaSensor.alar_duracion = 0
                                        _alarmaSensor.alar_codigo_config = _alerta.sen_id
                                        'verifico si envio mail o sms

                                        If _alerta.sen_enviar_mail And Not tramaMemoria Then
                                            clsFunciones.Send_Mail_Alarma_Clientes("Su movil patente " + _alerta.Vehiculo.veh_patente + ", Disparo el sensor " + _alerta.Sensores.sen_nombre.ToUpper() + " en la ubicacion: " + _registro.NOMBRE_VIA + "," + _registro.LOCALIDAD + "," + _registro.PROVINCIA + ". <br/> El día " + _alarmaSensor.alar_fecha.ToString("dd/MM/yyyy HH:mm") + ". <br/> El movil se desplazaba a " + _registro.VELOCIDAD.ToString() + " Kms/h.", _movil.Cliente.cli_email)
                                            _alarmaSensor.mail_enviado = DateTime.Now
                                        End If

                                        InsertAlarma(_alarmaSensor)
                                    End If
                                Next
                            Else
                                Dim AlertasSensores As List(Of Sensores_Configurados) = SelectAlertaSensores(sensor.sen_posicion, _movil.veh_id)

                                'verifico si reporto una alarma de sensor antes, para calcular el tiempo que duro

                                If AlertasSensores.Count = 1 Then
                                    Dim _alerta As Sensores_Configurados = AlertaSensorSelect(sensor.sen_id, _movil.veh_id)
                                    If _alerta IsNot Nothing Then

                                        sensor = SelectSensor(_alerta.sen_id)
                                        'solo grabo esto para sensores que tengan que guardar duracion
                                        Dim _alarmaAnterior As Alarmas = SearchAlarmaSensorAnterior(_movil.veh_id, 2, _alerta.sen_id)
                                        If sensor.sen_duracion Then


                                            If _alarmaAnterior IsNot Nothing Then

                                                'verifico si tenia el sensor configurado

                                                If GetAlertasReportadas(_movil.veh_id, _alerta.sen_id, 21).Count = 0 Then 'si lo reporte antes no lo reporto de nuevo

                                                    Dim _alarmaSensor = New Alarmas()
                                                    _alarmaSensor.alar_fecha = _registro.FECHA
                                                    _alarmaSensor.alar_Categoria = "Sensores"
                                                    _alarmaSensor.alar_Localidad = _registro.LOCALIDAD
                                                    _alarmaSensor.alar_nombre_via = _registro.NOMBRE_VIA
                                                    _alarmaSensor.alar_Provincia = _registro.PROVINCIA
                                                    _alarmaSensor.alar_valor = _registro.VELOCIDAD
                                                    _alarmaSensor.veh_id = _movil.veh_id
                                                    _alarmaSensor.veh_patente = _movil.veh_patente
                                                    _alarmaSensor.alar_nombre = "Fin " + sensor.sen_nombre
                                                    _alarmaSensor.alar_lat = _registro.LATITUD
                                                    _alarmaSensor.alar_lng = _registro.LONGITUD
                                                    _alarmaSensor.alar_tipo = 21
                                                    _alarmaSensor.alar_vista = False
                                                    _alarmaSensor.alar_vista_admin = False
                                                    _alarmaSensor.veh_conductor = _movil.veh_nombre_conductor
                                                    _alarmaSensor.alar_mostrar = True
                                                    _alarmaSensor.alar_codigo_config = _alerta.sen_id
                                                    'verifico si envio mail o sms

                                                    If _alerta.sen_enviar_mail And tramaMemoria Then
                                                        clsFunciones.Send_Mail_Alarma_Clientes("Su movil patente " + _alerta.Vehiculo.veh_patente + ", apago el sensor " + _alerta.Sensores.sen_nombre.ToUpper() + " en la ubicacion: " + _registro.NOMBRE_VIA + "," + _registro.LOCALIDAD + "," + _registro.PROVINCIA + ". <br/> El día " + _alarmaSensor.alar_fecha.ToString("dd/MM/yyyy HH:mm") + ". <br/> El movil se desplazaba a " + _registro.VELOCIDAD.ToString() + " Kms/h.", _movil.Cliente.cli_email)
                                                        _alarmaSensor.mail_enviado = DateTime.Now
                                                    End If

                                                    'actualizo la duracion en minutos
                                                    Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                                                    _alarmaSensor.alar_duracion = duracion

                                                    InsertAlarma(_alarmaSensor)
                                                    If duracion = 0 Then duracion = 1
                                                    UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                                                End If
                                            End If

                                        Else
                                            'duracion de la alarma en uno para que no la tome de nuevo
                                            If _alarmaAnterior IsNot Nothing Then UpdateAlarmaDuracion(_alarmaAnterior.alar_id, 1)
                                        End If

                                    End If

                                End If
                            End If
                        End If

                    Next
                End If
            End If

        End If
    End Sub

    Private Shared Sub ControlarAlarmaExcesosKms(ByVal _movil As Vehiculo, _registro As vMonitoreo, ByVal tramaMemoria As Boolean)
        'ver q  no reporte dos veces la semanal y mensual 
        Dim AlertasExcesosKms As List(Of Alamas_Kms_Excedidos) = AlertaExcesoKmsByMovil(_movil.veh_id)
        For Each _alerta In AlertasExcesosKms
            'verifico la frecuencia
            'Diaria sumo los kms recorridos para este dia
            If _alerta.alak_frecuencia = "1" Then
                If clsVehiculo.KmsAcumuladosDiario(_alerta.veh_id, _registro.FECHA) >= _alerta.alak_cant_km Then
                    InsertAlarmaExcesoKms(_registro, _movil, _alerta, "Exceso De Kms Acumulados", 9, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"))
                End If
            Else
                If _alerta.alak_frecuencia = "2" Then
                    'semanal, sumo los kms recorridos para la semana en curso - ver si reporto para esta semana del mes no la vuelvo a grabar, solo si cambio de semana
                    If clsVehiculo.KmsAcumuladosSemanal(_alerta.veh_id, _registro.FECHA.ToString("yyyyMMdd")) >= _alerta.alak_cant_km Then
                        InsertAlarmaExcesoKms(_registro, _movil, _alerta, "Exceso De Kms Acumulados", 9, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"))
                    End If
                Else
                    If _alerta.alak_frecuencia = "3" Then
                        'Mensual, sumo los kms recorridos para este mes
                        If clsVehiculo.KmsAcumuladosMensual(_alerta.veh_id, _registro.FECHA) >= _alerta.alak_cant_km Then
                            InsertAlarmaExcesoKms(_registro, _movil, _alerta, "Exceso De Kms Acumulados", 9, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"))
                        End If

                    End If
                End If
            End If

        Next
    End Sub

    Private Shared Sub ControlarAlarmaInicioActividadDiario(ByVal _movil As Vehiculo, ByVal _registro As vMonitoreo, ByVal tramaMemoria As Boolean)
        'verifico el sensor de encendido y velocidad mayor a 20
        'grabo una sola alarma por dia
        If _registro.VELOCIDAD >= 20 And _registro.SENSORES(3) = "1" Then
            'alarma inicio actividad diaria
            Dim AlertasInicioActividad As List(Of Alarma_Inicio_Actividad) = AlertaInicioActividadByMovil(_movil.veh_id)
            For Each _alerta In AlertasInicioActividad
                'verifico si selecciono dias
                If _alerta.Alarmas_Inicio_Actividad_Dias.Count > 0 Then
                    For Each _frecuencia In _alerta.Alarmas_Inicio_Actividad_Dias
                        If _frecuencia.alaricd_dia_semana = DateTime.Now.DayOfWeek.GetHashCode() Then

                            'comparo las horas
                            If _alerta.alar_horainicio <> "" Then
                                Dim horaInicio As Date, h2 As Date

                                horaInicio = Format((TimeValue(_alerta.alar_horainicio.ToString())), "HH:mm:ss")
                                h2 = Format(_registro.FECHA, "HH:mm:ss")
                                'notificar si empieza antes
                                If _alerta.alaric_inicio_anteshorario Then
                                    If h2 < horaInicio Then
                                        InsertAlarmaInicioActividad(_registro, _movil, _alerta, "Inicio de Actividad", 81, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"), tramaMemoria)
                                    End If
                                Else
                                    If h2 >= horaInicio Then
                                        InsertAlarmaInicioActividad(_registro, _movil, _alerta, "Inicio de Actividad", 8, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"), tramaMemoria)
                                    End If
                                End If

                            Else
                                InsertAlarmaInicioActividad(_registro, _movil, _alerta, "Inicio de Actividad", 8, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"), tramaMemoria)
                            End If

                        End If
                    Next

                Else

                    'si no  puedo que tenga que camparar para todos los dias en el rango horario
                    If _alerta.alar_horainicio <> "" Then
                        Dim horaInicio As Date, h2 As Date

                        horaInicio = Format((TimeValue(_alerta.alar_horainicio.ToString())), "HH:mm:ss")
                        h2 = Format(_registro.FECHA, "HH:mm:ss")

                        'notificar si empieza antes
                        If _alerta.alaric_inicio_anteshorario Then
                            If h2 < horaInicio Then
                                InsertAlarmaInicioActividad(_registro, _movil, _alerta, "Inicio de Actividad", 81, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"), tramaMemoria)
                            End If

                        End If

                        If h2 >= horaInicio Then
                            InsertAlarmaInicioActividad(_registro, _movil, _alerta, "Inicio de Actividad", 8, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"), tramaMemoria)
                        End If

                    End If
                End If
            Next
        End If
    End Sub

    Private Shared Sub ControlarAlarmaUsoFueraHorario(ByVal _movil As Vehiculo, _registro As vMonitoreo, ByVal tramaMemoria As Boolean)
        Dim fecha_actual As DateTime = DateTime.Now
        Dim _fechaDesde As DateTime
        Dim _fechaHasta As DateTime

        'verifico velocidad configurada
        'verifico sensor de encendido

        Dim AlertasFueraHorario As List(Of Alarmas_Fuera_Horario) = AlertaFueraHoraByMovil(_movil.veh_id)
        For Each _alerta In AlertasFueraHorario
            If _registro.SENSORES(3) = "1" Then
                If _registro.VELOCIDAD >= _alerta.alah_velocidad_minima Then
                    If _alerta.alah_fechadesde <> "" Or _alerta.alah_fechahasta <> "" Then
                        If _alerta.alah_fechadesde.ToString().Contains("00:00:00") Then
                            fecha_actual = CDate(DateTime.Now.ToString("dd/MM/yyy") + " 00:00:00")
                        End If
                        'comparo las fechas
                        If _alerta.alah_fechadesde <> "" And _alerta.alah_fechahasta <> "" Then
                            'verifico si tengo hora
                            If _alerta.alah_horadesde <> "" Then
                                _fechaDesde = CDate(_alerta.alah_fechadesde & " " & _alerta.alah_horadesde)
                            Else
                                _fechaDesde = CDate(_alerta.alah_fechadesde & " 00:00")
                            End If

                            If _alerta.alah_horahasta <> "" Then
                                _fechaHasta = CDate(_alerta.alah_fechahasta & " " & _alerta.alah_horahasta)
                            Else
                                _fechaHasta = CDate(_alerta.alah_fechahasta & " 23:59")
                            End If

                            If fecha_actual >= _fechaDesde And fecha_actual <= _fechaHasta Then

                                VerificarUsoFueraHora(_registro, _movil, _alerta, tramaMemoria)
                            Else
                                'dejo de usarlo cuento duracion elimino el registro de uso 
                                FinUsoFueraHora(_movil, _alerta, _registro)

                            End If
                        Else
                            'comparo solo una fecha
                            If _alerta.alah_fechadesde <> "" Then
                                'verifico si tengo hora
                                If _alerta.alah_horadesde <> "" Then
                                    _fechaDesde = CDate(_alerta.alah_fechadesde & " " & _alerta.alah_horadesde)
                                Else
                                    _fechaDesde = CDate(_alerta.alah_fechadesde & " 00:00")
                                End If
                                If fecha_actual >= _alerta.alah_fechadesde Then

                                    VerificarUsoFueraHora(_registro, _movil, _alerta, tramaMemoria)
                                Else
                                    'dejo de usarlo cuento duracion elimino el registro de uso 
                                    FinUsoFueraHora(_movil, _alerta, _registro)
                                End If
                            Else
                                If _alerta.alah_fechahasta <> "" Then
                                    If _alerta.alah_horahasta <> "" Then
                                        _fechaHasta = CDate(_alerta.alah_fechahasta & " " & _alerta.alah_horahasta)
                                    Else
                                        _fechaHasta = CDate(_alerta.alah_fechahasta & " 23:59")
                                    End If
                                    If fecha_actual <= _alerta.alah_fechahasta Then
                                        VerificarUsoFueraHora(_registro, _movil, _alerta, tramaMemoria)
                                    Else
                                        Dim _alarmaAnterior As Alarmas = SearchAnterior(_movil.veh_id, 7, _alerta.alah_id, _registro.FECHA)
                                        'dejo de usarlo cuento duracion elimino el registro de uso 
                                        FinUsoFueraHora(_movil, _alerta, _registro)
                                    End If
                                End If
                            End If
                        End If
                    Else

                        'no tengo fecha especifica, verifico los dias de la semana
                        If _alerta.Alarmas_Fuera_Horario_Dias.Count > 0 Then
                            For Each _frecuencia In _alerta.Alarmas_Fuera_Horario_Dias
                                If _frecuencia.alah_dia_semana = DateTime.Now.DayOfWeek.GetHashCode() Then
                                    'comparo las horas
                                    If _alerta.alah_horadesde <> "" And _alerta.alah_horahasta <> "" Then
                                        Dim h1 As Date, h2 As Date, h3 As Date

                                        h1 = Format((TimeValue(_alerta.alah_horadesde.ToString())), "HH:mm:ss")
                                        h2 = Format((TimeValue(DateTime.Now)), "HH:mm:ss")
                                        h3 = Format((TimeValue(_alerta.alah_horahasta.ToString())), "HH:mm:ss")

                                        If h1 <= h2 And h2 <= h3 Then
                                            VerificarUsoFueraHora(_registro, _movil, _alerta, tramaMemoria)
                                        Else
                                            'dejo de usarlo cuento duracion elimino el registro de uso 
                                            FinUsoFueraHora(_movil, _alerta, _registro)
                                        End If
                                    Else
                                        VerificarUsoFueraHora(_registro, _movil, _alerta, tramaMemoria)
                                    End If

                                End If
                            Next

                        Else

                            'si no tengo ni fecha ni dias, puedo que tenga que camparar para todos los dias en el rango horario
                            If _alerta.alah_horadesde <> "" And _alerta.alah_horahasta <> "" Then
                                Dim h1 As Date, h2 As Date, h3 As Date

                                h1 = Format((TimeValue(_alerta.alah_horadesde.ToString())), "HH:mm:ss")
                                h2 = Format((TimeValue(DateTime.Now)), "HH:mm:ss")
                                h3 = Format((TimeValue(_alerta.alah_horahasta.ToString())), "HH:mm:ss")

                                If h1 <= h2 And h2 <= h3 Then
                                    VerificarUsoFueraHora(_registro, _movil, _alerta, tramaMemoria)
                                Else
                                    'dejo de usarlo cuento duracion elimino el registro de uso 
                                    FinUsoFueraHora(_movil, _alerta, _registro)
                                End If

                            End If
                        End If
                    End If


                End If
            Else
                'esta inactivo
                If _registro.SENSORES(3) = "0" Then
                    'dejo de usarlo cuento duracion elimino el registro de uso 
                    FinUsoFueraHora(_movil, _alerta, _registro)
                End If
            End If

        Next
    End Sub

    Private Shared Sub ControlarAlarmaEntradaSalidaZona(ByVal _movil As Vehiculo, _registro As vMonitoreo, ByVal _previusRegistro As vMonitoreo, ByVal tramaMemoria As Boolean)
        'busco las zonas del cliente que esten activas
        Dim fecha_actual As DateTime = DateTime.Now

        Dim AlertasZonas As List(Of Alertas_Zonas) = AlertaZonaActivaByMovil(_movil.veh_id)
        For Each _alerta In AlertasZonas
            'verifico la frecuencia con la que tengo que evaluar esta zona
            If _alerta.azon_fecha_desde IsNot Nothing Or _alerta.azon_fecha_hasta IsNot Nothing Then
                If _alerta.azon_fecha_desde.ToString().Contains("00:00:00") Then
                    fecha_actual = CDate(DateTime.Now.ToString("dd/MM/yyy") + " 00:00:00")
                End If
                'comparo las fechas
                If _alerta.azon_fecha_desde IsNot Nothing And _alerta.azon_fecha_hasta IsNot Nothing Then

                    If fecha_actual >= _alerta.azon_fecha_desde And fecha_actual <= _alerta.azon_fecha_hasta Then
                        'compruebo zona
                        VerificarZona(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                    End If
                Else
                    'comparo solo una fecha
                    If _alerta.azon_fecha_desde IsNot Nothing Then
                        If fecha_actual >= _alerta.azon_fecha_desde Then
                            'compruebo zona
                            VerificarZona(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                        End If
                    Else
                        If _alerta.azon_fecha_hasta IsNot Nothing Then
                            If fecha_actual <= _alerta.azon_fecha_hasta Then
                                'compruebo zona
                                VerificarZona(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                            End If
                        End If
                    End If
                End If
            Else

                'la hora is nothig la alrma se controla siempre

                If _alerta.Alertas_Zonas_Frecuencias.Count > 0 Then
                    'dias y horas especificos
                    For Each _frecuencia In _alerta.Alertas_Zonas_Frecuencias
                        If _frecuencia.zon_dia_semana = DateTime.Now.DayOfWeek.GetHashCode() Then
                            'comparo las horas
                            Dim h1 As Date, h2 As Date, h3 As Date

                            h1 = Format((TimeValue(_frecuencia.zon_hora_desde.ToString())), "HH:mm:ss")
                            h2 = Format((TimeValue(DateTime.Now)), "HH:mm:ss")
                            h3 = Format((TimeValue(_frecuencia.zon_hora_hasta.ToString())), "HH:mm:ss")

                            If h1 <= h2 And h2 <= h3 Then
                                'compruebo zona
                                VerificarZona(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                            End If
                        End If

                    Next
                Else
                    '24 horas todos los dias
                    'compruebo zona
                    VerificarZona(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                End If
            End If


        Next
    End Sub

    Private Shared Sub ControlarAlarmaDesvioRecorridos(ByVal _movil As Vehiculo, _registro As vMonitoreo, ByVal _previusRegistro As vMonitoreo, ByVal tramaMemoria As Boolean)

        Dim fecha_actual As DateTime = DateTime.Now

        Dim AlertasRecorrido As List(Of Alertas_Recorridos) = AlertaRecorridoActivoByMovil(_movil.veh_id)
        For Each _alerta In AlertasRecorrido
            'verifico la frecuencia con la que tengo que evaluar este recorrido
            If _alerta.arec_fecha_desde IsNot Nothing Or _alerta.arec_fecha_hasta IsNot Nothing Then
                If _alerta.arec_fecha_desde.ToString().Contains("00:00:00") Then
                    fecha_actual = CDate(DateTime.Now.ToString("dd/MM/yyy") + " 00:00:00")
                End If
                'comparo las fechas
                If _alerta.arec_fecha_desde IsNot Nothing And _alerta.arec_fecha_hasta IsNot Nothing Then

                    If fecha_actual >= _alerta.arec_fecha_desde And fecha_actual <= _alerta.arec_fecha_hasta Then
                        'compruebo zona
                        VerificarRecorrido(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                    End If
                Else
                    'comparo solo una fecha
                    If _alerta.arec_fecha_desde IsNot Nothing Then
                        If fecha_actual >= _alerta.arec_fecha_desde Then
                            'compruebo zona
                            VerificarRecorrido(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                        End If
                    Else
                        If _alerta.arec_fecha_hasta IsNot Nothing Then
                            If fecha_actual <= _alerta.arec_fecha_hasta Then
                                'compruebo zona
                                VerificarRecorrido(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                            End If
                        End If
                    End If
                End If
            Else
                If _alerta.Alertas_Recorridos_Frecuencias.Count > 0 Then
                    'dias y horas especificos
                    For Each _frecuencia In _alerta.Alertas_Recorridos_Frecuencias
                        If _frecuencia.rec_dia_semana = DateTime.Now.DayOfWeek.GetHashCode() Then
                            'comparo las horas
                            Dim h1 As Date, h2 As Date, h3 As Date

                            h1 = Format((TimeValue(_frecuencia.rec_hora_desde.ToString())), "HH:mm:ss")
                            h2 = Format((TimeValue(DateTime.Now)), "HH:mm:ss")
                            h3 = Format((TimeValue(_frecuencia.rec_hora_hasta.ToString())), "HH:mm:ss")

                            If h1 <= h2 And h2 <= h3 Then
                                'compruebo zona
                                VerificarRecorrido(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                            End If
                        End If

                    Next
                Else

                    '24 horas todos los dias
                    'compruebo zona
                    VerificarRecorrido(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                End If
            End If

            '********** verifico PUNTOS OBLIGATORIOS
            'busco para esta fecha y hora si tengo punto configurado
            'tengo que verificar que este en ese punto en este momento, sino disparo alarma de que no entro
            'controlo si entra y si sale
            If (_alerta.Alertas_Recorrido_Puntos_Visitar.Count > 0) Then
                For Each _punto As Alertas_Recorrido_Puntos_Visitar In _alerta.Alertas_Recorrido_Puntos_Visitar
                    'verifico si esta en este punto ahora
                    VerificarPuntoObligatorio(_registro, _previusRegistro, _movil, _punto, _alerta, tramaMemoria)

                Next
            End If
        Next

    End Sub

    Private Shared Sub ControlarAlarmaEntradaSalidaDirecciones(ByVal _movil As Vehiculo, _registro As vMonitoreo, ByVal _previusRegistro As vMonitoreo, ByVal tramaMemoria As Boolean)

        Dim fecha_actual As DateTime = DateTime.Now
        'busco las alertas configuradas para las coordenadas actuales, uso el margen de desvio en kms
        'tambien filtro por la fecha y hora actual si es que la alarma tiene frecuencia

        Dim AlertasDirecciones As List(Of Alertas_Direcciones) = AlertaDireccionByMovil(_movil.veh_id)
        For Each _alerta In AlertasDirecciones

            If _alerta.adir_fecha_desde IsNot Nothing Or _alerta.adir_fecha_hasta IsNot Nothing Then
                If _alerta.adir_fecha_desde.ToString().Contains("00:00:00") Then
                    fecha_actual = CDate(DateTime.Now.ToString("dd/MM/yyy") + " 00:00:00")
                End If
                'comparo las fechas
                If _alerta.adir_fecha_desde IsNot Nothing And _alerta.adir_fecha_hasta IsNot Nothing Then

                    If fecha_actual >= _alerta.adir_fecha_desde And fecha_actual <= _alerta.adir_fecha_hasta Then
                        'compruebo direccion
                        VerificarDireccion(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                    End If
                Else
                    'comparo solo una fecha
                    If _alerta.adir_fecha_desde IsNot Nothing Then
                        If fecha_actual >= _alerta.adir_fecha_desde Then
                            'compruebo zona
                            VerificarDireccion(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                        End If
                    Else
                        If _alerta.adir_fecha_hasta IsNot Nothing Then
                            If fecha_actual <= _alerta.adir_fecha_hasta Then
                                'compruebo zona
                                VerificarDireccion(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                            End If
                        End If
                    End If
                End If
            End If

            If _alerta.Alertas_Direcciones_Frecuencia.Count > 0 Then
                'dias y horas especificos
                For Each _frecuencia In _alerta.Alertas_Direcciones_Frecuencia
                    If _frecuencia.dir_dia_semana = DateTime.Now.DayOfWeek.GetHashCode() Then
                        'comparo las horas
                        Dim h1 As Date, h2 As Date, h3 As Date

                        h1 = Format((TimeValue(_frecuencia.dir_frec_hora_desde.ToString())), "HH:mm:ss")
                        h2 = Format((TimeValue(DateTime.Now)), "HH:mm:ss")
                        h3 = Format((TimeValue(_frecuencia.dir_frec_hora_hasta.ToString())), "HH:mm:ss")

                        If h1 <= h2 And h2 <= h3 Then
                            'compruebo zona
                            VerificarDireccion(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
                        End If
                    End If

                Next
            Else
                '24 horas todos los dias
                'compruebo zona
                VerificarDireccion(_registro, _previusRegistro, _movil, _alerta, tramaMemoria)
            End If
        Next
    End Sub

    'seteo la alrma con lso valores de la trama de memoria y la actualizo
    Private Shared Sub refrescarDatosAlarma(ByVal _alarma As Alarmas, ByVal _registro As vMonitoreo)
        _alarma.alar_fecha = _registro.FECHA

        _alarma.alar_Localidad = _registro.LOCALIDAD
        _alarma.alar_nombre_via = _registro.NOMBRE_VIA
        _alarma.alar_Provincia = _registro.PROVINCIA
        _alarma.alar_valor = _registro.VELOCIDAD

        _alarma.alar_lat = _registro.LATITUD
        _alarma.alar_lng = _registro.LONGITUD
        _alarma.alar_tipo = 1
        _alarma.alar_vista = False
        _alarma.alar_vista_admin = False

        _alarma.alar_mostrar = True

        _alarma.alar_duracion = 0

        clsAlarma.UpdateAlarma(_alarma)
    End Sub
    'control solo de sensores

    Public Shared Sub verificarAlarmaSensores(ByVal Tsensores As String, ByVal id_modulo As String)

        Dim _movil As Vehiculo = clsVehiculo.Seleccionar(id_modulo)
        'Alarmas de sensores
        'Verifico si el cliente tiene el modulo con sensores
        'Alarmas de sensores
        'Verifico si el cliente tiene el modulo con sensores

        If _movil IsNot Nothing Then
            If _movil.veh_modulo_sensor Then
                'busco los sensores que tiene configurados el cliente
                Dim _sensores As List(Of Sensores_Moviles) = SelectSensoresByMovil(_movil.veh_id)

                If _sensores.Count > 0 Then
                    'veo que sensores me llegan
                    If Tsensores <> "" Then

                        'busco las alarmas de Sensores_Clientes configuradas para este sensor y el movil que estoy evaluando
                        For Each _sensor As Sensores_Moviles In _sensores

                            'Solo traigo los sensores marcados como binarios, y verifico si disparan alarmas
                            If _sensor.Sensores.sen_alarma = True Then
                                'para el caso del sendor de encendido y ocupacion tengo que guardar tambien un true o false en algun campo

                                If Tsensores(_sensor.Sensores.sen_posicion - 1) = "1" Then
                                    'verifico si tengo que reportar alerta por este sensor, traigo solo sensores que reportan alertas
                                    Dim AlertasSensores As List(Of Sensores_Configurados) = SelectAlertaSensores(_sensor.Sensores.sen_posicion, _movil.veh_id)

                                    For Each _alerta As Sensores_Configurados In AlertasSensores


                                        If GetAlertasReportadas(_movil.veh_id, _alerta.sen_id, 2).Count = 0 Then
                                            Dim _alarmaSensor = New Alarmas()
                                            _alarmaSensor.alar_fecha = DateTime.Now
                                            _alarmaSensor.alar_Categoria = "Sensores"
                                            _alarmaSensor.alar_Localidad = ""
                                            _alarmaSensor.alar_nombre_via = "Sin Datos."
                                            _alarmaSensor.alar_Provincia = ""
                                            _alarmaSensor.alar_valor = 0
                                            _alarmaSensor.veh_id = _movil.veh_id
                                            _alarmaSensor.veh_patente = _movil.veh_patente
                                            _alarmaSensor.alar_nombre = _alerta.Sensores.sen_nombre
                                            _alarmaSensor.alar_lat = "0"
                                            _alarmaSensor.alar_lng = "0"
                                            _alarmaSensor.alar_tipo = 2
                                            _alarmaSensor.alar_vista = False
                                            _alarmaSensor.alar_vista_admin = False
                                            _alarmaSensor.veh_conductor = _movil.veh_nombre_conductor
                                            _alarmaSensor.alar_mostrar = True
                                            _alarmaSensor.alar_duracion = 0
                                            _alarmaSensor.alar_codigo_config = _alerta.sen_id
                                            'verifico si envio mail o sms

                                            If _alerta.sen_enviar_mail Then
                                                clsFunciones.Send_Mail_Alarma_Clientes("Su movil patente " + _alerta.Vehiculo.veh_patente + ", Disparo el sensor " + _alerta.Sensores.sen_nombre.ToUpper() + ". <br/> El día " + _alarmaSensor.alar_fecha.ToString("dd/MM/yyyy HH:mm"), _movil.Cliente.cli_email)
                                                _alarmaSensor.mail_enviado = DateTime.Now
                                            End If

                                            InsertAlarma(_alarmaSensor)
                                        End If

                                    Next
                                Else
                                    Dim AlertasSensores As List(Of Sensores_Configurados) = SelectAlertaSensores(_sensor.Sensores.sen_posicion, _movil.veh_id)

                                    'verifico si reporto una alarma de sensor antes, para calcular el tiempo que duro

                                    If AlertasSensores.Count = 1 Then
                                        Dim _alerta As Sensores_Configurados = AlertaSensorSelect(_sensor.sen_id, _movil.veh_id)
                                        If _alerta IsNot Nothing Then

                                            'solo grabo esto para sensores que tengan que guardar duracion
                                            Dim _alarmaAnterior As Alarmas = SearchAlarmaSensorAnterior(_movil.veh_id, 2, _alerta.sen_id)
                                            If _alerta.Sensores.sen_duracion Then


                                                If _alarmaAnterior IsNot Nothing Then

                                                    'verifico si tenia el sensor configurado
                                                    If GetAlertasReportadas(_movil.veh_id, _alerta.sen_id, 21).Count = 0 Then 'si lo reporte antes no lo reporto de nuevo

                                                        Dim _alarmaSensor = New Alarmas()
                                                        _alarmaSensor.alar_fecha = DateTime.Now
                                                        _alarmaSensor.alar_Categoria = "Sensores"
                                                        _alarmaSensor.alar_Localidad = ""
                                                        _alarmaSensor.alar_nombre_via = "Sin Datos."
                                                        _alarmaSensor.alar_Provincia = ""
                                                        _alarmaSensor.alar_valor = 0
                                                        _alarmaSensor.veh_id = _movil.veh_id
                                                        _alarmaSensor.veh_patente = _movil.veh_patente
                                                        _alarmaSensor.alar_nombre = "Fin " + _alerta.Sensores.sen_nombre
                                                        _alarmaSensor.alar_lat = "0"
                                                        _alarmaSensor.alar_lng = "0"
                                                        _alarmaSensor.alar_tipo = 21
                                                        _alarmaSensor.alar_vista = False
                                                        _alarmaSensor.alar_vista_admin = False
                                                        _alarmaSensor.veh_conductor = _movil.veh_nombre_conductor
                                                        _alarmaSensor.alar_mostrar = True
                                                        _alarmaSensor.alar_codigo_config = _alerta.sen_id
                                                        'verifico si envio mail o sms

                                                        If _alerta.sen_enviar_mail Then
                                                            clsFunciones.Send_Mail_Alarma_Clientes("Su movil patente " + _alerta.Vehiculo.veh_patente + ", apago el sensor " + _alerta.Sensores.sen_nombre.ToUpper() + " <br/> El día " + _alarmaSensor.alar_fecha.ToString("dd/MM/yyyy HH:mm") + ". <br/>", _movil.Cliente.cli_email)
                                                            _alarmaSensor.mail_enviado = DateTime.Now
                                                        End If

                                                        'actualizo la duracion en minutos
                                                        Dim _duracion As Double = (DateTime.Now - _alarmaAnterior.alar_fecha).TotalMinutes

                                                        If (_duracion < 0) Then _duracion = (_alarmaAnterior.alar_fecha - DateTime.Now).TotalMinutes

                                                        _alarmaSensor.alar_duracion = _duracion

                                                        InsertAlarma(_alarmaSensor)
                                                        If _duracion = 0 Then _duracion = 1
                                                        UpdateAlarmaDuracion(_alarmaAnterior.alar_id, _duracion)
                                                    End If

                                                End If
                                            Else
                                                'duracion de la alarma en uno para que no la tome de nuevo
                                                If _alarmaAnterior IsNot Nothing Then UpdateAlarmaDuracion(_alarmaAnterior.alar_id, 1)
                                            End If

                                        End If

                                    End If
                                End If
                            End If

                        Next
                    End If
                End If

            End If
        End If

    End Sub
    ' load  polygon points from the data base
    Public Shared Sub loadData(ByVal puntos_zona As List(Of Zonas_Puntos))

        For Each punto As Zonas_Puntos In puntos_zona
            Dim p As New RouteGeoFence.Point()

            'Convert Latitude into degrees
            Dim Lat As String = punto.zon_latitud.Replace(".", ",")
            ' Dim LatSec As Double = [Double].Parse(Lat.Substring(4, 4)) / 6000
            'Dim LatMin As Double = ([Double].Parse(Lat.Substring(2, 2)) + LatSec) / 60
            p.X = Double.Parse(Lat)

            'Convert Longitude into degrees
            Dim Lng As String = punto.zon_longitud.Replace(".", ",")
            ' Dim LongSec As Double = [Double].Parse(Lng.Substring(5, 4)) / 6000
            'Dim LongMin As Double = ([Double].Parse(Lng.Substring(4, 2)) + LongSec) / 60
            p.Y = Double.Parse(Lng)

            points.Add(p)
        Next

    End Sub



    Public Shared Sub loadDataR(ByVal puntos_recorrido As List(Of Recorridos_Puntos))

        For Each punto As Recorridos_Puntos In puntos_recorrido
            Dim p As New RouteGeoFence.Point()

            'Convert Latitude into degrees
            Dim Lat As String = punto.rec_latitud
            Dim LatSec As Double = [Double].Parse(Lat.Substring(4, 4)) / 6000
            Dim LatMin As Double = ([Double].Parse(Lat.Substring(2, 2)) + LatSec) / 60
            p.X = Double.Parse(Lat.Substring(0, 2)) + LatMin

            'Convert Longitude into degrees
            Dim Lng As String = punto.rec_longitud
            Dim LongSec As Double = [Double].Parse(Lng.Substring(5, 4)) / 6000
            Dim LongMin As Double = ([Double].Parse(Lng.Substring(4, 2)) + LongSec) / 60
            p.Y = Double.Parse(Lng.Substring(0, 3)) + LongMin

            points.Add(p)
        Next

    End Sub


    Public Shared Sub VerificarPuntoObligatorio(ByVal _registro As vMonitoreo, ByVal _previusRegistro As vMonitoreo, ByVal movil As Vehiculo, ByVal punto As Alertas_Recorrido_Puntos_Visitar, ByVal alerta As Alertas_Recorridos, ByVal tramaMemoria As Boolean)
        'verifico si es entrada o salida
        'para saber si aun esta dentro de la zona y no volver a disparar la alarma tengo que verificar la posicion anterior a la actual

        'habia 3 tipos de notificaciones me acuerdo, 
        '1) si pasaba en horario correcto, 
        '2) si no pasaba en el horario configurado y
        ' 3) si pasaba una vez pasado el horario configurado

        Dim distancia As Double = Distance(Decimal.Parse(punto.Recorridos_Puntos.rec_latitud.Replace(".", ",")), Decimal.Parse(punto.Recorridos_Puntos.rec_longitud.Replace(".", ",")), Decimal.Parse(_registro.LATITUD.Replace(".", ",")), Decimal.Parse(_registro.LONGITUD.Replace(".", ",")))
        Dim distanciaAnt As Double = Distance(punto.Recorridos_Puntos.rec_latitud.Replace(".", ","), punto.Recorridos_Puntos.rec_longitud.Replace(".", ","), Decimal.Parse(_previusRegistro.LATITUD.Replace(".", ",")), Decimal.Parse(_previusRegistro.LONGITUD.Replace(".", ",")))
        'tengo que ver la posicion anterior para saber si esta en esa dir y sale o si no estaba y llega
        Dim _umbral As Double = Double.Parse(alerta.arec_umbral_desvio.Replace(".", ","))
        Dim diferencia As Long = 0
        'verifico el umbral - 'esta dentro ahora, y antes no estaba afuera disparo la alarma
        Dim nombreDir As String = ""
        If (alerta.Recorridos.rec_nombre.Length > 30) Then
            nombreDir = alerta.Recorridos.rec_nombre.Substring(0, 30)
        Else
            nombreDir = alerta.Recorridos.rec_nombre
        End If

        If punto.rec_punto_fecha_llegada <> "" Then
            'verifico si esta en ese punto y despues el rango de hora para saber si llego a tiempo o no
            If _registro.FECHA >= CDate(punto.rec_punto_fecha_llegada & " " & punto.rec_punto_horario_desde) And _registro.FECHA <= CDate(punto.rec_punto_fecha_llegada & " " & punto.rec_punto_horario_hasta) Then
                'verifico si esta en este punto ahora
                If (distanciaAnt > _umbral And distancia <= _umbral) Then

                    Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, punto.rec_punto_id, 12, "Llegada a Punto Obligatorio " + nombreDir, alerta.arec_enviar_mail)

                    ' If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                    'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de Llegada a Punto Obligatorio en el recorrido: " & nombreDir & ", el día " & _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    'End If

                    Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)
                    If _alarmaAnterior IsNot Nothing And inserto Then
                        Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                        UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                    End If
                Else
                    'no estaba adentro antes ni ahora, no esta en el punto
                    If (distanciaAnt > _umbral And distancia > _umbral) Then
                        If _registro.FECHA > CDate(punto.rec_punto_fecha_llegada & " " & punto.rec_punto_horario_hasta) Then
                            'verifico si se salio del rango 
                            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 12, punto.rec_punto_id, _registro.FECHA)
                            If _alarmaAnterior Is Nothing Then
                                Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, punto.rec_punto_id, 14, "Punto No Visitado " & nombreDir, alerta.arec_enviar_mail)

                                '  If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                                'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de  Punto No Visitado: " & nombreDir & ", el día " & _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                                'End If
                            End If
                        End If
                    Else
                        'verifico si estaba adentro antes y ahora esta afuera disparo una alarma de salida y grabo duracion
                        If (distanciaAnt <= _umbral And distancia > _umbral) Then
                            'si tengo una alarma de antes actualizo la duracion
                            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)

                            If _alarmaAnterior IsNot Nothing Then
                                Dim _alerta As Alertas_Direcciones = SelectAlertaDireccionById(_alarmaAnterior.alar_codigo_config)
                                Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                                UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                            End If
                        End If
                    End If
                End If

            Else
                '    'no esta en el rango llegada al punto fuera de hora
                If _registro.FECHA > CDate(punto.rec_punto_fecha_llegada & " " & punto.rec_punto_horario_hasta) Then
                    If (distanciaAnt > _umbral And distancia <= _umbral) Then
                        diferencia = DateDiff(DateInterval.Minute, _registro.FECHA, CDate(punto.rec_punto_fecha_llegada & " " & punto.rec_punto_horario_hasta))
                        Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, punto.rec_punto_id, 12, "Llegada Fuera Horario a Punto " & nombreDir, alerta.arec_enviar_mail)
                        '        'si tengo una alarma de antes actualizo la duracion
                        ' If alerta.arec_enviar_mail And inserto Then
                        'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de Llegada Fuera Horario a Punto " & punto.rec_punto_direccion & ", Retraso: " & diferencia.ToString() & " min." & nombreDir & ", el día " & _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                        ' End If
                        Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)
                        If _alarmaAnterior IsNot Nothing And inserto And Not tramaMemoria Then

                            Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                            UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                        End If

                    Else
                        '            'no estaba adentro antes ni ahora, no esta en el punto
                        If (distanciaAnt > _umbral And distancia > _umbral) Then
                            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 12, punto.rec_punto_id, _registro.FECHA)
                            If _alarmaAnterior Is Nothing Then
                                Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, punto.rec_punto_id, 14, "Punto Obligatorio No visitado " & nombreDir, alerta.arec_enviar_mail)
                                ' If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                                'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de Punto Obligatorio No visitado: " & nombreDir & ", el día " & _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                                ' End If
                            End If
                        Else
                            'verifico si estaba adentro antes y ahora esta afuera disparo una alarma de salida y grabo duracion
                            If (distanciaAnt <= _umbral And distancia > _umbral) Then
                                'si tengo una alarma de antes actualizo la duracion
                                Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)

                                If _alarmaAnterior IsNot Nothing Then
                                    Dim _alerta As Alertas_Direcciones = SelectAlertaDireccionById(_alarmaAnterior.alar_codigo_config)
                                    Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                                    UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                                End If
                            End If
                    End If

                    End If
            End If
            End If
        Else
            'verifico todo los dias para el horario configurado
            If _registro.FECHA >= CDate(DateTime.Now.ToString("dd/MM/yyyy") & " " & punto.rec_punto_horario_desde) And _registro.FECHA <= CDate(DateTime.Now.ToString("dd/MM/yyyy") & " " & punto.rec_punto_horario_hasta) Then

                If (distanciaAnt > _umbral And distancia <= _umbral) Then
                    Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, punto.rec_punto_id, 12, "Llegada a Punto  Obligatorio " & nombreDir, alerta.arec_enviar_mail)
                    'If inserto Then
                    'If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                    'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de Llegada a Punto  Obligatorio : " & nombreDir & ", el día " & _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    ' End If

                    ' End If
                    Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)
                    If _alarmaAnterior IsNot Nothing And inserto And Not tramaMemoria Then

                        Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                        UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                    End If

                Else
                    'no estaba adentro antes ni ahora, no esta en el punto
                    If (distanciaAnt > _umbral And distancia > _umbral) Then
                        'verifico si se salio del rango 
                        If _registro.FECHA > CDate(DateTime.Now.ToString("dd/MM/yyyy") & " " & punto.rec_punto_horario_hasta) Then
                            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 12, punto.rec_punto_id, _registro.FECHA)
                            If _alarmaAnterior Is Nothing Then
                                InsertAlarmaPuntoObligatorio(_registro, movil, alerta, "Punto No Visitado " & punto.rec_punto_direccion, 14, punto.rec_punto_id)
                            End If
                        End If
                    Else
                        'verifico si estaba adentro antes y ahora esta afuera disparo una alarma de salida y grabo duracion
                        If (distanciaAnt <= _umbral And distancia > _umbral) Then
                            'si tengo una alarma de antes actualizo la duracion
                            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)

                            If _alarmaAnterior IsNot Nothing Then
                                Dim _alerta As Alertas_Direcciones = SelectAlertaDireccionById(_alarmaAnterior.alar_codigo_config)
                                Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                                UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                            End If
                        End If
                    End If

                End If

            Else
                '    'no esta en el rango llegada al punto fuera de hora
                If _registro.FECHA > CDate(DateTime.Now.ToString("dd/MM/yyyy") & " " & punto.rec_punto_horario_desde) And _registro.FECHA > CDate(DateTime.Now.ToString("dd/MM/yyyy") & " " & punto.rec_punto_horario_hasta) Then
                    If (distanciaAnt > _umbral And distancia <= _umbral) Then
                        diferencia = DateDiff(DateInterval.Minute, _registro.FECHA, CDate(DateTime.Now.ToString("dd/MM/yyyy") & " " & punto.rec_punto_horario_hasta))
                        Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, punto.rec_punto_id, 12, "Llegada Fuera Horario a Punto " & nombreDir, alerta.arec_enviar_mail)
                        If alerta.arec_enviar_mail And inserto Then
                            clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de Llegada Fuera Horario a Punto " & punto.rec_punto_direccion & ", Retraso: " & diferencia.ToString() & " min." & nombreDir & ", el día " & _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                        End If
                        '        'si tengo una alarma de antes actualizo la duracion
                        Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)
                        If _alarmaAnterior IsNot Nothing And inserto And Not tramaMemoria Then

                            Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                            UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                        End If
                    Else
                        '            'no estaba adentro antes ni ahora, no esta en el punto
                        If (distanciaAnt > _umbral And distancia > _umbral) Then
                            'si reporte ya una alrma de que visito este punto no lo reporto
                            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 12, punto.rec_punto_id, _registro.FECHA)
                            If _alarmaAnterior IsNot Nothing Then
                                Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, punto.rec_punto_id, 12, "Punto No Visitado " & nombreDir, alerta.arec_enviar_mail)
                                ''If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                                'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de Punto No Visitado " & nombreDir & ", el día " & _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                                'End If
                            End If

                        Else
                            'verifico si estaba adentro antes y ahora esta afuera disparo una alarma de salida y grabo duracion
                            If (distanciaAnt <= _umbral And distancia > _umbral) Then
                                'si tengo una alarma de antes actualizo la duracion
                                Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 14, punto.rec_punto_id, _registro.FECHA)

                                If _alarmaAnterior IsNot Nothing Then
                                    Dim _alerta As Alertas_Direcciones = SelectAlertaDireccionById(_alarmaAnterior.alar_codigo_config)
                                    Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                                    UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                                End If
                            End If
                    End If

                    End If
                End If
        End If
        End If

    End Sub


    Public Shared Sub VerificarDireccion(ByVal _registro As vMonitoreo, ByVal _previusRegistro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Direcciones, ByVal tramaMemoria As Boolean)
        'verifico si es entrada o salida
        'para saber si aun esta dentro de la zona y no volver a disparar la alarma tengo que verificar la posicion anterior a la actual
        Dim nombreDir As String = ""
        If (alerta.Direcciones.dir_direccion.Length > 30) Then
            nombreDir = alerta.Direcciones.dir_direccion.Substring(0, 30)
        Else
            nombreDir = alerta.Direcciones.dir_direccion
        End If

        Dim distancia As Double = Distance(Decimal.Parse(alerta.Direcciones.dir_latitud.Replace(".", ",")), Decimal.Parse(alerta.Direcciones.dir_longitud.Replace(".", ",")), Decimal.Parse(_registro.LATITUD.Replace(".", ",")), Decimal.Parse(_registro.LONGITUD.Replace(".", ",")))
        Dim distanciaAnt As Double = Distance(alerta.Direcciones.dir_latitud.Replace(".", ","), alerta.Direcciones.dir_longitud.Replace(".", ","), Decimal.Parse(_previusRegistro.LATITUD.Replace(".", ",")), Decimal.Parse(_previusRegistro.LONGITUD.Replace(".", ",")))
        'tengo que ver la posicion anterior para saber si esta en esa dir y sale o si no estaba y llega

        Dim _umbral As Double = Double.Parse(alerta.adir_umbral_desvio.Replace(".", ","))
        'verifico el umbral - 'esta dentro ahora, y antes no estaba afuera disparo la alarma
        If (distanciaAnt > _umbral And distancia <= _umbral) Then

            'si tengo una alarma de antes actualizo la duracion
            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 31, alerta.adir_id, _registro.FECHA)
            Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.adir_id, 3, "Entrada a la Direccion: " & nombreDir, alerta.adir_enviar_mail)

            'If alerta.adir_enviar_mail And inserto And Not tramaMemoria Then
            'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Entrada a la Direccion: " + alerta.Direcciones.dir_direccion.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
            'End If

            If _alarmaAnterior IsNot Nothing And inserto Then

                Dim _alerta As Alertas_Direcciones = SelectAlertaDireccionById(_alarmaAnterior.alar_codigo_config)
                Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                'si no tengo alarma anterior inserto la alarma de entrada
                ' InsertAlarmaDir(enMemoria, _registro, movil, alerta, "Entrada a Dirección", 3)
            End If
        Else
            'verifico si estaba adentro antes y ahora esta afuera disparo una alarma y grabo duracion
            If (distanciaAnt <= _umbral And distancia > _umbral) Then
                'si tengo una alarma de antes actualizo la duracion
                Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 3, alerta.adir_id, _registro.FECHA)
                Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.adir_id, 31, "Salida de Direccion: " & nombreDir, alerta.adir_enviar_mail)
                ' If alerta.adir_enviar_mail And inserto And Not tramaMemoria Then
                'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Salida de Direccion: " + alerta.Direcciones.dir_direccion.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                'End If

                If _alarmaAnterior IsNot Nothing And inserto Then
                    Dim _alerta As Alertas_Direcciones = SelectAlertaDireccionById(_alarmaAnterior.alar_codigo_config)
                    Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)

                    UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                End If
        End If


        End If



    End Sub

    Public Shared Sub FinInactividad(ByVal movil As Vehiculo, ByVal alerta As Alarmas_Inactividad, _registro As vMonitoreo)
        Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 10, alerta.alari_id, _registro.FECHA)
        If _alarmaAnterior IsNot Nothing Then
            'inserto alarma de dejo de usarlo
            Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
            UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

            'reporto alamra de fin
            Dim _alarma As New Alarmas()
            _alarma.alar_fecha = DateTime.Now
            _alarma.alar_Categoria = "Inactividad"
            _alarma.alar_Localidad = _registro.LOCALIDAD
            _alarma.alar_nombre_via = _registro.NOMBRE_VIA
            _alarma.alar_Provincia = _registro.PROVINCIA
            _alarma.alar_valor = _registro.VELOCIDAD
            _alarma.veh_id = movil.veh_id
            _alarma.veh_patente = movil.veh_patente
            _alarma.alar_nombre = "Retono a la Actividad"
            _alarma.alar_lat = _registro.LATITUD
            _alarma.alar_lng = _registro.LONGITUD
            _alarma.alar_tipo = 11
            _alarma.alar_vista = False
            _alarma.alar_vista_admin = False
            _alarma.veh_conductor = movil.veh_nombre_conductor
            _alarma.alar_mostrar = True
            _alarma.alar_codigo_config = alerta.alari_id
            _alarma.alar_duracion = 1
            'verifico si envio mail o sms

            If alerta.alari_enviar_mail Then
                clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Retorno a la Actividad el día " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                _alarma.mail_enviado = DateTime.Now
            End If

            InsertAlarma(_alarma)

        End If


        DeleteRegistroInactividadMovil(movil.veh_id, alerta.alari_id)
    End Sub

    Public Shared Sub FinUsoFueraHora(ByVal movil As Vehiculo, ByVal alerta As Alarmas_Fuera_Horario, _registro As vMonitoreo)
        Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 7, alerta.alah_id, _registro.FECHA)
        If _alarmaAnterior IsNot Nothing Then
            'inserto alarma de dejo de usarlo
            Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
            UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

            'reporto alamra de fin

            Dim _alarma As New Alarmas()
            _alarma.alar_fecha = CDate(_registro.FECHA)
            _alarma.alar_Categoria = "Uso Fuera de Horario"
            _alarma.alar_Localidad = _registro.LOCALIDAD
            _alarma.alar_nombre_via = _registro.NOMBRE_VIA
            _alarma.alar_Provincia = _registro.PROVINCIA
            _alarma.alar_valor = _registro.VELOCIDAD
            _alarma.veh_id = movil.veh_id
            _alarma.veh_patente = movil.veh_patente
            _alarma.alar_nombre = "Fin Uso Fuera de Hora"
            _alarma.alar_lat = _registro.LATITUD
            _alarma.alar_lng = _registro.LONGITUD
            _alarma.alar_tipo = 71
            _alarma.alar_vista = False
            _alarma.alar_vista_admin = False
            _alarma.veh_conductor = movil.veh_nombre_conductor
            _alarma.alar_mostrar = True
            _alarma.alar_codigo_config = alerta.alah_id
            _alarma.alar_duracion = 0
            'verifico si envio mail o sms

            If alerta.alah_enviar_mail Then
                clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Fin Uso Fuera de Horario el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                _alarma.mail_enviado = DateTime.Now
            End If

            InsertAlarma(_alarma)
            DeleteRegistroUsoMovil(movil.veh_id)
        End If


    End Sub

    Public Shared Sub VerificarUsoFueraHora(ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alarmas_Fuera_Horario, ByVal tramaMemoria As Boolean)

        'verifico velocidad y tiempo
        Dim nombreAlarma As String = ""
        If (alerta.alah_descripcion.Length > 30) Then
            nombreAlarma = alerta.alah_descripcion.Substring(0, 30)
        Else
            nombreAlarma = alerta.alah_descripcion
        End If

        If alerta.alah_tiempo_minimo <> 0 Then
            'tengo que ver si ya grabe un registro para este movil en la tabla de uso de moviles, si es asi tengo que ir contando el tiempo

            Dim _usomovil As Uso_Moviles = SelectRegistroUsoMovil(movil.veh_id)
            If _usomovil IsNot Nothing Then
                Dim tiempoUso As Integer = 0

                tiempoUso = (_registro.FECHA - _usomovil.uso_fecha_inicio).TotalMinutes
                If tiempoUso >= alerta.alah_tiempo_minimo Then
                    Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.alah_id, 7, " Uso Fuera de Horario " & nombreAlarma, alerta.alah_enviar_mail)
                    'If alerta.alah_enviar_mail And inserto And Not tramaMemoria Then
                    'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Uso Fuera de Horario, para la alarma Uso Fuera de Horario configurada con el nombre " + alerta.alah_descripcion.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    'End If

                End If

            Else
                'sino agrego un registro y empiezo a contar.
                _usomovil = New Uso_Moviles()
                _usomovil.uso_fecha_inicio = _registro.FECHA
                _usomovil.veh_id = movil.veh_id

                InsertUsoMovil(_usomovil)
            End If

        Else

            'disparo alarma uso fuera de horario
            Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.alah_id, 7, " Uso Fuera de Horario " & nombreAlarma, alerta.alah_enviar_mail)
            ' If alerta.alah_enviar_mail And inserto And Not tramaMemoria Then
            'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Uso Fuera de Horario, para la alarma Uso Fuera de Horario configurada con el nombre " + alerta.alah_descripcion.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
            'End If
        End If



    End Sub

    Public Shared Sub VerificarInactividad(ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alarmas_Inactividad, ByVal tramaMemoria As Boolean)


        If alerta.alari_tiempo_minimo <> 0 Then
            'tengo que ver si ya grabe un registro para este movil en la tabla de uso de moviles, si es asi tengo que ir contando el tiempo
            'por movil y id de alarma
            Dim inactividad As Inactividad_Moviles = SelectRegistroInactividadMovil(movil.veh_id, alerta.alari_id)

            If inactividad IsNot Nothing Then
                Dim tiempoIactivo As Integer = 0

                tiempoIactivo = (DateTime.Now - inactividad.inc_fecha_inicio).TotalMinutes
                If tiempoIactivo >= alerta.alari_tiempo_minimo Then
                    '  InsertReporteAlarma(_registro, movil, alerta.alari_id, 10, "Inactividad por más de " + alerta.alari_tiempo_minimo.ToString() + "'", alerta.alari_enviar_mail)
                    InsertAlarmaInactividad(_registro, movil, alerta, "Inactividad por más de " + alerta.alari_tiempo_minimo.ToString() + "'", 10, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), tramaMemoria)
                End If
            Else
                'creo el registro
                inactividad = New Inactividad_Moviles()

                inactividad.inc_fecha_inicio = _registro.FECHA
                inactividad.veh_id = _registro.ID_VEHICULO
                inactividad.alar_id = alerta.alari_id
                InsertInactividadMovil(inactividad)
            End If

        Else

            'disparo alarma uso fuera de horario
            InsertAlarmaInactividad(_registro, movil, alerta, "Inactividad", 10, _registro.FECHA.ToString("dd/MM/yyyy HH:mm:ss"), tramaMemoria)

        End If



    End Sub

    Public Shared Sub VerificarZona(ByVal _registro As vMonitoreo, ByVal _previusRegistro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Zonas, ByVal tramaMemoria As Boolean)

        'loadData(alerta.Zonas.Zonas_Puntos.ToList())


        Dim myRoute As New RouteGeoFence.PolyGon(points)
        Dim _distanciAnt As Boolean = False
        Dim _distanciaAct As Boolean = False
        Dim _enzona As Boolean = False
        Dim _enzonaPrev As Boolean = False
        Dim nombrezona As String = ""
        ' Dim stat As Boolean = myRoute.FindPoint(Double.Parse(_registro.LATITUD.Replace(".", ",")), Double.Parse(_registro.LONGITUD.Replace(".", ","))) ' true esta en el poligono
        ' Dim prevStat As Boolean = myRoute.FindPoint(Double.Parse(_previusRegistro.LATITUD.Replace(".", ",")), Double.Parse(_previusRegistro.LONGITUD.Replace(".", ",")))

        'nueva verificacion con la formula por metodo radial
        Dim stat As Boolean = isPointZonaInPolygonExcel(alerta.Zonas.Zonas_Puntos.ToList(), Double.Parse(_registro.LATITUD.Replace(".", ",")), Double.Parse(_registro.LONGITUD.Replace(".", ",")))
        Dim prevStat As Boolean = isPointZonaInPolygonExcel(alerta.Zonas.Zonas_Puntos.ToList(), Double.Parse(_previusRegistro.LATITUD.Replace(".", ",")), Double.Parse(_previusRegistro.LONGITUD.Replace(".", ",")))

        ' _distanciAnt = distanciaZona(alerta.Zonas.Zonas_Puntos.ToList(), Double.Parse(alerta.azon_umbral_desvio.Replace(".", ",")), _previusRegistro.LATITUD, _previusRegistro.LONGITUD)
        ' _distanciaAct = distanciaZona(alerta.Zonas.Zonas_Puntos.ToList(), Double.Parse(alerta.azon_umbral_desvio.Replace(".", ",")), _registro.LATITUD, _registro.LONGITUD)

        'no esta dentro de la zona veo los segmentos
        If Not stat Then
            _enzona = EnZona(alerta.Zonas.Zonas_Puntos.ToList(), Double.Parse(alerta.azon_umbral_desvio.Replace(".", ",")), _registro.LATITUD, _registro.LONGITUD)
        Else
            _enzona = stat
        End If

        'no esta dentro de la zona antes, veo los segmentos
        If Not prevStat Then
            _enzonaPrev = EnZona(alerta.Zonas.Zonas_Puntos.ToList(), Double.Parse(alerta.azon_umbral_desvio.Replace(".", ",")), _previusRegistro.LATITUD, _previusRegistro.LONGITUD)
        Else
            _enzonaPrev = prevStat
        End If

        If (alerta.Zonas.zon_nombre.Length > 30) Then
            nombrezona = alerta.Zonas.zon_nombre.Substring(0, 30)
        Else
            nombrezona = alerta.Zonas.zon_nombre
        End If

        'ESTA EN LA ZONA
        '-- 
        If _enzona And Not _enzonaPrev Then 'antes estaba afuera ahora estoy adentro
            'si estoy en el borde del lado de afuera de la zona me va a decir que estoy adentro, tengoq ue ver la distancia actual si esta dentro del umbral


            Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.azon_id, 5, "Entrada a Zona " & nombrezona, alerta.azon_enviar_mail)
           

            Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 51, alerta.azon_id, _registro.FECHA)
            If _alarmaAnterior IsNot Nothing And inserto Then
                'si tengo una alarma de salida de zona anterior cuento el tiempo
                Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
            End If

        Else
            'no esta en la zona
            If Not _enzona And _enzonaPrev Then
                Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.azon_id, 51, "Salida de Zona " & nombrezona, alerta.azon_enviar_mail)
                'verifico si envio mail o sms

                ' If alerta.azon_enviar_mail And inserto And Not tramaMemoria Then
                'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Salida de Zona " + nombrezona + "  ,para la Zona configurada con el nombre " + alerta.Zonas.zon_nombre.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                'End If
                'si tengo una alarma de entrada de zona anterior cuento el tiempo
                Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 5, alerta.azon_id, _registro.FECHA)

                If _alarmaAnterior IsNot Nothing And inserto Then

                    Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                    UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                End If

        End If

        End If

    End Sub

    Public Shared Sub VerificarRecorrido(ByVal _registro As vMonitoreo, ByVal _previusRegistro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Recorridos, ByVal tramaMemoria As Boolean)
        '  loadDataR(alerta.Recorridos.Recorridos_Puntos.ToList())
        ' Dim myRoute As New RouteGeoFence.PolyGon(points)
        ' Dim stat As Boolean = myRoute.FindPoint([Double].Parse(_registro.LATITUD), [Double].Parse(_registro.LONGITUD)) ' true esta en el poligono
        ' Dim prevStat As Boolean = myRoute.FindPoint([Double].Parse(_previusRegistro.LATITUD), [Double].Parse(_previusRegistro.LONGITUD))

        Dim _enRecorrido As Boolean = False
        Dim _enRecorridoPrev As Boolean = False
        Dim nombreRecorrido As String = ""


        If (alerta.Recorridos.rec_nombre.Length > 30) Then
            nombreRecorrido = alerta.Recorridos.rec_nombre.Substring(0, 30)
        Else
            nombreRecorrido = alerta.Recorridos.rec_nombre
        End If

        'nueva verificacion con la formula por metodo radial
        '  Dim stat As Boolean = isPointRecInPolygonExcel(alerta.Recorridos.Recorridos_Puntos.ToList(), Double.Parse(_registro.LATITUD.Replace(".", ",")), Double.Parse(_registro.LONGITUD.Replace(".", ",")))
        '  Dim prevStat As Boolean = isPointRecInPolygonExcel(alerta.Recorridos.Recorridos_Puntos.ToList(), Double.Parse(_previusRegistro.LATITUD.Replace(".", ",")), Double.Parse(_previusRegistro.LONGITUD.Replace(".", ",")))

        '*****
        'Solo voy a usar el calculo por distancias
        '**************
        'si no esta dentro del recorrido verifico los segmentos
        'If Not stat Then
        _enRecorrido = EnRecorrido(alerta.Recorridos.Recorridos_Puntos.ToList(), Double.Parse(alerta.arec_umbral_desvio.Replace(".", ",")), _registro.LATITUD, _registro.LONGITUD)
        'Else
        '_enRecorrido = stat
        'End If

        'If Not prevStat Then
        _enRecorridoPrev = EnRecorrido(alerta.Recorridos.Recorridos_Puntos.ToList(), Double.Parse(alerta.arec_umbral_desvio.Replace(".", ",")), _previusRegistro.LATITUD, _previusRegistro.LONGITUD)
        'Else
        '_enRecorridoPrev = prevStat
        'End If


        'SALE DEL RECORRIDO
        If Not alerta.arec_no_deseado Then
            'sale
            If Not _enRecorrido And _enRecorridoPrev Then 'esta fuera ahora y antes estaba adentro disparo la alarma

                Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 61, alerta.arec_id, _registro.FECHA)

                Dim insert As Boolean = InsertReporteAlarma(_registro, movil, alerta.arec_id, 6, "Desvio de Recorrido: " & nombreRecorrido, alerta.arec_enviar_mail)

                ' If alerta.arec_enviar_mail And insert And Not tramaMemoria Then
                'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Desvio de Recorrido,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                ' End If

                If _alarmaAnterior IsNot Nothing And insert Then

                    Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                    UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                End If
            Else

                If _enRecorrido And Not _enRecorridoPrev Then
                    'ENTRA
                    Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 6, alerta.arec_id, _registro.FECHA)
                    Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.arec_id, 61, "Retorno al Recorrido " & nombreRecorrido, alerta.arec_enviar_mail)

                    'If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                    'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Retorno al Recorrido ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    'End If
                    If _alarmaAnterior IsNot Nothing And inserto Then

                        Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                        UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)
                    End If
            End If

            End If
        Else 'RECORRIDO NO DESEADO
            'entra
            If _enRecorrido And Not _enRecorridoPrev Then 'esta adentro ahora y antes estaba afuera
                Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 63, alerta.rec_id, _registro.FECHA)

                Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.rec_id, 63, "Entrada a Recorrido No Deseado " & nombreRecorrido, alerta.arec_enviar_mail)

                If _alarmaAnterior IsNot Nothing And inserto Then
                    Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                    UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                    ' If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                    'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Entrada a Recorrido No Deseado ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    'End If
                End If

            Else
                If Not _enRecorrido And _enRecorridoPrev Then 'estaba adentro y ahora esta afuera
                    'SALE
                    Dim _alarmaAnterior As Alarmas = SearchAnterior(movil.veh_id, 62, alerta.rec_id, _registro.FECHA)
                    Dim inserto As Boolean = InsertReporteAlarma(_registro, movil, alerta.rec_id, 63, "Salida de Recorrido No Deseado " & nombreRecorrido, alerta.arec_enviar_mail)

                    ' If alerta.arec_enviar_mail And inserto And Not tramaMemoria Then
                    'clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma Salida de Recorrido No Deseado ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    ' End If

                    If _alarmaAnterior IsNot Nothing Then
                        Dim duracion As Long = DateDiff(DateInterval.Second, _alarmaAnterior.alar_fecha, _registro.FECHA)
                        UpdateAlarmaDuracion(_alarmaAnterior.alar_id, duracion)

                    End If
            End If
        End If

        End If

    End Sub

    Public Shared Function distanciaZona(ByVal puntos_recorrido As List(Of Zonas_Puntos), ByVal umbral As Double, ByVal ubicacion_lat As String, ByVal ubicacion_lng As String) As Boolean
        Dim EnZona As Boolean = False
        Dim distancia As Double = 0

        For Each punto As Zonas_Puntos In puntos_recorrido
            distancia = Distance(Decimal.Parse(punto.zon_latitud.Replace(".", ",")), Decimal.Parse(punto.zon_longitud.Replace(".", ",")), Decimal.Parse(ubicacion_lat.Replace(".", ",")), Decimal.Parse(ubicacion_lng.Replace(".", ",")))
            If distancia <= umbral Then EnZona = True
        Next

        Return EnZona

    End Function


    Public Shared Function distanciaRecorrido(ByVal puntos_recorrido As List(Of Recorridos_Puntos), ByVal umbral As Double, ByVal ubicacion_lat As String, ByVal ubicacion_lng As String) As Boolean
        Dim EnZona As Boolean = False
        Dim distancia As Double = 0

        For Each punto As Recorridos_Puntos In puntos_recorrido
            distancia = Distance(Decimal.Parse(punto.rec_latitud.Replace(".", ",")), Decimal.Parse(punto.rec_longitud.Replace(".", ",")), Decimal.Parse(ubicacion_lat.Replace(".", ",")), Decimal.Parse(ubicacion_lng.Replace(".", ",")))

            If distancia <= umbral Then EnZona = True
        Next

        Return EnZona

    End Function

    '***********
    'INICIO INSERTAR REPORTE ALARMA

    'la alarma tiene un tipo
    '1 - exceso de velocidad
    ' 2 - sensores
    '3 - direcciones entrada
    '31 direccion salida
    '4 - sale recorrido
    ' 41 entra en recorrido
    ' 5- zonas entrada
    '51 - zona salida
    '6 - recordatorios

    Public Shared Function InsertReporteAlarma(ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal referencia_id As Integer, ByVal tipo As Integer, ByVal nombreAlarma As String, ByVal enviarMail As Boolean) As Boolean
        Dim _alarma = New Alarmas()
        Dim nombrezona As String = ""
        Dim alarmaCategoria As String = ""

        Dim insertoAlarma As Boolean = False

        Select Case tipo
            Case 1
                alarmaCategoria = Categoria.Zonas
            Case 2
                alarmaCategoria = Categoria.Sensores
            Case 3
                alarmaCategoria = Categoria.Direccion
            Case 31
                alarmaCategoria = Categoria.Direccion
            Case 5
                alarmaCategoria = Categoria.Zonas
            Case 51
                alarmaCategoria = Categoria.Zonas
            Case 6
                alarmaCategoria = Categoria.Desvio
            Case 61
                alarmaCategoria = Categoria.Desvio

            Case 63
                alarmaCategoria = Categoria.RecorridoND
            Case 62
                alarmaCategoria = Categoria.RecorridoND
            Case 14
                alarmaCategoria = Categoria.Punto
            Case 12
                alarmaCategoria = Categoria.Punto
            Case Else

        End Select

        'verifico si no hay una alarma igual para el mismo dia

        Dim _alarmas As List(Of Alarmas) = GetAlertasReportadas(movil.veh_id, referencia_id, tipo)
        If _alarmas.Count = 0 Then
            _alarma.alar_fecha = _registro.FECHA
            _alarma.alar_Categoria = alarmaCategoria
            _alarma.alar_Localidad = _registro.LOCALIDAD
            _alarma.alar_nombre_via = _registro.NOMBRE_VIA
            _alarma.alar_Provincia = _registro.PROVINCIA
            _alarma.alar_valor = _registro.VELOCIDAD
            _alarma.veh_id = movil.veh_id
            _alarma.veh_patente = movil.veh_patente
            _alarma.alar_nombre = nombreAlarma
            _alarma.alar_lat = _registro.LATITUD
            _alarma.alar_lng = _registro.LONGITUD
            _alarma.alar_tipo = tipo
            _alarma.alar_vista = False
            _alarma.alar_vista_admin = False
            _alarma.veh_conductor = movil.veh_nombre_conductor
            _alarma.alar_mostrar = True
            _alarma.alar_codigo_config = referencia_id
            _alarma.alar_duracion = 0
            'verifico si envio mail o sms

            If enviarMail Then
                _alarma.mail_enviado = DateTime.Now

                clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + _alarma.veh_patente + " disparo la alarma " + _alarma.alar_Categoria + " " + nombrezona + "  , configurada con el nombre " + nombreAlarma + ", el día " + _registro.FECHA.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)


            End If

            InsertAlarma(_alarma)

            insertoAlarma = True
        Else
            'hay una alarma verifico si la trama que analizo es de memoria
            'entonces tengo que actualizar la fecha de la al arma con la que trae la trama de memoria
            If _registro.de_memoria Then
                _alarma = _alarmas(0)
                _alarma.alar_fecha = _registro.FECHA
                UpdateAlarma(_alarma)
            End If

        End If


        Return insertoAlarma

    End Function


    Public Shared Sub InsertAlarmaPuntoObligatorio(ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Recorridos, ByVal nombre As String, ByVal tipo As Integer, ByVal punto_id As Integer)
        Dim nombreDir As String = ""


        If GetAlertasReportadasPunto(movil.veh_id, punto_id, tipo).Count = 0 Then
            If (alerta.Recorridos.rec_nombre.Length > 30) Then
                nombreDir = alerta.Recorridos.rec_nombre.Substring(0, 30)
            Else
                nombreDir = alerta.Recorridos.rec_nombre
            End If

            Dim _alarma = New Alarmas()
            _alarma.alar_fecha = _registro.FECHA
            _alarma.alar_Categoria = "Entrada/Salida Punto Obligatorio"
            _alarma.alar_Localidad = _registro.LOCALIDAD
            _alarma.alar_nombre_via = _registro.NOMBRE_VIA
            _alarma.alar_Provincia = _registro.PROVINCIA
            _alarma.alar_valor = _registro.VELOCIDAD
            _alarma.veh_id = movil.veh_id
            _alarma.veh_patente = movil.veh_patente
            _alarma.alar_nombre = nombre & ", recorrido: " & alerta.Recorridos.rec_nombre
            _alarma.alar_lat = _registro.LATITUD
            _alarma.alar_lng = _registro.LONGITUD
            _alarma.alar_tipo = tipo
            _alarma.alar_vista = False
            _alarma.alar_vista_admin = False
            _alarma.veh_conductor = movil.veh_nombre_conductor
            _alarma.alar_mostrar = True
            _alarma.alar_codigo_config = punto_id
            _alarma.alar_duracion = 0

            'verifico si envio mail o sms

            If alerta.arec_enviar_mail Then
                clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " & alerta.Vehiculo.veh_patente & " disparo la alarma de Entrada/Salida de Punto Obligatorio en el recorrido: " & nombre & ", el día " & _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                _alarma.mail_enviado = DateTime.Now
            End If

            InsertAlarma(_alarma)
        End If


    End Sub



    Public Shared Sub InsertAlarmaDir_Retorno(ByVal enMemoria As Boolean, ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Direcciones, ByVal nombre As String, ByVal tipo As Integer, ByVal duracion As Integer)
        Dim nombreDir As String = ""

        If enMemoria Then
            If GetAlertasReportadas(movil.veh_id, alerta.adir_id, tipo, _registro.FECHA).Count = 0 Then

                If (alerta.Direcciones.dir_direccion.Length > 30) Then
                    nombreDir = alerta.Direcciones.dir_direccion.Substring(0, 30)
                Else
                    nombreDir = alerta.Direcciones.dir_direccion
                End If

                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Entrada/Salida Direccion"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = nombre + " " + alerta.Direcciones.dir_direccion.Substring(0, 30)
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = tipo
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = False
                _alarma.alar_codigo_config = alerta.adir_id
                _alarma.alar_duracion = duracion

                'verifico si envio mail o sms

                If alerta.adir_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + ": " + alerta.Direcciones.dir_direccion.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            End If
        Else
            If GetAlertasReportadas(movil.veh_id, alerta.adir_id, tipo).Count = 0 Then

                If (alerta.Direcciones.dir_direccion.Length > 30) Then
                    nombreDir = alerta.Direcciones.dir_direccion.Substring(0, 30)
                Else
                    nombreDir = alerta.Direcciones.dir_direccion
                End If

                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Entrada/Salida Direccion"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = nombre + " " + alerta.Direcciones.dir_direccion.Substring(0, 30)
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = tipo
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = False
                _alarma.alar_codigo_config = alerta.adir_id
                _alarma.alar_duracion = duracion

                'verifico si envio mail o sms

                If alerta.adir_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + ": " + alerta.Direcciones.dir_direccion.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            End If
        End If

    End Sub

    Public Shared Sub InsertAlarmaExcesoKms(ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alamas_Kms_Excedidos, ByVal nombre As String, ByVal tipo As Integer, ByVal fecha As String)
        Dim _alarma = New Alarmas()
        Dim nombrezona As String = ""

        Dim _registrada As Integer = 0

        If alerta.alak_frecuencia = "1" Then
            _registrada = GetAlertasReportadasFecha(movil.veh_id, alerta.alak_id, tipo, CDate(fecha).ToString("dd/MM/yyyy")).Count
        End If

        If alerta.alak_frecuencia = "2" Then
            _registrada = GetAlertasReportadasSemana(movil.veh_id, alerta.alak_id, tipo, CDate(fecha).ToString("yyyyMMdd")).Count
        End If

        If alerta.alak_frecuencia = "3" Then
            _registrada = GetAlertasReportadasMes(movil.veh_id, alerta.alak_id, tipo, CDate(fecha).ToString("dd/MM/yyyy")).Count
        End If


        'verifico si no hay una alarma igual para el mismo dia
        If _registrada = 0 Then
            If (alerta.alak_descripcion.Length > 30) Then
                nombrezona = alerta.alak_descripcion.Substring(0, 30)
            Else
                nombrezona = alerta.alak_descripcion
            End If
            _alarma.alar_fecha = _registro.FECHA
            _alarma.alar_Categoria = "Exceso de Kms Acumulados"
            _alarma.alar_Localidad = IIf(_registro.LOCALIDAD Is Nothing, "", _registro.LOCALIDAD)
            _alarma.alar_nombre_via = _registro.NOMBRE_VIA
            _alarma.alar_Provincia = _registro.PROVINCIA
            _alarma.alar_valor = _registro.VELOCIDAD
            _alarma.veh_id = movil.veh_id
            _alarma.veh_patente = movil.veh_patente
            _alarma.alar_nombre = nombre + " " + nombrezona
            _alarma.alar_lat = _registro.LATITUD
            _alarma.alar_lng = _registro.LONGITUD
            _alarma.alar_tipo = tipo
            _alarma.alar_vista = False
            _alarma.alar_vista_admin = False
            _alarma.veh_conductor = movil.veh_nombre_conductor
            _alarma.alar_mostrar = True
            _alarma.alar_codigo_config = alerta.alak_id
            _alarma.alar_duracion = 0
            'verifico si envio mail o sms

            If alerta.alak_enviar_mail Then
                clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + "  ,para la alarma de Exceso de Kms Acumulados, configurada con el nombre " + alerta.alak_descripcion.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                _alarma.mail_enviado = DateTime.Now
            End If

            InsertAlarma(_alarma)
        End If
    End Sub


    Public Shared Sub InsertAlarmaInicioActividad(ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alarma_Inicio_Actividad, ByVal nombre As String, ByVal tipo As Integer, ByVal fecha As String, ByVal tramaMemoria As Boolean)
        Dim _alarma = New Alarmas()
        Dim nombrezona As String = ""


        'verifico si no hay una alarma igual para el mismo dia
        'no tengo en cuenta la duracion porque esta alarma es una por dia
        Dim alarmasAnteriores As List(Of Alarmas)
        If tramaMemoria Then
            alarmasAnteriores = GetAlertasReportadasFecha(movil.veh_id, alerta.alaric_id, tipo, CDate(fecha).ToString("yyyy/MM/dd"))
        Else
            alarmasAnteriores = GetAlertasReportadas(movil.veh_id, alerta.alaric_id, tipo, CDate(fecha).ToString("yyyy/MM/dd"))
        End If

        If alarmasAnteriores.Count = 0 Then

            If (alerta.alaric_descripcion.Length > 30) Then
                nombrezona = alerta.alaric_descripcion.Substring(0, 30)
            Else
                nombrezona = alerta.alaric_descripcion
            End If

            _alarma.alar_fecha = _registro.FECHA
            _alarma.alar_Categoria = "Inicio Actividad"
            _alarma.alar_Localidad = _registro.LOCALIDAD
            _alarma.alar_nombre_via = _registro.NOMBRE_VIA
            _alarma.alar_Provincia = _registro.PROVINCIA
            _alarma.alar_valor = _registro.VELOCIDAD
            _alarma.veh_id = movil.veh_id
            _alarma.veh_patente = movil.veh_patente
            _alarma.alar_nombre = nombre + " " + nombrezona
            _alarma.alar_lat = _registro.LATITUD
            _alarma.alar_lng = _registro.LONGITUD
            _alarma.alar_tipo = tipo
            _alarma.alar_vista = False
            _alarma.alar_vista_admin = False
            _alarma.veh_conductor = movil.veh_nombre_conductor
            _alarma.alar_mostrar = True
            _alarma.alar_codigo_config = alerta.alaric_id
            _alarma.alar_duracion = 0
            'verifico si envio mail o sms

            If alerta.alar_enviar_mail Then
                clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + "  ,para la alarma de Inicio de Actividad diario, configurada con el nombre " + alerta.alaric_descripcion.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                _alarma.mail_enviado = DateTime.Now
            End If

            InsertAlarma(_alarma)
        End If
    End Sub

    Public Shared Sub InsertAlarmaInactividad(ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alarmas_Inactividad, ByVal nombre As String, ByVal tipo As Integer, ByVal fecha As String, ByVal tramaMemoria As Boolean)
        Dim _alarma = New Alarmas()
        Dim nombrezona As String = ""


        'verifico si no hay una alarma igual para el mismo dia
        If GetAlertasReportadas(movil.veh_id, alerta.alari_id, tipo).Count = 0 Then
            If (alerta.alari_descripcion.Length > 30) Then
                nombrezona = alerta.alari_descripcion.Substring(0, 30)
            Else
                nombrezona = alerta.alari_descripcion
            End If
            _alarma.alar_fecha = DateTime.Now
            _alarma.alar_Categoria = "Inactividad"
            _alarma.alar_Localidad = _registro.LOCALIDAD
            _alarma.alar_nombre_via = _registro.NOMBRE_VIA
            _alarma.alar_Provincia = _registro.PROVINCIA
            _alarma.alar_valor = _registro.VELOCIDAD
            _alarma.veh_id = movil.veh_id
            _alarma.veh_patente = movil.veh_patente
            _alarma.alar_nombre = nombre + " alarma " + nombrezona
            _alarma.alar_lat = _registro.LATITUD
            _alarma.alar_lng = _registro.LONGITUD
            _alarma.alar_tipo = tipo
            _alarma.alar_vista = False
            _alarma.alar_vista_admin = False
            _alarma.veh_conductor = movil.veh_nombre_conductor
            _alarma.alar_mostrar = True
            _alarma.alar_codigo_config = alerta.alari_id
            _alarma.alar_duracion = 0
            'verifico si envio mail o sms

            If alerta.alari_enviar_mail And Not tramaMemoria Then
                clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + "  ,para la alarma de Inactividad configurada con el nombre " + alerta.alari_descripcion.ToUpper + ", el día " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                _alarma.mail_enviado = DateTime.Now
            End If

            InsertAlarma(_alarma)
        End If
    End Sub


    Public Shared Sub InsertAlarmaR(ByVal enMemoria As Boolean, ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Recorridos, ByVal nombre As String, ByVal tipo As Integer)
        Dim nombreRecorrido As String = ""

        If (alerta.Recorridos.rec_nombre.Length > 30) Then
            nombreRecorrido = alerta.Recorridos.rec_nombre.Substring(0, 30)
        Else
            nombreRecorrido = alerta.Recorridos.rec_nombre
        End If

        If enMemoria Then
            Dim _alarmaReportada As List(Of Alarmas) = GetAlertasReportadas(movil.veh_id, alerta.arec_id, tipo, _registro.FECHA)
            If _alarmaReportada.Count = 0 Then
                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Desvio Recorridos"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = nombre + " " + nombreRecorrido
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = tipo
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = True
                _alarma.alar_codigo_config = alerta.arec_id
                _alarma.alar_duracion = 0
                'verifico si envio mail o sms

                If alerta.arec_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + "  ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            Else

                'cambio los datos de la alarma ya reportada
                refrescarDatosAlarma(_alarmaReportada(0), _registro)

            End If
        Else
            If GetAlertasReportadas(movil.veh_id, alerta.arec_id, tipo).Count = 0 Then
                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Desvio Recorridos"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = nombre + " " + nombreRecorrido
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = tipo
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = True
                _alarma.alar_codigo_config = alerta.arec_id
                _alarma.alar_duracion = 0
                'verifico si envio mail o sms

                If alerta.arec_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + "  ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            End If
        End If

    End Sub

    Public Shared Sub InsertAlarmaRN(ByVal enMemoria As Boolean, ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Recorridos, ByVal tipo As String)
        Dim nombreRecorrido As String = ""

        If (alerta.Recorridos.rec_nombre.Length > 30) Then
            nombreRecorrido = alerta.Recorridos.rec_nombre.Substring(0, 30)
        Else
            nombreRecorrido = alerta.Recorridos.rec_nombre
        End If

        If enMemoria Then

            Dim _alarmaReportada As List(Of Alarmas) = GetAlertasReportadas(movil.veh_id, alerta.arec_id, 62, _registro.FECHA)
            If _alarmaReportada.Count = 0 Then
                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Recorrido No Deseado"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = tipo + " " + nombreRecorrido
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = 62
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = True
                _alarma.alar_codigo_config = alerta.arec_id
                _alarma.alar_duracion = 0
                'verifico si envio mail o sms

                If alerta.arec_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + tipo + "  ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            Else
                'cambio los datos de la alarma ya reportada
                refrescarDatosAlarma(_alarmaReportada(0), _registro)
            End If
        Else
            If GetAlertasReportadas(movil.veh_id, alerta.arec_id, 62).Count = 0 Then
                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Recorrido No Deseado"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = tipo + " " + nombreRecorrido
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = 62
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = True
                _alarma.alar_codigo_config = alerta.arec_id
                _alarma.alar_duracion = 0
                'verifico si envio mail o sms

                If alerta.arec_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + tipo + "  ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            End If
        End If

    End Sub

    Public Shared Sub InsertAlarmaR_retorno(ByVal enMemoria As Boolean, ByVal _registro As vMonitoreo, ByVal movil As Vehiculo, ByVal alerta As Alertas_Recorridos, ByVal nombre As String, ByVal tipo As Integer, ByVal duracion As Integer)
        Dim nombreRecorrido As String = ""

        If (alerta.Recorridos.rec_nombre.Length > 30) Then
            nombreRecorrido = alerta.Recorridos.rec_nombre.Substring(0, 30)
        Else
            nombreRecorrido = alerta.Recorridos.rec_nombre
        End If

        If enMemoria Then
            If GetAlertasReportadas(movil.veh_id, alerta.arec_id, tipo, _registro.FECHA).Count = 0 Then
                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Desvio Recorridos"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = nombre + " " + nombreRecorrido
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = tipo
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = True
                _alarma.alar_codigo_config = alerta.arec_id
                _alarma.alar_duracion = duracion
                'verifico si envio mail o sms

                If alerta.arec_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + " " + nombreRecorrido + "  ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            End If
        Else
            If GetAlertasReportadas(movil.veh_id, alerta.arec_id, tipo).Count = 0 Then
                Dim _alarma = New Alarmas()
                _alarma.alar_fecha = _registro.FECHA
                _alarma.alar_Categoria = "Desvio Recorridos"
                _alarma.alar_Localidad = _registro.LOCALIDAD
                _alarma.alar_nombre_via = _registro.NOMBRE_VIA
                _alarma.alar_Provincia = _registro.PROVINCIA
                _alarma.alar_valor = _registro.VELOCIDAD
                _alarma.veh_id = movil.veh_id
                _alarma.veh_patente = movil.veh_patente
                _alarma.alar_nombre = nombre + " " + nombreRecorrido
                _alarma.alar_lat = _registro.LATITUD
                _alarma.alar_lng = _registro.LONGITUD
                _alarma.alar_tipo = tipo
                _alarma.alar_vista = False
                _alarma.alar_vista_admin = False
                _alarma.veh_conductor = movil.veh_nombre_conductor
                _alarma.alar_mostrar = True
                _alarma.alar_codigo_config = alerta.arec_id
                _alarma.alar_duracion = duracion
                'verifico si envio mail o sms

                If alerta.arec_enviar_mail Then
                    clsFunciones.Send_Mail_Alarma_Clientes("Su movil Patente " + alerta.Vehiculo.veh_patente + " disparo la alarma " + nombre + " " + nombreRecorrido + "  ,para el Recorrido configurado con el nombre " + alerta.Recorridos.rec_nombre.ToUpper + ", el día " + _alarma.alar_fecha.ToString("dd/MM/yyyy HH:mm"), movil.Cliente.cli_email)
                    _alarma.mail_enviado = DateTime.Now
                End If

                InsertAlarma(_alarma)
            End If
        End If

    End Sub

    '***
    'FIN INSERTAR REPORTE ALARMAS

    '  1) calcular la distancia contra todos los puntos del poligono o recorrido									
    '2)Identificar los 2 puntos mas cercanos (respetando el orden)									
    '3)usar la formula que da como resultado 0 y 1. Si da mas de 1, 									
    'solo calcular la distancia contra el punto mas cercano de lo 2 identificados previamente. 									
    'Disparar alarma si si la distancia es mayor al umbral									
    '4) Si da menos de 1, usar la formula que proyecta el punto en el segmento del poligono o recorrido									
    '5)Calcular la distancia entre el punto proyectado y el movil. Disparar la alarma si esta distancia es mayor al umbral									
    'en el punto 2 cuando pongo "respetando el orden" significa que si el usuario armo un poligono con punto 1,2,3 y 4.   Los segmentos hay que tomarlos asi 1-2 o 2-3 o 3-4 o 4-1. No puede estar formado por 1-3 porque seria una linea atravesaria por dentro									


    'FUNCIONES PRIVADAS
    Public Const RadioTierraKm As Single = 6378.0F

    Private Shared Function EnZona(ByVal puntos_recorrido As List(Of Zonas_Puntos), ByVal umbral As Double, ByVal ubicacion_lat As String, ByVal ubicacion_lng As String) As Boolean
        Dim _enzona As Boolean = False
        Dim _lat As Decimal = Decimal.Parse(ubicacion_lat.Replace(".", ","))
        Dim _long As Decimal = Decimal.Parse(ubicacion_lng.Replace(".", ","))

        For i As Integer = 0 To puntos_recorrido.Count - 1


            Dim latPunto1 As Decimal = Decimal.Parse(puntos_recorrido(i).zon_latitud.Replace(".", ","))
            Dim lngPunto1 As Decimal = Decimal.Parse(puntos_recorrido(i).zon_longitud.Replace(".", ","))

            'si es el ultimo punto lo uno con el primero
            'siguiente punto
            Dim latPunto2 As Decimal = 0
            Dim longPunto2 As Decimal = 0
            Try
                latPunto2 = Decimal.Parse(puntos_recorrido(i + 1).zon_latitud.Replace(".", ","))
                longPunto2 = Decimal.Parse(puntos_recorrido(i + 1).zon_longitud.Replace(".", ","))

            Catch ex As Exception
                latPunto2 = Decimal.Parse(puntos_recorrido(0).zon_latitud.Replace(".", ","))
                longPunto2 = Decimal.Parse(puntos_recorrido(0).zon_longitud.Replace(".", ","))
            End Try



            If ((Pow((latPunto2 - latPunto1), 2) + Pow(longPunto2 - lngPunto1, 2))) > 0 Then

                '((lat-lat1)*(lat2-lat1)+(lngMovil-lng1)*(lng2-lng1))/((POTENCIA(lat2-lat1;2)+POTENCIA(lng2-lng1;2)))
                Dim inSegmento As Decimal = ((_lat - latPunto1) * (latPunto2 - latPunto1) + (_long - lngPunto1) * (longPunto2 - lngPunto1)) / ((Pow((latPunto2 - latPunto1), 2) + Pow(longPunto2 - lngPunto1, 2)))

                'verifico segmento = 0 o OR= 1 
                'sino es caclulo la distancia a los puntos contra el umbral 

                If inSegmento >= 0 And inSegmento <= 1 Then
                    'fomula 2
                    Dim puntoX As Decimal = (latPunto2 - latPunto1) * inSegmento + latPunto1
                    Dim PuntoY As Decimal = (longPunto2 - lngPunto1) * inSegmento + lngPunto1

                    Dim _distancia As Decimal = Distance(puntoX, PuntoY, Decimal.Parse(ubicacion_lat.Replace(".", ",")), Decimal.Parse(ubicacion_lng.Replace(".", ",")))

                    If _distancia <= umbral Then _enzona = True
                End If
            Else
                _enzona = True
            End If
        Next


        'si no esta en la zona verifico la distancia a cada punto, para asegurar que no este dentro del umbral
        If Not _enzona Then
            _enzona = distanciaZona(puntos_recorrido, umbral, ubicacion_lat, ubicacion_lng)

        End If


        Return _enzona

    End Function

    Private Shared Function EnRecorrido(ByVal puntos_recorrido As List(Of Recorridos_Puntos), ByVal umbral As Double, ByVal ubicacion_lat As String, ByVal ubicacion_lng As String) As Boolean
        Dim _enzona As Boolean = False

        Dim _lat As Decimal = Decimal.Parse(ubicacion_lat.Replace(".", ","))
        Dim _long As Decimal = Decimal.Parse(ubicacion_lng.Replace(".", ","))

        For i As Integer = 0 To puntos_recorrido.Count - 2


            Dim latPunto1 As Decimal = Decimal.Parse(puntos_recorrido(i).rec_latitud.Replace(".", ","))
            Dim lngPunto1 As Decimal = Decimal.Parse(puntos_recorrido(i).rec_longitud.Replace(".", ","))

            'siguiente punto
            Dim latPunto2 As Decimal = Decimal.Parse(puntos_recorrido(i + 1).rec_latitud.Replace(".", ","))
            Dim longPunto2 As Decimal = Decimal.Parse(puntos_recorrido(i + 1).rec_longitud.Replace(".", ","))

            If ((Pow((latPunto2 - latPunto1), 2) + Pow(longPunto2 - lngPunto1, 2))) > 0 Then

                '((lat-lat1)*(lat2-lat1)+(lngMovil-lng1)*(lng2-lng1))/((POTENCIA(lat2-lat1;2)+POTENCIA(lng2-lng1;2)))
                Dim inSegmento As Decimal = ((_lat - latPunto1) * (latPunto2 - latPunto1) + (_long - lngPunto1) * (longPunto2 - lngPunto1)) / ((Pow((latPunto2 - latPunto1), 2) + Pow(longPunto2 - lngPunto1, 2)))

                'verifico segmento = 0 o OR= 1 
                'sino es caclulo la distancia a los puntos contra el umbral 

                If inSegmento >= 0 And inSegmento <= 1 Then
                    'fomula 2
                    Dim puntoX As Decimal = (latPunto2 - latPunto1) * inSegmento + latPunto1
                    Dim PuntoY As Decimal = (longPunto2 - lngPunto1) * inSegmento + lngPunto1

                    Dim _distancia As Decimal = Distance(puntoX, PuntoY, Decimal.Parse(ubicacion_lat.Replace(".", ",")), Decimal.Parse(ubicacion_lng.Replace(".", ",")))

                    If _distancia <= umbral Then _enzona = True
                End If
            Else
                _enzona = True
            End If
        Next

        'no esta en la zona verifico la distancia a cada punto
        If Not _enzona Then
            _enzona = distanciaRecorrido(puntos_recorrido, umbral, ubicacion_lat, ubicacion_lng)

        End If

        Return _enzona

    End Function


    Private Shared Function isPointZonaInPolygonExcel(ByVal _puntos As List(Of Zonas_Puntos), ByVal lat As Double, ByVal lng As Double) As Boolean
        Dim isInside As Boolean = False

        Dim _vertices As List(Of Vertice) = New List(Of Vertice)
        Dim _angulos As List(Of Double) = New List(Of Double)

        'voy calculando los vertices
        'puntoLat - lat , puntolng - lng
        Dim puntovertice As Vertice
        For i As Integer = 0 To _puntos.Count - 1
            puntovertice = New Vertice()

            puntovertice.verticeX = Double.Parse(_puntos(i).zon_latitud.Replace(".", ",")) - lat
            puntovertice.verticeY = Double.Parse(_puntos(i).zon_longitud.Replace(".", ",")) - lng
            _vertices.Add(puntovertice)
        Next

        'agrego en la ultima posicion el primer vertice para cerrar el circulo
        puntovertice = New Vertice()

        puntovertice.verticeX = Double.Parse(_puntos(0).zon_latitud.Replace(".", ",")) - lat
        puntovertice.verticeY = Double.Parse(_puntos(0).zon_longitud.Replace(".", ",")) - lng
        _vertices.Add(puntovertice)

        'calculo los angulos
        For i As Integer = 0 To _vertices.Count - 2
            Dim angulo As Double = 0
            Dim signo As Decimal = 0

            'angulo = grados(acos((x de la posicion i+1 * y de la posicion i + 1) + (x * y) / raiz cuadrada ((x* x) + (y * y)) * raiz cuadrada ((x de la posicion i+1 * x de la posicion i+1) + (y de la posicion i+1 * y de la posicion i+1))) ' se redondead en 4 decimales para que de 360 o 0

            angulo = Math.Round(((Math.Acos(((_vertices(i).verticeX * _vertices(i + 1).verticeX) + ((_vertices(i).verticeY * _vertices(i + 1).verticeY))) / (Math.Sqrt((_vertices(i).verticeX * _vertices(i).verticeX) + (_vertices(i).verticeY * _vertices(i).verticeY)) * Math.Sqrt(((_vertices(i + 1).verticeX * _vertices(i + 1).verticeX) + (_vertices(i + 1).verticeY * _vertices(i + 1).verticeY)))))) * 180) / Math.PI, 4)


            signo = ((_vertices(i).verticeX * _vertices(i + 1).verticeY) - (_vertices(i).verticeY * _vertices(i + 1).verticeX))
            signo = signo / Math.Sqrt((_vertices(i).verticeX * _vertices(i).verticeX) + (_vertices(i).verticeY * _vertices(i).verticeY)) * Math.Sqrt(((_vertices(i + 1).verticeX * _vertices(i + 1).verticeX) + (_vertices(i + 1).verticeY * _vertices(i + 1).verticeY)))

            Try
                signo = (Math.Asin(signo) * 180) / Math.PI
            Catch ex As Exception
                signo = 0
            End Try

            'signo= devuelve el signo de un numero. si es positivo  te da 1, si es 0  te da 0, y si es negativo te da -1
            ' angulo = angulo * signo
            If signo < 0 Then angulo = angulo * -1
            _angulos.Add(angulo)
        Next

        'f3 = X    G3 = Y
        'f4 = x + 1 G4 = y +1


        'REDONDEAR(GRADOS(ACOS((F3*F4+G3*G4)/(RAIZ(F3^2+G3^2)*RAIZ(F4^2+G4^2))));4)*
        'SIGNO(GRADOS(ASENO((F3 * G4 - G3 * F4) / (RAIZ(F3 ^ 2 + G3 ^ 2) * RAIZ(F4 ^ 2 + G4 ^ 2)))))

        'despues se suman todos los angulos si es 360 esta adentro sino esta afuera
        Dim _sumAngulo As Double = 0
        For i As Integer = 0 To _angulos.Count - 1
            _sumAngulo = _angulos(i) + _sumAngulo
        Next

        'paso la suma del angulo a positivo 
        If _sumAngulo < 0 Then _sumAngulo = _sumAngulo * -1


        If Math.Floor(_sumAngulo) >= 356 And Math.Floor(_sumAngulo) <= 361 Then isInside = True

        Return isInside
    End Function

    Private Shared Function isPointRecInPolygonExcel(ByVal _puntos As List(Of Recorridos_Puntos), ByVal lat As Double, ByVal lng As Double) As Boolean
        Dim isInside As Boolean = False

        Dim _vertices As List(Of Vertice) = New List(Of Vertice)
        Dim _angulos As List(Of Double) = New List(Of Double)

        'voy calculando los vertices
        'puntoLat - lat , puntolng - lng
        Dim puntovertice As Vertice
        For i As Integer = 0 To _puntos.Count - 1
            puntovertice = New Vertice()

            puntovertice.verticeX = Double.Parse(_puntos(i).rec_latitud.Replace(".", ",")) - lat
            puntovertice.verticeY = Double.Parse(_puntos(i).rec_longitud.Replace(".", ",")) - lng
            _vertices.Add(puntovertice)
        Next

        'agrego en la ultima posicion el primer vertice para cerrar el circulo
        puntovertice = New Vertice()

        puntovertice.verticeX = Double.Parse(_puntos(0).rec_latitud.Replace(".", ",")) - lat
        puntovertice.verticeY = Double.Parse(_puntos(0).rec_longitud.Replace(".", ",")) - lng
        _vertices.Add(puntovertice)

        'calculo los angulos
        For i As Integer = 0 To _vertices.Count - 2
            Dim angulo As Double = 0
            Dim signo As Decimal = 0

            'angulo = grados(acos((x de la posicion i+1 * y de la posicion i + 1) + (x * y) / raiz cuadrada ((x* x) + (y * y)) * raiz cuadrada ((x de la posicion i+1 * x de la posicion i+1) + (y de la posicion i+1 * y de la posicion i+1))) ' se redondead en 4 decimales para que de 360 o 0

            angulo = Math.Round(((Math.Acos(((_vertices(i).verticeX * _vertices(i + 1).verticeX) + ((_vertices(i).verticeY * _vertices(i + 1).verticeY))) / (Math.Sqrt((_vertices(i).verticeX * _vertices(i).verticeX) + (_vertices(i).verticeY * _vertices(i).verticeY)) * Math.Sqrt(((_vertices(i + 1).verticeX * _vertices(i + 1).verticeX) + (_vertices(i + 1).verticeY * _vertices(i + 1).verticeY)))))) * 180) / Math.PI, 4)


            signo = ((_vertices(i).verticeX * _vertices(i + 1).verticeY) - (_vertices(i).verticeY * _vertices(i + 1).verticeX))
            signo = signo / Math.Sqrt((_vertices(i).verticeX * _vertices(i).verticeX) + (_vertices(i).verticeY * _vertices(i).verticeY)) * Math.Sqrt(((_vertices(i + 1).verticeX * _vertices(i + 1).verticeX) + (_vertices(i + 1).verticeY * _vertices(i + 1).verticeY)))

            Try
                signo = (Math.Asin(signo) * 180) / Math.PI
            Catch ex As Exception
                signo = 0
            End Try

            'signo= devuelve el signo de un numero. si es positivo  te da 1, si es 0  te da 0, y si es negativo te da -1
            ' angulo = angulo * signo
            If signo < 0 Then angulo = angulo * -1
            _angulos.Add(angulo)
        Next

        'f3 = X    G3 = Y
        'f4 = x + 1 G4 = y +1


        'REDONDEAR(GRADOS(ACOS((F3*F4+G3*G4)/(RAIZ(F3^2+G3^2)*RAIZ(F4^2+G4^2))));4)*
        'SIGNO(GRADOS(ASENO((F3 * G4 - G3 * F4) / (RAIZ(F3 ^ 2 + G3 ^ 2) * RAIZ(F4 ^ 2 + G4 ^ 2)))))

        'despues se suman todos los angulos si es 360 esta adentro sino esta afuera
        Dim _sumAngulo As Double = 0
        For i As Integer = 0 To _angulos.Count - 1
            _sumAngulo = _angulos(i) + _sumAngulo
        Next

        'paso la suma del angulo a positivo 
        If _sumAngulo < 0 Then _sumAngulo = _sumAngulo * -1


        If Math.Floor(_sumAngulo) = 360 Then isInside = True



        Return isInside
    End Function

    Private Shared Function IsPointInPolygon(ByVal polygon As System.Drawing.PointF(), ByVal point As PointF) As Boolean
        Dim isInside As Boolean = False

        Dim i As Integer = 0, j As Integer = polygon.Length - 1
        While i < polygon.Length

            If ((polygon(i).Y > point.Y) <> (polygon(j).Y > point.Y)) AndAlso (point.X < (polygon(j).X - polygon(i).X) * (point.Y - polygon(i).Y) / (polygon(j).Y - polygon(i).Y) + polygon(i).X) Then

                isInside = Not isInside

            End If
            j = System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While

        Return isInside

    End Function



    Public Shared Function Distance(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double) As Double
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

    Private Shared Function DistanceOLD(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double) As Double

        Dim r As Double = 6371.0 ' approx. radius of earth in km
        Dim lat1Radians As Double = (lat1 * 0.01745329252) 'Multiplico los grados(y decimales) por 0,01745329252 para pasarlos a radianes y aplicarlos a la formula
        Dim lon1Radians As Double = (lon1 * 0.01745329252)
        Dim lat2Radians As Double = (lat2 * 0.01745329252)
        Dim lon2Radians As Double = (lon2 * 0.01745329252)
        Dim d As Double = (Math.Sin(lat1Radians) * Math.Sin(lat2Radians)) + (Math.Cos(lat1Radians) * Math.Cos(lat2Radians) * Math.Cos(lat1Radians - lat2Radians))
        Dim distancia As Double = ((Math.Acos(d) * 57.59577951) * 111.194) ' para obtener la distancia en kms lo multiplico por este valor
        Return distancia
    End Function




End Class



Public Class Posicion
    Public Sub New(ByVal latitud__1 As Single, ByVal longitud__2 As Single)
        Latitud = latitud__1
        Longitud = longitud__2
    End Sub
    Public Property Latitud() As Single
        Get
            Return m_Latitud
        End Get
        Set(ByVal value As Single)
            m_Latitud = value
        End Set
    End Property
    Private m_Latitud As Single
    Public Property Longitud() As Single
        Get
            Return m_Longitud
        End Get
        Set(ByVal value As Single)
            m_Longitud = value
        End Set
    End Property
    Private m_Longitud As Single
End Class

Enum TipoAlarmas
    EntradaZona = 5
    SalidaZona = 51

End Enum

Structure Categoria
    Const Zonas = "Entrada/Salida Zonas"
    Const RecorridoND = "Recorrido No Deseado"
    Const Sensores = "Sensores"
    Const Desvio = "Desvio Recorridos"
    Const VelocidadCalles = "Exceso Vel. en calles"
    Const VelocidadAv = "Exceso Vel. en Av."
    Const FueraHorario = "Uso Fuera de Horario"
    Const Inactividad = "Inactividad"
    Const InicioActividad = "Inactividad"
    Const Direccion = "Entrada/Salida Direccion"
    Const Punto = "Entrada/Salida Punto Obligatorio"
End Structure

'http://www.ytechie.com/2009/08/determine-if-a-point-is-contained-within-a-polygon/
'GPRMC,210525.000,A,3436.170027,S,05826.026897,W,16.360,236.7,270814,,,A*57,E000000000000T 250R125ID10003  



