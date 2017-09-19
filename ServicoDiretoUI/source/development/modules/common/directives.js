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



