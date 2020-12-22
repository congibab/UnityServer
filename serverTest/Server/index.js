const { DH_NOT_SUITABLE_GENERATOR } = require('constants');
const { json } = require('express');
const { v4: uuidv4 } = require('uuid');

var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

var consola = require('consola');
//=======================================
//=======================================

var Clients =[];
var Rooms = [];
var Massages = [];

server.listen(3000, () => {
	console.log('URL = ws://localhost:3000/socket.io/?EIO=4&transport=websocket');
	consola.success('URL = ws://localhost:3000/socket.io/?EIO=4&transport=websocket');
});

app.get('/', function (req, res) {
	res.send('Hellow world"/"');
});

io.on('connection', function (socket) {

	var thisPlayerId = uuidv4();
	var lobby = 'lobby';
	socket.join(lobby);

	Clients[thisPlayerId] = socket;
	console.log("Another user connection in lobbty :" + thisPlayerId);
	// var player = {
	// 	Room: lobby,
	// 	id: thisPlayerId,
	// 	x: 0,
	// 	y: 0,
	// 	z: 0
	// };

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
		io.to(lobby).emit('UpdateRoomList', Room);
	});

	socket.on('joinRoom', function (data) {
		console.log('joinRoom : ' + data.name + ' UUID : ' + data.UUID);

		if (Rooms[data.name].currnetUUID[0] == '') {
			Rooms[data.name].currnetUUID[0] = data.UUID;
		}

		else if (Rooms[data.name].currnetUUID[0] != '') {
			Rooms[data.name].currnetUUID[1] = data.UUID;
		}

		console.log(Rooms[data.name].currnetUUID);
		socket.leave(lobby);
		socket.join(data.name);
		
		//socket.emit('GameInit', Rooms[data.name]);
		//socket.to(data.name).emit('GameInit', Rooms[data.name]);
		io.to(data.name).emit('GameInit', Rooms[data.name]);

		//delete[Rooms[data.name]];
		//socket.emit('RemoveRoom', {});

		if (Rooms[data.name].currnetUUID[0] != '' && Rooms[data.name].currnetUUID[1] != '') {
			delete Rooms[data.name];
		}
	});

	//  socket.on('Movement', function (data) {
	// 	console.log(data);
	// 	socket.emit('UpdatePosition', data);
	// 	socket.broadcast.emit('UpdatePosition', data);
	// });

	socket.on('MovementRequest', function (data){
		console.log(data);
		io.to(data.RoomName).emit('UpdatePosition', data);
	});

	socket.on('BallMovementRequest', function (data){
		consola.log('BallMovementRequest');
		console.log(data);
		io.to(data.RoomName).emit('UpdateBallPosition', data);
	});

	socket.on('BallPositionReset', function (data){
		consola.log('BallReset');
		console.log(data);
		io.to(data.RoomName).emit('ballPositionReset', data);
	});

	socket.on('UpdateSore', function (data2){
		consola.log('UpdateSore');
		console.log(data2);
		io.to(data2.RoomName).emit('UpdateSore', data2);
	});

	socket.on('disconnect', () => {
		console.log('recv: player disconnected: ' + thisPlayerId);
		 delete Clients[thisPlayerId];
		// socket.broadcast.emit('disconnected', { id: thisPlayerId });
		console.log('some user disconnection');
	});
});

//172.19.78.102