(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('profileController', profileController);

    profileController.$inject = ['$scope', '$location', 'userService', 'pieceService', 'suggestionService']; 

    function profileController($scope, $location, userService, pieceService, suggestionService) {
        $scope.title = 'profileController';

        activate();

        $scope.user = userService.getUser();
        
        suggestionService.getSuggestionsToUser($scope.user.username).success(function (suggestions) {
            $scope.user.suggestions = suggestions;
        });

        var i = 0;

        pieceService.getSamplePiece().success(function (piece) {
            console.log(piece);
            $scope.user.suggestions[i].piece = piece;
            $scope.user.suggestions[i].piece.components = pieceService.formatComponents(pieceService.mergeComponents(piece).components);
            console.log($scope.user.suggestions[i].piece.components);
        });



        if ($scope.user === {}) {
            $location.url = '/#/';
        }

        function populateSuggestions(suggestions) {
            var returnSuggs = suggestions;
            for (var i = 0; i < returnSuggs.length; i++) {
                returnSuggs[i].pieceTitle = "Glass";
            }
            return returnSuggs;
        }

        function activate() { }
    }
})();
