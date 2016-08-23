<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaDetalle.aspx.vb" Inherits="GPSWeb.pAlarmaDetalle" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script> 
  <script type="text/javascript">

      var poly, map;
      var markers = [];
      var path = new google.maps.MVCArray;
      var polyline;
      var markersArray = [];

      function verMapa() {
          try {
              var myOptions = {
                  center: new google.maps.LatLng(-35, -64),
                  zoom: 14,
                  mapTypeId: google.maps.MapTypeId.ROADMAP
              };

               map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
              var latLng = new google.maps.LatLng(document.getElementById('<%= lat.ClientID %>').value, document.getElementById('<%= lng.ClientID %>').value);
               marker = new google.maps.Marker({
                  map: map, //el mapa creado en el paso anterior
                  position: latLng, //objeto con latitud y longitud
                  draggable: false //que el marcador se pueda arrastrar
              });
              map.setCenter(latLng);

          }
          catch (err) {
          }
      }

      //zona

      function verZona() {
          searchRecorrido(map, document.getElementById("<%= hdnzon_id.ClientID %>").value, path)
          poly = new google.maps.Polygon({
              strokeWeight: 3,
              fillColor: '#5555FF'
          });
          poly.setMap(map);
          poly.setPaths(new google.maps.MVCArray([path]));
      }
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
                  success: function (response) {
                      var ruta = response.d;
                      $.each(ruta, function (index, paso) {

                          path.push(new google.maps.LatLng(paso.lat, paso.lng));

                      });
                      map.setCenter(new google.maps.LatLng(ruta[0].lat, ruta[0].lng));
                      map.setZoom(11);
                  },
                  error: function (jqXHR, textStatus, errorThrown) {
                      alert(textStatus + ": " + XMLHttpRequest.responseText);
                  }
              });
          }


      }

      function verDireccion() {
       var image = new google.maps.MarkerImage(
	              '../images/google-map-point-azul.png',
		           new google.maps.Size(28, 45),
		            new google.maps.Point(0, 0),
		            new google.maps.Point(0, 45)
	            );

       var latLng = new google.maps.LatLng(document.getElementById("<%= latDir.ClientID %>").value, document.getElementById("<%= lngDir.ClientID %>").value);
          //creamos el marcador en el mapa
          marker = new google.maps.Marker({
              map: map, //el mapa creado en el paso anterior
              position: latLng, //objeto con latitud y longitud
              icon: image,
              title: 'Dirección Original',
              draggable: true //que el marcador se pueda arrastrar
          });
      }

      function verRecorrido() {

          if (document.getElementById("<%= hdnrec_id.ClientID %>").value != "0") {
              searchRecorrido(map, document.getElementById("<%= hdnrec_id.ClientID %>").value, path)

              polyline = new google.maps.Polyline({
                  path: path
        , map: map
        , strokeColor: '#ff0000'
        , strokeWeight: 3
        , strokeOpacity: 0.6
        , clickable: false
              });
          }

      }

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

                          //creamos el marcador en el mapa
                          /*  var markerRec = new google.maps.Marker({
                          map: map, //el mapa creado en el paso anterior
                          position: new google.maps.LatLng(paso.lat, paso.lng), //objeto con latitud y longitud
                          draggable: true //que el marcador se pueda arrastrar

                          });
                          markersArray.push(markerRec);
                          marker.setTitle("#" + path.length);*/


                      });
                      //  map.setCenter(new google.maps.LatLng(puntos[0].lat, puntos[0].lng));
                      map.setZoom(14);
                  },
                  error: function (jqXHR, textStatus, errorThrown) {
                      alert(textStatus + ": " + XMLHttpRequest.responseText);
                  }
              });
          }


      }
       </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
         <asp:HiddenField ID="hdnrec_id" runat="server" Value="0" />
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <asp:HiddenField ID="hdnzon_id" runat="server" Value="0" />
     <asp:HiddenField ID="latDir" runat="server" Value="" />
    <asp:HiddenField ID="lngDir" runat="server" Value="" />
     <asp:HiddenField ID="lat" runat="server" />
    <asp:HiddenField ID="lng" runat="server" />
 <div style="float: left; width: 80%; height: 700px">
 <div style="margin-left:30px;">
 <div style="margin-left:50px; width:90%;height:auto;">
 <h3> <asp:Label ID="lbltitulo" runat="server" Text="Detalle de Alarma Reportada"></asp:Label> 
 </h3>
 <hr style="border: 1px solid #F58634; height: 2px;"/>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
 </div>
   
   <div style="margin-left:50px; width:95%;z-index:0;">

 <table style="font-size:12px;" cellspacing="5" cellpadding="5">
    <tr>
    <td>
    <asp:Label ID="Label2" runat="server" Text="Alarma:" Font-Bold="true"></asp:Label><br />      
       <asp:Label ID="lblAlarma" runat="server" Text="" ></asp:Label>
    </td>
    <td><asp:Label ID="Label22" runat="server" Text="Fecha:" Font-Bold="true"></asp:Label><br />   
        <asp:Label ID="lblFecha" runat="server" Text="" ></asp:Label>
                              </td>
</tr>
<tr>
<td><asp:Label ID="Label25" runat="server" Text="Patente Movil:" Font-Bold="true"></asp:Label><br />   
                            <asp:Label ID="lblPatente" runat="server" Text="" ></asp:Label></td>
<td><asp:Label ID="Label3" runat="server" Text="Conductor del Movil:" Font-Bold="true"></asp:Label><br />  
     <asp:Label ID="lblConductor" runat="server" Text="" ></asp:Label></td>
</tr>
<tr>
<td colspan="2"><asp:Label ID="Label26" runat="server" Text="Ubicación :" Font-Bold="true"></asp:Label><br />     
                          <asp:Label ID="lblubicacion" runat="server" Text="" ></asp:Label>
                             </td>

</tr>
<tr>
<td><asp:Label ID="Label4" runat="server" Text="Velocidad Kms/h" Font-Bold="true"></asp:Label><br />      
        <asp:Label ID="lblVelocidad" runat="server" Text="" ></asp:Label></td>

</tr>
<tr>
<td colspan="2"> 

<div id="map_canvas" style="width: 480px; height:310px">
                        </div>
     </td>

</tr>
<tr style="text-align:right;"><td colspan="2">    &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Volver" CssClass="button2" PostBackUrl="~/Panel_Control/pAlarmas.aspx" /></td></tr>
</table>
     

 

 </div>
 </div>                  
 </div>
 <script type="text/javascript">     verMapa(); verZona();verDireccion();</script>
</asp:Content>