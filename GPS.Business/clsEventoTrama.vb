Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

Public Class clsEventoTrama

    Public Shared Function SelectByCodigo(ByVal eve_codigo As Integer) As Evento
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Eventos.Where(Function(d) d.eve_codigo = eve_codigo).FirstOrDefault()
    End Function

    Public Shared Function SelectById(ByVal eve_id As Integer) As Evento
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Eventos.Where(Function(d) d.eve_id = eve_id).FirstOrDefault()
    End Function

    Public Shared Function List() As List(Of Evento)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Eventos.ToList()
    End Function



    Public Shared Function Insert(ByVal evento As Evento) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Eventos.InsertOnSubmit(evento)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Shared Function Update(ByVal evento As Evento) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim oOriginal As Evento = DB.Eventos.Where(Function(d) d.eve_id = evento.eve_id).FirstOrDefault()

            oOriginal.eve_nombre = evento.eve_nombre

            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function Delete(ByVal eve_id As Integer) As Boolean
        Dim DB As New DataClassesGPSDataContext()

        Try

            Dim evento As Evento = DB.Eventos.SingleOrDefault(Function(d) d.eve_id = eve_id)
            DB.Eventos.DeleteOnSubmit(evento)
            DB.SubmitChanges()

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
