<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAddRecorrido.aspx.vb" Inherits="GPSWeb.pAddRecorrido" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&libraries=geometry&sensor=false"> 
</script>

<script type="text/javascript">

    var map = null;
    var polyline;
    var directionsDisplay = null;
    var directionsService = null;
    var markersArray = [];
   
    var lat = null;
    var lng = null;
    var map = null;
    var geocoder = null;
    var routes = new google.maps.MVCArray();
var path = new google.maps.MVCArray;   
    //definir mapa con sus opciones
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

      /*  google.maps.event.addListener(map, 'click', function(event) {
        processReverseGeocoding(event.latLng, showMarkerEvent);
        });*/

      
        //  searchRecorrido(map, document.getElementById("<%= hdnrec_id.ClientID %>").value, routes);
        //si es una edicion pinto la ruta
      /*  if(document.getElementById("<%= hdnrec_id.ClientID %>").value != "0")
                getRuta();*/

        searchRecorrido(map, document.getElementById("<%= hdnrec_id.ClientID %>").value, path)

        polyline = new google.maps.Polyline({
            path: path
        , map: map
        , strokeColor: '#ff0000'
        , strokeWeight: 3
        , strokeOpacity: 0.6
        , clickable: false
        });

        polyline.setMap(map);
      
        //asigno el evento click para recueprar los puntos que va marcando
        google.maps.event.addListener(map, 'click', showMarkerEvent);

    }

    function showMarkerEvent(event) {
       
         path = polyline.getPath();
        path.push(event.latLng);
        //agrego al hidden donde voy a guardar las cordenadas
        var rutaAcumulada = document.getElementById("<%= hdnRuta.ClientID %>").value;
        jQuery('#<%= hdnRuta.ClientID %>').val(rutaAcumulada + "|" + event.latLng.lat() + "," + event.latLng.lng());

        //creamos el marcador en el mapa
       var marker = new google.maps.Marker({
            map: map, //el mapa creado en el paso anterior
            position: event.latLng, //objeto con latitud y longitud
            draggable: true //que el marcador se pueda arrastrar
           
        });
        markersArray.push(marker);
        marker.setTitle("#" + path.length);
        updatePosition(event.latLng);
        google.maps.event.addListener(marker, 'click', function() {
            marker.setMap(null);
            //al eliminar un punto elimino los que tengo delante de este
            for (var i = 0, I = markersArray.length; i < I && markersArray[i] != marker; ++i);
            //borro el hidden
            var rutaAcumulada = document.getElementById("<%= hdnRuta.ClientID %>").value;           
            document.getElementById("<%= hdnRuta.ClientID %>").value = rutaAcumulada.replace("|" + marker.getPosition().lat() + "," + marker.getPosition().lng(), '');
            markersArray.splice(i, 1);
            path.removeAt(i);
            limpiarRecorrido(i);
           
        }
      );
      
      google.maps.event.addListener(marker, 'dragend', function() {
            for (var i = 0, I = markersArray.length; i < I && markersArray[i] != marker; ++i);
            path.setAt(i, marker.getPosition());
        });

    }
    //http://blog.koalite.com/2011/11/modificar-una-clase-en-javascript-eliminar-los-markers-de-google-maps/
    //http://stackoverflow.com/questions/1544739/google-maps-api-v3-how-to-remove-all-markers

    function limpiarRecorrido(position) {
        if (markersArray) {
            for (var j = position; j < markersArray.length; j++) {
                var rutaAcumulada = document.getElementById("<%= hdnRuta.ClientID %>").value;
                document.getElementById("<%= hdnRuta.ClientID %>").value = rutaAcumulada.replace("|" + markersArray[j].getPosition().lat() + "," + markersArray[j].getPosition().lng(), '');
                markersArray[j].setMap(null);
                markersArray.splice(j, 1);
                path.removeAt(j); 
               
            }    
           // alert(markersArray.length);
            //alert(path.length);      
        }
        
    }

  function limpiarRuta() {

      clearOverlays();
     
   polyline.setMap(null);

       polyline = new google.maps.Polyline({
           path: routes
            , map: map
            , strokeColor: '#ff0000'
            , strokeWeight: 3
            , strokeOpacity: 0.6
            , clickable: false
       });
       jQuery('#<%= hdnRuta.ClientID %>').val('');
       directionsDisplay.setMap(null);
       directionsDisplay = null;


   }

   //ejecuto consulta contra la componente de negocios para traer los datos de los recorridos
   function searchRecorrido(map, rec_id, path) {

       if (rec_id != "0") {
           $.ajax({
               async: false,
               type: 'POST',
               url: "wsDatos.asmx/GetRecorrido",
               data: "{'rec_id': '" + rec_id + "'}",
               contentType: 'application/json; charset=utf-8',
               dataType: 'json',
               success: function (response) {
                   var puntos = response.d;
                   $.each(puntos, function (index, paso) {

                       path.push(new google.maps.LatLng(paso.lat, paso.lng));
                       //agrego al hidden donde voy a guardar las cordenadas
                     //  var rutaAcumulada = document.getElementById("<%= hdnRuta.ClientID %>").value;
                     //  jQuery('#<%= hdnRuta.ClientID %>').val(rutaAcumulada + "|" + paso.lat + "," + paso.lng);

                       //creamos el marcador en el mapa
                       var marker = new google.maps.Marker({
                           map: map, //el mapa creado en el paso anterior
                           position: new google.maps.LatLng(paso.lat, paso.lng), //objeto con latitud y longitud
                           draggable: true //que el marcador se pueda arrastrar

                       });
                       markersArray.push(marker);
                       marker.setTitle("#" + path.length);
                     //  updatePosition(new google.maps.LatLng(paso.lat, paso.lng));
                       google.maps.event.addListener(marker, 'click', function () {
                           marker.setMap(null);
                           //al eliminar un punto elimino los que tengo delante de este
                           for (var i = 0, I = markersArray.length; i < I && markersArray[i] != marker; ++i);
                           //borro el hidden
                           var rutaAcumulada = document.getElementById("<%= hdnRuta.ClientID %>").value;
                           document.getElementById("<%= hdnRuta.ClientID %>").value = rutaAcumulada.replace("|" + marker.getPosition().lat() + "," + marker.getPosition().lng(), '');
                           markersArray.splice(i, 1);
                           path.removeAt(i);
                           limpiarRecorrido(i);

                       }
      );

                       google.maps.event.addListener(marker, 'dragend', function () {
                           for (var i = 0, I = markersArray.length; i < I && markersArray[i] != marker; ++i);
                           path.setAt(i, marker.getPosition());
                       });

                   });
                   map.setCenter(new google.maps.LatLng(puntos[0].lat, puntos[0].lng));
                   map.setZoom(13);
               },
               error: function (jqXHR, textStatus, errorThrown) {
                   alert(textStatus + ": " + XMLHttpRequest.responseText);
               }
           });
       }


   }

 function getRuta() {
           var start = $('#<%= txtDireccionOrigen.ClientID %>').val();
            var end = $('#<%= txtDireccionDestino.ClientID %>').val();


            if (!start || !end) {
                alert("Ingrese la Dirección de origen y destino");
                return;
            }
            else {
                if (start == "Av/Bv Calle Nro, Localidad, Provincia") {
                    alert("Ingrese la Dirección de origen.");
                    return;
                }
                if (end == "Av/Bv Calle Nro, Localidad, Provincia") {
                    alert("Ingrese la Dirección de destino.");
                    return;
                }
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
                  //  directionsDisplay.setPanel($("#directions_panel").get(0));
                    directionsDisplay.setDirections(response);
                } else {
                    alert("No existe la ubicación ingresada.");
                }
            });

        }

     

        //funcion que simplemente actualiza los campos del formulario
        function updatePosition(latLng) {

            if (latLng.lat() != "-34.604" && latLng.lng() != "-58.382")
                codeLatLng(latLng.lat(), latLng.lng());
        }
        
        //pongo la direccion en el text
        function codeLatLng(lat, lng) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/GetDireccion",
                data: "{'lat': '" + lat + "','lng': '" + lng + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var direccion = response.d;
                    alert(document.getElementById('<%= txtDireccionOrigen.ClientID %>').value);
                    if (document.getElementById('<%= txtDireccionOrigen.ClientID %>').value == 'Av/Bv Calle Nro, Localidad, Provincia' || document.getElementById('<%= txtDireccionOrigen.ClientID %>').value == '') {
                        document.getElementById('<%= txtDireccionOrigen.ClientID %>').value = direccion;
                        document.getElementById('<%= hdnDir1.ClientID %>').value = direccion;
                    }
                    else {
                        document.getElementById('<%= txtDireccionDestino.ClientID %>').value = direccion;
                        document.getElementById('<%= hdnDir2.ClientID %>').value = direccion;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

        }


        //asociar enter al boton IR que busca las direcciones
        $(document).keypress(function(e) {
            if (e.keyCode == 13) {
                getRuta();
                return (e.keyCode != 13); //evito que el Enter ejecute el submit del formulario
            }
        });
        //ejecuto consulta contra la componente de negocios para traer los datos de los recorridos
        /*function searchRecorrido(map, rec_id, routes) {
         

            // var veh_id = '1';

            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/GetAlertaRecorrido",
                data: "{'rec_id': '" + rec_id + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var ruta = response.d;
                    $.each(ruta, function(index, paso) {

                    routes.push(new google.maps.LatLng(paso.lat, paso.lng));

                    });
                    map.setCenter(new google.maps.LatLng(ruta[0].lat, ruta[0].lng));
                    map.setZoom(12);
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });


        }*/
        
    //cargar mapa cuando se carga la pagina
    $(document).ready(function() {
        initialize();
        dejardeMarcar();
    });

    function mostrar() {

        if (document.getElementById("rutina").style.display == "inline-block") {
            document.getElementById("rutina").style.display = "none";
        }
        else {
            document.getElementById("rutina").style.display = "inline-block"
        }
    }

    

    function volveraMarcar() {
        google.maps.event.addListener(map, 'click', showMarkerEvent);
        document.getElementById("noMarcar").style.display = "inline-block";
        document.getElementById("Marcar").style.display = "none";

    }

    function dejardeMarcar() {
        google.maps.event.clearListeners(map, 'click');
  
        document.getElementById("noMarcar").style.display = "none";
        document.getElementById("Marcar").style.display = "inline-block";

    }


    function setMarketPositions()
    {
       
       if (markersArray) {
           document.getElementById("<%= hdnRuta.ClientID %>").value = '';
                for (i in markersArray) {
                         var rutaAcumulada = document.getElementById("<%= hdnRuta.ClientID %>").value;
                        document.getElementById("<%= hdnRuta.ClientID %>").value = rutaAcumulada + "|" + markersArray[i].getPosition().lat() + "," + markersArray[i].getPosition().lng();
             
                }
            }
        
    }


 </script>
 <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>

  <script type="text/javascript">

      $(init);

      function init() {
          $('#draggable').draggable();
      }
 
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdncli_id" runat="server" />
      <asp:HiddenField ID="hdnrec_id" runat="server" Value="0" />
       <asp:HiddenField ID="hdnRuta" runat="server" Value="0"/>
        <asp:HiddenField ID="hdnDir1" runat="server" Value=""/>
         <asp:HiddenField ID="hdnDir2" runat="server" Value=""/>
    <div  id="draggable" style="border:2px solid #373435; float: left; width: 35%; height: auto; z-index:99; position:absolute; top:130px; background-color:White; vertical-align:middle; left:660px; border-radius: 8px 8px 8px 8px;">
     <br />
       <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="5" cellpaging="5">
     <tr style="height:40px;"><td><span> Nombre Recorrido:</span>
     </td>
     <td>
     <asp:TextBox ID="txtNombre" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
     </td>
     </tr>
      <tr style="height:40px;"><td><span>Dirección Origen:</span>
     </td>
     <td><asp:TextBox ID="txtDireccionOrigen" runat="server" Width="250px"></asp:TextBox>
            <ajaxtoolkit:textboxwatermarkextender id="TBWE4" runat="server" targetcontrolid="txtDireccionOrigen" watermarktext='Av/Bv Calle Nro, Localidad, Provincia' /></td>
     </tr>
      <tr style="height:40px;"><td><span>Dirección Destino:</span>
     </td>
     <td> <asp:TextBox ID="txtDireccionDestino" runat="server" Width="250px"></asp:TextBox>
            <ajaxtoolkit:textboxwatermarkextender id="TBWE5" runat="server" targetcontrolid="txtDireccionDestino" watermarktext='Av/Bv Calle Nro, Localidad, Provincia' />
            <a href="#" onclick="getRuta();" title="Marcar Recorrido en el Mapa"><img src="../images/panel_buscar.png" alt="Buscar Direccion"  /></a>
            </td>
     </tr>
     
     <tr><td colspan="2">    </td></tr>
    <tr><td colspan="2"  style="text-align:right;"><asp:Button ID="Button2" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAdminZonaRecorrido.aspx" CssClass="button2" />
             &nbsp;
             <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2" OnClientClick="setMarketPositions();"/></td></tr>
    </table>

 </div>
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Definir Recorridos Genéricos</h3>
   <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>

       <br />

   <asp:Label ID="Label1" runat="server" Text="Ingrese la Dirección de Origen y de Destino del Recorrido o seleccione los puntos del recorrido gráficamente en el Mapa." Font-Size="14px" ForeColor="#D85639"></asp:Label>
       <br />
   <br />
     <input id="Limpiar" type="button" value="Limpiar Ruta" class="button2" onclick="javascript:limpiarRuta();" style="width:100px;"/>
      <a id="noMarcar" href="#" onclick="javascript:dejardeMarcar();" style="color:#4D4F4F; font-size:12px; font-weight:bold;">No Marcar Puntos</a>
    <a id="Marcar" href="#" onclick="javascript:volveraMarcar();" style="color:#4D4F4F; font-size:12px; font-weight:bold;display:none;">Marcar Puntos</a>
 </div>
   <div style="float: left; width:92%; height:auto;margin-left:50px;">
       <div> 
   <br />
      </div>
         <div id="map_canvas" style="width:100%; height:600px;">
            </div>
         
   </div> 
     
</asp:Content>