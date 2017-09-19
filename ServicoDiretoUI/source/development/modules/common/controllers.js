
angular.module('CommonApp')
    .controller('CommonController', ['$scope', '$location', 'CommonService', function ($scope, $location, CommonService) {
        
        $scope.error = null;
        //Alternar templates
        $scope.templates =
            [{ name: 'footer', url: 'modules/common/views/footer.html' },
             { name: 'navbar', url: 'modules/common/views/navbar.html' },
             { name: 'searchbox', url: 'modules/common/views/searchbox.html' }
             ];

        $scope.getTemplateUrl = function (name) {
            for (var i = 0; i < $scope.templates.length; i++) {
                if ($scope.templates[i].name == name)
                    return $scope.templates[i].url;
            }
        };
    }])        
    .controller('TermsConditionsController', ['$state', '$scope', '$timeout', function($state, $scope, $timeout){        
    }])
    .controller('AboutUsController', ['$state', '$scope', '$timeout', function($state, $scope, $timeout){        
    }])
    .controller('StatusCommonController', ['$scope', '$stateParams', 'CommonService', function ($scope, $stateParams, CommonService) 
    {
       $scope.message = $stateParams.message;
    }])    
    .controller('SideBarController', ['$state', '$rootScope', '$scope', '$timeout', 'ElementService', function($state, $rootScope, $scope, $timeout, ElementService)
    {        
        $scope.customer = null;             
        $scope.properties  = [];
        $scope.recordCount = 0;
        
        $rootScope.filter = 
        {   
            ActiveView : 1,
            groupIn : 3,
            recordCount : 0,
            totalRecodsPerPage : 12,
            orderBy : "createDate DESC",
            pageIndex : 0,
            idCulture : moment.locale()
        };  

        var _listProperties = function()
        {
            $rootScope.filter.IDPurpose = [];           
            $rootScope.filter.FilterAttribute = [];
            if($rootScope.filter.singleIDPurpose != null)
                $rootScope.filter.IDPurpose.push($rootScope.filter.singleIDPurpose);        

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
                        $state.go('properties-listing-lines');
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
        
        //Carregando imóveis em destaque do anunciante em questão
        ElementService.ListFeatured.query({idCulture : moment.locale()})
        .$promise 
                .then(function (data)  
                {
                    $scope.elementFeatured = data;  
                },
                function (reason) {
                    $rootScope.error = reason;
                });            
    }])
    .controller('ContactController', ['$scope', '$rootScope', 'toastr', 'CustomerService',  function ($scope, $rootScope, toastr, CustomerService) 
    {        
        $scope.contact = {
            name : '',
            message : '',
            email : '',
            idCulture : moment.locale()
        };

        $scope.sendEmail = function(){
            CustomerService.SendEmailContact.execute( $scope.contact )
            .$promise 
                .then(function (data)  
                {
                    toastr.success("Email enviado com sucesso", 'Confirmação!');                                               
                },
                function (reason) {
                    $rootScope.error = reason;
                });
        };   
    }]);         




