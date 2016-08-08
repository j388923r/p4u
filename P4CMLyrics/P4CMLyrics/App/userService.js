(function () {
    'use strict';

    angular
        .module('myApp')
        .service('userService', userService);

    userService.$inject = ['$http'];

    function userService($http) {
        this.getUser = getUser;
        this.login = login;
        this.signup = signup;

        var user = {};

        function getUser() {
            return user;
        }

        function login(username, password, $uibModalInstance) {
            $http.post('/api/login', { username: username, password: password }).success(function (data) {
                user = data;

                $uibModalInstance.close({
                    username: username
                });
            });
        }

        function signup(username, password, name) {
            $http.post('/api/register', { username: username, password: password, Name: name }).success(function (data) {
                user = data;
            });
        }

        function getSuggestions(username) {

        }

        function logout() {
            user = {};
        }
    }
})();