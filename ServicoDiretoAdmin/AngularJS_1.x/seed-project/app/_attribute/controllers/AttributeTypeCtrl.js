'use strict';
//
angular.module('app.attribute')
	.controller('AttributeTypeListController', ['$scope', '$rootScope', 'AttributeTypeService', '$state', '$filter', 'toastr', 
        function ($scope, $rootScope, AttributeTypeService, $state, $filter, toastr) 
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
                    field: 'idAttributeType',
                },
                {
                    caption: 'Nome',
                    field: 'description',
                },
                {
                    caption: 'Sigla',
                    field: 'acronym',
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
			$state.go('app.attribute.type-create', { idAttributeType : row.idAttributeType });       
	    }		
        //
        $scope.search = function()
        {
            AttributeTypeService.List.query( { idCulture : moment.locale() } )
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
                        items.push(item.idAttributeType);
                    });

                    if(items.length > 0)
                    {
                        AttributeTypeService.Deactivate.execute( items )
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
	.controller('AttributeTypeCreateController', ['$scope', '$rootScope', '$state', '$stateParams', 'toastr', 'AttributeTypeService', 'IconService' ,
        function ($scope, $rootScope, $state, $stateParams, toastr, AttributeTypeService, IconService)  
	{	
        $scope.settings = {
          bootstrap2: false,
          filterClear: 'Exibir todos!',
          filterPlaceHolder: 'Filtrar!',
          moveSelectedLabel: 'Mover selecionados apenas!',
          moveAllLabel: 'Mover todos!',
          removeSelectedLabel: 'Remover selecionado apenas',
          removeAllLabel: 'Remover todos!',
          moveOnSelect: false,
          preserveSelection: 'moved',
          selectedListLabel: 'Atributos selecionados',
          nonSelectedListLabel: 'Atributos não selecionados',
          postfix: '_helperz',
          selectMinHeight: 130,
          filter: true,
          //filterNonSelected: '1',
          //filterSelected: '4',
          infoAll: 'Exibindo todos {0}!',
          infoFiltered: '<span class="label label-warning">Filtrado</span> {0} de {1}!',
          infoEmpty: 'Lista vazia!',
          filterValues: true
        };


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
            $state.go('app.attribute.type-list');
        };
        //
        $scope.new = function()
        {
            $state.go('app.attribute.type-create', {idAttributeType : ''});
        };
        //        
        $scope.load = function(_idAttributeType)
        {
            $scope.loadIcons();

            if(_idAttributeType == '')
                _idAttributeType = 0;

            AttributeTypeService.GetByID.query( { idAttributeType: _idAttributeType, idCulture :  moment.locale() } )
            .$promise 
                    .then(function (data)  
                    {
                        $scope.record = data;
                        //$scope.record.availableAttributes = [{"id":"1", "text":"valor1", "checked":true}];
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
            AttributeTypeService.Save.execute( $scope.record )
            .$promise 
                    .then(function (data)  
                    {
                        toastr.success(data.message, 'Confirmação!');                        
                        $state.go('app.attribute.type-list');
                    },
                    function (reason) {
                        $rootScope.error = reason;
                    });         
        };

        //
        $scope.load( $stateParams.idAttributeType );
	}]);
