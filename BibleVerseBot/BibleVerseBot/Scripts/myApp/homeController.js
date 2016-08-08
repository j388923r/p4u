(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('homeController', homeController);

    homeController.$inject = ['$scope']; 

    function homeController($scope) {
        $scope.title = 'homeController';

        activate();

        function activate() { }
    }
})();
