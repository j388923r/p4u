(function() {
    'use strict';

    angular
        .module('p4u')
        .directive('jcEnter', jcEnter);

    jcEnter.$inject = ['$window'];
    
    function jcEnter($window) {
        console.log('directive');
        // Usage:
        //     <jc-enter></jc-enter>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA'
        };
        return directive;

        function link(scope, element, attrs) {
            console.log('link');
            element.bind("keypress", function (event) {
                if (event.keyCode == 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.jcEnter);
                    });

                    event.preventDefault();
                }
            });
        }
    }

})();