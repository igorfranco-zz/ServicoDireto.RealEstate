'use strict';
//
angular.module('app.hierarchy')
	.controller('PurposeListController', ['$scope', '$rootScope', 'PurposeService', '$state', '$filter', 'toastr', 
        function ($scope, $rootScope, PurposeService, $state, $filter, toastr) 
	{
        $scope.gridConfig = 
        {
            data: [ ],
            colModel: [
                {
                    caption: '',
                    checkbox: true,
                    hide: true,
                    field:"checked"
                },              
                {
                    caption: '',
                    edit: true,
                    hide: true
                },              
                {
                    caption: 'ID',
                    field: 'idPurpose',
                },
                {
                    caption: 'Nome',
                    field: 'description',
                },
                {
                    caption: 'Ícone',
                    field: 'iconName',
                },                
                {
                    caption: 'Ícone Caminho',
                    field: 'iconPath',
                },                                
                {
                    caption: 'Status',
                    field: 'status',
                    sortable: true
                },
                {
                    caption: 'Data Criação',
                    field: 'createDate',
                },
                {
                    caption: 'Criado por',
                    field: 'createdBy',
                },
                {
                    caption: 'Data alteração',
                    field: 'modifyDate',
                },
                {
                    caption: 'Alterado por',
                    field: 'modifiedBy',
                }
            ]
        };
        //
	    $scope.editRow = function(row){	    	
			$state.go('app.hierarchy.purpose-create', { idPurpose : row.idPurpose, idPurpose : row.idPurpose });       
	    }		
        //
        $scope.new = function()
        {
            $state.go('app.hierarchy.purpose-create', {idPurpose : 0, idCulture : moment.locale()});
        };        
        //
        $scope.search = function()
        {
            PurposeService.List.query( { idCulture : moment.locale() } )
            .$promise 
                .then(function (data)  
                {
                    $scope.gridConfig.data = data                        
                },
                function (reason) {
                    $rootScope.error = reason;
                });         
        };
        //
        $scope.deactivate = function()
        {
            $.SmartMessageBox({
                title: "Inativar!",
                content: "Deseja realmente desativar estes registros?",
                buttons: '[Não][Sim]'
            }, function (ButtonPressed) {
                if (ButtonPressed === "Sim") 
                {
                    var items = [];
                    angular.forEach($filter('filter')($scope.gridConfig.data, { checked:true }), function(item){
                        items.push(item.idPurpose);
                    });

                    if(items.length > 0)
                    {
                        PurposeService.Deactivate.execute( items )
                        .$promise 
                            .then(function (data)  
                            {
                                toastr.success(data.message, 'Confirmação!'); 
                                $scope.search();
                            },
                            function (reason) {
                                $rootScope.error = reason;
                            });                             
                    }
                    else
                    {
                        //TODO: mensagem indicando que nao foram selecionados        
                    }
                }                
            });            
        }
        //
        $scope.search();
	}])
	.controller('PurposeCreateController', ['$scope', '$rootScope', '$state', '$stateParams', 'toastr', 'PurposeService', 'IconService' ,
        function ($scope, $rootScope, $state, $stateParams, toastr, PurposeService, IconService)  
	{	
        //
        $scope.status = [
            { key: 0, value:"Inativo" },
            { key: 1, value:"Ativo" }           
        ];
        //            
        $scope.record = null;        
        $scope.cultures = null;
        $scope.icons = null;

        $scope.cancel = function()
        {
            $state.go('app.hierarchy.purpose-list');
        };
        //
        $scope.new = function()
        {
            $state.go('app.hierarchy.purpose-create', {idPurpose : 0, idCulture : moment.locale()});
        };
        //        
        $scope.load = function(_idPurpose, _idCulture)
        {
            $scope.loadIcons();

            if(_idPurpose == '')
                _idPurpose = 0;

            PurposeService.GetByID.query( { idCulture: _idCulture, idPurpose: _idPurpose } )
            .$promise 
                    .then(function (data)  
                    {
                        $scope.record = data;
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });                                             
        };            
        
        $scope.loadIcons = function()
        {
            IconService.ListActive.query( )
            .$promise 
                    .then(function (data)  
                    {
                        $scope.icons = data;
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });                             
        };    

        /*
        $scope.loadCulture = function(_idPurpose)
        {
            PurposeService.ListCulture.query( { idPurpose: _idPurpose })
            .$promise 
                    .then(function (data)  
                    {
                        $scope.cultures = data;
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });                             

        };*/                 

        $scope.save = function()
        {
            PurposeService.Save.execute( $scope.record )
            .$promise 
                    .then(function (data)  
                    {
                        toastr.success(data.message, 'Confirmação!');                        
                        $state.go('app.hierarchy.purpose-list');
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });         
        };

        //
        $scope.load( $stateParams.idPurpose, $stateParams.idCulture );
	}]);
