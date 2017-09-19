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
