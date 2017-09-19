(function () {
	'use strict';
	angular
		.module('angular-owl-carousel', [])
		.directive('owlCarousel', [
			'$parse',
			owlCarouselDirective
		]);

	function owlCarouselDirective($parse) {
		var owlOptions = [	
			'onChanged',
			'onInitialized',
			'animateOut',
			'loop',
			'autoplayTimeout',
			'responsiveClass',
			'responsiveBaseElement',
			'autoHeight',
			'dots',
			'margin',
			'rtl',
			'items',
			'itemsDesktop',
			'itemsDesktopSmall',
			'itemsTablet',
			'itemsTabletSmall',
			'itemsMobile',
			'itemsCustom',
			'singleItem',
			'itemsScaleUp',
			'slideSpeed',
			'paginationSpeed',
			'rewindSpeed',
			'autoPlay',
			'stopOnHover',
			'navigation',
			'navigationText',
			'nav',
			'navText',
			
			'rewindNav',
			'scrollPerPage',
			'pagination',
			'paginationNumbers',
			'responsive',
			'responsiveRefreshRate',
			'responsiveBaseWidth',
			'baseClass',
			'theme',
			'lazyLoad',
			'lazyFollow',
			'lazyEffect',
			'autoHeight',
			'jsonPath',
			'jsonSuccess',
			'dragBeforeAnimFinish',
			'mouseDrag',
			'touchDrag',
			'addClassActive',
			'transitionStyle',
			// Callbacks
			'beforeUpdate',
			'afterUpdate',
			'beforeInit',
			'afterInit',
			'beforeMove',
			'afterMove',
			'afterAction',
			'startDragging',
			'afterLazyLoad'
		];

		return {
			restrict: 'A',
			transclude: true,
			link: function (scope, element, attributes, controller, $transclude) {

				scope.owlCarousel = {};

				var options = {},
					id = attributes.id || 1,
					$element = $(element),
					propertyName = attributes.owlCarousel;

				for (var i = 0; i < owlOptions.length; i++) {
					var opt = owlOptions[i];
					if (attributes[opt] !== undefined) {
						options[opt] = $parse(attributes[opt])();
					}
				}

				scope.$watchCollection(propertyName, function (newItems, oldItems) {
					if (scope.owlCarousel[id]) {
						scope.owlCarousel[id].destroy();
					}
					$element.empty();

					for (var i in newItems) {
						$transclude(function (clone, scope) {
							scope.item = newItems[i];
							$element.append(clone[1]);
						});
					}


					$element.owlCarousel(options);
					scope.owlCarousel[id] = $element.data('owlCarousel');
				});
			}
		};
	}

})();
