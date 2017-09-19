

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
  var module = angular.module('zonerPaginationModule', [])
  .factory('ZonerPagination', function( $rootScope, $document, $window, $compile, config) {
    'use strict';

    /**
     * ZonerPagination
     *
     * @param {ngScope} scope            The AngularJS scope
     * @param {Element} sliderElem The slider directive element wrapped in jqLite
     * @constructor
     */
    var ZonerPagination = function(scope, zonerPaginationElem) 
    {
      this.scope = scope;
      this.ZonerPaginationElem = zonerPaginationElem;
      //this.PageIndex  =  this.scope.pageIndex;
      //this.ArrayData  =  this.scope.array; 
      //this.TotalRecodsPerPage =  this.scope.totalRecodsPerPage;      
    };

    // Add instance methods
    ZonerPagination.prototype = 
    {
      
    };      

    return ZonerPagination;
  })
  .directive('zonerPagination', function(ZonerPagination) {
    'use strict';

    var controller = ['$scope', '$filter', function ($scope, $filter)    
    {     
      $scope.$watchCollection('sourceArray', function(newVal, oldValue) 
      {
        $scope.doPagging();
      });     

      $scope.$watch('orderBy', function(newVal, oldValue) 
      {
        
        if(newVal != null && newVal != "" && newVal != oldValue )
        {
          //console.log('orderBy changed:'  + newVal);
          //$scope.pageIndex = 0;
        }
        
      });     
      
      $scope.changePageIndex = function(index)
      {
          console.log('changePageIndex called');
          if(index < -1)
            $scope.pageIndex = 0;
          else
            $scope.pageIndex = index;

          $scope.doPagging();
      };  

      $scope.doPagging = function()
      {
        if($scope.sourceArray != [])
        {
          //Total de páginas a serem exibidas
          if($scope.totalPagesToShow == null)
            $scope.totalPagesToShow = 9;

          //flag para exibir >>
          $scope.showBefore = false;
          //flag para exibir <<
          $scope.showAfter = false;

          $scope.pages = [];          
          var recordCount = $scope.recordCount;
          var _end = $scope.totalPagesToShow;          
          var _start = 0;
          
          //Total de paginas
          $scope.totalPages = parseInt( recordCount / $scope.totalRecodsPerPage);
          if( recordCount % $scope.totalRecodsPerPage > 0)
             $scope.totalPages++;         

          if( $scope.totalPages > $scope.totalPagesToShow)
          {
            _end = $scope.pageIndex + _end; 
            if(_end > $scope.totalPages)
            {
                _end = $scope.totalPages;              
                _start = $scope.pageIndex - 1;
            }
            else
            {
              _start = $scope.pageIndex;            
            }

            if(_start > 0 && _end > $scope.totalPagesToShow)
              $scope.showBefore = true;

            if(!(_start + $scope.totalPagesToShow >= $scope.totalPages))
              $scope.showAfter = true;      
          }      
          else
          {
            _start = 0;
            _end = $scope.totalPages;

          }

          for (var i = _start; i < _end; i++) {
                $scope.pages.push( { index: i  } );
          };      

          //$scope.pagedArray = $filter('limitTo')($filter('orderBy')($scope.sourceArray, $scope.orderBy), $scope.totalRecodsPerPage, $scope.pageIndex * $scope.totalRecodsPerPage);
        }        
      }

    }];

    return {
      restrict: 'E',
      scope: {
         pageIndex: '=',
         totalPagesToShow: '=',
         recordCount: '=',
         totalRecodsPerPage: '=',
         pagedArray: '=',
         sourceArray:'=',
         orderBy: '='

      },
      controller: controller,

      templateUrl: function(elem, attrs) {
        return './modules/_components/zonerPagination/partials/template.html';
      },

      link: function(scope, elem) {
        return new ZonerPagination(scope, elem);
      }
    };
  });

  return module;
  }));
