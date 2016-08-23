'Pagina que para hacer un seguimiento de los moviles del cliente
Imports GPS.Business
Imports GPS.Data
Imports System.Linq

Partial Public Class pAdminMapa
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'cargo el combo de tipo de vehiculos

        Dim listClientes As List(Of Cliente) = clsCliente.List
        Dim item As ListItem = New ListItem("- Seleccione Cliente -", "0")
        ddlCliente.Items.Insert(0, item)

        For Each cliente As Cliente In listClientes
            item = New ListItem(cliente.cli_apellido + " " + cliente.cli_nombre, cliente.cli_id.ToString)
            ddlCliente.Items.Add(item)

        Next

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'muestro los moviles del cliente
            If Session("Admin") Is Nothing Then
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            Else
                'verifico si tengo por parametro el id del movil

                'verifico si filtra desde el mapa por codigo del vehiculo
                If Request.Params("veh_id") IsNot Nothing Then
                    Dim _movil As Vehiculo = clsVehiculo.Seleccionar(CInt(Request.Params("veh_id").ToString()))
                    ddlCliente.SelectedValue = _movil.cli_id
                    ddlCliente_SelectedIndexChanged(sender, e)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        'busco los vehiculos del cliente y cargo la lista de checks
        DataListVehiculos.DataSource = clsVehiculo.List(CInt(ddlCliente.SelectedValue))
        DataListVehiculos.DataBind()
        hdncli_id.Value = ddlCliente.SelectedValue
        panelMoviles.Visible = True

        ' ScriptManager.RegisterStartupScript(Me.UpdatePanel1, GetType(String), "funcionRefresco", "mostrarUbicacion();", True)
    End Sub
End Class