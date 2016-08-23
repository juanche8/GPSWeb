Imports GPS.Business
Imports GPS.Data
Public Class pAlarmaInactividad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Session("Cliente") IsNot Nothing Then
                lblError.Text = ""
                If Not IsPostBack Then
                    hdncli_id.Value = Session("Cliente").ToString()

                    'verifico si tiene grupos y cargo el combo
                    Dim _grupos As List(Of Grupos) = clsGrupo.Search(CInt(hdncli_id.Value))

                    If _grupos.Count > 0 Then

                        ddlgrupo.DataSource = _grupos
                        ddlgrupo.DataBind()

                        ddlgrupo.Items.Insert(0, New ListItem("--Seleccione--", 0))

                        PanelGrupo.Visible = True
                    End If

                    'verifico si esta editando la alerta, busco los datos ya guardados
                    If Request.Params("alah_id") IsNot Nothing Then
                        If Request.Params("alah_id").ToString().Contains("|") Then
                            Dim valores As String() = Request.Params("alah_id").ToString().Split("|")
                            hdnveh_id.Value = valores(1)
                            hdnalah_id_id.Value = valores(0)

                            Dim _alarma As Alarmas_Inactividad = clsAlarma.AlertaInactividadSelect(hdnalah_id_id.Value)

                            lblMovil.Text = "Vehículo Patente: " & _alarma.Vehiculo.veh_patente

                            ddlhoraDesde.SelectedValue = _alarma.alari_horadesde.Split(":")(0).Trim()
                            ddlminDesde.SelectedValue = _alarma.alari_horadesde.Split(":")(1).Trim()
                            ddlHoraHasta.SelectedValue = _alarma.alari_horahasta.Split(":")(0).Trim()
                            ddlMinHasta.SelectedValue = _alarma.alari_horahasta.Split(":")(1).Trim()

                            txtDescripcion.Text = _alarma.alari_descripcion
                            txtMinutos.Text = _alarma.alari_tiempo_minimo

                            txtFechaDesde.Text = _alarma.alari_fechadesde
                            txtFechaHasta.Text = _alarma.alari_fechahasta
                            chkMail.Checked = _alarma.alari_enviar_mail

                            'marco los dias - si cargo fechas no muestro dias
                            If _alarma.alari_fechadesde <> "" Or _alarma.alari_fechahasta <> "" Then
                                rdbTipo.SelectedValue = "1"
                            Else

                                Dim _dias As List(Of Alarmas_Inactividad_Dias) = clsAlarma.AlertaInactividadDiasList(_alarma.alari_id)

                                'verifico los dias que eligio
                                For Each dia As ListItem In chkDias.Items

                                    If _dias.Where(Function(d) d.alari_dia_semana = dia.Value).FirstOrDefault() IsNot Nothing Then
                                        dia.Selected = True
                                    End If
                                Next

                                rdbTipo.SelectedValue = "2"
                            End If
                            rdbTipo_SelectedIndexChanged(sender, e)

                            PanelGrupo.Visible = False
                            PanelMoviles.Visible = False
                        Else
                            hdnveh_id.Value = Request.Params("alah_id").ToString()
                            hdnalah_id_id.Value = "0"

                            PanelMoviles.Visible = True
                            cargarMoviles(hdnveh_id.Value)
                        End If

                       
                    Else
                        'cargo los vehiculos activos

                        PanelMoviles.Visible = True
                        cargarMoviles(hdnveh_id.Value)

                    End If



                End If
            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS FUERA HORARIO - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos de la Alerta.Contacte al administrador."
        End Try

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Try
            'Doy de alta la alarmas
            'verifico que no exista uno igual para el mismo movil
            Dim selecciono As Boolean = True
            'verifico si eligio un movil
            For Each row As DataListItem In DataListVehiculos.Items 'asocio los moviles que selecciono
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                If rdnMovil.Checked Then
                    selecciono = True
                    Exit For
                Else
                    selecciono = False
                End If
            Next

            If selecciono = False Then
                lblError.Text = "Debe seleccionar al menos un Móvil."
                Exit Sub
            End If

            'NUEVO
            If hdnalah_id_id.Value = "0" Then
                'recorro el check y guardo los moviles
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        Dim alarma As Alarmas_Inactividad = New Alarmas_Inactividad()
                        alarma.alari_descripcion = txtDescripcion.Text

                        alarma.alari_horadesde = ddlhoraDesde.SelectedValue & ":" & ddlminDesde.SelectedValue
                        'si hora hasta el 00 le pongo 23:59
                        If ddlHoraHasta.SelectedValue = "00" And ddlMinHasta.SelectedValue = "00" Then
                            alarma.alari_horahasta = "23:59"
                        Else
                            alarma.alari_horahasta = ddlHoraHasta.SelectedValue & ":" & ddlMinHasta.SelectedValue
                        End If

                        alarma.veh_id = CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString())
                        If txtMinutos.Text <> "" Then
                            alarma.alari_tiempo_minimo = CInt(txtMinutos.Text)
                        Else
                            alarma.alari_tiempo_minimo = 0
                        End If


                        alarma.alari_fechadesde = txtFechaDesde.Text
                        alarma.alari_fechahasta = txtFechaHasta.Text
                        alarma.alari_enviar_mail = chkMail.Checked
                        alarma.alari_enviar_sms = False
                        alarma.alah_velocidad_minima = 20
                        clsAlarma.InsertAlertaInactividad(alarma)

                        'verifico los dias que eligio
                        For Each dias As ListItem In chkDias.Items

                            If dias.Selected Then
                                Dim alarmaDia As Alarmas_Inactividad_Dias = New Alarmas_Inactividad_Dias()
                                'dia cero genero un registro para cada dia
                                alarmaDia.alari_dia_semana = dias.Value
                                alarmaDia.alari_id = alarma.alari_id
                                clsAlarma.InsertAlertaInactividadDia(alarmaDia)
                            End If
                        Next
                    End If
                Next
            Else
                'modifico para un auto
                Dim alarma As Alarmas_Inactividad = clsAlarma.AlertaInactividadSelect(hdnalah_id_id.Value)
                alarma.alari_descripcion = txtDescripcion.Text

                alarma.alari_horadesde = ddlhoraDesde.SelectedValue & ":" & ddlminDesde.SelectedValue
                alarma.alari_horahasta = ddlHoraHasta.SelectedValue & ":" & ddlMinHasta.SelectedValue

                If txtMinutos.Text <> "" Then
                    alarma.alari_tiempo_minimo = CInt(txtMinutos.Text)
                Else
                    alarma.alari_tiempo_minimo = 0
                End If

                alarma.alari_fechadesde = txtFechaDesde.Text
                alarma.alari_fechahasta = txtFechaHasta.Text
                alarma.alari_enviar_mail = chkMail.Checked
                alarma.alari_enviar_sms = False

                clsAlarma.UpdateAlertaInactividad(alarma)

                clsAlarma.DeleteAlertas_Inactividad_Dias(alarma)
                'verifico los dias que eligio
                For Each dias As ListItem In chkDias.Items

                    If dias.Selected Then
                        Dim alarmaDia As Alarmas_Inactividad_Dias = New Alarmas_Inactividad_Dias()
                        'dia cero genero un registro para cada dia
                        alarmaDia.alari_dia_semana = dias.Value
                        alarmaDia.alari_id = alarma.alari_id
                        clsAlarma.InsertAlertaInactividadDia(alarmaDia)
                    End If
                Next
            End If


            'retorno al listado
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)

        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS INACTIVIDAD GRABAR - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Granado los datos de la Alerta.Contacte al administrador."
        End Try
    End Sub

    Private Sub cargarMoviles(ByVal veh_id As String)
        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(CInt(hdncli_id.Value))

        DataListVehiculos.DataSource = vehiculos
        DataListVehiculos.DataBind()

        'marco el ckeck con el movil y lo desabilito
        If veh_id <> "0" Then
            For Each row As DataListItem In DataListVehiculos.Items
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                rdnMovil.Checked = False
                If DataListVehiculos.DataKeys(row.ItemIndex).ToString() = veh_id Then rdnMovil.Checked = True
                rdnMovil.Enabled = False
            Next

        End If
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

    Protected Sub rdbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdbTipo.SelectedIndexChanged
        If rdbTipo.SelectedValue = "1" Then
            PanelFecha.Visible = True
            PanelDias.Visible = False
        Else
            PanelFecha.Visible = False
            PanelDias.Visible = True
        End If
    End Sub

    Protected Sub LinkDestildar_Click(sender As Object, e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = False
        Next
    End Sub

    Protected Sub LinkTildar_Click(sender As Object, e As EventArgs) Handles LinkTildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = True
        Next
    End Sub
End Class