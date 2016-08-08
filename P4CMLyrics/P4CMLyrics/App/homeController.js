(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('homeController', homeController);

    homeController.$inject = ['$scope', '$uibModal', '$sce', 'pieceService', 'userService']; 

    function homeController($scope, $uibModal, $sce, pieceService, userService) {
        $scope.title = 'homeController';

        activate();

        $scope.user = userService.getUser();
        console.log($scope.user);

        pieceService.getSamplePiece().success(function (piece) {
            console.log(piece);
            $scope.featuredpiece = piece;
            $scope.link = "http://www.youtube.com/watch?" + piece.p4cmlink;
            $scope.artist = piece.artist;
            $scope.user = { name: piece.owner };
            $scope.featuredpiece.components = pieceService.formatComponents(pieceService.mergeComponents(piece).components);
            $scope.featuredpiece.components.forEach(function (component) {
                if (component.type === "highlight") {
                    component.annotationHTML = $sce.trustAsHtml(component.annotationHTML.replace(/\n/g, "</br>"));
                }
            });
            console.log(piece);
        });

        /*function mergeComponents(piece) {
            piece.components = [];
            for (var i = 0; i < piece.plainComponents.length; i++) {
                piece.components[piece.plainComponents[i].index] = piece.plainComponents[i];
            }

            for (var i = 0; i < piece.highlightComponents.length; i++) {
                piece.components[piece.highlightComponents[i].index] = piece.highlightComponents[i];
            }
            return piece;
        }*/

        $scope.selectComponent = function (index) {
            if ($scope.featuredpiece.components[index] === $scope.selectedComponent) {
                $scope.selectedComponent = undefined;
            }
            else if ($scope.featuredpiece.components[index].type == 'highlight') {
                $scope.selectedComponent = $scope.featuredpiece.components[index];
            } else {
                $scope.selectedComponent = undefined;
            }
        }

        $scope.openModal = function (type) {
            if(type === 'login') {
                var modalInstance = $uibModal.open({
                    templateUrl: '/Views/loginModal.html',
                    controller: 'loginModalController',
                    resolve: {
                        userService: function () {
                            return userService
                        },
                        passedIn: function () {
                            return [];
                        }
                    }
                });

                modalInstance.result.then(function (user) {
                    $scope.user = userService.getUser();
                    console.log($scope.user);
                }, function () {
                    console.log('error', $scope);
                });
            } else if (type === 'signup') {
                var modalInstance = $uibModal.open({
                    templateUrl: '/Views/signupModal.html',
                    controller: 'signupModalController',
                    resolve: {
                        passedIn: function () {
                            return ['a'];
                        }
                    }
                });

                modalInstance.result.then(function (user) {
                    $scope.user = userService.getUser();
                    console.log($scope.user);
                }, function () {
                    console.log('error', $scope);
                });
            }
        };

        function activate() { }
    }
    
    angular.module('myApp').controller('loginModalController', function ($scope, $uibModalInstance, userService, passedIn) {

        $scope.ok = function () {
            userService.login($scope.username, $scope.password, $uibModalInstance);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });

    angular.module('myApp').controller('signupModalController', function ($scope, $uibModalInstance, userService, passedIn) {

        console.log(userService);

        $scope.ok = function () {
            if ($scope.password === $scope.confpassword) {
                userService.signup($scope.username, $scope.password, $scope.name);
                $uibModalInstance.close({
                    username: $scope.username
                });
            }
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });
})();
