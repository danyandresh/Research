(function () {
    var githubService = function ($http) {

        var returnData = function (response) {
            return response.data;
        };

        var getUser = function (username) {
            return $http.get("https://api.github.com/users/" + username)
                .then(returnData);
        };

        var getRepos = function (user) {
            return $http.get(user.repos_url)
                .then(returnData);
        };

        return {
            getUser: getUser,
            getRepos: getRepos
        };
    };

    var demoModule = angular.module('demoModule');
    demoModule.factory("githubService", githubService);
}());