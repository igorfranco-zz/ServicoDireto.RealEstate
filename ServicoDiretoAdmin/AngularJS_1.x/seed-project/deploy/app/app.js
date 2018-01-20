'use strict';

/**
 * @ngdoc overview
 * @name app [smartadminApp]
 * @description
 * # app [smartadminApp]
 *
 * Main module of the application.
 */

angular.module('app', [
    'ngSanitize',
    'ngAnimate',
    'ngResource', 
    'restangular',
    'LocalStorageModule',     
    'ui.router',
    'ui.bootstrap',
    'toastr',
    'jcs-autoValidate',

    // Smartadmin Angular Common Module
    'SmartAdmin',

    // App
    //'app.helpers',
    'app.auth',
    'app.layout',
    'app.geo',
    'app.structure',
    'app.hierarchy',
    'app.attribute',
    'app.chat',
    'app.dashboard',
    'app.calendar',
    'app.inbox',
    'app.graphs',
    'app.tables',
    'app.forms',
    'app.ui',
    'app.widgets',
    'app.maps',
    'app.appViews',
    'app.misc',
    'app.smartAdmin',
    'app.eCommerce',
    'app.home'
])
.config(function ($provide, $httpProvider, RestangularProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
    /*
    // Intercept http calls.
    $provide.factory('ErrorHttpInterceptor', function ($q) {
        var errorCounter = 0;
        function notifyError(rejection){
            console.log(rejection);
            $.bigBox({
                title: rejection.status + ' ' + rejection.statusText,
                content: rejection.data,
                color: "#C46A69",
                icon: "fa fa-warning shake animated",
                number: ++errorCounter,
                timeout: 6000
            });
        }

        return {
            // On request failure
            requestError: function (rejection) {
                // show notification
                notifyError(rejection);

                // Return the promise rejection.
                return $q.reject(rejection);
            },

            // On response failure
            responseError: function (rejection) {
                // show notification
                notifyError(rejection);
                // Return the promise rejection.
                return $q.reject(rejection);
            }
        };
    });
    */

    // Add the interceptor to the $httpProvider.
    //$httpProvider.interceptors.push('ErrorHttpInterceptor');

    RestangularProvider.setBaseUrl(location.pathname.replace(/[^\/]+?$/, ''));

})
.constant('APP_CONFIG', window.appConfig)

/*
.run(function ($rootScope, $state, $stateParams) 
{
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    // editableOptions.theme = 'bs3';
});
*/
.run(['bootstrap3ElementModifier', 'defaultErrorMessageResolver', '$rootScope', 'toastr', 'AccountService', '$state', 
        function (bootstrap3ElementModifier, defaultErrorMessageResolver, $rootScope, toastr, AccountService, $state) {
            //$rootScope.bodyClass = "page-homepage map-google";
            //$rootScope.bodyClass = "page-homepage navigation-fixed-top page-slider horizontal-search"
            $rootScope.bodyClass = "page-sub-page page-submission-success";
            //carrega a variavel inicial
            AccountService.fillAuthData();
            // To change the root resource file path
            //bootstrap3ElementModifier.enableValidationStateIcons(true);
            defaultErrorMessageResolver.setI18nFileRootPath('/bower_components/angular-auto-validate/dist/lang');
            defaultErrorMessageResolver.setCulture('pt-BR');
            defaultErrorMessageResolver.getErrorMessages().then(function (errorMessages) {
                errorMessages['matchPassword'] = 'As senhas não conferem.';
                errorMessages['maxHeight'] = 'O tamanho da imagem excedeu o permitido';
                errorMessages['maxHeight'] = 'Valor informado inválido!';
            });

            //Tratamento e exibição de mensagem de erro que acontecam
            $rootScope.$watch('error', function(newValue)
            {
                console.log($rootScope.error);
                if($rootScope.error && $rootScope.error.data)
                {
                    var errors = [];
                    console.log($rootScope.error);                
                    if($rootScope.error.data.modelState)
                    {
                        errors.push('<ul>')
                        angular.forEach($rootScope.error.data.modelState, function(value, key){
                            errors.push( '<li>' + key + ': ' + value + '</li>');
                        });
                        errors.push('</ul>')
                    }

                    if( $rootScope.error.data.exceptionMessage)
                        errors.push($rootScope.error.data.exceptionMessage);

                    toastr.error($rootScope.error.data.message + errors.join(' '), $rootScope.error.statusText, { allowHtml: true, closeButton: true });                            
                }
            });        

            //tratando autenticacao
            $rootScope.$on("$stateChangeStart", function(event, toState, toParams, fromState, fromParams)
            {
                console.log('$rootScope.authData: ' + $rootScope.authData);
                //set sempre com default
                //$rootScope.bodyClass = "page-homepage navigation-fixed-top page-slider";
                $rootScope.bodyClass = "page-sub-page page-submission-success"
                if($rootScope.authData == undefined || $rootScope.authData == null || $rootScope.authData.isAuthenticated == false){
                    //Forcando pela interface que esteja autenticado, bem como server-side
                    if(toState.name != 'login'){
                        $state.go('login');
                        event.preventDefault();
                    }
                }
            });    

            angular.forEach([ '$stateChangeSuccess', '$stateChangeError'], function(event) {
              $rootScope.$on(event, function(event, toState, toParams, fromState, fromParams, error) {
                //console.log('$stateChangeSuccess');
                //initializeOwl(false);
              });
            });            
        }
    ])    
    .factory('authInterceptorService', ['$q', '$injector', 'localStorageService', 
        function ($q, $injector, localStorageService) 
        {
                var authInterceptorServiceFactory = {};
                var _request = function (config) {
                    config.headers = config.headers || {};                   
                    var authData = localStorageService.get('authorizationData');
                    if (authData) {
                        config.headers.Authorization = 'Bearer ' + authData.bearerToken;
                    }

                    return config;
                }

                var _responseError = function (rejection) {                    
                    if (rejection.status === 401) 
                    {
                        var authService = $injector.get('AccountService');
                        var authData = localStorageService.get('authorizationData');
                        var state = $injector.get('$state');

                        /*
                        if (authData) {
                            if (authData.useRefreshTokens) {
                                $location.path('/refresh');
                                return $q.reject(rejection);
                            }
                        }
                        */
                        authService.logOut();
                        state.go('login', {}, {reload: true});

                        //$location.path('sign-in');
                    }
                    return $q.reject(rejection);
                }

                authInterceptorServiceFactory.request = _request;
                authInterceptorServiceFactory.responseError = _responseError;

                return authInterceptorServiceFactory;        
        }]);
        

