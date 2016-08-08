(function () {
    'use strict';

    angular
        .module('myApp')
        .service('pieceService', pieceService);

    pieceService.$inject = ['$http', '$sce'];

    function pieceService($http, $sce) {
        this.getPiece = getPiece;
        this.getSamplePiece = getSamplePiece;
        this.formatComponents = formatComponents;
        this.mergeComponents = mergeComponents;
        this.savePiece = savePiece;

        function getPiece(pieceId) {
            return $http.get('/api/Piece?pieceId=' + pieceId).success(function (piece) {
                return piece;
            });
        }

        function getComponents(blobUri) {
            return $http.get('/api/Piece?blobUri=' + blobUri).success(function (data) {
                return data;
            });
        }

        function getSamplePiece() {
            return $http.get('/api/Piece?pieceId=Dear Man').success(function (piece) {
                return piece;
            });
        }

        function mergeComponents(piece) {
            piece.components = [];
            for (var i = 0; i < piece.plainComponents.length; i++) {
                piece.components[piece.plainComponents[i].index] = piece.plainComponents[i];
            }

            for (var i = 0; i < piece.highlightComponents.length; i++) {
                piece.components[piece.highlightComponents[i].index] = piece.highlightComponents[i];
            }
            return piece;
        }

        function filterComponents(componentList, type) {
            var returnList = [];
            for (var i = 0; i < componentList.length; i++) {
                if (componentList[i].type === type) {
                    componentList[i].index = i;
                    returnList.push(componentList[i]);
                }
            }
            return returnList;
        }

        function formatComponents(componentList) {
            for (var i = 0; i < componentList.length; i++) {
                componentList[i].text = $sce.trustAsHtml(componentList[i].text.replace(/\n/g, "<br />"));
            }
            return componentList;
        }

        function savePiece(piece) {
            piece.plainComponents = filterComponents(piece.components, "plain");
            piece.highlightComponents = filterComponents(piece.components, "highlight");
            console.log(piece);
            $http.post('/api/piece', piece).success(function (data) {
                console.log("saved");
            });
        }
    }
})();