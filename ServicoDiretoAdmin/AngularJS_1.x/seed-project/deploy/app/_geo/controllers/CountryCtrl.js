'use strict';
//
angular.module('app.geo')
	.controller('CountryListController', ['$scope', '$rootScope', 'CountryService', '$state', '$filter', 'toastr', function ($scope, $rootScope, CountryService, $state, $filter, toastr) 
	{
        $scope.paging = 
        {
            page:1, //indice da pagina atual   
            maximumRows:20,  //numero de registros a serem exibido por página
            maximumRange:10, //total de itens a serem exibidos na paginação
            recordCount:0, //numero de registros retornados
            orderBy:"", //ordenar por 
            totalPages:0, //numero de páginas 
            display:[], //coleção de items a serem exibidos na paginação
            createDisplay:true
        }

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
                    field: 'idCountry',
                },
                {
                    caption: 'Nome',
                    field: 'name',
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

        $scope.$on("$doSearch", function (event, args) 
        {
            $scope.paging.createDisplay = args;
            $scope.search();
        });
        //
	    $scope.editRow = function(row){	    	
			$state.go('app.geo.country-create', { idCountry : row.idCountry });       
	    }		
        //
        $scope.search = function()
        {
            CountryService.List.query( $scope.paging )
            .$promise 
                    .then(function (data)  
                    {
                        $scope.paging.recordCount = data.recordCount;
                        $scope.paging.totalPages = data.recordCount / $scope.paging.maximumRows;                        
                        if(data.recordCount % $scope.paging.maximumRows > 0)
                            $scope.paging.totalPages++;
                        
                        $scope.gridConfig.data = data.records;
                        //    
                        $rootScope.$broadcast("$searchCompleted");
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
                        items.push(item.idCountry);
                    });

                    if(items.length > 0)
                    {
                        CountryService.Deactivate.execute( items )
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
	}])
	.controller('CountryCreateController', ['$scope', '$rootScope', '$state', '$stateParams', 'toastr', 'CountryService', function ($scope, $rootScope, $state, $stateParams, toastr, CountryService)  
	{	
        //
        $scope.status = [
            { key: 0, value:"Inativo" },
            { key: 1, value:"Ativo" }           
        ];
        //            
        $scope.record = null;        
        $scope.cancel = function()
        {
            $state.go('app.geo.country-list');
        };

        $scope.new = function()
        {
            $state.go('app.geo.country-create', { idCountry : '' });
        };

        $scope.load = function(_idCountry)
        {
            if(_idCountry != '')
            {
                CountryService.GetByID.query( { idCountry: _idCountry } )
                .$promise 
                        .then(function (data)  
                        {
                            $scope.record = data;
                        },
                        function (reason) {
                            $rootScope.error = reason;
                        });                             
            } 
            else
            {
                $scope.record = {status : 1}
            }
        };            

        //    
        $scope.save = function()
        {
            CountryService.Save.execute( $scope.record )
            .$promise 
                    .then(function (data)  
                    {
                        toastr.success(data.message, 'Confirmação!');                        
                        $state.go('app.geo.country-list');
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });         
        };
        //
        $scope.load( $stateParams.idCountry );
	}]);
