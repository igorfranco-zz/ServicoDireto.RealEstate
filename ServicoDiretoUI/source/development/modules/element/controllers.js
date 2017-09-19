angular.module('ElementApp')
    .controller('ElementController', ['$timeout', '$rootScope', '$scope', '$filter', '$state', '$window', 'ElementService', 'StructureService', 'toastr', '$uibModal', function ($timeout, $rootScope, $scope, $filter, $state, $window, ElementService, StructureService, toastr, $uibModal) 
    {   
    	var _switchView = function (view)
    	{
			if(view == 1) //Listagem
				$state.go('properties-listing-lines');
			else //Mapa
				$state.go('properties-map');
		};

    	$scope.initialize = function()
    	{
	        $scope.purposes = [];        
			$rootScope.filter = null;
			$scope.elementResultPaged=[];
	    	$scope.elementResult = [];			    	
	    	$scope.elementFeatured = [];
	    	$scope.listTopViewed = [];    	
			$scope.showSearchEngine = true;
			$scope.templates = [{ name: 'featured', url: 'modules/element/views/featured.html' } ];
	    	$rootScope.filter = 
	    	{   
	    		/*
				minArea : 0,
	    		maxArea: 10000, 			    		
	    		minGarage : 1,
	    		maxGarage: 4, 
	    		minRooms : 1,
	    		maxRooms: 10, 		
	    		minPrice : 0,
	    		maxPrice : 2000000,
	    		singleIDPurpose : '',
	    		TotalRecords : 50,
				IDElement : "",
				IDCustomer : "0",
	            ActiveView : 2	, //usado para indicar qual o resultado se map ou listagem 1 usar mapa
	            BaseAddress: "",
	            FilterAttribute: [],
	            IDCity: "",
	            IDCountry: "",
	            IDHierarchyStructure: "",
	            IDHierarchyStructureParent: "",
	            IDPurpose: [],
	            IDStateProvince: '',
	            LatitudeBase:  '-30.02000000',
	            LongitudeBase: '-51.11000000',
	            OrderBy: "",
	            Radius: 0,
	            TotalRecodsPerPage : 10,
	            PageIndex : 0
	            */
			};
			$scope.listPurpose();
    	};


    	//
		$scope.changeFilterPurpose = function(idPurpose)
		{
			$rootScope.filter.singleIDPurpose = idPurpose;			
		}
		//
		$scope.doFilterListing = function(view)
		{
			$rootScope.filter.ActiveView = view;		
			try
			{
				//verificar o endereço informado;			
				var geocoder = new google.maps.Geocoder();
	            geocoder.geocode({ 'address': $rootScope.filter.BaseAddress }, function (results, status) 
	            {
	                if (status == google.maps.GeocoderStatus.OK) 
	                {
	                    if (results.length > 0) 
	                    {
	                        if (results.length > 1) 
	                        {
	                        	console.log(results);
								var modalInstance = $uibModal.open({
							      templateUrl: 'select-address-template.html',
							      controller: 'AddressSelectController',
							      size: 20,			      
							      resolve: 
							      {
							      	addresses: function () {
							          return results;
							        },			      						        
							    }});

								modalInstance.result.then(function (response) 
								{
									$rootScope.filter.LatitudeBase 	= response.address.geometry.location.lat()
									$rootScope.filter.LongitudeBase = response.address.geometry.location.lng()
									$rootScope.filter.BaseAddress 	= response.address.formatted_address;
									_switchView(view);
								}, 
								function () {
									console.log('Modal dismissed at: ' + new Date());
								});
	                        }
	                        else 
	                        {
	                        	$rootScope.filter.LatitudeBase = results[0].geometry.location.lat()
								$rootScope.filter.LongitudeBase = results[0].geometry.location.lng()
								$rootScope.filter.BaseAddress = results[0].formatted_address;
								_switchView(view);
	                        }
	                    }
						else 
		                {
		                	toastr.warning("Endereço não foi encontrado!", 'Informação');
		                }                    
	                }
	                else 
	                {
	                	toastr.warning("Endereço não foi encontrado!", 'Informação');
	                }
	            });   
			}			
			catch(err)
			{
				toastr.error("API Google não carregada.", 'Informação');
			}
		}
		//
        $scope.getTemplateUrl = function (name) {
            for (var i = 0; i < $scope.templates.length; i++) {
                if ($scope.templates[i].name == name)
                    return $scope.templates[i].url;
            }
        };
		//
		$scope.switchSearchEngine = function() {$scope.showSearchEngine = !$scope.showSearchEngine};
		//
        $scope.listFeatured = function (_idCustomer) 
        {
        	if(_idCustomer == null)
        		_idCustomer = 0;

			ElementService.ListFeatured.query(  { idCustomer : _idCustomer,  idCulture:  moment.locale() } )
			.$promise 
					.then(function (data)  
					{
	                    $scope.elementFeatured = data;  
	                    setCarouselWidth();
	                },
	                function (reason) {
	                    $rootScope.error = reason;
	                });			
        };
    	//
        $scope.listTopViewed =  function (_idCustomer) 
        {
        	if(_idCustomer == null)
        		_idCustomer = 0;

        	ElementService.ListTopViewed.query(  { idCustomer : _idCustomer, idCulture:  moment.locale() } )
			.$promise 
					.then(function (data)  
					{
	                    $scope.listTopViewed = data;  
	                },
	                function (reason) {
	                    $rootScope.error = reason;
	                });			
        };
        //
        $scope.listPurpose = function()
        {
    		StructureService.ListPurpose.query( { idCulture:  moment.locale() })
	         .$promise
                .then(function (item) 
                {
                    $scope.purposes = item;                        
                },
                function (reason) {
                    $scope.error = reason;
                });		
        };

    }])
	//Carregar o resultado do filtro e mostra na listagem
	.controller('ElementFilterController', ['$timeout', '$rootScope', '$scope', '$filter', '$state', '$window', 'ElementService', 'StructureService', function ($timeout, $rootScope, $scope, $filter, $state, $window, ElementService, StructureService) 
    {       	
    	if($rootScope.filter.ActiveView == 1) //Listagem
		{
			$rootScope.filter.groupIn = 3;	    	
			$rootScope.filter.totalRecodsPerPage = 12;
		}
		else //Mapa
		{
			$rootScope.filter.groupIn = 1;	    	
			$rootScope.filter.totalRecodsPerPage = 30;
		}

		$rootScope.filter.recordCount = 0;
		$rootScope.filter.orderBy = "createDate DESC";
		$rootScope.filter.pageIndex = 0;
		$rootScope.filter.idCulture = moment.locale();

		var _listProperties = function()
    	{
    		$rootScope.filter.IDPurpose = [];			
			$rootScope.filter.FilterAttribute = [];
			if($rootScope.filter.singleIDPurpose != null)
				$rootScope.filter.IDPurpose.push($rootScope.filter.singleIDPurpose);		

			//Preço
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minPrice != $rootScope.filter.minPrice || $rootScope.filter.defaultvalues.maxPrice != $rootScope.filter.maxPrice))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'VALT', InitialValue : $rootScope.filter.minPrice, FinalValue : $rootScope.filter.maxPrice });

			//Quartos
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minRooms != $rootScope.filter.minRooms || $rootScope.filter.defaultvalues.maxRooms != $rootScope.filter.maxRooms))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'BEDS', InitialValue : $rootScope.filter.minRooms, FinalValue : $rootScope.filter.maxRooms });

			//Garagens 				
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minGarage != $rootScope.filter.minGarage || $rootScope.filter.defaultvalues.maxGarage != $rootScope.filter.maxGarage))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'GAR',  InitialValue : $rootScope.filter.minGarage, FinalValue : $rootScope.filter.maxGarage });

			//Área
			if($rootScope.filter.defaultvalues != null && ($rootScope.filter.defaultvalues.minArea != $rootScope.filter.minArea || $rootScope.filter.defaultvalues.maxArea != $rootScope.filter.maxArea))
				$rootScope.filter.FilterAttribute.push({ Acronym : 'TA', InitialValue : $rootScope.filter.minArea, FinalValue : $rootScope.filter.maxArea });

			console.log($rootScope.filter.FilterAttribute);

			ElementService.ListElement.query( $rootScope.filter )
			.$promise 
				.then(function (data)  
				{
					$timeout(function () {
						$scope.properties = data.records;  
						$scope.recordCount = data.recordCount;
						$rootScope.filter.recordCount = data.recordCount;
					}, 100);
		        },
		        function (reason) {
		            $rootScope.error = reason;
		        });			    
    		
    	}
    	//
    	$scope.$on("filterRequested", function (event, args) 
    	{
			_listProperties();
    	});
    	//
    	_listProperties();		  	
		
    }])
	//Buscar o detalhe do elemento de maneira separada
    .controller('ElementDetailController', ['$rootScope', '$scope', '$stateParams', 'ElementService', 'CustomerService', function ($rootScope, $scope, $stateParams, ElementService, CustomerService) 
    {      
    	//'use strict';    	
    	$scope.state = "empty";
    	$scope.element = null;
		$scope.customer = null;
		$scope.elementFeatured = [];
		$scope.elementSimilar = [];

		if($stateParams.idElement != null)
		{
	    	//Chamada principal
			ElementService.GetElement.query(  { idCulture : moment.locale(), idElement : $stateParams.idElement } )
			.$promise 
					.then(function (data)  
					{
	                    $scope.element = data;  
	                    $scope.$broadcast("elementLoaded", data);	   

						ElementService.ListImages.execute( { idCulture : moment.locale(), idElement : data.idElement, groupIn : 0 })	
						.$promise 
							.then(function (data)  
							{
								var images = [];
								for (var i = 0; i < data.images.length; i++) 
								{
									images.push({index:i, image:data.images[i]});
								};		
								$scope.element.images = images;
						    },
						    function (reason) {
						        $rootScope.error = reason;
						    });

	                    //Buscando os dados do aunciante
						CustomerService.GetCustomer.query(  { idCustomer : data.idCustomer } )
						.$promise 
							.then(function (data)  
							{
			                    $scope.customer = data;  
			                },
			                function (reason) {
			                    $rootScope.error = reason;
			                });		                    

			            //Buscando os imóveis destacados    
						ElementService.ListFeatured.query(  { idCulture : moment.locale(), idCustomer : data.idCustomer} )
						.$promise 
							.then(function (data)  
							{
			                    $scope.elementFeatured = data;  
			                },
			                function (reason) {
			                    $rootScope.error = reason;
			                });			            

						//Buscando os imóveis similares
						ElementService.ListSimilar.query(  { idCulture : moment.locale(), idElement : data.idElement, idCustomer : data.idCustomer} )
						.$promise 
							.then(function (data)  
							{
			                    $scope.elementSimilar = data;  
			                },
			                function (reason) {
			                    $rootScope.error = reason;
			                });			                
	                },
	                function (reason) {
	                    $scope.error = reason;
	                });			        
		}
	}])
	//Buscar o detalhe do elemento de maneira separada
    .controller('ElementSubmitController', ['$state', '$rootScope', '$scope', '$stateParams', '$filter', 'ElementService', 'toastr', 'Upload', 'config', '$timeout', '$uibModal', function ($state, $rootScope, $scope, $stateParams, $filter, ElementService, toastr, Upload, config, $timeout, $uibModal) 
    {      		
    	var _idElement = $stateParams.idElement != null ? $stateParams.idElement : null;
		var _uploadImages = function( idElement ) {
			if ($scope.element.files && $scope.element.files.length) 
			{
				var firstSubmit = true;
				var files = $scope.element.files;

		        angular.forEach($scope.element.files, function(file) 
		        {
		            file.upload = Upload.upload({
		                url: config.apiUrl + "/apielement/UploadImages",
						data: { file: file, idElement : idElement, deleteDir : firstSubmit }
		            });
		            firstSubmit = false;

		            file.upload.then(function (response) {
		                $timeout(function () {
		                    //toastr.success(response.data.message, 'Confirmação!');
		                    file.result = 'OK';

		                    //ultimo arquivo enviado;
		                    //if(key == files.length - 1)
		                    _listImages(idElement);
		                });
		            }, function (response) {
						if (response.status > 0)
                    		$scope.errorMsg = response.status + ': ' + response.data;		            	
		            }, function (evt) 
		            {
		                file.progress = Math.min(100, parseInt(100.0 *  evt.loaded / evt.total));
		            });
		        });
	    	}
    	}
		// 
    	var _listImages = function(_idElement)
    	{
			ElementService.ListImages.execute( { idElement : _idElement, groupIn : 3 })	
				.$promise 
					.then(function (data)  
					{
						$scope.element.agregatedImages = data.images;
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });
    	}
    	//
    	$scope.initialize = function()
    	{
    		$scope.element = { idCulture : moment.locale() };
	    	$scope.basicAttributes = [];
	    	$scope.infrastructureAttributes = [];

			if(_idElement != '')
			{
		    	//Chamada principal
				ElementService.GetElement.query(  { idCulture : moment.locale(), idElement : _idElement, igoreAttributes : true, validateCustomer : true } )
				.$promise 
					.then(function (data)  
					{
						$scope.element = data;  
						_listImages($scope.element.idElement);
	                    //Avisando controller secundarios que o elemento foi carregado
	                    /*
	                    $scope.$broadcast("elementLoaded", {
		                    	idCountry : data.idCountry, 
		                    	idStateProvince: data.idStateProvince, 
		                    	idCity : data.idCity, 
		                    	idPurpose : data.idPurpose,
		                    	idCategory: data.idHierarchyStructureParent 
		                    });
		                */    
	                    $scope.$broadcast("elementLoaded", data);	                    
	                    $state.go('submit');
		            },
	                function (reason) 
	                {
	                    $rootScope.error = reason;
	                    $state.go('sign-in');
	                });			        
			}

			//Carregar os atributos
			$scope.listBasicAttributes();
	        $scope.listInfrastructureAttributes();			    	
    	};
		//	
    	$scope.insertUpdateElement = function()
    	{
    		var _element = $scope.element;
    		
    		_element.basicAttributes = $filter('filter')($scope.infrastructureAttributes, { checked:true });  //filtrando pela propriedade Checked
			angular.forEach($scope.basicAttributes, function(group){
				angular.forEach(group, function(item){				                   
					item.Checked = true;
    				_element.basicAttributes.push(item);
				});                   
            });

    		ElementService.InsertUpdateElement.save(  _element )
			.$promise 
				.then(function (data)  
				{
					$scope.element.idElement = data.idElement;
					$scope.element.idCustomer = data.idCustomer;					
					_uploadImages(data.idElement);
					
					toastr.success(data.message, 'Confirmação!');
					$scope.element.files = null;
					//$state.go('submit', { idElement: data.idElement }, {reload: true, notify: true});
					//$state.go('my-properties');
                },
                function (reason) {
                    $rootScope.error = reason;
                });    		
    	};
		//	
		$scope.listBasicAttributes =  function()
		{
			ElementService.ListBasicAttributes.query(  { idCulture : moment.locale(), idElement : _idElement} )
			.$promise 
				.then(function (data)  
				{
                    $scope.basicAttributes = data;  

                },
                function (reason) {
                    $rootScope.error = reason;
                });			        
		} 
		//	
		$scope.listInfrastructureAttributes =  function()
		{
			ElementService.ListInfrastructureAttributes.query(  { idCulture : moment.locale(), idElement : _idElement } )
			.$promise 
				.then(function (data)  
				{
                    $scope.infrastructureAttributes = data;  
                    //$('.checkbox').iCheck();
                },
                function (reason) {
                    $rootScope.error = reason;
                });			        
		} 		
		//
		$scope.setDefaultImage = function(_idElement, _idElementAttribute){
			ElementService.SetDefaultImage.execute( { idElement : _idElement, idElementAttribute: _idElementAttribute })	
			.$promise 
				.then(function (data)  
				{
					$scope.element.defaultPicturePath = data.defaultPicturePath;
					toastr.success(data.message, 'Confirmação!');
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    	
		}
		//
    	$scope.deleteImage = function(_idElement, _idElementAttribute)
    	{
    		var _templateUrl;
    		if(_idElementAttribute == null)
    			_templateUrl = 'delete-all-images-template.html';
    		else
    			_templateUrl = 'delete-image-template.html';    			

			var modalInstance = $uibModal.open({
			      //animation: $scope.animationsEnabled,
			      templateUrl: _templateUrl,
			      controller: 'DeleteImageElementController',
			      size: 20,			      
			      resolve: 
			      {
			      	idElement: function () {
			          return _idElement;
			        },			      
			        idElementAttribute: function () {
			          return _idElementAttribute;
			        }			      
			    }});

			modalInstance.result.then(function (response) 
			{
				_listImages(response.idElement);
				//var item = $filter('filter')($scope.element.agregatedImages, response.imageName)[0];
				toastr.success(response.message, 'Confirmação!');
			}, 
			function () {
				console.log('Modal dismissed at: ' + new Date());
			});
    	};

		$scope.initialize();
	}])
	.controller('DeleteImageElementController', ['$scope', '$rootScope', 'ElementService', 'idElement', 'idElementAttribute', '$uibModalInstance', function ($scope, $rootScope, ElementService, idElement, idElementAttribute, $uibModalInstance) 
    {    
    	$scope.idElement = idElement;
    	$scope.idElementAttribute = idElementAttribute;

		//
		$scope.confirmDeleteImage = function(){
			ElementService.DeleteElementImage.execute( { idElement : $scope.idElement, idElementAttribute: $scope.idElementAttribute})	
			.$promise 
				.then(function (data)  
				{
					$uibModalInstance.close( { message : data.message, 	
											   idElement: $scope.idElement, 
											   idElementAttribute: $scope.idElementAttribute } );
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    		
		}
		//
		$scope.cancelDeleteImage = function(){
			$uibModalInstance.dismiss('cancel');
		}
		//
		$scope.confirmDeleteAllImages = function(){
			ElementService.DeleteElementImage.execute( { idElement : $scope.idElement })	
			.$promise 
				.then(function (data)  
				{
					$uibModalInstance.close( { message : data.message, 
											   idElement: $scope.idElement } );
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    		
		}
		//
		$scope.cancelDeleteAllImagese = function(){
			$uibModalInstance.dismiss('cancel');
		}
    }])
	.controller('AddressSelectController', ['$scope', '$rootScope', 'addresses', '$uibModalInstance', function ($scope, $rootScope, addresses, $uibModalInstance) 
    {   
    	$scope.addresses = addresses;
		$scope.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};		
		//
		$scope.select = function (_address) 
		{			
			$uibModalInstance.close( { address : _address } );
		};
    }]);