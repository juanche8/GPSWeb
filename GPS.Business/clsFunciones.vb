Imports System.Diagnostics
Imports System.Net
Public Class clsFunciones
    Public Shared Function WriteToEventLog(ByVal message As String) As Boolean
        Dim cs As String = "GPSBusiness"
        Dim elog As New EventLog()
        If Not EventLog.SourceExists(cs) Then
            EventLog.CreateEventSource(cs, "Application")
        End If

        EventLog.WriteEntry(cs, message, EventLogEntryType.Error)

        ' Send_Mail_Alerta(message)
        Return True
    End Function
    Public Shared Sub Send_Mail_Alerta(ByVal sError As String)

        Try

            'Create instance of main mail message class.
            Dim mailMessage As New System.Net.Mail.MailMessage()

            'Configure mail mesage
            'Set the From address with user input
            mailMessage.From = New System.Net.Mail.MailAddress("javiermale@gmail.com")
            'Get From address in web.config
            'Another option is the "from" attirbute in the <smtp> element in the web.config.

            'Set additinal addresses
            mailMessage.[To].Add(New System.Net.Mail.MailAddress("soporte@rastreourbano.com"))

            'Set additional options
            mailMessage.Priority = System.Net.Mail.MailPriority.High
            'Text/HTML
            mailMessage.IsBodyHtml = True

            'Set the subjet and body text
            mailMessage.Subject = "ALERTA - Error en sistema GPS Web - SERVIDOR"
            mailMessage.Body = "Se ha producido un error en el sistema. <br/><br/> DETALLES: <br/>" + sError
            'Use a Try/Catch block to trap sending errors
            'Especially useful when looping through multiple sends

            Try
                'Create an instance of the SmtpClient class for sending the email
                Dim smtpClient As New System.Net.Mail.SmtpClient()
                Dim nc As System.Net.NetworkCredential = New System.Net.NetworkCredential("info@rastreourbano.com", "Rastreo2015")
                smtpClient.Credentials = nc
                'smtpClient.EnableSsl = True
                smtpClient.Send(mailMessage)

            Catch smtpExc As System.Net.Mail.SmtpException

                'Log error information on which email failed.
                ' EventLog.WriteEntry("GPSBusiness", "error enviando mail - " + smtpExc.ToString(), EventLogEntryType.Error)

                'Throw
            End Try

        Catch ex As Exception
            ' EventLog.WriteEntry("GPSBusiness", "error enviando mail - " + ex.ToString(), EventLogEntryType.Error)

        End Try

    End Sub

    Public Shared Sub Send_Mail_Alarma_Clientes(ByVal sError As String, ByVal sMail As String)

        Try

            'Create instance of main mail message class.
            Dim mailMessage As New System.Net.Mail.MailMessage()

            'Configure mail mesage
            'Set the From address with user input
            mailMessage.From = New System.Net.Mail.MailAddress("alarmas@rastreourbano.com", "Alarmas Rastreo Urbano")
            'Get From address in web.config
            'Another option is the "from" attirbute in the <smtp> element in the web.config.

            'Set additinal addresses
            mailMessage.[To].Add(New System.Net.Mail.MailAddress(sMail))


            'Set additional options
            mailMessage.Priority = System.Net.Mail.MailPriority.High
            'Text/HTML
            mailMessage.IsBodyHtml = True

            'Set the subjet and body text
            mailMessage.Subject = "Nueva ALARMA Reportada por su Movil"
            mailMessage.Body = sError
            'Use a Try/Catch block to trap sending errors
            'Especially useful when looping through multiple sends

            Try
                'Create an instance of the SmtpClient class for sending the email
                Dim smtpClient As New System.Net.Mail.SmtpClient()
                Dim nc As System.Net.NetworkCredential = New System.Net.NetworkCredential("info@rastreourbano.com", "Rastreo2015")
                smtpClient.Credentials = nc
                ' smtpClient.EnableSsl = True

                smtpClient.Send(mailMessage)

            Catch smtpExc As System.Net.Mail.SmtpException

                Dim mensaje As String = smtpExc.Message
                'Log error information on which email failed.
                'EventLog.WriteEntry("GPSBusiness", "error enviando mail - " + smtpExc.ToString(), EventLogEntryType.Error)

                '  Throw smtpExc
            End Try

        Catch ex As Exception
            ' EventLog.WriteEntry("GPSBusiness", "error enviando mail - " + ex.ToString(), EventLogEntryType.Error)
            'Throw ex
        End Try

    End Sub
End Class
