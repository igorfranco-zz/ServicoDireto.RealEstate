angular.module('MainApp')
    .controller('MainController', ['$scope', 'config', function ($scope, config) 
    {
    	$scope.config = config;	
    }]);