angular.module("umbraco").controller("SelfServiceCategoriesDialogController", function ($scope, $http) {

    // Skip if we doesn't have an array
    if (!Array.isArray($scope.dialogOptions.selected)) return;

    // Update the categories already selected
    $scope.bacon = [];
    angular.forEach($scope.dialogOptions.selected, function (cat) {
        $scope.bacon.push(cat);
    });

    // Array representing the category tree
    $scope.categoriesTree = [];

    // Gets whether the specified category is selected
    function isSelected(cat) {
        for (var i = 0; i < $scope.bacon.length; i++) {
            if (cat.id == $scope.bacon[i].id) return true;
        }
        return false;
    }

    // Updates properties of the category tree with their initial values
    function update(categories) {

        angular.forEach(categories, function(cat) {
            cat.checked = isSelected(cat);
            cat.expanded = false;
            angular.forEach(cat.children, function(child) {
                child.$parent = cat;
            });
            update(cat.children);
        });

        angular.forEach(categories, function (cat) {
            bubble(cat);
        });

    }

    // Gets whether the category has any selected children 
    function hasSelectedChildren(cat) {
        for (var i = 0; i < cat.children.length; i++) {
            var checked = (cat.children[i].checked || hasSelectedChildren(cat.children[i]));
            if (checked) return true;
        }
        return false;
    }

    // Updates up the tree whether the category has any selected children
    function bubble(cat) {
        cat.checked2 = hasSelectedChildren(cat);
        if (cat.$parent) bubble(cat.$parent);
    }

    // Make a call to the API to get the category tree
    $http.get('/umbraco/api/SelfService/GetCategoriesTree').success(function(body) {

        // Set the categories in the scope
        $scope.categoriesTree = body;
        
        // Update internal properties
        update($scope.categoriesTree);

    });

    // Toggles whether the specified category should be selected
    $scope.toggle = function (category) {

        // Find the index of the category (if already selected)
        var index = -1;
        for (var i = 0; i < $scope.bacon.length; i++) {
            if (category.id == $scope.bacon[i].id) {
                index = i;
                break;
            }
        }

        // Update whether the category was selected or deselected
        if (index >= 0) {
            $scope.bacon.splice(index, 1);
            category.checked = false;
        } else {
            $scope.bacon.push({
                id: category.id,
                name: category.name,
                icon: category.icon
            });
            category.checked = true;
        }

        // Bubble the event up the tree
        bubble(category);

    };

    // Submits the selection
    $scope.select = function () {
        $scope.submit($scope.bacon);
    };

});