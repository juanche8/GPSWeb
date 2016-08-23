'Pagina para configurar las alarmas que quiere recibir el cliente.
Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

Partial Public Class pAdminAlarmas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'cargo el listado de categorias
        'cargo el listado de alarmas por defecto la primer categoria
        Try
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then

                    hdncli_id.Value = DirectCast(Session("Cliente"), Integer).ToString()

                    'Dim alarmas As List(Of Categorias_Alarmas) = clsCategoriaAlarma.List()
                    'DataListCategorias.DataSource = alarmas
                    'DataListCategorias.DataBind()

                    'lblCategoria.Text = alarmas(0).cat_descripcion
                    ''busco las alertas configuradas para esta categoria
                    'GetAlarmasGenericas(alarmas(0).cat_id)


                    'categorias propias del cliente - la categoria servicios es reemplazada por estas alarmas
                    'donde el cleinte puede especificar lo que va a controlar, km iniciales y cada cuantos km se dispara
                   
                    'verifico si retorna de la configuracion de recorridos, zonas o direcciones para mostrar la grilla
                    If Request.Params("tipo_retorno") IsNot Nothing Then
                        If Request.Params("tipo_retorno").ToString() = "recorrido" Then
                            mostrarListados("4")
                        End If

                        If Request.Params("tipo_retorno").ToString() = "direccion" Then
                            mostrarListados("6")
                        End If

                        If Request.Params("tipo_retorno").ToString() = "zona" Then
                            mostrarListados("5")
                        End If

                    End If

                End If

            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

   

  

    'filtro las alarmas por categoria
    Protected Sub DataListCategorias_Command(ByVal source As Object, ByVal e As DataListCommandEventArgs)
        If e.CommandName = "Filtrar" Then
            lblCategoria.Text = DirectCast(e.Item.FindControl("lbtnCategoria"), LinkButton).Text

            mostrarListados(DataListCategorias.DataKeys(e.Item.ItemIndex).ToString())

        End If
    End Sub

    Private Sub GetAlarmasGenericas(ByVal cat_id)
        'Dim dt As DataTable = New DataTable()
        'Dim alarmas As List(Of Alertas_Configuradas) = clsCategoriaAlarma.ListConfiguradas(cat_id)

        'hdncat_id.Value = cat_id.ToString

        'If alarmas.Count > 0 Then
        '    dt.Columns.Add("movil")
        '    dt.Columns.Add("alarma")
        '    dt.Columns.Add("valor")
        '    dt.Columns.Add("enviar_mail")
        '    dt.Columns.Add("enviar_sms")
        '    dt.Columns.Add("ale_id", GetType(Integer))
        '    dt.Columns.Add("ultimo_cambio")

        '    For Each d As Alertas_Configuradas In alarmas

        '        Dim dr As DataRow = dt.NewRow()
        '        dr(0) = d.Vehiculos.veh_descripcion + "-" + d.Vehiculos.veh_patente

        '        If d.SubCategorias_Alarmas.cat_id = 2 Then
        '            dr(1) = d.SubCategorias_Alarmas.Categorias_Alarmas.cat_descripcion

        '        Else
        '            dr(1) = d.SubCategorias_Alarmas.Categorias_Alarmas.cat_descripcion + " - " + d.SubCategorias_Alarmas.subcat_descripcion

        '        End If

        '        dr(2) = d.ale_valor_maximo.ToString
        '        dr(3) = d.ale_enviar_mail
        '        dr(4) = d.ale_enviar_SMS

        '        dr(5) = d.ale_id
        '        dr(6) = d.ale_ultimo_cambio

        '        dt.Rows.Add(dr)
        '    Next

        'End If

        'GridViewGenericas.DataSource = dt
        'HideShowColumns(cat_id)
        'GridViewGenericas.DataBind()




    End Sub

    Private Sub mostrarListados(ByVal categoria As String)
        Try
            'si no tiene subcategorias tengo que ver si es alarto de recorridos, zonas o de direcciones y muestro el panel correspondiente
            'tomo los ids en duro
            If categoria = "4" Then 'desvios de recorridos
                PanelAlarmasGenericas.Visible = False
                PanelRecorridos.Visible = True
                PanelDirecciones.Visible = False
                PanelZona.Visible = False
                PanelAlarmasCliente.Visible = False

                datagridRecorridos.DataSource = clsAlarma.AlertaRecorridoList(CInt(hdncli_id.Value))
                datagridRecorridos.DataBind()
            Else
                If categoria = "6" Then 'entradas y salidas de direcciones
                    PanelAlarmasGenericas.Visible = False
                    PanelRecorridos.Visible = False
                    PanelDirecciones.Visible = True
                    PanelZona.Visible = False
                    PanelAlarmasCliente.Visible = False

                    GridViewDirecciones.DataSource = clsAlarma.AlertaDireccionList(CInt(hdncli_id.Value))
                    GridViewDirecciones.DataBind()
                Else
                    If categoria = "5" Then 'entradas y salidas de zonas
                        PanelAlarmasGenericas.Visible = False
                        PanelRecorridos.Visible = False
                        PanelDirecciones.Visible = False
                        PanelZona.Visible = True
                        PanelAlarmasCliente.Visible = False

                        GridViewZonas.DataSource = clsAlarma.AlertaZonaList(CInt(hdncli_id.Value))
                        GridViewZonas.DataBind()
                    Else
                        PanelAlarmasGenericas.Visible = True
                        PanelRecorridos.Visible = False
                        PanelDirecciones.Visible = False
                        PanelZona.Visible = False
                        PanelAlarmasCliente.Visible = False

                        GetAlarmasGenericas(CInt(categoria))

                    End If
                End If
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
     
    End Sub
    Protected Sub datagridGenerica_RowCommand(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Borrar" Then
                clsAlarma.DeleteById(CInt(e.CommandArgument))
                GetAlarmasGenericas(CInt(hdncat_id.Value))
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
        

    End Sub

    Protected Sub datagridRecorridos_RowCommand(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Borrar" Then
                clsAlarma.DeleteAlerta_Recorrido(CInt(e.CommandArgument))
                datagridRecorridos.DataSource = clsAlarma.AlertaRecorridoList(CInt(hdncli_id.Value))
                datagridRecorridos.DataBind()
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub


    Protected Sub GridViewDirecciones_RowCommand(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Borrar" Then
                clsAlarma.DeleteAlerta_Direccion(CInt(e.CommandArgument))
                GridViewDirecciones.DataSource = clsAlarma.AlertaDireccionList(CInt(hdncli_id.Value))
                GridViewDirecciones.DataBind()
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub GridViewZonas_RowCommand(ByVal source As Object, ByVal e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "Borrar" Then
                clsAlarma.DeleteAlerta_Zona(CInt(e.CommandArgument))
                GridViewZonas.DataSource = clsAlarma.AlertaZonaList(CInt(hdncli_id.Value))
                GridViewZonas.DataBind()
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub DataListCategoriaCliente_Command(ByVal source As Object, ByVal e As DataListCommandEventArgs)
        Try
            If e.CommandName = "Filtrar" Then

                'lblCategoria.Text = DirectCast(e.Item.FindControl("lbtnCategoria"), LinkButton).Text

                ''busco las alarmas configuradas para esta categoria propia del cliente
                'GridViewAlarmasCliente.DataSource = clsAlarma.AlertaClienteList(CInt(e.CommandArgument))
                'GridViewAlarmasCliente.DataBind()

                'PanelAlarmasGenericas.Visible = False
                'PanelRecorridos.Visible = False
                'PanelDirecciones.Visible = False
                'PanelZona.Visible = False
                'PanelAlarmasCliente.Visible = True
            End If
        Catch ex As Exception
            Funciones.WriteToEventLog("Admin ALARMAS - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub btnNueva_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNueva.Click
        Response.Redirect("pAlarmaAdd.aspx?cat_id=" + hdncat_id.Value, False)
    End Sub

    Private Sub HideShowColumns(ByVal cat_id As Integer)
        If cat_id = 2 Then
            GridViewGenericas.Columns(2).Visible = False
            GridViewGenericas.Columns(3).Visible = False
        Else
            If cat_id = 1 Then
                GridViewGenericas.Columns(2).Visible = False
                GridViewGenericas.Columns(3).Visible = True
            Else
                GridViewGenericas.Columns(2).Visible = True
                GridViewGenericas.Columns(3).Visible = True
            End If
            
        End If


    End Sub

End Class