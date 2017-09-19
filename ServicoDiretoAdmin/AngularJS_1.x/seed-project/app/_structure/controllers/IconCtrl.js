'use strict';
//
angular.module('app.structure')
	.controller('IconListController', ['$scope', '$rootScope', 'IconService', '$state', '$filter', 'toastr', function ($scope, $rootScope, IconService, $state, $filter, toastr) 
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
                    field: 'idIcon',
                },
                {
                    caption: 'Nome',
                    field: 'name',
                },        
                {
                    caption: 'Caminho',
                    field: 'path',
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
			$state.go('app.structure.icon-create', { idIcon : row.idIcon });       
	    }		
        //
        $scope.search = function()
        {
            IconService.List.query( $scope.paging )
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
                        items.push(item.idIcon);
                    });

                    if(items.length > 0)
                    {
                        IconService.Deactivate.execute( items )
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
	.controller('IconCreateController', ['$scope', '$rootScope', '$state', '$stateParams', 'toastr', 'IconService', 
        function ($scope, $rootScope, $state, $stateParams, toastr, IconService)  
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
            $state.go('app.structure.icon-list');
        };
        //
        $scope.new = function()
        {
            $state.go('app.structure.icon-create', {idIcon : ''});
        };
        //        
        $scope.load = function(_idIcon)
        {
            if(_idIcon != '')
            {
                IconService.GetByID.query( { idIcon: _idIcon } )
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
        
        $scope.save = function()
        {
            IconService.Save.execute( $scope.record )
            .$promise 
                    .then(function (data)  
                    {
                        toastr.success(data.message, 'Confirmação!');                        
                        $state.go('app.structure.icon-list');
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });         
        };

        //
        $scope.load( $stateParams.idIcon );
	}]);
