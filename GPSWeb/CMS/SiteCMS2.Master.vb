Imports GPS.Business
Imports GPS.Data
Public Class SiteCMS2
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
            If Session("Admin") IsNot Nothing Then
                Dim administrador As Administradores = DirectCast(Session("Admin"), Administradores)
                lblUsuario.Text = administrador.adm_nombre
                cliente_id.Value = administrador.adm_id.ToString()

                'busco ultimas alarmas
                GetAlarmas()
            End If
        End If

    End Sub


    Private Sub GetAlarmas()
        Try
            Dim dt As DataTable = New DataTable()
            Dim alarmas As List(Of Alarmas) = clsAlarma.GetAlertasPanico()

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
            Funciones.WriteToEventLog("ADMIN Cabecera - " + ex.Message + " - " + ex.StackTrace)
        End Try



    End Sub


    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Session("Cliente") IsNot Nothing Then
            cliente.Value = CInt(Session("Cliente").ToString)
        End If
        GetAlarmas()
    End Sub

    Protected Sub linkSalir_Click(sender As Object, e As EventArgs) Handles linkSalir.Click
        Session.Remove("Admin")
        Response.Redirect("~/login_admin.aspx", False)
    End Sub
End Class