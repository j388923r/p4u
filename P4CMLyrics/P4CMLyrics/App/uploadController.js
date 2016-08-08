(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('uploadController', uploadController);

    uploadController.$inject = ['$scope', '$routeParams', '$sce', 'pieceService', 'userService'];

    function uploadController($scope, $routeParams, $sce, pieceService, userService) {

        $scope.user = userService.getUser();

        $scope.currentComponent = { type: "plain" };
        $scope.components = [];
        $scope.currentIndex = 0;
        $scope.date = Date.now();
        $scope.currentPiece = {};

        pieceService.getPiece('Dear Man').success(function (piece) {
            $scope.currentPiece = piece;
            console.log(piece);
            $scope.title = piece.title;
            $scope.link = piece.p4cmlink;
            $scope.artist = piece.artist;
            $scope.user = { name: piece.owner };
            $scope.unformattedComponents = pieceService.mergeComponents(piece).components;
            console.log($scope.unformattedComponents);
            $scope.components = pieceService.formatComponents($scope.unformattedComponents);
            $scope.currentIndex = $scope.components.length;
            console.log(piece);
        });

        activate();

        $scope.add = function () {
            console.log($scope.currentComponent);
            if ($scope.currentIndex === $scope.components.length) {
                $scope.unformattedComponents[$scope.currentIndex] = $scope.currentComponent;
                $scope.currentComponent.text = $sce.trustAsHtml($scope.currentComponent.text.replace(/\n/g, "</br>"));
                $scope.components[$scope.currentIndex] = ($scope.currentComponent);
            }

            $scope.currentIndex = $scope.components.length;
            $scope.currentComponent = { type: "plain" };
        }

        $scope.edit = function () {
            if ($scope.currentIndex !== $scope.components.length) {
                $scope.unformattedComponents[$scope.currentIndex] = $scope.currentComponent;
                $scope.currentComponent.text = $sce.trustAsHtml($scope.currentComponent.text.replace(/\n/g, "</br>"));
                $scope.components[$scope.currentIndex] = $scope.currentComponent;

            }
        }

        $scope.select = function (index) {
            $scope.currentIndex = index;
            $scope.currentComponent = $scope.components[$scope.currentIndex];
            $scope.cancelComponent = {
                annotationHTML: $scope.currentComponent.annotationHTML,
                index: $scope.currentComponent.index,
                referenceType: $scope.currentComponent.referenceType,
                text: $scope.currentComponent.text,
                type: $scope.currentComponent.type,
            };
            console.log("Select", $scope.currentComponent);
        }

        $scope.cancel = function () {
            $scope.components[$scope.currentIndex] = $scope.currentComponent = $scope.cancelComponent;
            $scope.cancelComponent = undefined
        }

        $scope.save = function () {
            var components = [];
            for (var i = 0; i < $scope.unformattedComponents.length; i++) {
                components.push({
                    annotationHTML: $scope.unformattedComponents[i].annotationHTML,
                    index: $scope.unformattedComponents[i].index,
                    referenceType: $scope.unformattedComponents[i].referenceType,
                    text: $scope.unformattedComponents[i].text.$$unwrapTrustedValue(),
                    type: $scope.unformattedComponents[i].type,
                })
            }
            console.log(components);
            pieceService.savePiece({title : $scope.title, p4cmlink: $scope.link, artist: $scope.artist, owner: $scope.user.name, components : components}).success(function (response) {

            });
        }

        function activate() { }
    }
})();
