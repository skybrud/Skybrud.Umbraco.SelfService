angular.module("umbraco").directive('skybrudEnter', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.skybrudEnter);
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    fn(scope, { $event: event });
                });
                event.preventDefault();
            }
        });
    };
});