(function () {
    'use strict';

    var myApp = angular.module('myApp', [
        // Angular modules
        'ngAnimate',
        'ngRoute',

        // Custom modules
        'p4u'

        // 3rd Party Modules
        
    ]);

    myApp.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/', {
            controller: 'homeController',
            templateUrl: 'Views/home.html'
        }).when('/custom', {
            controller: 'customController',
            templateUrl: 'Views/custom.html'
        });
    }]);
})();
