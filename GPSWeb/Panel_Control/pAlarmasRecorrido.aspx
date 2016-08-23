<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmasRecorrido.aspx.vb" Inherits="GPSWeb.pAlarmasRecorrido" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>  
<link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
    
 <script src="../scripts/ui/jquery.ui.tabs.min.js" type="text/javascript"></script>
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
var path;   
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
     /*   if(document.getElementById("<%= hdnrec_id.ClientID %>").value != "0")
                getRuta();*/

        path = new google.maps.MVCArray; 
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

        map.setZoom(14);

    }

    //ejecuto consulta contra la componente de negocios para traer los datos de los recorridos
    function searchRecorrido(map, rec_id, path) {
        markersArray = [];
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
                    //  map.setCenter(new google.maps.LatLng(puntos[0].lat, puntos[0].lng));

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        }


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
           // alert(path.length);      
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

 function getRuta() {
     directionsDisplay = new google.maps.DirectionsRenderer();
     directionsService = new google.maps.DirectionsService();

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


                    if (document.getElementById('<%= txtDireccionOrigen.ClientID %>').value == 'Av/Bv Calle Nro, Localidad, Provincia' || document.getElementById('<%= txtDireccionOrigen.ClientID %>').value == '') {
                        document.getElementById('<%= txtDireccionOrigen.ClientID %>').value = direccion;
                        document.getElementById('<%= hdnDir1.ClientID %>').value = direccion;
                    }
                    else {
                        jQuery('#<%= txtDireccionDestino.ClientID %>').val(direccion);
                        document.getElementById('<%= hdnDir2.ClientID %>').value = direccion;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

        }


       /* function seleccionarTodos() {
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
        }

        function verifyCheckboxList(source, arguments) {
            var chkListaTipoModificaciones = document.getElementById('');
            var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
            for (var i = 0; i < chkLista.length; i++) {
                if (chkLista[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            arguments.IsValid = false;
        }*/

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
        $(document).ready(function () {
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

    function setMarketPositions() {

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
   
   
 <div  id="draggable" style="border:2px solid #373435; float: left; width: 45%; height: auto; z-index:99; position:absolute; top:90px; background-color:White; vertical-align:middle; left:710px; border-radius: 8px 8px 8px 8px;">

  
       <div id="tabs" style="width:98%">
           <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <br />
                            <ul>
                            <li><a href="#tabs-1">Principal</a></li>  
                                <li><a href="#tabs-2">Periodo</a></li>
                             <li><a href="#tabs-3">Días de la Semana</a></li>
                              <li><a href="#tabs-4">Puntos Obligatorios</a></li>
                            </ul>

    <div id="tabs-1" style="width:100%;">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">  
    <ContentTemplate>
  <asp:HiddenField ID="hdncli_id" runat="server" />
    <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
      <asp:HiddenField ID="hdnrec_id" runat="server" Value="0" />
       <asp:HiddenField ID="hdnarec_id" runat="server" Value="0" />
       <asp:HiddenField ID="hdnRuta" runat="server" Value=""/>
        <asp:HiddenField ID="hdnDir1" runat="server" Value=""/>
         <asp:HiddenField ID="hdnDir2" runat="server" Value=""/>
        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Seleccione él o los móviles a los que quiera asignales la alarma y defina el recorrido a controlar." Font-Size="13px" ></asp:Label><br />
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="12px"></asp:Label>
         <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial" cellspacing="8" cellpaging="5">
     <tr><td colspan="2"><asp:Label ID="lblMovil" runat="server" Text="Moviles:"></asp:Label>
     </td>    

    </tr>
     <tr><td colspan="2">
     <asp:Panel ID="PanelGrupo" runat="server" Visible ="true">
           
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
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="5" CellPadding="3"  Font-Names="Arial" Font-Bold="false" Width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="chkMoviles" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
    <div>
     <br />
      <asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton>
     </div>
       
      </asp:Panel>
      
    </td></tr>
     <tr><td colspan="2"> <asp:Label ID="Label2" runat="server" Text="Disparar Alarma cuando ocurra un:" Font-Bold="true"></asp:Label></td></tr>
    <tr>
    <td colspan="2">  
    <asp:RadioButtonList ID="rdnTipoAlarma" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Value="0" Selected="True">Desvío de Recorrido</asp:ListItem>
            <asp:ListItem Value="1">Recorrido no Deseado</asp:ListItem>
        </asp:RadioButtonList>
     </td></tr>
       <tr>
             <td colspan="2">
                 <asp:Label ID="Label14" runat="server" Font-Bold="true" 
                     Text="Para usar un Recorrido ya creado elijalo del siguiente listado:"></asp:Label>
             </td>
            </tr>
     <tr>
    <td colspan="2">   <asp:DropDownList ID="ddlRecorrido" runat="server" DataValueField="rec_id" DataTextField="rec_nombre" AutoPostBack="true">
             </asp:DropDownList>  </td></tr>
   
    <tr><td colspan="2"> <asp:Label ID="Label8" runat="server" Text="Para dar de alta un nuevo Recorrido ingrese el Nombre Identificador y las direcciones:" Font-Bold="true"></asp:Label></td></tr>
         
    <tr><td colspan="2">Nombre Identificador: <asp:TextBox ID="txtNombre" runat="server" Width="253px" MaxLength="50"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Nombre Identificador"  Text="*" ControlToValidate="txtNombre"></asp:RequiredFieldValidator></td></tr>
    
    <tr><td colspan="2">   Dirección Origen: <asp:TextBox ID="txtDireccionOrigen" runat="server" Width="263px"></asp:TextBox>
            <ajaxtoolkit:textboxwatermarkextender id="TBWE4" runat="server" targetcontrolid="txtDireccionOrigen" watermarktext='Av/Bv Calle Nro, Localidad, Provincia' /></td></tr>
    <tr><td  colspan="2"> Dirección Destino: <asp:TextBox ID="txtDireccionDestino" runat="server" Width="255px"></asp:TextBox>
            <ajaxtoolkit:textboxwatermarkextender id="TBWE5" runat="server" targetcontrolid="txtDireccionDestino" watermarktext='Av/Bv Calle Nro, Localidad, Provincia' /></td></tr>
<tr><td colspan="2" style="text-align:right;"><input type="button" value="Verificar" onclick="getRuta();" class="button2" /></td>
</tr>
<tr><td colspan="2">  <asp:Label ID="Label4" runat="server" Font-Bold="false" Text="También puede Seleccionar en el Mapa la Ruta que desea configurar para esta Alerta."></asp:Label></td>
            </tr>    
    <tr><td><asp:Label ID="Label17" runat="server" Text="Notificar Alarma por e-mail:" Font-Bold="true"></asp:Label></td>
    <td>   <asp:CheckBox ID="chkMail" runat="server" Text="Si" Checked="true" /></td>
    </tr>
    <tr><td> <asp:Label ID="Label18" runat="server" Text="Umbral de Desvio (Kms):" Font-Bold="true"></asp:Label></td>
    <td>  <asp:TextBox ID="txtKms" runat="server" Width="96px" Text="0.1"></asp:TextBox> <asp:Label ID="Label16" runat="server" Text="Valor mínimo recomendado 0,1 kms." Font-Bold="true"></asp:Label></td>
    </tr>
  <tr><td> <asp:Label ID="Label23" runat="server" Text="Alarma Activa:" Font-Bold="true"></asp:Label></td>
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
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Periodo Fecha Inicio" ControlToValidate="txtFechaDesde" Text="*"></asp:RequiredFieldValidator></td></tr>
    
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
                 TargetControlID="txtFechaHasta" PopupButtonID="txtFechaHasta" PopupPosition="Left"/>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" runat="server" ErrorMessage="Periodo Fecha Fin" ControlToValidate="txtFechaHasta" Text="*"></asp:RequiredFieldValidator></td></tr>
    
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
  <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial" cellspacing="8" cellpaging="5">
  <tr><td colspan="2"> <a href="#" style="color:#D85639; font-size:12px;">Configurar Horarios de Rutina </a></td></tr>

<tr><td colspan="2">

  
           <asp:Label ID="Label3" runat="server" Font-Bold="false" Text="Establezca los Dias y Horarios en que se va a controlar el Desvío de este Recorrido."></asp:Label>
        </td></tr>
    <tr><td> <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="5" cellpaging="5">
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
               </table></td></tr>
              
 </table>

    </div>
    <div id="tabs-4" style="width:100%;">
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div style="width:60%; float:right;">
                <img src="../images/FhHRx.gif" alt="procesando" />
                 <span style="font-size:14px;color:Red; ">Pocesando, aguarde...</span></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
     <asp:UpdatePanel ID="UpdatePanel2" runat="server">  
    <ContentTemplate>
 <table style="width:98%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial" cellspacing="8" cellpaging="5">             
            <tr><td colspan="2"> 
               <asp:Label ID="Label15" runat="server" Font-Bold="true" Text="Configurar Puntos Obligatorios de Visitar." Font-Size="13px" ></asp:Label>
           </td></tr>
             <tr><td colspan="2"> <asp:LinkButton ID="linkPuntos"  Font-Names="Arial" ForeColor="#D85639" Font-Size="12px" runat="server" CausesValidation="false">Ver/Actualizar Puntos</asp:LinkButton><br />
<asp:Label ID="Label6" runat="server" Text="Al actualizar perdera los datos grabados de Fechas y Horas." Font-Bold="true" Font-Size="10px"></asp:Label>
              <asp:Label ID="lblErrorPuntos" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="12px"></asp:Label>
             </td></tr>
    <tr><td colspan="2">
     <asp:Panel ID="PanelPuntos" runat="server" Visible="false">
           <asp:Label ID="Label5" runat="server" Font-Bold="false" Text="Seleccione los Puntos del Recorrido que son obligatorios que se visiten cierto día y horario."></asp:Label><br /><br />
           <asp:DataList  ID="DataListPuntos" runat="server" Width="100%" Font-Bold="false" Font-Names="Arial" DataKeyField="idPunto" OnItemDataBound="DataListPuntos_ItemDataBound">
           <ItemTemplate>
           <asp:Label ID="lblLat" runat="server" Text='<%# Eval("lat")%>' Visible="false"></asp:Label>
            <asp:Label ID="lblLng" runat="server" Text='<%# Eval("lng")%>' Visible="false"></asp:Label>
             <asp:Label ID="lblMarcado" runat="server" Text='<%# Eval("Marcar")%>' Visible="false"></asp:Label>
               <asp:CheckBox ID="chkDireccion" runat="server" />
                 <asp:Label ID="Label13" runat="server" Text='Punto: ' Font-Bold="true"></asp:Label>
               <asp:Label ID="lblDireccion" runat="server" Text='<%# Eval("direccion")%>'></asp:Label><br />
                <asp:Label ID="Label7" runat="server" Text="Fecha:"></asp:Label>
               <asp:TextBox ID="txtFecha" runat="server" Width="70px" Text='<%# Eval("Fecha")%>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="!"
                                                        ControlToValidate="txtFecha" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
             <ajaxtoolkit:calendarextender ID="CalendarExtender2" runat="server" CssClass="black" 
                 TargetControlID="txtFecha" PopupButtonID="txtFecha"/>
                <asp:Label ID="Label9" runat="server" Text="Entre las:"></asp:Label>
              <asp:DropDownList ID="ddlHoraDesde" runat="server" Width="55px">
        <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem>
         <asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem>
         <asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem>
         <asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem>
         <asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem>
         <asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem>
         <asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem>
         <asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem><asp:ListItem>23</asp:ListItem>
         </asp:DropDownList>
                
                 <asp:DropDownList ID="ddlMinDesde" runat="server" Width="55px">
       <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
        <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
        <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem>
        <asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem>
        <asp:ListItem>55</asp:ListItem> <asp:ListItem>59</asp:ListItem></asp:DropDownList>
        <asp:Label ID="Label11" runat="server" Text=" y las "></asp:Label>
           <asp:DropDownList ID="ddlHoraHasta" runat="server" Width="55px">
             <asp:ListItem>23</asp:ListItem>
        <asp:ListItem>00</asp:ListItem><asp:ListItem>01</asp:ListItem>
         <asp:ListItem>02</asp:ListItem><asp:ListItem>03</asp:ListItem><asp:ListItem>04</asp:ListItem>
         <asp:ListItem>05</asp:ListItem><asp:ListItem>06</asp:ListItem><asp:ListItem>07</asp:ListItem>
         <asp:ListItem>08</asp:ListItem><asp:ListItem>09</asp:ListItem><asp:ListItem>10</asp:ListItem>
         <asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem>
         <asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem>
         <asp:ListItem>17</asp:ListItem><asp:ListItem>18</asp:ListItem><asp:ListItem>19</asp:ListItem>
         <asp:ListItem>20</asp:ListItem><asp:ListItem>21</asp:ListItem><asp:ListItem>22</asp:ListItem>
          
         </asp:DropDownList>
               
                 <asp:DropDownList ID="ddlMinHasta" runat="server" Width="55px"><asp:ListItem>59</asp:ListItem>
       <asp:ListItem>00</asp:ListItem><asp:ListItem>05</asp:ListItem>
        <asp:ListItem>10</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>20</asp:ListItem>
        <asp:ListItem>25</asp:ListItem><asp:ListItem>30</asp:ListItem><asp:ListItem>35</asp:ListItem>
        <asp:ListItem>40</asp:ListItem><asp:ListItem>45</asp:ListItem><asp:ListItem>50</asp:ListItem>
        <asp:ListItem>55</asp:ListItem> </asp:DropDownList>
               <hr />
           </ItemTemplate>
           </asp:DataList>
            
         </asp:Panel>
    </td></tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
    </div>
  

         </div>
<table>
<tr><td colspan="2" style="text-align:right;">  <asp:Button ID="Button2" runat="server" CssClass="button2" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" />
         &nbsp;
         <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2" OnClientClick="setMarketPositions();"/></td></tr>
          </table>
        
 </div>
 <div style="margin-left:50px; width:90%;height:auto;">
 
   
 <h4>Configurar Alarma de Desvío de Recorridos</h4>

 </div>
   <div style="width: 98%; height:auto;">
        <div style="margin-left:50px; width:90%; vertical-align:middle;">
        <br />
           <a id="Limpiar" href="#" onclick="javascript:limpiarRuta();" style="color:#cc0000; font-size:12px; font-weight:bold;">Limpiar Ruta - </a>
      <a id="noMarcar" href="#" onclick="javascript:dejardeMarcar();" style="color:#cc0000; font-size:12px; font-weight:bold;">No Marcar Puntos</a>
    <a id="Marcar" href="#" onclick="javascript:volveraMarcar();" style="color:#cc0000; font-size:12px; font-weight:bold;display:none;">Marcar Puntos</a>
            </div> 
          <div id="map_canvas" style="width:100%; height: 750px;">
            </div> 
         
   </div> 

  <script type="text/javascript">

      $(function () {
      
          $("#tabs").tabs();
      });

    
 </script>
     
</asp:Content>