Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Public Class clsRecorrido
    Public Shared Function SelectById(ByVal rec_id As Integer) As Recorridos
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Recorridos.Where(Function(d) d.rec_id = rec_id).FirstOrDefault()
    End Function

    Public Shared Function SelectByDireccion(ByVal dir1 As String, ByVal dir2 As String, ByVal rec_id As Integer) As Recorridos
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Recorridos.Where(Function(d) (d.rec_id <> rec_id Or rec_id = 0) And d.rec_origen.ToLower() = dir1 And d.rec_destino.ToLower() = dir2).FirstOrDefault()
    End Function

    Public Shared Function List() As List(Of Recorridos)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Recorridos.ToList()
    End Function

    Public Shared Function ListByCliente(ByVal cli_id As Integer) As List(Of Recorridos)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Recorridos.Where(Function(s) s.cli_id = cli_id).ToList()
    End Function

    Public Shared Function Insert(ByVal punto As Recorridos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Recorridos.InsertOnSubmit(punto)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function RecorridoPuntosList(ByVal rec_id As Integer) As List(Of Recorridos_Puntos)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Recorridos_Puntos.Where(Function(s) s.rec_id = rec_id).ToList()
    End Function

    Public Shared Function DeleteRecorrido_Puntos(ByVal recorrido As Recorridos) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            For Each punto As Recorridos_Puntos In recorrido.Recorridos_Puntos
                Dim alerta As Recorridos_Puntos = DB.Recorridos_Puntos.SingleOrDefault(Function(d) d.rec_punto_id = punto.rec_punto_id)
                DB.Recorridos_Puntos.DeleteOnSubmit(alerta)
                DB.SubmitChanges()
            Next

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertPunto(ByVal punto As Recorridos_Puntos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            'verifico que no exista ya
            Dim _punto As Recorridos_Puntos = DB.Recorridos_Puntos.FirstOrDefault(Function(d) d.rec_id = punto.rec_id And d.rec_latitud = punto.rec_latitud And d.rec_longitud = punto.rec_longitud)

            If _punto Is Nothing Then
                DB.Recorridos_Puntos.InsertOnSubmit(punto)
                DB.SubmitChanges()
            End If
           
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateRecorrido(ByVal rec As Recorridos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim oOriginal As Recorridos = DB.Recorridos.Where(Function(d) d.rec_id = rec.rec_id).FirstOrDefault()

            oOriginal.rec_nombre = rec.rec_nombre
            oOriginal.rec_destino = rec.rec_destino
            oOriginal.rec_origen = rec.rec_origen


            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteRecorrido(ByVal rec_id As Integer) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            Dim zona As Recorridos = DB.Recorridos.SingleOrDefault(Function(d) d.rec_id = rec_id)
            DB.Recorridos_Puntos.DeleteAllOnSubmit(zona.Recorridos_Puntos)
            DB.Recorridos.DeleteOnSubmit(zona)
            DB.SubmitChanges()

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function ListRegistrosSinProcesar(ByVal mod_id As String) As List(Of vMonitoreo)
        Dim DB As New DataClassesGPSDataContext()
        Try
            Return DB.ExecuteQuery(Of vMonitoreo)("SELECT * FROM vMonitoreos " + _
        "WHERE ID_MODULO= '" & mod_id & "' AND  DE_MEMORIA = 1 AND PROCESADO_RECALCULAR = 0 ORDER BY FECHA").ToList
        Catch ex As Exception
            Throw ex
        End Try
        
    End Function

    Public Shared Function ListRegistrosSinTiempo(ByVal mod_id As String) As List(Of vMonitoreo)
        Dim DB As New DataClassesGPSDataContext()
        Try
            Return DB.ExecuteQuery(Of vMonitoreo)("SELECT * FROM vMonitoreos " + _
     "WHERE ID_MODULO= '" & mod_id & "' AND  TIEMPO_PARCIAL IS NULL ORDER BY FECHA").ToList
        Catch ex As Exception
            Throw ex
        End Try
     
    End Function

    Public Shared Function ListRegistrosSinProcesarAlarmas(ByVal mod_id As String) As List(Of vMonitoreo)
        Dim DB As New DataClassesGPSDataContext()
        Try
            DB.CommandTimeout = 120
            Return DB.ExecuteQuery(Of vMonitoreo)("SELECT Top 10 * FROM vMonitoreos " + _
                "WHERE ID_MODULO= '" & mod_id & "' AND  PROCESADO_ALARMA = 0  ORDER BY FECHA").ToList
        Catch ex As Exception
            Throw ex
        End Try
       
    End Function

    'Solo tramas hasta dos horas desde la fecha actual
    Public Shared Function ListRegistrosSinProcesarAlarmasDeMemoria(ByVal mod_id As String, ByVal fecha As String) As List(Of vMonitoreo)
        Dim DB As New DataClassesGPSDataContext()
        Try
            Return DB.ExecuteQuery(Of vMonitoreo)("SELECT * FROM vMonitoreos " + _
       "WHERE ID_MODULO= '" & mod_id & "' AND  PROCESADO_ALARMA = 0 AND de_memoria = 1 AND FECHA <= '" + fecha + "' ORDER BY FECHA").ToList
        Catch ex As Exception
            Throw ex
        End Try
       
    End Function
End Class
