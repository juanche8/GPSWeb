<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pDistancia.aspx.vb" Inherits="GPSWeb.pDistancia" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&libraries=geometry&sensor=false"> 
</script>
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
        var myLatlng = new google.maps.LatLng(-34.604, -58.382);
        var myOptions = {
            zoom: 12,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map($("#map_canvas").get(0), myOptions);
        directionsDisplay = new google.maps.DirectionsRenderer();
        directionsService = new google.maps.DirectionsService();

        // Relación del evento de clic sobre el mapa con el
        // procedimiento de georreferenciación inversa

        google.maps.event.addListener(map, 'click', function(event) {
            processReverseGeocoding(event.latLng, showMarkerPar);
        });
    }

  

//pongo la direccion en el text
    function codeLatLng(lat, lng) {
        //voy ejecutar la busqueda de direccion desde el servidor
        //porque la api de js da error al presionar mas de 20 solicitudes por segundo
        $.ajax({
            async: false,
            type: 'POST',
            url: "wsDatos.asmx/GetDireccion",
            data: "{'lat': '" + lat + "','lng': '" + lng + "'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function(response) {
                var direccion = response.d;
                if (document.getElementById('<%= txtDesde.ClientID %>').value == 'Av/Bv Calle Nro Localidad Provincia' || document.getElementById('<%= txtDesde.ClientID %>').value == '')
                    jQuery('#<%= txtDesde.ClientID %>').val(direccion);
                else
                    jQuery('#<%= txtHasta.ClientID %>').val(direccion);
            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert(textStatus);
            }
        }
        );
      
   
    }
   
 
    //funcion que traduce la direccion en coordenadas
    function marcarDireccion() {
        if (document.getElementById("<%= txtDireccion.ClientID %>").value != '') {
            var marker = new google.maps.Marker({
                map: map, //el mapa creado en el paso anterior                    
                draggable: true //que el marcador se pueda arrastrar
            });

            geocoder = new google.maps.Geocoder();
            //obtengo la direccion del formulario
            var address = document.getElementById("<%= txtDireccion.ClientID %>").value;
            //hago la llamada al geodecoder
            geocoder.geocode({ 'address': address }, function(results, status) {

                //si el estado de la llamado es OK
                if (status == google.maps.GeocoderStatus.OK) {
                    //centro el mapa en las coordenadas obtenidas
                    map.setCenter(results[0].geometry.location);
                    //cambio el zoom
                    map.setZoom(13);
                    //coloco el marcador en dichas coordenadas
                    marker.setPosition(results[0].geometry.location);

                    updatePosition(marker.getPosition());
                    markersArray.push(marker);
                    //Añado un listener para cuando el markador se termine de arrastrar
                    //actualize el formulario con las nuevas coordenadas
                    google.maps.event.addListener(marker, 'dragend', function() {
                        updatePosition(marker.getPosition());
                        markersArray.push(marker);

                    });
                } else {
                    //si no es OK devuelvo error
                    alert("No podemos encontrar la direcci&oacute;n, error: " + status);
                }
            });
        }
    }

    //funcion que simplemente actualiza los campos del formulario
    function updatePosition(latLng) {
       
        if (latLng.lat() != "-34.604" && latLng.lng() != "-58.382")
            codeLatLng(latLng.lat(), latLng.lng());      

    }
   
    function getDirections() {
        var start = $('#<%= txtDesde.ClientID %>').val();
        var end = $('#<%= txtHasta.ClientID %>').val();

        if (start == end ) 
        {
          alert('No se puede elegir el mismo valor en origen y destino');
            return;
        }

        if (start == 'Av/Bv Calle Nro Localidad Provincia' || start == '') {
            alert("Ingrese la Dirección de Origen");
            return;
        }

        if (end == 'Av/Bv Calle Nro Localidad Provincia' || end == '') {
            alert("Ingrese la Dirección de Destino");
            return;
        }

        
        var request = {
            origin: start,
            destination: end,
            travelMode: google.maps.DirectionsTravelMode.DRIVING,
            unitSystem: google.maps.DirectionsUnitSystem.METRIC,
            provideRouteAlternatives: true
        };
        directionsService.route(request, function(response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                directionsDisplay.setMap(map);
                directionsDisplay.setPanel($("#directions_panel").get(0));
                directionsDisplay.setDirections(response);
            } else {
                alert("No existe la ubicación ingresada.");
            }
        });
    }

    $('#search').live('click', function() { getDirections(); });
    $('.routeOptions').live('change', function() { getDirections(); });

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
        document.getElementById('<%= txtDesde.ClientID %>').value = '';
        document.getElementById('<%= txtHasta.ClientID %>').value = '';
        directionsDisplay.setMap(null);
        directionsDisplay.setPanel(null);
    }

    //asociar enter al boton IR que busca las direcciones
    $(document).keypress(function(e) {
        if (e.keyCode == 13) {
            getDirections();
        }
        return (e.keyCode != 13); //evito que el Enter ejecute el submit del formulario
    });



    function volveraMarcar() {
        google.maps.event.addListener(map, 'click', function (event) {
            processReverseGeocoding(event.latLng, showMarkerPar);
        });
        document.getElementById("noMarcar").style.display = "block";
        document.getElementById("Marcar").style.display = "none";

    }

    function dejardeMarcar() {

        google.maps.event.clearListeners(map, 'click');
        document.getElementById("noMarcar").style.display = "none";
        document.getElementById("Marcar").style.display = "block";

    }

    $(document).ready(function () {
        dejardeMarcar();
    });

    function mostrarTrafico() {

        transitLayer = new google.maps.TrafficLayer();
        transitLayer.setMap(map);

        document.getElementById("ocultartrafico").style.display = ' inline-block';
        document.getElementById("vertrafico").style.display = 'none';
    }

    function ocultarTrafico() {


        transitLayer.setMap(null);

        document.getElementById("ocultartrafico").style.display = 'none';
        document.getElementById("vertrafico").style.display = ' inline-block';
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
 
    <ContentTemplate>
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <div class="inline" style="border: thin solid #00A6C6; float: left; width: 25%; height:100%">
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label><br />
     <asp:Label ID="Label1" runat="server" Font-Size="14px" Text="Ingrese las Direcciones para calcular la distancia." Font-Bold="true"></asp:Label><br />
     <br />
  Dirección Origen&nbsp;&nbsp;<asp:TextBox ID="txtDesde" runat="server" Width="250px"></asp:TextBox>
    <ajaxtoolkit:textboxwatermarkextender id="TBWE4" runat="server" targetcontrolid="txtDesde" watermarktext='Av/Bv Calle Nro Localidad Provincia' />
    <br/><br> 
        <asp:Label ID="Label3" runat="server" Text="Dirección Origen de un Movil."></asp:Label> 
        
        <br>
        <asp:DropDownList ID="ddlVehiculoOrigen" runat="server" AutoPostBack="true" 
            DataTextField="veh_descripcion" DataValueField="veh_id" >
        </asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" 
            Text="Dirección Origen de un Marcador ya creado."></asp:Label>
        <br>
        <asp:DropDownList ID="ddlMarcaOrigen" runat="server" AutoPostBack="true" 
            DataTextField="marc_nombre" DataValueField="marc_id">
        </asp:DropDownList>
        <br />
        <br />
        <hr />
        Dirección Destino
        <asp:TextBox ID="txtHasta" runat="server" Width="250px"></asp:TextBox>
        <ajaxToolkit:TextBoxWatermarkExtender ID="TBWE5" runat="server" 
            targetcontrolid="txtHasta" 
            watermarktext="Av/Bv Calle Nro Localidad Provincia" />
        <br />
        <br>
        <asp:Label ID="Label4" runat="server" Text="Dirección Destino de un Movil."></asp:Label> 
        
        <br>
        <asp:DropDownList ID="ddlVehiculoDestino" runat="server" AutoPostBack="true" 
            DataTextField="veh_descripcion" DataValueField="veh_id" >
        </asp:DropDownList>
        <br /><br />
        <asp:Label ID="Label5" runat="server" 
            Text="Dirección Destino de un Marcador ya creado."></asp:Label>
        <br>
        <asp:DropDownList ID="ddlMarcaDestino" runat="server" AutoPostBack="true" 
            DataTextField="marc_nombre" DataValueField="marc_id">
        </asp:DropDownList>
        <br />
      
        <br />
        <input id="search" type="button" value="Calcular Distancia" onclick="javascript:void(0);" />
        <input id="Button1" type="button" value="Limpiar Mapa" onclick="javascript:clearOverlays();" />
        <br />
        <div ID="directions_panel" 
            style="width:100%; height: 390px; overflow: auto; float: right;">
        </div>
        <br>
       
       
         
        </div>
         </ContentTemplate>
    </asp:UpdatePanel>
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 74%; height:100%">
  <div> 
   <br />
    <input type="button" id="noMarcar" onclick="dejardeMarcar();"  value="No Marcar Puntos" />
        <input type="button" id="Marcar" onclick="volveraMarcar();" style="display:none;" value="Marcar Puntos" />
      </div>
  <div>
     <asp:Label ID="Label8" runat="server" Text="Buscar Direccion:" Font-Bold="true"></asp:Label>
         &nbsp;&nbsp;
        <asp:TextBox ID="txtDireccion" runat="server" Width="603px"></asp:TextBox>
      &nbsp;<input id="btnDireccion" type="button" value="Ir" onclick="marcarDireccion();" />
       <a id="vertrafico" href="#" onclick="javascript:mostrarTrafico();" style="color:#cc0000; font-size:12px; font-weight:bold;">Mostrar Tráfico</a>
      <a id="ocultartrafico" href="#" onclick="javascript:ocultarTrafico();" style="color:#cc0000; font-size:12px; font-weight:bold; display:none;">Ocultar Tráfico</a>
  
         <br />
    </div>
<div id="map_canvas" style="width:auto; height:600px">&nbsp;</div>

   
    </div>
   
</asp:Content>
