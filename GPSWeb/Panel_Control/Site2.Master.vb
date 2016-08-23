Imports GPS.Business
Imports GPS.Data
Public Class Site2
    Inherits System.Web.UI.MasterPage
    Public _defaultButton As String = ""
    'propiedad para definir el default button que ejecuta al hacer ENTER    
    Public Property DefaultButton() As String
        Get
            Return _defaultButton
        End Get
        Set(ByVal value As String)
            form1.DefaultButton = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            If Session("Cliente") IsNot Nothing Then
                cliente.Value = CInt(Session("Cliente").ToString)
                Dim _cliente As Cliente = clsCliente.SelectById(CInt(cliente.Value))
                lblUsuario.Text = _cliente.cli_nombre & " " & _cliente.cli_apellido
                lblUsuario2.Text = _cliente.cli_nombre & " " & _cliente.cli_apellido
                lblNombreUsuario.Text = _cliente.cli_nombre & " " & _cliente.cli_apellido
                lblMail.Text = _cliente.cli_email
                lblTel.Text = _cliente.cli_telefono

                cliente_id.Value = _cliente.cli_id.ToString()

                'busco ultimas alarmas
                GetAlarmas()
            End If
        End If
       
    End Sub

    Protected Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Session.Remove("Cliente")
        Response.Redirect("~/login.aspx", False)
    End Sub


    Private Sub GetAlarmas()
        Try
            Dim dt As DataTable = New DataTable()
            Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertasAll(CInt(cliente_id.Value), DateTime.Now.AddDays(-1), DateTime.Now)

            If alarmas.Count > 0 Then

                lblNoData.Visible = False
                dt.Columns.Add("Alarma")
                dt.Columns.Add("Movil")
                dt.Columns.Add("Fecha")
                dt.Columns.Add("alar_id", GetType(Integer))
                dt.Columns.Add("ubicacion")
                For Each d As Alarmas In alarmas

                    Dim dr As DataRow = dt.NewRow()
                    dr(0) = d.alar_nombre
                    dr(1) = d.veh_patente & "-"
                    dr(2) = d.alar_fecha.ToString("dd/MM/yyyy HH:mm:ss")
                    dr(3) = d.alar_id
                    dr(4) = d.alar_nombre_via & "," & d.alar_Localidad & "," & d.alar_Provincia

                    dt.Rows.Add(dr)
                Next
                

            Else

                lblNoData.Visible = True
            End If

            DataListAlarma.DataSource = dt
            DataListAlarma.DataBind()



        Catch ex As Exception
            Funciones.WriteToEventLog("MAPA - " + ex.Message + " - " + ex.StackTrace)
        End Try



    End Sub

   
    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Session("Cliente") IsNot Nothing Then
            cliente.Value = CInt(Session("Cliente").ToString)
        End If
        GetAlarmas()
    End Sub
End Class