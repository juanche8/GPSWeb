Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Public Class clsParametros
    'tipo de clientes

    Public Shared Function TipoClienteList() As List(Of Tipos_Clientes)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Tipos_Clientes.ToList()
    End Function

    Public Shared Function ParametrosVelocidad() As List(Of Parametros)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Parametros.Where(Function(c) c.par_nombre.Contains("velocidad_")).ToList()
    End Function

    Public Shared Function TipoClienteSelect(ByVal tipo_cli_id As Integer) As Tipos_Clientes
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Tipos_Clientes.Where(Function(v) v.tipo_cli_id = tipo_cli_id).SingleOrDefault()
    End Function

    Public Shared Function ParametroSelect(ByVal nombre As String) As Parametros
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Parametros.Where(Function(v) v.par_nombre = nombre).SingleOrDefault()
    End Function

    Public Shared Function ParametroSelectbyId(ByVal id As Integer) As Parametros
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Parametros.Where(Function(v) v.par_id = id).SingleOrDefault()
    End Function

    Public Shared Function Update(ByVal parametro As Parametros) As Boolean
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Parametros = DB.Parametros.Where(Function(b) b.par_id = parametro.par_id).SingleOrDefault()

            oOriginal.par_valor = parametro.par_valor
            DB.SubmitChanges()

            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function TipoClienteUpdate(ByVal cliente As Tipos_Clientes) As Boolean
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Clientes = DB.Tipos_Clientes.Where(Function(b) b.tipo_cli_id = cliente.tipo_cli_id).SingleOrDefault()

            oOriginal.tipo_cli_nombre = cliente.tipo_cli_nombre
            DB.SubmitChanges()

            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function TipoClienteInsert(ByVal cliente As Tipos_Clientes) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Tipos_Clientes.InsertOnSubmit(cliente)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function TipoClienteDelete(ByVal Id As Integer) As [Boolean]

        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Clientes = DB.Tipos_Clientes.Where(Function(c) c.tipo_cli_id = Id).SingleOrDefault()

            DB.Tipos_Clientes.DeleteOnSubmit(oOriginal)
            DB.SubmitChanges()
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'tipos marcadores
    Public Shared Function TipoMarcadorSelect(ByVal tipo_marc_id As Integer) As Tipos_Marcadores
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Tipos_Marcadores.Where(Function(v) v.tipo_marc_id = tipo_marc_id).SingleOrDefault()
    End Function

    Public Shared Function TipoMarcadorUpdate(ByVal marcador As Tipos_Marcadores) As Boolean
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Marcadores = DB.Tipos_Marcadores.Where(Function(b) b.tipo_marc_id = marcador.tipo_marc_id).SingleOrDefault()

            oOriginal.tipo_marc_nombre = marcador.tipo_marc_nombre
            oOriginal.tipo_marc_imagen = marcador.tipo_marc_imagen
            DB.SubmitChanges()

            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function TipoMarcadorInsert(ByVal marcador As Tipos_Marcadores) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Tipos_Marcadores.InsertOnSubmit(marcador)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function TipoMarcadorDelete(ByVal Id As Integer) As [Boolean]

        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Marcadores = DB.Tipos_Marcadores.Where(Function(c) c.tipo_marc_id = Id).SingleOrDefault()

            DB.Tipos_Marcadores.DeleteOnSubmit(oOriginal)
            DB.SubmitChanges()
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'tipos vehiculos
    Public Shared Function TipoVehiculoSelect(ByVal veh_tipo_id As Integer) As Tipos_Vehiculos
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Tipos_Vehiculos.Where(Function(v) v.veh_tipo_id = veh_tipo_id).SingleOrDefault()
    End Function

    Public Shared Function TipoVehiculoUpdate(ByVal movil As Tipos_Vehiculos) As Boolean
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Vehiculos = DB.Tipos_Vehiculos.Where(Function(b) b.veh_tipo_id = movil.veh_tipo_id).SingleOrDefault()

            oOriginal.veh_tipo_detalle = movil.veh_tipo_detalle
            oOriginal.veh_tipo_icono = movil.veh_tipo_icono
            DB.SubmitChanges()

            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function TipoVehiculoInsert(ByVal movil As Tipos_Vehiculos) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Tipos_Vehiculos.InsertOnSubmit(movil)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function TipoVehiculoDelete(ByVal Id As Integer) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Vehiculos = DB.Tipos_Vehiculos.Where(Function(c) c.veh_tipo_id = Id).SingleOrDefault()

            DB.Tipos_Vehiculos.DeleteOnSubmit(oOriginal)
            DB.SubmitChanges()
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'tipos usos
    Public Shared Function TipoUsoSelect(ByVal tipo_uso_id As Integer) As Tipos_Usos_Moviles
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Tipos_Usos_Moviles.Where(Function(v) v.tipo_uso_id = tipo_uso_id).SingleOrDefault()
    End Function

    Public Shared Function TipoUsoUpdate(ByVal uso As Tipos_Usos_Moviles) As Boolean
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Usos_Moviles = DB.Tipos_Usos_Moviles.Where(Function(b) b.tipo_uso_id = uso.tipo_uso_id).SingleOrDefault()

            oOriginal.tipo_uso_descripcion = uso.tipo_uso_descripcion
            DB.SubmitChanges()

            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function TipoUsoInsert(ByVal uso As Tipos_Usos_Moviles) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Tipos_Usos_Moviles.InsertOnSubmit(uso)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function TipoUsoDelete(ByVal Id As Integer) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Usos_Moviles = DB.Tipos_Usos_Moviles.Where(Function(c) c.tipo_uso_id = Id).SingleOrDefault()

            DB.Tipos_Usos_Moviles.DeleteOnSubmit(oOriginal)
            DB.SubmitChanges()
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'tipos vias
    Public Shared Function TiemposEsperaList() As List(Of Parametros)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Parametros.Where(Function(b) b.par_nombre.Contains("velocidad")).ToList()
    End Function

    'tipos vias
    Public Shared Function ParametrosList() As List(Of Parametros)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Parametros.Where(Function(b) Not b.par_nombre.Contains("velocidad")).ToList()
    End Function
    'tipos vias
    Public Shared Function TipoViaList() As List(Of Tipos_Vias)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Tipos_Vias.ToList()
    End Function

    Public Shared Function TipoViaUpdate(ByVal via As Tipos_Vias) As Boolean
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Vias = DB.Tipos_Vias.Where(Function(b) b.tipo_via_id = via.tipo_via_id).SingleOrDefault()

            oOriginal.tipo_via_nombre = via.tipo_via_nombre
            DB.SubmitChanges()

            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function TipoViaSelect(ByVal tipo_via_id As Integer) As Tipos_Vias
        Dim DB As New DataClassesGPSDataContext()

        Return DB.Tipos_Vias.Where(Function(v) v.tipo_via_id = tipo_via_id).SingleOrDefault()
    End Function

    Public Shared Function TipoViaInsert(ByVal via As Tipos_Vias) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()

        Try
            DB.Tipos_Vias.InsertOnSubmit(via)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function TipoViaDelete(ByVal Id As Integer) As [Boolean]
        Dim DB As New DataClassesGPSDataContext()
        Try
            Dim oOriginal As Tipos_Vias = DB.Tipos_Vias.Where(Function(c) c.tipo_via_id = Id).SingleOrDefault()

            DB.Tipos_Vias.DeleteOnSubmit(oOriginal)
            DB.SubmitChanges()
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
