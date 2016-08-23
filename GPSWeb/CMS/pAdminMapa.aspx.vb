'Pagina para seguir la ubicacion de los moviles de un cliente
Imports GPS.Business
Imports GPS.Data
Imports System.Web.Services
Imports System.Text
Imports System.Net
Imports System.Xml
Imports System.IO
Partial Public Class pAdminMapa1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then
                    'muestro el listado de clientes
                    Dim listClientes As List(Of Cliente) = clsCliente.ListActivos
                    Dim item As ListItem = New ListItem("- Seleccione Cliente -", "0")
                    ddCliente.Items.Insert(0, item)

                    For Each cliente As Cliente In listClientes
                        item = New ListItem(cliente.cli_apellido + " " + cliente.cli_nombre, cliente.cli_id.ToString)
                        ddCliente.Items.Add(item)
                    Next

                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos - " + ex.Message
        End Try


    End Sub


    Protected Sub ddCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddCliente.SelectedIndexChanged
        'busco la ubicacion de los vehiculos del cliente seleccionado
        Try
            hdncli_id.Value = ddCliente.SelectedValue
            GetAlarmas()
            'ejecuto el jquery
            ' Page.ClientScript.RegisterStartupScript(Me.GetType(), "funcionJq", "mostrarVehiculos(0);", True)
            ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionJq", "mostrarVehiculos(0);", True)
        Catch ex As Exception
            lblError.Text = "Ocurrio un error buscando lso datos del cliente - " + ex.Message
        End Try
    End Sub

    Private Sub GetAlarmas()
        Dim dt As DataTable = New DataTable()
        If hdncli_id.Value = "" Then hdncli_id.Value = 0
        Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertas(CInt(hdncli_id.Value), 0, DateTime.Now.Date.AddDays(-1), DateTime.Now.Date.AddDays(1), "")

        If alarmas.Count > 0 Then
            dt.Columns.Add("Alarma")
            dt.Columns.Add("Categoria")
            dt.Columns.Add("Patente")
            dt.Columns.Add("Fecha", GetType(Date))
            dt.Columns.Add("Hora", GetType(TimeSpan))
            dt.Columns.Add("alar_id", GetType(Integer))

            For Each d As Alarmas In alarmas

                Dim dr As DataRow = dt.NewRow()
                dr(0) = d.alar_nombre
                dr(1) = d.alar_Categoria

                dr(2) = d.veh_patente
                dr(3) = d.alar_fecha
                dr(4) = d.alar_fecha.ToString("HH:mm:ss")

                dr(5) = d.alar_id

                dt.Rows.Add(dr)
            Next

        End If

        ' datagridAlertas.DataSource = dt
        'datagridAlertas.DataBind()


    End Sub

    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick
        'ultimas alarmas
        ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "searchVehiculos();", True)
        ' GetAlarmas()
    End Sub

    'funcion que llama el jquery para armar el listado de moviles y sus datos de ubicacion, aplica filtro sobre todos los campos
    <WebMethod()> _
 Public Shared Function MostarVehiculos(ByVal cli_id As Integer, ByVal filtro As String) As String
        Dim sTabla As StringBuilder = New StringBuilder()
        Dim vehiculos As List(Of Vehiculo)

        ' If getSession("vehiculos") IsNot Nothing Then
        'vehiculos = DirectCast(getSession("vehiculos"), List(Of Vehiculos))
        ' 'tengo que filtrar sobre la session
        'Else
        
        vehiculos = clsVehiculo.ListActivos(cli_id, filtro)

        'End If

        Dim ubicacion As vMonitoreo = New vMonitoreo()
  sTabla.AppendLine("<ul id='navigation'>")
        Try
            If vehiculos.Count = 0 Then
                sTabla.AppendLine("<tr><td>No se encontraron Datos para este Cliente</td></tr>")
            Else
                Dim minutos_rojo As Integer = CInt(clsParametros.ParametroSelect("No_reporta_rojo").par_valor)
                Dim minutos_amarillo As Integer = CInt(clsParametros.ParametroSelect("No_reporta_amarillo").par_valor)
                Dim estado = ""
                Dim onOff = "on.png"
                Dim ocupado = "<span style='font-weight:bold;color:Red;' >OCUPADO</span>"

                For Each movil As Vehiculo In vehiculos
                    ubicacion = clsVehiculo.searchLastLocation(movil.mod_id)

                    sTabla.AppendLine("<li>")
                    If ubicacion IsNot Nothing Then


                        estado = "autito_verde.gif"
                        'si tiene mas de un dia sin reportar lo pongo en rojo
                        If (DateTime.Now - ubicacion.FECHA).TotalDays > 1 Then
                            estado = "autito_rojo.gif" 'rojo

                        Else
                            If (DateTime.Now - ubicacion.FECHA).TotalMinutes > minutos_rojo Then
                                estado = "autito_rojo.gif" 'rojo
                            Else
                                If (DateTime.Now - ubicacion.FECHA).TotalMinutes > minutos_amarillo Then
                                    estado = "autito_amarillo.gif" 'amarillo
                                End If
                            End If
                        End If

                        If ubicacion.ENCENDIDO Then
                            onOff = "on.png"
                        Else
                            onOff = "off.jpeg"
                        End If

                        If ubicacion.OCUPADO Then
                            ocupado = "<span style='font-weight:bold;color:Red;' >OCUPADO</span>"
                        Else
                            ocupado = "<span style='font-weight:bold;color:#00CC00;' >LIBRE</span>"
                        End If

                        sTabla.AppendLine("<img alt='auto'  src='../images/iconos_movil/" & estado & "' style='border-width:0px;' />")
                        sTabla.AppendLine("<img alt='OnOff'  src='../images/iconos_movil/" & onOff & "' style='border-width:0px;' />")
                        sTabla.AppendLine("<a onclick='javascript:mostrarVehiculos(" + movil.veh_id.ToString() + ");' href='#v" & movil.veh_id.ToString() & "' title='Mostrar Ubicación' style='text-decoration:none; font-weight:bold; color:Black;'>") 'llamo a la funcion que busca la ubicacion pero solo para un movil
                        sTabla.AppendLine("<span>" + movil.veh_descripcion + "</span>-")
                        sTabla.AppendLine("<span>" + movil.veh_patente + "</span>      ")
                        sTabla.AppendLine("</a>")
                        sTabla.AppendLine("<a title='Alarmas' href='pAdminAlarmas.aspx?veh_id=" + movil.veh_id.ToString() & "'><img alt='Alarmas' src='../images/icoWarning.png' style='border-width:0px;' /></a>")
                        sTabla.AppendLine("<a title='Seguimiento' href='pAdminSeguimiento.aspx?veh_id=" + movil.veh_id.ToString() & "'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>")

                        sTabla.AppendLine("<ul><li><div>" & ocupado & "<span>-" & movil.veh_nombre_conductor & "</span><br />")

                        sTabla.AppendLine("<span>Ultimo Reporte: " & ubicacion.FECHA.ToString("dd/MM/yyyy") & "-" & ubicacion.FECHA.ToString("HH:mm:ss") & "</span><br />")
                        sTabla.AppendLine("<span>" & ubicacion.VELOCIDAD.ToString & " Kms/H - " & "RPM: " & ubicacion.RPM & " - Temp: " & ubicacion.TEMP & " - Bat:" & ubicacion.BATERIA & " </span><br/>")
                        sTabla.AppendLine(" <span>" & ubicacion.PROVINCIA + "</span>")
                        sTabla.AppendLine(" <span>," & ubicacion.LOCALIDAD + "</span>")
                        sTabla.AppendLine("<span>," & ubicacion.NOMBRE_VIA + "</span>")


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

                        sTabla.AppendLine("</li></ul>")
                    Else

                        sTabla.AppendLine("<img alt='auto'  src='../images/iconos_movil/autito_gris.gif' style='border-width:0px;' />")
                        sTabla.AppendLine("<span>" + movil.veh_descripcion + "</span>-")
                        sTabla.AppendLine("<span>" + movil.veh_patente + "</span>")
                        sTabla.AppendLine("<a title='Reportes' href='pReportes.aspx?veh_id=" + movil.veh_id.ToString() + "'><img alt='Reporte'  src='../images/report_ico.png' style='border-width:0px;' /></a>")
                        sTabla.AppendLine("<a title='Alarmas' href='pAlarmas.aspx?veh_id=" + movil.veh_id.ToString() + "'><img alt='Alarmas' src='../images/icoWarning.png' style='border-width:0px;' /></a>")
                        sTabla.AppendLine("<a title='Recorridos' href='pHistorialRecorridos.aspx?veh_id=" + movil.veh_id.ToString() + "'><img alt='Recorrido' src='../images/recorrido.jpg' style='border-width:0px;' /></a>")
                        sTabla.AppendLine("<a title='Seguimiento' href='pSeguimientos.aspx?veh_id=" + movil.veh_id.ToString() + "'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>")

                        sTabla.AppendLine("<ul><li><span>" & movil.veh_nombre_conductor & "</span><br />")
                        sTabla.AppendLine("<span>Sin Datos de Ubicación</span><br/>")
                        sTabla.AppendLine("</li></ul>")
                    End If

                    sTabla.AppendLine("</li>")

                Next
            End If
         

        Catch ex As Exception
            sTabla.AppendLine("<li>Ocurrio un error recuperando los datos. - " + ex.Message + "</li>")
        End Try

       sTabla.AppendLine("</ul>")
        sTabla.AppendLine("<div><br /><a href='#' onclick='mostrarVehiculos(0);' style='color:#ccc000; font-size:12px;'>Ver todos los moviles de la empresa en el mapa</a></div>")
        Return sTabla.ToString()

    End Function

End Class