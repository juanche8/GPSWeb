<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pSeguimientos.aspx.vb" Inherits="GPSWeb.pSeguimientos" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>
    <script type="text/javascript">
        var infos = [];
        var map;
        var ruta = new Array();
        var moviles = new Array();
        var markersArray = [];
        var mostrarZoom = true;
        
        //busco la ubicacion actual del o los vehiculos seleccionados
        function mostrarUbicacion() {
            mostrarZoom = true;
            moviles = new Array();
            for (i = 0; i < 9; i++) {
                div = document.getElementById("map_canvas" + (i + 1));
                div.style.display = '';
            }            
           
            var myOptions = {
                center: new google.maps.LatLng(-35, -64),
                zoom: 9,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

          //  var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

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

            
            for (i = 0; i < moviles.length; i++) {

                //tengo que hacer un mapa por cada movil que haya elegido
                //los mpasa los pongo dentro de una tabla
                //y genero la div que contiene el mapa

                var map = new google.maps.Map(document.getElementById("map_canvas" + (i + 1)), myOptions);               
                searchubicacion(map, moviles[i]);
               
            }

            //tengo que ocultar las divs que no se van a mostrar y agrandar los mapas
            for (i = moviles.length; i < 9; i++) {
                div = document.getElementById("map_canvas"+(i+1));
                div.style.display = 'none';
                div.style.height = "300px";
                div.style.width = "400px";
            }

            if (moviles.length == 2) {
                for (i = 0; i < moviles.length; i++) {

                    div = document.getElementById("map_canvas" + (i + 1));
                    div.style.height = "600px";
                    div.style.width = "600px";
                }
            }
            
            if (moviles.length == 1) {
                for (i = 0; i < moviles.length; i++) {

                    div = document.getElementById("map_canvas" + (i + 1));
                    div.style.height = "600px";
                    div.style.width = "650px";
                }
            }

            if (moviles.length >= 3) {
                for (i = 0; i < moviles.length; i++) {

                    div = document.getElementById("map_canvas" + (i + 1));
                    div.style.height = "300px";
                    div.style.width = "400px";
                }
            }
            setTimeout("mostrarUbicacion()", 10000);
        }

        

        //ejecuto consulta contra la componente de negocios para traer los datos de las ubicaciones actuales de los vehiculos de la empresa logeada
        function searchubicacion(map, veh_id) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/GetUbicacion",
                data: "{'veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var vehiculos = response.d;
                    $.each(vehiculos, function(index, movil) {

                        var htmlVentana = "Identificador:" + movil.id + "</b><br>Nombre:" + movil.nombre + "</b><br>Patente:" + movil.patente + "<br>Ubicación:" + movil.ubicacion + "<br>Velocidad:" + movil.velocidad + "<br>Hora:" + movil.hora;
                        var marker = createMarker(map, movil.lng, movil.lat, htmlVentana, movil.icono);

                    });
                    if (vehiculos.length > 0) {
                        map.setCenter(new google.maps.LatLng(vehiculos[0].lat, vehiculos[0].lng));
                        map.setZoom(16);
                           
                    }

                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });


        }
        
       function initialize() {
        var myOptions = {
        center: new google.maps.LatLng(-35, -64),
            zoom: 9,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

        }

        //busco todos los moviles de la empresa para consultar
        function getMoviles() {
            var cli_id = document.getElementById("<%= hdncli_id.ClientID %>").value
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/GetVehiculosActivosTop",
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
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        }

        $(document).ready(function() {
           // if (document.getElementById("<%= hdnveh_id.ClientID %>").value != '')
         mostrarUbicacion();

     });

     //asocio el enter al evento del boton Ver Recorridos
     //asociar enter al boton IR que busca las direcciones
     $(document).keypress(function(e) {
         if (e.keyCode == 13) {

             mostrarUbicacion();
         }
         return (e.keyCode != 13); //evito que el Enter ejecute el submit del formulario
     });  

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
  
    <ContentTemplate>
    <asp:HiddenField ID="hdnveh_id" runat="server" />
    <asp:HiddenField ID="hdnFechaDesde" runat="server" />
      <asp:HiddenField ID="hdnFechaHasta" runat="server" />   
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <div style="border: thin solid #00A6C6">
      <asp:Label ID="Label9" runat="server" Text="Consulta de Ubicación Actual de los Vehiculos." Font-Size="14px" Font-Bold="true"></asp:Label><br />
      <asp:Label ID="lblMovil" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue"></asp:Label>
        <br />
        <br />
      <asp:Panel ID="PanelMoviles" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Seleccione uno o más Vehiculos para hacer el seguimiento (Máximo 9)" Font-Bold="true"></asp:Label><br />
         <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label><br />
 <!--litado vehiculos-->
        <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" RepeatColumns="5" RepeatDirection="Horizontal">
             <ItemTemplate>
                 <input id="chkVehiculo" name="chkVehiculo" type="checkbox" value='<%# Eval("veh_id")%>' />
                  <asp:Label ID="Label2" runat="server" Text='<%# Eval("veh_descripcion")%>' ></asp:Label>-
                  <asp:Label ID="Label3" runat="server" Text='<%# Eval("veh_patente")%>' ></asp:Label>
                 
             </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
        
        <br />
        <input id="Button2" type="button" value="Ver Seguimiento" onclick="mostrarUbicacion();"/>
       
 </div>
        
</ContentTemplate>
</asp:UpdatePanel>
 <div style="border: thin solid #00A6C6; height:auto;" id="mapas">
<table style="width:100%" cellspacing="0" cellpadding="0">
<tr>
<td><div id="map_canvas1" style="border: thin solid #CCCCCC;width:400px;height:300px;"></div></td>
  <td><div id="map_canvas2" style="border: thin solid #CCCCCC; width:400px;height:300px;" ></div></td>
    <td><div id="map_canvas3" style="border: thin solid #CCCCCC; width:390px;height:300px;" ></div></td>
</tr>
<tr>
<td><div id="map_canvas4" style="border: thin solid #CCCCCC;width:400px;height:300px;"></div></td>
  <td><div id="map_canvas5" style="border: thin solid #CCCCCC; width:400px;height:300px;" ></div></td>
    <td><div id="map_canvas6" style="border: thin solid #CCCCCC; width:390px;height:300px;" ></div></td>
</tr>
<tr>
<td><div id="map_canvas7" style="border: thin solid #CCCCCC;width:400px;height:300px;"></div></td>
  <td><div id="map_canvas8" style="border: thin solid #CCCCCC; width:400px;height:300px;" ></div></td>
    <td><div id="map_canvas9" style="border: thin solid #CCCCCC; width:390px;height:300px;" ></div></td>
</tr>
</table>

 </div>
</asp:Content>
