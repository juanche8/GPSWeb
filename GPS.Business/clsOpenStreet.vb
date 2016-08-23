'funciones para buscar direccion en base a las coordenadas
Imports System.Net
Imports System.Xml
Imports System.IO
Imports GPSDataOSM
Public Class clsOpenStreet
    'Obtiene la direccion a partir de las coordenadas
    Public Shared Function getdireccion(ByVal latitud As String, ByVal longitud As String, ByRef sError As String) As Direccione
        Dim direccion As String = ""
        Dim _direccion As Direccione = New Direccione()
        Try

            'primero busco sino existen la lat y lng en la base
            _direccion = SelectByCoordenadas(latitud, longitud)

            If _direccion Is Nothing Then

                _direccion = New Direccione
                _direccion.dir_latitud = ""
                _direccion.dir_longitud = ""
                _direccion.dir_tipo_via = 1
                _direccion.dir_localidad = ""
                _direccion.dir_provinica = ""
                _direccion.dir_nombre_via = ""

                Dim request As WebRequest = WebRequest.Create("http://nominatim.openstreetmap.org/reverse?email=soporte@rastreourbano.com&format=xml&lat=" + latitud + "&lon=" + longitud + "&zoom=18&addressdetails=1")
                request.Method = "GET"
                Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Dim sr As StreamReader = New StreamReader(response.GetResponseStream())
                Dim result1 As String = sr.ReadToEnd()
                'cargo el resultado como xml y busco las coordenadas

                Dim xmlRespuesta As XmlDocument = New XmlDocument()
                xmlRespuesta.LoadXml(result1)

                If (xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts") IsNot Nothing) Then
                    direccion = xmlRespuesta.SelectSingleNode("//reversegeocode/result").InnerText
                    ' direccion += "," + xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts/city").InnerText
                    ' If xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts/state_district") IsNot Nothing Then direccion += "," + xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts/state_district").InnerText
                    ' If xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts/suburb") IsNot Nothing Then direccion += "," + xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts/suburb").InnerText
                    ' If xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts/state") IsNot Nothing Then direccion += "," + xmlRespuesta.SelectSingleNode("//reversegeocode/addressparts/state").InnerText

                    Dim datos As String() = direccion.Split(",")
                    '312, Bernardo de Irigoyen, Monserrat, CABA, Comuna 1, Buenos Aires, C1084AAU, Argentina
                    _direccion = New Direccione

                    'tipo de via 
                    '1:          Calle
                    '2:          Avenida
                    '3:          Ruta
                    '4:          Autopista

                    _direccion.dir_latitud = latitud
                    _direccion.dir_longitud = longitud
                    _direccion.dir_tipo_via = 1

                    If datos.Count = 7 Or datos.Count = 8 Then

                        'si viene house number se lo sumo
                        If (xmlRespuesta.SelectSingleNode("//house_number") IsNot Nothing) Then
                            _direccion.dir_nombre_via = datos(1).ToString.Trim() & " " & xmlRespuesta.SelectSingleNode("//house_number").InnerText
                        Else
                            _direccion.dir_nombre_via = datos(1).ToString.Trim() & " " & datos(0).ToString.Trim()
                        End If

                        _direccion.dir_localidad = datos(2).ToString.Trim() + "-" + datos(3).ToString.Trim()

                        If (datos.Count = 8) Then
                            _direccion.dir_provinica = datos(5).ToString.Trim()
                        Else
                            _direccion.dir_provinica = datos(4).ToString.Trim()
                        End If


                        'caso av gral paz CABA
                        If _direccion.dir_nombre_via = "General Paz" Then
                            _direccion.dir_tipo_via = 2
                        Else

                            If datos(1).ToString().Contains("Av.") Then _direccion.dir_tipo_via = 2
                            If datos(1).ToString().Contains("Avenida") Then _direccion.dir_tipo_via = 2
                            If datos(1).ToString().Contains("RP") Then _direccion.dir_tipo_via = 3
                            If datos(1).ToString().Contains("RN") Then _direccion.dir_tipo_via = 3
                            If datos(1).ToString().Contains("Ruta Nacional") Then _direccion.dir_tipo_via = 3
                            If datos(1).ToString().Contains("Ruta Provincial") Then _direccion.dir_tipo_via = 3
                            If datos(1).ToString().Contains("Aut.") Then _direccion.dir_tipo_via = 4
                            If datos(1).ToString().Contains("Autopista") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Au") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Au.") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Panamericana") Then _direccion.dir_tipo_via = 4
                        End If

                    Else
                        If datos.Count = 5 Or datos.Count = 6 Then
                            If datos(0).ToString().Contains("Av.") Then _direccion.dir_tipo_via = 2
                            If datos(1).ToString().Contains("Avenida") Then _direccion.dir_tipo_via = 2
                            If datos(0).ToString().Contains("RP") Then _direccion.dir_tipo_via = 3
                            If datos(0).ToString().Contains("RN") Then _direccion.dir_tipo_via = 3
                            If datos(0).ToString().Contains("Ruta Nacional") Then _direccion.dir_tipo_via = 3
                            If datos(0).ToString().Contains("Ruta Provincial") Then _direccion.dir_tipo_via = 3
                            If datos(1).ToString().Contains("Aut.") Then _direccion.dir_tipo_via = 4
                            If datos(1).ToString().Contains("Autopista") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Au") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Au.") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Panamericana") Then _direccion.dir_tipo_via = 4

                            _direccion.dir_nombre_via = datos(0).ToString.Trim()
                            _direccion.dir_localidad = datos(1).ToString.Trim() + "-" + datos(2).ToString.Trim()
                            _direccion.dir_provinica = datos(3).ToString.Trim()
                        Else
                            If datos(0).ToString().Contains("Av.") Then _direccion.dir_tipo_via = 2
                            If datos(0).ToString().Contains("RP") Then _direccion.dir_tipo_via = 3
                            If datos(0).ToString().Contains("RN") Then _direccion.dir_tipo_via = 3
                            If datos(0).ToString().Contains("Ruta Nacional") Then _direccion.dir_tipo_via = 3
                            If datos(0).ToString().Contains("Ruta Provincial") Then _direccion.dir_tipo_via = 3
                            If datos(1).ToString().Contains("Aut.") Then _direccion.dir_tipo_via = 4
                            If datos(1).ToString().Contains("Autopista") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Au") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Au.") Then _direccion.dir_tipo_via = 4
                            If datos(0).ToString().Contains("Panamericana") Then _direccion.dir_tipo_via = 4

                            _direccion.dir_nombre_via = datos(0).ToString.Trim()
                            _direccion.dir_localidad = datos(2).ToString.Trim()
                            _direccion.dir_provinica = datos(3).ToString.Trim()

                        End If

                    End If

                    'inserto la direccion en la tabla
                    _direccion.dir_origen = "OSM"
                    Insert(_direccion)

                    'tengo que retornar el objeto direccion


                Else
                    sError = "La Dirección ingresada no se pudo encontrar en el sistema de Geoposicionamiento. Verifique. Respuesta: " & xmlRespuesta.InnerText
                End If
            End If

        Catch e As WebException
            Dim Stream As System.IO.Stream = e.Response.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(Stream)
            sError = reader.ReadToEnd()
            Stream.Close()
            Stream.Close()

        Catch ex As Exception
            sError = ex.Message
        End Try

        Return _direccion
    End Function

    ' Santander Rio, Av. Cabildo, Barrio Chino, Belgrano, Ciudad Autónoma de Buenos Aires, 1426, Argentina
    '<?xml version="1.0" encoding="UTF-8" ?>
    '<reversegeocode timestamp='Tue, 03 Dec 13 18:44:46 +0000' attribution='Data © OpenStreetMap contributors, ODbL 1.0. http://www.openstreetmap.org/copyright' querystring='format=xml&amp;lat=-34.8175666666667&amp;lon=-58.3510333333333&amp;zoom=18&amp;addressdetails=1'>
    '<result place_id="43849606" osm_type="way" osm_id="34387948" ref="Av. Eva Perón" lat="-34.8189713" lon="-58.3427714">
    'Av. Eva Perón, Don Orione, Partido de Almirante Brown, Buenos Aires, B1854BBB, Argentina</result>
    '<addressparts>
    '<road>Av. Eva Perón</road>
    '<city>Don Orione</city>
    '<state_district>Partido de Almirante Brown</state_district>
    '<state>Buenos Aires</state><postcode>B1854BBB</postcode>
    '<country>Argentina</country>
    '<country_code>ar</country_code>
    '</addressparts></reversegeocode>

    '<addressparts><road>Camino General Belgrano</road>
    '<suburb>Villa Barilari</suburb>
    '<city>Lanús</city><state>Buenos Aires</state>
    '<country>Argentina</country><country_code>ar</country_code></addressparts>

    '   <?xml version="1.0" encoding="UTF-8" ?>
    '<reversegeocode timestamp='Wed, 04 Dec 13 19:26:58 +0000' attribution='Data © OpenStreetMap contributors, ODbL 1.0. http://www.openstreetmap.org/copyright' querystring='format=xml&amp;lat=-34.6045&amp;lon=-58.39226&amp;zoom=18&amp;addressdetails=1'>
    '<result place_id="9146494329" osm_type="node" osm_id="2161333695" lat="-34.6045553333333" lon="-58.3922548966667">
    '  1790, Valentín Gómez, Abasto, Balvanera, Ciudad Autónoma de Buenos Aires, 1204, Argentina</result>
    '<addressparts>
    '<house_number>1790</house_number>
    '<road>Valentín Gómez</road>
    '<suburb>Abasto</suburb>
    '<city_district>Balvanera</city_district>
    '<city>Ciudad Autónoma de Buenos Aires</city>
    '<state>Ciudad Autónoma de Buenos Aires</state>
    '<postcode>1204</postcode><country>Argentina</country
    '><country_code>ar</country_code>
    '</addressparts></reversegeocode>

    '    <?xml version="1.0" encoding="UTF-8"?>
    '        <reversegeocode timestamp="Tue, 07 Jan 14 18:12:27 +0000" attribution="Data © OpenStreetMap contributors, 
    'ODbL 1.0. http://www.openstreetmap.org/copyright" querystring="format=xml&amp;lat=-34.63737869&amp;lon=-58.37073135&amp;zoom=18&amp;addressdetails=1">
    '<result place_id="84084332" osm_type="way" osm_id="147999177" ref="Ruy Díaz de Guzmán" lat="-34.6381548" lon="-58.3704937">
    '    Ruy Díaz de Guzmán, Barracas, Ciudad Autónoma de Buenos Aires, C1169AAD, Argentina</result>
    '<addressparts><road>Ruy Díaz de Guzmán</road>
    '<suburb>Barracas</suburb><city_district>Barracas</city_district><city>Ciudad Autónoma de Buenos Aires</city><state>Ciudad Autónoma de Buenos Aires</state><postcode>C1169AAD</postcode><country>Argentina</country>
    '<country_code>ar</country_code></addressparts></reversegeocode>

    Public Shared Sub getdireccion2(ByVal latitud As String, ByVal longitud As String, ByRef direccion As String, ByRef sError As String)


        Dim request As WebRequest = WebRequest.Create("http://open.mapquestapi.com/geocoding/v1/reverse?key=Fmjtd%7Cluubn962nl%2Cax%3Do5-907xqw&callback=renderReverse&xml=<reverse><location><latLng><lat>" + latitud + "</lat><lng>" + longitud + "</lng></latLng></location></reverse>")
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

    Public Shared Function Insert(ByVal direccion As Direccione) As [Boolean]
        Dim DB As New DataClassesDireccionesDataContext()

        Try
            DB.Direcciones.InsertOnSubmit(direccion)
            DB.SubmitChanges()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function SelectByCoordenadas(ByVal latitud As String, ByVal longitud As String) As Direccione
        Dim DB As New DataClassesDireccionesDataContext()

        If latitud.Length > 7 Then
            Return DB.Direcciones.Where(Function(d) d.dir_latitud.Substring(0, 8) = latitud.Substring(0, 8) And d.dir_longitud.Substring(0, 8) = longitud.Substring(0, 8)).FirstOrDefault()

        Else
            Return DB.Direcciones.Where(Function(d) d.dir_latitud.Substring(0, 8) = latitud And d.dir_longitud.Substring(0, 8) = longitud.Substring(0, 8)).FirstOrDefault()

        End If
    End Function




    'TYP = 3
    'bridleway
    'construction
    'cycleway
    'footway
    'living_street
    'mini_roundabout
    'motorway
    'motorway_link
    'path
    'pedestrian
    'primary
    'primary_link
    'residential
    'road
    'secondary
    'service
    'steps
    'tertiary
    'track
    'trunk
    'trunk_link
    'unclassified*/

    'PAIS
    'SELECT     RelationId, Typ, Info
    'FROM         tRelationTag
    'WHERE     (RelationId = 286393) AND (Typ = 141)

    'typ = 64 Ciudad

    'ver enq ue tabla esta la localidad


End Class
