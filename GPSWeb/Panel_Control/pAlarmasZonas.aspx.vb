'Pagina para configurar las alarmas de entradas y salidas de zonas que quiere recibir el cliente.
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Xml
Imports System.IO
Partial Public Class pAlarmasZonas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim masOpciones As Boolean = False
            If Session("Cliente") IsNot Nothing Then

                hdncli_id.Value = Session("Cliente").ToString()

                lblError.Text = ""
                If Not IsPostBack Then
                    'busco las zonas creadas del cliente
                    ddlZona.DataSource = clsZona.ListByCliente(CInt(hdncli_id.Value))
                    ddlZona.DataBind()
                    Dim item As ListItem = New ListItem("- Seleccione Zona -", "0")
                    ddlZona.Items.Insert(0, item)

                    If Request.Params("veh_id") IsNot Nothing Then

                        If Request.Params("veh_id").ToString().Contains("|") Then
                            Dim valores As String() = Request.Params("veh_id").ToString().Split("|")
                            hdnveh_id.Value = valores(1)
                            hdnazon_id.Value = valores(0)
                            Dim zona As Alertas_Zonas = clsAlarma.AlertaZonaSelect(CInt(hdnazon_id.Value), CInt(hdnveh_id.Value))
                            lblMovil.Text = "Vehículo Patente: " & zona.Vehiculo.veh_patente





                            ' chkSMS.Checked = zona.azon_enviar_sms
                            chkMail.Checked = zona.azon_enviar_mail

                            txtKms.Text = zona.azon_umbral_desvio

                            rdnActiva.SelectedValue = zona.azon_activa.ToString()

                            hdnzon_id.Value = zona.zon_id.ToString()

                            If zona.azon_fecha_desde IsNot Nothing Then
                                txtFechaDesde.Text = zona.azon_fecha_desde.Value.ToString("dd/MM/yyyy")
                                ddlHoraDesde.SelectedValue = zona.azon_fecha_desde.Value.Hour.ToString().PadLeft(2, "0")
                                ddlMinDesde.SelectedValue = zona.azon_fecha_desde.Value.Minute.ToString()
                            End If

                            If zona.azon_fecha_hasta IsNot Nothing Then
                                txtFechaHasta.Text = zona.azon_fecha_hasta.Value.ToString("dd/MM/yyyy")
                                ddlHorahasta.SelectedValue = zona.azon_fecha_hasta.Value.Hour.ToString().PadLeft(2, "0")
                                ddlMinHasta.SelectedValue = zona.azon_fecha_hasta.Value.Minute.ToString()
                            End If
                            PanelTildar.Visible = False


                            '----
                            'Frecuencia
                            For Each frecuencia As Alertas_Zonas_Frecuencias In zona.Alertas_Zonas_Frecuencias
                                'dias 
                                If frecuencia.zon_dia_semana = 1 Then 'lunes
                                    chkLunes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeL.SelectedValue = frecuencia.zon_hora_desde.Value.Hours
                                    ddlMinDesdeL.SelectedValue = frecuencia.zon_hora_desde.Value.Minutes

                                    ddlhoraHastaL.SelectedValue = frecuencia.zon_hora_hasta.Value.Hours
                                    ddlMinHastaL.SelectedValue = frecuencia.zon_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.zon_dia_semana = 2 Then
                                    chkMartes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeM.SelectedValue = frecuencia.zon_hora_desde.Value.Hours
                                    ddlminDesdeM.SelectedValue = frecuencia.zon_hora_desde.Value.Minutes

                                    ddlhoraHastaM.SelectedValue = frecuencia.zon_hora_hasta.Value.Hours
                                    ddlminHastaM.SelectedValue = frecuencia.zon_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.zon_dia_semana = 3 Then
                                    chkMiercoles.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeMi.SelectedValue = frecuencia.zon_hora_desde.Value.Hours
                                    ddlminDesdeMi.SelectedValue = frecuencia.zon_hora_desde.Value.Minutes

                                    ddlhoraHastaMi.SelectedValue = frecuencia.zon_hora_hasta.Value.Hours
                                    ddlminHastaMi.SelectedValue = frecuencia.zon_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.zon_dia_semana = 4 Then
                                    chkJueves.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeJ.SelectedValue = frecuencia.zon_hora_desde.Value.Hours
                                    ddlminDesdeJ.SelectedValue = frecuencia.zon_hora_desde.Value.Minutes

                                    ddlhoraHastaJ.SelectedValue = frecuencia.zon_hora_hasta.Value.Hours
                                    ddlminHastaJ.SelectedValue = frecuencia.zon_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.zon_dia_semana = 5 Then
                                    chkViernes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeV.SelectedValue = frecuencia.zon_hora_desde.Value.Hours
                                    ddlminDesdeV.SelectedValue = frecuencia.zon_hora_desde.Value.Minutes

                                    ddlhoraHastaV.SelectedValue = frecuencia.zon_hora_hasta.Value.Hours
                                    ddlminHastaV.SelectedValue = frecuencia.zon_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.zon_dia_semana = 6 Then
                                    chkSabado.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeS.SelectedValue = frecuencia.zon_hora_desde.Value.Hours
                                    ddlminDesdeS.SelectedValue = frecuencia.zon_hora_desde.Value.Minutes

                                    ddlhoraHastaS.SelectedValue = frecuencia.zon_hora_hasta.Value.Hours
                                    ddlminHastaS.SelectedValue = frecuencia.zon_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.zon_dia_semana = 0 Then
                                    chkDomingo.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeD.SelectedValue = frecuencia.zon_hora_desde.Value.Hours
                                    ddlminDesdeD.SelectedValue = frecuencia.zon_hora_desde.Value.Minutes

                                    ddlhoraHastaD.SelectedValue = frecuencia.zon_hora_hasta.Value.Hours
                                    ddlminHastaD.SelectedValue = frecuencia.zon_hora_hasta.Value.Minutes

                                End If
                            Next

                            'si es edicion marco la zona y la pinto
                            ddlZona.SelectedValue = hdnzon_id.Value
                            ddlZona_SelectedIndexChanged(sender, e)


                            PanelMoviles.Visible = False
                            PanelGrupo.Visible = False
                        Else
                            hdnveh_id.Value = Request.Params("veh_id").ToString()
                            hdnazon_id.Value = "0"
                        End If
                        


                        cargarMoviles(hdnveh_id.Value)
                    Else
                        cargarMoviles(0)
                        'verifico si tiene grupos y cargo el combo
                        Dim _grupos As List(Of Grupos) = clsGrupo.Search(CInt(hdncli_id.Value))

                        If _grupos.Count > 0 Then

                            ddlgrupo.DataSource = _grupos
                            ddlgrupo.DataBind()

                            ddlgrupo.Items.Insert(0, New ListItem("--Seleccione--", 0))
                            PanelGrupo.Visible = True

                        Else
                            'cargo los vehiculos activos
                            PanelGrupo.Visible = False


                        End If
                    End If

                    If masOpciones Then
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " mostrar();", True)
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "funcion", "mostrar();", True)
                    End If
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " dejardeMarcar();", True)


                End If
            Else
                'no esta logeado
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "redirigir", " parent.iraLogin();", True)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS ZONAS - " + ex.ToString + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos. Contacte al administrador."
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        'guardo esta definicion de zona
        Try
            Dim alarzona = New Alertas_Zonas()
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

            If hdnZona.Value <> "" Then
                If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                    If DateTime.Parse(txtFechaHasta.Text) < DateTime.Parse(txtFechaHasta.Text) Then
                        lblError.Text = "La Fecha de Finalización debe ser Mayor a la de Inicio."
                        Exit Sub
                    End If
                End If
                'verifico si es edicion
                If hdnazon_id.Value <> "0" Then
                    alarzona = clsAlarma.SelectAlertaZonaById(hdnazon_id.Value)

                    'borro la configuracion de frecuencia previa y la vuelvo a grabar
                    clsAlarma.DeleteAlertas_Zona_Frecuencia(alarzona)

                    'tengo que ver si elige una zona nueva para darla de alta
                    If hdnzon_id.Value = "0" Then
                        Dim zona As Zonas = New Zonas()

                        zona.zon_nombre = txtNombre.Text
                        zona.cli_id = CInt(hdncli_id.Value)

                        clsZona.Insert(zona)
                        alarzona.zon_id = zona.zon_id

                        'creo los puntos
                        Dim puntos As String() = hdnZona.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            Dim coordena As String() = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Zonas_Puntos()

                                punto.zon_id = zona.zon_id
                                punto.zon_latitud = coordena(0).ToString()
                                punto.zon_longitud = coordena(1).ToString()

                                clsZona.InsertPunto(punto)
                            End If

                        Next
                    Else
                        alarzona.zon_id = CInt(hdnzon_id.Value)

                        'elimino los puntos y los inserto de nuevo
                        Dim zona As Zonas = clsZona.SelectById(hdnzon_id.Value)
                        'borro los puntos y la vuelvo a grabar
                        clsZona.DeleteZona_Puntos(zona)
                        zona.zon_nombre = txtNombre.Text
                        zona.cli_id = CInt(hdncli_id.Value)
                        clsZona.UpdateZona(zona)

                        'recupero los puntos que eligio para marcar la ruta
                        Dim puntos As String() = hdnZona.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            Dim coordena As String() = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Zonas_Puntos()

                                punto.zon_id = zona.zon_id
                                punto.zon_latitud = coordena(0).ToString()
                                punto.zon_longitud = coordena(1).ToString()

                                clsZona.InsertPunto(punto)
                            End If

                        Next
                    End If

                    alarzona.azon_enviar_mail = chkMail.Checked
                    alarzona.azon_enviar_sms = False
                    If txtKms.Text <> "" Then
                        alarzona.azon_umbral_desvio = txtKms.Text
                    Else
                        alarzona.azon_umbral_desvio = "0"
                    End If


                    alarzona.azon_activa = CBool(rdnActiva.SelectedValue)
                    alarzona.azon_tipo = 1

                    If txtFechaDesde.Text <> "" Then
                        alarzona.azon_fecha_desde = CDate(txtFechaDesde.Text + " " + ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + ":00")
                    Else
                        alarzona.azon_fecha_desde = Nothing
                    End If

                    If txtFechaHasta.Text <> "" Then
                        alarzona.azon_fecha_hasta = CDate(txtFechaHasta.Text + " " + ddlHorahasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + ":00")
                    Else
                        alarzona.azon_fecha_hasta = Nothing
                    End If


                    clsAlarma.UpdateAlertaZona(alarzona)

                Else
                    'si la alerta no existe la doy de alta
                    Dim zona As Zonas
                    If hdnzon_id.Value = "0" Then
                        zona = New Zonas()

                        zona.zon_nombre = txtNombre.Text
                        zona.cli_id = CInt(hdncli_id.Value)

                        clsZona.Insert(zona)
                        alarzona.zon_id = zona.zon_id

                        'creo los puntos
                        Dim puntos As String() = hdnZona.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            Dim coordena As String() = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Zonas_Puntos()

                                punto.zon_id = zona.zon_id
                                punto.zon_latitud = coordena(0).ToString()
                                punto.zon_longitud = coordena(1).ToString()

                                clsZona.InsertPunto(punto)
                            End If

                        Next
                    Else
                        'si existe elimino los puntos y los creo de nuevo por si modifico algo
                        zona = clsZona.SelectById(hdnzon_id.Value)

                        'borro los puntos y la vuelvo a grabar
                        clsZona.DeleteZona_Puntos(zona)
                        zona.zon_nombre = txtNombre.Text
                        zona.cli_id = CInt(hdncli_id.Value)
                        clsZona.UpdateZona(zona)

                        'recupero los puntos que eligio para marcar la ruta
                        Dim puntos As String() = hdnZona.Value.Split("|")
                        For i As Integer = 0 To puntos.Length - 1
                            Dim coordena As String() = puntos(i).Split(",")

                            If coordena(0).ToString() <> "" And coordena(0).ToString() <> "0" Then
                                Dim punto = New Zonas_Puntos()

                                punto.zon_id = zona.zon_id
                                punto.zon_latitud = coordena(0).ToString()
                                punto.zon_longitud = coordena(1).ToString()

                                clsZona.InsertPunto(punto)
                            End If

                        Next
                    End If


                    For Each row As DataListItem In DataListVehiculos.Items 'asocio los moviles que selecciono
                        Dim rdnMovil As CheckBox = DirectCast(row.FindControl("chkMoviles"), CheckBox)
                        If rdnMovil.Checked Then
                            alarzona = New Alertas_Zonas()


                            alarzona.zon_id = zona.zon_id
                            alarzona.azon_enviar_mail = chkMail.Checked
                            alarzona.azon_enviar_sms = False
                            If txtKms.Text <> "" Then
                                alarzona.azon_umbral_desvio = txtKms.Text
                            Else
                                alarzona.azon_umbral_desvio = "0"
                            End If

                            alarzona.azon_activa = CBool(rdnActiva.SelectedValue)
                            alarzona.azon_tipo = 1

                            If txtFechaDesde.Text <> "" Then
                                alarzona.azon_fecha_desde = CDate(txtFechaDesde.Text + " " + ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + ":00")
                            Else
                                alarzona.azon_fecha_desde = Nothing
                            End If

                            If txtFechaHasta.Text <> "" Then
                                alarzona.azon_fecha_hasta = CDate(txtFechaHasta.Text + " " + ddlHorahasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + ":00")
                            Else
                                alarzona.azon_fecha_hasta = Nothing
                            End If


                            alarzona.veh_id = CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString())
                            clsAlarma.InsertAlertaZona(alarzona)
                        End If
                    Next


                End If


                'guardo la frecuencia segun lo que eligio


                If chkLunes.Checked Then
                    Dim frecuencia As Alertas_Zonas_Frecuencias = New Alertas_Zonas_Frecuencias()
                    frecuencia.zon_id = alarzona.azon_id
                    frecuencia.zon_dia_semana = 1

                    frecuencia.zon_hora_desde = TimeSpan.Parse(ddlhoraDesdeL.SelectedValue + ":" + ddlMinDesdeL.SelectedValue)
                    frecuencia.zon_hora_hasta = TimeSpan.Parse(ddlhoraHastaL.SelectedValue + ":" + ddlMinHastaL.SelectedValue)
                    clsAlarma.InsertAlertaZonaFrecuencia(frecuencia)
                End If

                If chkMartes.Checked Then
                    Dim frecuencia As Alertas_Zonas_Frecuencias = New Alertas_Zonas_Frecuencias()
                    frecuencia.zon_id = alarzona.azon_id
                    frecuencia.zon_dia_semana = 2

                    frecuencia.zon_hora_desde = TimeSpan.Parse(ddlhoraDesdeM.SelectedValue + ":" + ddlminDesdeM.SelectedValue)
                    frecuencia.zon_hora_hasta = TimeSpan.Parse(ddlhoraHastaM.SelectedValue + ":" + ddlminHastaM.SelectedValue)
                    clsAlarma.InsertAlertaZonaFrecuencia(frecuencia)
                End If

                If chkMiercoles.Checked Then
                    Dim frecuencia As Alertas_Zonas_Frecuencias = New Alertas_Zonas_Frecuencias()
                    frecuencia.zon_id = alarzona.azon_id
                    frecuencia.zon_dia_semana = 3

                    frecuencia.zon_hora_desde = TimeSpan.Parse(ddlhoraDesdeMi.SelectedValue + ":" + ddlminDesdeMi.SelectedValue)
                    frecuencia.zon_hora_hasta = TimeSpan.Parse(ddlhoraHastaMi.SelectedValue + ":" + ddlminHastaMi.SelectedValue)
                    clsAlarma.InsertAlertaZonaFrecuencia(frecuencia)

                End If

                If chkJueves.Checked Then
                    Dim frecuencia As Alertas_Zonas_Frecuencias = New Alertas_Zonas_Frecuencias()
                    frecuencia.zon_id = alarzona.azon_id
                    frecuencia.zon_dia_semana = 4

                    frecuencia.zon_hora_desde = TimeSpan.Parse(ddlhoraDesdeJ.SelectedValue + ":" + ddlminDesdeJ.SelectedValue)
                    frecuencia.zon_hora_hasta = TimeSpan.Parse(ddlhoraHastaJ.SelectedValue + ":" + ddlminHastaJ.SelectedValue)
                    clsAlarma.InsertAlertaZonaFrecuencia(frecuencia)

                End If

                If chkViernes.Checked Then
                    Dim frecuencia As Alertas_Zonas_Frecuencias = New Alertas_Zonas_Frecuencias()
                    frecuencia.zon_id = alarzona.azon_id
                    frecuencia.zon_dia_semana = 5

                    frecuencia.zon_hora_desde = TimeSpan.Parse(ddlhoraDesdeV.SelectedValue + ":" + ddlminDesdeV.SelectedValue)
                    frecuencia.zon_hora_hasta = TimeSpan.Parse(ddlhoraHastaV.SelectedValue + ":" + ddlminHastaV.SelectedValue)
                    clsAlarma.InsertAlertaZonaFrecuencia(frecuencia)
                End If

                If chkSabado.Checked Then
                    Dim frecuencia As Alertas_Zonas_Frecuencias = New Alertas_Zonas_Frecuencias()
                    frecuencia.zon_id = alarzona.azon_id
                    frecuencia.zon_dia_semana = 6

                    frecuencia.zon_hora_desde = TimeSpan.Parse(ddlhoraDesdeS.SelectedValue + ":" + ddlminDesdeS.SelectedValue)
                    frecuencia.zon_hora_hasta = TimeSpan.Parse(ddlhoraHastaS.SelectedValue + ":" + ddlminHastaS.SelectedValue)
                    clsAlarma.InsertAlertaZonaFrecuencia(frecuencia)
                End If

                If chkDomingo.Checked Then
                    Dim frecuencia As Alertas_Zonas_Frecuencias = New Alertas_Zonas_Frecuencias()
                    frecuencia.zon_id = alarzona.azon_id
                    frecuencia.zon_dia_semana = 0

                    frecuencia.zon_hora_desde = TimeSpan.Parse(ddlhoraDesdeD.SelectedValue + ":" + ddlminDesdeD.SelectedValue)
                    frecuencia.zon_hora_hasta = TimeSpan.Parse(ddlhoraHastaD.SelectedValue + ":" + ddlminHastaD.SelectedValue)
                    clsAlarma.InsertAlertaZonaFrecuencia(frecuencia)
                End If


                'retorno al listado
                Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)


            Else
                'no marco los puntos muestro error
                lblError.Text = "Debe seleccionar o crear una zona."
            End If

        Catch ex As Exception
            lblError.Text = "Ocurrio un error Grabando los datos. Contacte al administrador. " + ex.ToString()
            Funciones.WriteToEventLog("ALARMAS ZONAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Private Sub cargarMoviles(ByVal veh_id As String)
        'si viene de una edicion marca el movil que esta editando
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

    Protected Sub ddlgrupo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlgrupo.SelectedIndexChanged
        Try
            'cargo lso moviles del grupo
            'si selecciono ver todos los moviles los cargo a todos
            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivosGrupo(CInt(ddlgrupo.SelectedValue), "")
            ' chkMoviles.Items.Clear()
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
            'chkMoviles.Items.Clear()
        End If

    End Sub

    'Private Sub selectedChecks(ByVal frecuencia As List(Of Alertas_Zonas_Frecuencias))
    '    Dim result As Boolean = False
    '    For Each check As ListItem In chkDias.Items
    '        If check.Value = "Lunes" And frecuencia.SingleOrDefault().zon_frec_lunes Then
    '            check.Selected = True
    '        End If

    '        If check.Value = "Martes" And frecuencia.SingleOrDefault().zon_frec_martes Then
    '            check.Selected = True
    '        End If

    '        If check.Value = "Miercoles" And frecuencia.SingleOrDefault().zon_frec_miercoles Then
    '            check.Selected = True
    '        End If

    '        If check.Value = "Jueves" And frecuencia.SingleOrDefault().zon_frec_jueves Then
    '            check.Selected = True
    '        End If

    '        If check.Value = "Viernes" And frecuencia.SingleOrDefault().zon_frec_viernes Then
    '            check.Selected = True
    '        End If

    '        If check.Value = "Sabado" And frecuencia.SingleOrDefault().zon_frec_sabado Then
    '            check.Selected = True
    '        End If

    '        If check.Value = "Domingo" And frecuencia.SingleOrDefault().zon_frec_domingo Then
    '            check.Selected = True
    '        End If
    '    Next

    'End Sub

    Protected Sub ddlZona_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlZona.SelectedIndexChanged
        'si selecciona una zona la busco y la pinto
        Dim zona As Zonas = clsZona.SelectById(CInt(ddlZona.SelectedValue))
        If zona IsNot Nothing Then
            txtNombre.Text = zona.zon_nombre
            hdnzon_id.Value = zona.zon_id.ToString()
            For Each puntos As Zonas_Puntos In zona.Zonas_Puntos
                hdnZona.Value += "|" + puntos.zon_latitud + "," + puntos.zon_longitud
            Next
        End If
        
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " initialize();", True)
    End Sub

    Protected Sub LinkTildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkTildar.Click
        'marco todos los check de moviles

        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = True
        Next
    End Sub

    Protected Sub LinkDestildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = False
        Next
    End Sub
End Class