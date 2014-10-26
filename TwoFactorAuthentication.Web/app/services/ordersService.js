'use strict';
app.factory('ordersService', [
    '$http', 'ngAuthSettings', function($http, ngAuthSettings) {

        var serviceBase = ngAuthSettings.apiResourceBaseUri;

        var ordersServiceFactory = {};

        var getOrders = function() {

            return $http.get(serviceBase + 'api/orders').then(function(results) {
                return results;
            });
        };

        ordersServiceFactory.getOrders = getOrders;

        return ordersServiceFactory;

    }
]);