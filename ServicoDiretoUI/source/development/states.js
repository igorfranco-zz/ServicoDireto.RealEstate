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