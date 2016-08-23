'Pagina para configurar las alertas de llegada o salida de direcciones
Imports GPS.Business
Imports GPS.Data
Imports System.Net
Imports System.Xml
Imports System.IO

Partial Public Class pAlarmasDireccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim masOpciones As Boolean = False
            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then

                    hdncli_id.Value = Session("Cliente").ToString()
                    'busco las direcciones creadas del cliente
                    ddlDireccion.DataSource = clsDireccion.ListByCliente(CInt(hdncli_id.Value))
                    ddlDireccion.DataBind()
                    Dim item As ListItem = New ListItem("- Seleccione Dirección -", "0")
                    ddlDireccion.Items.Insert(0, item)

                    'cargo el combo de marcadores
                    ddlMarcador.DataSource = clsMarcador.List(CInt(hdncli_id.Value))
                    ddlMarcador.DataBind()
                    item = New ListItem("- Seleccione Marcador -", "0")
                    ddlMarcador.Items.Insert(0, item)

                    'verifico si esta editando la alerta, busco los datos ya guardados
                    If Request.Params("dir_id") IsNot Nothing Then

                        If Request.Params("dir_id").ToString().Contains("|") Then
                            Dim valores As String() = Request.Params("dir_id").ToString().Split("|")
                            hdnveh_id.Value = valores(1)
                            hdnadir_id.Value = valores(0)

                            Dim direccion As Alertas_Direcciones = clsAlarma.AlertaDireccionSelect(CInt(hdnadir_id.Value))

                            lblMovil.Text = "Vehiculo Patente: " & direccion.Vehiculo.veh_patente

                            txtDireccion.Text = direccion.Direcciones.dir_direccion
                            txtNombre.Text = direccion.Direcciones.dir_nombre

                            ' ddlVehiculo.SelectedValue = direccion.veh_id.ToString()
                            lat.Value = direccion.Direcciones.dir_latitud
                            lng.Value = direccion.Direcciones.dir_longitud
                            ' chkSMS.Checked = direccion.adir_enviar_sms
                            chkMail.Checked = direccion.adir_enviar_mail
                            txtKms.Text = direccion.adir_umbral_desvio

                            hdndir_id.Value = direccion.dir_id.ToString()
                            rdnActiva.SelectedValue = direccion.adir_activa.ToString()


                            If direccion.adir_fecha_desde IsNot Nothing Then
                                txtFechaDesde.Text = direccion.adir_fecha_desde.Value.ToString("dd/MM/yyyy")
                                ddlHoraDesde.SelectedValue = direccion.adir_fecha_desde.Value.Hour.ToString().PadLeft(2, "0")
                                ddlMinDesde.SelectedValue = direccion.adir_fecha_desde.Value.Minute.ToString().PadLeft(2, "0")
                            End If

                            If direccion.adir_fecha_hasta IsNot Nothing Then
                                txtFechaHasta.Text = direccion.adir_fecha_hasta.Value.ToString("dd/MM/yyyy")
                                ddlHorahasta.SelectedValue = direccion.adir_fecha_hasta.Value.Hour.ToString().PadLeft(2, "0")
                                ddlMinHasta.SelectedValue = direccion.adir_fecha_hasta.Value.Minute.ToString().PadLeft(2, "0")
                            End If

                            'si es edicion marco la zona y la pinto
                            ddlDireccion.SelectedValue = hdndir_id.Value
                            '  ddlDireccion_SelectedIndexChanged(sender, e)

                            '  PanelTildar.Visible = False
                            '----
                            'Frecuencia
                            For Each frecuencia As Alertas_Direcciones_Frecuencia In direccion.Alertas_Direcciones_Frecuencia
                                'dias 
                                If frecuencia.dir_dia_semana = 1 Then 'lunes
                                    chkLunes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeL.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Hours
                                    ddlMinDesdeL.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Minutes

                                    ddlhoraHastaL.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Hours
                                    ddlMinHastaL.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.dir_dia_semana = 2 Then
                                    chkMartes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeM.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Hours
                                    ddlminDesdeM.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Minutes

                                    ddlhoraHastaM.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Hours
                                    ddlminHastaM.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.dir_dia_semana = 3 Then
                                    chkMiercoles.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeMi.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Hours
                                    ddlminDesdeMi.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Minutes

                                    ddlhoraHastaMi.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Hours
                                    ddlminHastaMi.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.dir_dia_semana = 4 Then
                                    chkJueves.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeJ.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Hours
                                    ddlminDesdeJ.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Minutes

                                    ddlhoraHastaJ.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Hours
                                    ddlminHastaJ.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.dir_dia_semana = 5 Then
                                    chkViernes.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeV.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Hours
                                    ddlminDesdeV.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Minutes

                                    ddlhoraHastaV.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Hours
                                    ddlminHastaV.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.dir_dia_semana = 6 Then
                                    chkSabado.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeS.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Hours
                                    ddlminDesdeS.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Minutes

                                    ddlhoraHastaS.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Hours
                                    ddlminHastaS.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Minutes

                                End If

                                If frecuencia.dir_dia_semana = 0 Then
                                    chkDomingo.Checked = True
                                    masOpciones = True
                                    ddlhoraDesdeD.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Hours
                                    ddlminDesdeD.SelectedValue = frecuencia.dir_frec_hora_desde.Value.Minutes

                                    ddlhoraHastaD.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Hours
                                    ddlminHastaD.SelectedValue = frecuencia.dir_frec_hora_hasta.Value.Minutes

                                End If
                            Next

                            PanelMoviles.Visible = False
                            PanelGrupo.Visible = False
                        Else
                            hdnveh_id.Value = Request.Params("dir_id").ToString()
                            hdnadir_id.Value = "0"
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
                            'cargo los vehiculos activos

                         


                    End If
                    PanelMoviles.Visible = True

                    cargarMoviles(hdnveh_id.Value)

                        If masOpciones Then

                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " mostrar();", True)
                        End If
                    End If

            Else
                'no esta logeado
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "redirect", "parent.iraLogin();", True)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS DIRECCION - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos de la Alerta.Contacte al administrador."
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try

        
            'guardo esta definicion de la alerta

            Dim latitud As String = lat.Value
            Dim longitud As String = lng.Value
            Dim dir As String = txtDireccion.Text
            Dim sError As String = ""
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

            'verifico si eligio un punto en el mapa, sino llamo a la api de google para obtener la lat y lng en base a la direccion
            If latitud = "-34.604" And longitud = "-58.382" Then
                'si la direccion esta vacio tiro alerta

                If (dir <> "Ruta Nacional 188, La Pampa Province, Argentina") Then
                    clsGoogle.getLatLng(latitud, longitud, dir, sError)
                Else
                    lblError.Text = "Ingrese la Dirección o seleccione el punto en el mapa"
                    Exit Sub
                End If

            Else
                'si eligio un punto recupero la direccion
                clsGoogle.getdireccion(latitud, longitud, dir, sError)
            End If

            If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
                If DateTime.Parse(txtFechaHasta.Text) < DateTime.Parse(txtFechaHasta.Text) Then
                    lblError.Text = "La Fecha de Finalización debe ser Mayor a la de Inicio."
                    Exit Sub
                End If
            End If

            Dim adireccion = New Alertas_Direcciones()
            Dim direccion As New Direcciones()
            'si la alerta no existe la doy de alta

            'verifico si es edicion 
            If hdnadir_id.Value <> "0" Then
                adireccion = clsAlarma.AlertaDireccionSelect(CInt(hdnadir_id.Value))
                If hdndir_id.Value = "0" Then ' no existe la direccion
                    direccion = New Direcciones()
                    direccion.dir_direccion = txtDireccion.Text
                    direccion.dir_latitud = latitud
                    direccion.dir_longitud = longitud
                    direccion.dir_nombre = txtNombre.Text
                    direccion.cli_id = CInt(hdncli_id.Value)
                    clsDireccion.Insert(direccion)

                    adireccion.dir_id = direccion.dir_id
                Else
                    'actualizo
                    direccion = clsDireccion.SelectById(CInt(hdndir_id.Value))
                    direccion.dir_direccion = txtDireccion.Text
                    direccion.dir_latitud = latitud
                    direccion.dir_longitud = longitud
                    direccion.dir_nombre = txtNombre.Text
                    clsDireccion.UpdateDireccion(direccion)

                    adireccion.dir_id = direccion.dir_id
                End If

                adireccion.adir_enviar_mail = chkMail.Checked
                adireccion.adir_enviar_sms = False
                adireccion.adir_tipo = 1
                adireccion.adir_activa = CBool(rdnActiva.SelectedValue)
                If txtKms.Text <> "" Then
                    adireccion.adir_umbral_desvio = txtKms.Text
                Else
                    adireccion.adir_umbral_desvio = "0"
                End If



                If txtFechaDesde.Text <> "" Then
                    adireccion.adir_fecha_desde = CDate(txtFechaDesde.Text + " " + ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + ":00")
                Else
                    adireccion.adir_fecha_desde = Nothing
                End If

                If txtFechaHasta.Text <> "" Then
                    adireccion.adir_fecha_hasta = CDate(txtFechaHasta.Text + " " + ddlHorahasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + ":00")
                Else
                    adireccion.adir_fecha_hasta = Nothing
                End If

                'borro la configuracion de frecuencia previa y la vuelvo a grabar
                clsAlarma.DeleteAlertas_Direcciones_Frecuencia(adireccion)
                clsAlarma.UpdateAlertaDireccion(adireccion)
            Else

                If hdndir_id.Value = "0" Then ' no existe la direccion
                    direccion = New Direcciones()
                    direccion.dir_direccion = txtDireccion.Text
                    direccion.dir_latitud = latitud
                    direccion.dir_longitud = longitud
                    direccion.dir_nombre = txtNombre.Text
                    direccion.cli_id = CInt(hdncli_id.Value)
                    clsDireccion.Insert(direccion)


                Else
                    'actualizo
                    direccion = clsDireccion.SelectById(CInt(hdndir_id.Value))
                    direccion.dir_direccion = txtDireccion.Text
                    direccion.dir_latitud = latitud
                    direccion.dir_longitud = longitud
                    direccion.dir_nombre = txtNombre.Text
                    clsDireccion.UpdateDireccion(direccion)


                End If

                'asocio los moviles que selecciono
                'Ver si eta editando alarma desde un movil pre elegido 
                For Each row As DataListItem In DataListVehiculos.Items 'asocio los moviles que selecciono
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("chkMoviles"), CheckBox)
                    If rdnMovil.Checked Then
                        adireccion = New Alertas_Direcciones()
                        adireccion.dir_id = direccion.dir_id

                        adireccion.adir_enviar_mail = chkMail.Checked
                        adireccion.adir_enviar_sms = False
                        adireccion.adir_tipo = 1
                        adireccion.adir_activa = CBool(rdnActiva.SelectedValue)
                        If txtKms.Text <> "" Then
                            adireccion.adir_umbral_desvio = txtKms.Text
                        Else
                            adireccion.adir_umbral_desvio = "0"
                        End If

                        If txtFechaDesde.Text <> "" Then
                            adireccion.adir_fecha_desde = CDate(txtFechaDesde.Text + " " + ddlHoraDesde.SelectedValue + ":" + ddlMinDesde.SelectedValue + ":00")
                        Else
                            adireccion.adir_fecha_desde = Nothing
                        End If

                        If txtFechaHasta.Text <> "" Then
                            adireccion.adir_fecha_hasta = CDate(txtFechaHasta.Text + " " + ddlHorahasta.SelectedValue + ":" + ddlMinHasta.SelectedValue + ":00")
                        Else
                            adireccion.adir_fecha_hasta = Nothing
                        End If


                        adireccion.veh_id = CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString())
                        clsAlarma.InsertAlertaDireccion(adireccion)
                    End If
                Next
            End If
          
          
            If chkLunes.Checked Then
                'guardo la frecuencia segun lo que eligio
                Dim frecuencia As Alertas_Direcciones_Frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia.dir_dia_semana = 1

                frecuencia.dir_frec_hora_desde = TimeSpan.Parse(ddlhoraDesdeL.SelectedValue + ":" + ddlMinDesdeL.SelectedValue)
                frecuencia.dir_frec_hora_hasta = TimeSpan.Parse(ddlhoraHastaL.SelectedValue + ":" + ddlMinHastaL.SelectedValue)

                clsAlarma.InsertAlertaDireccionFrecuencia(frecuencia)

            End If

            If chkMartes.Checked Then
                Dim frecuencia As Alertas_Direcciones_Frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id
                frecuencia.dir_dia_semana = 2
                frecuencia.dir_frec_hora_desde = TimeSpan.Parse(ddlhoraDesdeM.SelectedValue + ":" + ddlminDesdeM.SelectedValue)
                frecuencia.dir_frec_hora_hasta = TimeSpan.Parse(ddlhoraHastaM.SelectedValue + ":" + ddlminHastaM.SelectedValue)
                clsAlarma.InsertAlertaDireccionFrecuencia(frecuencia)

            End If

            If chkMiercoles.Checked Then
                Dim frecuencia As Alertas_Direcciones_Frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id
                frecuencia.dir_dia_semana = 3

                frecuencia.dir_frec_hora_desde = TimeSpan.Parse(ddlhoraDesdeMi.SelectedValue + ":" + ddlminDesdeMi.SelectedValue)
                frecuencia.dir_frec_hora_hasta = TimeSpan.Parse(ddlhoraHastaMi.SelectedValue + ":" + ddlminHastaMi.SelectedValue)
                clsAlarma.InsertAlertaDireccionFrecuencia(frecuencia)
            End If

            If chkJueves.Checked Then
                Dim frecuencia As Alertas_Direcciones_Frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id
                frecuencia.dir_dia_semana = 4

                frecuencia.dir_frec_hora_desde = TimeSpan.Parse(ddlhoraDesdeJ.SelectedValue + ":" + ddlminDesdeJ.SelectedValue)
                frecuencia.dir_frec_hora_hasta = TimeSpan.Parse(ddlhoraHastaJ.SelectedValue + ":" + ddlminHastaJ.SelectedValue)
                clsAlarma.InsertAlertaDireccionFrecuencia(frecuencia)

            End If

            If chkViernes.Checked Then
                Dim frecuencia As Alertas_Direcciones_Frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id
                frecuencia.dir_dia_semana = 5

                frecuencia.dir_frec_hora_desde = TimeSpan.Parse(ddlhoraDesdeV.SelectedValue + ":" + ddlminDesdeV.SelectedValue)
                frecuencia.dir_frec_hora_hasta = TimeSpan.Parse(ddlhoraHastaV.SelectedValue + ":" + ddlminHastaV.SelectedValue)
                clsAlarma.InsertAlertaDireccionFrecuencia(frecuencia)

            End If

            If chkSabado.Checked Then
                Dim frecuencia As Alertas_Direcciones_Frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia.dir_dia_semana = 6

                frecuencia.dir_frec_hora_desde = TimeSpan.Parse(ddlhoraDesdeS.SelectedValue + ":" + ddlminDesdeS.SelectedValue)
                frecuencia.dir_frec_hora_hasta = TimeSpan.Parse(ddlhoraHastaS.SelectedValue + ":" + ddlminHastaS.SelectedValue)
                clsAlarma.InsertAlertaDireccionFrecuencia(frecuencia)

            End If

            If chkDomingo.Checked Then
                Dim frecuencia As Alertas_Direcciones_Frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia = New Alertas_Direcciones_Frecuencia()
                frecuencia.dir_id = adireccion.adir_id

                frecuencia.dir_dia_semana = 0
                frecuencia.dir_frec_hora_desde = TimeSpan.Parse(ddlhoraDesdeD.SelectedValue + ":" + ddlminDesdeD.SelectedValue)
                frecuencia.dir_frec_hora_hasta = TimeSpan.Parse(ddlhoraHastaD.SelectedValue + ":" + ddlminHastaD.SelectedValue)
                clsAlarma.InsertAlertaDireccionFrecuencia(frecuencia)
            End If



            'retorno al listado
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)


        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS DIRECCION - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Grabando los datos de la Alerta.Contacte al administrador."
        End Try

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
    
    Protected Sub ddlDireccion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDireccion.SelectedIndexChanged
        Dim direccion As Direcciones = clsDireccion.SelectById(CInt(ddlDireccion.SelectedValue))
        If direccion IsNot Nothing Then
            txtNombre.Text = direccion.dir_nombre
            txtDireccion.Text = direccion.dir_direccion
            hdndir_id.Value = direccion.dir_id.ToString()
            lat.Value = direccion.dir_latitud
            lng.Value = direccion.dir_longitud

            ddlMarcador.SelectedValue = "0"
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " codeAddress();", True)

        End If
    End Sub

    Protected Sub ddlMarcador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarcador.SelectedIndexChanged
        Try
            Dim sError As String = ""
            Dim latitud As String = ""
            Dim longitud As String = ""
            'tomo la direccion del marcador
            Dim _marcador As Marcadores = clsMarcador.SelectById(CInt(ddlMarcador.SelectedValue))

            If _marcador IsNot Nothing Then
                txtNombre.Text = _marcador.marc_nombre
                txtDireccion.Text = _marcador.marc_direccion
                lat.Value = _marcador.marc_latitud
                lng.Value = _marcador.marc_longitud
            End If
           

            ddlDireccion.SelectedValue = "0"

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", " codeAddress();", True)
        Catch ex As Exception
            Funciones.WriteToEventLog("DIRECCION MARCADOR - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error obteniendo los datos.Contacte al administrador."
        End Try
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
            ' chkMoviles.Items.Clear()
        End If

    End Sub

    Protected Sub LinkDestildar_Click(sender As Object, e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = False
        Next
    End Sub

    Protected Sub LinkTildar_Click(sender As Object, e As EventArgs) Handles LinkTildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("chkMoviles"), CheckBox)
            movil.Checked = True
        Next
    End Sub
End Class