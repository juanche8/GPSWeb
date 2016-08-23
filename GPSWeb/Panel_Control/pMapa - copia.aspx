<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pMapa.aspx.vb" Inherits="GPSWeb.pMapa" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script src="../scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="http://maps.google.com/maps?file=api&amp;v=3&amp;sensor=true&amp;key=ABQIAAAADuah2gDIAy3jjIIzy1lyQBTraYcv9GXdi4eo_7y-vk4zC9IZ_hTwiM4hGL8CH-fK3ncjDKOhYuQPrw" type="text/javascript"></script>
   
    <script type="text/javascript">

        
        
        function createMarker(point, htmlVentana, icono) {
            var Icon = new GIcon(baseIcon);
            Icon.image = '../images/' + icono;  //"../images/camion_rojo.png";
            markerOptions = { icon: Icon };
            var marker = new GMarker(point, markerOptions);
            GEvent.addListener(marker, "click", function() {
                marker.openInfoWindowHtml(htmlVentana);
            });
            return marker;
        }


        function getMarkersWS(map, bounds) {
            alert('get');
            $.ajax({
                type: "POST",
                cache: false,
                url: "wsDatos.asmx/GetVehiculos",
                data: "{'cli_id': '" + document.getElementById("<%= hdncli_id.ClientID %>").value + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(response) {
                    var vehiculos = response.d;
                    $.each(vehiculos, function(index, movil) {
                        var point = new GLatLng(movil.lng, movil.lat);
                        var htmlVentana = "<img src='../fotos/" + movil.foto + "'><br> Identificador:" + movil.id + "</b><br>Nombre:" + movil.nombre + "</b><br>Patente:" + movil.patente + "<br>Ubicación:" + movil.ubicacion + "<br>Hora:" + movil.hora;
                        var marker = createMarker(point, htmlVentana, movil.icono);
                        bounds.extend(point);
                        map.addOverlay(marker);
                        $("<li />").html(movil.nombre + "  " + movil.id + " <b>" + movil.patente + "</b>").click(function() {
                            marker.openInfoWindowHtml(htmlVentana);
                        }).appendTo("#list");
                    });

                    map.setCenter(new GLatLng(vehiculos[0].lng, vehiculos[0].lat));
                },

                failure: function(msg) {
                    alert(msg);
                }

            }); //fin llamada ajax


        } //fin funcion getMarkersWS

       // setTimeout("mueveReloj()", 1000)
        $(document).ready(function() {

            if (GBrowserIsCompatible()) {
                var map = new google.maps.Map(document.getElementById('map_canvas'));
                map.setCenter(new GLatLng(20, 30), 13);
                map.setUIToDefault();
                var bounds = new GLatLngBounds();
                /* Creamos un objeto vacio GLatLngBounds() */
                var baseIcon = new GIcon(G_DEFAULT_ICON);
                //  baseIcon.shadow = "../images/flecha-verde.png";
                baseIcon.iconSize = new GSize(30, 35);
                //  baseIcon.shadowSize = new GSize(37, 34);
                baseIcon.iconAnchor = new GPoint(9, 34);
                baseIcon.infoWindowAnchor = new GPoint(9, 9);
                alert('llamar get');
                getMarkersWS(map, bounds);

                /* Cuando hayamos incluido todos los puntos seteamos el centro y el zoom usando el objeto 'bounds' */
                // map.setZoom(map.getBoundsZoomLevel(bounds));

                //  map.setCenter(bounds.getCenter());

            }



            function AjaxError(result) {
                alert("ERROR " + result.status + ' ' + result.statusText);
            }

        });               //fin de Ready


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <div class="inline" style="border: thin solid #00A6C6; float: left; width: 25%; height: 700px">
        <asp:Label ID="Label1" runat="server" Text="Mis Vehiculos:" Font-Bold="true"></asp:Label><br />
 <!--litado vehiculos-->
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id">
     <ItemTemplate>
         <asp:CheckBox ID="CheckBox1" runat="server" Text='<%# Eval("veh_descripcion")%>' />
         <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/report_ico.png" ToolTip="Historico" />
         <asp:ImageButton ID="btnAlerts" runat="server" ImageUrl="~/images/icoWarning.png" ToolTip="Alarmas"  />
         <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/recorrido.jpg" ToolTip="Recorrido" />
         <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/i_map.png" ToolTip="Seguimiento"/>
     </ItemTemplate>
     </asp:DataList>
     <br /><br />
</div>
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 74%">
   <div id="map_canvas" style="width: 720px; height: 690px">

    </div>

    <ul id="list">

    </ul>
 </div>
</asp:Content>