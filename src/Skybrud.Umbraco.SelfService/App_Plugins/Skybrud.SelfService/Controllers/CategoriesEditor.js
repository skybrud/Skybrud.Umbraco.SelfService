angular.module("umbraco").controller("SelfServiceCategoriesController", function ($scope, $http, $routeParams, notificationsService, dialogService) {

    $scope.startNode = 0;
    $scope.ids = [];
    $scope.categories = [];

    if ($scope.model.value && typeof($scope.model.value) == 'string') {
        $scope.ids = $scope.model.value.split(' ');
    }

    $scope.addCategories = function () {
        dialogService.open({
            template: '/App_Plugins/Skybrud.SelfService/Views/CategoriesDialog.html',
            show: true,
            selected: $scope.categories,
            callback: function (selected) {
                $scope.categories = selected;
                updateIds();
            }
        });
    };

    function updateIds() {
        $scope.ids = [];
        angular.forEach($scope.categories, function (item) {
            $scope.ids.push(item.id + '');
        });
        $scope.model.value = $scope.ids.join(' ');
    }

    $scope.removeCategory = function (index) {
        $scope.categories.splice(index, 1);
        updateIds();
    };

    $http.get('/umbraco/backoffice/api/SelfServiceAdmin/GetCategoriesContext?ids=' + $scope.ids.join(',')).success(function (body) {
        $scope.startNode = body.startNodeId;
        $scope.categories = body.selected;
        updateIds();
    });


});