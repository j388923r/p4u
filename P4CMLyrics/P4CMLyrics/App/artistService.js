(function () {
    'use strict';

    angular
        .module('myApp')
        .service('artistService', artistService);

    artistService.$inject = ['$http'];

    function artistService($http) {
        this.getArtist = getArtist;
        this.getSampleArtist = getSampleArtist;

        function getArtist(artistId) {
            return $http.get('/api/Artist?artistId=' + artistId).success(function (artist) {
                return artist;
            });
        }

        function getSampleArtist() {
            return $http.get('/api/Artist?artistId=sample').success(function (artist) {
                return artist;
            })
        }
    }
})();