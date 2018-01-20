'use strict';
//
angular.module('app.attribute')
	.controller('AttributeListController', ['$scope', '$rootScope', 'AttributeService', '$state', '$filter', 'toastr', 
        function ($scope, $rootScope, AttributeService, $state, $filter, toastr) 
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
            createDisplay:true,
            idCulture : moment.locale()
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
                    field: 'idAttribute',
                },
                {
                    caption: 'Nome',
                    field: 'name',
                },
                {
                    caption: 'Grupo',
                    field: 'group',
                },                
                {
                    caption: 'Sigla',
                    field: 'acronym',
                },                
                {
                    caption: 'Indice',
                    field: 'index',
                },                
                {
                    caption: 'Máscara exib.',
                    field: 'displayMask',
                },                                                
                {
                    caption: 'Máscara edit',
                    field: 'editMask',
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
        $scope.$on("$doSearch", function (event, args) 
        {
            $scope.paging.createDisplay = args;
            $scope.search();
        });        
        //
	    $scope.editRow = function(row){	    	
			$state.go('app.attribute.attribute-create', { idAttribute : row.idAttribute });       
	    }		
        //
        $scope.search = function()
        {
            AttributeService.List.query( $scope.paging )
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
                        items.push(item.idAttribute);
                    });

                    if(items.length > 0)
                    {
                        AttributeService.Deactivate.execute( items )
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
	.controller('AttributeCreateController', ['$scope', '$rootScope', '$state', '$stateParams', 'toastr', 'AttributeService', 'IconService' ,
        function ($scope, $rootScope, $state, $stateParams, toastr, AttributeService, IconService)  
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
            $state.go('app.attribute.attribute-list');
        };
        //
        $scope.new = function()
        {
            $state.go('app.attribute.attribute-create', {idAttribute : ''});
        };
        //        
        $scope.load = function(_idAttribute)
        {
            $scope.loadIcons();

            if(_idAttribute == '')
                _idAttribute = 0;

            AttributeService.GetByID.query( { idAttribute: _idAttribute } )
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
        
        $scope.save = function()
        {
            AttributeService.Save.execute( $scope.record )
            .$promise 
                    .then(function (data)  
                    {
                        toastr.success(data.message, 'Confirmação!');                        
                        $state.go('app.attribute.attribute-list');
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });         
        };

        //
        $scope.load( $stateParams.idAttribute );
	}]);
