const { DH_NOT_SUITABLE_GENERATOR } = require('constants');

var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

var store = {};

server.listen(3000, () => {
	console.log('URL = ws://localhost:3000/socket.io/?EIO=4&transport=websocket');
});

app.get('/', function (req, res) {
	res.send('Hellow world"/"');
});

io.on('connection', function (socket) {

	socket.on('player connect', function (msg) {
		var userobj = {
			'room' : msg.roomid,
			'name' : msg.name,
		};
		
		store[msg.id] = userobj;
		socket.join(msg.roomid);
		
		console.log('recv: player connect');
	});

	socket.on('play', function () {
		console.log('test Console log');
	});

	socket.on('Maching', function () {

		console.log('Room Maching');
	});

	socket.on('disconnect', () => {
		console.log('recv: player disconnected');
	});
});