'clase para manejar consultas de geoposicionamiento
Imports System.Net
Imports System.Xml
Imports System.IO
Imports GPSDataOSM

Public Class clsGoogle

    'obtiene a partir de la direccion las coordenadas
    Public Shared Sub getLatLng(ByRef latitud As String, ByRef longitud As String, ByVal direccion As String, ByRef sError As String)

        Dim request As WebRequest = WebRequest.Create("http://maps.google.com/maps/api/geocode/xml?address=" + direccion + "&sensor=true")
        request.Method = "GET"
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
        Dim result1 As String = sr.ReadToEnd()
        'cargo el resultado como xml y busco las coordenadas

        Dim xmlRespuesta As XmlDocument = New XmlDocument()
        xmlRespuesta.LoadXml(result1)

        If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then
            latitud = xmlRespuesta.SelectSingleNode("//geometry/location/lat").InnerText
            longitud = xmlRespuesta.SelectSingleNode("//geometry/location/lng").InnerText
        Else
            sError = "La Dirección ingresada no se pudo encontrar en el sistema de Geoposicionamiento. Verifique. Estatus: " & xmlRespuesta.SelectSingleNode("//status").InnerText
        End If

    End Sub

    'Obtiene la direccion a partir de las coordenadas
    Public Shared Sub getdireccion(ByVal latitud As String, ByVal longitud As String, ByRef direccion As String, ByRef sError As String)

        Dim request As WebRequest = WebRequest.Create("http://maps.google.com/maps/api/geocode/xml?latlng=" + latitud + "," + longitud + "&sensor=true")
        request.Method = "GET"
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
        Dim result1 As String = sr.ReadToEnd()
        'cargo el resultado como xml y busco las coordenadas

        Dim xmlRespuesta As XmlDocument = New XmlDocument()
        xmlRespuesta.LoadXml(result1)

        If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then
            direccion = xmlRespuesta.SelectSingleNode("//formatted_address").InnerText

        Else
            sError = "La Dirección ingresada no se pudo encontrar en el sistema de Geoposicionamiento. Verifique"
        End If

    End Sub

    Public Shared Function getdireccion(ByVal latitud As String, ByVal longitud As String, ByRef sError As String) As Direccione
        Dim direccion As String = ""
        Dim _direccion As Direccione = New Direccione()
        Try
            _direccion = clsOpenStreet.SelectByCoordenadas(latitud, longitud)
            Dim _oversize As GPS.Data.Parametros
            If _direccion Is Nothing Then
                _direccion = New Direccione
                _direccion.dir_latitud = ""
                _direccion.dir_longitud = ""
                _direccion.dir_tipo_via = 1
                _direccion.dir_localidad = ""
                _direccion.dir_provinica = ""
                _direccion.dir_nombre_via = ""


                'verifico parametro de over size para saber si hoy agote el total de consultas, si la fecha es anterior sigo consultando a google
                ' _oversize = clsParametros.ParametroSelect("over_size_google")

                '  If (DateTime.Now - DateTime.Parse(_oversize.par_valor)).TotalDays > 1 Then
                Dim request As WebRequest = WebRequest.Create("http://maps.google.com/maps/api/geocode/xml?latlng=" + latitud + "," + longitud + "&sensor=true")
                request.Method = "GET"
                Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
                Dim result1 As String = sr.ReadToEnd()
                'cargo el resultado como xml y busco las coordenadas

                Dim xmlRespuesta As XmlDocument = New XmlDocument()
                xmlRespuesta.LoadXml(result1)

                If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then

                    direccion = xmlRespuesta.SelectSingleNode("//formatted_address").InnerText
                    Dim datos As String() = direccion.Split(",")
                    _direccion.dir_nombre_via = datos(0).ToString.Trim()
                    _direccion.dir_localidad = datos(1).ToString.Trim()
                    _direccion.dir_provinica = datos(2).Replace("Province", "").Trim()
                    _direccion.dir_provinica = _direccion.dir_provinica.Replace("Autonomous City of", "Ciudad Autónoma de")

                    If _direccion.dir_provinica = "Ciudad Autónoma de Buenos Aires" Then
                        _direccion.dir_localidad = datos(2).Replace("Province", "").Trim()
                        _direccion.dir_provinica = datos(1).ToString.Trim()
                    End If

                    Dim tipoVia As String = xmlRespuesta.SelectSingleNode("//result/address_component/type").InnerText
                    _direccion.dir_latitud = latitud
                    _direccion.dir_longitud = longitud
                    _direccion.dir_tipo_via = 1

                    'caso av gral paz CABA
                    If _direccion.dir_nombre_via = "General Paz" Then
                        _direccion.dir_tipo_via = 2
                    Else
                        If tipoVia.Contains("street") Then _direccion.dir_tipo_via = 1
                        If datos(0).ToString().Contains("Av.") Then _direccion.dir_tipo_via = 2
                        If datos(0).ToString().Contains("Avenida") Then _direccion.dir_tipo_via = 2
                        If datos(0).ToString().Contains("Avenue") Then _direccion.dir_tipo_via = 2
                        If datos(0).ToString().Contains("RP") Then _direccion.dir_tipo_via = 3
                        If datos(0).ToString().Contains("RN") Then _direccion.dir_tipo_via = 3
                        If datos(0).ToString().Contains("Ruta Nacional") Then _direccion.dir_tipo_via = 3
                        If datos(0).ToString().Contains("Ruta Provincial") Then _direccion.dir_tipo_via = 3
                        If datos(0).ToString().Contains("Aut.") Then _direccion.dir_tipo_via = 4
                        If datos(0).ToString().Contains("Au.") Then _direccion.dir_tipo_via = 4
                        If datos(0).ToString().Contains("Au") Then _direccion.dir_tipo_via = 4
                        If datos(0).ToString().Contains("Acceso") Then _direccion.dir_tipo_via = 4
                        If datos(0).ToString().Contains("Autopista") Then _direccion.dir_tipo_via = 4
                        If datos(0).ToString().Contains("Panamericana") Then _direccion.dir_tipo_via = 4
                    End If

                    'inserto la direccion en la tabla
                    _direccion.dir_origen = "Google"
                    clsOpenStreet.Insert(_direccion)

                    'tengo que retornar el objeto direccion

                Else

                    If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OVER_QUERY_LIMIT") Then
                        'actualizo el parametro de over size con la fecha para no seguir consultando
                        _oversize = clsParametros.ParametroSelect("over_size_google")
                        _oversize.par_valor = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now)
                        clsParametros.Update(_oversize)
                        'llamo a OSM
                        _direccion = clsOpenStreet.getdireccion(latitud, longitud, sError)
                    Else

                        sError = "La Dirección ingresada no se pudo encontrar en el sistema de Geoposicionamiento. Verifique"
                    End If


                End If
                'Else
                'llamo a OSM
                ' _direccion = clsOpenStreet.getdireccion(latitud, longitud, sError)
                '  End If

            End If
        Catch ex As Exception
            sError = ex.Message
        End Try

        Return _direccion

    End Function

    'Actualiza los datos de calle, localidad, tipo de via en la tabla Monitoreos
    'Public Shared Sub setDireccion(ByVal latitud As String, ByVal longitud As String, ByVal id As Integer)
    '    Try
    '        Dim DB As New DataClassesGPSDataContext()
    '        Dim direccion As String()
    '        Dim nombreVia As String = ""
    '        Dim localidad As String = ""
    '        Dim provincia As String = ""
    '        Dim tipoVia As String = ""

    '        Dim request As WebRequest = WebRequest.Create("http://maps.google.com/maps/api/geocode/xml?latlng=" + latitud + "," + longitud + "&sensor=true")
    '        request.Method = "GET"
    '        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
    '        Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
    '        Dim result1 As String = sr.ReadToEnd()
    '        'cargo el resultado como xml y busco las coordenadas

    '        Dim xmlRespuesta As XmlDocument = New XmlDocument()
    '        xmlRespuesta.LoadXml(result1)

    '        If (xmlRespuesta.SelectSingleNode("//status").InnerText = "OK") Then
    '            direccion = xmlRespuesta.SelectSingleNode("//formatted_address").InnerText.Split(",")
    '            nombreVia = direccion(0).Trim()
    '            localidad = direccion(1).Trim
    '            provincia = direccion(2).Replace("Province", "").Trim()
    '            provincia = provincia.Replace("Autonomous City of", "")
    '            tipoVia = xmlRespuesta.SelectSingleNode("//result/address_component/type").InnerText
    '        Else
    '            clsFunciones.WriteToEventLog("Actualizacion de Direccion en registro de monitoreo. La Dirección ingresada no se pudo encontrar en el sistema de Geoposicionamiento.")
    '        End If

    '        'tipos de via route, street_number
    '        'las autopistas tienen en el nombre "Au."
    '        'las avenidas tienen en el nombre "Avenida"

    '        'actualizar el registro de la posicion del movil
    '        Dim registro As vMonitoreos = DB.vMonitoreos.Where(Function(b) b.Codigo = id).SingleOrDefault()
    '        registro.NOMBRE_VIA = nombreVia
    '        registro.LOCALIDAD = localidad
    '        registro.PROVINCIA = provincia
    '        'verifico el tipo de via
    '        If nombreVia.Contains("Au.") Then
    '            registro.TIPO_VIA = 4 'autopista
    '        Else
    '            If nombreVia.Contains("Avenida") Then
    '                registro.TIPO_VIA = 2
    '            Else
    '                If tipoVia = "route" Then
    '                    registro.TIPO_VIA = 3
    '                Else
    '                    registro.TIPO_VIA = 1
    '                End If

    '            End If
    '        End If


    '        DB.SubmitChanges()

    '    Catch ex As Exception
    '        clsFunciones.WriteToEventLog("Actualizacion de Direccion en registro de monitoreo.Error: " + ex.Message + "-" + ex.StackTrace)
    '    End Try

    'End Sub

    
End Class
'http://wiki.openstreetmap.org/wiki/Nominatim#Parameters