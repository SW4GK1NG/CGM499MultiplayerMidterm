var express = require('express')
var http = require('http')
var socketIO = require('socket.io')

var app = express()
var server = http.Server(app)
server.listen(5000)
var io = socketIO(server)
var sum = randomNumber();

io.on('connection',function(socket){

    console.log('client connected')
    console.log(sum);

    socket.on('message.send',function(data){
        var playerData = JSON.stringify(data);
        var player = JSON.parse(playerData);
        var number = player.number;
        var name = player.name;

        console.log(name);
        console.log(number);

        if (number == sum) {

            console.log("V1 ZULUL");
            socket.emit('youwin', data);
            socket.broadcast.emit('vivonzulul', data);

        } else if (number > sum) {

            console.log("Too High");
            socket.emit('high');

        } else if (number < sum) {

            console.log("Too Low");
            socket.emit('low');

        }
    });

    socket.on('reset', function(){
        socket.broadcast.emit('disableReset');
        sum = randomNumber();
        console.log(sum);
    });

});

function randomNumber() {
    return Math.floor(Math.random() * 101);
};

console.log('server statred')