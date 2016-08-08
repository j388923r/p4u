(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('customController', customController);

    customController.$inject = ['$scope', '$http', '$interval']; 

    function customController($scope, $http, $interval) {

        $scope.start = function() {
            $http.post("/api/directline/", {}).success(function(conversation) {
                $scope.conversationId = conversation.conversationId;
            });
        };

        $scope.sendMessage = function (text) {
            $scope.messages.push({
                text: $scope.messageText,
                from: $scope.sessiomUserId
            });
            $scope.messageText = "";
            $http.post("/api/directline/" + $scope.conversationId + "?watermark=" + $scope.watermark, {
                text: text,
                created: Date.now(),
                eTag: "*",
                from: $scope.sessiomUserId
            }).success(function (conversation) {
                $scope.conversation = conversation;
                $http.get("/api/directline/" + $scope.conversationId).success(function (messages) {
                    console.log(messages);
                    $scope.watermark = messages.watermark;
                    for (var key in messages) {
                        if (messages[key].from !== $scope.sessiomUserId) {
                            $scope.messages.push({
                                text: messages[key].text,
                                from: messages[key].from
                            });
                        }
                    }
                });
            });
        };

        activate();

        function activate()
        {
            $scope.messages = [];
            $scope.sessiomUserId = "thatgirl";
            $scope.watermark = 0;
            $scope.start();
        }
    }
})();
