<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmasZonas.aspx.vb" Inherits="GPSWeb.pAlarmasZonas" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>  
<link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
    
 <script src="../scripts/ui/jquery.ui.tabs.min.js" type="text/javascript"></script>
<script type="text/javascript">

    var poly, map;
    var markers = [];
    var path = new google.maps.MVCArray;   

    //definir mapa con sus opciones
    function initialize() {
       
     var myLatlng = new google.maps.LatLng(-34.604, -58.382);
        var myOptions = {
            zoom: 15,
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

        google.maps.event.addListener(marker, 'dragend', function(e) {
            for (var i = 0, I = markers.length; i < I && markers[i] != marker; ++i);
            path.setAt(i, marker.getPosition());
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
     markers = [];
     document.getElementById("<%= hdnZona.ClientID %>").value = '';


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

        markers = [];
        
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
                        });

                        google.maps.event.addListener(marker, 'dragend', function () {
                            for (var i = 0, I = markers.length; i < I && markers[i] != marker; ++i);
                            path.setAt(i, marker.getPosition());
                        } );

                    });
                    map.setCenter(new google.maps.LatLng(ruta[0].lat, ruta[0].lng));
                    map.setZoom(15);
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
                map.setZoom(15);
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

    

   /*  function seleccionarTodos() {
         var chkListaTipoModificaciones = document.getElementById('');
         var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
         for (var i = 0; i < chkLista.length; i++) {
            
                chkLista[i].checked = true;
         }
     }


     function DeseleccionarTodos() {
         var chkListaTipoModificaciones = document.getElementById('');
         var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
         for (var i = 0; i < chkLista.length; i++) {
                 chkLista[i].checked = false;
            
         }
     }*/
    

    

     function mostrar() {

         if (document.getElementById("rutina").style.display == "inline-block") {
             document.getElementById("rutina").style.display = "none";
         }
         else {
             document.getElementById("rutina").style.display = "inline-block"
         }
     }

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

    <div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Configurar Alarma de Entrada/Salida de Zonas</h3>
  
  <br />
   <asp:Label ID="Label18" runat="server" Text="Seleccione en el Mapa un mínimo de 3 puntos para delimitar la zona que desea controlar (se le informara el ingreso y la salida de la zona)." Font-Size="14px" ForeColor="#D85639"></asp:Label>
        
 </div>
     <br />
            <div style="margin-left:5px; width:100%; vertical-align:middle;">
     <asp:Label ID="Label9" runat="server" Text="Buscar Direccion:" Font-Bold="true" Font-Size="12px"></asp:Label>
         &nbsp;&nbsp;
        <asp:TextBox ID="txtDireccion" runat="server" Width="590px"></asp:TextBox>
      &nbsp;<a href="#" onclick="marcarDireccion();" title="Buscar Dirección"><img src="../images/panel_buscar.png" alt="Buscar Direccion"  /></a>
       
     <a id="Limpiar" href="#" onclick="javascript:limpiarZona();" style="color:#cc0000; font-size:12px; font-weight:bold;">Limpiar Zona - </a>
      <a id="noMarcar" href="#" onclick="javascript:dejardeMarcar();" style="color:#cc0000; font-size:12px; font-weight:bold;">No Marcar Puntos</a>
    <a id="Marcar" href="#" onclick="javascript:volveraMarcar();" style="color:#cc0000; font-size:12px; font-weight:bold; display:none;">Marcar Puntos</a><br />
&nbsp;<br />
         <div id="map_canvas" style="width: 100%; height:800px;">
            </div>
      </div>
  <div  id="draggable" style="border:2px solid #373435; float: left; width: 40%; height: auto; z-index:99; position:absolute; top:140px; background-color:White; vertical-align:middle; left:750px; border-radius: 8px 8px 8px 8px;">
 
   

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
      <asp:HiddenField ID="hdnzon_id" runat="server" Value="0" />
        <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
       <asp:HiddenField ID="hdnZona" runat="server" />
    <asp:HiddenField ID="hdnazon_id" runat="server" Value="0" />
      <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Seleccione él o los móviles a los que quiera asignales la alarma y defina la Zona a controlar." Font-Size="12px" ></asp:Label><br />
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"  Font-Size="14px"></asp:Label>
         <table style="width:98%; vertical-align:middle; font-size:12px; font-family:Arial; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">
     <tr><td colspan="2"><asp:Label ID="lblMovil" runat="server" Text="Moviles:"></asp:Label>
     </td>    

    </tr>
     <tr><td colspan="2">
     <asp:Panel ID="PanelGrupo" runat="server" Visible ="true">
       <asp:Label ID="Label16" runat="server" Text="Filtrar Por Grupo:"></asp:Label>   
         &nbsp;<asp:DropDownList ID="ddlgrupo" runat="server" AutoPostBack="true" DataTextField="grup_nombre" DataValueField="grup_id">
         </asp:DropDownList>
         &nbsp;&nbsp; 
         <asp:CheckBox ID="chkTodos" runat="server" Text="Ver todos los Moviles" Font-Bold="true" AutoPostBack="true" />
         <br />
     </asp:Panel>
    </td></tr>
    <tr><td colspan="2">
     <asp:Panel ID="PanelMoviles" runat="server">
     <div style="height:200px;overflow-y: scroll; font-family:Arial;width:320px; border-color:LightGray; border-width:1px; border-style:solid;"">       
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="5" CellPadding="5"  Font-Names="Arial" Font-Bold="false" width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="chkMoviles" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
         
      </asp:Panel>
       <asp:Panel ID="PanelTildar" runat="server">
     <br />
     <asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton>
      </asp:Panel>   
    </td></tr>
   
    <tr>
    <td colspan="2"><asp:Label ID="Label14" runat="server" Text="Para usar una Zona ya creada seleccionela del siguiente listado:" Font-Bold="true"></asp:Label></td></tr>
    <tr>
    <td colspan="2">  <asp:DropDownList ID="ddlZona" runat="server" DataValueField="zon_id" DataTextField="zon_nombre" AutoPostBack="true">
             </asp:DropDownList>            </td></tr>
    <tr><td colspan="2"> <asp:Label ID="Label2" runat="server" Text="Para dar de alta una nueva zona ingrese el Nombre Identificador para la Zona:" Font-Bold="true"></asp:Label></td></tr>
    <tr><td colspan="2"> <asp:TextBox ID="txtNombre" runat="server" Width="287px" MaxLength="50"></asp:TextBox></td></tr>
    
    <tr><td><asp:Label ID="Label5" runat="server" Text="Notificar Alarma por e-mail:" Font-Bold="true"></asp:Label></td>
    <td> <asp:CheckBox ID="chkMail" runat="server" Text="Si" Checked="true" /></td>
    </tr>
    <tr><td> <asp:Label ID="Label13" runat="server" Text="Umbral de Desvio (Kms):" Font-Bold="true"></asp:Label></td>
    <td> <asp:TextBox ID="txtKms" runat="server" Width="96px" Text=""></asp:TextBox></td>
    </tr>
 <tr><td> <asp:Label ID="Label15" runat="server" Text="Alarma Activa:" Font-Bold="true"></asp:Label></td>
     <td> <asp:RadioButtonList ID="rdnActiva" runat="server" RepeatDirection="Horizontal">
                 <asp:ListItem Value="True" Selected="True">Si</asp:ListItem>
                 <asp:ListItem Value="False" >No</asp:ListItem>
             </asp:RadioButtonList></td></tr>
    </table>
     </ContentTemplate>
  </asp:UpdatePanel>      
    </div>
  <div id="tabs-2" style="width:100%;">
   <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial" cellspacing="8" cellpaging="5">
    <tr><td colspan="2"> <asp:Label ID="Label19" runat="server" Text="Frecuencia de Monitoreo:" Font-Bold="true"></asp:Label></td></tr>
    <tr><td colspan="2"> <asp:Label ID="Label20" runat="server" Text="Verificar los Desvíos para el siguiente rango de días y horario."></asp:Label></td></tr>
     <tr><td>  <asp:Label ID="Label21" runat="server" Text="Inica el Día:"></asp:Label></td>
     <td><asp:TextBox ID="txtFechaDesde" runat="server" Width="96px"></asp:TextBox>
              <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="!"
                                                        ControlToValidate="txtFechaDesde" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
             <ajaxtoolkit:calendarextender ID="CalendarExtender2" runat="server" CssClass="black" 
                 TargetControlID="txtFechaDesde" PopupButtonID="txtFechaDesde"/>
             </td>
    </tr>
    <tr>
    <td>
             <asp:Label ID="Label22" runat="server" Text="Finaliza el Día:"></asp:Label></td>
        <td><asp:TextBox ID="txtFechaHasta" runat="server" Width="99px"></asp:TextBox>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="!"
                                                        ControlToValidate="txtFechaHasta" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
              <ajaxtoolkit:calendarextender ID="CalendarExtender4" runat="server"  CssClass="black"
                 TargetControlID="txtFechaHasta" PopupButtonID="txtFechaHasta" PopupPosition="Left"/></td>
     </tr>
      <tr><td>Entre las</td>
     <td>  <asp:DropDownList ID="ddlHoraDesde" runat="server" Width="55px">
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
         <asp:DropDownList ID="ddlMinDesde" runat="server" Width="55px">
       <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
        <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
        <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem>
        <asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem>
        <asp:ListItem>55</asp:ListItem> <asp:ListItem>59</asp:ListItem></asp:DropDownList>
          &nbsp; y las&nbsp;
      <asp:DropDownList ID="ddlHorahasta" runat="server" Width="55px">
         <asp:ListItem>23</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem>
         <asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem>
         <asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem>
         <asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem>
         <asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem>
         <asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem>
         <asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem>
         <asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem>
         </asp:DropDownList>
         <asp:DropDownList ID="ddlMinHasta" runat="server" Width="55px">
        <asp:ListItem>59</asp:ListItem><asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
        <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
        <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem>
        <asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem>
        <asp:ListItem>55</asp:ListItem></asp:DropDownList>
        &nbsp;Hs.
        </td>        
     </tr>
   </table>
  </div>
    <div id="tabs-3" style="width:100%;">
  <table style="width:98%; vertical-align:middle; font-size:12px; font-family:Arial; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">

<tr><td colspan="2">
 
           
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
        
</td></tr>
 </table>
    </div>
  
  </div>
  <table style="width:98%; vertical-align:middle; font-size:12px; font-family:Arial; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">
<tr><td colspan="2" style="text-align:right;">  <asp:Button ID="Button2" runat="server" CssClass="button2" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" />
         &nbsp;
         <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2" OnClientClick="setMarketPositions();"/></td></tr>
          </table>
 
   

               
 </div>
 
  </div>
   <script type="text/javascript">

       $(function () {

           $("#tabs").tabs();
       });</script>
</asp:Content>
