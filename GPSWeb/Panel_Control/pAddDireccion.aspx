<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAddDireccion.aspx.vb" Inherits="GPSWeb.pAddDireccion" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>
<script type="text/javascript">

    var map = null;
    var zoom = 16;
    var markersArray = [];
    var lat = null;
    var lng = null; 
    var geocoder = null;

    //definir mapa con sus opciones
    function initialize() {
        var nZoom = 16;
        geocoder = new google.maps.Geocoder();

        //Si hay valores creamos un objeto Latlng
        if (lat != '' && lng != '') {
            var latLng = new google.maps.LatLng(lat, lng);
            zoom = 16;
        }
        else {
            //Si no creamos el objeto con una latitud cualquiera como la de Mar del Plata, Argentina por ej
            var latLng = new google.maps.LatLng(-34.604, -58.382);
        }
        //Definimos algunas opciones del mapa a crear
        var myOptions = {
            center: latLng, //centro del mapa
            zoom: 14, //zoom del mapa
            mapTypeId: google.maps.MapTypeId.ROADMAP //tipo de mapa, carretera, híbrido,etc
        };
        //creamos el mapa con las opciones anteriores y le pasamos el elemento div
        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

      //  searchMarcas(map);
        // Relación del evento de clic sobre el mapa con el
        // procedimiento de georreferenciación inversa

        google.maps.event.addListener(map, 'click', function(event) {
            clearOverlays();
            processReverseGeocoding(event.latLng, showMarker);
        });

        //creamos el marcador en el mapa
        marker = new google.maps.Marker({
            map: map, //el mapa creado en el paso anterior
            position: latLng, //objeto con latitud y longitud
            draggable: true //que el marcador se pueda arrastrar
        });
        markersArray.push(marker);       
        //función que actualiza los input del formulario con las nuevas latitudes
        //Estos campos suelen ser hidden
        updatePosition(latLng);

        google.maps.event.addListener(marker, 'dragend', function(event) {
        clearOverlays();
            processReverseGeocoding(event.latLng, showMarker);
        });
    }
  
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
                jQuery('#<%= txtDireccion.ClientID %>').val(direccion);
            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    
    }

   

    //funcion que simplemente actualiza los campos del formulario
    function updatePosition(latLng) {

        codeLatLng(latLng.lat(), latLng.lng());

        jQuery('#<%= lat.ClientID %>').val(latLng.lat());
        jQuery('#<%= lng.ClientID %>').val(latLng.lng());

    }

    //funcion que traduce la direccion en coordenadas
    function codeAddress() {

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
                //actualizo el formulario     
                updatePosition(results[0].geometry.location);

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
   
    //cargar mapa cuando se carga la pagina
    jQuery(document).ready(function() {
        //obtenemos los valores en caso de tenerlos en un formulario ya guardado en la base de datos
        lat = jQuery('#<%= lat.ClientID %>').val();
        lng = jQuery('#<%= lng.ClientID %>').val();
        //Asignamos al evento click del boton la funcion codeAddress
        jQuery('#pasar').click(function() {
            codeAddress();
            return false;
        });
        //Inicializamos la función de google maps una vez el DOM este cargado
        initialize();
    });

   
    //asociar enter al boton IR que busca las direcciones
    $(document).keypress(function(e) {
        if (e.keyCode == 13) {
            codeAddress();
            return (e.keyCode != 13); //evito que el Enter ejecute el submit del formulario
        }
    });

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
   
   
  <div  id="draggable" style="border:2px solid #373435; float: left; width: 40%; height: auto; z-index:99; position:absolute; top:130px; background-color:White; vertical-align:middle; left:700px; border-radius: 8px 8px 8px 8px;">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">  
    <ContentTemplate>   
     <asp:HiddenField ID="hdncli_id" runat="server" />
    <asp:HiddenField ID="hdndir_id" runat="server" Value="0" />
     <asp:HiddenField ID="lat" runat="server" />
    <asp:HiddenField ID="lng" runat="server" />
  
 <div>
     <br />
<table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="5" cellpaging="5">
     <tr style="height:40px;"><td><span>Dirección Completa:</span>
     </td>
     <td> <asp:TextBox ID="txtDireccion" runat="server" Width="287px" MaxLength="100"></asp:TextBox> &nbsp;          
         <ajaxtoolkit:textboxwatermarkextender id="TBWE4" runat="server" targetcontrolid="txtDireccion" watermarktext='Av/Bv Calle Nro, Localidad, Provincia' />
              <a href="#" onclick="codeAddress();" title="Marcar Dirección en el Mapa"><img src="../images/panel_buscar.png" alt="Buscar Direccion"  /></a>
      
     </td>
    </tr>
    <tr>
    <td><asp:Label ID="Label5" runat="server" Text="Marcador:"></asp:Label></td>
   
    <td ><asp:DropDownList ID="ddlMarcador" runat="server" AutoPostBack="true" DataTextField="marc_nombre" DataValueField="marc_id">
             </asp:DropDownList> 
    <span style="font-size:11px; font-weight:normal;">
        <br />
        Seleccionar la dirección de un marcador ya creado.</span>
             </td>    
    </tr>
    <tr style="height:40px;"><td><span>Nombre Identificador:</span></td>
    <td> <asp:TextBox ID="txtNombre" runat="server" Width="260px" MaxLength="100"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
       </td>
    </tr>
    <tr>
    <td colspan="2" style="text-align:right;">
    <asp:Button ID="Button2" runat="server" Text="Volver" CssClass="button2" CausesValidation="false" PostBackUrl="~/Panel_Control/pAdminZonaRecorrido.aspx" />
     &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2"/>
    </td>
    </tr>
</table>
 </div>
  </ContentTemplate>     
 </asp:UpdatePanel>  
           
 </div>
 <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Definir Direcciones Genéricas</h3>
 <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
   <br />
   <asp:Label ID="Label6" runat="server" Text="Ingrese la Dirección Completa  o seleccionela marcando un punto en el Mapa." Font-Size="14px" ForeColor="#D85639"></asp:Label>
</div>
 <div style="float:left; width:98%; height:auto;z-index: 9999;margin-left:50px;"> <br />
          <div id="map_canvas" style="width: 95%; height: 600px;" >
            </div>
      
   </div>
 
</asp:Content>
