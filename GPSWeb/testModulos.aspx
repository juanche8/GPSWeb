<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="testModulos.aspx.vb" Inherits="GPSWeb.testModulos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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

        google.maps.event.addListener(map, 'click', function(event) {
        jQuery('#<%= txtLatitud.ClientID %>').val(event.latLng.lat());
        jQuery('#<%= txtLongitud.ClientID %>').val(event.latLng.lng())   
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

    //funcion que traduce la direccion en coordenadas
    function marcarDireccion() {

        var result = "";
        if (document.getElementById("<%= txtDireccion.ClientID %>").value != '') {
            var marker = new google.maps.Marker({
                map: map, //el mapa creado en el paso anterior                    
                draggable: true //que el marcador se pueda arrastrar
            });
            geocoder = new google.maps.Geocoder();
            //obtengo la direccion del formulario
            var address = document.getElementById("<%= txtDireccion.ClientID %>").value;
            document.getElementById("<%= txtDireccion1.ClientID %>").value = address;
            //hago la llamada al geodecoder               
            geocoder.geocode({ 'address': address }, function(results, status) {
                //si el estado de la llamado es OK               
                if (status == google.maps.GeocoderStatus.OK) { //
                    //centro el mapa en las coordenadas obtenidas
                    map.setCenter(results[0].geometry.location);
                    //cambio el zoom
                    map.setZoom(15);
                    //coloco el marcador en dichas coordenadas
                    marker.setPosition(results[0].geometry.location);

                    //Añado un listener para cuando el markador se termine de arrastrar
                    //actualize el formulario con las nuevas coordenadas
                    google.maps.event.addListener(marker, 'dragend', function() {
                        // updatePosition(marker.getPosition());
                        // markersArray.push(marker);

                    });
                } else {
                    //si no es OK devuelvo error
                    alert("No podemos encontrar la direcci&oacute;n, error: " + status);
                }
            });
        }
    }

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
   
    <asp:Label ID="Label1" runat="server" Text="Datos para Insertar un nuevo Registro de Recorrido para un movil" Font-Bold="true"></asp:Label>
        <br />
        <br />
  Latitud&nbsp;&nbsp;<asp:TextBox ID="txtLatitud" runat="server" Width="259px"></asp:TextBox>
		<br/>
Longitud <asp:TextBox ID="txtLongitud" runat="server" Width="259px"></asp:TextBox>
		<br />
		Id Modulo&nbsp;&nbsp;<asp:TextBox ID="txtModulo" runat="server" Width="211px"></asp:TextBox>
		<br/>
		Fecha&nbsp;&nbsp;<asp:TextBox ID="txtFecha" runat="server" Width="211px"></asp:TextBox>
		<br/>
		
		Nombre Calle y Altura&nbsp;&nbsp;<asp:TextBox ID="txtDireccion1" runat="server" Width="211px"></asp:TextBox>
		<br/>
		Localidad&nbsp;&nbsp;<asp:TextBox ID="txtLocalidad" runat="server" Width="211px" Text="Capital Federal"></asp:TextBox>
		<br/>
		Provincia&nbsp;&nbsp;<asp:TextBox ID="txtProvincia" runat="server" Width="211px" Text="Buenos Aires" ></asp:TextBox>
		<br/>
		Km Recorridos&nbsp;&nbsp;<asp:TextBox ID="txtkms" runat="server" Width="70px" 
            Text="200" ></asp:TextBox>
		<br/>
<br /><br />
           
        <asp:Button ID="Button1" runat="server" Text="Insertar Registro" />
           
   
       
        </div>
         </ContentTemplate>
    </asp:UpdatePanel>
 <div style="border: thin solid #00A6C6; float: left; width: 68%">
  <div>
     <asp:Label ID="Label8" runat="server" Text="Buscar Direccion:" Font-Bold="true"></asp:Label>
         &nbsp;&nbsp;
        <asp:TextBox ID="txtDireccion" runat="server" Width="603px"></asp:TextBox>
      &nbsp;<input id="btnDireccion" type="button" value="Ir" onclick="marcarDireccion();" />
         <br />
    </div>
<div id="map_canvas" style="width: 800px; height: 690px"></div>

   
    </div>
    </div>
    </form>
</body>
</html>
