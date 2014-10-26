
var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

app.config(function($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/transhistory", {
        controller: "moneyTransController",
        templateUrl: "/app/views/transhistory.html"
    });

    $routeProvider.when("/transfermoney", {
        controller: "transferMoneyController",
        templateUrl: "/app/views/transferMoney.html"
    });
    $routeProvider.when("/orders", {
        controller: "ordersController",
        templateUrl: "/app/views/orders.html"
    });

    $routeProvider.when("/refresh", {
        controller: "refreshController",
        templateUrl: "/app/views/refresh.html"
    });

    $routeProvider.when("/tokens", {
        controller: "tokensManagerController",
        templateUrl: "/app/views/tokens.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/app/views/associate.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

var authBase = 'http://localhost:55435/';
var resourceBase = 'http://localhost:47039/';
app.constant('ngAuthSettings', {
    apiAuthBaseUri: authBase,
    apiResourceBaseUri: resourceBase,
    clientId: 'ngAuthApp'
});

app.config(function($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run([
    'authService', function(authService) {
        authService.fillAuthData();
    }
]);