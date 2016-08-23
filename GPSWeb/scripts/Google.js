function createMarker(map, lng, lat, htmlVentana, icono) {
    var image = new google.maps.MarkerImage(
	              '../images/' + icono,
		           new google.maps.Size(40, 58),
		            new google.maps.Point(0, 0),
		            new google.maps.Point(0, 58)
	            );

    marker = new google.maps.Marker({
        map: map,
        content: htmlVentana,
        icon: image,
        position: new google.maps.LatLng(lat, lng),
        title: ''
    });

    markersArray.push(marker);

    //definicion para usar multiples info windows, le asocio una funcion que cierra la actual al abrir una nueva
    google.maps.event.addListener(marker, 'click', function() {

        /* close the previous info-window */
        closeInfos();
        /* the marker's content gets attached to the info-window: */
        var info = new google.maps.InfoWindow({ content: this.content });
        /* trigger the infobox's open function */
        info.open(map, this);
        /* keep the handle, in order to close it on next click event */
        infos[0] = info;       
    });
    return marker;
}

function createMarkerMarca(map, lng, lat, htmlVentana, icono) {
    var image = new google.maps.MarkerImage(
	              '../images/' + icono,
		           new google.maps.Size(25, 25),
		            new google.maps.Point(0, 0),
		            new google.maps.Point(0, 25)
	            );

    marker = new google.maps.Marker({
        map: map,
        content: htmlVentana,
        icon: image,
        position: new google.maps.LatLng(lat, lng),
        title: ''
    });

    markersArrayMarc.push(marker);

    //definicion para usar multiples info windows, le asocio una funcion que cierra la actual al abrir una nueva
    google.maps.event.addListener(marker, 'click', function() {

        /* close the previous info-window */
        closeInfos();
        /* the marker's content gets attached to the info-window: */
        var info = new google.maps.InfoWindow({ content: this.content });
        /* trigger the infobox's open function */
        info.open(map, this);
        /* keep the handle, in order to close it on next click event */
        infos[0] = info;
    });
    return marker;
}

function createMarkerMarcaG(map, lng, lat, htmlVentana, icono) {
    var image = new google.maps.MarkerImage(
	              '../images/' + icono,
		           new google.maps.Size(25, 25),
		            new google.maps.Point(0, 0),
		            new google.maps.Point(0, 25)
	            );

    marker = new google.maps.Marker({
        map: map,
        content: htmlVentana,
        icon: image,
        position: new google.maps.LatLng(lat, lng),
        title: ''
    });

    markersArrayMarcG.push(marker);

    //definicion para usar multiples info windows, le asocio una funcion que cierra la actual al abrir una nueva
    google.maps.event.addListener(marker, 'click', function () {

        /* close the previous info-window */
        closeInfos();
        /* the marker's content gets attached to the info-window: */
        var info = new google.maps.InfoWindow({ content: this.content });
        /* trigger the infobox's open function */
        info.open(map, this);
        /* keep the handle, in order to close it on next click event */
        infos[0] = info;
    });
    return marker;
}

function createMarkerZoom(map, lng, lat, htmlVentana, icono) {
    var image = new google.maps.MarkerImage(
	              '../images/' + icono,
		           new google.maps.Size(46, 72),
		            new google.maps.Point(0, 0),
		            new google.maps.Point(0, 72)
	            );

    marker = new google.maps.Marker({
        map: map,
        content: htmlVentana,
        icon: image,
        position: new google.maps.LatLng(lat, lng),
        title: ''
    });

    markersArrayUb.push(marker);

    //definicion para usar multiples info windows, le asocio una funcion que cierra la actual al abrir una nueva
    google.maps.event.addListener(marker, 'click', function() {

        /* close the previous info-window */
        closeInfos();
        /* the marker's content gets attached to the info-window: */
        var info = new google.maps.InfoWindow({ content: this.content });
        /* trigger the infobox's open function */
        info.open(map, this);
        /* keep the handle, in order to close it on next click event */
        infos[0] = info;
        map.setZoom(16);
    });
    
      google.maps.event.addListener(marker, 'mouseover',function() {

          /* close the previous info-window */
          closeInfos();
          /* the marker's content gets attached to the info-window: */
          var info = new google.maps.InfoWindow({ content: this.content });
          /* trigger the infobox's open function */
          info.open(map, this);
          /* keep the handle, in order to close it on next click event */
          infos[0] = info;
         
      });
    return marker;
}

function createMarker2(map, lng, lat, htmlVentana, icono) {
    var image = new google.maps.MarkerImage(
	              '../images/' + icono,
		           new google.maps.Size(40, 35),
		            new google.maps.Point(0, 0),
		            new google.maps.Point(0, 35)
	            );

    marker = new google.maps.Marker({
        map: map,
        content: htmlVentana,
        icon: image,
        position: new google.maps.LatLng(lat, lng),
        title: ''
    });

    //markersArray.push(marker);

    //definicion para usar multiples info windows, le asocio una funcion que cierra la actual al abrir una nueva
    google.maps.event.addListener(marker, 'click', function() {

        /* close the previous info-window */
        closeInfos();
        /* the marker's content gets attached to the info-window: */
        var info = new google.maps.InfoWindow({ content: this.content });
        /* trigger the infobox's open function */
        info.open(map, this);
        /* keep the handle, in order to close it on next click event */
        infos[0] = info;
    });
    return marker;
}

function closeInfos() {

    if (infos.length > 0) {

        /* detach the info-window from the marker */
        infos[0].set("marker", null);

        /* and close it */
        infos[0].close();

        /* blank the array */
        infos.length = 0;
    }
}

function addMarker(location) {
    marker = new google.maps.Marker({
        position: location,
        map: map
    });
    markersArray.push(marker);
}

// Removes the overlays from the map, but keeps them in the array
function clearOverlays() {

    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
    }
}

function clearOverlaysMovil() {

    if (markersArrayUb) {
        for (i in markersArrayUb) {
            markersArrayUb[i].setMap(null);
        }
    }
}

function clearOverlays2() {

    if (markersArray) {
       
        for (var i = 0; i < this.markersArray.length -1; i++) {
            this.markersArray[i].setMap(null);
        }
       

    }

   
}

function processReverseGeocoding(location, callback) {
    // Propiedades de la georreferenciación

    var request = {
        latLng: location
    }

    // Invocación a la georreferenciación (proceso asíncrono)

    geocoder.geocode(request, function(results, status) {

        // En caso de terminarse exitosamente el proyecto ...

        if (status == google.maps.GeocoderStatus.OK) {
            // Invoca la función de callback
            callback(results);

            // Retorna los resultados obtenidos
            return results;
        }

        // En caso de error retorna el estado
        return status;
    });
}


// Shows any overlays currently in the array
function showOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(map);
        }
    }
}

// Deletes all markers in the array by removing references to them
function deleteOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
}

function showMarker(locations) {

    // Centra el mapa en la ubicación especificada

    map.setCenter(locations[0].geometry.location);

    //creamos el marcador en el mapa
    marker = new google.maps.Marker({
        map: map, //el mapa creado en el paso anterior
        position: locations[0].geometry.location, //objeto con latitud y longitud
        draggable: true //que el marcador se pueda arrastrar
    });

    markersArray.push(marker);
    updatePosition(locations[0].geometry.location)

    google.maps.event.addListener(marker, 'dragend', function() {
        updatePosition(marker.getPosition());
        markersArray.push(marker);

    });

}

function showMarkerUnique(locations) {

    // Centra el mapa en la ubicación especificada
    clearOverlays();
    map.setCenter(locations[0].geometry.location);

    //creamos el marcador en el mapa
    marker = new google.maps.Marker({
        map: map, //el mapa creado en el paso anterior
        position: locations[0].geometry.location, //objeto con latitud y longitud
        draggable: true //que el marcador se pueda arrastrar
    });

    markersArray.push(marker);
    updatePosition(locations[0].geometry.location)

    google.maps.event.addListener(marker, 'dragend', function() {
        updatePosition(marker.getPosition());
        markersArray.push(marker);

    });

}

function showMarkerPar(locations) {

    // Centra el mapa en la ubicación especificada
 //   clearOverlays2();
    map.setCenter(locations[0].geometry.location);

    //creamos el marcador en el mapa
    marker = new google.maps.Marker({
        map: map, //el mapa creado en el paso anterior
        position: locations[0].geometry.location, //objeto con latitud y longitud
        draggable: true //que el marcador se pueda arrastrar
    });

    markersArray.push(marker);
    updatePosition1(locations[0].geometry.location)

    google.maps.event.addListener(marker, 'dragend', function () {
   
      updatePosition1(marker.getPosition());
        markersArray.push(marker);

    });

}
