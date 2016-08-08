(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('pieceController', pieceController);

    pieceController.$inject = ['$scope']; 

    function pieceController($scope, $http, $routeParams) {
        $scope.title = 'pieceController';

        activate();

        function activate() { }
    }
})();
