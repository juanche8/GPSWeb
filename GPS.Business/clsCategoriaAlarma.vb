Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Public Class clsCategoriaAlarma
    Public Shared Function ListExcVelocidad() As List(Of Alarmas_Velocidad)
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Alarmas_Velocidad.ToList()
    End Function

    Public Shared Function ListSensoresAlarma() As List(Of Sensores)
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Sensores.Where(Function(s) s.sen_binario = True).OrderBy(Function(s) s.sen_nombre).ToList()
    End Function

    Public Shared Function ListSensores() As List(Of Sensores)
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Sensores.OrderBy(Function(s) s.sen_nombre).ToList()
    End Function

    Public Shared Function ListSensoresByMovil(ByVal veh_id As Integer) As List(Of Sensores)
        Dim DB As New DataClassesGPSDataContext()

        Dim sensores = (From s In DB.Sensores _
                        Join c In DB.Sensores_Moviles On s.sen_id Equals c.sen_id
            Where c.veh_id = veh_id _
                           Select s).ToList()
        Return sensores
    End Function

    Public Shared Function GetSensorByMovil(ByVal veh_id As Integer, ByVal posicion As Integer) As Sensores
        Dim DB As New DataClassesGPSDataContext()

        Dim sensores = (From s In DB.Sensores _
                        Join c In DB.Sensores_Moviles On s.sen_id Equals c.sen_id
            Where c.veh_id = veh_id And s.sen_posicion = posicion _
                           Select s).FirstOrDefault()
        Return sensores
    End Function

    Public Shared Function SensorByMovil(ByVal veh_id As Integer, ByVal sen_id As Integer) As Sensores
        Dim DB As New DataClassesGPSDataContext()

        Dim sensores = (From s In DB.Sensores _
                        Join c In DB.Sensores_Moviles On s.sen_id Equals c.sen_id
            Where c.veh_id = veh_id And s.sen_id = sen_id _
                           Select s).FirstOrDefault()
        Return sensores
    End Function

    Public Shared Function SensorByPosicion(ByVal posicion As Integer) As Sensores
        Dim DB As New DataClassesGPSDataContext()

        Dim sensores = (From s In DB.Sensores _
            Where s.sen_posicion = posicion _
                           Select s).FirstOrDefault()
        Return sensores
    End Function

    Public Shared Function GetSensorAsignado(ByVal veh_id As Integer, ByVal sen_id As Integer) As Sensores_Moviles
        Dim DB As New DataClassesGPSDataContext()

        Dim sensores = (From s In DB.Sensores_Moviles
            Where s.veh_id = veh_id And s.sen_id = sen_id _
                           Select s).FirstOrDefault()
        Return sensores
    End Function

    Public Shared Function SelectAlarmaVelocidad(ByVal vel_id As Integer) As Alarmas_Velocidad
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Alarmas_Velocidad.Where(Function(v) v.vel_id = vel_id).SingleOrDefault()
    End Function

    Public Shared Function ListConfiguradas(ByVal vel_id As Integer) As List(Of Alertas_Velocidad_Configuradas)
        Dim DB As New DataClassesGPSDataContext()

        Dim alarmas = (From p In DB.Alertas_Velocidad_Configuradas _
            Where p.vel_id = vel_id _
                           Select p).ToList()
        Return alarmas

    End Function

    Public Shared Function DeleteSensorByMovil(ByVal veh_id As Integer) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim original As List(Of Sensores_Moviles) = DB.Sensores_Moviles.Where(Function(b) b.veh_id = veh_id).ToList()

            If original IsNot Nothing Then
                DB.Sensores_Moviles.DeleteAllOnSubmit(original)
                DB.SubmitChanges()
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertSensorMovil(ByVal sensor As Sensores_Moviles) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Sensores_Moviles.InsertOnSubmit(sensor)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
