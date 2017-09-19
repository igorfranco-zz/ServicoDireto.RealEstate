
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
