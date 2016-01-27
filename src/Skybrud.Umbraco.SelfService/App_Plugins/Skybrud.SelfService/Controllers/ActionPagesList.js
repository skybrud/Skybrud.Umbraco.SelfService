angular.module("umbraco").controller("SelfServiceActionPagesListController", function ($scope, $http, $timeout, dialogService) {

    // Initial model
    $scope.results = [];
    $scope.categories = [];
    $scope.loading = true;
    $scope.text = '';

    // Initial sorting options
    var sorting = {
        field: 'name',
        order: 'asc'
    };

    // Initial pagination options
    $scope.pagination = {
        page: 1,
        pages: 0,
        limit: 20,
        offset: 0,
        pagination: []
    };

    // Uodate the sorting to match "field" and "order"
    $scope.sort = function (field, order) {

        // Update the sorting options
        if (order != 'desc') order = 'asc';
        if (field == sorting.field) {
            sorting.order = (sorting.order == 'asc' ? 'desc' : 'asc');
        } else {
            sorting.field = field;
            sorting.order = order;
        }

        // Update the list with the new sorting options
        $scope.updateList();

    };

    // Check the sorting direction (used in the view)
    $scope.isSortDirection = function (field, order) {
        return field == sorting.field && order == sorting.order;
    };

    // Loads the previous page
    $scope.prev = function () {
        if ($scope.pagination.page > 1) $scope.updateList($scope.pagination.page - 1);
    };

    // Loads the next pages
    $scope.next = function () {
        if ($scope.pagination.page < $scope.pagination.pages) $scope.updateList($scope.pagination.page + 1);
    };

    // Updates the list based on current arguments
    $scope.updateList = function (page) {

        // If a page is specified, we load that page
        page = (page ? page : $scope.pagination.page);

        // Set the loading flag
        $scope.loading = true;

        // Declare the arguments (makeing up the query string) for the call to the API
        var args = {
            limit: $scope.pagination.limit,
            page: page,
            sort: sorting.field,
            order: sorting.order
        };

        // Append the search text (if specified)
        if ($scope.text) {
            args.text = $scope.text;
        }

        // Append the IDs of the selected category (if any)
        if ($scope.categories.length > 0) {
            args.categories = $scope.categories.map(function (cat) { return cat.id; }).join(',');
        }

        // Declare the HTTP options
        var req = {
            method: 'GET',
            url: '/umbraco/api/SelfService/GetActionPages',
            params: args
        };

        // Make the call to the API
        $http(req).success(function (body) {

            // Update our model
            $scope.loading = false;
            $scope.results = body.data;
            sorting = body.sorting;

            // Update our pagination model
            $scope.pagination = body.pagination;
            $scope.pagination.pagination = [];
            for (var i = 0; i < $scope.pagination.pages; i++) {
                $scope.pagination.pagination.push({
                    val: (i + 1),
                    isActive: $scope.pagination.page == (i + 1)
                });
            }

        });

    };

    // Opens up a dialog for selecting the categories
    $scope.selectCategories = function() {
        dialogService.open({
            template: '/App_Plugins/Skybrud.SelfService/Views/CategoriesDialog.html',
            show: true,
            selected: $scope.categories,
            callback: function (selected) {
                $scope.categories = selected;
                $scope.updateList();
            }
        });
    };

    // NAY
    var initialized = false;

    // Listen for changes of the input field of the search text
    var wait = null;
    $scope.$watch('text', function () {
        if (!initialized) return;
        if (wait) $timeout.cancel(wait);
        wait = $timeout(function () {
            $scope.updateList();
        }, 300);
    }, true);

    // Update the list
    $scope.updateList();

    // YAY
    initialized = true;

});