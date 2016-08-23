'pagina para ahcer un seguimiento detallado de cada movil
Imports GPS.Business
Imports GPS.Data
Partial Public Class pSeguimientos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then
                    'busco los vehiculos del usuario
                    Dim cli_id As Integer = DirectCast(Session("Cliente"), Integer)

                    hdncli_id.Value = cli_id.ToString()

                    'verifico si va a filtrar por un solo movil
                    If Request.Params("veh_id") IsNot Nothing Then
                        hdnveh_id.Value = Request.Params("veh_id").ToString()
                        'PanelCustomizado.Visible = False
                        PanelMoviles.Visible = False
                        lblMovil.Text = "Vehiculo Seleccionado " + clsVehiculo.Seleccionar(CInt(hdnveh_id.Value)).veh_patente
                    Else
                        'busco los vehiculos del usuario
                        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListActivos(cli_id)

                        DataListVehiculos.DataSource = vehiculos
                        DataListVehiculos.DataBind()

                    End If
                    
                End If
            Else
                'no esta logeado
                Response.Redirect("~/login.aspx", False)
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("SEGUIMIENTOS -" + ex.Message + " - " + ex.StackTrace)
            lblError.Text = "Ocurrio un error cargando los datos. Contacte al Administrador."
        End Try
    End Sub

End Class