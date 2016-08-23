
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

''' <summary>
''' Esta Clase representa a las tramas enviadas por un movil
''' </summary>
Public Class Tramas
    Private m_id As Integer
    Public Property ID() As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = value
        End Set
    End Property
    Public Property FECHA() As DateTime
        Get
            Return m_Fecha
        End Get
        Set(value As DateTime)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As DateTime

    Public Property MODULO() As String
        Get
            Return m_id_modulo
        End Get
        Set(value As String)
            m_id_modulo = value
        End Set
    End Property
    Private m_id_modulo As String

    Public Property Trama() As String
        Get
            Return m_Mensaje
        End Get
        Set(value As String)
            m_Mensaje = value
        End Set
    End Property
    Private m_Mensaje As String

End Class

''' <summary>
''' ﻿'Esta clase administra los datos referidos a un vehiculo
''' Alta,baja y modifcacionde vehiculos
''' Ubicacion segun los datos reportados por el modulo
''' </summary>

Public Class clsVehiculo



    Public Shared Function List() As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.ToList()

    End Function

    Public Shared Function List(ByVal cli_id As Integer) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) v.cli_id = cli_id).ToList()

    End Function

    Public Shared Function List(ByVal cli_id As Integer, ByVal veh_patente As String, ByVal veh_conductor As String, ByVal veh_marca As String, ByVal veh_modelo As String) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) (v.cli_id = cli_id Or cli_id = 0) And (v.veh_patente.Contains(veh_patente) Or veh_patente = "") And (v.veh_nombre_conductor.Contains(veh_conductor) Or veh_patente = "") And (v.veh_marca.Contains(veh_marca) Or veh_marca = "") And (v.veh_modelo.Contains(veh_modelo) Or veh_modelo = "")).ToList()

    End Function

    Public Shared Function ListActivos(ByVal cli_id As Integer) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) v.cli_id = cli_id And v.veh_activo = True).ToList()

    End Function

    Public Shared Function ListActivos() As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) v.veh_activo = True).ToList()

    End Function

    Public Shared Function ListNotGrupo(ByVal cli_id As Integer) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Dim query As String = " SELECT        veh_id, cli_id, veh_descripcion, veh_patente, veh_tipo_id, veh_imagen, mod_id, veh_nombre_conductor, veh_activo, veh_marca, veh_modelo, veh_color, " + _
                             " tip_uso_id, veh_kilometros, veh_modulo_sensor " + _
                             " FROM Vehiculos " + _
                        " WHERE       veh_activo = 1 AND (cli_id = " & cli_id & ") AND (veh_id NOT IN" + _
                            " (SELECT        veh_id" + _
                              " FROM            Grupos_Vehiculos) ) ORDER BY tip_uso_id"

        Return DB.ExecuteQuery(Of Vehiculo)(query).ToList()

    End Function

    Public Shared Function ListActivosTop(ByVal cli_id As Integer) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Vehiculos.Where(Function(v) v.cli_id = cli_id And v.veh_activo = True).Take(9).ToList()

    End Function

    Public Shared Function ListActivos(ByVal cli_id As Integer, ByVal filtro As String) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) v.cli_id = cli_id And v.veh_activo = True And ((v.veh_patente.Contains(filtro) Or filtro = "") Or (v.veh_descripcion.Contains(filtro) Or filtro = ""))).ToList()

    End Function


    Public Shared Function ListActivosGrupo(ByVal grup_id As Integer, ByVal filtro As String) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return (From v In DB.Vehiculos _
                       Join g In DB.Grupos_Vehiculos On v.veh_id Equals g.veh_id
        Where g.grup_id = grup_id And v.veh_activo = True And ((v.veh_patente.Contains(filtro) Or filtro = "") Or (v.veh_descripcion.Contains(filtro) Or filtro = "")) _
              Select v).OrderBy(Function(v) v.tip_uso_id).ToList()

    End Function

    Public Shared Function ListNoAlarmaDireccion(ByVal cli_id As Integer) As List(Of Vehiculo)
        Dim DB As New DataClassesGPSDataContext()
        Return (From v In DB.Vehiculos Where Not DB.Alertas_Direcciones.Any(Function(pu) pu.veh_id = v.veh_id) And v.cli_id = cli_id And v.veh_activo = True).ToList()

    End Function

    Public Shared Function Seleccionar(ByVal veh_id As Integer) As Vehiculo
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) v.veh_id = veh_id).FirstOrDefault()

    End Function

    Public Shared Function GetAlarma(ByVal veh_id As Integer, ByVal lat As String, ByVal lng As String) As Alarmas
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Alarmas.Where(Function(v) v.veh_id = veh_id And v.alar_lat = lat And v.alar_lng = lng).FirstOrDefault()

    End Function

    Public Shared Function SeleccionarPatente(ByVal veh_patente As String) As Vehiculo
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) v.veh_patente = veh_patente).FirstOrDefault()

    End Function

    Public Shared Function Seleccionar(ByVal id_modulo As String) As Vehiculo
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Vehiculos.Where(Function(v) v.mod_id = id_modulo).FirstOrDefault()

    End Function

    Public Shared Function SeleccionarPosicion(ByVal id_posicion As Integer) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Return DB.vMonitoreos.Where(Function(v) v.Codigo = id_posicion).FirstOrDefault()

    End Function

    Public Shared Function InsertTrama(ByVal mensaje As String, ByVal modulo As String, ByVal version As String) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""


                sentencia = "INSERT INTO Recorridos.dbo.Tramas(trama,modulo, fecha, version) VALUES ('" & mensaje & "','" & modulo & "',GetDate(),'" & version & "') " & _
                            "SELECT @@Identity as codigo"


                Dim codigo As String = DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()


                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function UpdateMonitoreo(ByVal recorrido As vMonitoreo) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""

                sentencia = "UPDATE Recorridos.dbo.Monitoreos SET TIEMPO_PARCIAL=" & recorrido.TIEMPO_PARCIAL.ToString & " ,ESTADO = " & recorrido.ESTADO.ToString & ", KMS_RECORRIDOS = " & String.Format("{0:####0.000}", recorrido.KMS_RECORRIDOS).Replace(",", ".") & _
                            " WHERE codigo = " & recorrido.Codigo

                Dim codigo As String = DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()

                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function UpdateMonitoreoKms(ByVal recorrido As vMonitoreo) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""

                sentencia = "UPDATE Recorridos.dbo.Monitoreos SET  KMS_RECORRIDOS = " & String.Format("{0:####0.000}", recorrido.KMS_RECORRIDOS).Replace(",", ".") & _
                            " WHERE codigo = " & recorrido.Codigo

                Dim codigo As String = DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()

                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function UpdateMonitoreoTiempo(ByVal recorrido As vMonitoreo) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""

                sentencia = "UPDATE Recorridos.dbo.Monitoreos SET TIEMPO_PARCIAL=" & recorrido.TIEMPO_PARCIAL.ToString & " ,ESTADO = " & recorrido.ESTADO.ToString & " WHERE codigo = " & recorrido.Codigo

                Dim codigo As String = DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()

                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function UpdateMonitoreoProcesado(ByVal codigo As Integer) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""

                sentencia = "UPDATE Recorridos.dbo.Monitoreos SET procesado_recalcular= 1 WHERE codigo = " & codigo

                DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()

                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function UpdateMonitoreoProcesadoAlarma(ByVal codigo As Integer) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""

                sentencia = "UPDATE Recorridos.dbo.Monitoreos SET procesado_alarma= 1 WHERE codigo = " & codigo

                DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()

                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function Insert(ByVal movil As Vehiculo) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Vehiculos.InsertOnSubmit(movil)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function Update(ByVal movil As Vehiculo) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Vehiculo = DB.Vehiculos.Where(Function(b) b.veh_id = movil.veh_id).FirstOrDefault()

                oOriginal.cli_id = movil.cli_id
                oOriginal.mod_id = movil.mod_id
                oOriginal.veh_activo = movil.veh_activo
                oOriginal.veh_descripcion = movil.veh_descripcion
                oOriginal.veh_imagen = movil.veh_imagen
                oOriginal.veh_nombre_conductor = movil.veh_nombre_conductor
                oOriginal.veh_patente = movil.veh_patente
                oOriginal.veh_tipo_id = movil.veh_tipo_id
                oOriginal.veh_color = movil.veh_color
                oOriginal.tip_uso_id = movil.tip_uso_id
                oOriginal.veh_kilometros = movil.veh_kilometros
                oOriginal.veh_marca = movil.veh_marca
                oOriginal.veh_modelo = movil.veh_modelo
                oOriginal.veh_modulo_sensor = movil.veh_modulo_sensor
                oOriginal.veh_kilometros_acumulados = movil.veh_kilometros_acumulados


                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function ListRecorridos(ByVal veh_id As Integer, ByVal fecha_desde As String, ByVal fecha_hasta As String) As List(Of vMonitoreo)
        Try
            Dim DB As New DataClassesGPSDataContext()
            Return DB.vMonitoreos.Where(Function(v) v.ID_VEHICULO = veh_id And (v.FECHA >= Date.Parse(fecha_desde) Or fecha_desde = "") And (v.FECHA <= Date.Parse(fecha_hasta) Or fecha_hasta = "")).OrderBy(Function(v) v.FECHA).ToList()

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Shared Function searchLastLocation(ByVal mod_id As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,FECHA, ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION,TEMP, RPM, ENCENDIDO, OCUPADO, SENSORES,ID_VEHICULO,BATERIA, ESTADO from vMonitoreos " + _
                                        " where(ID_MODULO = '" + mod_id.ToString() + "' ) order by FECHA desc")


        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchLastValidLocation(ByVal mod_id As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,FECHA, ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION,TEMP, RPM, ENCENDIDO, OCUPADO, SENSORES,ID_VEHICULO,BATERIA, ESTADO from vMonitoreos " + _
                                        " where(ID_MODULO = '" + mod_id.ToString() + "' AND LATITUD <> '0' AND NOMBRE_VIA <> 'Sin Señal GPS.') order by FECHA desc")


        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchLastLocationValidByVehiculo(ByVal veh_id As Integer) As vMonitoreo
        Try
            Dim DB As New DataClassesGPSDataContext()
            Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,FECHA, ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION,TEMP, RPM, ENCENDIDO, OCUPADO, SENSORES,ID_VEHICULO,BATERIA, ESTADO, de_memoria from vMonitoreos " + _
                                            " WHERE ID_VEHICULO =" & veh_id & "  AND LATITUD <> '0' AND NOMBRE_VIA <> 'Sin Señal GPS.'  order by FECHA desc")

            Return recorrido.FirstOrDefault()

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function searchLastLocationByVehiculo(ByVal veh_id As Integer) As vMonitoreo
        Try
            Dim DB As New DataClassesGPSDataContext()
            Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,FECHA, ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION,TEMP, RPM, ENCENDIDO, OCUPADO, SENSORES,ID_VEHICULO,BATERIA, ESTADO,de_memoria from vMonitoreos " + _
                                            " WHERE ID_VEHICULO =" & veh_id & " order by FECHA desc")

            Return recorrido.FirstOrDefault()

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Shared Function searchEmptyDirection() As List(Of vMonitoreo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.ExecuteQuery(Of vMonitoreo)("select * from vMonitoreos " + _
             "where  NOMBRE_VIA = '' AND (LATITUD <> '0' AND  LATITUD <> '')" + _
             "order by FECHA desc").ToList
    End Function

    Public Shared Sub UpdateDirection(ByVal _nombrevia As String, ByVal _localidad As String, ByVal _prov As String, ByVal codigo As String, ByVal TipoVia As String)
        Dim DB As New DataClassesGPSDataContext()
        'update long, lat, calle, prov, localidad
        Dim sentencia As String = "UPDATE Recorridos.dbo.Monitoreos Set NOMBRE_VIA = '" & _nombrevia.Replace("'", "''") & "',localidad = '" & _localidad & "',PROVINCIA= '" & _prov & "', TIPO_VIA = " & TipoVia & " WHERE codigo = " & codigo
        DB.ExecuteQuery(Of Integer)(sentencia)

    End Sub

    Public Shared Function searchLastValidLocationFecha(ByVal mod_id As String, ByVal Fecha As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,MAX(FECHA) as FECHA,LATITUD, LONGITUD ,NOMBRE_VIA, LOCALIDAD, PROVINCIA,ESTADO, TIEMPO_PARCIAL from vMonitoreos " + _
             "where  ID_MODULO = '" + mod_id.ToString() + "' AND FECHA < '" & Fecha & "' AND NOMBRE_VIA <> 'Sin Señal GPS.' Group By codigo,ESTADO, LATITUD, LONGITUD,NOMBRE_VIA, LOCALIDAD, PROVINCIA,TIEMPO_PARCIAL " + _
             "order by FECHA desc")

        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchLastLocationFecha(ByVal mod_id As String, ByVal Fecha As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,MAX(FECHA) as FECHA,LATITUD, LONGITUD ,NOMBRE_VIA, LOCALIDAD, PROVINCIA,ESTADO, TIEMPO_PARCIAL from vMonitoreos " + _
             "where  ID_MODULO = '" + mod_id.ToString() + "' AND FECHA < '" & Fecha & "' AND  Group By codigo,ESTADO, LATITUD, LONGITUD,NOMBRE_VIA, LOCALIDAD, PROVINCIA,TIEMPO_PARCIAL " + _
             "order by FECHA desc")

        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchPreviusLocation(ByVal mod_id As String, ByVal idRegistro As Integer) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,MAX(FECHA) as FECHA, ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION, TEMP, RPM, ENCENDIDO, OCUPADO,SENSORES,ID_VEHICULO from vMonitoreos " + _
         "where  ID_MODULO = '" + mod_id.ToString() + "' AND LATITUD <> '0' AND Codigo <> " + idRegistro.ToString() + " AND NOMBRE_VIA <> 'Sin Señal GPS.' Group By codigo,ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION, TEMP, RPM, ENCENDIDO, OCUPADO,SENSORES,ID_VEHICULO " + _
         "order by FECHA desc")

        ' Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,MAX(FECHA) as FECHA, ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION from vMonitoreo " + _
        '     "where  ID_MODULO = '" + mod_id.ToString() + "' AND Codigo= 2963 Group By codigo,ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION " + _
        '    "order by FECHA desc")

        ' Dim recorrido = (From p In DB.vMonitoreo _
        ' Where p.ID_MODULO = mod_id _
        ' Select p).Max(Function(p) p.FECHA).OrderByDescending(Function(p) p.HORA).FirstOrDefault()
        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchPreviusLocationValid(ByVal mod_id As String, ByVal idRegistro As Integer, ByVal Fecha As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,MAX(FECHA) as FECHA,LATITUD, LONGITUD ,NOMBRE_VIA, LOCALIDAD, PROVINCIA,ESTADO, TIEMPO_PARCIAL,ORIENTACION,de_memoria from vMonitoreos " + _
         "where  ID_MODULO = '" + mod_id.ToString() + "' AND LATITUD <> '0' AND Codigo <> " + idRegistro.ToString() + " AND FECHA < '" & Fecha & "' AND NOMBRE_VIA <> 'Sin Señal GPS.' Group By codigo,ESTADO, LATITUD, LONGITUD,NOMBRE_VIA, LOCALIDAD, PROVINCIA,TIEMPO_PARCIAL,ORIENTACION " + _
         "order by FECHA desc")


        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchPreviusLocation(ByVal mod_id As String, ByVal idRegistro As Integer, ByVal Fecha As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,MAX(FECHA) as FECHA,LATITUD, LONGITUD ,NOMBRE_VIA, LOCALIDAD, PROVINCIA,ESTADO, TIEMPO_PARCIAL,ORIENTACION,de_memoria from vMonitoreos " + _
         "where  ID_MODULO = '" + mod_id.ToString() + "' AND LATITUD <> '0' AND Codigo <> " + idRegistro.ToString() + " AND FECHA < '" & Fecha & "'  Group By codigo,ESTADO, LATITUD, LONGITUD,NOMBRE_VIA, LOCALIDAD, PROVINCIA,TIEMPO_PARCIAL,ORIENTACION,de_memoria " + _
         "order by FECHA desc")


        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchPreviusLocationM(ByVal mod_id As String, ByVal idRegistro As Integer) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = DB.ExecuteQuery(Of vMonitoreo)("select top 1 Codigo,MAX(FECHA) as FECHA, ID_MODULO, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION, TEMP, RPM from vMonitoreos " + _
         "where  ID_MODULO = '" + mod_id.ToString() + "' AND Codigo < " + idRegistro.ToString() + " AND NOMBRE_VIA <> 'Sin Señal GPS.' Group By codigo,ID_MODULO, LATITUD, LONGITUD,NOMBRE_VIA, LOCALIDAD, PROVINCIA, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, ORIENTACION, TEMP, RPM " + _
         "order by FECHA desc")


        Return recorrido.FirstOrDefault()

    End Function

    Public Shared Function searchLastLocation(ByVal mod_id As String, ByVal filtro As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = (From p In DB.vMonitoreos _
            Where p.ID_MODULO = mod_id And p.NOMBRE_VIA <> "Sin Señal GPS." Or ((p.NOMBRE_VIA.Contains(filtro) Or filtro = "") Or (p.PROVINCIA.Contains(filtro) Or filtro = "") Or (p.LOCALIDAD.Contains(filtro) Or filtro = "")) _
            Order By p.Codigo Descending _
                       Select p).FirstOrDefault()
        Return recorrido

    End Function

    'retorno un solo objeto con los datos del vehiculo y la ubicacion actual
    Public Shared Function searchUbicaciones(ByVal cli_id As Integer, ByVal filtro As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido = (From v In DB.Vehiculos _
        Join p In DB.vMonitoreos On p.ID_VEHICULO Equals v.veh_id _
            Where v.cli_id = cli_id And p.NOMBRE_VIA <> "Sin Señal GPS." Or (v.veh_descripcion.Contains(filtro) Or filtro = "") Or v.veh_patente.Contains(filtro) Or ((p.NOMBRE_VIA.Contains(filtro) Or filtro = "") Or (p.PROVINCIA.Contains(filtro) Or filtro = "") Or (p.LOCALIDAD.Contains(filtro) Or filtro = "")) _
            Order By p.Codigo Descending _
                       Select p).FirstOrDefault()
        Return recorrido

    End Function

    'busco si ya grabe un registro con la misma ubicacion
    Public Shared Function searchUbicacionExistente(ByVal modulo As String, ByVal fecha As String, ByVal lat As String, ByVal lng As String) As vMonitoreo
        Dim DB As New DataClassesGPSDataContext()
        Dim recorrido As vMonitoreo
        If lat.Length > 7 Then
            recorrido = (From p In DB.vMonitoreos _
             Where p.ID_MODULO = modulo And p.LATITUD.Substring(0, 8) = lat.Substring(0, 8) And p.LONGITUD.Substring(0, 8) = lng.Substring(0, 8) And p.FECHA = fecha _
             Order By p.Codigo Descending _
                        Select p).FirstOrDefault()
        Else
            recorrido = (From p In DB.vMonitoreos _
            Where p.ID_MODULO = modulo And p.LATITUD.Substring(0, 8) = lat And p.LONGITUD.Substring(0, 8) = lng.Substring(0, 8) And p.FECHA = fecha _
            Order By p.Codigo Descending _
                       Select p).FirstOrDefault()
        End If

        Return recorrido

    End Function


    'tipo de vehiculo
    Public Shared Function ListTipoVehiculo() As List(Of Tipos_Vehiculos)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Tipos_Vehiculos.ToList()

    End Function

    Public Shared Function ListTipoUso() As List(Of Tipos_Usos_Moviles)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Tipos_Usos_Moviles.ToList()

    End Function

    Public Shared Function Borrar(ByVal veh_id As Integer) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Vehiculo = DB.Vehiculos.Where(Function(d) d.veh_id = veh_id).FirstOrDefault()


                DB.Alertas_Velocidad_Configuradas.DeleteAllOnSubmit(oOriginal.Alertas_Velocidad_Configuradas)
                DB.Cortes_Corrientes.DeleteAllOnSubmit(oOriginal.Cortes_Corrientes)
                DB.Sensores_Configurados.DeleteAllOnSubmit(oOriginal.Sensores_Configurados)
                DB.Sensores_Moviles.DeleteAllOnSubmit(oOriginal.Sensores_Moviles)
                DB.Grupos_Vehiculos.DeleteAllOnSubmit(oOriginal.Grupos_Vehiculos)


                For Each dir As Alertas_Direcciones In oOriginal.Alertas_Direcciones
                    clsAlarma.DeleteAlerta_Direccion(dir.adir_id)
                Next

                For Each zona As Alertas_Zonas In oOriginal.Alertas_Zonas
                    clsAlarma.DeleteAlerta_Zona(zona.azon_id)
                Next

                For Each rec As Alertas_Recorridos In oOriginal.Alertas_Recorridos
                    clsAlarma.DeleteAlerta_Recorrido(rec.arec_id)
                Next

                DB.Alamas_Kms_Excedidos.DeleteAllOnSubmit(oOriginal.Alamas_Kms_Excedidos)


                For Each inac As Alarmas_Inactividad In oOriginal.Alarmas_Inactividads
                    clsAlarma.DeleteAlertas_Inactividad(inac.alari_id)
                Next


                For Each inac As Alarma_Inicio_Actividad In oOriginal.Alarma_Inicio_Actividads
                    clsAlarma.DeleteAlertas_InicioActividad(inac.alaric_id)
                Next

                For Each inac As Alarmas_Fuera_Horario In oOriginal.Alarmas_Fuera_Horarios
                    clsAlarma.DeleteAlertas_Fuera_Horario(inac.alah_id)
                Next

                For Each inac As Alertas_Recordatorios_Por_Fechas In oOriginal.Alertas_Recordatorios_Por_Fechas
                    clsAlarma.DeleteAlertas_Recordatorio_Fecha(inac.recf_id)
                Next

                For Each inac As Alertas_Recordatorios_Por_Km In oOriginal.Alertas_Recordatorios_Por_Kms
                    clsAlarma.DeleteAlertas_Recordatorio_Km(inac.reck_id)
                Next


                DB.Vehiculos.DeleteOnSubmit(oOriginal)

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''corte de corriente
    Public Shared Function CortarCorriente(ByVal movil As Cortes_Corriente) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Cortes_Corrientes.InsertOnSubmit(movil)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function GetEstado(ByVal veh_id As Integer) As String

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim estado = (From p In DB.Cortes_Corrientes _
             Where p.veh_id = veh_id _
             Select p).OrderByDescending(Function(p) p.cor_id).FirstOrDefault()

                If estado IsNot Nothing Then
                    Return (estado.cort_tipo)
                Else
                    Return "R"
                End If

            End Using
        Catch ex As Exception
            Throw ex
        End Try



    End Function

    Public Shared Function GetEstadoCorte(ByVal veh_id As Integer) As String

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim estado = (From p In DB.Cortes_Corrientes _
             Where p.veh_id = veh_id _
             Select p).OrderByDescending(Function(p) p.cor_id).FirstOrDefault()

                If estado IsNot Nothing Then
                    If estado.cort_hecho Then
                        Return (estado.cort_tipo)
                    Else
                        Return "No_Enviado"
                    End If
                End If

            End Using
        Catch ex As Exception
            Throw ex
        End Try



    End Function
    Public Shared Function InsertMonitoreo(ByVal registroMonitoreo As vMonitoreo) As Integer

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""
                Dim _estado As String = ""
                Dim _tiempo As Double = 0
                Dim sKms As String = ""
                sKms = String.Format("{0:####0.000}", registroMonitoreo.KMS_RECORRIDOS)
                sKms = sKms.Replace(",", ".")

                If sKms = "NeuN" Then sKms = "0"

                sentencia = "INSERT INTO Recorridos.dbo.Monitoreos(ID_VEHICULO,Id_modulo, FECHA, latitud, longitud, velocidad, altura, TIPO_VIA, " & _
                            "NOMBRE_VIA, localidad, PROVINCIA, KMS_RECORRIDOS, ORIENTACION, Sensores, RPM, Temp, ENCENDIDO, OCUPADO, BATERIA,CODIGO_EVENTO, PRESICION_SATELITE, ODOMETRO, CARGA_BATERIA_INTERNA,de_memoria, procesado_alarma, procesado_recalcular, estado) VALUES " & _
                            " (" & registroMonitoreo.ID_VEHICULO & ",'" & registroMonitoreo.ID_MODULO & "','" & registroMonitoreo.FECHA.ToString("yyyyMMdd HH:mm:ss") & "','" & registroMonitoreo.LATITUD & "','" & registroMonitoreo.LONGITUD & _
                            "'," & registroMonitoreo.VELOCIDAD.ToString().Replace(",", ".") & "," & registroMonitoreo.ALTURA & "," & registroMonitoreo.TIPO_VIA & ",'" & registroMonitoreo.NOMBRE_VIA.Replace("'", "''") & "','" & registroMonitoreo.LOCALIDAD & "','" & _
                            registroMonitoreo.PROVINCIA & "'," & sKms & ",'" & registroMonitoreo.ORIENTACION & "','" & registroMonitoreo.SENSORES & "'," & registroMonitoreo.RPM.ToString().Replace(",", ".") & "," & _
                            registroMonitoreo.TEMP.ToString().Replace(",", ".") & "," & IIf(registroMonitoreo.ENCENDIDO, 1, 0) & "," & IIf(registroMonitoreo.OCUPADO, 1, 0) & "," & registroMonitoreo.BATERIA.ToString.Replace(",", ".") & "," & registroMonitoreo.CODIGO_EVENTO & "," & _
                            registroMonitoreo.PRESICION_SATELITE & "," & registroMonitoreo.ODOMETRO & "," & registroMonitoreo.CARGA_BATERIA_INTERNA & "," & IIf(registroMonitoreo.de_memoria, 1, 0) & ",0,0," & registroMonitoreo.ESTADO & ") " & _
                            "SELECT @@Identity as codigo"


                Dim codigo As String = DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()

                'Estados
                '1 -En Movimiento ENCENDIDO = 1 AND VELOCIDAD >= 5
                '2 -Motor Apagado ENCENDIDO = 0
                '3-Detenido en okm ENCENDIDO = 1 AND VELOCIDAD < 5
                'busco si tengo un registro anterior, y tengo qeu ver en que estado esta el actual para actualizar los valores Estado y tiempo parcial
                Dim _previusRegistro As vMonitoreo = clsVehiculo.searchPreviusLocation(registroMonitoreo.ID_MODULO, codigo, registroMonitoreo.FECHA.ToString("yyyyMMdd HH:mm:ss"))
                If registroMonitoreo.ENCENDIDO And registroMonitoreo.VELOCIDAD >= 5 Then _estado = "1"
                If Not registroMonitoreo.ENCENDIDO Then _estado = "2"
                If registroMonitoreo.ENCENDIDO And registroMonitoreo.VELOCIDAD < 5 Then _estado = "3"

                If _previusRegistro IsNot Nothing Then
                    _tiempo = diferenciaSegundos(_previusRegistro.FECHA, registroMonitoreo.FECHA)
                End If

                sentencia = "UPDATE Recorridos.dbo.Monitoreos Set ESTADO = " & _estado & " WHERE codigo = " & codigo
                DB.ExecuteQuery(Of Integer)(sentencia)

                sentencia = "UPDATE Recorridos.dbo.Monitoreos Set TIEMPO_PARCIAL = " & _tiempo & " WHERE codigo = " & _previusRegistro.Codigo
                DB.ExecuteQuery(Of Integer)(sentencia)
                Return CInt(codigo)
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function InsertMonitoreoSinSatelite(ByVal id_vehiculo As String, ByVal Id_modulo As String, ByVal fecha As String, sensores As String, ByVal RPM As String, ByVal Temp As String, ByVal encendido As String, ByVal ocupado As String, ByVal bateria As String, ByVal presicionSatalite As Integer, ByVal odometro As Integer, ByVal cargaBateriaInterna As Integer, ByVal codigoEvento As String) As Integer
        Dim codigo As String = ""
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim sentencia As String = ""
                Dim _estado As String = ""
               

               
                'Estados
                '1 -En Movimiento ENCENDIDO = 1 AND VELOCIDAD >= 5
                '2 -Motor Apagado ENCENDIDO = 0
                '3-Detenido en okm ENCENDIDO = 1 AND VELOCIDAD < 5
                'busco si tengo un registro anterior, y tengo qeu ver en que estado esta el actual para actualizar los valores Estado y tiempo parcial
               
                If encendido = "0" Then _estado = "2"
                If encendido = "1" Then _estado = "3"
               
                Dim registroMonitoreo = New vMonitoreo()

                sentencia = "INSERT INTO Recorridos.dbo.Monitoreos(ID_MODULO," & _
               " ID_VEHICULO," & _
               " ALTURA, " & _
               " BATERIA, " & _
               " ENCENDIDO, " & _
                "CODIGO_EVENTO, " & _
                "FECHA, " & _
                "KMS_RECORRIDOS, " & _
                "LATITUD, " & _
                "LONGITUD, " & _
                "NOMBRE_VIA, " & _
                "OCUPADO, " & _
                "PROVINCIA, " & _
                "LOCALIDAD, " & _
                "RPM, " & _
                "SENSORES, " & _
                "Temp, " & _
                "TIPO_VIA, " & _
                "VELOCIDAD, " & _
                "CARGA_BATERIA_INTERNA," & _
               " ODOMETRO, " & _
               " PRESICION_SATELITE, " & _
               " ESTADO, " & _
                "ORIENTACION,de_memoria, procesado_alarma, procesado_recalcular) VALUES " & _
                        " ('" & Id_modulo & "'," & id_vehiculo & ",1," & bateria.Replace(",", ".") & "," & encendido & _
                        "," & codigoEvento & ",'" & DateTime.Parse(fecha).ToString("yyyyMMdd HH:mm:ss") & "',0," & _
                        "'0','0','Sin Señal GPS.'," & _
                        ocupado & ",'',''," & RPM.Replace(",", ".") & ",'" & sensores & "'," & Temp.Replace(",", ".") & ",1,0," & _
                        cargaBateriaInterna & "," & odometro & "," & presicionSatalite & "," & _estado & ",1,0,1,1) " & _
                        "SELECT @@Identity as codigo"

                codigo = DB.ExecuteQuery(Of Decimal)(sentencia).FirstOrDefault().ToString()

                'actualizo el registro anterior

                Dim _previusRegistro As vMonitoreo = clsVehiculo.searchPreviusLocation(Id_modulo, codigo, DateTime.Parse(fecha).ToString("yyyyMMdd HH:mm:ss"))

                If _previusRegistro IsNot Nothing Then

                    'Estados
                    '1 -En Movimiento ENCENDIDO = 1 AND VELOCIDAD >= 5
                    '2 -Motor Apagado ENCENDIDO = 0
                    '3-Detenido en okm ENCENDIDO = 1 AND VELOCIDAD < 5
                    'busco si tengo un registro anterior, y tengo qeu ver en que estado esta el actual para actualizar los valores Estado y tiempo parcial

                    Dim _tiempo As Double = diferenciaSegundos(_previusRegistro.FECHA, DateTime.Parse(fecha))

                    If registroMonitoreo.ENCENDIDO And registroMonitoreo.VELOCIDAD >= 5 Then _estado = "1"
                    If Not registroMonitoreo.ENCENDIDO Then _estado = "2"
                    If registroMonitoreo.ENCENDIDO And registroMonitoreo.VELOCIDAD < 5 Then _estado = "3"

                    sentencia = "UPDATE Recorridos.dbo.Monitoreos Set ESTADO = " & _estado & ", LATITUD='" & _previusRegistro.LATITUD & "', LONGITUD ='" & _previusRegistro.LONGITUD & "', ORIENTACION=" & _previusRegistro.ORIENTACION & " WHERE codigo = " & codigo
                    DB.ExecuteQuery(Of Integer)(sentencia)

                    sentencia = "UPDATE Recorridos.dbo.Monitoreos Set TIEMPO_PARCIAL = " & _tiempo & " WHERE codigo = " & _previusRegistro.Codigo
                    DB.ExecuteQuery(Of Integer)(sentencia)

                    Return codigo
                End If

            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function searchTramas(ByVal mod_id As String, ByVal FechasDesde As String, ByVal FechasHasta As String) As List(Of Tramas)
        Try
            Dim DB As New DataClassesGPSDataContext()
            Dim tramas As List(Of Tramas) = DB.ExecuteQuery(Of Tramas)("SELECT  Id, Fecha,Modulo,Trama  FROM Recorridos.dbo.Tramas WHERE (Modulo = '" & mod_id & "') AND (Fecha BETWEEN '" & FechasDesde & "' AND '" & FechasHasta & "')").ToList()

            Return tramas

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Private Shared Function diferenciaSegundos(fecha_desde As String, fecha_hasta As String) As Double
        Using DB As New DataClassesGPSDataContext()
            Dim inicio As DateTime = DateTime.Parse(fecha_desde)

            'EJECUCIÓN de un proceso

            Dim final As DateTime = DateTime.Parse(fecha_hasta)
            Dim duracion As TimeSpan = final - inicio
            Dim segundosTotales As Double = duracion.TotalSeconds

            Return segundosTotales
        End Using
    End Function

    Public Shared Function KmsAcumuladosByModulo(ByVal id_modulo As String) As Integer
        Using DB As New DataClassesGPSDataContext()
            Dim Kms As Integer = 0

            If DB.vMonitoreos.Where(Function(r) r.ID_MODULO = id_modulo).ToList().Count > 0 Then
                Kms = DB.vMonitoreos.Where(Function(r) r.ID_MODULO = id_modulo).Sum(Function(r) r.KMS_RECORRIDOS)
            End If

            Return Kms
        End Using
    End Function

    Public Shared Function KmsAcumuladosByVehiculo(ByVal veh_id As Integer) As Integer
        Using DB As New DataClassesGPSDataContext()
            Dim Kms As Integer = 0

            If DB.vMonitoreos.Where(Function(r) r.ID_VEHICULO = veh_id).ToList().Count > 0 Then
                Kms = DB.vMonitoreos.Where(Function(r) r.ID_VEHICULO = veh_id).Sum(Function(r) r.KMS_RECORRIDOS)
            End If

            Return Kms
        End Using
    End Function

    Public Shared Function KmsAcumuladosHastaByVehiculo(ByVal veh_id As Integer, ByVal fecha As String) As Decimal
        Using DB As New DataClassesGPSDataContext()
            Dim Kms As Decimal = 0

            Kms = DB.ExecuteQuery(Of vMonitoreo)(" SELECT KMS_RECORRIDOS FROM  vMonitoreos where ID_VEHICULO = " & veh_id & " AND FECHA <= '" + fecha + "'").Sum(Function(r) r.KMS_RECORRIDOS)

            Return Kms
        End Using
    End Function



    Public Shared Function KmsAcumuladosDiario(ByVal veh_id As Integer, ByVal fecha As DateTime) As Double
        Using DB As New DataClassesGPSDataContext()
            Dim Kms As Double = 0

            Kms = DB.vMonitoreos.Where(Function(r) r.ID_VEHICULO = veh_id And (r.FECHA >= CDate(fecha.ToString("dd/MM/yyyy 00:00:00")) And r.FECHA <= CDate(fecha.ToString("dd/MM/yyyy 23:59:00")))).Sum(Function(r) r.KMS_RECORRIDOS)

            Return Kms
        End Using
    End Function

    Public Shared Function KmsAcumuladosSemanal(ByVal veh_id As Integer, ByVal fecha As String) As Double
        Using DB As New DataClassesGPSDataContext()
            Dim Kms As Double = 0

            Kms = DB.ExecuteQuery(Of vMonitoreo)(" SELECT KMS_RECORRIDOS FROM  vMonitoreos where ID_VEHICULO = " & veh_id & " AND DatePart(week, FECHA) = DatePart(week, '" & fecha & "')").Sum(Function(r) r.KMS_RECORRIDOS)

            Return Kms
        End Using
    End Function

    Public Shared Function KmsAcumuladosMensual(ByVal veh_id As Integer, ByVal fecha As DateTime) As Integer
        Using DB As New DataClassesGPSDataContext()
            Dim Kms As Integer = 0

            If DB.vMonitoreos.Where(Function(r) r.ID_VEHICULO = veh_id).ToList().Count > 0 Then
                Kms = DB.vMonitoreos.Where(Function(r) r.ID_VEHICULO = veh_id And r.FECHA.Month = fecha.Month).Sum(Function(r) r.KMS_RECORRIDOS)
            End If

            Return Kms
        End Using
    End Function


    Public Shared Sub Backup(ByVal veh_id As Integer, ByVal fechaD As String)
        Try
            Using DB As New DataClassesGPSDataContext()

                DB.ExecuteQuery(Of vMonitoreo)(" INSERT INTO RecorridosBack" & DateTime.Now.Year & ".dbo.Monitoreos SELECT (2000)  ID_MODULO, FECHA, LATITUD, LONGITUD, VELOCIDAD, ALTURA, TIPO_VIA, NOMBRE_VIA, LOCALIDAD, PROVINCIA, KMS_RECORRIDOS, " &
                " ORIENTACION, Sensores, TEMP, RPM, ID_VEHICULO, ENCENDIDO, OCUPADO, BATERIA, ESTADO, TIEMPO_PARCIAL, PRESICION_SATELITE, ODOMETRO, " &
                " CARGA_BATERIA_INTERNA, CODIGO_EVENTO, de_memoria, procesado_alarma, procesado_recalcular " &
                " FROM Recorridos.dbo.Monitoreos where ID_VEHICULO = " & veh_id & " AND FECHA <= '" + fechaD + "'")

                DB.ExecuteQuery(Of vMonitoreo)("DELETE TOP (2000) Recorridos.dbo.Monitoreos where ID_VEHICULO = " & veh_id & " AND FECHA <= '" + fechaD + "'")

            End Using
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Function BackupCantRegistros(ByVal veh_id As Integer, ByVal fechaD As String) As Integer
        Try
            Dim total As Integer = 0
            Using DB As New DataClassesGPSDataContext()

                total = DB.ExecuteQuery(Of Integer)("SELECT COUNT(Codigo) as total FROM Recorridos.dbo.Monitoreos where ID_VEHICULO = " & veh_id & " AND FECHA <= '" + fechaD + "'").FirstOrDefault()

            End Using

            Return total
        Catch ex As Exception
            Throw ex
        End Try


    End Function

End Class
