var app = angular.module("FlatsApp", []);
app.controller("FlatsController", function ($scope, $http) {
    $scope.orderingItems = [
        { name: "price", label: "Цена" },
        { name: "metro", label: "Метро" },
        { name: "address", label: "Адрес" }
    ];
    $scope.loadingText = true;
    $http.get('/Home/GetAllFlats')
        .then(
        function successCallback(response) {
            $scope.loadingText = false;
            if (response.data.length == 0)
                $scope.noFlatsText = true;
            $scope.data = response.data;
        },
        function errorCallback(response) {
            alert("Error!");
        });
});