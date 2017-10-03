var app = angular.module("FlatsApp", ['angularLazyImg']);
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
            if (response.data.length === 0)
                $scope.noFlatsText = true;
            $scope.data = response.data;
        },
        function errorCallback(response) {
            alert("Error!");
        });
});

app.filter('rubleFormat', function () {
    return function (input) {
        input = String(input);
        var ruble = "руб.";
        var parts = [];
        var partsCount = Math.ceil(input.length / 3);
        for (; partsCount > 0; partsCount--) {
            var part = input.substring(
                input.length - 3 * partsCount,
                input.length - 3 * (partsCount - 1)
            );
            parts.push(part);
        }
        return parts.join(' ') + " " + ruble;
    }
});