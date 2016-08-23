Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Public Class clsZona
    Public Shared Function SelectById(ByVal zon_id As Integer) As Zonas
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Zonas.Where(Function(d) d.zon_id = zon_id).FirstOrDefault()
    End Function

    Public Shared Function List() As List(Of Zonas)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Zonas.ToList()
    End Function

    Public Shared Function ListByCliente(ByVal cli_id As Integer) As List(Of Zonas)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Zonas.Where(Function(s) s.cli_id = cli_id).ToList()
    End Function

    Public Shared Function Insert(ByVal punto As Zonas) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Zonas.InsertOnSubmit(punto)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function ZonaPuntosList(ByVal zon_id As Integer) As List(Of Zonas_Puntos)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Zonas_Puntos.Where(Function(s) s.zon_id = zon_id).ToList()
    End Function

    Public Shared Function DeleteZona_Puntos(ByVal zona As Zonas) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            For Each punto As Zonas_Puntos In zona.Zonas_Puntos
                Dim alerta As Zonas_Puntos = DB.Zonas_Puntos.SingleOrDefault(Function(d) d.zon_punto_id = punto.zon_punto_id)
                DB.Zonas_Puntos.DeleteOnSubmit(alerta)
                DB.SubmitChanges()
            Next

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function InsertPunto(ByVal punto As Zonas_Puntos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Zonas_Puntos.InsertOnSubmit(punto)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateZona(ByVal zona As Zonas) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim oOriginal As Zonas = DB.Zonas.Where(Function(d) d.zon_id = zona.zon_id).FirstOrDefault()

            oOriginal.zon_nombre = zona.zon_nombre
           
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteZona(ByVal zon_id As Integer) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            Dim zona As Zonas = DB.Zonas.SingleOrDefault(Function(d) d.zon_id = zon_id)
            DB.Zonas_Puntos.DeleteAllOnSubmit(zona.Zonas_Puntos)
            DB.Zonas.DeleteOnSubmit(zona)
            DB.SubmitChanges()

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class

'http://www.pronetsi.com.ar/Sistema-Gestion-Administrativa.html