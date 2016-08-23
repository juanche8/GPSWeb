<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminSeguimiento.aspx.vb" Inherits="GPSWeb.pAdminMapa" MasterPageFile="~/CMS/SitePages.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&sensor=false"> 
</script>
    <script type="text/javascript">
        var infos = [];
        var map;
        var ruta = new Array();
        var moviles = new Array();
        var markersArray = [];
        //busco la ubicacion actual del o los vehiculos seleccionados
        function mostrarUbicacion() {

            for (i = 0; i < 8; i++) {
                div = document.getElementById("map_canvas" + (i + 1));
                div.style.display = '';
            }
          
            var myOptions = {
                center: new google.maps.LatLng(-35, -64),
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

          //  var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

            //voy a buscar en al base de datos los puntos del recorrido del vehiculo seleccionado
            //tengo que recorrer los elements y ver cual selecciono para consultar por cada id de movil
            var checkboxes = document.getElementById("aspnetForm").chkVehiculo;

            moviles = new Array();
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
               
                var map = new google.maps.Map(document.getElementById("map_canvas"+ (i +1)), myOptions);
                searchubicacion(map, moviles[i]);                
            }

            //tengo que ocultar las divs que no se van a mostrar y agrandar los mapas
            for (i = moviles.length; i < 8; i++) {
                div = document.getElementById("map_canvas" + (i + 1));
                div.style.display = 'none';
                div.style.height = "275px";
                div.style.width = "435px";
            }

            if (moviles.length == 2) {
                for (i = 0; i < moviles.length; i++) {

                    div = document.getElementById("map_canvas" + (i + 1));
                    div.style.height = "500px";
                    div.style.width = "450px";
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
                    div.style.height = "275px";
                    div.style.width = "435px";
                }
            }
            setTimeout("mostrarUbicacion()", 10500);
        }

       

        //ejecuto consulta contra la componente de negocios para traer los datos de las ubicaciones actuales de los vehiculos de la empresa logeada
        function searchubicacion(map, veh_id) {

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
            zoom: 7,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        map = new google.maps.Map(document.getElementById("map_canvas1"), myOptions);
        div = document.getElementById("map_canvas1");
        div.style.height = "600px";
        div.style.width = "650px";

        }

        //busco todos los moviles de la empresa para consultar
        function getMoviles() {
            var cli_id = document.getElementById("<%= hdncli_id.ClientID %>").value
            $.ajax({
                async: false,
                type: 'POST',
                url: "../Panel_Control/wsDatos.asmx/GetVehiculos",
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
            initialize();

        });  

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <div>
  <asp:Label ID="Label2" runat="server" Text="SEGUIMIENTO MOVILES" Font-Bold="true" Font-Size="16px"></asp:Label>
  </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">  
    <ContentTemplate>
    <asp:HiddenField ID="hdnveh_id" runat="server" />
    <asp:HiddenField ID="hdnFechaDesde" runat="server" />
      <asp:HiddenField ID="hdnFechaHasta" runat="server" />   
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <div style="border: thin solid #00A6C6; float: left; width: 28%;" class="inline">
      <asp:Label ID="Label3" runat="server" Text="Seleccione un Cliente:" Font-Bold="true"></asp:Label><br />
        <asp:DropDownList ID="ddlCliente" runat="server" DataTextField="cli_nombre" DataValueField="cli_id" AutoPostBack="true">
        </asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Seleccione uno o más Vehiculos para hacer el seguimiento (Maximo 8)" Font-Bold="true"></asp:Label><br />
 <!--litado vehiculos-->
 <asp:Panel ID="panelMoviles" Visible="false" runat="server">
        <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id">
             <ItemTemplate>
                 <input id="chkVehiculo" name="chkVehiculo" type="checkbox" value='<%# Eval("veh_id")%>' />
                  <asp:Label ID="Label2" runat="server" Text='<%# Eval("veh_descripcion")%>' ></asp:Label>-
                   <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' ></asp:Label>
                 
             </ItemTemplate>
            </asp:DataList>
        <br />
        <input id="btnSeguimiento" type="button" value="Ver Seguimiento" onclick="mostrarUbicacion();"/></asp:Panel> 
 </div>
        
</ContentTemplate>
</asp:UpdatePanel>
 <div style="border: thin solid #00A6C6;float: left; width: 71%; height:100%;" id="mapas" class="inline">
<table style="width:100%;height:100%;" cellspacing="1" cellpadding="1">
<tr>
<td><div id="map_canvas1" style="width:435px;height:275px;"></div></td>
  <td><div id="map_canvas2" style="width:435px;height:275px;" ></div></td>   
</tr>
<tr>
<td><div id="map_canvas3" style="width:435px;height:275px;"></div></td>
  <td><div id="map_canvas4" style="width:435px;height:275px;" ></div></td>   
</tr>
<tr>
<td><div id="map_canvas5" style="width:435px;height:275px;"></div></td>
  <td><div id="map_canvas6" style="width:435px;height:275px;" ></div></td>  
</tr>
<tr>
<td><div id="map_canvas7" style="width:435px;height:300px;"></div></td>
  <td><div id="map_canvas8" style="width:435px;height:300px;" ></div></td>  
</tr>
</table>

 </div>
</asp:Content>
