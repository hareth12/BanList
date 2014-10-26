﻿'use strict';
app.factory('authInterceptorService', [
    '$q', '$injector', '$location', 'localStorageService', function($q, $injector, $location, localStorageService) {

        var authInterceptorServiceFactory = {};

        var request = function(config) {

            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        };
        var responseError = function(rejection) {
            if (rejection.status === 401) {
                if ((rejection.data.code) && (rejection.data.code = 100)) {
                    //Case that OTP is not valid but access token is valid
                    return $q.reject(rejection);
                }

                var authService = $injector.get('authService');
                var authData = localStorageService.get('authorizationData');
                if (authData) {
                    if (authData.useRefreshTokens) {
                        $location.path('/refresh');
                        return $q.reject(rejection);
                    }
                }
                authService.logOut();
                $location.path('/login');
            }
            return $q.reject(rejection);
        };
        authInterceptorServiceFactory.request = request;
        authInterceptorServiceFactory.responseError = responseError;

        return authInterceptorServiceFactory;
    }
]);