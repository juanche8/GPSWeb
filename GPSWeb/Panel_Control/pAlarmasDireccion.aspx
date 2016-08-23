<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmasDireccion.aspx.vb" Inherits="GPSWeb.pAlarmasDireccion" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>
<link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
    
 <script src="../scripts/ui/jquery.ui.tabs.min.js" type="text/javascript"></script>
<script type="text/javascript">

    var map = null;
    var zoom = 15;
    var markersArray = [];
    var lat = null;
    var lng = null; 
    var geocoder = null;

    //definir mapa con sus opciones
    function initialize() {
        var nZoom = 13;
        geocoder = new google.maps.Geocoder();

        //Si hay valores creamos un objeto Latlng
        if (lat != '' && lng != '') {
            var latLng = new google.maps.LatLng(lat, lng);
            zoom = 14;
        }
        else {
            //Si no creamos el objeto con una latitud cualquiera como la de Mar del Plata, Argentina por ej
            var latLng = new google.maps.LatLng(-34.604, -58.382);
        }
        //Definimos algunas opciones del mapa a crear
        var myOptions = {
            center: latLng, //centro del mapa
            zoom: 12, //zoom del mapa
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
      /*  jQuery('#pasar').click(function() {
            codeAddress();
            return false;
        });*/
        //Inicializamos la función de google maps una vez el DOM este cargado
        initialize();
    });

  
   

    function mostrar() {

        if (document.getElementById("rutina").style.display == "block") {
            document.getElementById("rutina").style.display = "none";
        }
        else {
            document.getElementById("rutina").style.display = "block"
        }
    }

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
    <div style="margin-left:50px; width:90%;height:auto;">
 <h4>Configurar Alarma de Desvío de Direcciones</h4>
 </div>
  <div style="width: 98%; height:auto;">
    <div id="map_canvas" style="width:100%; height: 750px;">
            </div> 
   </div> 
  <div  id="draggable" style="border:2px solid #373435; float: left; width: 45%; height: auto; z-index:99; position:absolute; top:90px; background-color:White; vertical-align:middle; left:710px; border-radius: 8px 8px 8px 8px;">
  
       <div id="tabs" style="width:98%">
    <br />
                            <ul>
                            <li><a href="#tabs-1">Principal</a></li>  
                                <li><a href="#tabs-2">Periodo</a></li>
                             <li><a href="#tabs-3">Días de la Semana</a></li>                            
                            </ul>
<div id="tabs-1" style="width:98%;">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">  
    <ContentTemplate>
      <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
    <asp:HiddenField ID="hdndir_id" runat="server" Value="0" />
     <asp:HiddenField ID="hdnadir_id" runat="server" Value="0" />
     <asp:HiddenField ID="lat" runat="server" />
    <asp:HiddenField ID="lng" runat="server" />
 <br />&nbsp;
 <asp:Label ID="Label17" runat="server" Font-Bold="true" Text="Seleccione él o los móviles a los que quiera asignales la alarma y defina la dirección a controlar (se le informara el ingreso y la salida de dicha dirección)." Font-Size="13px" ></asp:Label><br />

 <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="13px"></asp:Label>
 
       <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">
     <tr><td><asp:Label ID="lblMovil" runat="server" Text="Moviles:"></asp:Label>
     </td>
     </tr>
     <tr><td colspan="2">  <asp:Panel ID="PanelGrupo" runat="server" Visible ="true">
       <asp:Label ID="Label16" runat="server" Text="Filtrar por Grupo:" Font-Bold="true"></asp:Label>   
         &nbsp;<asp:DropDownList ID="ddlgrupo" runat="server" AutoPostBack="true" DataTextField="grup_nombre" DataValueField="grup_id">
         </asp:DropDownList>
         &nbsp;&nbsp; 
         <asp:CheckBox ID="chkTodos" runat="server" Text="Ver todos los Moviles" Font-Bold="true" AutoPostBack="true" />
         <br />
     </asp:Panel></td>
     <tr><td colspan="2">
      <asp:Panel ID="PanelMoviles" runat="server" Visible="false">
   <div style="height:200px;overflow-y: scroll; font-family:Arial;width:320px; border-color:LightGray; border-width:1px; border-style:solid;">       
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="5" CellPadding="5"  Font-Names="Arial" Font-Bold="false" Width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="chkMoviles" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
     
</asp:Panel> 
<div>
      <br />
         <asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton>
      </div>
     </td>
     
     </tr>
     <tr><td colspan="2"><asp:Label ID="Label14" runat="server" Text="Para usar una Dirección ya creada selecciónela del siguiente listado:" Font-Bold="true"></asp:Label></td></tr>
     <tr><td> <asp:DropDownList ID="ddlDireccion" runat="server" DataValueField="dir_id" DataTextField="dir_nombre" AutoPostBack="true">
             </asp:DropDownList>  </td></tr>
   <tr><td colspan="2"><asp:Label ID="Label9" runat="server" Text="También puede seleccionar la dirección de un Marcador creado:"></asp:Label></td></tr>
   <tr><td colspan="2">   <asp:DropDownList ID="ddlMarcador" runat="server" AutoPostBack="true" DataTextField="marc_nombre" DataValueField="marc_id">
             </asp:DropDownList></td></tr>
<tr><td colspan="2">
        <asp:Label ID="Label2" runat="server" Text="Nueva Dirección:" Font-Bold="true"></asp:Label><br />
         <asp:Label ID="Label4" runat="server" Text="Ingrese la Direccion Completa o seleccionela en el mapa." Font-Bold="false"></asp:Label> 
             </td></tr>
<tr><td colspan="2">
  <asp:TextBox ID="txtDireccion" runat="server" Width="350px" MaxLength="100"></asp:TextBox> &nbsp; 
   <a href="#" onclick="codeAddress();" title="Marcar Dirección en el Mapa"><img src="../images/panel_buscar.png" alt="Buscar Direccion"  /></a>
 <ajaxtoolkit:textboxwatermarkextender id="TBWE4" runat="server" targetcontrolid="txtDireccion" watermarktext='Av/Bv Calle Nro, Localidad, Provincia' />
</td></tr>
<tr><td><asp:Label ID="Label1" runat="server" Text="Nombre Identificador de la dirección" Font-Bold="true"></asp:Label></td>
<td colspan="2"> <asp:TextBox ID="txtNombre" runat="server" Width="200px" MaxLength="100"></asp:TextBox></td>
</tr>

<tr><td><asp:Label ID="Label3" runat="server" Text="Notificar por e-mail:" Font-Bold="true"></asp:Label></td>
<td colspan="2">  <asp:CheckBox ID="chkMail" runat="server" Text="Si" Checked="true" /></td>
</tr>
  <tr><td> <asp:Label ID="Label13" runat="server" Text="Umbral de Desvio (Kms):" Font-Bold="true"></asp:Label></td>
    <td> <asp:TextBox ID="txtKms" runat="server" Width="96px" Text="10"></asp:TextBox></td>
    </tr>
<tr><td><asp:Label ID="Label15" runat="server" Text="Alarma Activa:" Font-Bold="true"></asp:Label></td>
<td colspan="3">  <asp:RadioButtonList ID="rdnActiva" runat="server" RepeatDirection="Horizontal">
                 <asp:ListItem Value="True" Selected="True">Si</asp:ListItem>
                 <asp:ListItem Value="False" >No</asp:ListItem>
             </asp:RadioButtonList></td>
</tr>
</table>

  </ContentTemplate>   
   </asp:UpdatePanel> 
 </div> 

 <div id="tabs-2" style="width:100%;">
   <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial" cellspacing="8" cellpaging="5">
<tr><td colspan="2"><asp:Label ID="Label6" runat="server" Text="Frecuencia de Monitoreo:" Font-Bold="true"></asp:Label><br />
         <asp:Label ID="Label7" runat="server" Text="Verificar los desvios para los Días y horarios especificados." Font-Bold="false"></asp:Label></td>
</tr>
<tr><td> <asp:Label ID="Label10" runat="server" Text="Inica el Día:"></asp:Label></td>
<td colspan="2"><asp:TextBox ID="txtFechaDesde" runat="server" Width="96px"></asp:TextBox>
             <ajaxtoolkit:calendarextender ID="CalendarExtender3" runat="server"  CssClass="black"
                 TargetControlID="txtFechaDesde" PopupButtonID="txtFechaDesde"/> </td>
</tr>
<tr><td> <asp:Label ID="Label11" runat="server" Text="Finaliza el Día:"></asp:Label></td>
<td colspan="2"><asp:TextBox ID="txtFechaHasta" runat="server" Width="99px"></asp:TextBox>
              <ajaxtoolkit:calendarextender ID="CalendarExtender1" runat="server"  CssClass="black"
                 TargetControlID="txtFechaHasta" PopupButtonID="txtFechaHasta"/></td>
</tr>
 <tr><td>Entre las</td>
     <td colspan="2">  <asp:DropDownList ID="ddlHoraDesde" runat="server" Width="51px">
        <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem>
         <asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem>
         <asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem>
         <asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem>
         <asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem>
         <asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem>
         <asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem>
         <asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem>
          <asp:ListItem>23</asp:ListItem>
         </asp:DropDownList>
         <asp:DropDownList ID="ddlMinDesde" runat="server" Width="51px">
       <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
        <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
        <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem>
        <asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem>
        <asp:ListItem>55</asp:ListItem> <asp:ListItem>59</asp:ListItem></asp:DropDownList>
       y las&nbsp;
      <asp:DropDownList ID="ddlHorahasta" runat="server" Width="51px">
         <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem>
         <asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem>
         <asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem>
         <asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem>
         <asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem>
         <asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem>
         <asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem>
         <asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem>
         </asp:DropDownList>
         <asp:DropDownList ID="ddlMinHasta" runat="server" Width="51px">
        <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
        <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
        <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem>
        <asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem>
        <asp:ListItem>55</asp:ListItem></asp:DropDownList>
        Hs.
        </td>

        
     </tr>

</table>
</div>
<div id="tabs-3" style="width:100%;">
 <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">
<tr><td colspan="4">  <div id="rutina" >
            <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="5" cellpaging="5">
               <tr><td colspan="4"><asp:Label ID="Label12" runat="server" Text="Frecuencia para recorridos de Rutina"></asp:Label></td></tr>
               <tr><td> <asp:CheckBox ID="chkLunes" runat="server" Text="Lunes entre las "/></td>
               <td><asp:DropDownList ID="ddlhoraDesdeL" runat="server" Width="55px">
            
            <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlMinDesdeL" runat="server" Width="55px">            
         <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
         <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
         <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem>
         <asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem>
         <asp:ListItem>55</asp:ListItem></asp:DropDownList></td>
               <td>y las</td>
               <td> <asp:DropDownList ID="ddlhoraHastaL" runat="server" Width="55px">
           <asp:ListItem>23</asp:ListItem>
            <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem></asp:DropDownList>
            <asp:DropDownList ID="ddlMinHastaL" runat="server" Width="55px">
             <asp:ListItem>59</asp:ListItem>
         <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
         <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
         <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem>
         <asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem>
         </asp:DropDownList>&nbsp;Hs.</td>
               </tr>
               <tr><td>  <asp:CheckBox ID="chkMartes" runat="server" Text="Martes entre las " /></td>
               <td> <asp:DropDownList ID="ddlhoraDesdeM" runat="server" Width="55px">
            <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminDesdeM" runat="server" Width="55px">
         <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList></td>
               <td>y las</td>
               <td> <asp:DropDownList ID="ddlhoraHastaM" runat="server" Width="55px">
         <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminHastaM" runat="server" Width="55px">
        <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList>&nbsp;Hs.</td>
               </tr>
               <tr><td>  <asp:CheckBox ID="chkMiercoles" runat="server" Text="Miercoles entre las" /></td>
               <td>  <asp:DropDownList ID="ddlhoraDesdeMi" runat="server" Width="55px">      
            <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminDesdeMi" runat="server" Width="55px">       
         <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList></td>
               <td>y las</td>
               <td> <asp:DropDownList ID="ddlhoraHastaMi" runat="server" Width="55px">
          <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminHastaMi" runat="server" Width="55px">
        <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList>&nbsp;Hs.</td>
               </tr>
               <tr><td> <asp:CheckBox ID="chkJueves" runat="server" Text="Jueves entre las" /></td>
               <td> <asp:DropDownList ID="ddlhoraDesdeJ" runat="server" Width="50px">
         <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminDesdeJ" runat="server" Width="55px">          
        <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList></td>
               <td>y las</td>
               <td>  <asp:DropDownList ID="ddlhoraHastaJ" runat="server" Width="55px">  
           <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminHastaJ" runat="server" Width="55px">
          <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList>&nbsp;Hs.</td>
               </tr>
               <tr>
               <td> <asp:CheckBox ID="chkViernes" runat="server" Text="Viernes entre las" /></td>
               <td><asp:DropDownList ID="ddlhoraDesdeV" runat="server" Width="55px">      
            <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminDesdeV" runat="server" Width="55px">   
        
         <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList></td>
               <td> y las</td>
               <td> <asp:DropDownList ID="ddlhoraHastaV" runat="server" Width="55px">
          <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminHastaV" runat="server" Width="55px">        
          <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList>&nbsp;Hs.</td>
               </tr>
                <tr>
               <td><asp:CheckBox ID="chkSabado" runat="server" Text="Sábado entre las" /></td>
               <td> <asp:DropDownList ID="ddlhoraDesdeS" runat="server" Width="55px">        
            <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminDesdeS" runat="server" Width="55px">
        <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList></td>
               <td>y las</td>
               <td>   <asp:DropDownList ID="ddlhoraHastaS" runat="server" Width="55px">
         <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminHastaS" runat="server" Width="55px">
         <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList>&nbsp;Hs.</td>
               </tr>
                <tr>
               <td> <asp:CheckBox ID="chkDomingo" runat="server" Text="Domingo entre las" /></td>
               <td> <asp:DropDownList ID="ddlhoraDesdeD" runat="server" Width="55px">
      
            <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminDesdeD" runat="server" Width="55px">
       
         <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList></td>
               <td>y las</td>
               <td>  <asp:DropDownList ID="ddlhoraHastaD" runat="server" Width="55px">
          <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem><asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem><asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem><asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem></asp:DropDownList><asp:DropDownList ID="ddlminHastaD" runat="server" Width="55px">
         <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem><asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem><asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem><asp:ListItem>55</asp:ListItem></asp:DropDownList>&nbsp;Hs.</td>
               </tr>
               </table>
         </div></td></tr>
         </table>
</div>
</div> 
   <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">
         <tr><td colspan="4" style="text-align:right"> <asp:Button ID="Button2" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" CssClass="button2" />
     &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2"/></td></tr>
 </table>
</div> 
  <script type="text/javascript">

      $(function () {

          $("#tabs").tabs();
      });</script>
</asp:Content>