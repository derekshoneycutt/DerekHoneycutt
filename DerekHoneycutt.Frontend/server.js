'use strict';
var fs = require('fs');
var http = require('http');
var port = process.env.PORT || 1337;

http.createServer(function (req, res) {

    let url = req.url.trim();
    url = url.split('#')[0].split('?')[0];
    url = url.replace('/', '\\');
    if (url.length < 1 || url === '\\')
        url = "\\index.html";

    fs.readFile(`${__dirname}\\wwwroot${url}`, function (err, data) {
        if (err) {
            res.writeHead(404);
            res.end(JSON.stringify(err));
            return;
        }
        res.writeHead(200);
        res.end(data);
    });
}).listen(port);
