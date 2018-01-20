'use strict';
//
angular.module('app.geo')
	.controller('StateProvinceListController', ['$scope', '$rootScope', 'StateProvinceService', '$state', '$filter', 'toastr', function ($scope, $rootScope, StateProvinceService, $state, $filter, toastr) 
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
                    field: 'idStateProvince',
                },
                {
                    caption: 'País',
                    field: 'countryName',
                },        
                {
                    caption: 'Nome',
                    field: 'name',
                },
                {
                    caption: 'Acronym',
                    field: 'acronym',
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

        $scope.$on("$doSearch", function (event, args) 
        {
            $scope.paging.createDisplay = args;
            $scope.search();
        });
		
        //
	    $scope.editRow = function(row){	    	
			$state.go('app.geo.stateprovince-create', { idStateProvince : row.idStateProvince });       
	    }		
        //
        $scope.search = function()
        {
            StateProvinceService.List.query( $scope.paging )
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
                        items.push(item.idStateProvince);
                    });

                    if(items.length > 0)
                    {
                        StateProvinceService.Deactivate.execute( items )
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
	.controller('StateProvinceCreateController', ['$scope', '$rootScope', '$state', '$stateParams', 'toastr', 'StateProvinceService', 'CountryService', 
        function ($scope, $rootScope, $state, $stateParams, toastr, StateProvinceService, CountryService)  
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
            $state.go('app.geo.stateprovince-list');
        };
        //
        $scope.new = function()
        {
            $state.go('app.geo.stateprovince-create', {idStateProvince : ''});
        };
        //        
        $scope.load = function(_idStateProvince)
        {
            //carregando os países
            $scope.loadCountries();
            if(_idStateProvince != '')
            {
                StateProvinceService.GetByID.query( { idStateProvince: _idStateProvince } )
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
        //
        $scope.loadCountries = function()
        {
            CountryService.List.query( {page : 0 })
                .$promise 
                    .then(function (data)  
                    {
                        $scope.country = data.records;
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });                             
        }
        //
        $scope.save = function()
        {
            StateProvinceService.Save.execute( $scope.record )
            .$promise 
                    .then(function (data)  
                    {
                        toastr.success(data.message, 'Confirmação!');                        
                        $state.go('app.geo.stateprovince-list');
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });         
        };

        //
        $scope.load( $stateParams.idStateProvince );
	}]);
