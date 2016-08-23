<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminMapa.aspx.vb" Inherits="GPSWeb.pAdminMapa1" MasterPageFile="~/CMS/SiteCMS2.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../scripts/jquery-treeview/jquery.treeview.css" rel="stylesheet" type="text/css" />

	<script src="../scripts/jquery-treeview/lib/jquery.cookie.js" type="text/javascript"></script>
	<script src="../scripts/jquery-treeview/jquery.treeview.js" type="text/javascript"></script>
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>
    <script type="text/javascript">
        var infos = [];
        var map;
        var geocoder = null;
        var marker;
        var aLatlng = [];
        var markersArray = [];
        var mostrarZoom = true;
        
        function mostrarVehiculos(veh_id) {
            mostrarZoom = true;
            document.getElementById("<%= hdnveh_id.ClientID %>").value = veh_id
            //ejecuto la consulta contra el negocio para traer los recorridos
            //recupero los parametros para aplicar el filtro, fechas y id vehiculo
            var myOptions = {
                center: new google.maps.LatLng(-35, -64),
                zoom: 10
            };

            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);        

            zoom_actual = map.getZoom();
            //voy a buscar en al base de datos los puntos del recorrido del vehiculo seleccionado
            searchubicacion();
            searchVehiculos();
        }

      
        
        //ejecuto consulta contra la componente de negocios para traer los datos de las ubicaciones actuales de los vehiculos de la empresa logeada
        function searchubicacion() {
            //elimino el market anterior
            clearOverlays();
           
                $.ajax({
                    async: false,
                    type: 'POST',
                    url: "../Panel_Control/wsDatos.asmx/GetVehiculosUbicacion",
                    data: "{'cli_id': '" + document.getElementById("<%= hdncli_id.ClientID %>").value + "' }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function(response) {
                        var vehiculos = response.d;
                        $.each(vehiculos, function(index, movil) {

                            var htmlVentana = "<img src='../fotos/" + movil.foto + "'><br> Identificador:" + movil.id + "</b><br>Nombre:" + movil.nombre + "</b><br>Patente:" + movil.patente + "<br>Ubicación:" + movil.ubicacion + "," + movil.orientacion + "<br>Velocidad:" + movil.velocidad + " Km/H<br>Hora:" + movil.hora;

                            var marker = createMarker(map, movil.lng, movil.lat, htmlVentana, movil.icono);
                            aLatlng.push(new google.maps.LatLng(movil.lat, movil.lng))

                        });
                        map.setCenter(new google.maps.LatLng(vehiculos[0].lat, vehiculos[0].lng));
                        // map.setZoom(11);
                    },
                    error: function(jqXHR, textStatus, errorThrown) {
                        alert('searchubicacion ' + textStatus);
                    }
                });
                if (document.getElementById("<%= hdnveh_id.ClientID %>").value == "0") {
                //centrar mapa
                    if (mostrarZoom) {
                        var latlngbounds = new google.maps.LatLngBounds();
                        for (var i = 0; i < aLatlng.length; i++) {
                            latlngbounds.extend(aLatlng[i]);
                        }
                        map.fitBounds(latlngbounds);

                        map.setZoom(7);
                    }
                }
                else {
                    searchubicacionMovil(document.getElementById("<%= hdnveh_id.ClientID %>").value)
                }

            setTimeout("searchubicacion()", 40000);  //seteo un refresco automatico del mapa
        }

//busco la ubicacion de un movil y hago un zoom mas grande
        function searchubicacionMovil(veh_id) {
            //elimino el market anterior
          //  clearOverlays();
            $.ajax({
                async: false,
                type: 'POST',
                url: "../Panel_Control/wsDatos.asmx/GetUbicacion",
                data: "{'veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var vehiculos = response.d;
                    $.each(vehiculos, function(index, movil) {

                        var htmlVentana = "<img src='../fotos/" + movil.foto + "'><br> Identificador:" + movil.id + "</b><br>Nombre:" + movil.nombre + "</b><br>Patente:" + movil.patente + "<br>Ubicación:" + movil.ubicacion + "<br>Velocidad:" + movil.velocidad + "<br>Hora:" + movil.hora;
                        var marker = createMarker(map, movil.lng, movil.lat, htmlVentana, movil.icono);

                    });
                    map.setCenter(new google.maps.LatLng(vehiculos[0].lat, vehiculos[0].lng));
                    if (mostrarZoom) {
                        map.setZoom(16);
                        mostrarZoom = false; //solo muestro el zoom la primera vez que ejecuta la fucion para evitar que el cambio de zoom del usuario se pierda
                    }           
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert('searchubicacionMovil ' + textStatus);
                }
            });


        }

//funcion que muestra el lsitado de vehiculos con sus datos actuales
        function searchVehiculos() {
        var result ="";
            $.ajax({
                async: false,
                type: 'POST',
                url: "pAdminMapa.aspx/MostarVehiculos",
                data: "{'cli_id': '" + document.getElementById("<%= hdncli_id.ClientID %>").value + "','filtro':'" + document.getElementById("buscador").value + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                result = response.d ? response.d : response;
                document.getElementById("divVehiculos").innerHTML = result;
                    
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            setTimeout("searchVehiculos()", 40000);
            // arbol moviles
            $("#navigation").treeview({
                persist: "location",
                collapsed: false,
                unique: false
            });

        }

        function initialize() {
            var myOptions = {
                center: new google.maps.LatLng(-35, -64),
                zoom: 6,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
          
     }

     $('#buscador').live('keyup', function () { Filtrar(); });

     $(document).ready(function() {         
         //mostrarVehiculos(0);
         initialize();
        
     });

        //funcion que traduce la direccion en coordenadas
        function marcarDireccion() {

            var marker = new google.maps.Marker({
                map: map, //el mapa creado en el paso anterior                    
                draggable: true //que el marcador se pueda arrastrar
            });
            geocoder = new google.maps.Geocoder();
            //obtengo la direccion del formulario
            var address = document.getElementById("txtDireccion").value;
            //hago la llamada al geodecoder
            geocoder.geocode({ 'address': address }, function(results, status) {

                //si el estado de la llamado es OK
                if (status == google.maps.GeocoderStatus.OK) {
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

        

        function mostrarTrafico() {

            transitLayer = new google.maps.TrafficLayer();
            transitLayer.setMap(map);

            document.getElementById("ocultartrafico").style.display = 'block';
            document.getElementById("vertrafico").style.display = ' inline-block';
        }

        function ocultarTrafico() {


            transitLayer.setMap(null);

            document.getElementById("ocultartrafico").style.display = 'none';
            document.getElementById("vertrafico").style.display = ' inline-block';
        }

        function Filtrar() {
            if ($('#buscador').val().length >= 3) {
                document.getElementById("<%= hdnveh_id.ClientID %>").value = 0;
                mostrarVehiculos(0);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <asp:Timer ID="Timer1" runat="server" Interval="40000">
        </asp:Timer>  
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <Triggers>
      <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
      <asp:AsyncPostBackTrigger ControlID="ddCliente" EventName="SelectedIndexChanged"  />
    </Triggers>
    <ContentTemplate>   
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
    <div class="inline" style="float: left; width: 25%; height: 100%">
     <div>
     <asp:Label ID="Label3" runat="server" Text="Buscar Vehiculo:" Font-Bold="true"></asp:Label>
         &nbsp;
         <input type="text" id="buscador" value="" style="width: 190px"/>     
         <br />         
    </div>
    <br />
    <asp:Label ID="Label5" runat="server" Text="Clientes:" Font-Bold="true"></asp:Label><br />
    <asp:Label ID="Label6" runat="server" Text="Seleccione un Cliente para ver la ubicación actual de sus moviles."></asp:Label>
        <br />
        <asp:DropDownList ID="ddCliente" runat="server" DataTextField="cli_nombre" DataValueField="cli_id" AutoPostBack="true">
        </asp:DropDownList>
       
        <br />
     <br />
        <asp:Label ID="Label1" runat="server" Text="Vehiculos:" Font-Bold="true"></asp:Label><br />
 <!--litado vehiculos-->
 <div id="divVehiculos" style="overflow-y: scroll;height:300px;">
   
    </div>
   
    
</div>    
     </ContentTemplate>
</asp:UpdatePanel>
    
     <div style="width:60%; z-index:99; position:absolute; top:0px; left:30%; background-color:White; vertical-align:middle; height:30px; padding-top:1%; white-space: nowrap; margin-right:0%">
  <table cellspacing="2" cellpadding="2" style="width:100%">
  <tr>
  <td valign="middle" style="width:18%"> <asp:Label ID="Label2" runat="server" Text="Buscar Dirección:" Font-Bold="true" Font-Size="12px"></asp:Label>&nbsp;&nbsp; </td>
  <td valign="middle" aling="left"> <input type="text" id="txtDireccion" value="" style="width:95%"/> &nbsp;&nbsp; </td>
  <td style="vertical-align:text-bottom;width:6%; margin-right:1%"><a href="#" onclick="marcarDireccion();" title="Buscar Dirección"><img src="../images/panel_buscar.png" alt="Buscar Direccion"  /></a></td>
  <td style="vertical-align:middle;width:19%; text-align:center">
   <a id="ocultarMarca" style="display:none;" href="#" title="Ocultar Puntos de Interes" onclick="javascript:ocultarMarcores();"><img src="../images/panel_alfiler_off.png" alt="Ocultar Marcadores"  /></a>
    <a id="mostrarMarca"  href="#" title="Mostrar Puntos de Interes" onclick="javascript:mostrarMarcores();"><img src="../images/panel_alfiler_on.png" alt="Mostrar Marcadores"  /></a>
  <a id="A1" href="#" title="Mostrar el nivel de Tráfico en el Mapa" onclick="javascript:mostrarTrafico();" style="color:#cc0000; font-size:11px; font-weight:bold;">Mostrar Tráfico</a>
      <a id="A2" href="#" title="No Mostrar el nivel de Tráfico en el Mapa" onclick="javascript:ocultarTrafico();" style="color:#cc0000; font-size:11px; font-weight:bold; display:none;">Ocultar Tráfico</a></td>
  </tr>
  </table>      
 </div>
 

 <div id="map_canvas" style="height:450px; width:100%">
    </div>

     <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
</asp:Content>