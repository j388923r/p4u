(function() {
    'use strict';

    angular
        .module('myApp')
        .directive('headerDirective', headerDirective);

    headerDirective.$inject = ['$window'];
    
    function headerDirective ($window) {
        // Usage:
        //     <headerDirective></headerDirective>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA',
            templateUrl: '/Views/header.html'
        };
        return directive;

        function link(scope, element, attrs) {
            console.log("header directive");
        }
    }

})();