Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.IO
Partial Public Class pAdminParametros
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'cargo el listado de categorias
        'cargo el listado de alarmas por defecto la primer categoria
        Dim veh_id As Integer = 0
        Try
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
               

                If Not IsPostBack Then

                    'muestro el listadod e tipos de clientes por defecto
                    lblTabla.Text = " Tipos de Clientes"
                    datagridTipoCliente.DataSource = clsParametros.TipoClienteList()
                    datagridTipoCliente.DataBind()
                    hdnTipo.Value = "Clientes"
                End If

            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If


        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador. - " + ex.Message
            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub lbtnTClientes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnTClientes.Click
        'muestor la grilla de tipos de clientes
        lblTabla.Text = " Tipos de Clientes"
        datagridTipoCliente.DataSource = clsParametros.TipoClienteList()
        datagridTipoCliente.DataBind()

        datagridTipoCliente.Visible = True
        GridViewMarcador.Visible = False
        GridViewVehiculo.Visible = False
        GridViewUsos.Visible = False
        GridViewVias.Visible = False

        hdnTipo.Value = "Clientes"
    End Sub

    Protected Sub btnFinalizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinalizar.Click
        Try
            'guardo la modificacion o alta del parametro.
            Select Case hdnTipo.Value
                Case "Clientes"
                    If hdnAccion.Value = "agregar" Then
                        Dim cliente As Tipos_Clientes = New Tipos_Clientes()
                        cliente.tipo_cli_nombre = txtValor.Text
                        clsParametros.TipoClienteInsert(cliente)
                        txtValor.Text = ""
                    Else
                        Dim cliente As Tipos_Clientes = clsParametros.TipoClienteSelect(CInt(hdnId.Value))
                        cliente.tipo_cli_nombre = txtValor.Text
                        clsParametros.TipoClienteUpdate(cliente)
                        txtValor.Text = ""
                    End If
                    datagridTipoCliente.DataSource = clsParametros.TipoClienteList()
                    datagridTipoCliente.DataBind()

                Case "Marcadores"
                    If hdnAccion.Value = "agregar" Then
                        Dim marcador As Tipos_Marcadores = New Tipos_Marcadores()
                        marcador.tipo_marc_nombre = txtValor.Text
                        marcador.tipo_marc_imagen = ""
                        'upload de la imagen elegida

                        If FileUpload1.HasFile Then
                            FileUpload1.PostedFile.SaveAs(Server.MapPath("~") + "\images\Marcadores\" + Path.GetFileName(FileUpload1.FileName))
                            marcador.tipo_marc_imagen = "marcadores/" + Path.GetFileName(FileUpload1.FileName)
                        End If
                        clsParametros.TipoMarcadorInsert(marcador)
                    Else
                        Dim marcador As Tipos_Marcadores = clsParametros.TipoMarcadorSelect(CInt(hdnId.Value))
                        marcador.tipo_marc_nombre = txtValor.Text
                         'upload de la imagen elegida

                        If FileUpload1.HasFile Then
                            FileUpload1.PostedFile.SaveAs(Server.MapPath("~") + "\images\Marcadores\" + Path.GetFileName(FileUpload1.FileName))
                            marcador.tipo_marc_imagen = "marcadores/" + Path.GetFileName(FileUpload1.FileName)
                        End If
                        clsParametros.TipoMarcadorUpdate(marcador)

                    End If
                    txtValor.Text = ""
                    Image1.ImageUrl = ""
                    GridViewMarcador.DataSource = clsMarcador.ListTiposMarcador()
                    GridViewMarcador.DataBind()

                Case "Vehiculos"
                    If hdnAccion.Value = "agregar" Then
                        Dim movil As Tipos_Vehiculos = New Tipos_Vehiculos()
                        movil.veh_tipo_detalle = txtValor.Text
                        If FileUpload1.HasFile Then
                            FileUpload1.PostedFile.SaveAs(Server.MapPath("~") + "\images\iconos_movil\" + Path.GetFileName(FileUpload1.FileName))
                            movil.veh_tipo_icono = "iconos_movil/" + Path.GetFileName(FileUpload1.FileName)
                        End If
                        clsParametros.TipoVehiculoInsert(movil)
                        txtValor.Text = ""
                    Else
                        Dim movil As Tipos_Vehiculos = clsParametros.TipoVehiculoSelect(CInt(hdnId.Value))
                        movil.veh_tipo_detalle = txtValor.Text
                        
                        If FileUpload1.HasFile Then
                            FileUpload1.PostedFile.SaveAs(Server.MapPath("~") + "\images\iconos_movil\" + Path.GetFileName(FileUpload1.FileName))
                            movil.veh_tipo_icono = "iconos_movil/" + Path.GetFileName(FileUpload1.FileName)
                        End If

                        clsParametros.TipoVehiculoUpdate(movil)
                        txtValor.Text = ""
                    End If
                    GridViewVehiculo.DataSource = clsVehiculo.ListTipoVehiculo()
                    GridViewVehiculo.DataBind()

                Case "Usos"
                    If hdnAccion.Value = "agregar" Then
                        Dim movil As Tipos_Usos_Moviles = New Tipos_Usos_Moviles()
                        movil.tipo_uso_descripcion = txtValor.Text
                        clsParametros.TipoUsoInsert(movil)
                        txtValor.Text = ""
                    Else
                        Dim movil As Tipos_Usos_Moviles = clsParametros.TipoUsoSelect(CInt(hdnId.Value))
                        movil.tipo_uso_descripcion = txtValor.Text
                        clsParametros.TipoUsoUpdate(movil)
                        txtValor.Text = ""
                    End If
                    GridViewUsos.DataSource = clsVehiculo.ListTipoUso()
                    GridViewUsos.DataBind()

                Case "Vias"
                    If hdnAccion.Value = "agregar" Then
                        Dim via As Tipos_Vias = New Tipos_Vias()
                        via.tipo_via_nombre = txtValor.Text
                        clsParametros.TipoViaInsert(via)
                        txtValor.Text = ""
                    Else
                        Dim via As Tipos_Vias = clsParametros.TipoViaSelect(CInt(hdnId.Value))
                        via.tipo_via_nombre = txtValor.Text
                        clsParametros.TipoViaUpdate(via)
                        txtValor.Text = ""
                    End If
                    GridViewVias.DataSource = clsParametros.TipoViaList()
                    GridViewVias.DataBind()
                Case "Tiempos"
                    
                    Dim tiempo As Parametros = clsParametros.ParametroSelectbyId(CInt(hdnId.Value))
                    tiempo.par_valor = txtValor.Text
                    clsParametros.Update(tiempo)
                        txtValor.Text = ""

                    GridViewTiempos.DataSource = clsParametros.TiemposEsperaList()
                    GridViewTiempos.DataBind()
                Case "Parametro"

                    Dim tiempo As Parametros = clsParametros.ParametroSelectbyId(CInt(hdnId.Value))
                    tiempo.par_valor = txtValor.Text
                    clsParametros.Update(tiempo)
                    txtValor.Text = ""

                    GridViewParametro.DataSource = clsParametros.ParametrosList()
                    GridViewParametro.DataBind()
            End Select
        Catch ex As Exception
            lblError.Text = "Ocurrio un error grabando los datos. - " + ex.Message
            Funciones.WriteToEventLog("ADMIN PARAMETROS - Grabar Datos: " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub lbtnTMarcadores_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnTMarcadores.Click
        'muestro la grilla de tipos de marcadores
        lblTabla.Text = " Tipos de Marcadores"
        GridViewMarcador.DataSource = clsMarcador.ListTiposMarcador()
        GridViewMarcador.DataBind()

        GridViewMarcador.Visible = True
        datagridTipoCliente.Visible = False
        GridViewVehiculo.Visible = False
        GridViewUsos.Visible = False
        GridViewVias.Visible = False
        GridViewTiempos.Visible = False
        GridViewParametro.Visible = False
        Button1.Visible = True
        hdnTipo.Value = "Marcadores"
    End Sub

    Protected Sub lbtnTVehiculos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnTVehiculos.Click
        'muestro la grilla de tipos de vehiculos
        lblTabla.Text = " Tipos de Vehiculos"
        GridViewVehiculo.DataSource = clsVehiculo.ListTipoVehiculo()
        GridViewVehiculo.DataBind()

        GridViewVehiculo.Visible = True
        datagridTipoCliente.Visible = False
        GridViewMarcador.Visible = False
        GridViewUsos.Visible = False
        GridViewVias.Visible = False
        GridViewTiempos.Visible = False
        GridViewParametro.Visible = False
        Button1.Visible = True
        hdnTipo.Value = "Vehiculos"
    End Sub

    Protected Sub lbtnTUsos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnTUsos.Click
        'muestro la grilla de tipos de uso de vehiculos
        lblTabla.Text = " Tipos de Usos de Vehiculos"
        GridViewUsos.DataSource = clsVehiculo.ListTipoUso()
        GridViewUsos.DataBind()

        GridViewUsos.Visible = True
        datagridTipoCliente.Visible = False
        GridViewMarcador.Visible = False
        GridViewVehiculo.Visible = False
        GridViewVias.Visible = False
        GridViewTiempos.Visible = False
        GridViewParametro.Visible = False
        Button1.Visible = True
        hdnTipo.Value = "Usos"
    End Sub

    Protected Sub lbtnTVias_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnTVias.Click
        'muestro la grilla de tipos de Vias
        lblTabla.Text = " Tipos de Vias"
        GridViewVias.DataSource = clsParametros.TipoViaList()
        GridViewVias.DataBind()

        GridViewVias.Visible = True
        datagridTipoCliente.Visible = False
        GridViewMarcador.Visible = False
        GridViewVehiculo.Visible = False
        GridViewUsos.Visible = False
        GridViewTiempos.Visible = False
        GridViewParametro.Visible = False
        Button1.Visible = True
        hdnTipo.Value = "Vias"
    End Sub

    Protected Sub lbtnEspera_Click(sender As Object, e As EventArgs) Handles lbtnEspera.Click
        lblTabla.Text = " Tiempos de Espera"
        GridViewTiempos.DataSource = clsParametros.TiemposEsperaList()
        GridViewTiempos.DataBind()

        GridViewTiempos.Visible = True
        GridViewParametro.Visible = False
        datagridTipoCliente.Visible = False
        GridViewMarcador.Visible = False
        GridViewVehiculo.Visible = False
        GridViewUsos.Visible = False
        GridViewVias.Visible = False
        hdnTipo.Value = "Tiempos"
        Button1.Visible = False
    End Sub

    Protected Sub GridViewTiempos_Command(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Editar" Then
                'levanto el pop up con los datos para editar
                Dim tiempo As Parametros = clsParametros.ParametroSelectbyId(CInt(e.CommandArgument))
                txtValor.Text = tiempo.par_valor
                hdnId.Value = tiempo.par_id.ToString
                lblPrametro.Text = tiempo.par_nombre
                lblTablam.Text = "Modificar Tiempos Espera"
                hdnAccion.Value = "editar"
                PanelFile.Visible = False
                mpeEditar.Show()
            End If

           
        Catch ex As Exception
            If ex.Message.Contains("Instrucción DELETE en conflicto con la restricción REFERENCE") Then
                lblError.Text = "No puede eliminar el regitro ya que esta relacionado con otros Datos."
            Else
                lblError.Text = "Ocurrio un error cargando los datos. - " + ex.Message
            End If
            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try

    End Sub

    Protected Sub GridViewParametro_Command(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Editar" Then
                'levanto el pop up con los datos para editar
                Dim tiempo As Parametros = clsParametros.ParametroSelectbyId(CInt(e.CommandArgument))
                txtValor.Text = tiempo.par_valor
                hdnId.Value = tiempo.par_id.ToString
                lblPrametro.Text = tiempo.par_nombre
                lblTablam.Text = "Modificar Parametro"
                hdnAccion.Value = "editar"
                PanelFile.Visible = False
                mpeEditar.Show()
            End If


        Catch ex As Exception
            If ex.Message.Contains("Instrucción DELETE en conflicto con la restricción REFERENCE") Then
                lblError.Text = "No puede eliminar el regitro ya que esta relacionado con otros Datos."
            Else
                lblError.Text = "Ocurrio un error cargando los datos. - " + ex.Message
            End If
            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try

    End Sub

    Protected Sub datagridTipoCliente_Command(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Editar" Then
                'levanto el pop up con los datos para editar
                Dim cliente As Tipos_Clientes = clsParametros.TipoClienteSelect(CInt(e.CommandArgument))
                txtValor.Text = cliente.tipo_cli_nombre
                hdnId.Value = cliente.tipo_cli_id.ToString
                lblTablam.Text = "Modificar Tipo de Cliente"
                hdnAccion.Value = "editar"
                PanelFile.Visible = False
                mpeEditar.Show()
            End If

            If e.CommandName = "Borrar" Then
                'elimino el registro
                clsParametros.TipoClienteDelete(CInt(e.CommandArgument))
                datagridTipoCliente.DataSource = clsParametros.TipoClienteList()
                datagridTipoCliente.DataBind()

            End If
        Catch ex As Exception
            If ex.Message.Contains("Instrucción DELETE en conflicto con la restricción REFERENCE") Then
                lblError.Text = "No puede eliminar el regitro ya que esta relacionado con otros Datos."
            Else
                lblError.Text = "Ocurrio un error cargando los datos. - " + ex.Message
            End If
            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try

    End Sub

    Protected Sub GridViewMarcador_Command(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Editar" Then
                'levanto el pop up con los datos para editar
                Dim marcador As Tipos_Marcadores = clsParametros.TipoMarcadorSelect(CInt(e.CommandArgument))
                txtValor.Text = marcador.tipo_marc_nombre
                hdnId.Value = marcador.tipo_marc_id.ToString
                Image1.ImageUrl = "../images/" + marcador.tipo_marc_imagen
                lblTablam.Text = "Modificar Tipo de Marcador"
                hdnAccion.Value = "editar"
                PanelFile.Visible = True

                mpeEditar.Show()
            End If

            If e.CommandName = "Borrar" Then
                'elimino el registro
                clsParametros.TipoMarcadorDelete(CInt(e.CommandArgument))
                GridViewMarcador.DataSource = clsMarcador.ListTiposMarcador()
                GridViewMarcador.DataBind()
            End If
        Catch ex As Exception
            If ex.Message.Contains("Instrucción DELETE en conflicto con la restricción REFERENCE") Then
                lblError.Text = "No puede eliminar el regitro ya que esta relacionado con otros Datos."
            Else
                lblError.Text = "Ocurrio un error cargando los datos. - " + ex.Message
            End If
            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try

    End Sub

    Protected Sub GridViewVehiculo_Command(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Editar" Then
                'levanto el pop up con los datos para editar
                Dim vehiculo As Tipos_Vehiculos = clsParametros.TipoVehiculoSelect(CInt(e.CommandArgument))
                txtValor.Text = vehiculo.veh_tipo_detalle
                hdnId.Value = vehiculo.veh_tipo_id.ToString
                lblTablam.Text = "Modificar Tipo de Vehiculo"
                hdnAccion.Value = "editar"
                Image1.ImageUrl = "../images/" + vehiculo.veh_tipo_icono
                PanelFile.Visible = True
                mpeEditar.Show()
            End If

            If e.CommandName = "Borrar" Then
                'elimino el registro
                clsParametros.TipoVehiculoDelete(CInt(e.CommandArgument))
                GridViewVehiculo.DataSource = clsVehiculo.ListTipoVehiculo()
                GridViewVehiculo.DataBind()
            End If
        Catch ex As Exception
            'verifico error de integridad , no puede borrar registros que estan relacionados con otros
            If ex.Message.Contains("Instrucción DELETE en conflicto con la restricción REFERENCE") Then
                lblError.Text = "No puede eliminar el regitro ya que esta relacionado con otros Datos."
            Else
                lblError.Text = "Ocurrio un error cargando los datos. - " + ex.Message
            End If

            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try

    End Sub
    Protected Sub GridViewUsos_Command(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Editar" Then
                'levanto el pop up con los datos para editar
                Dim uso As Tipos_Usos_Moviles = clsParametros.TipoUsoSelect(CInt(e.CommandArgument))
                txtValor.Text = uso.tipo_uso_descripcion
                hdnId.Value = uso.tipo_uso_id.ToString
                lblTablam.Text = "Modificar Tipo de Usos de Vehiculos"
                hdnAccion.Value = "editar"
                PanelFile.Visible = False
                mpeEditar.Show()
            End If

            If e.CommandName = "Borrar" Then
                'elimino el registro
                clsParametros.TipoUsoDelete(CInt(e.CommandArgument))
                GridViewUsos.DataSource = clsVehiculo.ListTipoUso()
                GridViewUsos.DataBind()
            End If
        Catch ex As Exception
            If ex.Message.Contains("Instrucción DELETE en conflicto con la restricción REFERENCE") Then
                lblError.Text = "No puede eliminar el regitro ya que esta relacionado con otros Datos."
            Else
                lblError.Text = "Ocurrio un error cargando los datos. - " + ex.Message
            End If
            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try

    End Sub

    Protected Sub GridViewVias_Command(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Editar" Then
                'levanto el pop up con los datos para editar
                Dim via As Tipos_Vias = clsParametros.TipoViaSelect(CInt(e.CommandArgument))
                txtValor.Text = via.tipo_via_nombre
                hdnId.Value = via.tipo_via_id.ToString
                lblTablam.Text = "Modificar Tipo de Vía"
                hdnAccion.Value = "editar"
                PanelFile.Visible = False
                mpeEditar.Show()
            End If

            If e.CommandName = "Borrar" Then
                'elimino el registro
                clsParametros.TipoViaDelete(CInt(e.CommandArgument))
                GridViewVias.DataSource = clsParametros.TipoViaList()
                GridViewVias.DataBind()
            End If
        Catch ex As Exception
            If ex.Message.Contains("Instrucción DELETE en conflicto con la restricción REFERENCE") Then
                lblError.Text = "No puede eliminar el regitro ya que esta relacionado con otros Datos."
            Else
                lblError.Text = "Ocurrio un error cargando los datos. - " + ex.Message
            End If
            Funciones.WriteToEventLog("ADMIN PARAMETROS - " + ex.Message + " - " + ex.StackTrace)
        End Try

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        'levanto el pop up para agregar
        hdnAccion.Value = "agregar"
        Select Case hdnTipo.Value
            Case "Clientes"
                lblTablam.Text = "Agregar Tipo de Cliente"

                PanelFile.Visible = False
                mpeEditar.Show()
            Case "Marcadores"
                lblTablam.Text = "Agregar Tipo de Marcador"
                PanelFile.Visible = True
                mpeEditar.Show()
            Case "Vehiculos"
                lblTablam.Text = "Agregar Tipo de Vehiculo"
                PanelFile.Visible = False
                mpeEditar.Show()
            Case "Usos"
                lblTablam.Text = "Agregar Tipo de Uso de Vehiculo"
                PanelFile.Visible = False
                mpeEditar.Show()
            Case "Vias"
                lblTablam.Text = "Agregar Tipo de Vía"
                PanelFile.Visible = False
                mpeEditar.Show()
        End Select

    End Sub

   
    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        lblTabla.Text = " Parametros del Sistema"
        GridViewParametro.DataSource = clsParametros.ParametrosList()
        GridViewParametro.DataBind()

        GridViewParametro.Visible = True
        GridViewTiempos.Visible = False
        datagridTipoCliente.Visible = False
        GridViewMarcador.Visible = False
        GridViewVehiculo.Visible = False
        GridViewUsos.Visible = False
        GridViewVias.Visible = False
        hdnTipo.Value = "Parametro"
        Button1.Visible = False
    End Sub
End Class