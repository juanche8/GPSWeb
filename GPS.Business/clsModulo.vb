
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

''' <summary>
''' Esta clase maneja los metodos para acutalizar, insertar y consultar los datos de un modulo GPS instalado en un movil
''' registra las conexiones y desconexiones al socket, tambien adminsitra los comandos que se envian al modulo
''' </summary>

Public Class clsModulo


    ''' <summary>
    ''' Busca un modulo por su numero asignado - Utilizado en filtros
    ''' </summary>
    ''' <param name="nroModulo"></param>
    ''' <returns>List of Modulo</returns>
    ''' <remarks></remarks>
    Public Shared Function Search(ByVal nroModulo As String) As List(Of Modulo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos.Where(Function(c) c.mod_numero = nroModulo Or nroModulo = 0).ToList()

    End Function

    ''' <summary>
    ''' Lista todos los modulos creados en el sistema
    ''' </summary>
    ''' <returns>List of Modulo</returns>
    ''' <remarks></remarks>
    Public Shared Function List() As List(Of Modulo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos.ToList()


    End Function

    ''' <summary>
    ''' Lista todos los modulos creados en el sistema no asignados a un movil
    ''' </summary>
    ''' <returns>List of Modulo</returns>
    ''' <remarks></remarks>

    Public Shared Function ListDisponibles() As List(Of Modulo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos.Where(Function(c) c.mod_en_uso = False).ToList()

    End Function


    ''' <summary>
    ''' Busca un modulo por su numero asignado
    ''' </summary>
    ''' <param name="mod_numero"></param>
    ''' <returns>List of Modulo</returns>
    ''' <remarks></remarks>
    Public Shared Function Selecionar(ByVal mod_numero As String) As Modulo
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos.Where(Function(c) c.mod_numero = mod_numero).FirstOrDefault()

    End Function
	
	''' <summary>
    ''' Busca un modulo por su Id
    ''' </summary>
    ''' <param name="mod_id"></param>
    ''' <returns>List of Modulo</returns>
    ''' <remarks></remarks>
    Public Shared Function SelecionarById(ByVal mod_id As Integer) As Modulo
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos.Where(Function(c) c.mod_id = mod_id).FirstOrDefault()

    End Function

	''' <summary>
    ''' Elimina un registro de Modulo por su Id
    ''' </summary>
    ''' <param name="Id"></param>    
    ''' <remarks></remarks>
    Public Shared Function Delete(ByVal Id As Integer) As [Boolean]

        Using DB As New DataClassesGPSDataContext()
            Try
                Dim oOriginal As Modulo = DB.Modulos.Where(Function(c) c.mod_id = Id).FirstOrDefault()

                DB.Modulos.DeleteOnSubmit(oOriginal)
                DB.SubmitChanges()

                Return True

            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Function

    'CONEXIONES

    ''' <summary>
    ''' Busca todos los modulos informados como conectados al socket
    ''' </summary>  
    ''' <returns>List of Modulos_Conexiones object</returns>
    ''' <remarks></remarks>
    Public Shared Function ListConectados() As List(Of Modulos_Conexiones)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos_Conexiones.Where(Function(c) c.con_fecha_desconexion Is Nothing).OrderByDescending(Function(c) c.con_fecha).ToList()

    End Function

    ''' <summary>
    ''' Busca todos los modulos informados como conectados al socket para una version especifica de trama
    ''' </summary>  
    ''' <returns>List of Modulos_Conexiones object</returns>
    ''' <param name="versionTrama"></param>
    ''' <remarks></remarks>
    Public Shared Function ListConectadosByVersion(ByVal versionTrama As String) As List(Of Modulos_Conexiones)
        Dim DB As New DataClassesGPSDataContext()
        ' Return (From v In DB.Modulos_Conexiones _
        ' Join p In DB.Modulos On p.mod_numero Equals v.mod_id _
        '   Where v.con_fecha_desconexion Is Nothing And p.mod_version_trama = versionTrama _
        ' Select v).ToList()

        Return DB.ExecuteQuery(Of Modulos_Conexiones)("SELECT  Modulos_Conexiones.con_id, Modulos_Conexiones.con_fecha, Modulos_Conexiones.con_fecha_desconexion, Modulos_Conexiones.mod_id, " & _
               " Modulos_Conexiones.con_terminal FROM   Modulos_Conexiones INNER JOIN  Modulos ON Modulos_Conexiones.mod_id = Modulos.mod_numero " & _
          " WHERE (Modulos_Conexiones.con_fecha_desconexion IS NULL) AND (Modulos.mod_version_trama = '" & versionTrama & "')").ToList()

    End Function


    Public Shared Function ListByVersion(ByVal versionTrama As String) As List(Of Modulo)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos.Where(Function(c) c.mod_version_trama = versionTrama And c.mod_en_uso = True).ToList()

    End Function

     ''' <summary>
    ''' Busca el registro de conexion al socket de un modulo por su nro asignado
    ''' </summary>  
    ''' <returns>List of Modulos_Conexiones object</returns>
    ''' <param name="modulo"></param>
    ''' <remarks></remarks>
    Public Shared Function ListConexionesByModulo(ByVal modulo As String) As List(Of Modulos_Conexiones)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Modulos_Conexiones.Where(Function(c) c.mod_id = modulo And c.con_fecha_desconexion Is Nothing).OrderByDescending(Function(c) c.con_fecha).ToList()

    End Function

   ''' <summary>
    ''' Modifica los datos de un Modulo en el sistema
    ''' </summary> 
    ''' <param name="modulo">Object Modulo</param>
    ''' <remarks></remarks>
 Public Shared Function Update(ByVal modulo As Modulo) As [Boolean]
     
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Modulo = DB.Modulos.Where(Function(d) d.mod_id = modulo.mod_id).FirstOrDefault()

                oOriginal.mod_en_uso = modulo.mod_en_uso
                oOriginal.mod_numero = modulo.mod_numero
                oOriginal.mod_nro_cel = modulo.mod_nro_cel
                oOriginal.mod_version_trama = modulo.mod_version_trama

                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

   
    ''' <summary>
    ''' Inserta un nuevo Modulo en el sistema
    ''' </summary> 
    ''' <param name="modulo"></param>
    ''' <remarks></remarks>
    Public Shared Function Insert(ByVal modulo As Modulo) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Modulos.InsertOnSubmit(modulo)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Inserta un nueva desconexion de un Modulo al socket por IP
    ''' </summary>     
    ''' <param name="Ip"></param>
    ''' <remarks></remarks>
    Public Shared Function InsertDesconexion(ByVal Ip As String) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Modulos_Conexiones = DB.Modulos_Conexiones.Where(Function(d) d.con_terminal = Ip).OrderByDescending(Function(d) d.con_id).FirstOrDefault()

                If oOriginal IsNot Nothing Then
                    oOriginal.con_fecha_desconexion = DateTime.Now
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Modifica una conexion de unModulo al socket como desconectada, asignandole una fecha de desconexion
    ''' </summary>     
    ''' <param name="con_id">Integer Id del registro de Modulos Conexiones</param>
    ''' <remarks></remarks>
    Public Shared Function UpdateDesconexion(ByVal con_id As Integer) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Modulos_Conexiones = DB.Modulos_Conexiones.Where(Function(d) d.con_id = con_id).FirstOrDefault()

                If oOriginal IsNot Nothing Then
                    oOriginal.con_fecha_desconexion = DateTime.Now
                    DB.SubmitChanges()
                End If

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Inserta una conexion de un Modulo al socket
    ''' </summary>     
    ''' <param name="modulo">String nro de modulo asignado</param>
    ''' <param name="Ip">String Ip informada por el modulo</param>
    ''' <remarks></remarks>
    Public Shared Function InsertConexion(ByVal modulo As String, ByVal Ip As String) As [Boolean]

        Try
            Dim DB As New DataClassesGPSDataContext()
            'actualizo las otras conexiones del modulo con la fecha para que no queden abiertas
            Dim conexiones As List(Of Modulos_Conexiones) = ListConexionesByModulo(modulo)

            For Each con As Modulos_Conexiones In conexiones
                UpdateDesconexion(con.con_id)
            Next

            ' Dim sentencia As String = "INSERT INTO Modulos_Conexiones(con_fecha,mod_id,con_terminal) VALUES (GetDate(),'" & modulo.Trim() & "','" & Ip.Trim & "')" & _
            '                          "SELECT @@Identity as codigo"
            ' DB.ExecuteQuery(Of Decimal)(sentencia)

            Dim conexion As Modulos_Conexiones = New Modulos_Conexiones()
            conexion.con_fecha = DateTime.Now
            conexion.mod_id = modulo
            conexion.con_terminal = Ip
            DB.Modulos_Conexiones.InsertOnSubmit(conexion)
            DB.SubmitChanges()
            Return True



        Catch ex As Exception

            Throw ex
        End Try
    End Function

    'COMANDOS


    ''' <summary>
    ''' Inserta una nueva respuesta de un Modulo ante el envio de un comando
    ''' </summary>     
    ''' <param name="respuesta">Object Respuestas_comandos</param>
    ''' <remarks></remarks>
    Public Shared Function InsertRespuesta(ByVal respuesta As Respuestas_Comando) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.ExecuteQuery(Of Decimal)("INSERT INTO Respuestas_Comandos (mod_id,men_fecha,men_respuesta) VALUES('" & respuesta.mod_id & "','" & respuesta.men_fecha.ToString("yyyyMMdd HH:mm:ss") & "','" & respuesta.men_respuesta & "')")
                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

   ''' <summary>
    ''' Inserta una nuevo registro de envio de un comando a un modulo
    ''' </summary>     
    ''' <param name="Comando">Object Comandos_Enviados</param>
    ''' <remarks></remarks>
    Public Shared Function InsertComando(ByVal Comando As Comandos_Enviado) As [Boolean]

        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Comandos_Enviados.InsertOnSubmit(Comando)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

   
    ''' <summary>
    ''' Marca un mensaje como enviado a un modulo
    ''' </summary>
   ''' <param name="id_mensaje">Integer Id del registro de Comandos Enviados</param>
    ''' <remarks></remarks>
    Public Shared Function UpdateMensajeEnviado(ByVal id_mensaje As Integer) As [Boolean]
     
        Try
            Dim DB As New DataClassesGPSDataContext()
            Dim oOriginal As Comandos_Enviado = DB.Comandos_Enviados.Where(Function(d) d.men_id = id_mensaje).FirstOrDefault()

            If (oOriginal IsNot Nothing) Then
                oOriginal.men_enviado = True

                DB.SubmitChanges()
            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

   ''' <summary>
    ''' Marca un Corte como enviado a un modulo
    ''' </summary>
   ''' <param name="id_mensaje">Integer Id del registro de Cortes_Corrientes</param>
    ''' <remarks></remarks>
    Public Shared Function UpdateCorteEnviado(ByVal id_mensaje As Integer) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Cortes_Corriente = DB.Cortes_Corrientes.Where(Function(d) d.cor_id = id_mensaje).FirstOrDefault()

                If oOriginal IsNot Nothing Then
                    oOriginal.cort_hecho = True

                    DB.SubmitChanges()
                End If
              
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Lista todos los comandos dados de alta en el sistema
    ''' </summary>
    ''' <returns>List of Comandos</returns>
    ''' <remarks></remarks>
    Public Shared Function ListComandos() As List(Of Comando)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Comandos.ToList()


    End Function

    ''' <summary>
    ''' Lista los mensajes pendientes de envio para un modulo
    ''' </summary>
    ''' <param name="mod_id"></param>
    ''' <returns>list of Comandos_Enviados estado false</returns>
    ''' <remarks></remarks>
    Public Shared Function ListMensajes(ByVal mod_id As Integer) As List(Of Comandos_Enviado)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Comandos_Enviados.Where(Function(c) c.mod_id = mod_id And c.men_enviado = False).ToList()

    End Function

    ''' <summary>
    ''' Lista todas las respuestas a mensajes enviadas por los modulos
    ''' </summary>
    ''' <returns>list of Respuestas_Comandos</returns>
    ''' <remarks></remarks>
    Public Shared Function ListRespuestas() As List(Of Respuestas_Comando)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Respuestas_Comandos.OrderByDescending(Function(c) c.men_fecha).ToList()

    End Function

    ''' <summary>
    ''' Retorna los cortes pendientes de envio a un modulo
    ''' </summary>
    ''' <returns>list of Cortes_Corriente</returns>
    ''' <param name="modulo">Strig numero asignado al modulo</param>
    ''' <remarks></remarks>
    Public Shared Function ListCortes(ByVal modulo As String) As List(Of Cortes_Corriente)
        Dim DB As New DataClassesGPSDataContext()
            Return DB.Cortes_Corrientes.Where(Function(c) c.modulo_num = modulo And c.cort_hecho = False).ToList()

    End Function

    ''' <summary>
    ''' Retorna un objeto que representa a un Comando buscando por su Id
    ''' </summary>
    ''' <returns>Object Comandos</returns>
    ''' <param name="com_id">Integer Id del registro</param>
    ''' <remarks></remarks>
    Public Shared Function SelectComando(ByVal com_id As Integer) As Comando
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Comandos.Where(Function(c) c.com_id = com_id).FirstOrDefault()

    End Function

    Public Shared Function SelectComandoBySensor(ByVal sen_id As Integer, ByVal encender As Boolean) As Comando
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Comandos.Where(Function(c) c.sen_id = sen_id And c.com_encender = encender).FirstOrDefault()

    End Function

    Public Shared Function SelectComandoBySigla(ByVal sigla As String) As Comando
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Comandos.Where(Function(c) c.com_comando = sigla).FirstOrDefault()

    End Function
End Class
