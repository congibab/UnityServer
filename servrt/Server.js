//include Class
//==========================================
var Transform = require("./Class/Transform.js");
var transform = new Transform();
var JsonStatus = require("./Class/JsonStatus.js");
var jsonStatus = new JsonStatus();

const { v4: uuidv4 } = require('uuid');

//var Matrix = require('transformation-matrix');
//const { compose, translate, scale, rotate, applyToPoint } = require("transformation-matrix");

//include webSocek Class
var WebSocketServer = require('ws').Server
var wss = new WebSocketServer({
    port: 8090
});

var UUID = [];
var connections = [];

console.log('Server Start');

//for(var i = 0; i < 10; i++){
//    UUID[i] = uuidv4();
//    console.log(UUID[i]);
//}
var jsonData = jsonStatus.getData();
console.log(JSON.stringify(jsonData));

//=====================================
//=====================================
wss.on('connection', function connection(ws, req) {

    jsonData.Types = 'init';
    jsonData.UUID = uuidv4();

    ws.send(JSON.stringify(jsonData));

    console.log('some user connected');
    UUID.push(jsonData.UUID);
    connections.push(ws);

    //===================================================
    ws.on('close', function () {
        connections = connections.filter(function (conn, i) {
            return (conn === ws) ? false : true;
        });
    });

    ws.on('message', function incoming(message) {
        console.log('<<== received : %s', message)
        //===========================

        //Json type inspection
        try {
            var test = JSON.parse(message);
            switch (test['Types']) {
                case "init":
                    console.log("data type is   transform");
                    break;

                case "Player1_input":
                    console.log("data type is   input");
                    //ws.send(JSON.stringify(jsonData));
                    break;

                case "Player2_input":
                    console.log("data type is   input");
                    break;

                default:
                    console.log("data type not find");
                    break;
            }
            wss.clients.forEach(function (client) {
                // if(client !== ws){
                //     client.send(message);
                // }
                client.send(message);
            });

        } catch (e) {
            console.log("message is not JSON Type");
            ws.send(message);
        }
        //===========================
    })
    //===================================================
    ws.send('message sended from server');
});