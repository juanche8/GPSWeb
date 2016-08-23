Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Public Class clsMarcador
    Public Shared Function ListTiposMarcador() As List(Of Tipos_Marcadores)
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Tipos_Marcadores.ToList()
    End Function

    Public Shared Function List(ByVal cli_id As Integer) As List(Of Marcadores)
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Marcadores.Where(Function(c) c.cli_id = cli_id).ToList()
    End Function

    Public Shared Function SelectGenaricoById(ByVal mar_id As Integer) As Marcadores_Generico
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Marcadores_Genericos.Where(Function(c) c.marc_id = mar_id).SingleOrDefault()
    End Function

    Public Shared Function SelectById(ByVal mar_id As Integer) As Marcadores
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Marcadores.Where(Function(c) c.marc_id = mar_id).SingleOrDefault()
    End Function

    Public Shared Function SelectByLatLng(ByVal cli_id As Integer, ByVal lat As String, ByVal lng As String) As Marcadores
        Dim DB As New DataClassesGPSDataContext()
        Dim _marcadores As List(Of Marcadores)
        Dim _marca As Marcadores = New Marcadores
        _marcadores = DB.Marcadores.Where(Function(c) c.cli_id = cli_id).ToList()
        'tengo que buscar los marcadores por distancia de 200 metros
        For Each marca As Marcadores In _marcadores
            Dim distancia As Double = clsAlarma.Distance(Decimal.Parse(lat.Replace(".", ",")), Decimal.Parse(lng.Replace(".", ",")), Decimal.Parse(marca.marc_latitud.Replace(".", ",")), Decimal.Parse(marca.marc_longitud.Replace(".", ",")))

            If Math.Round(distancia, 2) <= Double.Parse("0,20") Then
                _marca = marca
            End If
        Next


        Return _marca
    End Function

    Public Shared Function SelectByNombre(ByVal cli_id As Integer, ByVal nombre As String) As Marcadores
        Dim DB As New DataClassesGPSDataContext()

        Dim _marca As Marcadores = New Marcadores
        _marca = DB.Marcadores.Where(Function(c) c.cli_id = cli_id And c.marc_nombre = nombre).FirstOrDefault()

        Return _marca
    End Function

    Public Shared Function SelectByLatLngEqual(ByVal cli_id As Integer, ByVal lat As String, ByVal lng As String) As Marcadores
        Dim DB As New DataClassesGPSDataContext()

        Dim _marca As Marcadores = New Marcadores
        _marca = DB.Marcadores.Where(Function(c) c.cli_id = cli_id And c.marc_latitud = lat And c.marc_longitud = lng).FirstOrDefault()

        Return _marca
    End Function

    Public Shared Function SelectByDirecc(ByVal direccion As String) As Marcadores
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Marcadores.Where(Function(c) c.marc_direccion = direccion).FirstOrDefault()
    End Function

    Public Shared Function SelectGenericoByLatLng(ByVal lat As String, ByVal lng As String) As Marcadores_Generico
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Marcadores_Genericos.Where(Function(c) c.marc_latitud = lat And c.marc_longitud = lng).FirstOrDefault()
    End Function

    Public Shared Function SelectGenericoByLatLng(ByVal cli_id As Integer, ByVal lat As String, ByVal lng As String) As Marcadores_Generico
        Dim DB As New DataClassesGPSDataContext()
        Dim _marcadores As List(Of Marcadores_Generico)
        Dim _marca As Marcadores_Generico = New Marcadores_Generico

        _marcadores = (From marca In DB.Marcadores_Genericos Join cliente In DB.Marcadores_GenericosXClientes On cliente.marc_id Equals marca.marc_id Where cliente.cli_id = cli_id Select marca).ToList()

        For Each marca As Marcadores_Generico In _marcadores
            Dim distancia As Double = clsAlarma.Distance(Decimal.Parse(lat.Replace(".", ",")), Decimal.Parse(lng.Replace(".", ",")), Decimal.Parse(marca.marc_latitud.Replace(".", ",")), Decimal.Parse(marca.marc_longitud.Replace(".", ",")))

            If Math.Round(distancia, 2) <= Double.Parse("0,20") Then
                _marca = marca
            End If
        Next


        Return _marca

    End Function

    Public Shared Function SelectGenericoByDirecc(ByVal direccion As String) As Marcadores_Generico
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Marcadores_Genericos.Where(Function(c) c.marc_direccion = direccion).FirstOrDefault()
    End Function

    Public Shared Function ListGenericos() As List(Of Marcadores_Generico)
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Marcadores_Genericos.ToList()
    End Function

    Public Shared Function ListGenericosByCliente(ByVal cli_id As Integer) As List(Of Marcadores_Generico)
        Dim DB As New DataClassesGPSDataContext()

        Return (From marca In DB.Marcadores_Genericos Join cliente In DB.Marcadores_GenericosXClientes On cliente.marc_id Equals marca.marc_id Where cliente.cli_id = cli_id Or marca.marc_mostrar_a_todos = True Select marca).ToList
    End Function

    Public Shared Function Insert(ByVal marca As Marcadores) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Marcadores.InsertOnSubmit(marca)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function InsertGenerico(ByVal marca As Marcadores_Generico) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Marcadores_Genericos.InsertOnSubmit(marca)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function InsertCliente(ByVal cliente As Marcadores_GenericosXClientes) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Marcadores_GenericosXClientes.InsertOnSubmit(cliente)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function UpdateGenerico(ByVal marca As Marcadores_Generico) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim oOriginal As Marcadores_Generico = DB.Marcadores_Genericos.Where(Function(d) d.marc_id = marca.marc_id).FirstOrDefault()

            oOriginal.marc_direccion = marca.marc_direccion
            oOriginal.marc_imagen = marca.marc_imagen
            oOriginal.marc_latitud = marca.marc_latitud
            oOriginal.marc_longitud = marca.marc_longitud
            oOriginal.marc_nombre = marca.marc_nombre
            oOriginal.tipo_marc_id = marca.tipo_marc_id
            oOriginal.marc_mostrar_a_todos = marca.marc_mostrar_a_todos
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function Update(ByVal marca As Marcadores) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            Dim oOriginal As Marcadores = DB.Marcadores.Where(Function(d) d.marc_id = marca.marc_id).FirstOrDefault()

            oOriginal.marc_direccion = marca.marc_direccion
            oOriginal.cli_id = marca.cli_id
            oOriginal.marc_latitud = marca.marc_latitud
            oOriginal.marc_longitud = marca.marc_longitud
            oOriginal.marc_nombre = marca.marc_nombre
            oOriginal.tipo_marc_id = marca.tipo_marc_id
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function


    Public Shared Function Delete(ByVal Id As Integer) As [Boolean]

        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Marcadores = DB.Marcadores.Where(Function(c) c.marc_id = Id).SingleOrDefault()

            DB.Marcadores.DeleteOnSubmit(oOriginal)

            DB.SubmitChanges()

            Return True

        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function DeleteGenerico(ByVal Id As Integer) As [Boolean]

        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Marcadores_Generico = DB.Marcadores_Genericos.Where(Function(c) c.marc_id = Id).SingleOrDefault()

            DB.Marcadores_GenericosXClientes.DeleteAllOnSubmit(oOriginal.Marcadores_GenericosXClientes)
            DB.Marcadores_Genericos.DeleteOnSubmit(oOriginal)

            DB.SubmitChanges()

            Return True

        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function DeleteCliente(ByVal marcador As Marcadores_Generico) As [Boolean]

        Dim DB As New DataClassesGPSDataContext()
        Try
            For Each cliente As Marcadores_GenericosXClientes In marcador.Marcadores_GenericosXClientes
                Dim marca As Marcadores_GenericosXClientes = DB.Marcadores_GenericosXClientes.SingleOrDefault(Function(d) d.marc_id = cliente.marc_id And d.cli_id = cliente.cli_id)
                DB.Marcadores_GenericosXClientes.DeleteOnSubmit(marca)
                DB.SubmitChanges()
            Next

            Return True

        Catch ex As Exception

            Throw ex
        End Try
    End Function
End Class
