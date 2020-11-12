var WebSocketServer = require('ws').Server
var wss = new WebSocketServer({
    port: 8090
});

var connections = [];

wss.on('connection', function connection(ws, req){
    console.log('some user connected');
    connections.push(ws);

    const ip = req.socket.remoteAddress;
    console.log(ip);

    //===================================================
    ws.on('close', function(){
        connections = connections.filter(function(conn, i){
            return (conn === ws) ? false : true;
        });
    });

    ws.on('message', function incoming(message){
        console.log('received : %s', message)
        console.log(ws);

        wss.clients.forEach(function(client){
            if(client !== ws){
                client.send(message);
            }
        });
        //ws.send(message);
    })
    //===================================================
    ws.send('message sended from server');
})

function broadcast(message){
    connections.forEach(function (con, i){
        con.send(message);
    });
};