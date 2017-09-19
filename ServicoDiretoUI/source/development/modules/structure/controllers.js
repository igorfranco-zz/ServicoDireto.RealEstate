 
angular.module('StructureApp')
    .controller('StructureController', ['$scope', '$location', 'StructureService', function ($scope, $location, StructureService) {   	
        //--------------------------------Events Handlers--------------------------------
        $scope.$on("elementLoaded", function (event, args) 
        {
            console.log('StructureController ---> elementLoaded Handled'); 
            $scope.listPurpose();
            if(args.idPurpose != null)
                $scope.listCategory(args.idPurpose);

            if(args.idHierarchyStructureParent != null)
                $scope.listType (args.idHierarchyStructureParent);
        });     
        //
         $scope.initialize = function()
        {
            $scope.purposes = [];
            $scope.categories = [];        
            $scope.types = [];        
        };
        //
		$scope.listPurpose = function ()
        {
            $scope.categories = [];        
            $scope.types = [];        

            StructureService.ListPurpose.query( { idCulture : moment.locale() })
             .$promise
                    .then(function (item) 
                    {
                        $scope.purposes = item;                        
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };		
        //
        $scope.listCategory = function (_idPurpose)
        {
            $scope.categories = [];        
            $scope.types = [];        
            if(_idPurpose != null)
            {
                //$scope.filter.singleIDPurpose
                StructureService.ListCategory.query( { idPurpose : _idPurpose, idCulture : moment.locale() })
                 .$promise
                        .then(function (item) 
                        {
                            $scope.categories = item;                        
                        },
                        function (reason) {
                            $scope.error = reason;
                        });
            }
        };  
        //
        $scope.listType = function (_idCategory)
        {
            $scope.types = [];        
            if(_idCategory != null)
            {
                //$scope.filter.IDHierarchyStructureParent
                StructureService.ListType.query( { idCategory : _idCategory, idCulture : moment.locale() })
                 .$promise
                        .then(function (item) 
                        {
                            $scope.types = item;                        
                        },
                        function (reason) {
                            $scope.error = reason;
                        });
            }                    
        };  
    }]);
