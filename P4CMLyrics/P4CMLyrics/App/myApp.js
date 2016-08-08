var app = (function () {
    'use strict';

    return angular.module('myApp', [
        // Angular modules
        'ngAnimate',
        'ngRoute',

        // Custom modules

        // 3rd Party Modules
        'ui.bootstrap'
    ]);
})();

app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        controller: 'homeController',
        templateUrl: '/Views/home.html'
    }).when('/profile', {
        controller: 'profileController',
        templateUrl: '/Views/profile.html'
    }).when('/upload', {
        controller: 'uploadController',
        templateUrl: '/Views/upload.html'
    }).when('/artists', {
        controller: 'artistsController',
        templateUrl: '/Views/artists.html'
    });
});