
angular.module('LocationApp')
    .controller('LocationController', ['$rootScope', '$scope', '$location', 'LocationService', function ($rootScope, $scope, $location, LocationService) {
        //--------------------------------Events Handlers--------------------------------
        $scope.$on("elementLoaded", function (event, args) 
        {
            $scope.listCountry();
            $scope.listStateProvince(args.idCountry);
            $scope.listCity (args.idCountry, args.idStateProvince);           
        });     
        //
        $scope.initialize = function()
        {
            console.log('LocationController.initialize called');
            $scope.countries = [];
            $scope.stateProvinces = [];
            $scope.cities = [];
        }    	
        //
		$scope.listCountry = function ()
        {
            $scope.stateProvinces = [];
            $scope.cities = [];

            LocationService.ListCountry.query()
             .$promise
                    .then(function (item) 
                    {
                        $scope.countries = item;                        
                    },
                    function (reason) {
                        console.log(reason);
                        $rootScope.error = reason;
                    });
        };		
        //                
        $scope.listStateProvince = function (_idCountry)
        {   
            console.log('listStateProvince called');
            $scope.stateProvinces = [];
            $scope.cities = [];

            if(_idCountry != null)
            {
                LocationService.ListStateProvince.query( {idCountry : _idCountry } )
                 .$promise
                        .then(function (item) 
                        {
                            $scope.stateProvinces = item;                        
                        },
                        function (reason) {
                            $rootScope.error = reason;
                        });
            }        
        };      
        //            
        $scope.listCity = function (_idCountry, _idStateProvince)
        {   
            console.log('listCity called');
            $scope.cities = [];

            if(_idCountry != null && _idStateProvince != null)
            {
                LocationService.ListCity.query( {idCountry :_idCountry, idStateProvince : _idStateProvince } )
                 .$promise
                        .then(function (item) 
                        {
                            $scope.cities = item;                        
                        },
                        function (reason) {
                            $rootScope.error = reason;
                        });
            }        
        };    
    }]);