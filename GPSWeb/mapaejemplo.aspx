<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="mapaejemplo.aspx.vb" Inherits="GPSWeb.mapaejemplo" %>

<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
        <meta name="apple-mobile-web-app-capable" content="yes" />
        <title>OpenStreetMap with Google Maps v3 API</title>
        <style type="text/css">
            html, body, #map {
                height: 100%;
                margin: 0;
                padding: 0;
            }
        </style>
    </head>
    <body>
        <div id="map"></div>
 
        <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
        <script type="text/javascript">
            var element = document.getElementById("map");
 
            /*
            Build list of map types.
            You can also use var mapTypeIds = ["roadmap", "satellite", "hybrid", "terrain", "OSM"]
            but static lists sucks when google updates the default list of map types.
            */
            var mapTypeIds = [];
            for(var type in google.maps.MapTypeId) {
                mapTypeIds.push(google.maps.MapTypeId[type]);
            }
            mapTypeIds.push("OSM");
 
            var map = new google.maps.Map(element, {
                center: new google.maps.LatLng(-34.4510128, -58.6267865),
                zoom: 14,
                mapTypeId: "OSM",
                mapTypeControlOptions: {
                    mapTypeIds: mapTypeIds
                }
            });
 
            map.mapTypes.set("OSM", new google.maps.ImageMapType({
                getTileUrl: function(coord, zoom) {
                    return "http://tile.openstreetmap.org/" + zoom + "/" + coord.x + "/" + coord.y + ".png";
                },
                tileSize: new google.maps.Size(256, 256),
                name: "OpenStreetMap",
                maxZoom: 18
            }));

            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(-34.4510128, -58.6267865),
                map: map,
                draggable: false
            });

        </script>
    </body>
</html>
