angular.module("ComponentApp")
    .directive("zonerFilter", ['$filter', function ($filter) {
        return {
          link: function($scope, el, attrs) 
          {
            $scope.radiusSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minRadius,
                      ceil: $scope.options.maxRadius,
                      translate: function(value) {
                        return "Raio: " + value + " Km"
                      }
                    }
                  };                  
            //      
            $scope.priceSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minPrice,
                      ceil: $scope.options.maxPrice,
                      step: $scope.options.priceStep,
                      translate: function(value) 
                      {
                        if(value == $scope.options.maxPrice)
                          return $filter("currency")(value) + ' ou mais';
                        else
                          return $filter("currency")(value);
                      }
                    }
                  }; 
            //      
            $scope.roomSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minRooms,
                      ceil: $scope.options.maxRooms,
                      translate: function(value) 
                      {
                        if(value == $scope.options.maxRooms)
                          return value + " ou mais";
                        else
                          return value + " Quartos(s)";
                      }
                    }
                  }; 
            //                   
            $scope.garageSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minGarage,
                      ceil: $scope.options.maxGarage,
                      translate: function(value) {
                        if(value == $scope.options.maxGarage)
                          return value + " ou mais"
                        else
                          return value + " Garagem(ns)"

                      }
                    }
                  };             
            //
            $scope.areaSlider = {
                    options: 
                    {
                      hideLimitLabels:true,
                      floor: $scope.options.minArea,
                      ceil: $scope.options.maxArea,
                      translate: function(value) {
                        if(value == $scope.options.maxArea)
                          return $filter("number")(value) + " m² ou mais"
                        else
                          return "Área " + $filter("currency")(value) + " m²"

                      }
                    }
                  };             
            //        
            $scope.filter.radius   = $scope.options.minRadius;   

            $scope.filter.minRooms = $scope.options.minRooms;
            $scope.filter.maxRooms = $scope.options.maxRooms;

            $scope.filter.minPrice = $scope.options.minPrice;
            $scope.filter.maxPrice = $scope.options.maxPrice;

            $scope.filter.minGarage = $scope.options.minGarage;
            $scope.filter.maxGarage = $scope.options.maxGarage;

            $scope.filter.minArea = $scope.options.minArea;
            $scope.filter.maxArea = $scope.options.maxArea;

            $scope.filter.defaultvalues = $scope.options;
          },          
          controller: ['$scope', '$rootScope', function ($scope, $rootScope){

              $scope.$watch('filter', function(oldValue, newValue){
                if(oldValue.pageIndex != newValue.pageIndex || oldValue.orderBy != newValue.orderBy ) 
                {
                  //$rootScope.$broadcast("filterRequested", $scope.filter ); 
                  $rootScope.$broadcast("filterRequested"); 
                }
              }, true);

              
              $scope.doFilter = function()
              {
                  console.log("filterRequested")
                  $rootScope.$broadcast("filterRequested");
                /*
                  $scope.filter.pageIndex = 0;
                  $scope.filter.FilterAttribute = [];
                  $scope.filter.IDPurpose = [];     
                  if($scope.filter.singleIDPurpose != null)
                    $scope.filter.IDPurpose.push($scope.filter.singleIDPurpose);      

                  $scope.filter.FilterAttribute.push
                  ( 
                    {
                      IDAttribute : 5, 
                      InitialValue : $scope.filter.minPrice,
                      FinalValue : $scope.filter.maxPrice 
                    }
                  );

                  $rootScope.$broadcast("filterRequested", $scope.filter ); 
                  */
              };            
          }],
          templateUrl: './modules/_components/zonerFilter/partials/template.html',
          restrict: "E",
          replace : true,
          scope: 
          {
            options: '=',
            filter: '=' //usar o mesmo do contexto
          }          
        };
      }])