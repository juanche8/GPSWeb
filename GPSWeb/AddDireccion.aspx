<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddDireccion.aspx.vb" Inherits="GPSWeb.AddDireccion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
     <script src="scripts/jquery-1.7.2.js" type="text/javascript"></script>

    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?libraries=geometry&sensor=false"></script>
    

<script type="text/javascript">
 
    var directionsDisplay = null;
    var directionsService = null;
    var markersArray = [];
    var marker = null;
    var lat = null;
    var lng = null;
    var map = null;
    var geocoder = null;

    function initialize() {
        geocoder = new google.maps.Geocoder();
        var myLatlng = new google.maps.LatLng(-34.74161249883172, -58.18359375);
        var myOptions = {
            zoom: 8,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map($("#map_canvas").get(0), myOptions);
     
        // Relación del evento de clic sobre el mapa con el
        // procedimiento de georreferenciación inversa

        google.maps.event.addListener(map, 'click', function (event) {
            clearOverlays();
            processReverseGeocoding(event.latLng, showMarker);
        });
    }


    function showMarker(locations) {

        // Centra el mapa en la ubicación especificada

        map.setCenter(locations[0].geometry.location);

        //creamos el marcador en el mapa
        marker = new google.maps.Marker({
            map: map, //el mapa creado en el paso anterior
            position: locations[0].geometry.location, //objeto con latitud y longitud
            draggable: true //que el marcador se pueda arrastrar
        });

        markersArray.push(marker);
        updatePosition(locations[0].geometry.location)

        google.maps.event.addListener(marker, 'dragend', function () {
            updatePosition(marker.getPosition());
            markersArray.push(marker);

        });

    }

    function updatePosition(latLng) {

        codeLatLng(latLng.lat(), latLng.lng());

        jQuery('#<%=txtLatitud.ClientID %>').val(latLng.lat());
        jQuery('#<%= txtLongitud.ClientID %>').val(latLng.lng());

    }

    function processReverseGeocoding(location, callback) {
        // Propiedades de la georreferenciación

        var request = {
            latLng: location
        }

        // Invocación a la georreferenciación (proceso asíncrono)

        geocoder.geocode(request, function (results, status) {

            // En caso de terminarse exitosamente el proyecto ...

            if (status == google.maps.GeocoderStatus.OK) {
                // Invoca la función de callback
                callback(results);

                // Retorna los resultados obtenidos
                return results;
            }

            // En caso de error retorna el estado
            return status;
        });
    }


    $(document).ready(function() {
        initialize();
        // gmaps_ads();
    });


    // Removes the overlays from the map, but keeps them in the array
    function clearOverlays() {
        if (markersArray) {
            for (i in markersArray) {
                markersArray[i].setMap(null);
            }
        }
        //limpio las direcciones
        document.getElementById('<%= txtLatitud.ClientID %>').value = '';
        document.getElementById('<%= txtLongitud.ClientID %>').value = '';
    }

    //asociar enter al boton IR que busca las direcciones
    $(document).keypress(function(e) {
        if (e.keyCode == 13) {
            marcarDireccion();
        }
        return (e.keyCode != 13); //evito que el Enter ejecute el submit del formulario
    });

  

</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
 
    <ContentTemplate>
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <div style="border: thin solid #00A6C6; float: left; width: 30%; height:100%">
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label><br />
   
    <asp:Label ID="Label1" runat="server" Text="Datos para Insertar un nuevo Registro de Direccion" Font-Bold="true"></asp:Label>
        <br />
        <br />
  Latitud&nbsp;&nbsp;<asp:TextBox ID="txtLatitud" runat="server" Width="259px"></asp:TextBox>
		<br/>
Longitud <asp:TextBox ID="txtLongitud" runat="server" Width="259px"></asp:TextBox>
		<br />
		
<br /><br />
        <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label>
           <br />
           <asp:Button ID="Button2" runat="server" Text="Buscar Direccion" />
        <asp:Button ID="Button1" runat="server" Text="Insertar Direccion" />
           
   
       
        </div>
         </ContentTemplate>
    </asp:UpdatePanel>
 <div style="border: thin solid #00A6C6; float: left; width: 68%">

<div id="map_canvas" style="width: 800px; height: 690px"></div>

   
    </div>
    </div>
    </form>
</body>
</html>
