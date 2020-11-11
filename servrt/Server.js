var WebSocketServer = require('ws').Server
var wss = new WebSocketServer({
    port: 8090
});

wss.on('connection', function(ws){
    console.log('some user connected');
    ws.on('message', function(message){
        console.log('received : %s', message)
        
        wss.clients.forEach(function(client){
                client.send(message);
        });
        //ws.send(message);
    })
    ws.send('message sended from server');
})