var express = require('express');
var path = require("path");
var bodyParser = require('body-parser');
var mongo = require("mongoose");

var db = mongo.connect("mongodb://yell:abcsda@52.169.255.164:27017/local?authSource=admin", function (err, response) {
    if (err) { console.log(err); }
    else { console.log('Connected to ' + db, ' + ', response); }
});

//mongo.set('debug', true);


var app = express()
app.use(bodyParser());
app.use(bodyParser.json({ limit: '5mb' }));
app.use(bodyParser.urlencoded({ extended: true }));


app.use(function (req, res, next) {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');
    res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With,content-type');
    res.setHeader('Access-Control-Allow-Credentials', true);
    next();
});

var Schema = mongo.Schema;

var UsersSchema = new Schema({
    name: { type: String },
    address: { type: String },
}, { 
    versionKey: false,
    strict: false
 });


var model = mongo.model('users', UsersSchema, 'users');

app.post("/api/users", function (req, res) {
    var mod = new model(req.body);

    model.updateOne({ 'username': req.body.username }, { 'username': req.body.username }, { upsert: true },
        function (err, data) {
            if (err) {
                res.send(err);
            }
            else {
                console.log(data);
                //{ 'points' : { $exists : false } }
                model.findOne({ 'username': req.body.username }, function (err, data) {
                    if (err) {
                        res.send(err);
                    }
                    else {
                        console.log(data);
                        res.send(data);
                    }
                });
            }
        });
})

app.get("/api/users", function (req, res) {
    model.find({}, function (err, data) {
        if (err) {
            res.send(err);
        }
        else {
            res.send(data);
        }
    });
})


app.listen(4204, "0.0.0.0", function () {

    console.log('Example app listening on port 4204')
})  