'Pagina para configurar las alarmas de desvio de recorridos que quiere recibir el cliente.
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Xml
Imports System.IO

Partial Public Class pAlarmasRecorrido
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim masOpciones As Boolean = False
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                   
                    PanelGrupo.Visible = False

                    hdncli_id.Value = Session("Cliente").ToString()
                    ddlRecorrido.DataSource = clsRecorrido.ListByCliente(CInt(hdncli_id.Value))
                    ddlRecorrido.DataBind()
                    Dim item As ListItem = New ListItem("- Seleccione Recorrido -", "0")
                    ddlRecorrido.Items.Insert(0, item)
                    'verifico si esta editando la alerta, busco los datos ya guardados

                    If Request.Params("rec_id") IsNot Nothing Then
                        If Request.Params("rec_id").ToString().Contains("|") Then
                            Dim valores As String() = Request.Params("rec_id").ToString().Split("|")
                            hdnveh_id.Value = valores(1)
                            hdnarec_id.Value = valores(0)

                            Dim recorrido As Alertas_Recorridos = clsAlarma.AlertaRecorridoSelect(CInt(hdnarec_id.Value))
                            lblMovil.Text = "Vehículo Patente: " & recorrido.Vehiculo.veh_patente
                            chkMail.Checked = recorrido.arec_enviar_sms
                            txtKms.Text = recorrido.arec_umbral_desvio
                            rdnActiva.SelectedValue = recorrido.arec_activa.ToString()
                            hdnrec_id.Value = recorrido.rec_id.ToString()
                            chkMail.Checked = recorrido.arec_enviar_mail

                            If (recorrido.arec_no_deseado) Then
                                rdnTipoAlarma.SelectedValue = "1"
                            Else
                                rdnTipoAlarma.SelectedValue = "0"
                            End If


                            If recorrido.arec_fecha_desde IsNot Nothing Then
                                txtFechaDesde.Text = recorrido.arec_fecha_desde.Value.ToString("dd/MM/yyyy")
                                ddlHoraDesde.SelectedValue = recorrido.arec_fecha_desde.Value.Hour.ToString().PadLeft(2, "0")
                                ddlMinDesde.SelectedValue = recorrido.arec_fecha_desde.Value.Minute.ToString().PadLeft(2, "0")

                            End If

                            If recorrido.arec_fecha_desde IsNot Nothing Then
                                txtFechaHasta.Text = recorrido.arec_fecha_hasta.Value.ToString("dd/MM/yyyy")
                                ddlHorahasta.SelectedValue = recorrido.arec_fecha_hasta.Value.Hour.ToString().PadLeft(2, "0")
                                ddlMinHasta.SelectedValue = recorrido.arec_fecha_hasta.Value.Minute.ToString().PadLeft(2, "0")

                            End If

                            'busco puntos obligatorios
                            MostrarPuntosObligatorios(recorrido)

                            'PanelTildar.Visible = False
                            'Frecuencia

                            For Each frecuencia As Alertas_Recorridos_Frecuencias In recorrido.Alertas_Recorridos_Frecuencias
                                'dias 
                                If frecuencia.rec_dia_semana = 1 Then 'lunes
                                    chkLunes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeL.SelectedValue = frecuencia.rec_hora_desde.Value.Hours
                                    ddlMinDesdeL.SelectedValue = frecuencia.rec_hora_desde.Value.Minutes

                                    ddlhoraHastaL.SelectedValue = frecuencia.rec_hora_hasta.Value.Hours
                                    ddlMinHastaL.SelectedValue = frecuencia.rec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.rec_dia_semana = 2 Then
                                    chkMartes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeM.SelectedValue = frecuencia.rec_hora_desde.Value.Hours
                                    ddlminDesdeM.SelectedValue = frecuencia.rec_hora_desde.Value.Minutes

                                    ddlhoraHastaM.SelectedValue = frecuencia.rec_hora_hasta.Value.Hours
                                    ddlminHastaM.SelectedValue = frecuencia.rec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.rec_dia_semana = 3 Then
                                    chkMiercoles.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeMi.SelectedValue = frecuencia.rec_hora_desde.Value.Hours
                                    ddlminDesdeMi.SelectedValue = frecuencia.rec_hora_desde.Value.Minutes

                                    ddlhoraHastaMi.SelectedValue = frecuencia.rec_hora_hasta.Value.Hours
                                    ddlminHastaMi.SelectedValue = frecuencia.rec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.rec_dia_semana = 4 Then
                                    chkJueves.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeJ.SelectedValue = frecuencia.rec_hora_desde.Value.Hours
                                    ddlminDesdeJ.SelectedValue = frecuencia.rec_hora_desde.Value.Minutes

                                    ddlhoraHastaJ.SelectedValue = frecuencia.rec_hora_hasta.Value.Hours
                                    ddlminHastaJ.SelectedValue = frecuencia.rec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.rec_dia_semana = 5 Then
                                    chkViernes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeV.SelectedValue = frecuencia.rec_hora_desde.Value.Hours
                                    ddlminDesdeV.SelectedValue = frecuencia.rec_hora_desde.Value.Minutes

                                    ddlhoraHastaV.SelectedValue = frecuencia.rec_hora_hasta.Value.Hours
                                    ddlminHastaV.SelectedValue = frecuencia.rec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.rec_dia_semana = 6 Then
                                    chkSabado.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeS.SelectedValue = frecuencia.rec_hora_desde.Value.Hours
                                    ddlminDesdeS.SelectedValue = frecuencia.rec_hora_desde.Value.Minutes

                                    ddlhoraHastaS.SelectedValue = frecuencia.rec_hora_hasta.Value.Hours
                                    ddlminHastaS.SelectedValue = frecuencia.rec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.rec_dia_semana = 0 Then
                                    chkDomingo.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeD.SelectedValue = frecuencia.rec_hora_desde.Value.Hours
                                    ddlminDesdeD.SelectedValue = frecuencia.rec_hora_desde.Value.Minutes

                                    ddlhoraHastaD.SelectedValue = frecuencia.rec_hora_hasta.Value.Hours
                                    ddlminHastaD.SelectedValue = frecuencia.rec_hora_hasta.Value.Minutes

                                End If
                            Next

                            PanelMoviles.Visible = False
                        Else
                            hdnveh_id.Value = Request.Params("rec_id").ToString()
                            hdnarec_id.Value = "0"
                        End If
                       

                       

                    Else
                        'verifico si tiene grupos y cargo el combo
                        Dim _grupos As List(Of Grupos) = clsGrupo.Search(CInt(hdncli_id.Value))

                        If _grupos.Count > 0 Then

                            ddlgrupo.DataSource = _grupos
                            ddlgrupo.DataBind()

                            ddlgrupo.Items.Insert(0, New ListItem("--Seleccione--", 0))

                            PanelGrupo.Visible = True

                        End If
                        PanelMoviles.Visible = True
                        cargarMoviles(0)


                    End If

                    'si es edicion marco la zona y la pinto
                    ddlRecorrido.SelectedValue = hdnrec_id.Value
                    ddlRecorrido_SelectedIndexChanged(sender, e)

                    'si esta dando de alta un movil pre seleccionado lo marco

                    cargarMoviles(hdnveh_id.Value)
                    If masOpciones Then

                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " mostrar();", True)
                    End If


                End If

            Else
                'no esta logeado
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "redirigir", " parent.iraLogin();", True)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS RECORRIDOS - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos de la Alerta.Contacte al administrador. " + ex.ToString()
        End Try

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try

            'guardo esta definicion de recorrido
            Dim dirOrigen As String = txtDireccionOrigen.Text
            Dim dirDestino As String = txtDireccionDestino.Text
            Dim sError As String = ""
            Dim _recorrido As Recorridos = New Recorridos()
            Dim puntos As String()
            Dim coordena As String()
            Dim selecciono As Boolean = False
            'verifico si eligio un movil
            For Each row As DataListItem In DataListVehiculos.Items 'asocio los moviles que selecciono
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("chkMoviles"), CheckBox)
                If rdnMovil.Checked Then
                    selecciono = True
                    Exit For
                End If
            Next

            If selecciono = False Then
                lblError.Text = "Debe seleccionar al menos un Móvil."
                Exit Sub
            End If

            'si eligio puntos obligatirios tengo que cargarle hora
            selecciono = True
            For Each item As DataListItem In DataListPuntos.Items

                Dim seleccionado As CheckBox = DirectCast(item.FindControl("chkDireccion"), CheckBox)
                If seleccionado.Checked Then
                    If DirectCast(item.FindControl("ddlHoraDesde"), DropDownList).SelectedValue = "" And (DirectCast(item.FindControl("ddlMinDesde"), DropDownList).SelectedValue) = "" And DirectCast(item.FindControl("ddlHoraHasta"), DropDownList).SelectedValue = "" And (DirectCast(item.FindControl("ddlMinhasta"), DropDownList).SelectedValue) = "" Then
                        selecciono = False
                        Exit For
                    End If
               
                End If
            Next

            If selecciono = False Then
                lblErrorPuntos.Text = "Seleccione Hora y Minutos en que se debe visitar el Punto Obligatirio."
                ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "mostrarTab", " $(function () {$('#tabs').tabs('select', document.getElementById('tabs-4').value);});", True)
                Exit Sub
            End If

            'busco las direcciones para el caso de que marco los puntos en el mapa
            If hdnRuta.Value <> "" Then
                'grabo el recorrido
                'verifico si es edicion
                If hdnrec_id.Value <> "0" Then

                    puntos = hdnRuta.Value.Split("|")
                    dirDestino = txtDireccionDestino.Text
                    dirOrigen = txtDireccionOrigen.Text

                    If txtDireccionDestino.Text = "" Then
                        'primer punto
                        coordena = puntos(1).Split(",")

                        If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                            clsGoogle.getdireccion(coordena(0).ToString(), coordena(1).ToString(), dirDestino, sError)
                        End If
                    End If

                    If txtDireccionOrigen.Text = "" Then
                        'ultimo punto
                        coordena = puntos(puntos.Length - 1).Split(",")

                        If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                            clsGoogle.getdireccion(coordena(0).ToString(), coordena(1).ToString(), dirOrigen, sError)
                        End If
                    End If

                    'verifico que no tenga otro recorrido con las mismas direcciones
                    Dim recor As Recorridos = clsRecorrido.SelectByDireccion(dirOrigen.ToLower, dirDestino.ToLower(), CInt(hdnrec_id.Value))

                    If recor Is Nothing Then
                        _recorrido = clsRecorrido.SelectById(hdnrec_id.Value)

                        'borro los puntos y la vuelvo a grabar
                        'puntos obligatorios
                        If (_recorrido.Alertas_Recorridos.Count > 0) Then clsAlarma.DeleteAlertas_Recorrido_Puntos(_recorrido.Alertas_Recorridos.FirstOrDefault())

                        clsRecorrido.DeleteRecorrido_Puntos(_recorrido)

                        _recorrido.rec_nombre = txtNombre.Text
                        _recorrido.cli_id = CInt(hdncli_id.Value)
                        _recorrido.rec_destino = dirOrigen
                        _recorrido.rec_origen = dirDestino
                        clsRecorrido.UpdateRecorrido(_recorrido)

                        'recupero los puntos que eligio para marcar la ruta
                        puntos = hdnRuta.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            coordena = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Recorridos_Puntos()

                                punto.rec_id = _recorrido.rec_id
                                punto.rec_latitud = coordena(0).ToString()
                                punto.rec_longitud = coordena(1).ToString()

                                clsRecorrido.InsertPunto(punto)
                            End If

                        Next
                    Else
                        lblError.Text = "Ya existe un Recorrido con el mismo Origen y Destino cargado en el sistema. Verifique."
                        Exit Sub
                    End If

                Else
                    puntos = hdnRuta.Value.Split("|")
                    dirDestino = txtDireccionDestino.Text
                    dirOrigen = txtDireccionOrigen.Text

                    If txtDireccionDestino.Text = "" Then
                        'primer punto
                        coordena = puntos(1).Split(",")

                        If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                            clsGoogle.getdireccion(coordena(0).ToString(), coordena(1).ToString(), dirOrigen, sError)
                        End If
                    End If

                    If txtDireccionOrigen.Text = "" Then
                        'ultimo punto
                        coordena = puntos(puntos.Length - 1).Split(",")

                        If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                            clsGoogle.getdireccion(coordena(0).ToString(), coordena(1).ToString(), dirDestino, sError)
                        End If
                    End If

                    Dim recor As Recorridos = clsRecorrido.SelectByDireccion(dirOrigen.ToLower, dirDestino.ToLower(), 0)
                    If recor Is Nothing Then

                        _recorrido = New Recorridos()
                        'recorrido nuevo
                        _recorrido.rec_nombre = txtNombre.Text
                        _recorrido.rec_destino = dirOrigen
                        _recorrido.rec_origen = dirDestino
                        _recorrido.cli_id = CInt(hdncli_id.Value)

                        clsRecorrido.Insert(_recorrido)
                        hdnrec_id.Value = _recorrido.rec_id.ToString()
                        'recupero los puntos que eligio para marcar la ruta
                        puntos = hdnRuta.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            coordena = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Recorridos_Puntos()

                                punto.rec_id = _recorrido.rec_id
                                punto.rec_latitud = coordena(0).ToString()
                                punto.rec_longitud = coordena(1).ToString()

                                clsRecorrido.InsertPunto(punto)
                            End If

                        Next
                    Else
                        lblError.Text = "Ya existe un Recorrido con el mismo Origen y Destino cargado en el sistema. Verifique."
                        Exit Sub
                    End If
                End If

            Else
                'si tengo un punto de origen y otro de destino tengo que ver como obetener los puntos intermedios, o como obtengo al ruta que tengo q ue controlar despues
                If txtDireccionOrigen.Text <> "" And txtDireccionDestino.Text <> "" Then


                    If hdnrec_id.Value = "0" Then
                        'nuevo recorrido
                        _recorrido = New Recorridos()

                        _recorrido.rec_nombre = txtNombre.Text
                        _recorrido.cli_id = CInt(hdncli_id.Value)
                        _recorrido.rec_origen = txtDireccionOrigen.Text
                        _recorrido.rec_destino = txtDireccionDestino.Text

                        clsRecorrido.Insert(_recorrido)
                        hdnrec_id.Value = _recorrido.rec_id.ToString()

                    Else
                        'si existe elimino los puntos y los creo de nuevo por si modifico algo
                        _recorrido = clsRecorrido.SelectById(hdnrec_id.Value)

                        'borro los puntos y la vuelvo a grabar

                        _recorrido.rec_nombre = txtNombre.Text
                        _recorrido.cli_id = CInt(hdncli_id.Value)
                        _recorrido.rec_origen = txtDireccionOrigen.Text
                        _recorrido.rec_destino = txtDireccionDestino.Text
                        clsRecorrido.UpdateRecorrido(_recorrido)
                        clsRecorrido.DeleteRecorrido_Puntos(_recorrido)

                    End If

                    'vuelvo a crear los puntos
                    If Not getRoute(txtDireccionOrigen.Text.Replace("'", " "), txtDireccionDestino.Text.Replace("'", " "), _recorrido.rec_id) Then
                        lblError.Text = "Las Direcciónes ingresadas no se encontraron en el sistema de Geoposicionamiento. Ingrese Calle, Localidad,Provincia."
                        Exit Sub
                    End If

                Else
                    lblError.Text = "Debe Ingresar la Dirección de Origen y Destino. O marcar el recorrido en el mapa."
                    Exit Sub
                End If
            End If

            If sError <> "" Then
                lblError.Text = sError
                Exit Sub
            End If

            Dim arecorrido = New Alertas_Recorridos()
               'verifico si es edicion
            If hdnarec_id.Value <> "0" Then

                arecorrido = clsAlarma.AlertaRecorridoSelect(CInt(hdnarec_id.Value))

                arecorrido.rec_id = CInt(hdnrec_id.Value)
                arecorrido.arec_enviar_mail = chkMail.Checked
                arecorrido.arec_enviar_sms = False
                If txtKms.Text <> "" Then
                    arecorrido.arec_umbral_desvio = txtKms.Text
                Else
                    arecorrido.arec_umbral_desvio = "0"
                End If

                arecorrido.arec_no_deseado = CBool(rdnTipoAlarma.SelectedValue)
                If txtFechaDesde.Text <> "" Then
                    arecorrido.arec_fecha_desde = CDate(txtFechaDesde.Text + " " + ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + ":00")
                Else
                    arecorrido.arec_fecha_desde = Nothing
                End If

                If txtFechaHasta.Text <> "" Then
                    arecorrido.arec_fecha_hasta = CDate(txtFechaHasta.Text + " " + ddlHorahasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + ":00")
                Else
                    arecorrido.arec_fecha_hasta = Nothing
                End If

                clsAlarma.UpdateAlertaRecorrido(arecorrido)


                'puntos obligatorios
                clsAlarma.DeleteAlertas_Recorrido_Puntos(arecorrido)

                'verifico puntos obligatorios de visitar
                'busco el id del punto en el recorrido que genere, lo busco por latitud y longitud

                For Each item As DataListItem In DataListPuntos.Items

                    Dim seleccionado As CheckBox = DirectCast(item.FindControl("chkDireccion"), CheckBox)
                    If seleccionado.Checked Then
                        Dim _direccion As String = DirectCast(item.FindControl("lblDireccion"), Label).Text
                        Dim _lat As String = DirectCast(item.FindControl("lblLat"), Label).Text
                        Dim _lng As String = DirectCast(item.FindControl("lblLng"), Label).Text
                        Dim _punto As New Alertas_Recorrido_Puntos_Visitar
                        _recorrido = clsRecorrido.SelectById(arecorrido.rec_id)
                        _punto.arec_id = arecorrido.arec_id
                        _punto.rec_punto_fecha_llegada = DirectCast(item.FindControl("txtFecha"), TextBox).Text
                        _punto.rec_punto_horario_desde = DirectCast(item.FindControl("ddlHoraDesde"), DropDownList).SelectedValue & ":" & DirectCast(item.FindControl("ddlMinDesde"), DropDownList).SelectedValue
                        _punto.rec_punto_horario_hasta = DirectCast(item.FindControl("ddlHoraHasta"), DropDownList).SelectedValue & ":" & DirectCast(item.FindControl("ddlMinHasta"), DropDownList).SelectedValue
                        _punto.rec_punto_id = _recorrido.Recorridos_Puntos.Where(Function(r) r.rec_latitud = _lat And r.rec_longitud = _lng).FirstOrDefault().rec_punto_id
                        _punto.rec_punto_direccion = _direccion
                        clsAlarma.InsertAlertaRecorridoPunto(_punto)
                    End If
                Next


                'borro la configuracion de frecuencia previa y la vuelvo a grabar, tambien borro los puntos
                clsAlarma.DeleteAlertas_Recorrido_Frecuencia(arecorrido)
                
            Else

                'nueva alerta
                arecorrido = New Alertas_Recorridos()


                'asocio los moviles que selecciono
                'Ver si eta editando alarma desde un movil pre elegido 

                For Each row As DataListItem In DataListVehiculos.Items 'asocio los moviles que selecciono
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("chkMoviles"), CheckBox)
                    If rdnMovil.Checked Then
                        arecorrido = New Alertas_Recorridos()
                        arecorrido.rec_id = CInt(hdnrec_id.Value)
                        arecorrido.arec_enviar_mail = chkMail.Checked
                        arecorrido.arec_enviar_sms = False

                        If txtKms.Text <> "" Then
                            arecorrido.arec_umbral_desvio = txtKms.Text
                        Else
                            arecorrido.arec_umbral_desvio = "0"
                        End If

                        arecorrido.arec_activa = CBool(rdnActiva.SelectedValue)
                        arecorrido.arec_no_deseado = CBool(rdnTipoAlarma.SelectedValue)

                        If txtFechaDesde.Text <> "" Then
                            arecorrido.arec_fecha_desde = CDate(txtFechaDesde.Text + " " + ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + ":00")
                        Else
                            arecorrido.arec_fecha_desde = Nothing
                        End If

                        If txtFechaHasta.Text <> "" Then
                            arecorrido.arec_fecha_hasta = CDate(txtFechaHasta.Text + " " + ddlHorahasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + ":00")
                        Else
                            arecorrido.arec_fecha_hasta = Nothing
                        End If

                        arecorrido.veh_id = CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString())
                        clsAlarma.InsertAlertaRecorrido(arecorrido)

                        'verifico puntos obligatorios de visitar
                        'busco el id del punto en el recorrido que genere, lo busco por latitud y longitud

                        For Each item As DataListItem In DataListPuntos.Items

                            Dim seleccionado As CheckBox = DirectCast(item.FindControl("chkDireccion"), CheckBox)
                            If seleccionado.Checked Then
                                _recorrido = clsRecorrido.SelectById(arecorrido.rec_id)
                                Dim _direccion As String = DirectCast(item.FindControl("lblDireccion"), Label).Text
                                Dim _lat As String = DirectCast(item.FindControl("lblLat"), Label).Text
                                Dim _lng As String = DirectCast(item.FindControl("lblLng"), Label).Text
                                Dim _punto As New Alertas_Recorrido_Puntos_Visitar

                                _punto.arec_id = arecorrido.arec_id
                                _punto.rec_punto_fecha_llegada = DirectCast(item.FindControl("txtFecha"), TextBox).Text
                                _punto.rec_punto_horario_desde = DirectCast(item.FindControl("ddlHoraDesde"), DropDownList).SelectedValue & ":" & DirectCast(item.FindControl("ddlMinDesde"), DropDownList).SelectedValue
                                _punto.rec_punto_horario_hasta = DirectCast(item.FindControl("ddlHoraHasta"), DropDownList).SelectedValue & ":" & DirectCast(item.FindControl("ddlMinHasta"), DropDownList).SelectedValue
                                _punto.rec_punto_id = _recorrido.Recorridos_Puntos.Where(Function(r) r.rec_latitud = _lat And r.rec_longitud = _lng).FirstOrDefault().rec_punto_id
                                _punto.rec_punto_direccion = _direccion
                                clsAlarma.InsertAlertaRecorridoPunto(_punto)
                            End If
                        Next
                      
                    End If
                Next

            End If



            'guardo la frecuencia segun lo que eligio
           
            If chkLunes.Checked Then
                Dim frecuencia As Alertas_Recorridos_Frecuencias = New Alertas_Recorridos_Frecuencias()
                frecuencia.rec_id = arecorrido.arec_id

                frecuencia.rec_dia_semana = 1
                frecuencia.rec_hora_desde = TimeSpan.Parse(ddlhoraDesdeL.SelectedValue + ":" + ddlMinDesdeL.SelectedValue)
                frecuencia.rec_hora_hasta = TimeSpan.Parse(ddlhoraHastaL.SelectedValue + ":" + ddlMinHastaL.SelectedValue)
                clsAlarma.InsertAlertaRecorridoFrecuencia(frecuencia)
            End If

            If chkMartes.Checked Then
                Dim frecuencia As Alertas_Recorridos_Frecuencias = New Alertas_Recorridos_Frecuencias()
                frecuencia.rec_id = arecorrido.arec_id

                frecuencia.rec_dia_semana = 2
                frecuencia.rec_hora_desde = TimeSpan.Parse(ddlhoraDesdeM.SelectedValue + ":" + ddlminDesdeM.SelectedValue)
                frecuencia.rec_hora_hasta = TimeSpan.Parse(ddlhoraHastaM.SelectedValue + ":" + ddlminHastaM.SelectedValue)
                clsAlarma.InsertAlertaRecorridoFrecuencia(frecuencia)
            End If

            If chkMiercoles.Checked Then
                Dim frecuencia As Alertas_Recorridos_Frecuencias = New Alertas_Recorridos_Frecuencias()
                frecuencia.rec_id = arecorrido.arec_id

                frecuencia.rec_dia_semana = 3
                frecuencia.rec_hora_desde = TimeSpan.Parse(ddlhoraDesdeMi.SelectedValue + ":" + ddlminDesdeMi.SelectedValue)
                frecuencia.rec_hora_hasta = TimeSpan.Parse(ddlhoraHastaMi.SelectedValue + ":" + ddlminHastaMi.SelectedValue)
                clsAlarma.InsertAlertaRecorridoFrecuencia(frecuencia)

            End If

            If chkJueves.Checked Then
                Dim frecuencia As Alertas_Recorridos_Frecuencias = New Alertas_Recorridos_Frecuencias()
                frecuencia.rec_id = arecorrido.arec_id

                frecuencia.rec_dia_semana = 4
                frecuencia.rec_hora_desde = TimeSpan.Parse(ddlhoraDesdeJ.SelectedValue + ":" + ddlminDesdeJ.SelectedValue)
                frecuencia.rec_hora_hasta = TimeSpan.Parse(ddlhoraHastaJ.SelectedValue + ":" + ddlminHastaJ.SelectedValue)

                clsAlarma.InsertAlertaRecorridoFrecuencia(frecuencia)

            End If

            If chkViernes.Checked Then
                Dim frecuencia As Alertas_Recorridos_Frecuencias = New Alertas_Recorridos_Frecuencias()
                frecuencia.rec_id = arecorrido.arec_id

                frecuencia.rec_dia_semana = 5
                frecuencia.rec_hora_desde = TimeSpan.Parse(ddlhoraDesdeV.SelectedValue + ":" + ddlminDesdeV.SelectedValue)
                frecuencia.rec_hora_hasta = TimeSpan.Parse(ddlhoraHastaV.SelectedValue + ":" + ddlminHastaV.SelectedValue)
                clsAlarma.InsertAlertaRecorridoFrecuencia(frecuencia)
            End If

            If chkSabado.Checked Then
                Dim frecuencia As Alertas_Recorridos_Frecuencias = New Alertas_Recorridos_Frecuencias()
                frecuencia.rec_id = arecorrido.arec_id

                frecuencia.rec_dia_semana = 6
                frecuencia.rec_hora_desde = TimeSpan.Parse(ddlhoraDesdeS.SelectedValue + ":" + ddlminDesdeS.SelectedValue)
                frecuencia.rec_hora_hasta = TimeSpan.Parse(ddlhoraHastaS.SelectedValue + ":" + ddlminHastaS.SelectedValue)
                clsAlarma.InsertAlertaRecorridoFrecuencia(frecuencia)
            End If

            If chkDomingo.Checked Then
                Dim frecuencia As Alertas_Recorridos_Frecuencias = New Alertas_Recorridos_Frecuencias()
                frecuencia.rec_id = arecorrido.arec_id

                frecuencia.rec_dia_semana = 0
                frecuencia.rec_hora_desde = TimeSpan.Parse(ddlhoraDesdeD.SelectedValue + ":" + ddlminDesdeD.SelectedValue)
                frecuencia.rec_hora_hasta = TimeSpan.Parse(ddlhoraHastaD.SelectedValue + ":" + ddlminHastaD.SelectedValue)
                clsAlarma.InsertAlertaRecorridoFrecuencia(frecuencia)
            End If

            'retorno al listado
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)

        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS RECORRIDO - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Grabando los datos de la Alerta.Contacte al administrador."
        End Try

    End Sub

    Private Function getRoutePuntos(ByVal origen As String, ByVal destino As String) As List(Of Punto)

        Dim request As WebRequest = WebRequest.Create(" http://maps.googleapis.com/maps/api/directions/xml?origin=" + origen + "&destination=" + destino + "&sensor=false")
        request.Method = "GET"
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
        Dim result1 As String = sr.ReadToEnd()
        Dim strError = ""
        'cargo el resultado como xml y busco las coordenadas

        Dim _puntos = New List(Of Punto)
        Dim xmlRespuesta As XmlDocument = New XmlDocument()
        xmlRespuesta.LoadXml(result1)

        If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then
            'recupero la ruta
            'tengo que leer los nodos steps y sacar los puntos

            For Each nodo As XmlNode In xmlRespuesta.SelectNodes("//leg/step")
              Dim _direccion As New Punto

                _direccion.idPunto = 0
                _direccion.lat = nodo.SelectSingleNode("start_location/lat").InnerText
                _direccion.lng = nodo.SelectSingleNode("start_location/lng").InnerText
                _direccion.direccion = clsGoogle.getdireccion(_direccion.lat, _direccion.lng, strError).dir_nombre_via
                _direccion.Fecha = ""
                _direccion.HoraDesde = ""
                _direccion.MinDesde = ""
                _direccion.HoraHasta = ""
                _direccion.MinHasta = ""
                _puntos.Add(_direccion)

                If strError <> "" Then
                    Funciones.WriteToEventLog(" GET PUNTOS OBLIGATORIOS -" + strError)
                End If
            Next

            Return _puntos
      
        End If

    End Function

    Private Function getRoute(ByVal origen As String, ByVal destino As String, ByVal rec_id As Integer) As Boolean

        Dim request As WebRequest = WebRequest.Create(" http://maps.googleapis.com/maps/api/directions/xml?origin=" + origen + "&destination=" + destino + "&sensor=false")
        request.Method = "GET"
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
        Dim result1 As String = sr.ReadToEnd()
        'cargo el resultado como xml y busco las coordenadas

        Dim xmlRespuesta As XmlDocument = New XmlDocument()
        xmlRespuesta.LoadXml(result1)

        If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then
            'recupero la ruta
            'tengo que leer los nodos steps y sacar los puntos

            For Each nodo As XmlNode In xmlRespuesta.SelectNodes("//leg/step")
                Dim punto = New Recorridos_Puntos()
                punto.rec_id = rec_id
                punto.rec_latitud = nodo.SelectSingleNode("start_location/lat").InnerText
                punto.rec_longitud = nodo.SelectSingleNode("start_location/lng").InnerText
                clsRecorrido.InsertPunto(punto)

              
            Next

            Return True
        Else
            Return False
            Exit Function
        End If

    End Function

    Protected Sub LinkTildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkTildar.Click
        'marco todos los check de moviles

        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = True
        Next

        If txtDireccionOrigen.Text = "" Then
            txtDireccionOrigen.Text = hdnDir1.Value
            txtDireccionDestino.Text = hdnDir2.Value
        End If
       
    End Sub

    Protected Sub LinkDestildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = False
        Next

        If txtDireccionOrigen.Text = "" Then
            txtDireccionOrigen.Text = hdnDir1.Value
            txtDireccionDestino.Text = hdnDir2.Value
        End If
    End Sub

    Private Sub cargarMoviles(ByVal veh_id As String)
        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(CInt(hdncli_id.Value))

        DataListVehiculos.DataSource = vehiculos
        DataListVehiculos.DataBind()

        'marco el ckeck con el movil y lo desabilito
        If veh_id <> "0" Then
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("chkMoviles"), CheckBox)
                rdnMovil.Checked = False
                If DataListVehiculos.DataKeys(row.ItemIndex).ToString() = veh_id Then rdnMovil.Checked = True
                rdnMovil.Enabled = False
            Next

        End If
    End Sub

    Protected Sub valInquiry_ServerValidation(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        '   args.IsValid = chkMoviles.SelectedItem IsNot Nothing
    End Sub


    Protected Sub ddlRecorrido_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlRecorrido.SelectedIndexChanged
        'si selecciona una zona la busco y la pinto
        Dim _recorrido As Recorridos = clsRecorrido.SelectById(CInt(ddlRecorrido.SelectedValue))

        If _recorrido IsNot Nothing Then
            txtNombre.Text = _recorrido.rec_nombre
            txtDireccionDestino.Text = _recorrido.rec_destino
            txtDireccionOrigen.Text = _recorrido.rec_origen
            hdnrec_id.Value = _recorrido.rec_id.ToString()
            For Each puntos As Recorridos_Puntos In _recorrido.Recorridos_Puntos
                hdnRuta.Value += "|" + puntos.rec_latitud + "," + puntos.rec_longitud
            Next
        End If

        'pinto el recorrido en el mapa

        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "mostrarRec", "  initialize();", True)

    End Sub

    Protected Sub ddlgrupo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlgrupo.SelectedIndexChanged
        Try
            'cargo lso moviles del grupo
            'si selecciono ver todos los moviles los cargo a todos
            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivosGrupo(CInt(ddlgrupo.SelectedValue), "")

            If vehiculos.Count > 0 Then
                DataListVehiculos.DataSource = vehiculos
                DataListVehiculos.DataBind()

                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("chkMoviles"), CheckBox)

                    'verifico si el movil tiene alarmas en esta categoria
                    Dim _alertas As List(Of Alertas_Velocidad_Configuradas) = clsAlarma.ListByMovil(CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString()))
                    If _alertas.Count > 0 Then rdnMovil.Checked = True

                Next

                PanelMoviles.Visible = True
                chkTodos.Checked = False
            Else
                lblError.Text = "No posee Moviles Activos para definir Alarmas. Contacte al Administrador."
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub chkTodos_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodos.CheckedChanged
        'muestro todos los auots del cliente
        If chkTodos.Checked Then
            cargarMoviles(0)
            ddlgrupo.SelectedValue = "0"
            PanelMoviles.Visible = True
        Else
            '  chkMoviles.Items.Clear()
        End If

    End Sub

    Protected Sub MostrarPuntosObligatorios(ByVal alarma As Alertas_Recorridos)
        'abro el panel de los puntos, tengo que llenar el data lista con los puntos

        Dim _puntos As New List(Of Punto)


        'al marcar el punto en el mapa voy a  buscar al direccion tambien
        For Each _punto As Recorridos_Puntos In alarma.Recorridos.Recorridos_Puntos

            Dim _direccion As New Punto

            _direccion.idPunto = 0
            _direccion.lat = _punto.rec_latitud
            _direccion.lng = _punto.rec_longitud

            'verifico si tengo este punto en los obligatorios
        
            Dim _puntoVisitar As Alertas_Recorrido_Puntos_Visitar = alarma.Alertas_Recorrido_Puntos_Visitar.Where(Function(p) p.rec_punto_id = _punto.rec_punto_id).FirstOrDefault()

                If _puntoVisitar IsNot Nothing Then
                    _direccion.Fecha = _puntoVisitar.rec_punto_fecha_llegada
                _direccion.HoraDesde = _puntoVisitar.rec_punto_horario_desde.Split(":")(0)
                _direccion.MinDesde = _puntoVisitar.rec_punto_horario_desde.Split(":")(1)
                _direccion.HoraHasta = _puntoVisitar.rec_punto_horario_hasta.Split(":")(0)
                _direccion.MinHasta = _puntoVisitar.rec_punto_horario_hasta.Split(":")(1)
                _direccion.Marcar = "true"
                _direccion.direccion = _puntoVisitar.rec_punto_direccion

            Else
                _direccion.direccion = clsGoogle.getdireccion(_punto.rec_latitud, _punto.rec_longitud, "").dir_nombre_via
                _direccion.Fecha = ""
                _direccion.HoraDesde = ""
                _direccion.MinDesde = ""
                _direccion.HoraHasta = ""
                _direccion.MinHasta = ""
                _direccion.Marcar = "false"
            End If

            _puntos.Add(_direccion)


        Next
        PanelPuntos.Visible = True

        DataListPuntos.DataSource = _puntos
        DataListPuntos.DataBind()
    End Sub

    Protected Sub linkPuntos_Click(sender As Object, e As EventArgs) Handles linkPuntos.Click
        Try
            'abro el panel de los puntos, tengo que llenar el data lista con los puntos
            Dim _error As String = ""
            'verifico si eligio armar el recorrido con el traso de google
            Dim _puntos As New List(Of Punto)
            Dim coordena As String()

            'al marcar el punto en el mapa voy a  buscar al direccion tambien

            If hdnRuta.Value <> "" Then
                Dim puntos = hdnRuta.Value.Split("|")
                For i As Integer = 0 To puntos.Length - 1
                    If puntos(i) <> "" Then
                        Dim _direccion As New Punto
                        coordena = puntos(i).Split(",")
                        _direccion.idPunto = 0
                        _direccion.lat = coordena(0)
                        _direccion.lng = coordena(1)
                        _direccion.direccion = clsGoogle.getdireccion(coordena(0).ToString, coordena(1).ToString, _error).dir_nombre_via
                        _direccion.Fecha = ""
                        _direccion.HoraDesde = ""
                        _direccion.MinDesde = ""
                        _direccion.HoraHasta = ""
                        _direccion.MinHasta = ""
                        _puntos.Add(_direccion)
                    End If

                Next
            Else
                'vuelvo a crear los puntos
                _puntos = getRoutePuntos(txtDireccionOrigen.Text.Replace("'", " "), txtDireccionDestino.Text.Replace("'", " "))

            End If

            If _puntos.Count > 0 Then
                PanelPuntos.Visible = True
                DataListPuntos.DataSource = _puntos
                DataListPuntos.DataBind()
            Else
                lblErrorPuntos.Text = "Seleccione un Recorrido para ver los Puntos."
            End If
           
            'muestro el tab donde estoy configurando los puntos
            ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "mostrarTab", " $(function () {$('#tabs').tabs('select', document.getElementById('tabs-4').value);});", True)

        Catch ex As Exception
            lblErrorPuntos.Text = ex.Message
        End Try
       
    End Sub

    Public Sub DataListPuntos_ItemDataBound(sender As Object, e As DataListItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or _
              e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblMarcar As Label = DirectCast(e.Item.FindControl("lblMarcado"), Label)
          
            Dim seleccionado As CheckBox = DirectCast(e.Item.FindControl("chkDireccion"), CheckBox)

            If lblMarcar.Text = "true" Then
                seleccionado.Checked = True
            Else
                seleccionado.Checked = False
            End If
        End If

    End Sub



End Class

Public Class Punto
    Private m_direccion As String
    Private m_lat As String
    Private m_lng As String
    Private m_idPunto As Int32
    Private m_Fecha As String
    Private m_HoraDesde As String
    Private m_MinDesde As String
    Private m_HoraHasta As String
    Private m_MinHasta As String
    Private m_marcar As String = "false"
    Public Property idPunto() As Int32
        Get
            Return m_idPunto
        End Get
        Set(ByVal value As Int32)
            m_idPunto = value
        End Set
    End Property

    Public Property direccion() As String
        Get
            Return m_direccion
        End Get
        Set(ByVal value As String)
            m_direccion = value
        End Set
    End Property
    Public Property lat() As String
        Get
            Return m_lat
        End Get
        Set(ByVal value As String)
            m_lat = value
        End Set
    End Property
    Public Property lng() As String
        Get
            Return m_lng
        End Get
        Set(ByVal value As String)
            m_lng = value
        End Set
    End Property
    Public Property Fecha() As String
        Get
            Return m_Fecha
        End Get
        Set(ByVal value As String)
            m_Fecha = value
        End Set
    End Property
    Public Property HoraDesde() As String
        Get
            Return m_HoraDesde
        End Get
        Set(ByVal value As String)
            m_HoraDesde = value
        End Set
    End Property
    Public Property MinDesde() As String
        Get
            Return m_MinDesde
        End Get
        Set(ByVal value As String)
            m_MinDesde = value
        End Set
    End Property

    Public Property HoraHasta() As String
        Get
            Return m_HoraHasta
        End Get
        Set(ByVal value As String)
            m_HoraHasta = value
        End Set
    End Property
    Public Property MinHasta() As String
        Get
            Return m_MinHasta
        End Get
        Set(ByVal value As String)
            m_MinHasta = value
        End Set
    End Property
    Public Property Marcar() As String
        Get
            Return m_marcar
        End Get
        Set(ByVal value As String)
            m_marcar = value
        End Set
    End Property
End Class