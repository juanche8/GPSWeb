Public Class clsTrama

    Private Sensores As String = ""
    ' ***** ATRIBUTOS
    Public Property enMemoria() As Boolean
        Get
            Return m_Memoria
        End Get
        Set(value As Boolean)
            m_Memoria = value
        End Set
    End Property
    Private m_Memoria As Boolean

    Public Property Tipo() As String
        Get
            Return m_tipo
        End Get
        Set(value As String)
            m_tipo = value
        End Set
    End Property
    Private m_tipo As String

    Public Property Fecha() As String
        Get
            Return m_Fecha
        End Get
        Set(value As String)
            m_Fecha = value
        End Set
    End Property
    Private m_Fecha As String

    Public Property Hora() As String
        Get
            Return m_Hora
        End Get
        Set(value As String)
            m_Hora = value
        End Set
    End Property
    Private m_Hora As String

    Public Property Modulo() As String
        Get
            Return m_modulo
        End Get
        Set(value As String)
            m_modulo = value
        End Set
    End Property
    Private m_modulo As String

    Public Property Velocidad() As String
        Get
            Return m_velocidad
        End Get
        Set(value As String)
            m_velocidad = value
        End Set
    End Property
    Private m_velocidad As String


    Public Property GradosLat() As String
        Get
            Return m_gradosLat
        End Get
        Set(value As String)
            m_gradosLat = value
        End Set
    End Property
    Private m_gradosLat As String

    Public Property GradosLng() As String
        Get
            Return m_gradosLng
        End Get
        Set(value As String)
            m_gradosLng = value
        End Set
    End Property
    Private m_gradosLng As String

    Public Property DatosExtras() As String
        Get
            Return m_extras
        End Get
        Set(value As String)
            m_extras = value
            m_modulo = value.Substring(33, 5)
        End Set
    End Property
    Private m_extras As String


    Public Property Movimiento() As String
        Get
            Return m_Movimiento
        End Get
        Set(value As String)
            m_Movimiento = value

        End Set
    End Property
    Private m_Movimiento As String



    '***** METODOS Y FUNCIONES

    Public Function obtenerFecha() As String
        Dim charArray As Char()
        Dim _fecha As String = ""
        Dim _hora As String = ""

        charArray = m_Fecha.ToCharArray()
        _fecha = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(charArray(0) + charArray(1) + "/" + charArray(2) + charArray(3) + "/" + charArray(4) + charArray(5)))
        charArray = m_Hora.ToCharArray()
        _hora = charArray(0) + charArray(1) + ":" + charArray(2) + charArray(3) + ":" + charArray(4) + charArray(5)

        'tengo que restarle tres horas
        Return String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Parse(_fecha + " " + _hora).AddHours(-3))


    End Function

    Public Function obtenerVelocidad() As Decimal
        Return Decimal.Parse(m_velocidad.Replace(".", ",")) * Decimal.Parse("1,852")

    End Function

    
    Public Function obtenerLatitud() As String
        Dim _latitud As String = ""
        Dim grados As String() = m_gradosLat.Split(".")
        _latitud = Integer.Parse(grados(0).Substring(0, 2)) + (Double.Parse(Mid(grados(0), Len(grados(0)) - 1) & "," & grados(1)) / 60).ToString()
        _latitud = "-" & _latitud.Replace(",", ".")

        Return _latitud

    End Function


    Public Function obtenerLongitud() As String
        Dim _longitud As String = ""
        Dim grados As String() = m_gradosLng.Split(".")

        Dim gradosEntero = grados(0)
        Dim gradosDecimal = grados(1)

        If gradosEntero.Substring(0, 1) = "0" Then gradosEntero = Mid(gradosEntero, 2, Len(gradosEntero) - 1)

        If gradosEntero.Length > 4 Then
            _longitud = Integer.Parse(gradosEntero.Substring(0, 3)) + (Double.Parse(Mid(gradosEntero, Len(gradosEntero) - 1) & "," & grados(1)) / 60).ToString()
        Else
            _longitud = Integer.Parse(gradosEntero.Substring(0, 2)) + (Double.Parse(Mid(gradosEntero, Len(gradosEntero) - 1) & "," & grados(1)) / 60).ToString()

        End If
        _longitud = "-" & _longitud.Replace(",", ".")

        Return _longitud

    End Function

    ' A*123A00032AFFE1000T-161R000B122ID10009
    Public Function obtenerSensores() As String

        Dim sensoresExa As String = m_extras.Substring(15, 4).Replace("z", "0")
        'si me da cero tengo que poner 16 ceros
        Sensores = Convert.ToString(Convert.ToInt32(sensoresExa, 16), 2).PadLeft(16, "0")

        Return Sensores

    End Function

    Public Function obtenerTemperatura() As Decimal

        Return CDec(buscarDatosExtra(m_extras.Substring(19, m_extras.Length - 19), "T", "R"))

    End Function

    Public Function obtenerRpm() As Integer
       
        Return (CInt(buscarDatosExtra(m_extras.Substring(19, m_extras.Length - 19), "R", "B")) * 10)

    End Function

    Public Function obtenerBateria() As Integer

        Return (CInt(buscarDatosExtra(m_extras.Substring(19, m_extras.Length - 19), "B", "I")) / 10)

    End Function

    '*******************
    'estos datos que estan en hexadecimal los tengo que pasar a decimales
    Public Function obtenerPresicionSatalite() As Integer

        Return Int32.Parse(m_extras.Substring(2, 4), System.Globalization.NumberStyles.HexNumber)

    End Function

    Public Function obtenerOdometro() As Integer

        Return Int32.Parse(m_extras.Substring(6, 5), System.Globalization.NumberStyles.HexNumber)

    End Function

    Public Function obtenerCargaBateriaInterna() As Integer

        Return Int32.Parse(m_extras.Substring(11, 1), System.Globalization.NumberStyles.HexNumber)

    End Function

    Public Function obtenerCodigoEvento() As Integer

        Return Int32.Parse(m_extras.Substring(12, 2), System.Globalization.NumberStyles.HexNumber)

    End Function

    Public Function obtenerDirecMovimiento() As Decimal

        Return Decimal.Parse(m_Movimiento.Replace(".", ","))

    End Function

    ' A*123A00032AFFE1000T-161R000B122ID10009v
    Private Function buscarDatosExtra(ByVal cadena As String, ByVal inicio As String, ByVal fin As String) As String
        Dim dato As String = ""

        dato = cadena.Substring(cadena.IndexOf(inicio) + 1, cadena.IndexOf(fin) - cadena.IndexOf(inicio) - 1)

        Return dato

    End Function
 
End Class
