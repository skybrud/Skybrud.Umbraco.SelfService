angular.module("umbraco").controller("SelfServiceListController", function ($scope, $element, $timeout, dialogService) {

    // Read from the configuration
    $scope.orderedList = $scope.model.config.orderedList === '1';
    $scope.maxItems = parseInt($scope.model.config.maxItems); $scope.maxItems = $scope.maxItems ? $scope.maxItems : 10;

    if (!$scope.model.value || typeof($scope.model.value) != 'object') {
        $scope.model.value = {
            items: [
                {
                    value: ''
                }
            ]
        };
    } else if ($scope.model.value.items.length == 0) {
        $scope.model.value.items.push({
            value: ''
        });
    }

    $scope.enter = function(event) {

        // Get the row holding the target input field
        var row = event.target.parentElement.parentElement;

        // Get the next row and it's input field
        var nextRow = row.nextElementSibling;
        var nextInput = nextRow ? nextRow.querySelector('input') : null;

        // If the next row has an input field, we just focus on that
        if (nextInput) {
            nextInput.focus();
            return;
        }

        // If the current input field has a value, we add a new row and focus
        // on it's input field. If the current input field doesn't have a
        // value, we simply do nothing
        if (event.target.value) {
            $scope.add();
        }

    };

    $scope.add = function () {

        // More items are not allowed
        if ($scope.model.value.items.length >= $scope.maxItems) {
            return;
        }

        $scope.model.value.items.push({
            value: ''
        });

        // Focus on the added input field (with a slight delay so Angular has added it to the DOM at this point)
        $timeout(function() {
            var items = $element.context.querySelectorAll('input');
            if (items.length > 0) {
                items[items.length - 1].focus();
            }
        }, 10);

    };

    // Strip empty input fields on submit (if no fields at all, we leave a single empty field)
    $scope.$on('formSubmitting', function (ev, args) {
        var temp = [];
        angular.forEach($scope.model.value.items, function (item) {
            if (item.value) {
                temp.push(item);
            }
        });
        if (temp.length == 0) {
            temp.push({
                value: ''
            });
        }
        $scope.model.value.items = temp;
    });

});