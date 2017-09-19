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
