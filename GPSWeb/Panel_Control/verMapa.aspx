<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="verMapa.aspx.vb" Inherits="GPSWeb.verMapa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
      <script src="../scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
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
                   title: 'Punto de Desvio',
                  draggable: false //que el marcador se pueda arrastrar
              });
              map.setCenter(latLng);

          }
          catch (err) {
          }
      }

      //zona

      function verZona() {

          if (document.getElementById("<%= hdnzon_id.ClientID %>").value != "0") {
              searchZona(map, document.getElementById("<%= hdnzon_id.ClientID %>").value, path)
              poly = new google.maps.Polygon({
                  strokeWeight: 3,
                  fillColor: '#5555FF'
              });
              poly.setMap(map);
              poly.setPaths(new google.maps.MVCArray([path]));

              map.setZoom(14);
          }
      }
      //ejecuto consulta contra la componente de negocios para traer los datos de los recorridos
      function searchZona(map, zon_id, path) {


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
</head>
<body>
    <form id="form1" runat="server">
       <asp:HiddenField ID="hdnzon_id" runat="server" Value="0" />
         <asp:HiddenField ID="hdnrec_id" runat="server" Value="0" />
     <asp:HiddenField ID="lat" runat="server" />
    <asp:HiddenField ID="lng" runat="server" />
       <asp:HiddenField ID="latDir" runat="server" Value="" />
    <asp:HiddenField ID="lngDir" runat="server" Value="" />
            <div id="map_canvas" style="width: 520px; height:400px">
                        </div>
                       <br />    
            <script type="text/javascript">                verMapa(); verZona(); verDireccion(); verRecorrido()</script>
    </form>
</body>
</html>
