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

    var indexCtrl = function($scope, $http) {

        
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

        $scope.search = function (username) {
            $http
                .get("https://api.github.com/users/" + username)
                .then(onUserComplete, onError);
        };

        $scope.username = "angular";
        $scope.message = "Github Viewer";
        $scope.repoSortOrder = "-stargazers_count";
        $scope.search($scope.username);
    };

    app.controller('indexCtrl', ['$scope', '$http', indexCtrl]);
}());
