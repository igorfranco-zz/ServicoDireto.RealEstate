
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
  var module = angular.module('zonerFilterModule', [])
  .factory('ZonerFilter', function( $rootScope, $document, $window, $compile, config) {
    'use strict';

    /**
     * ZonerFilter
     *
     * @param {ngScope} scope            The AngularJS scope
     * @param {Element} sliderElem The slider directive element wrapped in jqLite
     * @constructor
     */
    var ZonerFilter = function(scope, zonerFilterElem) 
    {
      this.scope = scope;
      this.ZonerFilterElem = zonerFilterElem;
      //this.PageIndex  =  this.scope.pageIndex;
      //this.ArrayData  =  this.scope.array; 
      //this.TotalRecodsPerPage =  this.scope.totalRecodsPerPage;      

      this.radiusSlider = null;
    };

    // Add instance methods
    ZonerFilter.prototype = 
    {
      init : function()
      {
        var  self = this;
      }  
      
    };      

    return ZonerFilter;
  })
  .directive('zonerFilter', function(ZonerFilter) {
    'use strict';

    var controller = ['$scope', '$filter', function ($scope, $filter)    
    {     
      $scope.$watchCollection('sourceArray', function(newVal, oldValue) 
      {
        
      });           
    }];

    return {
      restrict: 'E',
      scope: {         
         filter : '='

      },
      controller: controller,

      templateUrl: function(elem, attrs) {
        return './modules/_components/zonerFilter/partials/template.html';
      },

      link: function(scope, elem) {
        return new ZonerFilter(scope, elem);
      }
    };
  });

  return module;
  }));
