(function () {
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

    var indexCtrl = function($scope, $http, $interval, $log) {

        
        var onError = function (reason) {
            $scope.error = 'Could not fetch the data';
        };

        var onRepos = function(response) {
            $scope.repos = response.data;
        };

        var onUserComplete = function (response) {

            $scope.user = response.data;
            $http.get($scope.user.repos_url)
                .then(onRepos, onError);
        };

        var decrementCountdown = function() {
            $scope.countdown -= 1;
            if ($scope.countdown < 1) {
                $scope.search($scope.username);
            }
        };

        var countdownInterval = null;
        var startCountdown = function() {
            countdownInterval = $interval(decrementCountdown, 1000, $scope.countdown);
        }

        $scope.search = function (username) {
            $log.info('Searching for: ' + username);
            if (countdownInterval) {
                $interval.cancel(countdownInterval);
            };

            $scope.user = null;
            $scope.error = null;
            $http
                .get("https://api.github.com/users/" + username)
                .then(onUserComplete, onError);
        };

        $scope.username = "angular";
        $scope.message = "Github Viewer";
        $scope.repoSortOrder = "-stargazers_count";

        $scope.countdown = 5;
        //$scope.search($scope.username);
        startCountdown();
    };

    app.controller('indexCtrl', ['$scope', '$http', '$interval', '$log', indexCtrl]);
}());
