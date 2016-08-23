<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pHistorialRecorridos.aspx.vb" Inherits="GPSWeb.pHistorial" ValidateRequest="false" MasterPageFile="~/Panel_Control/Site2.Master" Culture="Auto" UICulture="Auto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&libraries=geometry&sensor=false"> 
</script>
 <div id="prueba" runat ="server" >
<script type="text/javascript">

    var direcciones = new Array();
    var ruta = new Array();
    var moviles = new Array();
    var polyline;
    var aLatlng = [];
    var markersArray = [];
    
   // var directionsDisplay = null;
   // var directionsService = null;
    var map = null;
    var geocoder = null;   

    //busco todos los moviles de la empresa para consultar
    function getMoviles() {
        var cli_id = document.getElementById("<%= hdncli_id.ClientID %>").value
        $.ajax({
            async: false,
            type: 'POST',
            url: "wsDatos.asmx/GetVehiculosActivos",
            data: "{'cli_id': '" + cli_id + "'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function(response) {
                var vehiculos = response.d;
                $.each(vehiculos, function(index, movil) {

                    moviles.push(movil.id);
                });
            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert('GetMoviles: ' + textStatus + ": " + XMLHttpRequest.responseText);
            }
        });
    }

    function mostrarRecorrido() {
        //ejecuto la consulta contra el negocio para traer los recorridos
        //recupero los parametros para aplicar el filtro, fechas y id vehiculo
        moviles = new Array();

        var myOptions = {
            center: new google.maps.LatLng(-35, -64),
            zoom: 13,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

        //voy a buscar en al base de datos los puntos del recorrido del vehiculo seleccionado
        //tengo que recorrer los elements y ver cual selecciono para consultar por cada id de movil
        var checkboxes = document.getElementById("aspnetForm").chkVehiculo;

        var cont = 0;
        if (checkboxes != null) {
            for (var x = 0; x < checkboxes.length; x++) {
                if (checkboxes[x].checked) {
                    moviles.push(checkboxes[x].value)
                    cont = cont + 1;
                }
            }
        }
        else {
            //tengo un movil preseleccionado
            moviles.push(document.getElementById("<%= hdnveh_id.ClientID %>").value)
            cont = 1;
        }

        //si no selecciono nada voy a tener que consultar cuantos moviles tiene el cliente

        if (cont == 0)
            getMoviles();

       
        //tengo que hacer un var lineas por cada vehiculo
        for (var i = 0; i < moviles.length; i++) {           
            searchRecorrido(map, moviles[i]);
            //uno las lineas del recorrido
            if (ruta.length > 0) {
                var lineas = new google.maps.Polyline({
                    path: ruta,
                    map: map,
                    strokeColor: dame_color_aleatorio(),
                    strokeWeight: 3,
                    strokeOpacity: 0.6,
                    clickable: false
                });


                var latlngbounds = new google.maps.LatLngBounds();
                for (var j = 0; j < aLatlng.length; j++) {
                    latlngbounds.extend(aLatlng[j]);
                }
                map.fitBounds(latlngbounds);
                map.setZoom(13);
            }
        }
    }

    function dame_color_aleatorio() {
        hexadecimal = new Array("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F")
        color_aleatorio = "#";
        for (h = 0; h < 6; h++) {
            posarray = aleatorio(0, hexadecimal.length)
            color_aleatorio += hexadecimal[posarray]
        }
        return color_aleatorio
    }

    function aleatorio(inferior, superior) {
        numPosibilidades = superior - inferior
        aleat = Math.random() * numPosibilidades
        aleat = Math.floor(aleat)
        return parseInt(inferior) + aleat
    } 

    function createMarker(map, lng, lat, fecha, hora, ubicacion, velocidad, patente) {
        marker = new google.maps.Marker({
            map: map,
            icon: '../images/red-point.png',
            position: new google.maps.LatLng(lat, lng),
            title: patente + ' - ' + fecha + '-' + hora + '- ' + velocidad + 'km/h' + ' - ' + ubicacion
        });
        return marker;
    }

    //ejecuto consulta contra la componente de negocios para traer los datos de los recorridos
    function searchRecorrido(map, veh_id) {

        var direcciones = new Array();
        
        var fechaDesde = document.getElementById("<%= txtfechadesde.ClientID %>").value;
        var fechaHasta = document.getElementById("<%= txtfechahasta.ClientID %>").value;
        var horaDesde = document.getElementById("<%= ddlhoraDesde.ClientID %>").value + ':' + document.getElementById("<%= ddlMinDesde.ClientID %>").value;
        var horaHasta = document.getElementById("<%= ddlhoraHasta.ClientID %>").value + ':' + document.getElementById("<%= ddlMinHasta.ClientID %>").value;
        var cli_id = document.getElementById("<%= hdncli_id.ClientID %>").value
        ruta = new Array();

        // var veh_id = '1';

        $.ajax({
            async: false,
            type: 'POST',
            url: "wsDatos.asmx/GetRecorridos",
            data: "{'cli_id': '" + cli_id + "','veh_id':'" + veh_id + "','fecha_desde':'" + fechaDesde + "','fecha_hasta':'" + fechaHasta + "', 'hora_desde':'" + horaDesde + "', 'hora_hasta':'" + horaHasta + "' }",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function(response) {
                var vehiculos = response.d;
                if (moviles.length != 0)
                {
                   // alert('No se Encontraron Datos para el Vehiculo ID: ' + veh_id);*/
                   // alert(vehiculos.length);
                    $.each(vehiculos, function(index, movil) {                    
                        ruta.push(new google.maps.LatLng(movil.lat, movil.lng));
                        //direcciones.push(new google.maps.LatLng(movil.lat, movil.lng))
                        var marker = createMarker(map, movil.lng, movil.lat, movil.fecha, movil.hora, movil.ubicacion, movil.velocidad, movil.patente);

                    });
                    if (vehiculos.length != 0)
                        aLatlng.push(new google.maps.LatLng(vehiculos[0].lat, vehiculos[0].lng))
                }
                if (moviles.length == 1 && vehiculos.length == 0)
                      alert('No se Encontraron Datos para el Vehiculo en las fechas consultadas.')
              //  map.setCenter(new google.maps.LatLng(vehiculos[0].lat, vehiculos[0].lng));
               // map.setZoom(9);

                /* for (j = 0; j < direcciones.length; j++) {

                    if ((j + 1) < direcciones.length)
                paintRuta(map, direcciones[j], direcciones[j + 1]);
                }*/


            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert('Buscar Recorrido: ' +textStatus + ": " + XMLHttpRequest.responseText);
            }
        });
    }
    
    //carga inicial del mapa
    function initialize() {
        var latLng = new google.maps.LatLng(-35, -64);
        var myOptions = {
        center: latLng,
            zoom: 6,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

         map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);        
        //centro el mapa en la ubicacion del usuario
        // map.setCenter(-31.411162, -64.162903);
     }
    
    $(document).ready(function() {
        initialize();
    });

    //pintar la ruta de recorrido a partir de dos puntos latlng
   /* function paintRuta(map, start, end) {
        directionsDisplay = new google.maps.DirectionsRenderer();
        directionsService = new google.maps.DirectionsService();
        
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
                //  directionsDisplay.setPanel($("#directions_panel").get(0));
                directionsDisplay.setDirections(response);
            } else {
               // alert("No existe la ubicación ingresada.");
            }
        });

    }*/

    function seleccionarTodos() {
        var checkboxes = document.getElementById("aspnetForm").chkVehiculo;
        if (checkboxes != null)
            for (var x = 0; x < checkboxes.length; x++) {
               checkboxes[x].checked = true;                   
                }
            }

            function DeseleccionarTodos() {
                var checkboxes = document.getElementById("aspnetForm").chkVehiculo;
                if (checkboxes != null)
                    for (var x = 0; x < checkboxes.length; x++) {
                     checkboxes[x].checked = false;
                   
                }
            }

            //funcion que traduce la direccion en coordenadas
            function marcarDireccion() {
                clearOverlays();
                if (document.getElementById("<%= txtDireccion.ClientID %>").value != "") {
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
                            map.setZoom(15);
                            //coloco el marcador en dichas coordenadas
                            marker.setPosition(results[0].geometry.location);
                            markersArray.push(marker);
                            //Añado un listener para cuando el markador se termine de arrastrar
                            //actualize el formulario con las nuevas coordenadas
                            google.maps.event.addListener(marker, 'dragend', function() {
                                //updatePosition(marker.getPosition());
                                // markersArray.push(marker);

                            });
                        } else {
                            //si no es OK devuelvo error
                            alert("No podemos encontrar la direcci&oacute;n ingresada. Verifique.");
                        }
                    });
                }
                else {
                    alert('Ingrese la Dirección a Buscar');
                }
            }

            //asociar enter al boton IR que busca las direcciones
            $(document).keypress(function(e) {
                if (e.keyCode == 13) {
                    //si presiono enter y la direccion esta vacia ejecuto el evento del boton Ver Recorridos
                    if (document.getElementById("<%= txtDireccion.ClientID %>").value != "")
                        marcarDireccion();
                    else
                        mostrarRecorrido();
                }
                return (e.keyCode != 13); //evito que el Enter ejecute el submit del formulario
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
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <asp:HiddenField ID="hdnveh_id" runat="server" />
    <div class="inline" style="border: thin solid #00A6C6; float: left; width: 25%; height: 100%">
     <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label><br />
      <asp:Label ID="Label9" runat="server" Text="Consulta de Recorridos para un Rango de Fechas." Font-Size="14px" Font-Bold="true"></asp:Label><br />
      <br />
     <asp:Label ID="lblMovil" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue"></asp:Label>
        <br />
        <br />
     <asp:Label ID="Label1" runat="server" Text="Seleccione Rango de Fechas y Vehiculos:" Font-Bold="true"></asp:Label><br />
     <br /> 
     <asp:Label ID="Label2" runat="server" Text="Desde:" Font-Bold="true"></asp:Label>
        &nbsp;
        &nbsp;<asp:TextBox ID="txtfechadesde" runat="server" Width="89px"></asp:TextBox> 
               <ajaxtoolkit:calendarextender ID="CalendarExtender2" runat="server" 
         TargetControlID="txtfechadesde" PopupButtonID="txtfechadesde"/>
         <asp:Label ID="Label4" runat="server" Text="Hora:" Font-Bold="true"></asp:Label>
         &nbsp;<asp:DropDownList ID="ddlhoraDesde" runat="server" Width="72px">
         <asp:ListItem Value="00">Hora</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList> 
        <asp:DropDownList ID="ddlMinDesde" runat="server" Width="72px">
         <asp:ListItem Value="00">Min</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>           
        </asp:DropDownList>
     <br /><asp:Label ID="Label3" runat="server" Text="Hasta:" Font-Bold="true"></asp:Label>
     &nbsp;&nbsp;
     &nbsp;<asp:TextBox ID="txtfechahasta" runat="server" Width="89px"></asp:TextBox>
     <ajaxtoolkit:calendarextender ID="CalendarExtender1" runat="server" TargetControlID="txtfechahasta" PopupButtonID="txtfechahasta"/>
      <asp:Label ID="Label5" runat="server" Text="Hora:" Font-Bold="true"></asp:Label>
         &nbsp;
         <asp:DropDownList ID="ddlhoraHasta" runat="server" Width="72px">
          <asp:ListItem Value="23">Hora</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList> 
        <asp:DropDownList ID="ddlMinHasta" runat="server" Width="72px">
         <asp:ListItem Value="59">Min</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>           
        </asp:DropDownList> 
        <br />
     <br />
    <asp:Label ID="Label6" runat="server" Text="Vehiculo:" Font-Bold="true"></asp:Label>
        <asp:Panel ID="PanelMoviles" runat="server">
        
      
         <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" RepeatColumns="2" RepeatDirection="Horizontal">
             <ItemTemplate>
                 <input id="chkVehiculo" name="chkVehiculo" type="checkbox" value='<%# Eval("veh_id")%>' />
                  <asp:Label ID="Label2" runat="server" Text='<%# Eval("veh_descripcion")%>' ></asp:Label>-
                   <asp:Label ID="Label7" runat="server" Text='<%# Eval("veh_patente")%>' ></asp:Label>                 
             </ItemTemplate>
            </asp:DataList>
            <br />
            <a href="#" onclick="seleccionarTodos();" style="color:#ccc000; font-size:12px;">Tildar Todos </a>
             <a href="#" onclick="DeseleccionarTodos();" style="color:#ccc000; font-size:12px;">/ Destildar Todos</a>
        </asp:Panel>
      
       
        <br />
        <input id="Button1" type="button" value="Ver Historial" onclick="mostrarRecorrido();"/>
        <div id ="directionpanel"  style="height: 290px;overflow: auto" ></div>
        </div>
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 73%; height:100%">
  <div>
     <asp:Label ID="Label8" runat="server" Text="Buscar Direccion:" Font-Bold="true"></asp:Label>
         &nbsp;&nbsp;
        <asp:TextBox ID="txtDireccion" runat="server" Width="603px"></asp:TextBox>
      &nbsp;<input id="btnDireccion" type="button" value="Ir" onclick="marcarDireccion();" />
       <a id="vertrafico" href="#" onclick="javascript:mostrarTrafico();" style="color:#cc0000; font-size:12px; font-weight:bold;">Mostrar Tráfico</a>
      <a id="ocultartrafico" href="#" onclick="javascript:ocultarTrafico();" style="color:#cc0000; font-size:12px; font-weight:bold; display:none;">Ocultar Tráfico</a>
   
         <br />
    </div>
<div id="map_canvas" style="width: 95%; height:600px">  </div>
    </div>
    
</asp:Content>