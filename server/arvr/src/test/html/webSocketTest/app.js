var stompClient = null;

function setConnected(connected) {
    $("#connect").prop("disabled", connected);
    $("#disconnect").prop("disabled", !connected);
}



function testAddPOI() {
	var str = $('#newPOI').val(); 
	
	addPOI(str); 
}

function testRemovePOI() {
	var str = $('#removePOI').val(); 
	
	removePOI(str); 
}


$(function () {
    $("form").on('submit', function (e) {
        e.preventDefault();
    });
    $( "#connect" ).click(function() { 
		connect(); 
		if( stompClient !== null ) 
			setConnected(true); 
		else
			setConnected(false); 
	});
    $( "#disconnect" ).click(function() { 
		disconnect();
		if( stompClient !== null ) 
			setConnected(true); 
		else
			setConnected(false); 
		});
});