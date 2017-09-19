
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