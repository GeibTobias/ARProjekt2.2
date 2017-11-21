var directionsDisplay;
var map;
var toVisitPoiIDs = [];
var pois = [];

function findPoi(id) {
    return pois.find(function (poi) {
        return poi.place_id == id;
    });
}

function addMarker(place, codeID) {
    id = codeID;
    if (id < 10) {
        id = '0' + id;
    }
    //var image = 'images/markers144/VuMark' + id + '.png'
    var image = 'images/Chateau/Chateau_0.png'

    // If the request succeeds, draw the place location on
    // the map as a marker, and register an event to handle a
    // click on the marker.
    findPoi(place.place_id).marker = new google.maps.Marker({
        map: map,
        position: place.geometry.location,
        icon: {
            url: image ,
            scaledSize: new google.maps.Size(146, 71)
        }
    });
}

function initPois(startLocation, callback) {
    // Specify location, radius and place types for your Places API search.
    var request = {
        location: startLocation,
        radius: '20000',
        type: 'point_of_interest'
    };

    // Create the PlaceService and send the request.
    // Handle the callback with an anonymous function.
    var service = new google.maps.places.PlacesService(map);
    var pagesCount = 0;

    service.nearbySearch(request, function (results, status, pagination) {
        if (status == google.maps.places.PlacesServiceStatus.OK) {
            for (var i = 0; i < results.length; i++) {

                place = results[i];
                pois.push(place);
                addMarker(place, pois.length - 1);
            }

            if (pagesCount < 1) {
                pagesCount++;
                pagination.nextPage()
            } else {
                callback();
            }
        }
    });
}

function onPoiListChanged(poiIDs) {
    toVisitPoiIDs = poiIDs;

    var waypoints = [];
    poiIDs.forEach(function (poiID) {
        waypoints.push({
            location: findPoi(poiID).geometry.location
        });
    });

    calculateRoute(waypoints);
}

function calculateRoute(waypoints) {
    // Set destination, origin and travel mode.
    var request = {
        destination: waypoints[waypoints.length - 1],
        origin: waypoints[0],
        travelMode: 'WALKING',
        waypoints: waypoints.slice(1, waypoints.length - 1),
        optimizeWaypoints: true
    };

    // Pass the directions request to the directions service.
    var directionsService = new google.maps.DirectionsService();
    directionsService.route(request, function (response, status) {
        if (status == 'OK') {
            // Display the route on the map.
            directionsDisplay.setDirections(response);
        }
    });
}

function initMap() {
    // New York
    var start = {
        lat: 40.697139,
        lng: -73.979781
    };

    map = new google.maps.Map(document.getElementById('map'), {
        center: start,
        zoom: 14,
        scrollwheel: false
    });

    directionsDisplay = new google.maps.DirectionsRenderer({
        map: map,
        suppressMarkers: true,
        preserveViewport: true,
        polylineOptions: {
            strokeColor: 'Red'
        }
    });

    initPois(start, function () {
        console.log(pois);
        onPoiListChanged([pois[0].place_id, pois[1].place_id, pois[2].place_id, pois[3].place_id, pois[4].place_id, pois[5].place_id, pois[6].place_id, pois[7].place_id, pois[8].place_id, pois[9].place_id]);
        
        adaptMarkerToZoomLevel(map.zoom);
    });

    map.addListener('zoom_changed', function () {
        console.log('zoom level: ' + map.zoom);
        
        adaptMarkerToZoomLevel(map.zoom)
    });
    
    connect();
    getRoute();
}

function adaptMarkerToZoomLevel(level) {
    var threshold = [1000000,
                     1000000,
                     1000000,
                     1000000,
                     1000000,
                     1000000,
                     1000000,
                     1000000,
                     262144,
                     131072,
                     65536,
                     32768,
                     32768,
                     2024,
                     1024,
                     512,
                     256,
                     128,
                     64,
                     32,
                     16,
                     8,
                     4,
                     2,
                     1,
                     1,
                     1,
                     1,
                     1,
                     1];
    
    for (var i = 0; i < pois.length - 1; i++) {
        pois[i].marker.setMap(map);
    }

    for (var i = 0; i < pois.length - 1; i++) {
        for (var j = i + 1; j < pois.length - 1; j++) {
            if (pois[i].marker.getMap() != null && getDistanceBetween(pois[i], pois[j]) < threshold[map.zoom]) {
                pois[j].marker.setMap(null);
            }
        }
    }
}

function getDistanceBetween(placeA, placeB) {
    return google.maps.geometry.spherical.computeDistanceBetween(placeA.geometry.location, placeB.geometry.location);
}

var stompClient = null;
var reconnect = false; 

//
// Connect to server websocket
//
function connect() {
	
	reconnect = true; 
	
    var socket = new SockJS('http://localhost:8080/ws-map-update');
    stompClient = Stomp.over(socket);
    stompClient.connect({}, function (frame) {
		console.log("connecting..."); 
        console.log('Connected: ' + frame);

		stompClient.subscribe('/map/test', function(data) {
			console.log("Received data from server: ", data); 
		}); 
		
		stompClient.subscribe('/map/route/update', function(route) {
			onRouteUpdate(JSON.parse(route.body)); 
		}); 
    });
	
	socket.onclose = function(e) {
		
		if( reconnect ) {
			setTimeout(() => {
				connect();
			  }, 500);
		}
	};
}

function disconnect() {
	
	reconnect = false; 
	
    if (stompClient !== null) {
        stompClient.disconnect();
		stompClient = null; 
    }

    console.log("Disconnected");
}

mapUpdateCon = null; 
// 
// Method to subscribe to map updates
//
function subscribeMapUpdates() {
	mapUpdateCon = stompClient.subscribe('/map/update', function (update) {
		onMapSettingUpdate(JSON.parse(update.body));
	});
}

//
// Method to unsubscribe map updates
//
function unsubscribeMapUpdates() {
	mapUpdateCon.unsubscribe(); 
	mapUpdateCon = null; 
}



function onRouteUpdate(routeList) {
	console.log(routeList); 
	
	// 
	// implement here what you wanna do with the new route updates
	//
    onPoiListChanged(routeList);
}

function onMapSettingUpdate(update) {
	console.log("update from server: ", update); 
	
	// 
	// implement here what you wanna do with map updates
	//
    map.setZoom(update.zoom);
    map.panTo(update.coords);
}

function sendMapUpdate(lattitude, longtitude, zoom) {
	
	//
	// pass to this function the necessary map updates
	// updates will be send to server
	//
	// example how to send!
    // old: stompClient.send("/app/setmap", {}, JSON.stringify({'coords': { 'lattitude' : 232.23, 'longtitude' : '4555.4323' }, 'zoom' : 1 }));
	stompClient.send("/app/setmap", {}, JSON.stringify({'coords': { 'lat' : 232.23, 'lng' : '4555.4323' }, 'zoom' : 1 }));
}


//
// Send a complete route to the server
// The old route will be replaced by this
//
function sendRoute(route) {
	stompClient.send("/app/map/route/clientupdate", {}, JSON.stringify(route)); 
}

//
// Send Request for route update
// subscribed method onRouteUpdate will be invoked wiht the response
//
function getRoute() {
	stompClient.send("/app/map/route/get", {}, null); 
}


function addPOI(poi_string) {
	stompClient.send("/app/map/route/add", {}, poi_string); 
}

function removePOI(poi_string) {
	stompClient.send("/app/map/route/remove", {}, poi_string); 
}