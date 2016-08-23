Imports GPS.Business
Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq
Public Class pComandos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblError.Text = ""
            If Session("Admin") IsNot Nothing Then
                If Not IsPostBack Then

                   
                    ddlComandos.DataSource = clsModulo.ListComandos()
                    ddlComandos.DataBind()
                    ddlComandos.Items.Insert(0, New ListItem("Seleccionar Comando", 0))

                    

                End If
            Else
                'no esta logeado
                Response.Redirect("~/login_admin.aspx", False)
            End If
        Catch ex As Exception
            lblError.Text = "Ocurrio un error cargando los datos - " + ex.Message
        End Try
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        'guardo en la tabla el mensaje para el modulo
        Try
            'recupero el parametro con el codigo que hay que adjuntar al comando

            Dim _parametro As Parametros = clsParametros.ParametroSelect("codigo_comando")
            Dim _mensaje As Comandos_Enviado = New Comandos_Enviado()
            'verifico si elegio varios modulos para mandar el mensaje
            For Each check As ListItem In chkMoviles.Items

                If check.Selected Then
                    Dim _modulo As Modulo = clsModulo.SelecionarById(check.Value)
                    _mensaje = New Comandos_Enviado()
                    _mensaje.com_id = CInt(ddlComandos.SelectedValue)
                    _mensaje.men_fecha = DateTime.Now
                    _mensaje.men_mensaje = _modulo.mod_numero & _parametro.par_valor & txtMensaje.Text
                    _mensaje.men_respuesta = ""
                    _mensaje.men_id_terminal = ""
                    _mensaje.men_enviado = False
                    _mensaje.mod_id = check.Value
                    clsModulo.InsertComando(_mensaje)
                End If
            Next



            lblError.Text = "Mensaje Insertado en Cola con exito."
        Catch ex As Exception
            lblError.Text = "Ocurrio un error Grabando los datos - " + ex.Message
        End Try
    End Sub

    Public Sub JQGridRespuestas_DataRequesting(ByVal sender As Object, ByVal e As Trirand.Web.UI.WebControls.JQGridDataRequestEventArgs)
        Dim filtro As String = e.SearchExpression

        Dim dt As DataTable = New DataTable()
        Dim respuestas As List(Of Respuestas_Comando) = clsModulo.ListRespuestas()

        If respuestas.Count > 0 Then
            dt.Columns.Add("mod_id", GetType(Integer))
            dt.Columns.Add("cliente")
            dt.Columns.Add("patente")
            dt.Columns.Add("men_fecha", GetType(Date))
            dt.Columns.Add("men_respuesta")


            For Each d As Respuestas_Comando In respuestas

                Dim _movil As Vehiculo = clsVehiculo.Seleccionar(d.mod_id.ToString())

                If _movil IsNot Nothing Then
                    Dim dr As DataRow = dt.NewRow()
                    dr(0) = d.mod_id
                    dr(1) = _movil.Cliente.cli_nombre & " " & _movil.Cliente.cli_apellido

                    dr(2) = _movil.veh_patente
                    dr(3) = d.men_fecha
                    dr(4) = d.men_respuesta

                    dt.Rows.Add(dr)
                End If

            Next

        End If

        JQGridRespuestas.DataSource = dt
        JQGridRespuestas.DataBind()



    End Sub

    Protected Sub ddlComandos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlComandos.SelectedIndexChanged
        'armo el mensaje

            txtMensaje.Text = clsModulo.SelectComando(CInt(ddlComandos.SelectedValue)).com_comando

    End Sub

    Protected Sub valInquiry_ServerValidation(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        args.IsValid = chkMoviles.SelectedItem IsNot Nothing
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        'busco por cliente, movil o patente el modulo
        chkMoviles.DataSource = clsModulo.List()
        chkMoviles.DataBind()
    End Sub
End Class