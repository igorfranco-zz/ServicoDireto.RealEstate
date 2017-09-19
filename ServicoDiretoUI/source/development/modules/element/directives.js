
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

