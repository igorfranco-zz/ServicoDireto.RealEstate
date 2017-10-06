// Define all your modules with no dependencies
angular.module('ElementApp', []);
angular.module('CultureApp', []);
angular.module('CommonApp', []);
angular.module('LocationApp', []);
angular.module('StructureApp', []);
angular.module('CustomerApp', []);
angular.module('AccountApp', []);
angular.module('ComponentApp', []);

// Lastly, define your "main" module and inject all other modules as dependencies
var mainApp = angular
    .module('MainApp', [
     //'angular-advanced-searchbox',
     'ui.router',
     'ngFileUpload',
     'LocalStorageModule',     
     'zonerMapModule',
     'jcs-autoValidate',
     //'ngMask',
     'angular-loading-bar',
     'ui.bootstrap',
     'ngAnimate', 
     'toastr',
     'rzModule', 
     'ngRoute',
     'ngResource', 
     'ngLocale', 
     //'angular-owl-carousel', 
     'zonerPaginationModule', 
     'ElementApp', 
     'CultureApp',
     'CommonApp',
     'ComponentApp',
     'AccountApp',
     'LocationApp', 
     'StructureApp', 
     'CustomerApp'])
    .constant( "config", 
    {
        skype: "igorfrancobrum",
        companyName : "Serviço Direto",
        appName :"Serviço Direto | Imóveis | Apartamentos, Casas e Imóveis à Venda e para Alugar",
        appVersion : 1.0,
        apiUrl: 'http://192.168.1.101/servicodireto/api',
        phone: "+55 (51) 994643433",
        address1:"Rua Saara, 10 apto 102",
        address2: "Porto Alegre, RS Brasil",
        email:"contato@servicodireto.com.br",
        adminSite : "http://192.168.1.101/servicodireto/",
        layoutSite : "http://localhost:8989",
        carouselInterval : 5000
    })
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


            //configurações basicas dos componentes de filtro
            $rootScope.options = {
                                         minArea:0, 
                                         maxArea:10000,                         
                                         minPrice:0, 
                                         maxPrice:2000000, 
                                         priceStep:200, 
                                         minRadius:0, 
                                         maxRadius:20,
                                         showRadius:true,
                                         minRooms:1, 
                                         maxRooms:10,
                                         minGarage:1,
                                         maxGarage:4
                                    };



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
                //set sempre com default
                //$rootScope.bodyClass = "page-homepage navigation-fixed-top page-slider";
                $rootScope.bodyClass = "page-sub-page page-submission-success"
                if($rootScope.authData == null || $rootScope.authData.isAuthenticated == false){
                    //Forcando pela interface que esteja autenticado, bem como server-side
                    if(toState.name == 'submit'){
                        $state.go('sign-in', { redirectTo : 'submit'});
                        event.preventDefault();
                    }
                }
            });    

            angular.forEach([ '$stateChangeSuccess', '$stateChangeError'], function(event) {
              $rootScope.$on(event, function(event, toState, toParams, fromState, fromParams, error) {
                //console.log('$stateChangeSuccess');
                initializeOwl(false);
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
                        state.go('sign-in', {}, {reload: true});

                        //$location.path('sign-in');
                    }
                    return $q.reject(rejection);
                }

                authInterceptorServiceFactory.request = _request;
                authInterceptorServiceFactory.responseError = _responseError;

                return authInterceptorServiceFactory;        
            }]);