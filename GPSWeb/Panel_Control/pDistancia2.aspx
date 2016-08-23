<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pDistancia2.aspx.vb" Inherits="GPSWeb.pDistancia2" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Buscar y trazar rutas | Alex Franco</title>

    <link type="text/css" rel="stylesheet" media="screen" href="/css/uncompressed/baseline.reset.css">
    <link type="text/css" rel="stylesheet" media="screen" href="/css/uncompressed/baseline.base.css">
    <link type="text/css" rel="stylesheet" media="screen" href="/css/uncompressed/baseline.type.css">
    <link type="text/css" rel="stylesheet" media="screen" href="/css/uncompressed/baseline.table.css">
    <link type="text/css" rel="stylesheet" media="screen" href="/css/uncompressed/baseline.form.css">
    <link type="text/css" rel="stylesheet" media="screen" href="/css/uncompressed/baseline.grid.css">
    <link type="text/css" rel="stylesheet" media="screen" href="/css/site.css">

    <link rel="shortcut icon" href="/favicon.ico">

    <script type="text/javascript" src="/scripts/html5.js"></script>
    <script type="text/javascript" src="/scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/scripts/jquery.twitter.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/scripts/demos.js"></script>
    <script src="//platform.linkedin.com/in.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBRxC6Y4f-j6nECyHWigtBATtJyXyha-XU&libraries=adsense&sensor=true&language=es"></script>
    <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-7130344-7']);
        _gaq.push(['_setDomainName', 'alexfranco.mx']);
        _gaq.push(['_trackPageview']);

        (function() {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

        window.google_analytics_uacct = "UA-7130344-7";
    </script>
</head>

<body class="dmaf">

<div id="page" class="layout-grid">
  
    <article id="main-content" class="width4">                <section class="column width4 first" xmlns="http://www.w3.org/1999/html">
    <h2>Buscar y trazar rutas</h2>
   
    <div class="column first" id="info"
      
        <div class="demo column width4 first">
            <div class="column width1 first">
Origen <input type="text" id="start" placeholder="address or coordinates" />
		<br/>
Destino <input type="text" id="end" placeholder="address or coordinates" />
		</div>
<div class="column unitx1 align-center">|</div>
<div class="column width1">
Tipo de Viaje 
		<select id="travelMode" class="routeOptions" >
			<option value="DRIVING" selected="selected">En Auto</option>
          	<option value="BICYCLING">En Bicicleta</option>
          	<option value="WALKING">Caminando</option>
      	</select><br/>
Unidades de medida
      	<select id="unitSystem" class="routeOptions">
          	<option value="METRIC" selected="selected">Métrico</option>
          	<option value="IMPERIAL">Imperial</option>
      	</select>
</div>
<div class="first">
<p class="button"><a href="javascript:void(0)" id="search" class="send" >Buscar Ruta</a></p>
</div>
<br/>
	<div id="results" style="width: 990px; height: 500px;" class="column first">
		<div id="map_canvas" style="width: 65%; height: 100%; float: left;"></div>
		<div id="directions_panel" style="width: 35%; height: 100%; overflow: auto; float: right;"></div>
	</div>        </div>
    </div>
</section>
            </article>
                    <div class="width4 first" id="footer"></div>
        </div>

                <script type='text/javascript'>
            var map = null;
	var directionsDisplay = null;
	var directionsService = null;

	function initialize() {
	    var myLatlng = new google.maps.LatLng(20.68009, -101.35403);
	    var myOptions = {
	        zoom: 4,
	        center: myLatlng,
	        mapTypeId: google.maps.MapTypeId.ROADMAP
	    };
	    map = new google.maps.Map($("#map_canvas").get(0), myOptions);
		directionsDisplay = new google.maps.DirectionsRenderer();
		directionsService = new google.maps.DirectionsService();
	}

	function getDirections(){
		var start = $('#start').val();
		var end = $('#end').val();
		if(!start || !end){
			alert("Start and End addresses are required");
			return;
		}
		var request = {
		        origin: start,
		        destination: end,
		        travelMode: google.maps.DirectionsTravelMode[$('#travelMode').val()],
		        unitSystem: google.maps.DirectionsUnitSystem[$('#unitSystem').val()],
		        provideRouteAlternatives: true
	    };
		directionsService.route(request, function(response, status) {
	        if (status == google.maps.DirectionsStatus.OK) {
	            directionsDisplay.setMap(map);
	            directionsDisplay.setPanel($("#directions_panel").get(0));
	            directionsDisplay.setDirections(response);
	        } else {
	            alert("There is no directions available between these two points");
	        }
	    });
	}

	$('#search').live('click', function(){ getDirections(); });
	$('.routeOptions').live('change', function(){ getDirections(); });
	
	$(document).ready(function() {
	    initialize();
   gmaps_ads();
	});        </script>
            </body>
</html>