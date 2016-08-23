Imports GPS.Business
Imports GPS.Data
Imports System.Web.Services
Imports System.Text

'Muestra la ubicacion actual de los vehiculos del cliente logeado.
Partial Public Class pMapa
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'cargo el combo de tipo de vhiculos

        ddlTipoMarcador.DataSource = clsMarcador.ListTiposMarcador()
        ddlTipoMarcador.DataBind()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            chkHoy.Attributes.Add("onclick", "LimpiarFechas();")
            lblError.Text = ""
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    'enter por defecto
                    ' DirectCast(Page.Master, SiteMaster).DefaultButton = btnDirecc ion.UniqueID

                    'busco los vehiculos del usuario
                    Dim cli_id As Integer = Session("Cliente").ToString()
                    '  Dim vehiculos As List(Of Vehiculos) = clsVehiculo.ListActivos(cli_id)

                    'DataListVehiculos.DataSource = vehiculos
                    ' DataListVehiculos.DataBind()
                    hdncli_id.Value = cli_id.ToString()

                    'datos para distancias

                    'cargo el combo de marcadores
                    Dim item = New ListItem("- Seleccione Marcador -", "0")
                    ddlMarcaDestino.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                    ddlMarcaDestino.DataBind()

                    ddlMarcaDestino.Items.Insert(0, item)

                    'cargo el combo de marcadores
                    ddlMarcaOrigen.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                    ddlMarcaOrigen.DataBind()
                    ddlMarcaOrigen.Items.Insert(0, item)

                    datagridMarcadores.DataSource = GetMarcadores()
                    datagridMarcadores.DataBind()

                    'busco los vehiculos del usuario
                    Dim _vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)

                    ddlVehiculoOrigen.DataSource = _vehiculos
                    ddlVehiculoOrigen.DataBind()
                    ddlVehiculoOrigen.Items.Insert(0, New ListItem("- Seleccione Vehiculo -", "0"))

                    ddlVehiculoDestino.DataSource = _vehiculos
                    ddlVehiculoDestino.DataBind()
                    ddlVehiculoDestino.Items.Insert(0, New ListItem("- Seleccione Vehiculo -", "0"))

                   
                    'recorridos
                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()
                        'PanelCustomizado.Visible = False
                        PanelMoviles.Visible = False
                        Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(hdnveh_id.Value))
                        lblMovil.Text = "Vehiculo Seleccionado" + _movil.veh_descripcion + "-" + _movil.veh_patente
                    Else
                        'busco los vehiculos del usuario

                        DataListVehiculos.DataSource = _vehiculos
                        DataListVehiculos.DataBind()

                    End If

                End If
            Else
                'no esta logeado
                If hdncli_id.Value <> "" Then
                    Session.Add("Cliente", hdncli_id.Value)
                Else
                    Response.Redirect("~/login.aspx", False)
                End If


            End If


        Catch ex As Exception
            'logeo el error en el visor de eventos          
            Funciones.WriteToEventLog("MAPA - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al administrador. MAPA - " + ex.Message + " - " + ex.StackTrace
        End Try
    End Sub

    'marcadores

    ' Public Sub JQGridMonitoreo_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
    ' Dim filtro As String = e.SearchExpression

    '    JQGridMonitoreo.DataSource = GetMarcadores()
    '   JQGridMonitoreo.DataBind()



    'End Sub

    Private Function GetMarcadores() As DataTable
        Dim _marcadores As List(Of Marcadores) = clsMarcador.List(CInt(hdncli_id.Value))
        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("marc_id")
        dt.Columns.Add("marc_nombre")
        dt.Columns.Add("marc_tipo")
        dt.Columns.Add("marc_direccion")
        dt.Columns.Add("marc_ubicacion")


        For Each marca In _marcadores
            Dim dr As DataRow = dt.NewRow()
            dr(0) = marca.marc_id
            dr(1) = marca.marc_nombre
            dr(2) = marca.Tipos_Marcadores.tipo_marc_nombre
            dr(3) = marca.marc_direccion
            dr(4) = marca.marc_latitud + "','" + marca.marc_longitud
            dt.Rows.Add(dr)
        Next

        Return dt

    End Function



    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            'guardo el marcador asociado al cliente
            Dim latitud As String = latm.Value
            Dim longitud As String = lngm.Value
            Dim sError As String = ""

            'verifico si eligio un punto en el mapa, sino llamo a la api de google para obtener la lat y lng en base a la direccion
            If latitud = "-35" And longitud = "-64" Then
                clsGoogle.getLatLng(latitud, longitud, direccion.Text, sError)

                If sError <> "" Then

                    ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "alert('" + sError + "');", True)
                    Exit Sub
                End If
            End If

            If latitud = "" And longitud = "" Then
                clsGoogle.getLatLng(latitud, longitud, direccion.Text, sError)

                If sError <> "" Then
                    ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "alert('" + sError + "');", True)
                    Exit Sub
                End If
            End If

            Dim marcador As Marcadores
            'verifico que no se duplique el marcador
            'lo busco por direccion
            marcador = clsMarcador.SelectByNombre(CInt(hdncli_id.Value), txtNombreMarcador.Text)

            If marcador IsNot Nothing Then
                'verifico que sea un alta
                If CInt(hdnmarc_id.Value) = 0 Then
                    If marcador.marc_id <> CInt(hdnmarc_id.Value) Then
                        sError = "Ya existe un Marcador cargado en el sistema con el mismo Nombre, veririque."
                        ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "alert('" + sError + "');", True)

                        Exit Sub
                    End If
                End If
                
                ' Else
                'lo busco por la direccion por si no coinciden las latitudes
                ' marcador = clsMarcador.SelectByDirecc(direccion.Text)
                ' If marcador IsNot Nothing Then
                'If marcador.marc_id <> CInt(hdnmarc_id.Value) Then
                'sError = "Ya existe un Marcador cargado en el sistema para la misma ubicación elegida."
                'ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "alert('" + sError + "');", True)

                'Exit Sub
                ' End If
                'End If
            End If

            marcador = New Marcadores()
            'tengo que ver si es una edicion
            If hdnmarc_id.Value <> "0" Then marcador = clsMarcador.SelectById(CInt(hdnmarc_id.Value))

            marcador.cli_id = CInt(hdncli_id.Value)
            marcador.marc_latitud = latitud
            marcador.marc_longitud = longitud
            marcador.marc_nombre = txtNombreMarcador.Text
            marcador.tipo_marc_id = CInt(ddlTipoMarcador.SelectedValue)
            marcador.marc_direccion = direccion.Text

            If hdnmarc_id.Value <> "0" Then
                clsMarcador.Update(marcador)
            Else
                clsMarcador.Insert(marcador)
            End If

            txtNombreMarcador.Text = ""
            direccion.Text = ""
            ddlTipoMarcador.SelectedIndex = -1
            hdnmarc_id.Value = "0"
            'actualizo la grilla
            datagridMarcadores.DataSource = GetMarcadores()
            datagridMarcadores.DataBind()

            'refresco el mapa
            ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "VerMarcas();", True)
            'hago zoom en el que creo
            ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "marcaZoom('" + marcador.marc_latitud + "','" + marcador.marc_longitud + "');", True)


        Catch ex As Exception
            lblError.Text = "Ocurrio un error grabando los datos. Contacte con el Administrador."
            Funciones.WriteToEventLog("MARCADORES - " + ex.Message + " - " + ex.StackTrace)
        End Try


    End Sub

    Protected Sub datagridMarcadores_itemcommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try
            If e.CommandName = "Borrar" Then
                clsMarcador.Delete(e.CommandArgument)
                'actualizo la grilla
                datagridMarcadores.DataSource = GetMarcadores()
                datagridMarcadores.DataBind()
                hdnmarc_id.Value = "0"
                'muestro de nuevo los marcadores
                'zoom en el mapa con este marcador - hago un set center y un zoom
                ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "searchMarcas(map);", True)

            End If

            If e.CommandName = "Editar" Then
                'cargo los datos del marcador
                Dim marca As Marcadores = clsMarcador.SelectById(CInt(e.CommandArgument.ToString))
                hdnmarc_id.Value = marca.marc_id.ToString
                latm.Value = marca.marc_latitud
                lngm.Value = marca.marc_longitud
                txtNombreMarcador.Text = marca.marc_nombre
                ddlTipoMarcador.SelectedValue = marca.tipo_marc_id.ToString
                direccion.Text = marca.marc_direccion
                'zoom en el mapa con este marcador - hago un set center y un zoom
                ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "marcaZoom('" + marca.marc_latitud + "','" + marca.marc_longitud + "');", True)

            End If

            If e.CommandName = "Ver" Then
                'zoom en el mapa con este marcador - hago un set center y un zoom
                Dim marca As Marcadores = clsMarcador.SelectById(CInt(e.CommandArgument.ToString))
                ScriptManager.RegisterStartupScript(Me.UpdatePanel3, GetType(String), "funcionRefresco", "marcaZoom('" + marca.marc_latitud + "','" + marca.marc_longitud + "');", True)
            End If

            If e.CommandName = "Sort" Then
                Dim sortExpression As String = e.CommandArgument

                Dim direction As String = String.Empty

                If SortDirection = SortDirection.Ascending Then

                    SortDirection = SortDirection.Descending
                    direction = " DESC"

                Else

                    SortDirection = SortDirection.Ascending
                    direction = " ASC"

                End If

                Dim table As DataTable = GetMarcadores()

                table.DefaultView.Sort = sortExpression & direction
                datagridMarcadores.DataSource = table
                datagridMarcadores.DataBind()
            End If

        Catch ex As Exception
            Funciones.WriteToEventLog("MAPA - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error modificando los datos. Contacte con el Administrador."
        End Try

    End Sub


    <WebMethod()> _
    Public Shared Function cortarCorriente(ByVal veh_id As String) As String
        'corto la corriente del movil
        'solo si la velocidad actual es menor a 20kms/h
        Dim _respuesta As String = ""

        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(CInt(veh_id))

        If ubicacion IsNot Nothing Then

            Dim _comando As Comando = clsModulo.SelectComandoBySensor(clsCategoriaAlarma.SensorByPosicion(1).sen_id, False)
            Dim _corte As Cortes_Corriente = New Cortes_Corriente()
            _corte.cort_admin = "Cliente"
            _corte.cort_fecha = DateTime.Now
            _corte.cort_hecho = False
            _corte.cort_tipo = _comando.com_comando
            _corte.modulo_num = ubicacion.ID_MODULO
            _corte.veh_id = CInt(veh_id)
            clsVehiculo.CortarCorriente(_corte)
        
        Else
        _respuesta = "El Movil no tiene reporte de Ubicación. No se puede ejecutar la acción."
        End If
        Return _respuesta
    End Function


    <WebMethod()> _
    Public Shared Function restablecerCorriente(ByVal veh_id As String) As String
        Dim _respuesta As String = ""

        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(CInt(veh_id))

        If ubicacion IsNot Nothing Then
            Dim _comando As Comando = clsModulo.SelectComandoBySensor(clsCategoriaAlarma.SensorByPosicion(1).sen_id, True)
            Dim _corte As Cortes_Corriente = New Cortes_Corriente()
            _corte.cort_admin = "Cliente"
            _corte.cort_fecha = DateTime.Now
            _corte.cort_hecho = False
            _corte.cort_tipo = _comando.com_comando
            _corte.veh_id = CInt(veh_id)
            _corte.modulo_num = ubicacion.ID_MODULO
            clsVehiculo.CortarCorriente(_corte)

        Else
            _respuesta = "El Movil no tiene reporte de Ubicación. No se puede ejecutar la acción."
        End If
        Return _respuesta
    End Function

    <WebMethod()> _
    Public Shared Function forzarEnvioTrama(ByVal veh_id As String) As String
        Dim _respuesta As String = ""

        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(CInt(veh_id))

        If ubicacion IsNot Nothing Then

            'busco en comando en base al sen_id
            Dim _comando As Comando = clsModulo.SelectComandoBySigla("ENT")

            Dim _parametro As Parametros = clsParametros.ParametroSelect("codigo_comando")
            Dim _mensaje As Comandos_Enviado = New Comandos_Enviado()
            _mensaje = New Comandos_Enviado()
            _mensaje.com_id = _comando.com_id
            _mensaje.men_fecha = DateTime.Now
            _mensaje.men_mensaje = ubicacion.ID_MODULO & _parametro.par_valor & "ENT"
            _mensaje.men_respuesta = ""
            _mensaje.men_id_terminal = ""
            _mensaje.men_enviado = False
            _mensaje.mod_id = clsModulo.Selecionar(ubicacion.ID_MODULO).mod_id
            clsModulo.InsertComando(_mensaje)

        Else
            _respuesta = "El Movil no tiene reporte de Ubicación. No se puede ejecutar la acción."
        End If
        Return _respuesta
    End Function

    <WebMethod()> _
    Public Shared Function dispararAlarmaCorta(ByVal veh_id As String) As String
        Dim _respuesta As String = ""

        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(CInt(veh_id))

        If ubicacion IsNot Nothing Then

            'busco en comando en base al sen_id
            Dim _comando As Comando = clsModulo.SelectComandoBySigla("BUA")

            Dim _parametro As Parametros = clsParametros.ParametroSelect("codigo_comando")
            Dim _mensaje As Comandos_Enviado = New Comandos_Enviado()
            _mensaje = New Comandos_Enviado()
            _mensaje.com_id = _comando.com_id
            _mensaje.men_fecha = DateTime.Now
            _mensaje.men_mensaje = ubicacion.ID_MODULO & _parametro.par_valor & "BUA"
            _mensaje.men_respuesta = ""
            _mensaje.men_id_terminal = ""
            _mensaje.men_enviado = False
            _mensaje.mod_id = clsModulo.Selecionar(ubicacion.ID_MODULO).mod_id
            clsModulo.InsertComando(_mensaje)

        Else
            _respuesta = "El Movil no tiene reporte de Ubicación. No se puede ejecutar la acción."
        End If
        Return _respuesta
    End Function

    <WebMethod()> _
    Public Shared Function EnviarComando(ByVal veh_id As String, ByVal sen_posicion As Integer, ByVal encender As Boolean) As String
        Dim _respuesta As String = ""

        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(CInt(veh_id))

        If ubicacion IsNot Nothing Then

            'busco en comando en base al sen_id

            Dim _comando As Comando = clsModulo.SelectComandoBySensor(clsCategoriaAlarma.SensorByPosicion(sen_posicion).sen_id, encender)

            If _comando IsNot Nothing Then
                Dim _parametro As Parametros = clsParametros.ParametroSelect("codigo_comando")
                Dim _mensaje As Comandos_Enviado = New Comandos_Enviado()
                _mensaje = New Comandos_Enviado()
                _mensaje.com_id = _comando.com_id
                _mensaje.men_fecha = DateTime.Now
                _mensaje.men_mensaje = ubicacion.ID_MODULO & _parametro.par_valor & _comando.com_comando
                _mensaje.men_respuesta = ""
                _mensaje.men_id_terminal = ""
                _mensaje.men_enviado = False
                _mensaje.mod_id = clsModulo.Selecionar(ubicacion.ID_MODULO).mod_id
                clsModulo.InsertComando(_mensaje)
            Else
                _respuesta = "No se encontro el comando asociado."
            End If


        Else
            _respuesta = "El Movil no tiene reporte de Ubicación. No se puede ejecutar la acción."
        End If
        Return _respuesta
    End Function

    'funcion que llama el jquery para armar el listado de moviles y sus datos de ubicacion, aplica filtro sobre todos los campos
    <WebMethod(EnableSession:=True)> _
    Public Shared Function MostarVehiculos(ByVal cli_id As Integer, ByVal filtro As String, ByVal verGrupo As String) As String
        Dim sTabla As StringBuilder = New StringBuilder()
        sTabla.AppendLine("<ul id='navigation'>")

        Try
            Dim vehiculos As List(Of Vehiculo)
            If System.Web.HttpContext.Current.Session("Cliente") IsNot Nothing Then
                Dim cliente As Integer = CInt(System.Web.HttpContext.Current.Session("Cliente").ToString)
                'System.Web.HttpContext.Current.Session.Add("Cliente", cliente)
            Else
                System.Web.HttpContext.Current.Session.Add("Cliente", cli_id)
            End If


            'tengo que armar la estructura en grupos
            'si no tengo grupos muestro los autos como arbol
            'tener en cuenta los autos que no estan en grupos

            Dim title = "Reportando Normal"

            If verGrupo = "1" Then
                Dim _grupos As List(Of Grupos) = clsGrupo.Search(cli_id)

                If _grupos.Count > 0 Then
                    For Each grupo As Grupos In _grupos

                        sTabla.AppendLine("<li><a onclick='javascript:mostrarGrupos(" & grupo.grup_id.ToString() & ");' href='#g" & grupo.grup_id.ToString() & "' style='text-decoration:none; font-weight:bold; color:Black;'>" & grupo.grup_nombre & "</a>")
                        sTabla.AppendLine("<ul>")

                        vehiculos = clsVehiculo.ListActivosGrupo(grupo.grup_id, filtro)
                        sTabla.AppendLine(listarMoviles(vehiculos))
                        sTabla.AppendLine("</ul>")
                        sTabla.AppendLine("</li>")
                    Next

                    'los moviles que no estan en grupos

                    vehiculos = clsVehiculo.ListNotGrupo(cli_id)
                    sTabla.AppendLine(listarMoviles(vehiculos))
                    sTabla.AppendLine("</ul>")


                Else
                    'armo la lista solo de los moviles
                    sTabla.AppendLine("<ul>")
                    vehiculos = clsVehiculo.ListActivosGrupo(cli_id, filtro)
                    sTabla.AppendLine(listarMoviles(vehiculos))
                    sTabla.AppendLine("</ul>")
                End If
            Else
                'TODOS
                vehiculos = clsVehiculo.ListActivos(cli_id, filtro)
                sTabla.AppendLine(listarMoviles(vehiculos))

                sTabla.AppendLine("</ul>")
            End If


        Catch ex As Exception
            sTabla.AppendLine("<tr><td>Ocurrio un error recuperando los datos. Contacte al Administrador." + "MAPA - " + ex.Message + " - " + ex.StackTrace + "</td></tr>")
            Funciones.WriteToEventLog("MAPA - " + ex.Message + " - " + ex.StackTrace)
        End Try

        Return sTabla.ToString()

    End Function

    Public Shared Function listarMoviles(ByVal Vehiculos As List(Of Vehiculo)) As String
        Dim ubicacion As vMonitoreo = New vMonitoreo()
        Dim sTabla As StringBuilder = New StringBuilder()

        'verifico cuanto hace que no reporta el movil para asignarle luego el color al titulo
        Dim minutos_rojo As Integer = CInt(clsParametros.ParametroSelect("No_reporta_rojo").par_valor)
        Dim minutos_amarillo As Integer = CInt(clsParametros.ParametroSelect("No_reporta_amarillo").par_valor)
        Dim estado As String


        For Each movil As Vehiculo In Vehiculos
            Dim onOff = "panel_on.png"
            Dim ocupado = "<span style='font-weight:bold;color:Red;' >OCUPADO</span>"
            Dim color As String = "#006633"
            Dim _mostrarEncendido As Boolean = False
            Dim _mostarOcupado As Boolean = False
            Dim _mostarRPM As Boolean = False
            Dim _mostrarTemp As Boolean = False
            Dim reporte As String = "Reportando Normalmente"
            Dim _estadoReporte As Integer = 1 'normal
            Dim _estadoConexion As String = ""

            Dim _sensores As List(Of Sensores) = clsCategoriaAlarma.ListSensoresByMovil(movil.veh_id)

            sTabla.AppendLine("<li style='font_family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 10px;color:#  ;'>")
            ubicacion = clsVehiculo.searchLastLocationByVehiculo(movil.veh_id)

            If ubicacion IsNot Nothing Then

                'encendido 4, Ocupacion 7, RPM 13 , TEmp 14
                If _sensores.Where(Function(s) s.sen_posicion = 4).FirstOrDefault() IsNot Nothing Then
                    _mostrarEncendido = True
                End If

                If _sensores.Where(Function(s) s.sen_posicion = 7).FirstOrDefault() IsNot Nothing Then
                    _mostarOcupado = True
                End If

                If _sensores.Where(Function(s) s.sen_posicion = 18).FirstOrDefault() IsNot Nothing Then
                    _mostarRPM = True
                End If

                If _sensores.Where(Function(s) s.sen_posicion = 17).FirstOrDefault() IsNot Nothing Then
                    _mostrarTemp = True
                End If

                'verifico si hay un marcador cerca de la ubicacion
                'marcadores creados por el cliente o marcadores creados por el administrador

                Dim smarcador As String = ""

                'Solo lo muestro si el modulo esta reportando normalmente

                If Not ubicacion.NOMBRE_VIA.Contains("Sin Señal") Then
                    Dim marcador_cliente As Marcadores = New Marcadores()
                    Dim marcador As Marcadores_Generico = New Marcadores_Generico()
                    If ubicacion.LATITUD <> "" And ubicacion.LONGITUD <> "" Then

                        marcador_cliente = clsMarcador.SelectByLatLng(movil.cli_id, ubicacion.LATITUD.Substring(0, 7), ubicacion.LONGITUD.Substring(0, 7))
                        marcador = clsMarcador.SelectGenericoByLatLng(movil.cli_id, ubicacion.LATITUD.Substring(0, 7), ubicacion.LONGITUD.Substring(0, 7))

                    End If

                    If marcador_cliente IsNot Nothing Then smarcador = marcador_cliente.marc_nombre

                    If marcador IsNot Nothing Then smarcador += " " + marcador.marc_nombre
                End If

                estado = "panel_auto_verde.png"
                'si tiene mas de un dia sin reportar lo pongo en rojo
                If (DateTime.Now - ubicacion.FECHA).TotalDays > 1 Then
                    estado = "panel_auto_rojo.png" 'rojo
                    color = "#CC0000"
                    reporte = "Sin Reporte en la última hora"
                    _estadoReporte = 3 'sin reporte

                Else
                    If (DateTime.Now - ubicacion.FECHA).TotalMinutes > minutos_rojo Then
                        estado = "panel_auto_rojo.png" 'rojo
                        color = "#CC0000"
                        reporte = "Sin Reporte en la última hora"
                        _estadoReporte = 3 'sin reporte
                    Else
                        If (DateTime.Now - ubicacion.FECHA).TotalMinutes > minutos_amarillo Then
                            estado = "panel_auto_amarillo.png" 'amarillo
                            color = "#FFCC00"
                            reporte = "Reportando con Lentitud."
                            _estadoReporte = 2 'reporte lento
                        End If
                    End If
                End If

                If ubicacion.ENCENDIDO Then
                    onOff = "panel_on.png"
                Else
                    onOff = "panel_off.png"
                End If

                If _mostarOcupado Then
                    If ubicacion.OCUPADO Then
                        ocupado = "<span style='font-weight:bold;color:Red;' >OCUPADO</span>"
                    Else
                        ocupado = "<span style='font-weight:bold;color:#006633;' >LIBRE</span>"
                    End If
                Else
                    ocupado = ""
                End If


                sTabla.AppendLine("<img alt='auto'  src='../images/iconos_movil/" & estado & "' style='border-width:0px;' title='" & reporte & "'/>")

                If _mostrarEncendido Then sTabla.AppendLine("<img alt='OnOff'  src='../images/iconos_movil/" & onOff & "' style='border-width:0px;' />")
                sTabla.AppendLine("<a onclick='javascript:mostrarVehiculos(" + movil.veh_id.ToString() + ");' href='#v' title='Mostrar Ubicación' style='text-decoration:none; font-weight:bold; color:Black;'>") 'llamo a la funcion que busca la ubicacion pero solo para un movil
                sTabla.AppendLine("<span style='font_family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 11px;'>" + movil.veh_descripcion)
                sTabla.AppendLine(movil.veh_patente + "</span>")
                sTabla.AppendLine("</a>")
                sTabla.AppendLine("<a title='Ver Reporte de Recorrido' href='#' onclick='irReporte(" & movil.veh_id.ToString() & ");'><img alt='Reporte'  src='../images/panel_reporte.png' style='border-width:0px;' /></a>")
                sTabla.AppendLine("<a title='Ver Estadisticas' href='#' onclick='irEstadisticas(" & movil.veh_id.ToString() & ");'><img alt='Reporte'  src='../images/panel_estadistica.png' style='border-width:0px;' /></a>")
                sTabla.AppendLine("<a title='Ver Alarmas' href='#' onclick='irAlarmas(" & movil.veh_id.ToString() & ");'><img alt='Alarmas' src='../images/panel_alarma.png' style='border-width:0px;' /></a>")
                sTabla.AppendLine("<a title='Ver Recorridos en el Mapa' href='#' onclick=' irRecorrido(" & movil.veh_id.ToString() & ");'><img alt='Recorrido' src='../images/panel_recorridos.png' style='border-width:0px;' /></a>")
                '  sTabla.AppendLine("<a title='Hacer Seguimiento' href='#'><img alt='Seguimiento' src='../images/panel_seguimiento.png' style='border-width:0px;' /></a>")
                'verifico si el modulo esta conectado al modulo para mostrar los botones
                'busco en la tabla de conexiones una conexion sin fecha de fin
                'si no tengo conexion no pinto los iconos.

                Dim conexiones As List(Of Modulos_Conexiones) = clsModulo.ListConexionesByModulo(movil.mod_id)

                If conexiones.Count > 0 Then
                    'verifico si esta reportando normalmente, sino lo deshabilito

                    ' tengo que verificar los sensores configurados para el movil y mostrar los iconos
                    'Posiciones

                    'Luces de Baliza: 16
                    Dim _alarmaLuces As Sensores = clsCategoriaAlarma.GetSensorByMovil(movil.veh_id, 16)
                    ' Reloj: 15
                    Dim _alarmaReloj As Sensores = clsCategoriaAlarma.GetSensorByMovil(movil.veh_id, 15)
                    'Los comandos los relaciono con el sensor y una bandera de si el comando es para encendero o no asi se cual ejecutar
                    'Asi puedo pasarle a la funcion que va a enviar el comando el parametro vehiculo y comando a ejecutar
                    ' Disparar Alarma: 12
                    Dim _alarmaLarga As Sensores = clsCategoriaAlarma.GetSensorByMovil(movil.veh_id, 12)
                    'corte de corriente
                    Dim _corteCorriente As Sensores = clsCategoriaAlarma.GetSensorByMovil(movil.veh_id, 1)


                    If _estadoReporte = 3 Then
                        'iconos desactivados no puedo enviar comandos
                        sTabla.AppendLine("<img  id='icocortar_" & movil.veh_id.ToString() & "' alt='Cortar Corriente' src='../images/electric_out.png' />")

                        If _alarmaLarga IsNot Nothing Then sTabla.AppendLine("<img id='icoalarma_" & movil.veh_id.ToString() & "' alt='Disparar Alarma' src='../images/sensores/alarm_out.png' style='border-width:0px;' />")
                        If _alarmaLarga IsNot Nothing Then sTabla.AppendLine("<img id='icoalarmac_" & movil.veh_id.ToString() & "' alt='Disparar Alarma' src='../images/sensores/alarm_s_out.png' style='border-width:0px;' />")

                        If _alarmaLuces IsNot Nothing Then sTabla.AppendLine("<img id='icoBaliza_" & movil.veh_id.ToString() & "' alt='Encender Baliza' src='../images/sensores/light_out.png' style='border-width:0px;' />")
                        If _alarmaReloj IsNot Nothing Then sTabla.AppendLine("<img id='icoReloj_" & movil.veh_id.ToString() & "' alt='Apagar Reloj' src='../images/sensores/clock_out.png' style='border-width:0px;' />")
                    Else


                        If _corteCorriente IsNot Nothing And ubicacion.SENSORES(0) = "1" Then sTabla.AppendLine("<a title='Restablecer Corriente' id='restablecer_" & movil.veh_id.ToString() & "'  href='#' onclick='javascript:restablecerCorriente(" + movil.veh_id.ToString() + ");'><img  id='icocortar_" & movil.veh_id.ToString() & "' alt='Corriente' src='../images/electric_active.png' style='border-width:0px;' /></a>")
                        If _corteCorriente IsNot Nothing And ubicacion.SENSORES(0) = "0" Then sTabla.AppendLine("<a title='Cortar Corriente' id='cortar_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:cortarCorriente(" + movil.veh_id.ToString() + ");'><img id='icocortar_" & movil.veh_id.ToString() & "' alt='Corriente' src='../images/electric_off.png' style='border-width:0px;' /></a>")

                        'alarma larga
                        If _alarmaLarga IsNot Nothing And ubicacion.SENSORES(11) = "0" Then sTabla.AppendLine("<a title='Disparar Alarma' id='alarma_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:enviarComando(" + movil.veh_id.ToString() + ",12,true);'><img id='icoalarma_" & movil.veh_id.ToString() & "' alt='Alarma' src='../images/sensores/alarm_active.png' style='border-width:0px;' /></a>")
                        If _alarmaLarga IsNot Nothing And ubicacion.SENSORES(11) = "1" Then sTabla.AppendLine("<a title='Cortar Alarma' id='alarma_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:enviarComando(" + movil.veh_id.ToString() + ",12,false);'><img id='icoalarma_" & movil.veh_id.ToString() & "' alt='Alarma' src='../images/sensores/alarm_off.png' style='border-width:0px;' /></a>")

                        'alarma corta
                        If _alarmaLarga IsNot Nothing Then sTabla.AppendLine("<a title='Disparar Alarma Corta' id='alarmac_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:alarmaCorta(" + movil.veh_id.ToString() + ");'><img id='icoalarmac_" & movil.veh_id.ToString() & "' alt='Alarma' src='../images/sensores/alarm_s_active.png' style='border-width:0px;' /></a>")

                        If _alarmaLuces IsNot Nothing And ubicacion.SENSORES(15) = "0" Then sTabla.AppendLine("<a title='Encender Baliza' id='baliza_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:enviarComando(" + movil.veh_id.ToString() + ",16,true);'><img id='icoBaliza_" & movil.veh_id.ToString() & "' alt='Alarma' src='../images/sensores/light_active.png' style='border-width:0px;' /></a>")
                        If _alarmaLuces IsNot Nothing And ubicacion.SENSORES(15) = "1" Then sTabla.AppendLine("<a title='Apagar Baliza' id='baliza_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:enviarComando(" + movil.veh_id.ToString() + ",16,false);'><img id='icoBaliza_" & movil.veh_id.ToString() & "' alt='Alarma' src='../images/sensores/light_off.png' style='border-width:0px;' /></a>")

                        If _alarmaReloj IsNot Nothing And ubicacion.SENSORES(14) = "0" Then sTabla.AppendLine("<a title='Apagar Reloj' id='reloj_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:enviarComando(" + movil.veh_id.ToString() + ",15,false);'><img id='icoReloj_" & movil.veh_id.ToString() & "' alt='Alarma' src='../images/sensores/clock_off.png' style='border-width:0px;' /></a>")
                        If _alarmaReloj IsNot Nothing And ubicacion.SENSORES(14) = "1" Then sTabla.AppendLine("<a title='Prender Reloj' id='reloj_" & movil.veh_id.ToString() & "' href='#'  onclick='javascript:enviarComando(" + movil.veh_id.ToString() + ",15,true);'><img id='icoReloj_" & movil.veh_id.ToString() & "' alt='Alarma' src='../images/sensores/clock_active.png' style='border-width:0px;' /></a>")


                    End If

                    'icono para forzar envio de trama
                    sTabla.AppendLine("<a title='Forzar Respuesta' id='aping_" & movil.veh_id.ToString() & "'  href='#' onclick='javascript:hacerPing(" + movil.veh_id.ToString() + ");'><img  id='icoPing_" & movil.veh_id.ToString() & "' alt='Ping' src='../images/sensores/ping_active.png' style='border-width:0px;' /></a>")
                    _estadoConexion = ""
                Else
                    _estadoConexion = "Sin Comunicación Celular"
                End If


                sTabla.AppendLine("<ul id='desc_" + movil.veh_id.ToString() + "'><li><div style='font_family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 11px;color:#555;'>" & ocupado & "-" & movil.veh_nombre_conductor & "</span><br />")

                sTabla.AppendLine("<span style='font_family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 11px;color:" & color & ";font-weight:bold;'>Ultimo Reporte: " + ubicacion.FECHA.ToString("dd/MM/yyyy") + "-" + ubicacion.FECHA.ToString("HH:mm:ss") + "</span><br />")
                sTabla.AppendLine("<span style='font_family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 11px;'>" + String.Format("{0:###,0}", ubicacion.VELOCIDAD) + " Kms/H - ")

                If _mostarRPM Then sTabla.Append("RPM: " & ubicacion.RPM)
                If _mostrarTemp Then sTabla.Append(" - T&#176;: " & ubicacion.TEMP)

                If ubicacion.BATERIA IsNot Nothing Then
                    sTabla.Append(" - BAT: " & ubicacion.BATERIA.Value)
                Else
                    sTabla.Append(" - BAT: Sin Datos")
                End If


                sTabla.Append("</span><br/>")
                sTabla.AppendLine("<span style='font_family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 11px;'>Ubicación: " & ubicacion.NOMBRE_VIA)
                sTabla.AppendLine("<br/>" & ubicacion.LOCALIDAD)
                sTabla.AppendLine(ubicacion.PROVINCIA & "</span>")


                Select Case ubicacion.ORIENTACION
                    Case "1"
                        sTabla.AppendLine(" <span>,N</span>")
                    Case "2"
                        sTabla.AppendLine(" <span>,S</span>")
                    Case "3"
                        sTabla.AppendLine(" <span>,E</span>")
                    Case "4"
                        sTabla.AppendLine(" <span>,O</span>")
                    Case "5"
                        sTabla.AppendLine(" <span>,NE</span>")
                    Case "6"
                        sTabla.AppendLine(" <span>,NO</span>")
                    Case "7"
                        sTabla.AppendLine(" <span>,SE</span>")
                    Case "8"
                        sTabla.AppendLine(" <span>,SO</span>")

                End Select

                sTabla.AppendLine("<span>" & smarcador & "</span></div>")
                sTabla.AppendLine("<div><span style='color:#cc0000;' ><b>" & _estadoConexion & "</b></span></div>")
                sTabla.AppendLine("</li></ul>")


            Else


                sTabla.AppendLine("<img alt='auto'  src='../images/iconos_movil/autito_gris.png' style='border-width:0px;' />")
                sTabla.AppendLine("<span style='font_family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 11px;'>" + movil.veh_descripcion)
                sTabla.AppendLine(movil.veh_patente + "</span>")
                sTabla.AppendLine("</a>")
                sTabla.AppendLine("<a title='Ver Reporte de Recorrido' href='#' onclick=' irReporte(" & movil.veh_id.ToString() & ");'><img alt='Reporte'  src='../images/panel_reporte.png' style='border-width:0px;' /></a>")
                sTabla.AppendLine("<a title='Ver Estadisticas' href='#' onclick= irEstadisticas(" & movil.veh_id.ToString() & ");'><img alt='Reporte'  src='../images/panel_estadistica.png' style='border-width:0px;' /></a>")
                sTabla.AppendLine("<a title='Ver Alarmas' href='#' onclick='irAlarmas(" & movil.veh_id.ToString() & ");'><img alt='Alarmas' src='../images/panel_alarma.png' style='border-width:0px;' /></a>")

                sTabla.AppendLine("<ul><li><span>" & movil.veh_nombre_conductor & "</span><br />")
                sTabla.AppendLine("<span>Sin Datos de Ubicación</span><br/>")
                sTabla.AppendLine("</li></ul>")

            End If


            sTabla.AppendLine("</li>")


        Next

        Return sTabla.ToString()
    End Function

 
    'Public Shared Sub ClearSession(ByVal key As String)
    '    HttpContext.Current.Session(key) = Nothing
    'End Sub

    'Public Shared Function getSession(ByVal key As String) As Object
    '    Return HttpContext.Current.Session(key)
    'End Function

    'Public Shared Sub setSession(ByVal key As String, ByVal valor As Object)
    '    HttpContext.Current.Session.Add(key, valor)
    'End Sub

    Protected Sub ddlMarcaOrigen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarcaOrigen.SelectedIndexChanged
        'pongo la direccion del marcador en el text Origen
        Dim _marcador As Marcadores = clsMarcador.SelectById(CInt(ddlMarcaOrigen.SelectedValue))
        txtDesde.Text = _marcador.marc_direccion

    End Sub

    Protected Sub ddlMarcaDestino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarcaDestino.SelectedIndexChanged
        'pongo la direccion del marcador en el text Destino
        Dim _marcador As Marcadores = clsMarcador.SelectById(CInt(ddlMarcaDestino.SelectedValue))
        txtHasta.Text = _marcador.marc_direccion
    End Sub

    Protected Sub ddlVehiculoOrigen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVehiculoOrigen.SelectedIndexChanged
        'pongo la direccion del marcador en el text Origen
        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(CInt(ddlVehiculoOrigen.SelectedValue))
        If ubicacion IsNot Nothing Then
            txtDesde.Text = ubicacion.NOMBRE_VIA + " " + ubicacion.LOCALIDAD + " " + ubicacion.PROVINCIA
        Else

            txtDesde.Text = "Movil sin reportes de ubicación."
        End If

    End Sub

    Protected Sub ddlVehiculoDestino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVehiculoDestino.SelectedIndexChanged
        'pongo la direccion del marcador en el text destino
        Dim ubicacion As vMonitoreo = clsVehiculo.searchLastLocationByVehiculo(CInt(ddlVehiculoDestino.SelectedValue))
        If ubicacion IsNot Nothing Then
            txtHasta.Text = ubicacion.NOMBRE_VIA + " " + ubicacion.LOCALIDAD + " " + ubicacion.PROVINCIA
        Else
            txtHasta.Text = "Movil sin reportes de ubicación."
        End If
    End Sub

    Protected Sub datagridMarcadores_pagechange(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        datagridMarcadores.CurrentPageIndex = e.NewPageIndex
        datagridMarcadores.DataSource = GetMarcadores()
        datagridMarcadores.DataBind()
    End Sub

    Protected Sub gridMarcadores_SortRecords(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        Dim sortExpression As String = e.SortExpression

        Dim direction As String = String.Empty

        If SortDirection = SortDirection.Ascending Then

            SortDirection = SortDirection.Descending
            direction = " DESC"

        Else

            SortDirection = SortDirection.Ascending
            direction = " ASC"

        End If

        Dim table As DataTable = GetMarcadores()

        table.DefaultView.Sort = sortExpression & direction
        datagridMarcadores.DataSource = table
        datagridMarcadores.DataBind()

    End Sub



    Public Property SortDirection() As SortDirection

        Get

            If ViewState("SortDirection") Is Nothing Then

                ViewState("SortDirection") = SortDirection.Ascending

            End If

            Return DirectCast(ViewState("SortDirection"), SortDirection)

        End Get

        Set(ByVal value As SortDirection)

            ViewState("SortDirection") = value

        End Set

    End Property
End Class

'funciones de jquery para filtrar tablas
'http://beosman.org/archivo/2012/informatica/funcion-en-javascript-para-filtrar-filas-de-una-tabla.html
'http://www.lawebera.es/como-hacer/ejemplos-jquery/uso-jquery-presentar-filtrar-datos-tablas-parte-i.php
'con java scriopt
'http://www.forosdelweb.com/f13/filtrar-contenido-tabla-305526/
