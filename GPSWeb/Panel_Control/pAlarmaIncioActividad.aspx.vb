Imports GPS.Business
Imports GPS.Data
Public Class pAlarmaIncioActividad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim masOpciones As Boolean = False
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

                            Dim _alarma As Alarma_Inicio_Actividad = clsAlarma.AlertaInicioActividadSelect(hdnalah_id_id.Value)

                            lblMovil.Text = "Vehículo Patente: " & _alarma.Vehiculo.veh_patente
                            txtDescripcion.Text = _alarma.alaric_descripcion
                            ddlhoraDesde.SelectedValue = _alarma.alar_horainicio.Split(":")(0).Trim()
                            ddlminDesde.SelectedValue = _alarma.alar_horainicio.Split(":")(1).Trim()
                            chkMail.Checked = _alarma.alar_enviar_mail

                            chkInicioAntes.Checked = _alarma.alaric_inicio_anteshorario
                            Dim _dias As List(Of Alarmas_Inicio_Actividad_Dias) = clsAlarma.AlertaInicioActividadDiasList(_alarma.alaric_id)

                            'verifico los dias que eligio
                            For Each dia As ListItem In chkDias.Items

                                If _dias.Where(Function(d) d.alaricd_dia_semana.ToString = dia.Value).FirstOrDefault() IsNot Nothing Then
                                    dia.Selected = True
                                End If
                            Next

                            PanelGrupo.Visible = False
                            PanelMoviles.Visible = False
                        Else
                            hdnveh_id.Value = Request.Params("alah_id").ToString()
                            hdnalah_id_id.Value = "0"
                            'cargo los vehiculos activos

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
            ' Funciones.WriteToEventLog("ALARMAS INICIO ACTIVIDAD - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos de la Alerta.Contacte al administrador."
        End Try

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            'Doy de alta la alarmas
            'verifico que no exista uno igual para el mismo movil
            Dim selecciono As Boolean = False
            'verifico si eligio un movil
            'solo para los casos de Nueva alarma

            If hdnalah_id_id.Value = "0" Then
                For Each row As DataListItem In DataListVehiculos.Items 'asocio los moviles que selecciono
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                    If rdnMovil.Checked Then
                        selecciono = True
                        Exit For
                    End If
                Next

                If selecciono = False Then
                    lblError.Text = "Debe seleccionar al menos un Móvil."
                    Exit Sub
                End If
            End If
           
            selecciono = False
            'verifico si eligio dias
            For Each dias As ListItem In chkDias.Items

                If dias.Selected Then
                    selecciono = True
                    Exit For
                End If
            Next

            If selecciono = False Then
                lblError.Text = "Debe seleccionar al menos un Día de la Semana."
                Exit Sub
            End If

            'NUEVO
            If hdnalah_id_id.Value = "0" Then
                'recorro el check y guardo los moviles
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        Dim alarma As Alarma_Inicio_Actividad = New Alarma_Inicio_Actividad()
                        alarma.alaric_descripcion = txtDescripcion.Text

                        alarma.alar_horainicio = ddlhoraDesde.SelectedValue & ":" & ddlminDesde.SelectedValue
                        alarma.veh_id = CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString())
                        alarma.alaric_inicio_anteshorario = chkInicioAntes.Checked
                        alarma.alar_enviar_mail = chkMail.Checked
                       
                        clsAlarma.InsertAlertaInicioActividad(alarma)

                        'verifico los dias que eligio
                        For Each dias As ListItem In chkDias.Items

                            If dias.Selected Then
                                Dim alarmaDia As Alarmas_Inicio_Actividad_Dias = New Alarmas_Inicio_Actividad_Dias()
                                'dia cero genero un registro para cada dia
                                alarmaDia.alaricd_dia_semana = dias.Value
                                alarmaDia.alaric_id = alarma.alaric_id
                                clsAlarma.InsertAlertaInicioActividadDia(alarmaDia)
                            End If
                        Next
                    End If
                Next
            Else
                'modifico para un auto
                Dim alarma As Alarma_Inicio_Actividad = clsAlarma.AlertaInicioActividadSelect(hdnalah_id_id.Value)
                alarma.alaric_descripcion = txtDescripcion.Text
                alarma.alaric_inicio_anteshorario = chkInicioAntes.Checked
                alarma.alar_horainicio = ddlhoraDesde.SelectedValue & ":" & ddlminDesde.SelectedValue
                alarma.alar_enviar_mail = chkMail.Checked

                clsAlarma.UpdateAlertaInicioActividad(alarma)

                clsAlarma.DeleteAlertas_InicioActividad_Dias(alarma)
                'verifico los dias que eligio
                For Each dias As ListItem In chkDias.Items

                    If dias.Selected Then
                        Dim alarmaDia As Alarmas_Inicio_Actividad_Dias = New Alarmas_Inicio_Actividad_Dias()
                        'dia cero genero un registro para cada dia
                        alarmaDia.alaricd_dia_semana = dias.Value
                        alarmaDia.alaric_id = alarma.alaric_id
                        clsAlarma.InsertAlertaInicioActividadDia(alarmaDia)
                    End If
                Next
            End If


            'retorno al listado
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)

        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS Inicio Actividad - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Granado los datos de la Alerta.Contacte al administrador.- " + ex.Message + " - " + ex.StackTrace
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