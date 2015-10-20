﻿(function () {
    var app = angular.module('demoModule', []);

    var createWorker = function() {

        var workCount = 0;

        var task1 = function() {
            workCount += 1;
            console.log('task 1 ' + workCount);
        }

        var task2 = function() {
            workCount += 1;
            console.log('task 2 ' + workCount);
        }

        return {
            job1: task1,
            job2: task2
        };
    }

    var worker = createWorker();

    worker.job1();
    worker.job2();

    var indexCtrl = function($scope, $http) {

        var person = {
            firstname: "Daniel"
        };

        $scope.person = person;

        var onUserComplete = function(response) {
            $scope.user = response.data;
        };

        var onError = function(reason) {
            $scope.error = 'Could not fetch the user';
        };

        $http
            .get('https://api.github.com/users/danyandresh')
            .then(onUserComplete, onError);
    };

    app.controller('indexCtrl', ['$scope', '$http', indexCtrl]);
}());
