//include Class
//==========================================
var Transform = require("./Class/Transform.js");
var transform = new Transform();
var JsonStatus = require("./Class/JsonStatus.js");
var jsonStatus = new JsonStatus();

//var Matrix = require('transformation-matrix');
//const { compose, translate, scale, rotate, applyToPoint } = require("transformation-matrix");

//include webSocek Class
var WebSocketServer = require('ws').Server
var wss = new WebSocketServer({
    port: 8090
});

console.log('Server Start');

//=====================================
wss.on('connection', function connection(ws, req){
    //include Test
    //========================
    //jsonStatus.test();
    //========================
   
    console.log('some user connected');
    const ip = req.socket.remoteAddress;
    connections.push(ws);
    //===================================================
    ws.on('close', function(){
        connections = connections.filter(function(conn, i){
            return (conn === ws) ? false : true;
        });
    });

    ws.on('message', function incoming(message){
        console.log('<<== received : %s', message)     
        //===========================

        //Json type inspection
        try{
            var test = JSON.parse(message);
            switch(test.type){
                case "transform":
                    console.log("data type is transform");
                break;
                default:
                    console.log("data type not find");
                break;
            }
            wss.clients.forEach(function(client){
                // if(client !== ws){
                //     client.send(message);
                // }
                client.send(message);
            });

        } catch(e){
            console.log("message is not JSON Type");
            ws.send(message);
        }
        //===========================
    })
    //===================================================
    ws.send('message sended from server');
});