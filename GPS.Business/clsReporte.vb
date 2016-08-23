Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

Public Class Kms

    Public Property FECHA() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property ID_MODULO() As String
        Get
            Return m_Patente
        End Get
        Set(value As String)
            m_Patente = value
        End Set
    End Property
    Private m_Patente As String

    Public Property KMS_RECORRIDOS() As Decimal
        Get
            Return m_Km
        End Get
        Set(value As Decimal)
            m_Km = value
        End Set
    End Property
    Private m_Km As Decimal
End Class

Public Class KmsMensuales

    Public Property FECHA() As Integer
        Get
            Return m_Fecha
        End Get
        Set(value As Integer)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property ID_MODULO() As String
        Get
            Return m_Patente
        End Get
        Set(value As String)
            m_Patente = value
        End Set
    End Property
    Private m_Patente As String

    Public Property KMS_RECORRIDOS() As Decimal
        Get
            Return m_Km
        End Get
        Set(value As Decimal)
            m_Km = value
        End Set
    End Property
    Private m_Km As Decimal
End Class
Public Class Tiempos

    Public Property Detenido_Encendido() As Integer
        Get
            Return m_detenido
        End Get
        Set(value As Integer)
            m_detenido = value
        End Set
    End Property
    Private m_detenido As Integer = 0

    Public Property Apagado() As Integer
        Get
            Return m_apagado
        End Get
        Set(value As Integer)
            m_apagado = value
        End Set
    End Property
    Private m_apagado As Integer = 0

    Public Property Movimiento() As Integer
        Get
            Return m_movimiento
        End Get
        Set(value As Integer)
            m_movimiento = value
        End Set
    End Property
    Private m_movimiento As Integer = 0
End Class


Public Class Velocidades

    Public Property FECHA() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property PATENTE() As String
        Get
            Return m_Patente
        End Get
        Set(value As String)
            m_Patente = value
        End Set
    End Property
    Private m_Patente As String

    Public Property ID_VEHICULO() As Integer
        Get
            Return m_Vehiculo
        End Get
        Set(value As Integer)
            m_Vehiculo = value
        End Set
    End Property
    Private m_Vehiculo As Integer

    Public Property VELOCIDAD_PROM() As Decimal
        Get
            Return m_prom
        End Get
        Set(value As Decimal)
            m_prom = value
        End Set
    End Property
    Private m_prom As Decimal

    Public Property VELOCIDAD_MAX() As Decimal
        Get
            Return m_max
        End Get
        Set(value As Decimal)
            m_max = value
        End Set
    End Property
    Private m_max As Decimal
End Class

Public Class Paradas

    Public Property FECHA() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property PATENTE() As String
        Get
            Return m_Patente
        End Get
        Set(value As String)
            m_Patente = value
        End Set
    End Property
    Private m_Patente As String

    

    Public Property CantParadas() As Integer
        Get
            Return m_cantparadas
        End Get
        Set(value As Integer)
            m_cantparadas = value
        End Set
    End Property
    Private m_cantparadas As Integer

    Public Property CantMotorApagado() As Integer
        Get
            Return m_cantapagado
        End Get
        Set(value As Integer)
            m_cantapagado = value
        End Set
    End Property
    Private m_cantapagado As Integer
End Class

Public Class Apagado

    Public Property Fecha() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property Cantidad() As Integer
        Get
            Return m_cantparadas
        End Get
        Set(value As Integer)
            m_cantparadas = value
        End Set
    End Property
    Private m_cantparadas As Integer

    Public Property Patente() As String
        Get
            Return m_Patente
        End Get
        Set(value As String)
            m_Patente = value
        End Set
    End Property
    Private m_Patente As String

    Public Property veh_id As Integer
        Get
            Return m_veh_id
        End Get
        Set(value As Integer)
            m_veh_id = value
        End Set
    End Property
    Private m_veh_id As Integer
End Class

Public Class Indicadores

    Public Property Patente() As String
        Get
            Return m_Patente
        End Get
        Set(value As String)
            m_Patente = value
        End Set
    End Property
    Private m_Patente As String

    Public Property Fecha() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property Temp_Min() As Integer
        Get
            Return m_Temp_Min
        End Get
        Set(value As Integer)
            m_Temp_Min = value
        End Set
    End Property
    Private m_Temp_Min As Integer
    

    Public Property Temp_Max As Integer
        Get
            Return m_Temp_Max
        End Get
        Set(value As Integer)
            m_Temp_Max = value
        End Set
    End Property
    Private m_Temp_Max As Integer

    Public Property Temp_Prom As Decimal
        Get
            Return m_Temp_Prom
        End Get
        Set(value As Decimal)
            m_Temp_Prom = value
        End Set
    End Property
    Private m_Temp_Prom As Decimal

    Public Property RPM_Max As Integer
        Get
            Return m_RMP_Max
        End Get
        Set(value As Integer)
            m_RMP_Max = value
        End Set
    End Property
    Private m_RMP_Max As Integer

    Public Property RPM_Min As Integer
        Get
            Return m_RMP_Min
        End Get
        Set(value As Integer)
            m_RMP_Min = value
        End Set
    End Property
    Private m_RMP_Min As Integer

    Public Property RPM_Prom As Decimal
        Get
            Return m_RPM_Prom
        End Get
        Set(value As Decimal)
            m_RPM_Prom = value
        End Set
    End Property
    Private m_RPM_Prom As Decimal

    Public Property Bat_Max As Integer
        Get
            Return m_Bat_Max
        End Get
        Set(value As Integer)
            m_Bat_Max = value
        End Set
    End Property
    Private m_Bat_Max As Integer

    Public Property Bat_Min As Integer
        Get
            Return m_Bat_Min
        End Get
        Set(value As Integer)
            m_Bat_Min = value
        End Set
    End Property
    Private m_Bat_Min As Integer

    Public Property Bat_Prom As Decimal
        Get
            Return m_Bat_Prom
        End Get
        Set(value As Decimal)
            m_Bat_Prom = value
        End Set
    End Property
    Private m_Bat_Prom As Decimal
End Class
Public Class clsReporte

    Public Shared Function RecorridosRutina(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As Integer, ByVal PageIndex As Integer, ByVal pageSize As Integer, ByVal order As String) As List(Of vMonitoreo)
        Dim recorridos As New List(Of vMonitoreo)
        Try
            Dim DB As New DataClassesGPSDataContext()
            'Dim mod_id As String = DB.Vehiculos.Where(Function(v) v.veh_id = veh_id).SingleOrDefault().mod_id
            '  recorridos = DB.vMonitoreo.Where(Function(v) v.ID_MODULO = mod_id And (v.FECHA >= fecha_desde And v.FECHA <= fecha_hasta)).OrderByDescending(Function(v) v.FECHA).ToList()

            recorridos = DB.ExecuteQuery(Of vMonitoreo)(" GetRecorridos '" & fecha_desde & "','" & fecha_hasta & "'," & veh_id & "," & PageIndex & "," & pageSize & ",'" & order & "'").ToList()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function RecorridosRutinaOld(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal mod_id As Integer) As List(Of vMonitoreo)
        Dim recorridos As New List(Of vMonitoreo)
        Try
            Dim DB As New DataClassesGPSDataContext()
            ' Dim mod_id As String = DB.Vehiculos.Where(Function(v) v.veh_id = veh_id).SingleOrDefault().mod_id
            ' recorridos = DB.vMonitoreo.Where(Function(v) v.ID_MODULO = mod_id And (v.FECHA >= fecha_desde And v.FECHA <= fecha_hasta)).OrderBy(Function(v) v.Codigo).ToList()

            recorridos = DB.ExecuteQuery(Of vMonitoreo)(" SELECT * FROM vMonitoreos  WHERE  (vMonitoreos.FECHA BETWEEN'" & fecha_desde & "' AND '" & fecha_hasta & "')  AND (ID_MODULO = '" & mod_id & "') order by codigo").ToList()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function RecorridosRutinaMapa(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As Integer, ByVal PageIndex As Integer, ByVal pageSize As Integer, ByVal order As String) As List(Of vMonitoreo)
        Dim recorridos As New List(Of vMonitoreo)
        Try
            Dim DB As New DataClassesGPSDataContext()
            'Dim mod_id As String = DB.Vehiculos.Where(Function(v) v.veh_id = veh_id).SingleOrDefault().mod_id
            '  recorridos = DB.vMonitoreo.Where(Function(v) v.ID_MODULO = mod_id And (v.FECHA >= fecha_desde And v.FECHA <= fecha_hasta)).OrderByDescending(Function(v) v.FECHA).ToList()

            recorridos = DB.ExecuteQuery(Of vMonitoreo)(" GetRecorridosMapa '" & fecha_desde & "','" & fecha_hasta & "'," & veh_id & "," & PageIndex & "," & pageSize & ",'" & order & "'").ToList()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

   
    Public Shared Function GetRecorridos(ByVal codigo As Integer, ByVal mod_id As String, ByVal fecha As String) As List(Of vMonitoreo)
        Dim recorridos As New List(Of vMonitoreo)
        Try
            Dim DB As New DataClassesGPSDataContext()

            recorridos = DB.ExecuteQuery(Of vMonitoreo)(" SELECT * FROM vMonitoreos  WHERE  (ID_MODULO = '" & mod_id & "') AND (FECHA >= '" & fecha & "' )   order by FECHA").ToList()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function GetRecorridosPosterior(ByVal codigo As Integer, ByVal mod_id As String, ByVal fecha As String) As vMonitoreo
        Dim recorridos As New vMonitoreo
        Try
            Dim DB As New DataClassesGPSDataContext()

            recorridos = DB.ExecuteQuery(Of vMonitoreo)(" SELECT TOP 2 * FROM vMonitoreos  WHERE  (ID_MODULO = '" & mod_id & "') AND (FECHA > '" & fecha & "' )  order by FECHA").FirstOrDefault()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function GetRecorridosValidos(ByVal codigo As Integer, ByVal mod_id As String, ByVal fecha As String) As List(Of vMonitoreo)
        Dim recorridos As New List(Of vMonitoreo)
        Try
            Dim DB As New DataClassesGPSDataContext()
            recorridos = DB.ExecuteQuery(Of vMonitoreo)(" SELECT * FROM vMonitoreos  WHERE  (ID_MODULO = '" & mod_id & "') AND (FECHA >= '" & fecha & "' ) AND NOMBRE_VIA <> 'Sin Señal GPS.'  order by FECHA").ToList()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function GetRecorridosValidosPosterior(ByVal codigo As Integer, ByVal mod_id As String, ByVal fecha As String) As vMonitoreo
        Dim recorridos As New vMonitoreo
        Try
            Dim DB As New DataClassesGPSDataContext()
            recorridos = DB.ExecuteQuery(Of vMonitoreo)(" SELECT TOP 2 * FROM vMonitoreos  WHERE  (ID_MODULO = '" & mod_id & "') AND (FECHA > '" & fecha & "' ) AND NOMBRE_VIA <> 'Sin Señal GPS.'  order by FECHA").FirstOrDefault()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function RecorridosRutinaTotal(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As Integer) As Integer
        Dim recorridos As Integer
        Try
            Dim DB As New DataClassesGPSDataContext()
            ' Dim mod_id As String = DB.Vehiculos.Where(Function(v) v.veh_id = veh_id).SingleOrDefault().mod_id
            '  recorridos = DB.vMonitoreo.Where(Function(v) v.ID_MODULO = mod_id And (v.FECHA >= fecha_desde And v.FECHA <= fecha_hasta)).OrderByDescending(Function(v) v.FECHA).ToList()

            recorridos = DB.ExecuteQuery(Of Integer)(" SELECT COUNT(Codigo) as cantidad FROM vMonitoreos  WHERE NOMBRE_VIA <> 'Sin Señal GPS.' AND FECHA BETWEEN '" & fecha_desde & "' AND '" & fecha_hasta & "' AND ID_VEHICULO = " & veh_id).FirstOrDefault()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function RecorridosRutinaTotalMapa(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As Integer) As Integer
        Dim recorridos As Integer
        Try
            Dim DB As New DataClassesGPSDataContext()
            ' Dim mod_id As String = DB.Vehiculos.Where(Function(v) v.veh_id = veh_id).SingleOrDefault().mod_id
            '  recorridos = DB.vMonitoreo.Where(Function(v) v.ID_MODULO = mod_id And (v.FECHA >= fecha_desde And v.FECHA <= fecha_hasta)).OrderByDescending(Function(v) v.FECHA).ToList()

            recorridos = DB.ExecuteQuery(Of Integer)(" SELECT COUNT(Codigo) as cantidad FROM vMonitoreos  WHERE NOMBRE_VIA <> 'Sin Señal GPS.' FECHA BETWEEN '" & fecha_desde & "' AND '" & fecha_hasta & "' AND ID_VEHICULO = " & veh_id).FirstOrDefault()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function KmsRecorridos(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As Integer) As List(Of Kms)
        Dim recorridos As New List(Of Kms)
        Try
            Dim DB As New DataClassesGPSDataContext()
            'Dim mod_id As String = DB.Vehiculos.Where(Function(v) v.veh_id = veh_id).SingleOrDefault().mod_id

            recorridos = DB.ExecuteQuery(Of Kms)(" SELECT SUM(vMonitoreos.KMS_RECORRIDOS) AS KMS_RECORRIDOS, vMonitoreos.ID_MODULO, convert(varchar,FECHA,111) " + _
                                " as FECHA FROM vMonitoreos WHERE  (vMonitoreos.FECHA BETWEEN'" & fecha_desde & "' AND '" & fecha_hasta & "')  AND (ID_VEHICULO = " & veh_id & ") GROUP BY vMonitoreos.ID_MODULO, convert(varchar,FECHA,111)").OrderByDescending(Function(v) v.FECHA).ToList()
            'recorridos = DB.vMonitoreo.Where(Function(v) v.ID_MODULO = mod_id And (v.FECHA >= fecha_desde And v.FECHA <= fecha_hasta)).ToList()

        Catch ex As Exception
            Throw ex
        End Try
        Return recorridos
    End Function

    Public Shared Function AlarmasRutina(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As Integer) As List(Of Alarmas)
        Dim alarmas As New List(Of Alarmas)
        Try
            Dim DB As New DataClassesGPSDataContext()
            ' alarmas = DB.Alarmas.Where(Function(v) v.veh_id = veh_id And (v.alar_fecha >= fecha_desde And v.alar_fecha <= fecha_hasta)).OrderByDescending(Function(v) v.alar_fecha).ToList()

            alarmas = DB.ExecuteQuery(Of Alarmas)("SELECT * FROM Alarmas WHERE veh_id = " & veh_id & " AND alar_fecha BETWEEN'" & fecha_desde & "' AND '" & fecha_hasta & "' ORDER BY alar_fecha DESC ").ToList()
        Catch ex As Exception
            Throw ex
        End Try
        Return alarmas
    End Function



    Public Shared Function CantidadAlarmas(ByVal fechaDesde As DateTime, ByVal veh_id As String) As Integer
        Dim cantidad As Integer = 0
        Dim alarmas As New List(Of Alarmas)
        Try
            Dim DB As New DataClassesGPSDataContext()

            alarmas = DB.ExecuteQuery(Of Alarmas)("SELECT * FROM Alarmas WHERE veh_id = " & veh_id & " AND alar_mostrar = 1 AND alar_fecha BETWEEN'" & String.Format("{0:yyyyMMdd HH:mm:ss}", fechaDesde.AddSeconds(-5)) & "' AND '" & String.Format("{0:yyyyMMdd HH:mm:ss}", fechaDesde.AddSeconds(5)) & "' ").ToList()

            cantidad = alarmas.Count()

        Catch ex As Exception

        End Try
        Return cantidad
    End Function

    Public Shared Function CantidadAlarmas(ByVal lat As String, ByVal lng As String, ByVal veh_id As String) As Integer
        Dim cantidad As Integer = 0
        Try
            Dim DB As New DataClassesGPSDataContext()

            cantidad = DB.Alarmas.Where(Function(v) v.veh_id = veh_id And v.alar_lat = lat And v.alar_lng = lng).Count()

        Catch ex As Exception

        End Try
        Return cantidad
    End Function

    '*******************
    ' ESTADISTICAS
    '**************
    'Velocidad maxima
    'agrupada por dia
    Public Shared Function VelocidadPromedioDiaria(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As List(Of Velocidades)
        Dim datos As New List(Of Velocidades)
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Velocidades)("  SELECT  AVG(vMonitoreos.VELOCIDAD) AS VELOCIDAD_PROM, MAX(vMonitoreos.VELOCIDAD) AS VELOCIDAD_MAX, vMonitoreos.ID_VEHICULO, CONVERT(varchar, " & _
                        " vMonitoreos.FECHA, 111) AS FECHA, Vehiculos.veh_descripcion + '-' + Vehiculos.veh_patente AS PATENTE " & _
                        "FROM  vMonitoreos INNER JOIN " & _
                         "Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO IN(" & veh_id & ") AND VELOCIDAD <> 0 AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') " & _
                        "GROUP BY vMonitoreos.ID_VEHICULO, CONVERT(varchar, vMonitoreos.FECHA, 111), Vehiculos.veh_patente, Vehiculos.veh_descripcion 	order by fecha desc, PATENTE").ToList()

        Catch ex As Exception

        End Try
        Return datos
    End Function


    Public Shared Function VelocidadMaxMovilDia(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Velocidades
        Dim datos As New Velocidades
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Velocidades)("  SELECT  AVG(vMonitoreos.VELOCIDAD) AS VELOCIDAD_PROM, MAX(vMonitoreos.VELOCIDAD) AS VELOCIDAD_MAX, vMonitoreos.ID_VEHICULO, CONVERT(varchar, " & _
                        " vMonitoreos.FECHA, 111) AS FECHA, Vehiculos.veh_descripcion + '-' + Vehiculos.veh_patente AS PATENTE " & _
                        "FROM  vMonitoreos INNER JOIN " & _
                         "Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO = " & veh_id & " AND VELOCIDAD <> 0 AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') " & _
                        "GROUP BY vMonitoreos.ID_VEHICULO, CONVERT(varchar, vMonitoreos.FECHA, 111), Vehiculos.veh_patente, Vehiculos.veh_descripcion 	order by fecha desc, PATENTE").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function VelocidadMaxMovilMes(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Velocidades
        Dim datos As New Velocidades
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Velocidades)("  SELECT  AVG(vMonitoreos.VELOCIDAD) AS VELOCIDAD_PROM, MAX(vMonitoreos.VELOCIDAD) AS VELOCIDAD_MAX, vMonitoreos.ID_VEHICULO, convert(varchar, Month(vMonitoreos.FECHA)) AS FECHA " & _
                        ", Vehiculos.veh_descripcion + '-' + Vehiculos.veh_patente AS PATENTE " & _
                        "FROM  vMonitoreos INNER JOIN " & _
                         "Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO = " & veh_id & "  AND VELOCIDAD <> 0 AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') " & _
                        "GROUP BY vMonitoreos.ID_VEHICULO, Month(vMonitoreos.FECHA), Vehiculos.veh_patente, Vehiculos.veh_descripcion ").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function VelocidadMaxMovilSemana(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Velocidades
        Dim datos As New Velocidades
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Velocidades)("  SELECT  AVG(vMonitoreos.VELOCIDAD) AS VELOCIDAD_PROM, MAX(vMonitoreos.VELOCIDAD) AS VELOCIDAD_MAX, vMonitoreos.ID_VEHICULO, convert(varchar, (DATEPART (week,vMonitoreos.FECHA)/4)) AS FECHA " & _
                        ", Vehiculos.veh_descripcion + '-' + Vehiculos.veh_patente AS PATENTE " & _
                        "FROM  vMonitoreos INNER JOIN " & _
                         "Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO = " & veh_id & " AND VELOCIDAD <> 0 AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') " & _
                        "GROUP BY vMonitoreos.ID_VEHICULO,(DATEPART (week,vMonitoreos.FECHA)/4), Vehiculos.veh_patente, Vehiculos.veh_descripcion ").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function VelocidadPromedioMensual(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As List(Of Velocidades)
        Dim datos As New List(Of Velocidades)
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Velocidades)("  SELECT  AVG(DISTINCT vMonitoreos.VELOCIDAD) AS VELOCIDAD_PROM, MAX(vMonitoreos.VELOCIDAD) AS VELOCIDAD_MAX, vMonitoreos.ID_VEHICULO, Month(" & _
                        " vMonitoreos.FECHA) AS FECHA, Vehiculos.veh_descripcion + '-' + Vehiculos.veh_patente AS PATENTE " & _
                        "FROM  vMonitoreos INNER JOIN " & _
                         "Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO IN(" & veh_id & ") AND VELOCIDAD <> 0 AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') " & _
                        "GROUP BY vMonitoreos.ID_VEHICULO, Month(vMonitoreos.FECHA), Vehiculos.veh_patente, Vehiculos.veh_descripcion 	order by fecha desc, PATENTE").ToList()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    'TODO: Cambiar el valor de Encendido a 1
    Public Shared Function TiempoMovimiento(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Long
        Dim datos As New List(Of vMonitoreo)
        Dim duracion As Long = 0
        Try
            Dim DB As New DataClassesGPSDataContext()
            'ESTADO 1 -En Movimiento ENCENDIDO = 1 AND VELOCIDAD >= 5
            duracion = DB.ExecuteQuery(Of Integer)("SELECT SUM(Tiempo_Parcial) FROM  vMonitoreos INNER JOIN Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO IN(" & veh_id & ") AND ESTADO = 1 AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') ").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return duracion
    End Function

    Public Shared Function ResumenTiempos(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Tiempos
        Dim datos As New List(Of vMonitoreo)
        Dim limiteSuperior As New vMonitoreo()
        Dim limiteInferior As New vMonitoreo()
        Dim duracion As Long = 0

        Dim _resumenTiempo As New Tiempos()
        Try
            Dim DB As New DataClassesGPSDataContext()
            'ESTADO 3-Detenido en okm ENCENDIDO = 1 AND VELOCIDAD < 5
            'ESTADO 1 -En Movimiento ENCENDIDO = 1 AND VELOCIDAD >= 5
            'ESTADO 2 Apagado
            datos = DB.ExecuteQuery(Of vMonitoreo)("SELECT CODIGO, FECHA, ISNULL(ESTADO,0) as ESTADO ,TIEMPO_PARCIAL, ID_VEHICULO FROM  vMonitoreos INNER JOIN Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO IN(" & veh_id & ") AND  (vMonitoreos.FECHA >= '" & CDate(fecha_desde).ToString("yyyyMMdd HH:mm:ss") & "') AND (vMonitoreos.FECHA <= '" & CDate(fecha_hasta).ToString("yyyyMMdd HH:mm:ss") & "') ORDER BY Fecha").ToList()
            'RESTAR la fecha del primer registro menos la fecha desde y generar un nuevo registro con el estado del registro anterior para el limite superior

            If datos.Count > 0 Then
                Dim _previusRegistro As vMonitoreo = clsVehiculo.searchPreviusLocation(datos(0).ID_VEHICULO, datos(0).Codigo, datos(0).FECHA.ToString("yyyyMMdd HH:mm:ss"))

                limiteSuperior.FECHA = CDate(fecha_desde)
                If _previusRegistro IsNot Nothing Then
                    limiteSuperior.ESTADO = _previusRegistro.ESTADO
                Else
                    limiteSuperior.ESTADO = datos(0).ESTADO
                End If

                limiteSuperior.TIEMPO_PARCIAL = Integer.Parse(diferenciaSegundos(fecha_desde, datos(0).FECHA))

                limiteInferior.FECHA = CDate(fecha_hasta)
                limiteInferior.ESTADO = datos(datos.Count - 1).ESTADO
                datos(datos.Count - 1).TIEMPO_PARCIAL = diferenciaSegundos(datos(datos.Count - 1).FECHA, fecha_hasta)

                If datos.Where(Function(r) r.ESTADO = 3).FirstOrDefault() IsNot Nothing Then _resumenTiempo.Detenido_Encendido = datos.Where(Function(r) r.ESTADO = 3).Sum(Function(r) r.TIEMPO_PARCIAL)
                If datos.Where(Function(r) r.ESTADO = 2).FirstOrDefault() IsNot Nothing Then _resumenTiempo.Apagado = datos.Where(Function(r) r.ESTADO = 2).Sum(Function(r) r.TIEMPO_PARCIAL)
                If datos.Where(Function(r) r.ESTADO = 1).FirstOrDefault() IsNot Nothing Then _resumenTiempo.Movimiento = datos.Where(Function(r) r.ESTADO = 1).Sum(Function(r) r.TIEMPO_PARCIAL)

                If limiteSuperior.ESTADO = 3 Then _resumenTiempo.Detenido_Encendido += limiteSuperior.TIEMPO_PARCIAL
                If limiteSuperior.ESTADO = 2 Then _resumenTiempo.Apagado += limiteSuperior.TIEMPO_PARCIAL
                If limiteSuperior.ESTADO = 1 Then _resumenTiempo.Movimiento += limiteSuperior.TIEMPO_PARCIAL

                ' If limiteInferior.ESTADO = 3 Then _resumenTiempo.Detenido_Encendido += limiteInferior.TIEMPO_PARCIAL
                'If limiteInferior.ESTADO = 2 Then _resumenTiempo.Apagado += limiteInferior.TIEMPO_PARCIAL
                'If limiteInferior.ESTADO = 1 Then _resumenTiempo.Movimiento += limiteInferior.TIEMPO_PARCIAL
            End If


        Catch ex As Exception

        End Try
        Return _resumenTiempo
    End Function

    Public Shared Function TiempoDetenidoApagado(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Long
        Dim datos As New List(Of vMonitoreo)
        Dim duracion As Long = 0
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of vMonitoreo)("SELECT * FROM  vMonitoreos INNER JOIN Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO IN(" & veh_id & ") AND (ENCENDIDO = 0 OR (ENCENDIDO =1 AND VELOCIDAD < 5)) AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') ").ToList()

            If datos.Count > 0 Then duracion = diferenciaSegundos(datos(0).FECHA, datos(datos.Count - 1).FECHA)

        Catch ex As Exception

        End Try
        Return duracion
    End Function

    Public Shared Function TiempoMotorApagado(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Long
        Dim datos As New List(Of vMonitoreo)
        Dim duracion As Long = 0
        Try
            Dim DB As New DataClassesGPSDataContext()

            duracion = DB.ExecuteQuery(Of Integer)("SELECT SUM(Tiempo_Parcial) FROM  vMonitoreos INNER JOIN Vehiculos ON vMonitoreos.ID_VEHICULO = Vehiculos.veh_id " & _
                        "WHERE ID_VEHICULO IN(" & veh_id & ") AND ESTADO = 2  AND (vMonitoreos.FECHA >= '" & fecha_desde & "') AND (vMonitoreos.FECHA <= '" & fecha_hasta & "') ").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return duracion
    End Function

    Public Shared Function CantidadAlarmasByVehiculo(ByVal fecha_desde As DateTime, ByVal fecha_hasta As DateTime, ByVal veh_id As Integer, ByVal tipo As Integer) As Integer
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Alarmas.Where(Function(d) d.veh_id = veh_id And d.alar_tipo = tipo And d.alar_fecha >= fecha_desde And d.alar_fecha <= fecha_hasta).ToList().Count

    End Function

    Public Shared Function CantidadDeParadas(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As List(Of Paradas)
        Dim datos As New List(Of Paradas)
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Paradas)(" spEstadisticasParadas '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").ToList()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function CantidadEncendidoCeroDia(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Apagado
        Dim datos As New Apagado
        Try
            Dim DB As New DataClassesGPSDataContext()

            ' datos = DB.ExecuteQuery(Of Apagado)("SELECT   COUNT(Codigo) as Cantidad,  CONVERT(varchar, vMonitoreo.FECHA, 111) as fecha  FROM vMonitoreo " & _
            '  "  WHERE  (ID_VEHICULO = " & veh_id & ") AND (FECHA >= '" & fecha_desde & "') AND (FECHA <= '" & fecha_hasta & "') AND Encendido = 0" & _
            '  " GROUP BY  CONVERT(varchar, vMonitoreo.FECHA, 111) " & _
            '  " order by fecha").FirstOrDefault()

            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasEncendidoCero '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function CantidadApagadoDia(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String, ByVal posi As Integer) As Apagado
        Dim datos As New Apagado
        Try
            Dim DB As New DataClassesGPSDataContext()

            ' datos = DB.ExecuteQuery(Of Apagado)("SELECT   COUNT(Codigo) as Cantidad,  CONVERT(varchar, vMonitoreo.FECHA, 111) as fecha  FROM vMonitoreo " & _
            '  "  WHERE  (ID_VEHICULO = " & veh_id & ") AND (FECHA >= '" & fecha_desde & "') AND (FECHA <= '" & fecha_hasta & "') AND Encendido = 0" & _
            '  " GROUP BY  CONVERT(varchar, vMonitoreo.FECHA, 111) " & _
            '  " order by fecha").FirstOrDefault()

            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasApagadoTotal '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function CantidadEncendidoSemanal(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Apagado
        Dim datos As New Apagado
        Try
            Dim DB As New DataClassesGPSDataContext()
            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasEncendidoCeroSemanal '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function CantidadApagadoSemana(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Apagado
        Dim datos As New Apagado
        Try
            Dim DB As New DataClassesGPSDataContext()
            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasApagadoSemanal '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function CantidadApagadoMes(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Apagado
        Dim datos As New Apagado
        Try
            Dim DB As New DataClassesGPSDataContext()
            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasApagadoMes '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function CantidadEncendidoMes(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Apagado
        Dim datos As New Apagado
        Try
            Dim DB As New DataClassesGPSDataContext()
            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasEncendidoMes '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function KmsRecorridosDia(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Kms
        Dim datos As New Kms
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Kms)("SELECT CONVERT(varchar, vMonitoreos.FECHA, 111) as FECHA, id_modulo,Sum(KMS_RECORRIDOS) as KMS_RECORRIDOS " & _
                                        " FROM vMonitoreos WHERE        (ID_VEHICULO = " & veh_id & ") AND (FECHA >= '" & fecha_desde & "') AND (FECHA <= '" & fecha_hasta & "')  " & _
                                " Group By  CONVERT(varchar, vMonitoreos.FECHA, 111),id_modulo").FirstOrDefault()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function KmsRecorridosSemana(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Kms
        Dim datos As New Kms
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Kms)("SELECT convert(varchar, (DATEPART (week,vMonitoreos.FECHA)/4)) as FECHA, '" & veh_id & "' as ID_MODULO,Sum(KMS_RECORRIDOS) as KMS_RECORRIDOS " & _
                                        " FROM vMonitoreos WHERE        (ID_VEHICULO = " & veh_id & ") AND (FECHA >= '" & fecha_desde & "') AND (FECHA <= '" & fecha_hasta & "')  " & _
                                " Group By convert(varchar,  (DATEPART (week,vMonitoreos.FECHA)/4)),id_modulo").FirstOrDefault()
        Catch ex As Exception

        End Try
        Return datos
    End Function

    Public Shared Function KmsRecorridosMensual(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As KmsMensuales
        Dim datos As New KmsMensuales
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of KmsMensuales)("SELECT Month(vMonitoreos.FECHA) as FECHA, '" & veh_id & "' as ID_MODULO ,Sum(KMS_RECORRIDOS) as KMS_RECORRIDOS " & _
                                        " FROM vMonitoreos WHERE        (ID_VEHICULO = " & veh_id & ") AND (FECHA >= '" & fecha_desde & "') AND (FECHA <= '" & fecha_hasta & "')  " & _
                                " Group By Month(vMonitoreos.FECHA),id_modulo").FirstOrDefault()
        Catch ex As Exception
            Throw ex
        End Try
        Return datos
    End Function

    Public Shared Function TotalKmsDia(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Decimal
        Dim datos As New List(Of Kms)
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Kms)(" SELECT SUM(vMonitoreos.KMS_RECORRIDOS) AS KMS_RECORRIDOS, vMonitoreos.ID_MODULO, convert(varchar,FECHA,111) " + _
                               " as FECHA FROM vMonitoreos WHERE  (vMonitoreos.FECHA BETWEEN'" & fecha_desde & "' AND '" & fecha_hasta & "')  AND (ID_VEHICULO = " & veh_id & ") GROUP BY vMonitoreos.ID_MODULO, convert(varchar,FECHA,111)").OrderByDescending(Function(v) v.FECHA).ToList()


        Catch ex As Exception

        End Try
        Return datos.Sum(Function(a) a.KMS_RECORRIDOS)
    End Function


    Public Shared Function TotalApagadoDia(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Integer
        Dim datos As New List(Of Apagado)
        Try
            Dim DB As New DataClassesGPSDataContext()

            '  datos = DB.ExecuteQuery(Of Apagado)("SELECT COUNT(Codigo) as Cantidad,  CONVERT(varchar, vMonitoreo.FECHA, 111) as fecha  FROM vMonitoreo " & _
            '    "  WHERE  (ID_VEHICULO =" & veh_id & ") AND (FECHA >= '" & fecha_desde & "') AND (FECHA <= '" & fecha_hasta & "') AND Encendido = 0" & _
            '     " GROUP BY  CONVERT(varchar, vMonitoreo.FECHA, 111) " & _
            '    " order by fecha").ToList()

            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasApagadoTotal '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").ToList()

        Catch ex As Exception

        End Try
        Return datos.Sum(Function(a) a.Cantidad)
    End Function

    Public Shared Function TotalEncendidoCeroDia(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Integer
        Dim datos As New List(Of Apagado)
        Try
            Dim DB As New DataClassesGPSDataContext()


            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasEncendidoCero '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").ToList()

        Catch ex As Exception

        End Try
        Return datos.Sum(Function(a) a.Cantidad)
    End Function

    Public Shared Function TotalApagadoGeneral(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As Integer
        Dim datos As New List(Of Apagado)
        Try
            Dim DB As New DataClassesGPSDataContext()

            '  datos = DB.ExecuteQuery(Of Apagado)("SELECT COUNT(Codigo) as Cantidad,  CONVERT(varchar, vMonitoreo.FECHA, 111) as fecha  FROM vMonitoreo " & _
            '    "  WHERE  (ID_VEHICULO =" & veh_id & ") AND (FECHA >= '" & fecha_desde & "') AND (FECHA <= '" & fecha_hasta & "') AND Encendido = 0" & _
            '     " GROUP BY  CONVERT(varchar, vMonitoreo.FECHA, 111) " & _
            '    " order by fecha").ToList()

            datos = DB.ExecuteQuery(Of Apagado)("spEstadisticasApagadoGeneral '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").ToList()

        Catch ex As Exception

        End Try
        Return datos.Sum(Function(a) a.Cantidad)
    End Function

    'RMP y TEMP General

    Public Shared Function IndicadoresGeneral(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String) As List(Of Indicadores)
        Dim datos As New List(Of Indicadores)
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Indicadores)("spEstadisticasRPMTemp '" & veh_id & "','" & fecha_desde & "','" & fecha_hasta & "'").ToList()

        Catch ex As Exception

        End Try
        Return datos
    End Function

    'sensores
    'busco para cada sensor la cantidad por auto para las fechas consultadas
    Public Shared Function SensoresGeneral(ByVal fecha_desde As String, ByVal fecha_hasta As String, ByVal veh_id As String, ByVal sen_id As Integer) As Integer
        Dim datos As Integer = 0
        Try
            Dim DB As New DataClassesGPSDataContext()

            datos = DB.ExecuteQuery(Of Integer)(" SELECT COUNT(Alarmas.alar_id) AS cant FROM  Alarmas INNER JOIN Sensores ON Alarmas.alar_codigo_config = Sensores.sen_id " & _
                                  "  WHERE  sen_id = " & sen_id & " AND (Alarmas.alar_tipo = 2) and veh_id = " & veh_id & " and alar_fecha >= '" & fecha_desde & "' And alar_fecha <= '" & fecha_hasta & "' GROUP BY Sensores.sen_nombre, Alarmas.veh_id").FirstOrDefault()
        Catch ex As Exception

        End Try
        Return datos
    End Function

    Private Shared Function diferenciaSegundos(fecha_desde As String, fecha_hasta As String) As Double
        Dim inicio As DateTime = DateTime.Parse(fecha_desde)

        'EJECUCIÓN de un proceso

        Dim final As DateTime = DateTime.Parse(fecha_hasta)
        Dim duracion As TimeSpan = final - inicio
        Dim segundosTotales As Double = duracion.TotalSeconds

        Return segundosTotales

    End Function
End Class
