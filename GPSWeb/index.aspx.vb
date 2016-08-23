Public Class index
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            'envio el mail con los datos del contacto

            'Creamos el contenido del mensaje
            Dim Mensaje As String = "Nueva consulta desde el sitio de RastreoUrbano (Formulario Rapido Pagina Inicio)<br /><br /><strong>Datos Personales</strong><br />Nombre: " + nombre.Text.Trim() & _
                     "<br /><br /><strong>Información Contacto</strong><br /><br />Télefono: " + tel.Text.Trim() & _
                   "<br />E-Mail: " + mail.Text.Trim() & _
                     "<br />Pide Resupuesto Para: " + cboPresupuesto.SelectedValue



            Dim mailMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()

            mailMessage.From = New System.Net.Mail.MailAddress("info@rastreourbano.com")
            mailMessage.To.Add(New System.Net.Mail.MailAddress("ventas@rastreourbano.com"))

            'Set the subjet and body text
            mailMessage.Subject = "Solicitud de Presupuesto desde el sitio."
            mailMessage.Body = Mensaje


            'Text/HTML
            mailMessage.IsBodyHtml = True
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal

            'Create an instance of the SmtpClient class for sending the email
            Dim smtpClient As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient("mail.rastreourbano.com")
            Dim nc As System.Net.NetworkCredential = New System.Net.NetworkCredential("info@rastreourbano.com", "Rastreo2015")
            smtpClient.Credentials = nc

            Try
                smtpClient.Send(mailMessage)
                lblResultado.Text = "Recibimos su solicitud lo contactaremos a la brevedad."
            Catch ex As Exception
                Throw ex
            End Try


        Catch ex As Exception

        End Try
    End Sub
End Class