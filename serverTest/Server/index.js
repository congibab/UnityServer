const { DH_NOT_SUITABLE_GENERATOR } = require('constants');
const { json } = require('express');
const { v4: uuidv4 } = require('uuid');

var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

var colors = require('colors/safe');
//=======================================
//=======================================

var players = [];
var Rooms = [];

server.listen(3000, () => {
	console.log(colors.rainbow('URL = ws://localhost:3000/socket.io/?EIO=4&transport=websocket'));
});

app.get('/', function (req, res) {
	res.send('Hellow world"/"');
});

io.on('connection', function (socket) {

	var thisPlayerId = uuidv4();
	var lobby = 'lobby';
	socket.join(lobby);
	console.log(colors.yellow("Another user connection in lobbty :" + thisPlayerId));

	// var player = {
	// 	Room: lobby,
	// 	id: thisPlayerId,
	// 	x: 0,
	// 	y: 0,
	// 	z: 0
	// };

	socket.emit("InitPlayerid", { id: thisPlayerId });

	// if (Rooms.length != 0) {
	// 	for (var i in Rooms) {
	// 		io.to(lobby).emit("UpdateRoomList", Rooms[i]);
	// 	}
	// }

	for (var i in Rooms) {
		io.to(lobby).emit("UpdateRoomList", Rooms[i]);
	}



	// socket.on('NetworkStart', () => {
	// 	players[thisPlayerId] = player;
	// 	players[lobby] = player;
	// 	//=======
	// 	console.log('Client connected, broadcast ing spawn, id: ', thisPlayerId);
	// 	socket.broadcast.emit('OtherSpawn', { id: thisPlayerId });
	// 	//=======

	// 	for (var playerId in players) {
	// 		if (playerId == thisPlayerId) {
	// 			socket.emit('PlayableSpawn', players[playerId]);
	// 		}
	// 		else {
	// 			socket.emit('OtherSpawn', players[playerId]);
	// 			console.log('Sending inti to new Player UUID: ', playerId);
	// 		}
	// 	};
	// });

	// socket.on('Movement', function (data) {
	// 	console.log(data);
	// 	socket.emit('UpdatePosition', data);
	// 	socket.broadcast.emit('UpdatePosition', data);
	// });

	socket.on('creatRoom', function (data) {
		console.log(colors.magenta('create room' + data));
		var Room = {
			name: data.name,
			//currnetUUID: [data.UUID, ''],
			currnetUUID: ['', ''],
		};
		//Rooms.push(Room);
		Rooms[data.name] = Room;
		//socket.emit('UpdateRoomList', data);
		io.to(lobby).emit('UpdateRoomList', Room);
	});

	socket.on('joinRoom', function (data) {
		console.log(colors.blue('joinRoom : ' + data.name + ' UUID : ' + data.UUID));

		// var maching = {
		// 	player1 : Rooms[data.index].UUID,
		// 	player2 : data.UUID
		// };

		// Rooms[data.index].currnetUUID[1] = data.UUID;
		// console.log(Rooms[data.index].currnetUUID);
		// socket.leave(lobby);
		// socket.join(data.name);
		// io.to(data.name).emit('test', Rooms[data.index]);

		//Rooms[data.name].currnetUUID[1] = data.UUID;
		if (Rooms[data.name].currnetUUID[0] == '') {
			Rooms[data.name].currnetUUID[0] = data.UUID;
		}

		else if (Rooms[data.name].currnetUUID[0] != '') {
			Rooms[data.name].currnetUUID[1] = data.UUID;
		}

		console.log(colors.blue(Rooms[data.name].currnetUUID));
		socket.leave(lobby);
		socket.join(data.name);
		io.to(data.name).emit('GameInit', Rooms[data.name]);
		//delete[Rooms[data.name]];
		socket.emit('RemoveRoom', {});
		//delete Rooms[data.name];
	});

	socket.on('joinRoom', function (data) {

	});

	socket.on('disconnect', () => {
		// console.log('recv: player disconnected: ' + thisPlayerId);
		// delete players[thisPlayerId];
		// socket.broadcast.emit('disconnected', { id: thisPlayerId });
		console.log(colors.red('some user disconnection'));
	});
});

//172.19.78.102