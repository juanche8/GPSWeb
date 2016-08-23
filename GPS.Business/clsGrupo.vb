Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data
Public Class clsGrupo
    Public Shared Function Search(ByVal cli_id As Integer) As List(Of Grupos)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Grupos.Where(Function(c) c.cli_id = cli_id).ToList()
    End Function

    Public Shared Function SelectById(ByVal grup_id As Integer) As Grupos
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Grupos.Where(Function(c) c.grup_id = grup_id).FirstOrDefault()
    End Function

    Public Shared Function PerteneceGrupo(ByVal grup_id As Integer, ByVal veh_id As Integer) As Grupos_Vehiculos
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Grupos_Vehiculos.Where(Function(c) c.grup_id = grup_id And c.veh_id = veh_id).FirstOrDefault()
    End Function

    Public Shared Function SelectByNombre(ByVal cli_id As Integer, ByVal nombre As String) As Grupos
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Grupos.Where(Function(c) c.grup_nombre = nombre And c.cli_id = cli_id).FirstOrDefault()
    End Function

    Public Shared Function List() As List(Of Grupos)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Grupos.ToList()
    End Function

    Public Shared Function Insert(ByVal grupo As Grupos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()
        Try
            DB.Grupos.InsertOnSubmit(grupo)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function InsertGrupo(ByVal movil As Grupos_Vehiculos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()
        Try
            DB.Grupos_Vehiculos.InsertOnSubmit(movil)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function Update(ByVal grupo As Grupos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim oOriginal As Grupos = DB.Grupos.Where(Function(d) d.grup_id = grupo.grup_id).FirstOrDefault()

            oOriginal.grup_nombre = grupo.grup_nombre
          

            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function Delete(ByVal grup_id As Integer) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            Dim grupo As Grupos = DB.Grupos.SingleOrDefault(Function(d) d.grup_id = grup_id)
            If grupo IsNot Nothing Then
                DB.Grupos_Vehiculos.DeleteAllOnSubmit(grupo.Grupos_Vehiculos)
                DB.Grupos.DeleteOnSubmit(grupo)
                DB.SubmitChanges()

            End If
          
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteMoviles(ByVal grup_id As Integer) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            Dim grupo As Grupos = DB.Grupos.SingleOrDefault(Function(d) d.grup_id = grup_id)
            DB.Grupos_Vehiculos.DeleteAllOnSubmit(grupo.Grupos_Vehiculos)
             DB.SubmitChanges()

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
