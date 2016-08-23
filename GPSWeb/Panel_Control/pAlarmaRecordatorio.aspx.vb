'Pagina para configurar las alarmas de recordatorios por contidad de kilometros acumulados o para una fecha especifica
Imports GPS.Business
Imports GPS.Data
Partial Public Class pAlarmaRecordatorio
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


                    End If
                    'cargo los vehiculos activos
                    PanelGrupo.Visible = False
                    PanelMoviles.Visible = True

                    If Request.Params("veh_id") IsNot Nothing Then
                        cargarMoviles(Request.Params("veh_id").ToString)
                    Else
                        cargarMoviles("0")
                    End If



                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("ALARMAS RECORDATORIOS - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Cargando los datos. Contacte al administrador."
        End Try
    End Sub

   

    Private Sub cargarMoviles(ByVal veh_id As String)
        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.List(Session("Cliente").ToString)

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

   
    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdnFrecuencia.SelectedIndexChanged
        'cuando cambia la seleccion muestro u oculto los paneles
        If rdnFrecuencia.SelectedValue = "dia" Then
            PanelDia.Visible = True
            PanelAnio.Visible = False
            PanelMes.Visible = False
        End If
        If rdnFrecuencia.SelectedValue = "mes" Then
            PanelDia.Visible = False
            PanelAnio.Visible = False
            PanelMes.Visible = True
        End If
        If rdnFrecuencia.SelectedValue = "anio" Then
            PanelDia.Visible = False
            PanelAnio.Visible = True
            PanelMes.Visible = False
        End If
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            'Doy de alta el recordatorio
            'verifico que no exista uno igual para el mismo movil
            Dim selecciono As Boolean = False
            'verifico si eligio un movil
            For Each row As DataListItem In DataListVehiculos.Items 'asocio los moviles que selecciono
                Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)
                If rdnMovil.Checked Then
                    selecciono = True
              
                End If
            Next

            If selecciono = False Then
                lblError.Text = "Debe seleccionar al menos un Móvil."
                Exit Sub
            End If

            'cargo datos para un recordatorio de kms
            If txtNombrekm.Text <> "" Or txtFrecuencia.Text <> "" Or txtProximaOcurrencia.Text <> "" Then
                If txtNombrekm.Text = "" Then
                    lblError.Text = "Para Configurar un Recordatorio por Kms ingrese una Descripción para el mismo."
                    Exit Sub
                End If

                If txtProximaOcurrencia.Text = "" Then
                    lblError.Text = "Para Configurar un Recordatorio por Kms ingrese la cantidad en Kms para disparar la primer alarma."
                    txtProximaOcurrencia.Focus()
                    Exit Sub
                End If

                If txtFrecuencia.Text = "" Then
                    lblError.Text = "Para Configurar un Recordatorio por Kms ingrese la frecuencia en Kms para disparar las alarmas."
                    txtFrecuencia.Focus()
                    Exit Sub
                End If

                'recorro el check y guardo los moviles
                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        Dim recodatorioKm As Alertas_Recordatorios_Por_Km = New Alertas_Recordatorios_Por_Km()

                        recodatorioKm.reck_descripcion = txtNombrekm.Text
                        recodatorioKm.reck_notificar_mail = chkMail.Checked
                        recodatorioKm.reck_kms_primer_alarma = txtProximaOcurrencia.Text
                        recodatorioKm.reck_ocurrencia_cada_km = CInt(txtFrecuencia.Text)

                        recodatorioKm.reck_kilm_proxima_alarma = recodatorioKm.reck_kms_primer_alarma + recodatorioKm.reck_ocurrencia_cada_km

                        recodatorioKm.reck_notificar_sms = False

                        recodatorioKm.veh_id = CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString())
                        clsAlarma.InsertAlertaRecordatorioKm(recodatorioKm)
                    End If

                Next


            End If

            'recorro el check y guardo los moviles
            
            'cargo datos para un recordatorio de fecha
            If txtNombreFecha.Text <> "" Or txtFecha.Text <> "" Or txtDiaMes.Text <> "" Or txtDia.Text <> "" Then

                For Each row As DataListItem In DataListVehiculos.Items
                    Dim rdnMovil As CheckBox = DirectCast(row.FindControl("rdnMovil"), CheckBox)

                    If rdnMovil.Checked Then
                        Dim recodatorioFecha As Alertas_Recordatorios_Por_Fechas = New Alertas_Recordatorios_Por_Fechas()
                        If txtNombreFecha.Text = "" Then
                            lblError.Text = "Para Configurar un Recordatorio para Fecha ingrese una Descripción para el mismo."
                            Exit Sub
                        End If
                        recodatorioFecha.recf_descripcion = txtNombreFecha.Text
                        If rdnFrecuencia.SelectedValue = "dia" Then
                            If txtFecha.Text = "" Then
                                lblError.Text = "Para Configurar un Recordatorio para Fecha para una Fecha especifica, ingrese la Fecha."
                                Exit Sub
                            End If

                            'fecha mayor a la actual
                            If DateTime.Parse(txtFecha.Text + " " + ddlhora.SelectedValue + ":" + ddlmin.SelectedValue + ":00") < DateTime.Now Then
                                lblError.Text = "La Fecha ingresada debe ser mayor a la actual."
                                Exit Sub
                            End If

                            recodatorioFecha.recf_periocidad = "dia"
                            recodatorioFecha.recf_proxima_ocurrencia = CDate(txtFecha.Text + " " + ddlhora.SelectedValue + ":" + ddlmin.SelectedValue + ":00")
                        End If
                        If rdnFrecuencia.SelectedValue = "mes" Then
                            If txtDiaMes.Text = "" Then
                                lblError.Text = "Para Configurar un Recordatorio para Fecha para Todos los Meses, ingrese el Día del Mes."
                                Exit Sub
                            End If
                            recodatorioFecha.recf_periocidad = "mes"

                            Dim fecha As DateTime = CDate(txtDiaMes.Text + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())

                            If (fecha < DateTime.Now) Then
                                recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + DateTime.Now.AddMonths(1).Month.ToString() + "/" + DateTime.Now.Year.ToString())

                            Else
                                recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())

                            End If
                        End If
                        If rdnFrecuencia.SelectedValue = "anio" Then
                            If txtDiaMes.Text = "" Then
                                lblError.Text = "Para Configurar un Recordatorio para Fecha para Todos los Años, ingrese el Día del Mes."
                                Exit Sub
                            End If
                            recodatorioFecha.recf_periocidad = "anio"
                            Dim fecha As DateTime = CDate(txtDiaMes.Text + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString())

                            'si el mes es menor al mes actual le tengo que poner el proximo año
                            If (fecha < DateTime.Now) Then
                                recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + ddlMes.SelectedValue + "/" + DateTime.Now.AddYears(1).Year.ToString())

                            Else
                                recodatorioFecha.recf_proxima_ocurrencia = CDate(txtDiaMes.Text + "/" + ddlMes.SelectedValue + "/" + DateTime.Now.Year.ToString())

                            End If

                        End If

                        recodatorioFecha.recf_notificar_sms = False
                        recodatorioFecha.recf_notificar_mail = chkMailF.Checked
                        recodatorioFecha.veh_id = CInt(DataListVehiculos.DataKeys(row.ItemIndex).ToString())
                        clsAlarma.InsertAlertaRecordatorioFecha(recodatorioFecha)
                    End If
                Next
            End If
            'retorno al listado
            Response.Redirect("~/Panel_Control/pAlarmas.aspx?tab=tabs-2", False)

        Catch ex As Exception

            Funciones.WriteToEventLog("ALARMAS RECORDATORIOS Error al grabar - " + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error Grabando los datos. Contacte al administrador."
        End Try


    End Sub

    Protected Sub valInquiry_ServerValidation(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        '   args.IsValid = chkMoviles.SelectedItem IsNot Nothing
    End Sub

    Protected Sub ddlgrupo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlgrupo.SelectedIndexChanged
        Try
            'cargo lso moviles del grupo
            'si selecciono ver todos los moviles los cargo a todos
            Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivosGrupo(CInt(ddlgrupo.SelectedValue), "")

            If vehiculos.Count > 0 Then
               
                DataListVehiculos.DataSource = vehiculos
                DataListVehiculos.DataBind()

                PanelMoviles.Visible = True
                chkTodos.Checked = False
            Else
                lblError.Text = "No posee Moviles Activos para definir Alarmas. Contacte al Administrador."
            End If
        Catch ex As Exception

        End Try
    End Sub

    'Protected Sub chkTodos_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodos.CheckedChanged
    '    'muestro todos los auots del cliente
    '    If chkTodos.Checked Then
    '        cargarMoviles()
    '        ddlgrupo.SelectedValue = "0"
    '        PanelMoviles.Visible = True
    '    Else
    '        chkMoviles.Items.Clear()
    '    End If

    'End Sub

   

    Protected Sub LinkTildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkTildar.Click
        'marco todos los check de moviles

        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = True
        Next
    End Sub

    Protected Sub LinkDestildar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDestildar.Click
        For Each item As DataListItem In DataListVehiculos.Items

            Dim movil As CheckBox = DirectCast(item.FindControl("rdnMovil"), CheckBox)
            movil.Checked = False
        Next
    End Sub
End Class