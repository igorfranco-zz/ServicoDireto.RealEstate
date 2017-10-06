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
angular
    .module('MainApp')
		.config(['$httpProvider', function ($httpProvider) {
		        $httpProvider.interceptors.push('authInterceptorService');
		        //$httpProvider.interceptors.push("cacheBusterFactory");
		    }])
	    .config(function(toastrConfig) {
		      angular.extend(toastrConfig, {
		        autoDismiss: false,
		        containerId: 'toast-container',
		        maxOpened: 0,    
		        newestOnTop: true,
		        positionClass: 'toast-top-right',
		        preventDuplicates: false,
		        preventOpenDuplicates: false,
		        target: 'body'
		      });
	    })
		.config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
		    cfpLoadingBarProvider.includeSpinner = false;
		  }]);	        

angular.module('MainApp')
  .config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {

      // For any unmatched url, redirect to /state1
      $urlRouterProvider.otherwise("/");

      $stateProvider
          .state("/", {
            url: "/", 
            templateUrl: 'modules/element/views/initial.html',
            controller: 'ElementController'          
          })
          .state('profile', {
             url:'/profile',
          	 templateUrl: 'modules/customer/views/profile.html',
             controller: 'ProfileCustomerController'          
          })
          .state('agent-detail', {
             url: '/agent-detail/:idCustomer',
             templateUrl: 'modules/customer/views/agent-detail.html',
             controller: 'AgentDetailCustomerController'          
          })          
          .state('sign-in', {
            controller: 'LoginAccountController',          
            templateUrl: 'modules/account/views/sign-in.html',
            params: {
              redirectTo : ''
            }
          })            
          .state('log-out', {
            url: "/log-out", 
            controller: 'LogoutAccountController',          
          })       
          .state('/activate-account', {
            url: "/activate-account/:code",
            controller: 'ActivateAccountController',          
          })                 
          .state('recovery', {
            controller: 'RecoveryAccountController',          
            templateUrl: 'modules/account/views/recovery.html',
            params: { 
              recoveryType: 0
            }
          }) 
         .state('change-password', {
            url: "/change-password/:code", 
            controller: 'ChangePasswordAccountController',          
            templateUrl: 'modules/account/views/change-password.html',
            params: { 
              code: ''
            }
          })           
          .state('status', {
          templateUrl: 'modules/common/views/status.html',
          controller: 'StatusCommonController',
          params: { 
              message: 
              {
                statusType : 0,
                header: "",
                description : ""
              }
            }
          })    
          .state('create-account', {
            templateUrl: 'modules/account/views/create-account.html',
            controller: 'CreateAccountController'
          })
          .state('terms-conditions', {
            url: "/terms-conditions", 
            templateUrl: 'modules/common/views/terms-conditions.html',
            controller: 'TermsConditionsController'
          })          
          .state('about-us', {
            url: "/about-us", 
            templateUrl: 'modules/common/views/about-us.html',
            controller: 'AboutUsController'
          })                    
          .state('contact', {
            templateUrl: 'modules/common/views/contact.html',
            controller: 'ContactController'
          })                    
          .state('property-detail', {
            url: '/property-detail?idElement',
            templateUrl: 'modules/element/views/property-detail.html',
            controller: 'ElementDetailController'          
          })                         
          .state('submit', { //?Optional
            url: '/submit/:idElement',
            templateUrl: 'modules/element/views/submit.html',
            controller: 'ElementSubmitController'          
          })
          .state('properties-listing-lines', {
            templateUrl: 'modules/element/views/properties-listing-lines.html',
            controller: 'ElementFilterController'
          })
          .state('properties-map', {
            templateUrl: 'modules/element/views/properties-map.html',
            controller: 'ElementFilterController'
          })
          .state('bookmarked', {
            url:"/bookmarked",
            templateUrl: 'modules/customer/views/bookmarked.html',
            controller: 'BookmarkedCustomerController',          
          })              
          .state('my-properties', {
            url:"/my-properties",
            templateUrl: 'modules/customer/views/my-properties.html',
            controller: 'MyPropertiesCustomerController',          
          })                              
    }]);    
angular.module('MainApp')
    .controller('MainController', ['$scope', 'config', function ($scope, config) 
    {
    	$scope.config = config;	
    }]);
//http://stackoverflow.com/questions/12371159/how-to-get-evaluated-attributes-inside-a-custom-directive
//http://stackoverflow.com/questions/25212859/using-angular-directive-to-watch-for-change-values-with-bootstrap-select

angular.module("MainApp")
    .directive("bootselectpicker", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
            scope.$watchCollection('array', function(newVal) {
              el.selectpicker('refresh');
            });
          },          
          restrict: "A",
          replace : true,
          scope: 
          {
            array: '=', //usar o mesmo do contexto
            class: '=' 
          }          
        };
    }])
    .directive("zCheckbox", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
              el.iCheck();
              console.log('zCheckbox called');

              scope.$watchCollection('array', function(newVal) {
                el.iCheck('refresh');
                console.log('zCheckbox called update');
              });              
          },          
          restrict: "A",
          replace : true,
        };
    }])
    .directive('icheck', ['$timeout', '$parse', function($timeout, $parse) {
      return {
              require: 'ngModel',
              link: function($scope, element, $attrs, ngModel) {
                  return $timeout(function() {
                      var value;
                      value = $attrs['value'];

                      $scope.$watch($attrs['ngModel'], function(newValue){
                          $(element).iCheck('update');
                      })

                      return $(element).iCheck(
                      {
                          //checkboxClass: 'icheckbox_flat-aero',
                          //radioClass: 'iradio_flat-aero'
                      }).on('ifChanged', function(event) {
                          if ($(element).attr('type') === 'checkbox' && $attrs['ngModel']) {
                              $scope.$apply(function() {
                                  return ngModel.$setViewValue(event.target.checked);
                              });
                          }
                          if ($(element).attr('type') === 'radio' && $attrs['ngModel']) {
                              return $scope.$apply(function() {
                                  return ngModel.$setViewValue(value);
                              });
                          }
                      });
                  });
              }
          };      
    }])    
    .directive("zFileInput", ["$timeout", "config", function ($timeout, config) {
        return {
          link: function(scope, el, attrs, ngModel){
            scope.$watch('element', function(newValue){
              console.log('zFileInput called');
              //if(scope.element != null) //presenca do atributo
              {                  
                  var initialPreviewImages = [];                
                  var btnCust = '<button type="button" class="btn btn-default" title="Imagem principal" ' + 
                      'onclick="alert(\'Call your custom code here.\')">' +
                      '<i class="glyphicon glyphicon-tag"></i>' +
                      '</button>'; 

                  if(scope.element != null && scope.element.Images != null) 
                  {   
                    for (var i = 0; i < scope.element.Images.length; i++) 
                    {
                      var image = scope.element.Images[i];
                      initialPreviewImages.push("<img alt='' title='' class='file-preview-image' src='" + config.adminSite + '/uploads/' + scope.element.IDCustomer + '/' + scope.element.IDElement + '/thumb/' + image + "' >");
                    };
                  }

                  el.fileinput({
                      initialPreview:  initialPreviewImages,
                      language: "pt-BR",
                      overwriteInitial: true,
                      uploadUrl: "/____LINK_UPLOAD/2",
                      allowedFileExtensions: ["jpg", "png", "gif"],
                      browseClass: "btn btn-default",
                      browseIcon: "<i class=\"glyphicon glyphicon-picture\"></i> ",
                      removeClass: "btn btn-danger",
                      removeIcon: "<i class=\"glyphicon glyphicon-trash\"></i> ",
                      removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
                      showUpload : false,
                      maxImageWidth: 200,
                      maxFileCount: 5,
                      resizeImage: true,
                      maxFileSize: 500,
                      layoutTemplates: 
                      {  
                        actions: btnCust + ' {delete}'//,
                      }});                  
                }
            });
          },          
          restrict: "A",
          require: 'ngModel',
          replace : true,
          scope: 
          {
            element: '=ngModel'
          }            
        };
    }])
    .directive("zFileInputAvatar", ["$timeout", "config", function ($timeout, config) {
        return {
          link: function(scope, el, attrs) {
              scope.$watch('logo', function(newValue){
                console.log('zFileInputAvatar called');
                  var initialPreviewImages = [];
                  if(newValue != null && newValue != undefined)  
                  {
                    initialPreviewImages.push('<img src="' + config.adminSite + newValue + '" alt="Foto">') ;
                    el.fileinput('refresh', {initialPreview:  initialPreviewImages});
                  }
                  else
                  {
                    el.fileinput({
                        maxFileCount: 1,
                        overwriteInitial: true,
                        maxFileSize: 1500,
                        showClose: false,
                        showCaption: false,
                        showUpload : false,
                        language: "pt-BR",
                        browseLabel: '',
                        removeLabel: '',
                        browseIcon: '<i class="glyphicon glyphicon-folder-open"></i>',
                        removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
                        removeTitle: 'Cancel or reset changes',
                        elErrorContainer: '#kv-avatar-errors',
                        msgErrorClass: 'alert alert-block alert-danger',
                        defaultPreviewContent: '<img src="assets/img/agent-01.jpg" alt="Foto" >',
                        allowedFileExtensions: ["jpg", "png", "gif"] });
                  }
              })
          },          
          restrict: "A",
          replace : true,
          scope: 
          {
            logo: '=ngModel'
          }                      
        };
    }])
    .directive("zTooltip", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
            console.log('zTooltip called');
            el.tooltip();
          },          
          restrict: "A",
          replace : true,
        };
    }])  
    .directive("meioMask", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
            console.log('meioMask called');
            el.setMask(attrs['mask']);
          },          
          restrict: "A",
          replace : true,
        };
    }]);      

angular.module("ComponentApp")
    .directive("zonerFilter", ['$filter', function ($filter) {
        return {
          link: function($scope, el, attrs) 
          {
            $scope.radiusSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minRadius,
                      ceil: $scope.options.maxRadius,
                      translate: function(value) {
                        return "Raio: " + value + " Km"
                      }
                    }
                  };                  
            //      
            $scope.priceSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minPrice,
                      ceil: $scope.options.maxPrice,
                      step: $scope.options.priceStep,
                      translate: function(value) 
                      {
                        if(value == $scope.options.maxPrice)
                          return $filter("currency")(value) + ' ou mais';
                        else
                          return $filter("currency")(value);
                      }
                    }
                  }; 
            //      
            $scope.roomSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minRooms,
                      ceil: $scope.options.maxRooms,
                      translate: function(value) 
                      {
                        if(value == $scope.options.maxRooms)
                          return value + " ou mais";
                        else
                          return value + " Quartos(s)";
                      }
                    }
                  }; 
            //                   
            $scope.garageSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minGarage,
                      ceil: $scope.options.maxGarage,
                      translate: function(value) {
                        if(value == $scope.options.maxGarage)
                          return value + " ou mais"
                        else
                          return value + " Garagem(ns)"

                      }
                    }
                  };             
            //
            $scope.areaSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minArea,
                      ceil: $scope.options.maxArea,
                      translate: function(value) {
                        if(value == $scope.options.maxArea)
                          return $filter("number")(value) + " m² ou mais"
                        else
                          return "Área " + $filter("currency")(value) + " m²"

                      }
                    }
                  };             
            //        
            $scope.filter.radius   = $scope.options.minRadius;   

            $scope.filter.minRooms = $scope.options.minRooms;
            $scope.filter.maxRooms = $scope.options.maxRooms;

            $scope.filter.minPrice = $scope.options.minPrice;
            $scope.filter.maxPrice = $scope.options.maxPrice;

            $scope.filter.minGarage = $scope.options.minGarage;
            $scope.filter.maxGarage = $scope.options.maxGarage;

            $scope.filter.minArea = $scope.options.minArea;
            $scope.filter.maxArea = $scope.options.maxArea;

            $scope.filter.defaultvalues = $scope.options;
          },          
          controller: ['$scope', '$rootScope', function ($scope, $rootScope){

              $scope.$watch('filter', function(oldValue, newValue){
                if(oldValue.pageIndex != newValue.pageIndex || oldValue.orderBy != newValue.orderBy ) 
                {
                  //$rootScope.$broadcast("filterRequested", $scope.filter ); 
                  $rootScope.$broadcast("filterRequested"); 
                }
              }, true);

              
              $scope.doFilter = function()
              {
                  console.log("filterRequested")
                  $rootScope.$broadcast("filterRequested");
                /*
                  $scope.filter.pageIndex = 0;
                  $scope.filter.FilterAttribute = [];
                  $scope.filter.IDPurpose = [];     
                  if($scope.filter.singleIDPurpose != null)
                    $scope.filter.IDPurpose.push($scope.filter.singleIDPurpose);      

                  $scope.filter.FilterAttribute.push
                  ( 
                    {
                      IDAttribute : 5, 
                      InitialValue : $scope.filter.minPrice,
                      FinalValue : $scope.filter.maxPrice 
                    }
                  );

                  $rootScope.$broadcast("filterRequested", $scope.filter ); 
                  */
              };            
          }],
          templateUrl: './modules/_components/zonerFilter/partials/template.html',
          restrict: "E",
          replace : true,
          scope: 
          {
            options: '=',
            filter: '=' //usar o mesmo do contexto
          }          
        };
      }])

angular.module('AccountApp')
    .controller('CreateAccountController', ['$scope', '$rootScope', 'toastr', '$state', 'AccountService', function ($scope, $rootScope, toastr, $state, AccountService) 
    {    
    	$scope.account =     	
        {  
            allowNewsletter :true, 
    		email : "",
			password : "",
    		confirmPassword : "",
            userName : "",
            idCulture:  moment.locale()
    	};

    	$scope.createAccount = function()
        {
            AccountService.CreateAccount.save(  $scope.account )
            .$promise 
                .then(function (data)  
                {
                    toastr.success(data.message, 'Confirmação!');
                    $state.go('status', {message : {
                        statusType : 1,
                        header: "Código de confirmação enviado!",
                        description : 'Para que seu cadastro seja confirmado acesse o seu e-mail "' + $scope.account.email +'".'
                    }});
                },
                function (reason) 
                {
                    $rootScope.error = reason;
                });                     
    	};
    }])
    .controller('ActivateAccountController', ['$scope', '$rootScope', 'toastr', '$state', 'AccountService', '$stateParams', function ($scope, $rootScope, toastr, $state, AccountService, $stateParams) 
    {                   
        AccountService.ActivateAccount.execute(  { code: $stateParams.code} )
        .$promise 
            .then(function (data)  
            {                  
                toastr.success(data.message, 'Confirmação!');
                $state.go('/');
            },
            function (reason) {
                $rootScope.error = reason;
                $state.go('create-account');
            });                     
    }])
    .controller('LoginAccountController', ['$scope', 'localStorageService', '$rootScope', 'toastr', '$state', '$stateParams', 'AccountService', function ($scope, localStorageService, $rootScope, toastr, $state, $stateParams, AccountService) 
    {                   
        $scope.account =        
        {  
            password : "",
            userName : "",
            grant_type:'password'
        };

        $scope.doLogin = function()
        {
            AccountService.Login( $scope.account )
                .then(function (response) 
                {
                    if($stateParams.redirectTo != '')     
                        $state.go($stateParams.redirectTo) 
                    else
                        $state.go('profile') 
                },
                function (err, status) 
                {
                    $rootScope.error = 
                    { 
                        statusText : err.error,
                        data:  { message: err.error_description } 
                    };
                });
        }
    }])
    .controller('ChangePasswordAccountController', ['$scope', '$rootScope', 'toastr', '$stateParams', '$state', 'AccountService', function ($scope, $rootScope, toastr, $stateParams, $state, AccountService) 
    {    
        $scope.password = 
        {
             oldPassword:"",
             newPassword :"",
             confirmPassword:"",
             activateCode: $stateParams.code
        };

        $scope.changePassword = function()
        {         
            AccountService.ChangePassword.execute(  $scope.password )
            .$promise 
                .then(function (data)  
                {
                    toastr.success(data.message, 'Confirmação!');
                    AccountService.logOut();
                    $state.go('sign-in');
                },
                function (reason) 
                {
                    $rootScope.error = reason;
                });                     
        };        
    }])
    .controller('LogoutAccountController', ['$scope', '$state', 'AccountService', function ($scope, $state, AccountService) 
    {    
        console.log('LogoutAccountController called');
        AccountService.logOut();
        $state.go('sign-in');        
    }])
    .controller('RecoveryAccountController', ['$scope', '$rootScope', 'toastr', '$state', '$stateParams', 'AccountService', function ($scope, $rootScope, toastr, $state, $stateParams, AccountService) 
    {              
        $scope.data = 
        {        
            userName : "",
            recoveryName : "",
            email : "",
            recovery : $stateParams.recoveryType,
            idCulture:  moment.locale() 
        };        

        var _message = 
        {
            statusType : 1,
            header: "",
            description : ""
        };

        if( $stateParams.recoveryType == 1 )
        {
            $scope.data.recoveryName = "Senha";
            _message.header = "Recuperação de senha efetuda com sucesso";
            _message.description = "Um e-mail com as instruções de recuperação de senha foi enviado a sua caixa postal";
        }
        else if( $stateParams.recoveryType == 2 )
        {
            $scope.data.recoveryName = "Usuário";
            _message.header = "Recuperação de usuário efetuda com sucesso";
            _message.description = "Um e-mail contendo o seu usuário foi enviado a sua caixa postal"            
        }

        $scope.requestRecovery = function()
        {
            AccountService.Recover.execute(  $scope.data )
            .$promise 
                .then(function (data)  
                {
                    //toastr.success(data.message, 'Confirmação!');
                    $state.go('status', {message : _message});
                },
                function (reason) 
                {
                    $rootScope.error = reason;
                });     

            //$state.go('status', {});
        }

    }]);    
angular.module("AccountApp")
  .directive('matchPassword', [ "$timeout", function($timeout) {
          return {
              restrict: 'A',
              require: 'ngModel',
              link: function(scope, elm, attrs, ctrl, ngModel) 
              {
                return $timeout(function() 
                {
                  var validateFn = function (viewValue) {
                    if(ctrl.$isEmpty(viewValue) || scope.password != viewValue)
                    {
                       ctrl.$setValidity('matchPassword', false);                                    
                       return viewValue;
                    }
                    else
                    {
                      ctrl.$setValidity('matchPassword', true);
                      return viewValue;
                    }                              
                  };
                  ctrl.$parsers.push(validateFn);
                  ctrl.$formatters.push(validateFn);
                });
              },
              scope: 
              {
                password: '=matchPassword',
                confirmPassword: '=ngModel'
              }  
          }
        }]);
      

angular
    .module('AccountApp')
    .factory('AccountService', ['$http', '$q', '$resource', '$rootScope', 'config', 'localStorageService',
        function ($http, $q, $resource, $rootScope, config, localStorageService) 
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
                CreateAccount: $resource(config.apiUrl + "/apiAccount/CreateAccount", {}, {
                    save: { method: 'POST', params: {} }
                }),
                Recover: $resource(config.apiUrl + "/apiAccount/Recover", {}, {
                    execute: { method: 'POST', params: {} }
                }),                
                ChangePassword: $resource(config.apiUrl + "/apiAccount/ChangePassword", {}, {
                    execute: { method: 'POST', params: {} }
                }),  
                Login: function(account){
                    var data = "grant_type=password&username=" + account.userName + "&password=" + account.password;
                    var deferred = $q.defer();
                    $http.post(config.apiUrl  + '/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' }})
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
                ActivateAccount: $resource(config.apiUrl + "/apiAccount/ActivateAccount", {}, {
                    execute: { method: 'GET', params: {} }
                })                                    
            };
        }]);
 

angular.module('CommonApp')
    .controller('CommonController', ['$scope', '$location', 'CommonService', function ($scope, $location, CommonService) {
        
        $scope.error = null;
        //Alternar templates
        $scope.templates =
            [{ name: 'footer', url: 'modules/common/views/footer.html' },
             { name: 'navbar', url: 'modules/common/views/navbar.html' },
             { name: 'searchbox', url: 'modules/common/views/searchbox.html' }
             ];

        $scope.getTemplateUrl = function (name) {
            for (var i = 0; i < $scope.templates.length; i++) {
                if ($scope.templates[i].name == name)
                    return $scope.templates[i].url;
            }
        };
    }])        
    .controller('TermsConditionsController', ['$state', '$scope', '$timeout', function($state, $scope, $timeout){        
    }])
    .controller('AboutUsController', ['$state', '$scope', '$timeout', function($state, $scope, $timeout){        
    }])
    .controller('StatusCommonController', ['$scope', '$stateParams', 'CommonService', function ($scope, $stateParams, CommonService) 
    {
       $scope.message = $stateParams.message;
    }])    
    .controller('SideBarController', ['$state', '$rootScope', '$scope', '$timeout', 'ElementService', function($state, $rootScope, $scope, $timeout, ElementService)
    {        
        $scope.customer = null;             
        $scope.properties  = [];
        $scope.recordCount = 0;
        
        $rootScope.filter = 
        {   
            ActiveView : 1,
            groupIn : 3,
            recordCount : 0,
            totalRecodsPerPage : 12,
            orderBy : "createDate DESC",
            pageIndex : 0,
            idCulture : moment.locale()
        };  

        var _listProperties = function()
        {
            $rootScope.filter.IDPurpose = [];           
            $rootScope.filter.FilterAttribute = [];
            if($rootScope.filter.singleIDPurpose != null)
                $rootScope.filter.IDPurpose.push($rootScope.filter.singleIDPurpose);        

                        if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minPrice != $rootScope.filter.minPrice || $rootScope.filter.defaultvalues.maxPrice != $rootScope.filter.maxPrice))
                $rootScope.filter.FilterAttribute.push({ Acronym : 'VALT', InitialValue : $rootScope.filter.minPrice, FinalValue : $rootScope.filter.maxPrice });

            //Quartos
            if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minRooms != $rootScope.filter.minRooms || $rootScope.filter.defaultvalues.maxRooms != $rootScope.filter.maxRooms))
                $rootScope.filter.FilterAttribute.push({ Acronym : 'BEDS', InitialValue : $rootScope.filter.minRooms, FinalValue : $rootScope.filter.maxRooms });

            //Garagens              
            if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minGarage != $rootScope.filter.minGarage || $rootScope.filter.defaultvalues.maxGarage != $rootScope.filter.maxGarage))
                $rootScope.filter.FilterAttribute.push({ Acronym : 'GAR',  InitialValue : $rootScope.filter.minGarage, FinalValue : $rootScope.filter.maxGarage });

            //Área
            if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minArea != $rootScope.filter.minArea || $rootScope.filter.defaultvalues.maxArea != $rootScope.filter.maxArea))
                $rootScope.filter.FilterAttribute.push({ Acronym : 'TA', InitialValue : $rootScope.filter.minArea, FinalValue : $rootScope.filter.maxArea });

            console.log($rootScope.filter.FilterAttribute);


            ElementService.ListElement.query( $rootScope.filter )
            .$promise 
                .then(function (data)  
                {
                    $timeout(function () {
                        $scope.properties = data.records;  
                        $scope.recordCount = data.recordCount;
                        $rootScope.filter.recordCount = data.recordCount;
                        $state.go('properties-listing-lines');
                    }, 100);
                },
                function (reason) {
                    $rootScope.error = reason;
                });             
            
        }
        //
        $scope.$on("filterRequested", function (event, args) 
        {
            _listProperties();
        });        
        
        //Carregando imóveis em destaque do anunciante em questão
        ElementService.ListFeatured.query({idCulture : moment.locale()})
        .$promise 
                .then(function (data)  
                {
                    $scope.elementFeatured = data;  
                },
                function (reason) {
                    $rootScope.error = reason;
                });            
    }])
    .controller('ContactController', ['$scope', '$rootScope', 'toastr', 'CustomerService',  function ($scope, $rootScope, toastr, CustomerService) 
    {        
        $scope.contact = {
            name : '',
            message : '',
            email : '',
            idCulture : moment.locale()
        };

        $scope.sendEmail = function(){
            CustomerService.SendEmailContact.execute( $scope.contact )
            .$promise 
                .then(function (data)  
                {
                    toastr.success("Email enviado com sucesso", 'Confirmação!');                                               
                },
                function (reason) {
                    $rootScope.error = reason;
                });
        };   
    }]);         





var _setImagePopup = function(){
  var imagePopup = $('.image-popup');
  if (imagePopup.length > 0) {
      imagePopup.magnificPopup({
          type:'image',
          removalDelay: 300,
          mainClass: 'mfp-fade',
          overflowY: 'scroll'
      });
  }
};

angular.module("CommonApp")
    .directive("propertyCarousel", ['$timeout', 'config', function ($timeout, config) {
            return {
              link: function(scope, el, attrs) 
              {
                  scope.config = config;
                  scope.$watchCollection('items', function(newVal) {
                    if(scope.items != null)
                    {
                      console.log(scope.items);
                      var owl = el.owlCarousel({
                          rtl: false,
                          items: 1,
                          responsiveBaseWidth: ".property-slide",
                          dots: false,
                          autoHeight : true,
                          margin:10,
                          navigation : true,
                          navText: ["",""]
                      });

                      _setImagePopup();                    
                    }
                  });              
              },          
              templateUrl: './modules/common/partials/property-carousel.html',
              restrict: "E",
              replace : true,
              scope: 
              {
                items: '=',
                element: '='
              }                        
            };
      }])
    .directive("homepageSlider", ['$timeout', 'config', function ($timeout, config) {
        return {
          link: function(scope, el, attrs) 
          {
             el.owlCarousel({
                navigation : true, // Show next and prev buttons
                slideSpeed : 300,
                paginationSpeed : 400,
                singleItem:true

                // "singleItem:true" is a shortcut for:
                // items : 1, 
                // itemsDesktop : false,
                // itemsDesktopSmall : false,
                // itemsTablet: false,
                // itemsMobile : false

            });
          },          
          templateUrl: './modules/common/partials/homepage-slider.html',
          restrict: "E",
          replace : true,
          scope: 
          {
            items: '=',
            element: '='
          }                        
        };
    }])
    .directive("sideBar", ['$timeout', 'config', function ($timeout, config) {
        return {
          link: function(scope, el, attrs) 
          {             
          },          
          //Valores default
          compile: function(element, attrs){
             if (!attrs.showFilter) { attrs.showFilter = 'true'; }
             if (!attrs.showFeaturedProperties) { attrs.showFeaturedProperties = 'true'; }
             if (!attrs.showGuides) { attrs.showGuides = 'true'; }             
          },          
          controller: 'SideBarController',          
          templateUrl: './modules/common/partials/sidebar.html',
          restrict: "E",          
          scope: 
          {
            showFilter: '=',
            showGuides: '=',
            showFeaturedProperties: '=',
            config: '='
          }                        
        };
    }]);    





angular
    .module('CommonApp')
	.factory("CommonService", function($q, $timeout, $location)
	{
		var message = null;
	    return {
	   		showMessage: function(value){
				var deferred = $q.defer();
				$timeout(function(){
					//deferred.resolve(message);
					message = value
					$location.path('status');
				},2000);
				return deferred.promise;
	       },
	       getMessage: function(){
	           var deferred = $q.defer();
	           $timeout(function(){
	               deferred.resolve(message);
	           },2000);
	           return deferred.promise;
	       }
	   }
});

angular.module('CultureApp')
    .controller('CultureController', ['$scope', 'CultureService', function ($scope, CultureService) {
        $scope.cultures = []
        $scope.culture = null;
        $scope.error = null
        //Alternar templates
        $scope.templates =
            [{ name: 'create', url: '/app/modules/culture/views/create.html' },
             { name: 'list', url: '/app/modules/culture/views/list.html' }];

        $scope.getTemplateUrl = function (name) {
            for (var i = 0; i < $scope.templates.length; i++) {
                if ($scope.templates[i].name == name)
                    return $scope.templates[i].url;
            }
        };

        $scope.basedate = new Date(2010, 11, 28, 14, 57);

        //$scope.template = $scope.templates[0];        

        $scope.delete = function () {
            $id = $scope.culture.IDCulture;
            CultureService
                .delete({ id: $id })
                .$promise
                    .then(function () {
                        if ($scope.cultures != []) {
                            $scope.cultures.pop($scope.culture);
                            $scope.culture = null;
                        }
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;
                    });
        };

        $scope.save = function () {
            $scope.culture
                .$save()
                    .then(function () {
                        $scope.cultures.push($scope.culture);
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;
                    });
        };

        $scope.update = function () {
            $id = $scope.culture.IDCulture;
            CultureService
                .update({ id: $id }, $scope.culture)
                .$promise
                    .then(function () {
                        $scope.listCulture();
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;

                    });
        };

        $scope.listCulture = function ()
        {
            CultureService.query()
             .$promise
                    .then(function (item) {
                        $scope.cultures = item;
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };

        $scope.getCulture = function (_id) {
            CultureService.get({}, { 'Id': _id })
                .$promise
                    .then(function (item) {
                        //item.CreateDate =  new Date(item.CreateDate);
                        $scope.culture = item;
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };
    }]);

//http://stackoverflow.com/questions/17160771/angularjs-a-service-that-serves-multiple-resource-urls-data-sources

angular
    .module('CultureApp')
    .factory("CultureService", function ($resource) {
    return $resource(
        CONFIG.SERVICE_URL + "/culture/:Id",
        { Id: "@Id" },
        {
            'save': { method: 'POST' },
            "update": { method: "PUT" }, //custom
            "reviews": { 'method': 'GET', 'params': { 'reviews_only': "true" }, isArray: true } //custom
        }
    );
});

angular.module('CustomerApp')
    .controller('ProfileCustomerController', ['$scope', '$rootScope', 'CustomerService', 'toastr', 'config', 'Upload', '$timeout',  function ($scope, $rootScope, CustomerService, toastr, config, Upload, $timeout) 
    {    
    	$scope.customer = null;

		//Chamada principal
		CustomerService.GetCustomer.query()
		.$promise 
			.then(function (data)  
			{
				$scope.customer = data;  

                $scope.$broadcast("elementLoaded", 
                    {
                    	idCountry : data.idCountry, 
                    	idStateProvince: data.idStateProvince, 
                    	idCity : data.idCity                     
                    }
                );				
		    },
		    function (reason) {
		        $rootScope.error = reason;
		    });	    		

		$scope.saveCustomer = function(){
			CustomerService.SaveCustomer.execute( $scope.customer )
				.$promise 
					.then(function (data)  
					{
						toastr.success("Perfil salvo com sucesso!", 'Confirmação!');
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });
		};
		
		$scope.uploadFiles = function(files, errFiles) {
	        $scope.files = files;
	        $scope.errFiles = errFiles;
	        
	        angular.forEach(files, function(file) {
	            file.upload = Upload.upload({
	                url: config.apiUrl + "/apicustomer/UploadAvatar",
	                data: {file: file}
	            });

	            file.upload.then(function (response) {
	                $timeout(function () {
	                    toastr.success(response.data.message, 'Confirmação!');
	                });
	            }, function (error) {
	                $rootScope.error = error;
	            }, function (evt) 
	            {
	                file.progress = Math.min(100, parseInt(100.0 *  evt.loaded / evt.total));
	            });
	        });
    	};
	}])
	.controller('BookmarkedCustomerController', ['$scope', '$rootScope', 'ElementService',  function ($scope, $rootScope, ElementService) 
    {    
    	$scope.bookmarkedElements = [];

		ElementService.ListBookmarked.query( {idCulture : moment.locale()} )
			.$promise 
				.then(function (data)  
				{
					$scope.bookmarkedElements = data;			    					
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });
    }])
    .controller('MyPropertiesCustomerController', ['$filter','$scope', '$rootScope','toastr', '$uibModal', 'ElementService', '$stateParams', function ($filter, $scope, $rootScope, toastr, $uibModal, ElementService, $stateParams) 
    {   
		$scope.properties  = [];
		$scope.recordCount = 0;
    	$scope.filter = 
    	{
    		recordCount : 0,
    		totalRecodsPerPage : 10,
    		orderBy : "name",
    		pageIndex : 0,
    		idCulture : moment.locale()
    	};
    	//
		$scope.$watch('filter', function(oldValue, newValue){
    	 	if(oldValue.pageIndex != newValue.pageIndex || oldValue.orderBy != newValue.orderBy )	
    	 		//if(oldValue.orderBy != newValue.orderBy)
    	 		//	$scope.filter.pageIndex = 0;
				_listProperties();
    	}, true);

    	/*
    	$scope.properties  = [];    	
    	$scope.filter = 
    	{
    		totalRecodsPerPage : 50,
    		orderBy : "name",
    		pageIndex : 0,
    	};
		//
	    $rootScope.$on('listProperties', function(event, args) {
			console.log('listProperties handled');
			_listProperties();
		});
		*/
    	var _listProperties = function(  )
    	{
    		$scope.filter.useInternalAuth = true;
			ElementService.ListElement.query( $scope.filter )
				.$promise 
					.then(function (data)  
					{
						$scope.properties = data.records;  
						$scope.recordCount = data.recordCount;
						$scope.filter.recordCount = data.recordCount;
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });	    		
    	};
    	//
    	$scope.listProperties = _listProperties;
		//    	
    	$scope.disableElement = function(_idElement)
    	{
			var modalInstance = $uibModal.open({
			      //animation: $scope.animationsEnabled,
			      templateUrl: 'myModalContent.html',
			      controller: 'DeleteElementCustomerController',
			      size: 20,			      
			      resolve: {
			        idElement: function () {
			          return _idElement;
			        }			      
			    }});

			modalInstance.result.then(function (response) 
			{
				toastr.success(response.message, 'Confirmação!');
				//Atualizando o status on the fly
				var item = $filter('filter')($scope.properties, {idElement : response.idElement})[0];
				item.status = 0;
			}, 
			function () {
				console.log('Modal dismissed at: ' + new Date());
			});
    	};

    	//Start
    	_listProperties();
    }])
	.controller('DeleteElementCustomerController', ['$scope', '$rootScope', 'ElementService', 'idElement', '$uibModalInstance', function ($scope, $rootScope, ElementService, idElement, $uibModalInstance) 
    {    
    	$scope.idElement = idElement;

		$scope.ok = function () 
		{			
			ElementService.InactivateElement.execute( { idElement : $scope.idElement })	
			.$promise 
				.then(function (data)  
				{
					$uibModalInstance.close( { message : data.message, idElement: $scope.idElement } );
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    		
			
		};

		$scope.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
    }])
    .controller('SendMessageCustomerController', ['$scope', '$rootScope', 'toastr', 'CustomerService',  function ($scope, $rootScope, toastr, CustomerService) 
    {    
    	$scope.showMessageForm = false;
    	$scope.contact = {
    		name : '',
    		message : '',
    		email : '',
    		idElement : 0,
    		idCustomer : 0,
    		idCulture : moment.locale()
    	};

    	var _fillFormFields = function(event, args) 
    	{
			$scope.contact.idElement = ( args.idElement != null ) ? args.idElement : null;
			$scope.contact.idCustomer = args.idCustomer;			

			//Carrega os dados da pessoa que está logada automaticamente
			if($rootScope.authData != null && $rootScope.authData.isAuthenticated == true)
			{
				CustomerService.GetCustomer.query()
				.$promise 
					.then(function (data)  
					{
						$scope.contact.name = data.name;
						$scope.contact.email = data.email;
						$scope.showMessageForm = true;
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });	    			
			}
			else
			{
				$scope.showMessageForm = true;
			}		
    	}
    	//
    	$scope.$on("customerLoaded", function (event, args) 
        {
        	_fillFormFields(event, args); 
        });  
        //
        $scope.$on("elementLoaded", function (event, args) 
        {
        	_fillFormFields(event, args);
        });  

    	$scope.sendEmail = function(){
    		CustomerService.SendEmailRequestInfo.execute( $scope.contact )
			.$promise 
				.then(function (data)  
				{
					toastr.success(data.message, 'Confirmação!');						    					
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });
    	};   
    }])
    .controller('AgentDetailCustomerController', ['$timeout','$scope', '$rootScope', 'CustomerService', 'ElementService', '$stateParams', '$controller', 'config', function ($timeout, $scope, $rootScope, CustomerService, ElementService, $stateParams, $controller, config) 
    {   
    	$scope.customer = null;  	    	
		$scope.properties  = [];
		$scope.recordCount = 0;
    	
		$scope.filter = 
    	{   
			groupIn : 3,
    		recordCount : 0,
    		totalRecodsPerPage : 12,
    		orderBy : "createDate DESC",
    		pageIndex : 0,
    		idCustomer : $stateParams.idCustomer,
    		idCulture : moment.locale()
		};  

		var _listProperties = function()
    	{
    		$scope.filter.IDPurpose = [];			
			$scope.filter.FilterAttribute = [];
			if($scope.filter.singleIDPurpose != null)
				$scope.filter.IDPurpose.push($scope.filter.singleIDPurpose);		

			//Preço
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minPrice != $scope.filter.minPrice || $scope.filter.defaultvalues.maxPrice != $scope.filter.maxPrice))
				$scope.filter.FilterAttribute.push({ IDAttribute : 5, InitialValue : $scope.filter.minPrice, FinalValue : $scope.filter.maxPrice });

			//Quartos
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minRooms != $scope.filter.minRooms || $scope.filter.defaultvalues.maxRooms != $scope.filter.maxRooms))
				$scope.filter.FilterAttribute.push({ IDAttribute : 1, InitialValue : $scope.filter.minRooms, FinalValue : $scope.filter.maxRooms });

			//Garagens 				
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minGarage != $scope.filter.minGarage || $scope.filter.defaultvalues.maxGarage != $scope.filter.maxGarage))
				$scope.filter.FilterAttribute.push({ IDAttribute : 11,  InitialValue : $scope.filter.minGarage, FinalValue : $scope.filter.maxGarage });

			//Área
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minArea != $scope.filter.minArea || $scope.filter.defaultvalues.maxArea != $scope.filter.maxArea))
				$scope.filter.FilterAttribute.push({ IDAttribute : 4, InitialValue : $scope.filter.minArea, FinalValue : $scope.filter.maxArea });

			ElementService.ListElement.query( $scope.filter )
			.$promise 
				.then(function (data)  
				{
					$timeout(function () {
						$scope.properties = data.records;  
						$scope.recordCount = data.recordCount;
						$scope.filter.recordCount = data.recordCount;
					}, 100);
		        },
		        function (reason) {
		            $rootScope.error = reason;
		        });			    
    		
    	}
    	//
    	$scope.$on("filterRequested", function (event, args) 
    	{
			_listProperties();
    	});
    	//
    	_listProperties();		  	
    	
		//Carregando imóveis em destaque do anunciante em questão
		ElementService.ListFeatured.query(  { idCustomer : $stateParams.idCustomer, idCulture : moment.locale() } )
		.$promise 
				.then(function (data)  
				{
                    $scope.elementFeatured = data;  
                },
                function (reason) {
                    $rootScope.error = reason;
                });			


		CustomerService.GetCustomer.query( { idCustomer :  $stateParams.idCustomer })
		.$promise 
			.then(function (data)  
			{
				$scope.customer = data;  
				$scope.$broadcast("customerLoaded", 
                {
	            	idCustomer : data.idCustomer	            	
                });						
				//Carregar imóveis vinculados a agente em questão
				//$rootScope.$emit('listProperties'/*, args*/);				
		    },
		    function (reason) {
		        $rootScope.error = reason;
		    });	    		
	}])
angular.module('CustomerApp')
	//Diretiva usada pra identificar quando o ngrepeat renderizou todos os items de uma coleção
	.directive('bookmarkLoaded', function() {
	  return function(scope, element, attrs) {
	    if (scope.$last)
	    {      
	    	console.log('bookmarkLoaded called')
			if($('.property').hasClass('masonry') )
	    	{
	    		console.log('masonry found');
		        var container = $('.grid');
		        container.imagesLoaded( function() {
		            container.masonry({
		                gutter: 15,
		                itemSelector: '.masonry'
		            });
	        	});

		        if ($(window).width() > 991) 
		        {
		            $('.masonry').hover(function() {
		                    $('.masonry').each(function () {
		                        $('.masonry').addClass('masonry-hide-other');
		                        $(this).removeClass('masonry-show');
		                    });
		                    $(this).addClass('masonry-show');
		                }, function() {
		                    $('.masonry').each(function () {
		                        $('.masonry').removeClass('masonry-hide-other');
		                    });
		                }
		            );

		            var config = {
		                after: '0s',
		                enter: 'bottom',
		                move: '20px',
		                over: '.5s',
		                easing: 'ease-out',
		                viewportFactor: 0.33,
		                reset: false,
		                init: true
		            };
		            window.scrollReveal = new scrollReveal(config);
		        }
	    	}		    	
	    }
	  };
	})
	.directive('agentPropertiesLoaded', function() {
		  return function(scope, element, attrs) {
		    if (scope.$last)
		    {      
		    	console.log('agentPropertiesLoaded called');
				 var rowsToShow = 2; // number of collapsed rows to show
				    var $layoutExpandable = $('.layout-expandable');
				    var layoutHeightOriginal = $layoutExpandable.height();
				    $layoutExpandable.height($('.layout-expandable .row').height()*rowsToShow-5);
				    $('.show-all').on("click", function() {
				    	console.log('show-all clicked');
				        if ($layoutExpandable.hasClass('layout-expanded')) {
				            $layoutExpandable.height($('.layout-expandable .row').height()*rowsToShow-5);
				            $layoutExpandable.removeClass('layout-expanded');
				            $('.show-all').removeClass('layout-expanded');
				        } else {
				            $layoutExpandable.height(layoutHeightOriginal);
				            $layoutExpandable.addClass('layout-expanded');
				            $('.show-all').addClass('layout-expanded');
				        }
				    });
			    }
		  };
	});


angular
    .module('CustomerApp')
    .factory('CustomerService', ['$resource', 'config', 'Upload',
        function ($resource, config, Upload){
            'use strict';
            return {
                GetCustomer: $resource(config.apiUrl + "/apicustomer/GetCustomerAsync", {}, {
                    query: { method: 'GET', params: {} }
                }),
                SaveCustomer: $resource(config.apiUrl + "/apicustomer/SaveCustomer", {}, {
                    execute: { method: 'POST', params: {} },
                }),
                ListProperties: $resource(config.apiUrl + "/apicustomer/ListProperties", {}, {
                    query: { method: 'POST', params: {} },
                }),
                SendEmailRequestInfo: $resource(config.apiUrl + "/apicustomer/SendEmailRequestInfo", {}, {
                    execute: { method: 'POST', params: {} },
                }),
                SendEmailContact: $resource(config.apiUrl + "/apicustomer/SendEmailContact", {}, {
                    execute: { method: 'POST', params: {} },
                })
            };
        }]);


angular.module('ElementApp')
    .controller('ElementController', ['$timeout', '$rootScope', '$scope', '$filter', '$state', '$window', 'ElementService', 'StructureService', 'toastr', '$uibModal', function ($timeout, $rootScope, $scope, $filter, $state, $window, ElementService, StructureService, toastr, $uibModal) 
    {   
    	var _switchView = function (view)
    	{
			if(view == 1) //Listagem
				$state.go('properties-listing-lines');
			else //Mapa
				$state.go('properties-map');
		};

    	$scope.initialize = function()
    	{
	        $scope.purposes = [];        
			$rootScope.filter = null;
			$scope.elementResultPaged=[];
	    	$scope.elementResult = [];			    	
	    	$scope.elementFeatured = [];
	    	$scope.listTopViewed = [];    	
			$scope.showSearchEngine = true;
			$scope.templates = [{ name: 'featured', url: 'modules/element/views/featured.html' } ];
	    	$rootScope.filter = 
	    	{   
	    		/*
				minArea : 0,
	    		maxArea: 10000, 			    		
	    		minGarage : 1,
	    		maxGarage: 4, 
	    		minRooms : 1,
	    		maxRooms: 10, 		
	    		minPrice : 0,
	    		maxPrice : 2000000,
	    		singleIDPurpose : '',
	    		TotalRecords : 50,
				IDElement : "",
				IDCustomer : "0",
	            ActiveView : 2	, //usado para indicar qual o resultado se map ou listagem 1 usar mapa
	            BaseAddress: "",
	            FilterAttribute: [],
	            IDCity: "",
	            IDCountry: "",
	            IDHierarchyStructure: "",
	            IDHierarchyStructureParent: "",
	            IDPurpose: [],
	            IDStateProvince: '',
	            LatitudeBase:  '-30.02000000',
	            LongitudeBase: '-51.11000000',
	            OrderBy: "",
	            Radius: 0,
	            TotalRecodsPerPage : 10,
	            PageIndex : 0
	            */
			};
			$scope.listPurpose();
    	};


    	//
		$scope.changeFilterPurpose = function(idPurpose)
		{
			$rootScope.filter.singleIDPurpose = idPurpose;			
		}
		//
		$scope.doFilterListing = function(view)
		{
			$rootScope.filter.ActiveView = view;		
			try
			{
				//verificar o endereço informado;			
				var geocoder = new google.maps.Geocoder();
	            geocoder.geocode({ 'address': $rootScope.filter.BaseAddress }, function (results, status) 
	            {
	                if (status == google.maps.GeocoderStatus.OK) 
	                {
	                    if (results.length > 0) 
	                    {
	                        if (results.length > 1) 
	                        {
	                        	console.log(results);
								var modalInstance = $uibModal.open({
							      templateUrl: 'select-address-template.html',
							      controller: 'AddressSelectController',
							      size: 20,			      
							      resolve: 
							      {
							      	addresses: function () {
							          return results;
							        },			      						        
							    }});

								modalInstance.result.then(function (response) 
								{
									$rootScope.filter.LatitudeBase 	= response.address.geometry.location.lat()
									$rootScope.filter.LongitudeBase = response.address.geometry.location.lng()
									$rootScope.filter.BaseAddress 	= response.address.formatted_address;
									_switchView(view);
								}, 
								function () {
									console.log('Modal dismissed at: ' + new Date());
								});
	                        }
	                        else 
	                        {
	                        	$rootScope.filter.LatitudeBase = results[0].geometry.location.lat()
								$rootScope.filter.LongitudeBase = results[0].geometry.location.lng()
								$rootScope.filter.BaseAddress = results[0].formatted_address;
								_switchView(view);
	                        }
	                    }
						else 
		                {
		                	toastr.warning("Endereço não foi encontrado!", 'Informação');
		                }                    
	                }
	                else 
	                {
	                	toastr.warning("Endereço não foi encontrado!", 'Informação');
	                }
	            });   
			}			
			catch(err)
			{
				toastr.error("API Google não carregada.", 'Informação');
			}
		}
		//
        $scope.getTemplateUrl = function (name) {
            for (var i = 0; i < $scope.templates.length; i++) {
                if ($scope.templates[i].name == name)
                    return $scope.templates[i].url;
            }
        };
		//
		$scope.switchSearchEngine = function() {$scope.showSearchEngine = !$scope.showSearchEngine};
		//
        $scope.listFeatured = function (_idCustomer) 
        {
        	if(_idCustomer == null)
        		_idCustomer = 0;

			ElementService.ListFeatured.query(  { idCustomer : _idCustomer,  idCulture:  moment.locale() } )
			.$promise 
					.then(function (data)  
					{
	                    $scope.elementFeatured = data;  
	                    setCarouselWidth();
	                },
	                function (reason) {
	                    $rootScope.error = reason;
	                });			
        };
    	//
        $scope.listTopViewed =  function (_idCustomer) 
        {
        	if(_idCustomer == null)
        		_idCustomer = 0;

        	ElementService.ListTopViewed.query(  { idCustomer : _idCustomer, idCulture:  moment.locale() } )
			.$promise 
					.then(function (data)  
					{
	                    $scope.listTopViewed = data;  
	                },
	                function (reason) {
	                    $rootScope.error = reason;
	                });			
        };
        //
        $scope.listPurpose = function()
        {
    		StructureService.ListPurpose.query( { idCulture:  moment.locale() })
	         .$promise
                .then(function (item) 
                {
                    $scope.purposes = item;                        
                },
                function (reason) {
                    $scope.error = reason;
                });		
        };

    }])
	//Carregar o resultado do filtro e mostra na listagem
	.controller('ElementFilterController', ['$timeout', '$rootScope', '$scope', '$filter', '$state', '$window', 'ElementService', 'StructureService', function ($timeout, $rootScope, $scope, $filter, $state, $window, ElementService, StructureService) 
    {       	
    	if($rootScope.filter.ActiveView == 1) //Listagem
		{
			$rootScope.filter.groupIn = 3;	    	
			$rootScope.filter.totalRecodsPerPage = 12;
		}
		else //Mapa
		{
			$rootScope.filter.groupIn = 1;	    	
			$rootScope.filter.totalRecodsPerPage = 30;
		}

		$rootScope.filter.recordCount = 0;
		$rootScope.filter.orderBy = "createDate DESC";
		$rootScope.filter.pageIndex = 0;
		$rootScope.filter.idCulture = moment.locale();

		var _listProperties = function()
    	{
    		$rootScope.filter.IDPurpose = [];			
			$rootScope.filter.FilterAttribute = [];
			if($rootScope.filter.singleIDPurpose != null)
				$rootScope.filter.IDPurpose.push($rootScope.filter.singleIDPurpose);		

			//Preço
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minPrice != $rootScope.filter.minPrice || $rootScope.filter.defaultvalues.maxPrice != $rootScope.filter.maxPrice))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'VALT', InitialValue : $rootScope.filter.minPrice, FinalValue : $rootScope.filter.maxPrice });

			//Quartos
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minRooms != $rootScope.filter.minRooms || $rootScope.filter.defaultvalues.maxRooms != $rootScope.filter.maxRooms))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'BEDS', InitialValue : $rootScope.filter.minRooms, FinalValue : $rootScope.filter.maxRooms });

			//Garagens 				
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minGarage != $rootScope.filter.minGarage || $rootScope.filter.defaultvalues.maxGarage != $rootScope.filter.maxGarage))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'GAR',  InitialValue : $rootScope.filter.minGarage, FinalValue : $rootScope.filter.maxGarage });

			//Área
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minArea != $rootScope.filter.minArea || $rootScope.filter.defaultvalues.maxArea != $rootScope.filter.maxArea))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'TA', InitialValue : $rootScope.filter.minArea, FinalValue : $rootScope.filter.maxArea });

			console.log($rootScope.filter.FilterAttribute);

			ElementService.ListElement.query( $rootScope.filter )
			.$promise 
				.then(function (data)  
				{
					$timeout(function () {
						$scope.properties = data.records;  
						$scope.recordCount = data.recordCount;
						$rootScope.filter.recordCount = data.recordCount;
					}, 100);
		        },
		        function (reason) {
		            $rootScope.error = reason;
		        });			    
    		
    	}
    	//
    	$scope.$on("filterRequested", function (event, args) 
    	{
			_listProperties();
    	});
    	//
    	_listProperties();		  	
		
    }])
	//Buscar o detalhe do elemento de maneira separada
    .controller('ElementDetailController', ['$rootScope', '$scope', '$stateParams', 'ElementService', 'CustomerService', function ($rootScope, $scope, $stateParams, ElementService, CustomerService) 
    {      
    	//'use strict';    	
    	$scope.state = "empty";
    	$scope.element = null;
		$scope.customer = null;
		$scope.elementFeatured = [];
		$scope.elementSimilar = [];

		if($stateParams.idElement != null)
		{
	    	//Chamada principal
			ElementService.GetElement.query(  { idCulture : moment.locale(), idElement : $stateParams.idElement } )
			.$promise 
					.then(function (data)  
					{
	                    $scope.element = data;  
	                    $scope.$broadcast("elementLoaded", data);	   

						ElementService.ListImages.execute( { idCulture : moment.locale(), idElement : data.idElement, groupIn : 0 })	
						.$promise 
							.then(function (data)  
							{
								var images = [];
								for (var i = 0; i < data.images.length; i++) 
								{
									images.push({index:i, image:data.images[i]});
								};		
								$scope.element.images = images;
						    },
						    function (reason) {
						        $rootScope.error = reason;
						    });

	                    //Buscando os dados do aunciante
						CustomerService.GetCustomer.query(  { idCustomer : data.idCustomer } )
						.$promise 
							.then(function (data)  
							{
			                    $scope.customer = data;  
			                },
			                function (reason) {
			                    $rootScope.error = reason;
			                });		                    

			            //Buscando os imóveis destacados    
						ElementService.ListFeatured.query(  { idCulture : moment.locale(), idCustomer : data.idCustomer} )
						.$promise 
							.then(function (data)  
							{
			                    $scope.elementFeatured = data;  
			                },
			                function (reason) {
			                    $rootScope.error = reason;
			                });			            

						//Buscando os imóveis similares
						ElementService.ListSimilar.query(  { idCulture : moment.locale(), idElement : data.idElement, idCustomer : data.idCustomer} )
						.$promise 
							.then(function (data)  
							{
			                    $scope.elementSimilar = data;  
			                },
			                function (reason) {
			                    $rootScope.error = reason;
			                });			                
	                },
	                function (reason) {
	                    $scope.error = reason;
	                });			        
		}
	}])
	//Buscar o detalhe do elemento de maneira separada
    .controller('ElementSubmitController', ['$state', '$rootScope', '$scope', '$stateParams', '$filter', 'ElementService', 'toastr', 'Upload', 'config', '$timeout', '$uibModal', function ($state, $rootScope, $scope, $stateParams, $filter, ElementService, toastr, Upload, config, $timeout, $uibModal) 
    {      		
    	var _idElement = $stateParams.idElement != null ? $stateParams.idElement : null;
		var _uploadImages = function( idElement ) {
			if ($scope.element.files && $scope.element.files.length) 
			{
				var firstSubmit = true;
				var files = $scope.element.files;

		        angular.forEach($scope.element.files, function(file) 
		        {
		            file.upload = Upload.upload({
		                url: config.apiUrl + "/apielement/UploadImages",
						data: { file: file, idElement : idElement, deleteDir : firstSubmit }
		            });
		            firstSubmit = false;

		            file.upload.then(function (response) {
		                $timeout(function () {
		                    //toastr.success(response.data.message, 'Confirmação!');
		                    file.result = 'OK';

		                    //ultimo arquivo enviado;
		                    //if(key == files.length - 1)
		                    _listImages(idElement);
		                });
		            }, function (response) {
						if (response.status > 0)
                    		$scope.errorMsg = response.status + ': ' + response.data;		            	
		            }, function (evt) 
		            {
		                file.progress = Math.min(100, parseInt(100.0 *  evt.loaded / evt.total));
		            });
		        });
	    	}
    	}
		// 
    	var _listImages = function(_idElement)
    	{
			ElementService.ListImages.execute( { idElement : _idElement, groupIn : 3 })	
				.$promise 
					.then(function (data)  
					{
						$scope.element.agregatedImages = data.images;
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });
    	}
    	//
    	$scope.initialize = function()
    	{
    		$scope.element = { idCulture : moment.locale() };
	    	$scope.basicAttributes = [];
	    	$scope.infrastructureAttributes = [];

			if(_idElement != '')
			{
		    	//Chamada principal
				ElementService.GetElement.query(  { idCulture : moment.locale(), idElement : _idElement, igoreAttributes : true, validateCustomer : true } )
				.$promise 
					.then(function (data)  
					{
						$scope.element = data;  
						_listImages($scope.element.idElement);
	                    //Avisando controller secundarios que o elemento foi carregado
	                    /*
	                    $scope.$broadcast("elementLoaded", {
		                    	idCountry : data.idCountry, 
		                    	idStateProvince: data.idStateProvince, 
		                    	idCity : data.idCity, 
		                    	idPurpose : data.idPurpose,
		                    	idCategory: data.idHierarchyStructureParent 
		                    });
		                */    
	                    $scope.$broadcast("elementLoaded", data);	                    
	                    $state.go('submit');
		            },
	                function (reason) 
	                {
	                    $rootScope.error = reason;
	                    $state.go('sign-in');
	                });			        
			}

			//Carregar os atributos
			$scope.listBasicAttributes();
	        $scope.listInfrastructureAttributes();			    	
    	};
		//	
    	$scope.insertUpdateElement = function()
    	{
    		var _element = $scope.element;
    		
    		_element.basicAttributes = $filter('filter')($scope.infrastructureAttributes, { checked:true });  //filtrando pela propriedade Checked
			angular.forEach($scope.basicAttributes, function(group){
				angular.forEach(group, function(item){				                   
					item.Checked = true;
    				_element.basicAttributes.push(item);
				});                   
            });

    		ElementService.InsertUpdateElement.save(  _element )
			.$promise 
				.then(function (data)  
				{
					$scope.element.idElement = data.idElement;
					$scope.element.idCustomer = data.idCustomer;					
					_uploadImages(data.idElement);
					
					toastr.success(data.message, 'Confirmação!');
					$scope.element.files = null;
					//$state.go('submit', { idElement: data.idElement }, {reload: true, notify: true});
					//$state.go('my-properties');
                },
                function (reason) {
                    $rootScope.error = reason;
                });    		
    	};
		//	
		$scope.listBasicAttributes =  function()
		{
			ElementService.ListBasicAttributes.query(  { idCulture : moment.locale(), idElement : _idElement} )
			.$promise 
				.then(function (data)  
				{
                    $scope.basicAttributes = data;  

                },
                function (reason) {
                    $rootScope.error = reason;
                });			        
		} 
		//	
		$scope.listInfrastructureAttributes =  function()
		{
			ElementService.ListInfrastructureAttributes.query(  { idCulture : moment.locale(), idElement : _idElement } )
			.$promise 
				.then(function (data)  
				{
                    $scope.infrastructureAttributes = data;  
                    //$('.checkbox').iCheck();
                },
                function (reason) {
                    $rootScope.error = reason;
                });			        
		} 		
		//
		$scope.setDefaultImage = function(_idElement, _idElementAttribute){
			ElementService.SetDefaultImage.execute( { idElement : _idElement, idElementAttribute: _idElementAttribute })	
			.$promise 
				.then(function (data)  
				{
					$scope.element.defaultPicturePath = data.defaultPicturePath;
					toastr.success(data.message, 'Confirmação!');
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    	
		}
		//
    	$scope.deleteImage = function(_idElement, _idElementAttribute)
    	{
    		var _templateUrl;
    		if(_idElementAttribute == null)
    			_templateUrl = 'delete-all-images-template.html';
    		else
    			_templateUrl = 'delete-image-template.html';    			

			var modalInstance = $uibModal.open({
			      //animation: $scope.animationsEnabled,
			      templateUrl: _templateUrl,
			      controller: 'DeleteImageElementController',
			      size: 20,			      
			      resolve: 
			      {
			      	idElement: function () {
			          return _idElement;
			        },			      
			        idElementAttribute: function () {
			          return _idElementAttribute;
			        }			      
			    }});

			modalInstance.result.then(function (response) 
			{
				_listImages(response.idElement);
				//var item = $filter('filter')($scope.element.agregatedImages, response.imageName)[0];
				toastr.success(response.message, 'Confirmação!');
			}, 
			function () {
				console.log('Modal dismissed at: ' + new Date());
			});
    	};

		$scope.initialize();
	}])
	.controller('DeleteImageElementController', ['$scope', '$rootScope', 'ElementService', 'idElement', 'idElementAttribute', '$uibModalInstance', function ($scope, $rootScope, ElementService, idElement, idElementAttribute, $uibModalInstance) 
    {    
    	$scope.idElement = idElement;
    	$scope.idElementAttribute = idElementAttribute;

		//
		$scope.confirmDeleteImage = function(){
			ElementService.DeleteElementImage.execute( { idElement : $scope.idElement, idElementAttribute: $scope.idElementAttribute})	
			.$promise 
				.then(function (data)  
				{
					$uibModalInstance.close( { message : data.message, 	
											   idElement: $scope.idElement, 
											   idElementAttribute: $scope.idElementAttribute } );
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    		
		}
		//
		$scope.cancelDeleteImage = function(){
			$uibModalInstance.dismiss('cancel');
		}
		//
		$scope.confirmDeleteAllImages = function(){
			ElementService.DeleteElementImage.execute( { idElement : $scope.idElement })	
			.$promise 
				.then(function (data)  
				{
					$uibModalInstance.close( { message : data.message, 
											   idElement: $scope.idElement } );
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    		
		}
		//
		$scope.cancelDeleteAllImagese = function(){
			$uibModalInstance.dismiss('cancel');
		}
    }])
	.controller('AddressSelectController', ['$scope', '$rootScope', 'addresses', '$uibModalInstance', function ($scope, $rootScope, addresses, $uibModalInstance) 
    {   
    	$scope.addresses = addresses;
		$scope.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};		
		//
		$scope.select = function (_address) 
		{			
			$uibModalInstance.close( { address : _address } );
		};
    }]);

angular.module('ElementApp')
	.directive('featuredLoaded', function() {
	  return function(scope, element, attrs) {
	    if (scope.$last){      
	    	console.log('featuredLoaded called')
	    }
	  };
	})
	.directive('propertyGalleryLoaded', function() {
	  return function(scope, element, attrs) {
	    if (scope.$last){      
	    	console.log('propertyGalleryLoaded called')
	    	 initializeOwl(false);
	    }
	  };
	})	
	.directive('zonerBookmark', ['$timeout', '$rootScope', 'ElementService', function($timeout, $rootScope, ElementService) {
	  return {
	    restrict: 'E',
	    replace: true,
	    scope: {
         elementData: '='
      	},
	    //template: '<p style="background-color:{{color}}">Hello World',
	    templateUrl: './modules/element/partials/bookmark.html',
	    link: function(scope, elem, attrs) 
	    {	    
			scope.$watch('elementData', function() 
	        {
	        	if(scope.elementData != null)
	        	{		       
					if(scope.elementData.AddedAsFavorite)
						elem.addClass('bookmark-added');
					else
						elem.removeClass('bookmark-added');
				}
	        }); 

			elem.bind('click', function() 
			{	
				if(scope.elementData.AddedAsFavorite == true)
				{				
					ElementService.RemoveAsFavorite.execute(  { idElement : scope.elementData.idElement } )
					.$promise 
						.then(function (item)  
						{			
							scope.elementData.AddedAsFavorite = false;
			    			elem.removeClass('bookmark-added');
				        },
				        function (reason) {
				            $rootScope.error = reason;
				        });		
				}
		    	else
		    	{
					ElementService.AddAsFavorite.execute(  { idElement : scope.elementData.idElement } )
					.$promise 
						.then(function (item)  
						{			
							scope.elementData.AddedAsFavorite = true;
			    			elem.addClass('bookmark-added');
				        },
				        function (reason) {
				            $rootScope.error = reason;
				        });				    		
		    	}

		    	//efetua a alteração do valor no escopo principal
				//scope.$apply();
			});					        
	    }
	  };
	}])
	.directive('smallProperty', ['config', function(config) {
	  return {
  	    //require: ['^array', '^limit'], //obrigatorio
  	    replace: true,
	    restrict: "E",
	    scope: {
         array: '=',
         limit:'='
      	},
	    templateUrl: './modules/element/partials/small-property.html',
	    link: function(scope, elem, attrs) 
	    {	    
	    }
	  };
	}])
	.directive('mediumProperty', ['config', function(config) {
	  return {
  	    //require: ['^array', '^limit'], //obrigatorio
	    restrict: "E",
	    scope: {
         element: '=',
         config: '='
      	},
	    templateUrl: './modules/element/partials/medium-property.html',
	    link: function($scope, elem, attrs) 
	    {	    
	    	//console.debug($scope);
	    }
	  };
	}]);



angular
    .module('ElementApp')
    .factory('ElementService', ['$resource', 'config',
        function ($resource, config) 
        {
            return {
                SetDefaultImage: $resource(config.apiUrl + "/apielement/SetDefaultImage", {}, {
                    execute: { method: 'GET', params: {} }
                }),                                                
                DeleteElementImage: $resource(config.apiUrl + "/apielement/DeleteElementImage", {}, {
                    execute: { method: 'GET', params: {} }
                }),                                
                ListImages: $resource(config.apiUrl + "/apielement/ListImages", {}, {
                    execute: { method: 'GET', params: {} }
                }),                    
                InsertUpdateElement: $resource(config.apiUrl + "/apielement/InsertUpdateElement", {}, {
                    save: { method: 'POST', params: {} }
                }),                
                ListElement: $resource(config.apiUrl + "/apielement/ListElement", {}, {
                    query: { method: 'POST', params: {}, isArray: false }
                }),
                ListFeatured: $resource(config.apiUrl + "/apielement/listfeatured", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListTopViewed: $resource(config.apiUrl + "/apielement/ListTopViewed", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListSimilar: $resource(config.apiUrl + "/apielement/ListSimilar", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListBasicAttributes: $resource(config.apiUrl + "/apielement/ListBasicAttributes", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListInfrastructureAttributes: $resource(config.apiUrl + "/apielement/ListInfrastructureAttributes", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                                
                GetElement: $resource(config.apiUrl + "/apielement/getelement", {}, {
                    query: { method: 'GET', params: {} }
                }),                
                InactivateElement: $resource(config.apiUrl + "/apielement/InactivateElement", {}, {
                    execute: { method: 'GET', params: {} }
                }),                                
                AddAsFavorite: $resource(config.apiUrl + "/apielement/AddAsFavorite", {}, {
                    execute: { method: 'GET', params: {} }
                }),
                RemoveAsFavorite: $resource(config.apiUrl + "/apielement/RemoveAsFavorite", {}, {
                    execute: { method: 'GET', params: {} }
                }),
                ListBookmarked: $resource(config.apiUrl + "/apielement/ListBookmarked", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                })               
            };
        }]);


angular.module('LocationApp')
    .controller('LocationController', ['$rootScope', '$scope', '$location', 'LocationService', function ($rootScope, $scope, $location, LocationService) {
        //--------------------------------Events Handlers--------------------------------
        $scope.$on("elementLoaded", function (event, args) 
        {
            $scope.listCountry();
            $scope.listStateProvince(args.idCountry);
            $scope.listCity (args.idCountry, args.idStateProvince);           
        });     
        //
        $scope.initialize = function()
        {
            console.log('LocationController.initialize called');
            $scope.countries = [];
            $scope.stateProvinces = [];
            $scope.cities = [];
        }    	
        //
		$scope.listCountry = function ()
        {
            $scope.stateProvinces = [];
            $scope.cities = [];

            LocationService.ListCountry.query()
             .$promise
                    .then(function (item) 
                    {
                        $scope.countries = item;                        
                    },
                    function (reason) {
                        console.log(reason);
                        $rootScope.error = reason;
                    });
        };		
        //                
        $scope.listStateProvince = function (_idCountry)
        {   
            console.log('listStateProvince called');
            $scope.stateProvinces = [];
            $scope.cities = [];

            if(_idCountry != null)
            {
                LocationService.ListStateProvince.query( {idCountry : _idCountry } )
                 .$promise
                        .then(function (item) 
                        {
                            $scope.stateProvinces = item;                        
                        },
                        function (reason) {
                            $rootScope.error = reason;
                        });
            }        
        };      
        //            
        $scope.listCity = function (_idCountry, _idStateProvince)
        {   
            console.log('listCity called');
            $scope.cities = [];

            if(_idCountry != null && _idStateProvince != null)
            {
                LocationService.ListCity.query( {idCountry :_idCountry, idStateProvince : _idStateProvince } )
                 .$promise
                        .then(function (item) 
                        {
                            $scope.cities = item;                        
                        },
                        function (reason) {
                            $rootScope.error = reason;
                        });
            }        
        };    
    }]);
angular
    .module('LocationApp')
    .factory('LocationService', ['$resource', 'config',
        function ($resource, config) 
        {
            return {
                ListCountry: $resource(config.apiUrl + "/apilocation/listcountry", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListStateProvince: $resource(config.apiUrl + "/apilocation/liststateprovince", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListCity: $resource(config.apiUrl + "/apilocation/listCity", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                })
            };
        }]);


angular.module('CultureApp')
    .controller('CultureController', ['$scope', 'CultureService', function ($scope, CultureService) {
        $scope.cultures = []
        $scope.culture = null;
        $scope.error = null
        //Alternar templates
        $scope.templates =
            [{ name: 'create', url: '/app/modules/culture/views/create.html' },
             { name: 'list', url: '/app/modules/culture/views/list.html' }];

        $scope.getTemplateUrl = function (name) {
            for (var i = 0; i < $scope.templates.length; i++) {
                if ($scope.templates[i].name == name)
                    return $scope.templates[i].url;
            }
        };

        $scope.basedate = new Date(2010, 11, 28, 14, 57);

        //$scope.template = $scope.templates[0];        

        $scope.delete = function () {
            $id = $scope.culture.IDCulture;
            CultureService
                .delete({ id: $id })
                .$promise
                    .then(function () {
                        if ($scope.cultures != []) {
                            $scope.cultures.pop($scope.culture);
                            $scope.culture = null;
                        }
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;
                    });
        };

        $scope.save = function () {
            $scope.culture
                .$save()
                    .then(function () {
                        $scope.cultures.push($scope.culture);
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;
                    });
        };

        $scope.update = function () {
            $id = $scope.culture.IDCulture;
            CultureService
                .update({ id: $id }, $scope.culture)
                .$promise
                    .then(function () {
                        $scope.listCulture();
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;

                    });
        };

        $scope.listCulture = function ()
        {
            CultureService.query()
             .$promise
                    .then(function (item) {
                        $scope.cultures = item;
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };

        $scope.getCulture = function (_id) {
            CultureService.get({}, { 'Id': _id })
                .$promise
                    .then(function (item) {
                        //item.CreateDate =  new Date(item.CreateDate);
                        $scope.culture = item;
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };
    }]);

//http://stackoverflow.com/questions/17160771/angularjs-a-service-that-serves-multiple-resource-urls-data-sources

angular
    .module('CultureApp')
    .factory("CultureService", function ($resource) {
    return $resource(
        "http://localhost:9090/api/culture/:Id",
        { Id: "@Id" },
        {
            'save': { method: 'POST' },
            "update": { method: "PUT" }, //custom
            "reviews": { 'method': 'GET', 'params': { }, isArray: true } //custom
        }
    );
});
 
angular.module('StructureApp')
    .controller('StructureController', ['$scope', '$location', 'StructureService', function ($scope, $location, StructureService) {   	
        //--------------------------------Events Handlers--------------------------------
        $scope.$on("elementLoaded", function (event, args) 
        {
            console.log('StructureController ---> elementLoaded Handled'); 
            $scope.listPurpose();
            if(args.idPurpose != null)
                $scope.listCategory(args.idPurpose);

            if(args.idHierarchyStructureParent != null)
                $scope.listType (args.idHierarchyStructureParent);
        });     
        //
         $scope.initialize = function()
        {
            $scope.purposes = [];
            $scope.categories = [];        
            $scope.types = [];        
        };
        //
		$scope.listPurpose = function ()
        {
            $scope.categories = [];        
            $scope.types = [];        

            StructureService.ListPurpose.query( { idCulture : moment.locale() })
             .$promise
                    .then(function (item) 
                    {
                        $scope.purposes = item;                        
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };		
        //
        $scope.listCategory = function (_idPurpose)
        {
            $scope.categories = [];        
            $scope.types = [];        
            if(_idPurpose != null)
            {
                //$scope.filter.singleIDPurpose
                StructureService.ListCategory.query( { idPurpose : _idPurpose, idCulture : moment.locale() })
                 .$promise
                        .then(function (item) 
                        {
                            $scope.categories = item;                        
                        },
                        function (reason) {
                            $scope.error = reason;
                        });
            }
        };  
        //
        $scope.listType = function (_idCategory)
        {
            $scope.types = [];        
            if(_idCategory != null)
            {
                //$scope.filter.IDHierarchyStructureParent
                StructureService.ListType.query( { idCategory : _idCategory, idCulture : moment.locale() })
                 .$promise
                        .then(function (item) 
                        {
                            $scope.types = item;                        
                        },
                        function (reason) {
                            $scope.error = reason;
                        });
            }                    
        };  
    }]);

angular
    .module('StructureApp')
    .factory('StructureService', ['$resource', 'config',
        function ($resource, config) 
        {
            return {
                ListPurpose: $resource(config.apiUrl + "/apistructure/listpurpose", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListCategory: $resource(config.apiUrl + "/apistructure/listcategory", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListType: $resource(config.apiUrl + "/apistructure/listtype", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
            };
        }]);


/*jslint unparam: true */
/*global angular: false, console: false, define, module */
(function(root, factory) {
  'use strict';
  if (typeof define === 'function' && define.amd) {
    // AMD. Register as an anonymous module.
    define(['angular'], factory);
  } else if (typeof module === 'object' && module.exports) {
    // Node. Does not work with strict CommonJS, but
    // only CommonJS-like environments that support module.exports,
    // like Node.
    // to support bundler like browserify
    module.exports = factory(require('angular'));
  } else {
    // Browser globals (root is window)
    factory(root.angular);
  }

}(this, function(angular) {
  'use strict';
  var module = angular.module('zonerMapModule', [])
  .factory('ZonerMap', function($timeout, $rootScope, $state, $document, $window, $compile, $filter, config, ElementService) {
    'use strict';

    /**
     * ZonerMap
     *
     * @param {ngScope} scope            The AngularJS scope
     * @param {Element} sliderElem The slider directive element wrapped in jqLite
     * @constructor
     */
    var ZonerMap = function(scope, zonerMapElem) 
    {
		this.scope = scope;
		this.zonerMapElem = zonerMapElem;
		this.filter =  this.scope.filterData;
		this.elementResult = this.scope.elementResultData;
		this.init();
    };

    var mapStyles = [{featureType:'water',elementType:'all',stylers:[{hue:'#d7ebef'},{saturation:-5},{lightness:54},{visibility:'on'}]},{featureType:'landscape',elementType:'all',stylers:[{hue:'#eceae6'},{saturation:-49},{lightness:22},{visibility:'on'}]},{featureType:'poi.park',elementType:'all',stylers:[{hue:'#dddbd7'},{saturation:-81},{lightness:34},{visibility:'on'}]},{featureType:'poi.medical',elementType:'all',stylers:[{hue:'#dddbd7'},{saturation:-80},{lightness:-2},{visibility:'on'}]},{featureType:'poi.school',elementType:'all',stylers:[{hue:'#c8c6c3'},{saturation:-91},{lightness:-7},{visibility:'on'}]},{featureType:'landscape.natural',elementType:'all',stylers:[{hue:'#c8c6c3'},{saturation:-71},{lightness:-18},{visibility:'on'}]},{featureType:'road.highway',elementType:'all',stylers:[{hue:'#dddbd7'},{saturation:-92},{lightness:60},{visibility:'on'}]},{featureType:'poi',elementType:'all',stylers:[{hue:'#dddbd7'},{saturation:-81},{lightness:34},{visibility:'on'}]},{featureType:'road.arterial',elementType:'all',stylers:[{hue:'#dddbd7'},{saturation:-92},{lightness:37},{visibility:'on'}]},{featureType:'transit',elementType:'geometry',stylers:[{hue:'#c8c6c3'},{saturation:4},{lightness:10},{visibility:'on'}]}];
    var map = null;

    // Add instance methods
    ZonerMap.prototype = {

      /**
       * Initialize slider
       *
       * @returns {undefined}
       */
		init: function() 
		{ 
			var self = this;

			// Enable Geo Location on button click
	        $('.geo-location').on("click", function() 
	        {	        	
	        	self.showLoading('#btnGeoLocation');
	        	$('#btnGeoLocation').addClass('spin-loading');	        	
	            if (navigator.geolocation) 
	            {
	                navigator.geolocation.getCurrentPosition(function(position)
	                {
	                	self.filter.LatitudeBase  = position.coords.latitude;
	                	self.filter.LongitudeBase = position.coords.longitude;	        
	                	self.scope.$apply();
	                	self.hideLoading('#btnGeoLocation');
						self.renderInitialMap();
	                });
	            } 
	            else 
	            {
	                $rootScope.error = 'Geo Location is not supported';
	                self.hideLoading('#btnGeoLocation');
	            }	            
	        });		

	  		self.renderInitialMap();
			this.scope.$on('executeSearchEvent', function(){
				self.showLoading('#btnSearch')				
      		}); 

			this.scope.$watch('elementResultData', function(newValue, oldValue) 
	        {
	        	self.updateMap(newValue, self.filter);
	        });       		
		},		
		showLoading : function(dom)
		{			
			console.log('showLoading called');
			$(dom)
				.addClass('spin-loading')
				.attr('disabled', 'disabled');

			$('#map').addClass('fade-map');
				
			//$('body').removeClass('loaded');
			//$('body').addClass('has-fullscreen-map');
			//$('#map').addClass('fade-map');			
		},		
		hideLoading : function(dom)
		{
			console.log('hideLoading called');
			$(dom)
				.removeClass('spin-loading')
				.removeAttr('disabled')

			$('#map').removeClass('fade-map');				
			/*
			$('body').addClass('loaded');
            setTimeout(function() {
                $('body').removeClass('has-fullscreen-map');
            }, 1000);
            $('#map').removeClass('fade-map');			
            */
		},		
		centerSearchBox : function()
		{
			console.log('centerSearchBox SELF Called');
		    var $searchBox = $('.search-box-wrapper');
		    var $navigation = $('.navigation');
		    var positionFromBottom = 20;
		    if ($('body').hasClass('navigation-fixed-top'))
		    {
		        $('#map, #slider').css('margin-top', $navigation.height());
		        $searchBox.css('z-index',98);
		    } 
		    else 
		    {
		        $('.leaflet-map-pane').css('top',-50);
		        $(".homepage-slider").css('margin-top', -$('.navigation header').height());
		    }

		    if ($(window).width() > 768) 
		    {
		        $('#slider .slide .overlay').css('margin-bottom',$navigation.height());
		        $('#map, #slider').each(function () 
		        {
		            if (!$('body').hasClass('horizontal-search-float'))
		            {
		                var mapHeight = $(this).height();
		                var contentHeight = $('.search-box').height();
		                var top;
		                if($('body').hasClass('has-fullscreen-map')) {
		                    top = (mapHeight / 2) - (contentHeight / 2);
		                }
		                else {
		                    top = (mapHeight / 2) - (contentHeight / 2) + $('.navigation').height();
		                }
		                $('.search-box-wrapper').css('top', top);
		            } 
		            else 
		            {
		                $searchBox.css('top', $(this).height() + $navigation.height() - $searchBox.height() - positionFromBottom);
		                $('#slider .slide .overlay').css('margin-bottom',$navigation.height() + $searchBox.height() + positionFromBottom);
		                if ( $('body').hasClass('has-fullscreen-map') ) 
		                {
		                    //$('.search-box-wrapper').css('top', $(this).height() - $('.navigation').height());
		                }
		            }
		        });
		    }
		},
		
		setMapHeight : function() {
			console.log('setMapHeight() called')
		    var $body = $('body');
		    if($body.hasClass('has-fullscreen-map')) {
		        $('#map').height($(window).height() - $('.navigation').height());
		    }
		    $('#map').height($(window).height() - $('.navigation').height());
		    var mapHeight = $('#map').height();
		    var contentHeight = $('.search-box').height();
		    var top;
		    top = (mapHeight / 2) - (contentHeight / 2);
		    if( !$('body').hasClass('horizontal-search-float') ){
		        $('.search-box-wrapper').css('top', top);
		    }
		    if ($(window).width() < 768) {
		        $('#map').height($(window).height() - $('.navigation').height());
		    }
		},
		renderInitialMap : function()
		{
			var self = this;
			map = new google.maps.Map(document.getElementById('map'), {
	            zoom: 14,
	            scrollwheel: false,
	            center: new google.maps.LatLng(self.filter.LatitudeBase, self.filter.LongitudeBase),
	            mapTypeId: google.maps.MapTypeId.ROADMAP,
	            styles: mapStyles
	        });			

			self.setMapHeight();
			self.centerSearchBox();
			self.hideLoading('#btnSearch');	               		      					        
		},
		updateMap : function(result, filter)
		{
			var self = this;
 			if( document.getElementById('map') != null && result != null)
		    {	
				var circle = null;
	            var i;
	            var newMarkers = [];
				var locations = result;
	            map = new google.maps.Map(document.getElementById('map'), {
	                zoom: 14,
	                scrollwheel: false,
	                center: new google.maps.LatLng(filter.LatitudeBase, filter.LongitudeBase),
	                mapTypeId: google.maps.MapTypeId.ROADMAP,
	                styles: mapStyles
	            });

				//Criando a indicação do ponto base
				var intialPoint = new MarkerWithLabel({
			            icon: " ",
	                    title: 'Ponto Base',
	                    position: new google.maps.LatLng(filter.LatitudeBase, filter.LongitudeBase),
	                    map: map,
	                });            			

				if(filter.Radius > 0)
				{
					var zoomRatio = 0;
					if(filter.Radius <= 5 )
						zoomRatio = 14
					else if(filter.Radius <= 10 )
						zoomRatio = 13;							
					else if(filter.Radius <= 15 )
						zoomRatio = 12;	
					else if(filter.Radius <= 20 )
						zoomRatio = 11;

					map.setOptions({ zoom: zoomRatio });
	                circle = new google.maps.Circle({
	                    map: map,
	                    fillColor: "blue",
	                    strokeWeight: 1,
	                    strokeColor: "blue",
	                    radius: filter.Radius * 1000
	                });
	                intialPoint.setOptions({ zIndex: 0 });
	                circle.bindTo('center', intialPoint, 'position');
	                google.maps.event.addListener(circle, 'click', function () {
	                    //intialPoint.setOptions({ zIndex: 0 });
	                });
            	}
            	else
					map.setOptions({ zoom: 8 });

	            for (i = 0; i < locations.length; i++) 
	            {
	            	var location = 	locations[i][0];
	                var pictureLabel = document.createElement("img");
	                pictureLabel.src = location.iconPath;
	                var boxText = document.createElement("div");
	                var infoboxOptions = {
	                    content: boxText,
	                    disableAutoPan: false,
	                    //maxWidth: 150,
	                    pixelOffset: new google.maps.Size(-100, 0),
	                    zIndex: null,
	                    alignBottom: true,
	                    boxClass: "infobox-wrapper",
	                    enableEventPropagation: true,
	                    closeBoxMargin: "0px 0px -8px 0px",
	                    closeBoxURL: "assets/img/close-btn.png",
	                    infoBoxClearance: new google.maps.Size(1, 1)
	                };

	                var marker = new MarkerWithLabel({
	                    title: location.name,
	                    position: new google.maps.LatLng(location.latitude, location.longitude),
	                    map: map,
	                    icon: 'assets/img/marker.png',
	                    labelContent: pictureLabel,
	                    labelAnchor: new google.maps.Point(50, 0),
	                    labelClass: "marker-style"
	                });

	                //var price = $filter("currency")(location.price);
	                var price = location.price;
	                newMarkers.push(marker);
	                boxText.innerHTML =
	                    '<div class="infobox-inner">' +
	                        '<a href="#/property-detail?idElement=' + location.idElement +'"  ui-sref="property-detail({ idElement: ' + location.idElement + '})">' +
	                        '<div class="infobox-image" style="position: relative">' +
	                        '<img src="' + (location.defaultPicturePath.includes('http') ? location.defaultPicturePath : config.adminSite + location.defaultPicturePath) + '">' + '<div><span class="infobox-price">' + price + '</span></div>' +
	                        '</div>' +
	                        '</a>' +
	                        '<div class="infobox-description">' +
	                        '<div class="infobox-title"><a href="#/property-detail?idElement=' + location.idElement +'"  ui-sref="property-detail({ idElement: ' + location.idElement + '})">' + location.name + '</a></div>' +
	                        '<div class="infobox-location">' + location.cityName + ', ' + location.stateProvinceName + ', ' + location.countryName + '</div>' +
	                        '</div>' +
	                        '</div>';
				  	
	                //Define the infobox
	                newMarkers[i].infobox = new InfoBox(infoboxOptions);
	                google.maps.event.addListener(marker, 'click', (function(marker, i) 
	                {
	                    return function() 
	                    {
	                    	var h;
	                        for (h = 0; h < newMarkers.length; h++) {
	                            newMarkers[h].infobox.close();
	                        }
	                        newMarkers[i].infobox.open(map, this);
	                    }
	                })(marker, i));
	            }

	            var clusterStyles = [{url: 'assets/img/cluster.png',height: 37,width: 37}];
	            var markerCluster = new MarkerClusterer(map, newMarkers, {styles: clusterStyles, maxZoom: 15});		            

	            google.maps.event.addListener(markerCluster, 'click', function() {
	            	if( circle != null)
						circle.setMap(null);
	            });

	            //  Dynamically show/hide markers --------------------------------------------------------------

	            google.maps.event.addListener(map, 'idle', function() 
	            {
	                for (var i=0; i < locations.length; i++) {
	                    if ( map.getBounds().contains(newMarkers[i].getPosition()) )
	                    {
	                        // newMarkers[i].setVisible(true); // <- Uncomment this line to use dynamic displaying of markers
	                        //newMarkers[i].setMap(map);
	                        //markerCluster.setMap(map);
	                    } else {
	                        // newMarkers[i].setVisible(false); // <- Uncomment this line to use dynamic displaying of markers
	                        //newMarkers[i].setMap(null);
	                        //markerCluster.setMap(null);
	                    }
	                }
	            });

				self.setMapHeight();
  				self.centerSearchBox();
  				self.hideLoading('#btnSearch');	               		      				
		    }	
		}
	};      

    return ZonerMap;
  })

  .directive('zonerMap', function(ZonerMap) {
    'use strict';

    return {
      restrict: 'E',
      scope: {
         filterData: '=',
         elementResultData: '='
      },

      /**
       * Return template URL
       *
       * @param {jqLite} elem
       * @param {Object} attrs
       * @return {string}
       */
      templateUrl: function(elem, attrs) {
        //noinspection JSUnresolvedVariable
        return './modules/zonerMap/partials/template.html';
      },

      link: function(scope, elem) {
        return new ZonerMap(scope, elem);
      }
    };
  });


  return module
}));



  (function(root, factory) {
  'use strict';
  if (typeof define === 'function' && define.amd) {
    // AMD. Register as an anonymous module.
    define(['angular'], factory);
  } else if (typeof module === 'object' && module.exports) {
    // Node. Does not work with strict CommonJS, but
    // only CommonJS-like environments that support module.exports,
    // like Node.
    // to support bundler like browserify
    module.exports = factory(require('angular'));
  } else {
    // Browser globals (root is window)
    factory(root.angular);
  }

  }(this, function(angular) {
  'use strict';
  var module = angular.module('zonerPaginationModule', [])
  .factory('ZonerPagination', function( $rootScope, $document, $window, $compile, config) {
    'use strict';

    /**
     * ZonerPagination
     *
     * @param {ngScope} scope            The AngularJS scope
     * @param {Element} sliderElem The slider directive element wrapped in jqLite
     * @constructor
     */
    var ZonerPagination = function(scope, zonerPaginationElem) 
    {
      this.scope = scope;
      this.ZonerPaginationElem = zonerPaginationElem;
      //this.PageIndex  =  this.scope.pageIndex;
      //this.ArrayData  =  this.scope.array; 
      //this.TotalRecodsPerPage =  this.scope.totalRecodsPerPage;      
    };

    // Add instance methods
    ZonerPagination.prototype = 
    {
      
    };      

    return ZonerPagination;
  })
  .directive('zonerPagination', function(ZonerPagination) {
    'use strict';

    var controller = ['$scope', '$filter', function ($scope, $filter)    
    {     
      $scope.$watchCollection('sourceArray', function(newVal, oldValue) 
      {
        $scope.doPagging();
      });     

      $scope.$watch('orderBy', function(newVal, oldValue) 
      {
        
        if(newVal != null && newVal != "" && newVal != oldValue )
        {
          //console.log('orderBy changed:'  + newVal);
          //$scope.pageIndex = 0;
        }
        
      });     
      
      $scope.changePageIndex = function(index)
      {
          console.log('changePageIndex called');
          if(index < -1)
            $scope.pageIndex = 0;
          else
            $scope.pageIndex = index;

          $scope.doPagging();
      };  

      $scope.doPagging = function()
      {
        if($scope.sourceArray != [])
        {
          //Total de páginas a serem exibidas
          if($scope.totalPagesToShow == null)
            $scope.totalPagesToShow = 9;

          //flag para exibir >>
          $scope.showBefore = false;
          //flag para exibir <<
          $scope.showAfter = false;

          $scope.pages = [];          
          var recordCount = $scope.recordCount;
          var _end = $scope.totalPagesToShow;          
          var _start = 0;
          
          //Total de paginas
          $scope.totalPages = parseInt( recordCount / $scope.totalRecodsPerPage);
          if( recordCount % $scope.totalRecodsPerPage > 0)
             $scope.totalPages++;         

          if( $scope.totalPages > $scope.totalPagesToShow)
          {
            _end = $scope.pageIndex + _end; 
            if(_end > $scope.totalPages)
            {
                _end = $scope.totalPages;              
                _start = $scope.pageIndex - 1;
            }
            else
            {
              _start = $scope.pageIndex;            
            }

            if(_start > 0 && _end > $scope.totalPagesToShow)
              $scope.showBefore = true;

            if(!(_start + $scope.totalPagesToShow >= $scope.totalPages))
              $scope.showAfter = true;      
          }      
          else
          {
            _start = 0;
            _end = $scope.totalPages;

          }

          for (var i = _start; i < _end; i++) {
                $scope.pages.push( { index: i  } );
          };      

          //$scope.pagedArray = $filter('limitTo')($filter('orderBy')($scope.sourceArray, $scope.orderBy), $scope.totalRecodsPerPage, $scope.pageIndex * $scope.totalRecodsPerPage);
        }        
      }

    }];

    return {
      restrict: 'E',
      scope: {
         pageIndex: '=',
         totalPagesToShow: '=',
         recordCount: '=',
         totalRecodsPerPage: '=',
         pagedArray: '=',
         sourceArray:'=',
         orderBy: '='

      },
      controller: controller,

      templateUrl: function(elem, attrs) {
        return './modules/_components/zonerPagination/partials/template.html';
      },

      link: function(scope, elem) {
        return new ZonerPagination(scope, elem);
      }
    };
  });

  return module;
  }));
