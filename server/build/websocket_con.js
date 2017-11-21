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
}

function onMapSettingUpdate(update) {
	console.log("update from server: ", update); 
	
	// 
	// implement here what you wanna do with map updates
	// 
}

function sendMapUpdate(lattitude, longtitude, zoom) {
	
	//
	// pass to this function the necessary map updates
	// updates will be send to server
	//
	// example how to send!
	stompClient.send("/app/setmap", {}, JSON.stringify({'coords': { 'lattitude' : 232.23, 'longtitude' : '4555.4323' }, 'zoom' : 1 }));
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