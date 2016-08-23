<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAddZona.aspx.vb" Inherits="GPSWeb.pAddZona" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>  
<script type="text/javascript">

    var poly, map;
    var markers = [];
    var path = new google.maps.MVCArray;   

    //definir mapa con sus opciones
 function initialize() {
     var myLatlng = new google.maps.LatLng(-34.604, -58.382);
        var myOptions = {
            zoom: 12,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map($("#map_canvas").get(0), myOptions);

        //verifico si hay un area ya cargada
        searchRecorrido(map, document.getElementById("<%= hdnzon_id.ClientID %>").value, path)
        poly = new google.maps.Polygon({
            strokeWeight: 3,
            fillColor: '#5555FF'
        });
        poly.setMap(map);
        poly.setPaths(new google.maps.MVCArray([path]));

        //asigno el evento click para recueprar los puntos que va marcando
        google.maps.event.addListener(map, 'click', addPoint); 
         
    }

    function addPoint(event) {
        path.insertAt(path.length, event.latLng);

        //agrego al hidden donde voy a guardar las cordenadas
        var rutaAcumulada = document.getElementById("<%= hdnZona.ClientID %>").value;
        jQuery('#<%= hdnZona.ClientID %>').val(rutaAcumulada + "|" + event.latLng.lat() + "," + event.latLng.lng());
        
        var marker = new google.maps.Marker({
            position: event.latLng,
            map: map,
            draggable: true
        });
        markers.push(marker);
        marker.setTitle("#" + path.length);

        google.maps.event.addListener(marker, 'click', function() {
        var rutaAcumulada = document.getElementById("<%= hdnZona.ClientID %>").value;
        document.getElementById("<%= hdnZona.ClientID %>").value = rutaAcumulada.replace("|" + marker.getPosition().lat() + "," + marker.getPosition().lng(), '');
        
            marker.setMap(null);
            for (var i = 0, I = markers.length; i < I && markers[i] != marker; ++i);
            markers.splice(i, 1);
            path.removeAt(i);
            poly.setPaths(new google.maps.MVCArray([path]));
        }
    );

        google.maps.event.addListener(marker, 'dragend', function () {
            for (var i = 0, I = markers.length; i < I && markers[i] != marker; ++i);
            path.setAt(i, marker.getPosition());
            addPoint;
        }
    );
  } 
    
 function limpiarZona() {
     poly.setMap(null);
     path = new google.maps.MVCArray;

     poly = new google.maps.Polygon({
         strokeWeight: 3,
         fillColor: '#5555FF'
     });
     poly.setMap(map);
     poly.setPaths(new google.maps.MVCArray([path]));

     clearOverlays();
     jQuery('#<%= hdnZona.ClientID %>').val('');      
        }

        // Removes the overlays from the map, but keeps them in the array
        function clearOverlays() {
            if (markers) {
                for (i in markers) {
                    markers[i].setMap(null);
                }
            }
        }
    
    //cargar mapa cuando se carga la pagina
    $(document).ready(function() {
        initialize();
    });

    //ejecuto consulta contra la componente de negocios para traer los datos de los recorridos
    function searchRecorrido(map, zon_id, path) {


        // var veh_id = '1';
        if (zon_id != "0") {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/GetArea",
                data: "{'zon_id': '" + zon_id + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var ruta = response.d;
                    $.each(ruta, function(index, paso) {

                        path.push(new google.maps.LatLng(paso.lat, paso.lng));

                        var marker = new google.maps.Marker({
                            position: new google.maps.LatLng(paso.lat, paso.lng),
                            map: map,
                            draggable: true
                        });
                        markers.push(marker);
                        marker.setTitle("#" + path.length);

                        google.maps.event.addListener(marker, 'click', function () {
                            var rutaAcumulada = document.getElementById("<%= hdnZona.ClientID %>").value;
                            document.getElementById("<%= hdnZona.ClientID %>").value = rutaAcumulada.replace("|" + marker.getPosition().lat() + "," + marker.getPosition().lng(), '');

                            marker.setMap(null);
                            for (var i = 0, I = markers.length; i < I && markers[i] != marker; ++i);
                            markers.splice(i, 1);
                            path.removeAt(i);
                            poly.setPaths(new google.maps.MVCArray([path]));
                        }
                        );

                        google.maps.event.addListener(marker, 'dragend', function () {
                            for (var i = 0, I = markers.length; i < I && markers[i] != marker; ++i);
                            path.setAt(i, marker.getPosition());
                        }
                        );

                    });
                    map.setCenter(new google.maps.LatLng(ruta[0].lat, ruta[0].lng));
                    map.setZoom(14);
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        }


    }

    //funcion que traduce la direccion en coordenadas
    function marcarDireccion() {

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
               // markersArray.push(marker);
               
            } else {
                //si no es OK devuelvo error
                alert("No podemos encontrar la direcci&oacute;n, error: " + status);
            }
        });
    }

    //funcion que simplemente actualiza los campos del formulario
    function updatePosition(latLng) {

        if (latLng.lat() != "-34.604" && latLng.lng() != "-58.382")
             path.push(new google.maps.LatLng(latLng.lat(), latLng.lng()));

     }

     //asociar enter al boton IR que busca las direcciones
     $(document).keypress(function(e) {
         if (e.keyCode == 13) {
             marcarDireccion();
             return (e.keyCode != 13); //evito que el Enter ejecute el submit del formulario
         }
     });

    

    

    
     function volveraMarcar() {
         google.maps.event.addListener(map, 'click', addPoint);
         document.getElementById("noMarcar").style.display = "inline-block";
         document.getElementById("Marcar").style.display = "none";

     }

     function dejardeMarcar() {
    
         google.maps.event.clearListeners(map, 'click');
        document.getElementById("noMarcar").style.display = "none";
        document.getElementById("Marcar").style.display = "inline-block";

    }

    function setMarketPositions() {

        if (markers) {
            document.getElementById("<%= hdnZona.ClientID %>").value = '';
            for (i in markers) {
                var rutaAcumulada = document.getElementById("<%= hdnZona.ClientID %>").value;
                document.getElementById("<%= hdnZona.ClientID %>").value = rutaAcumulada + "|" + markers[i].getPosition().lat() + "," + markers[i].getPosition().lng();

            }
        }

    }

     $(document).ready(function() {
         dejardeMarcar();
     });
 </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdncli_id" runat="server" />    
      <asp:HiddenField ID="hdnzon_id" runat="server" Value="0" />
       <asp:HiddenField ID="hdnZona" runat="server" />
   

 
 <div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Definir Zonas Genéricas</h3>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
  <br />
   <asp:Label ID="Label4" runat="server" Text="Seleccione en el Mapa un mínimo de 3 puntos para delimitar la zona que desea crear." Font-Size="14px" ForeColor="#D85639"></asp:Label>
        
 </div>
     <br />
       <div style="margin-left:50px; width:90%;"><asp:Label ID="Label2" runat="server" Text="Nombre Identificador para la Zona:" Font-Bold="true" Font-Size="12px"></asp:Label>
      
           <br />
      
       <asp:TextBox ID="txtNombre" runat="server" Width="287px" MaxLength="50"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
             <br />
             <br />
         </div>
    
         <div style="margin-left:50px; width:90%; vertical-align:middle;">
     <asp:Label ID="Label9" runat="server" Text="Buscar Direccion:" Font-Bold="true" Font-Size="12px"></asp:Label>
         &nbsp;&nbsp;
        <asp:TextBox ID="txtDireccion" runat="server" Width="590px"></asp:TextBox>
      &nbsp;<a href="#" onclick="marcarDireccion();" title="Buscar Dirección"><img src="../images/panel_buscar.png" alt="Buscar Direccion"  /></a>
       
     <a id="Limpiar" href="#" onclick="javascript:limpiarZona();" style="color:#cc0000; font-size:12px; font-weight:bold;">Limpiar Zona - </a>
      <a id="noMarcar" href="#" onclick="javascript:dejardeMarcar();" style="color:#cc0000; font-size:12px; font-weight:bold;">No Marcar Puntos</a>
    <a id="Marcar" href="#" onclick="javascript:volveraMarcar();" style="color:#cc0000; font-size:12px; font-weight:bold; display:none;">Marcar Puntos</a><br />
&nbsp;<br />
         <div id="map_canvas" style="width: 100%; height:500px;">
            </div>
      </div>
        
           <div style="text-align:right;">    <br />  
         <asp:Button ID="Button2" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAdminZonaRecorrido.aspx" CssClass="button2" />
         &nbsp;
         <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2" OnClientClick="setMarketPositions();"/>
      </div>
     
   </div>
</asp:Content>
