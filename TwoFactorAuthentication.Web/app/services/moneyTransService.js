'use strict';
app.factory('moneyTransService', [
    '$http', 'ngAuthSettings', function($http, ngAuthSettings) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var moneyTransServiceFactory = {};

        var getHistory = function() {

            return $http.get(serviceBase + 'api/transactions/history').then(function(results) {
                return results;
            });
        };

        var transferMoney = function(transferData) {

            return $http.post(serviceBase + 'api/transactions/transfer', transferData, { headers: { 'X-OTP': transferData.oneTimePassword } }).then(function(response) {
                return response;
            });

        };

        moneyTransServiceFactory.getHistory = getHistory;
        moneyTransServiceFactory.transferMoney = transferMoney;

        return moneyTransServiceFactory;

    }
]);