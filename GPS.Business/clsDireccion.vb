Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

Public Class clsDireccion
    Public Shared Function SelectById(ByVal dir_id As Integer) As Direcciones
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Direcciones.Where(Function(d) d.dir_id = dir_id).FirstOrDefault()
    End Function

    Public Shared Function SelectByLtLg(ByVal lat As String, ByVal lon As String, ByVal dir_id As Integer) As Direcciones
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Direcciones.Where(Function(d) d.dir_latitud = lat And d.dir_longitud = lon And (d.dir_id <> dir_id Or dir_id = 0)).FirstOrDefault()
    End Function

    Public Shared Function List() As List(Of Direcciones)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Direcciones.ToList()
    End Function

    Public Shared Function ListByCliente(ByVal cli_id As Integer) As List(Of Direcciones)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Direcciones.Where(Function(s) s.cli_id = cli_id).ToList()
    End Function

    Public Shared Function Insert(ByVal punto As Direcciones) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Direcciones.InsertOnSubmit(punto)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

   

    Public Shared Function UpdateDireccion(ByVal direccion As Direcciones) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim oOriginal As Direcciones = DB.Direcciones.Where(Function(d) d.dir_id = direccion.dir_id).FirstOrDefault()

            oOriginal.dir_direccion = direccion.dir_direccion
            oOriginal.dir_latitud = direccion.dir_latitud
            oOriginal.dir_longitud = direccion.dir_longitud
            oOriginal.dir_nombre = direccion.dir_nombre

            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteDireccion(ByVal dir_id As Integer) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            Dim dir As Direcciones = DB.Direcciones.SingleOrDefault(Function(d) d.dir_id = dir_id)
            DB.Direcciones.DeleteOnSubmit(dir)
            DB.SubmitChanges()

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
