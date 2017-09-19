
angular.module('CustomerApp')
    .controller('ProfileCustomerController', ['$scope', '$rootScope', 'CustomerService', 'toastr', 'config', 'Upload', '$timeout',  function ($scope, $rootScope, CustomerService, toastr, config, Upload, $timeout) 
    {    
    	$scope.customer = null;

		//Chamada principal
		CustomerService.GetCustomer.query()
		.$promise 
			.then(function (data)  
			{
				$scope.customer = data;  

                $scope.$broadcast("elementLoaded", 
                    {
                    	idCountry : data.idCountry, 
                    	idStateProvince: data.idStateProvince, 
                    	idCity : data.idCity                     
                    }
                );				
		    },
		    function (reason) {
		        $rootScope.error = reason;
		    });	    		

		$scope.saveCustomer = function(){
			CustomerService.SaveCustomer.execute( $scope.customer )
				.$promise 
					.then(function (data)  
					{
						toastr.success("Perfil salvo com sucesso!", 'Confirmação!');
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });
		};
		
		$scope.uploadFiles = function(files, errFiles) {
	        $scope.files = files;
	        $scope.errFiles = errFiles;
	        
	        angular.forEach(files, function(file) {
	            file.upload = Upload.upload({
	                url: config.apiUrl + "/apicustomer/UploadAvatar",
	                data: {file: file}
	            });

	            file.upload.then(function (response) {
	                $timeout(function () {
	                    toastr.success(response.data.message, 'Confirmação!');
	                });
	            }, function (error) {
	                $rootScope.error = error;
	            }, function (evt) 
	            {
	                file.progress = Math.min(100, parseInt(100.0 *  evt.loaded / evt.total));
	            });
	        });
    	};
	}])
	.controller('BookmarkedCustomerController', ['$scope', '$rootScope', 'ElementService',  function ($scope, $rootScope, ElementService) 
    {    
    	$scope.bookmarkedElements = [];

		ElementService.ListBookmarked.query( {idCulture : moment.locale()} )
			.$promise 
				.then(function (data)  
				{
					$scope.bookmarkedElements = data;			    					
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });
    }])
    .controller('MyPropertiesCustomerController', ['$filter','$scope', '$rootScope','toastr', '$uibModal', 'ElementService', '$stateParams', function ($filter, $scope, $rootScope, toastr, $uibModal, ElementService, $stateParams) 
    {   
		$scope.properties  = [];
		$scope.recordCount = 0;
    	$scope.filter = 
    	{
    		recordCount : 0,
    		totalRecodsPerPage : 10,
    		orderBy : "name",
    		pageIndex : 0,
    		idCulture : moment.locale()
    	};
    	//
		$scope.$watch('filter', function(oldValue, newValue){
    	 	if(oldValue.pageIndex != newValue.pageIndex || oldValue.orderBy != newValue.orderBy )	
    	 		//if(oldValue.orderBy != newValue.orderBy)
    	 		//	$scope.filter.pageIndex = 0;
				_listProperties();
    	}, true);

    	/*
    	$scope.properties  = [];    	
    	$scope.filter = 
    	{
    		totalRecodsPerPage : 50,
    		orderBy : "name",
    		pageIndex : 0,
    	};
		//
	    $rootScope.$on('listProperties', function(event, args) {
			console.log('listProperties handled');
			_listProperties();
		});
		*/
    	var _listProperties = function(  )
    	{
    		$scope.filter.useInternalAuth = true;
			ElementService.ListElement.query( $scope.filter )
				.$promise 
					.then(function (data)  
					{
						$scope.properties = data.records;  
						$scope.recordCount = data.recordCount;
						$scope.filter.recordCount = data.recordCount;
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });	    		
    	};
    	//
    	$scope.listProperties = _listProperties;
		//    	
    	$scope.disableElement = function(_idElement)
    	{
			var modalInstance = $uibModal.open({
			      //animation: $scope.animationsEnabled,
			      templateUrl: 'myModalContent.html',
			      controller: 'DeleteElementCustomerController',
			      size: 20,			      
			      resolve: {
			        idElement: function () {
			          return _idElement;
			        }			      
			    }});

			modalInstance.result.then(function (response) 
			{
				toastr.success(response.message, 'Confirmação!');
				//Atualizando o status on the fly
				var item = $filter('filter')($scope.properties, {idElement : response.idElement})[0];
				item.status = 0;
			}, 
			function () {
				console.log('Modal dismissed at: ' + new Date());
			});
    	};

    	//Start
    	_listProperties();
    }])
	.controller('DeleteElementCustomerController', ['$scope', '$rootScope', 'ElementService', 'idElement', '$uibModalInstance', function ($scope, $rootScope, ElementService, idElement, $uibModalInstance) 
    {    
    	$scope.idElement = idElement;

		$scope.ok = function () 
		{			
			ElementService.InactivateElement.execute( { idElement : $scope.idElement })	
			.$promise 
				.then(function (data)  
				{
					$uibModalInstance.close( { message : data.message, idElement: $scope.idElement } );
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });	    		
			
		};

		$scope.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
    }])
    .controller('SendMessageCustomerController', ['$scope', '$rootScope', 'toastr', 'CustomerService',  function ($scope, $rootScope, toastr, CustomerService) 
    {    
    	$scope.showMessageForm = false;
    	$scope.contact = {
    		name : '',
    		message : '',
    		email : '',
    		idElement : 0,
    		idCustomer : 0,
    		idCulture : moment.locale()
    	};

    	var _fillFormFields = function(event, args) 
    	{
			$scope.contact.idElement = ( args.idElement != null ) ? args.idElement : null;
			$scope.contact.idCustomer = args.idCustomer;			

			//Carrega os dados da pessoa que está logada automaticamente
			if($rootScope.authData != null && $rootScope.authData.isAuthenticated == true)
			{
				CustomerService.GetCustomer.query()
				.$promise 
					.then(function (data)  
					{
						$scope.contact.name = data.name;
						$scope.contact.email = data.email;
						$scope.showMessageForm = true;
				    },
				    function (reason) {
				        $rootScope.error = reason;
				    });	    			
			}
			else
			{
				$scope.showMessageForm = true;
			}		
    	}
    	//
    	$scope.$on("customerLoaded", function (event, args) 
        {
        	_fillFormFields(event, args); 
        });  
        //
        $scope.$on("elementLoaded", function (event, args) 
        {
        	_fillFormFields(event, args);
        });  

    	$scope.sendEmail = function(){
    		CustomerService.SendEmailRequestInfo.execute( $scope.contact )
			.$promise 
				.then(function (data)  
				{
					toastr.success(data.message, 'Confirmação!');						    					
			    },
			    function (reason) {
			        $rootScope.error = reason;
			    });
    	};   
    }])
    .controller('AgentDetailCustomerController', ['$timeout','$scope', '$rootScope', 'CustomerService', 'ElementService', '$stateParams', '$controller', 'config', function ($timeout, $scope, $rootScope, CustomerService, ElementService, $stateParams, $controller, config) 
    {   
    	$scope.customer = null;  	    	
		$scope.properties  = [];
		$scope.recordCount = 0;
    	
		$scope.filter = 
    	{   
			groupIn : 3,
    		recordCount : 0,
    		totalRecodsPerPage : 12,
    		orderBy : "createDate DESC",
    		pageIndex : 0,
    		idCustomer : $stateParams.idCustomer,
    		idCulture : moment.locale()
		};  

		var _listProperties = function()
    	{
    		$scope.filter.IDPurpose = [];			
			$scope.filter.FilterAttribute = [];
			if($scope.filter.singleIDPurpose != null)
				$scope.filter.IDPurpose.push($scope.filter.singleIDPurpose);		

			//Preço
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minPrice != $scope.filter.minPrice || $scope.filter.defaultvalues.maxPrice != $scope.filter.maxPrice))
				$scope.filter.FilterAttribute.push({ IDAttribute : 5, InitialValue : $scope.filter.minPrice, FinalValue : $scope.filter.maxPrice });

			//Quartos
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minRooms != $scope.filter.minRooms || $scope.filter.defaultvalues.maxRooms != $scope.filter.maxRooms))
				$scope.filter.FilterAttribute.push({ IDAttribute : 1, InitialValue : $scope.filter.minRooms, FinalValue : $scope.filter.maxRooms });

			//Garagens 				
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minGarage != $scope.filter.minGarage || $scope.filter.defaultvalues.maxGarage != $scope.filter.maxGarage))
				$scope.filter.FilterAttribute.push({ IDAttribute : 11,  InitialValue : $scope.filter.minGarage, FinalValue : $scope.filter.maxGarage });

			//Área
			if($scope.filter.defaultvalues != null && ($scope.filter.defaultvalues.minArea != $scope.filter.minArea || $scope.filter.defaultvalues.maxArea != $scope.filter.maxArea))
				$scope.filter.FilterAttribute.push({ IDAttribute : 4, InitialValue : $scope.filter.minArea, FinalValue : $scope.filter.maxArea });

			ElementService.ListElement.query( $scope.filter )
			.$promise 
				.then(function (data)  
				{
					$timeout(function () {
						$scope.properties = data.records;  
						$scope.recordCount = data.recordCount;
						$scope.filter.recordCount = data.recordCount;
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
    	
		//Carregando imóveis em destaque do anunciante em questão
		ElementService.ListFeatured.query(  { idCustomer : $stateParams.idCustomer, idCulture : moment.locale() } )
		.$promise 
				.then(function (data)  
				{
                    $scope.elementFeatured = data;  
                },
                function (reason) {
                    $rootScope.error = reason;
                });			


		CustomerService.GetCustomer.query( { idCustomer :  $stateParams.idCustomer })
		.$promise 
			.then(function (data)  
			{
				$scope.customer = data;  
				$scope.$broadcast("customerLoaded", 
                {
	            	idCustomer : data.idCustomer	            	
                });						
				//Carregar imóveis vinculados a agente em questão
				//$rootScope.$emit('listProperties'/*, args*/);				
		    },
		    function (reason) {
		        $rootScope.error = reason;
		    });	    		
	}])