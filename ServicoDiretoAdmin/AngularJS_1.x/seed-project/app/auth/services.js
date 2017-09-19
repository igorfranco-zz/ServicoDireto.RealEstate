"use strict";


angular
    .module('app.auth')
    .factory('AccountService', ['$http', '$q', '$resource', '$rootScope', 'APP_CONFIG', 'localStorageService',
        function ($http, $q, $resource, $rootScope, APP_CONFIG, localStorageService) 
        {
            'use strict';

            var authData = {
              isAuthenticated: false,
              userName: '',
              bearerToken: '',
              expirationDate: null
            };

            var _clearAuthData = function() 
            {
              console.log('clearAuthData called');
              authData.isAuthenticated = false;
              authData.userName = '';
              authData.bearerToken = '';
              authData.expirationDate = null;

              delete $rootScope.authData;
            };            

            var _logOut = function()
            {
                _clearAuthData();
                localStorageService.remove('authorizationData');
            };

            return {
                CreateAccount: $resource(APP_CONFIG.apiUrl + "/apiAccount/CreateAccount", {}, {
                    save: { method: 'POST', params: {} }
                }),
                Recover: $resource(APP_CONFIG.apiUrl + "/apiAccount/Recover", {}, {
                    execute: { method: 'POST', params: {} }
                }),                
                ChangePassword: $resource(APP_CONFIG.apiUrl + "/apiAccount/ChangePassword", {}, {
                    execute: { method: 'POST', params: {} }
                }),  
                Login: function(account){
                    var data = "grant_type=password&username=" + account.userName + "&password=" + account.password;
                    var deferred = $q.defer();
                    $http.post(APP_CONFIG.apiUrl  + '/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' }})
                    .error(function (err, status) {
                        _logOut();                        
                        deferred.reject(err);
                    })
                    .success(function (response) 
                    {
                        console.log(response);
                        //salvando estrutura de auth
                        authData.isAuthenticated = true;
                        authData.userName =  account.userName;
                        authData.bearerToken = response.access_token;
                        authData.expirationDate = response.expires_in;                        
                        localStorageService.set('authorizationData', authData); 
                        $rootScope.authData = authData;
                        deferred.resolve(response);
                    });

                    return deferred.promise;
                },        
                clearAuthData : _clearAuthData,
                logOut : _logOut,
                fillAuthData : function () {
                    console.log('fillAuthData called');
                    var authDataResponse = localStorageService.get('authorizationData');
                    if (authDataResponse)
                    {
                        authData = authDataResponse;
                        $rootScope.authData = authData;
                        $rootScope.$apply();
                        console.log('fillAuthData loaded');
                    }
                },
                ActivateAccount: $resource(APP_CONFIG.apiUrl + "/apiAccount/ActivateAccount", {}, {
                    execute: { method: 'GET', params: {} }
                })                                    
            };
        }]);
 