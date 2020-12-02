const { DH_NOT_SUITABLE_GENERATOR } = require('constants');
const { json } = require('express');
const { v4: uuidv4 } = require('uuid');

var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);
//=======================================
//=======================================

var players = [];

server.listen(3000, () => {
	console.log('URL = ws://localhost:3000/socket.io/?EIO=4&transport=websocket');
});

app.get('/', function (req, res) {
	res.send('Hellow world"/"');
});

io.on('connection', function (socket) {

	var thisPlayerId = uuidv4();

	var player = {
		id: thisPlayerId,
		x: 0,
		y: 0,
		z: 0
	};

	socket.on('NetworkStart', () => {
		players[thisPlayerId] = player;
		//=======
		console.log('Client connected, broadcast ing spawn, id: ', thisPlayerId);
		socket.broadcast.emit('OtherSpawn', { id: thisPlayerId });
		//=======

		for (var playerId in players) {
			if (playerId == thisPlayerId) {
				socket.emit('PlayableSpawn', players[playerId]);
			}
			else {
				socket.emit('OtherSpawn', players[playerId]);
				console.log('Sending inti to new Player UUID: ', playerId);
			}
		};
	});

	socket.on('Movement', function (data) {
		console.log(data);
		socket.emit('UpdatePosition', data);
		socket.broadcast.emit('UpdatePosition', data);
	});


	socket.on('disconnect', () => {
		console.log('recv: player disconnected: ' + thisPlayerId);
		delete players[thisPlayerId];
		socket.broadcast.emit('disconnected', { id: thisPlayerId });
	});
});