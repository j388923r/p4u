(function () {
    'use strict';

    angular
        .module('myApp')
        .service('suggestionService', suggestionService);

    suggestionService.$inject = ['$http'];

    function suggestionService($http) {
        this.getPieceSuggestions = getPieceSuggestions;
        this.getSuggestionsByUser = getSuggestionsByUser;
        this.getSuggestionsToUser = getSuggestionsToUser;

        function getPieceSuggestions(pieceId) {
            return $http.get('/api/Suggestion/piece?pieceId=' + pieceId).success(function (suggestions) {
                return suggestions;
            });
        }

        function getSuggestionsByUser(username) {
            return $http.get('/api/Suggestion/from?fromUser=' + username).success(function (suggestions) {
                return suggestions;
            });
        }

        function getSuggestionsToUser(username) {
            return $http.get('/api/Suggestion/to?toUser=' + username).success(function (suggestions) {
                return suggestions;
            });
        }
    }
})();