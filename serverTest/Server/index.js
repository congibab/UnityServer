const { DH_NOT_SUITABLE_GENERATOR } = require('constants');
const { json } = require('express');
const { v4: uuidv4 } = require('uuid');

var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

//var consola = require('consola');
//=======================================a
//=======================================

var Clients = [];
var Rooms = [];
var Massages = [];

server.listen(3000, () => {
	console.log('URL = ws://localhost:3000/socket.io/?EIO=4&transport=websocket');
	//consola.success('URL = ws://localhost:3000/socket.io/?EIO=4&transport=websocket');
});

app.get('/', function (req, res) {
	res.send('Hellow world"/"');
});

io.on('connection', function (socket) {

	var thisPlayerId = uuidv4();
	var lobby = 'lobby';
	socket.join(lobby);

	Clients[thisPlayerId] = socket;
	console.log(Object.keys(Clients).length);
	console.log("Another user connection in lobbty :" + thisPlayerId);

	socket.emit("InitPlayerid", { id: thisPlayerId });

	for (var i in Rooms) {
		//io.to(lobby).emit("UpdateRoomList", Rooms[i]);
		socket.emit("UpdateRoomList", Rooms[i]);
	}

	socket.on('creatRoom', function (data) {
		console.log('create room' + data);
		var Room = {
			name: data.name,
			//currnetUUID: [data.UUID, ''],
			currnetUUID: ['', ''],
		};
		//Rooms.push(Room);
		Rooms[data.name] = Room;
		//socket.emit('UpdateRoomList', data);
		//io.to(lobby).emit('UpdateRoomList', Room);
		socket.to(lobby).emit('UpdateRoomList', Room);
	});

	socket.on('joinRoom', function (data) {
		console.log('joinRoom : ' + data.name + ' UUID : ' + data.UUID);

		if (Rooms[data.name].currnetUUID[0] == '') {
			Rooms[data.name].currnetUUID[0] = data.UUID;
		}

		else {
			Rooms[data.name].currnetUUID[1] = data.UUID;
		}

		console.log(Rooms[data.name].currnetUUID);
		socket.leave(lobby);
		socket.join(data.name);

		io.to(data.name).emit('GameInit', Rooms[data.name]);

		if (Rooms[data.name].currnetUUID[0] != '' && Rooms[data.name].currnetUUID[1] != '') {
			socket.broadcast.emit('removeRoom', { name: data.name });
			delete Rooms[data.name];
		}
	});

	socket.on('joinlobby', function (data) {
		console.log('joinRoom : ' + data.name + ' UUID : ' + data.UUID);

		socket.leave(data.name);
		socket.join(lobby);

		//io.to(lobby).emit('GameInit', Rooms[data.name]);
	});

	//  socket.on('Movement', function (data) {
	// 	console.log(data);
	// 	socket.emit('UpdatePosition', data);
	// 	socket.broadcast.emit('UpdatePosition', data);
	// });

	socket.on('MovementRequest', function (data) {
		console.log(data);
		io.to(data.RoomName).emit('UpdatePosition', data);
	});

	socket.on('BallMovementRequest', function (data) {
		console.log('BallMovementRequest');
		console.log(data);
		io.to(data.RoomName).emit('UpdateBallPosition', data);
	});

	socket.on('BallPositionReset', function (data) {
		console.log('BallReset');
		console.log(data);
		io.to(data.RoomName).emit('ballPositionReset', data);
	});

	socket.on('UpdateSore', function (data2) {
		console.log('UpdateSore');
		console.log(data2);
		io.to(data2.RoomName).emit('UpdateSore', data2);
	});

	socket.on('UpdateChaingLog', function (data) {
		console.log(data);
		//socket.emit("UpdateChaingLog", data);
		io.to(lobby).emit('UpdateChaingLog', data);
	});

	socket.on('disconnect', () => {
		console.log('recv: player disconnected: ' + thisPlayerId);
		delete Clients[thisPlayerId];
		// socket.broadcast.emit('disconnected', { id: thisPlayerId });
		console.log('some user disconnection');
	});
});

//172.19.78.102