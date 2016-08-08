(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('artistsController', artistsController);

    artistsController.$inject = ['$scope']; 

    function artistsController($scope) {
        $scope.title = 'artistsController';

        activate();

        function activate() { }
    }
})();
