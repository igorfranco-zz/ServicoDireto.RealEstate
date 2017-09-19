
angular.module('AccountApp')
    .controller('CreateAccountController', ['$scope', '$rootScope', 'toastr', '$state', 'AccountService', function ($scope, $rootScope, toastr, $state, AccountService) 
    {    
    	$scope.account =     	
        {  
            allowNewsletter :true, 
    		email : "",
			password : "",
    		confirmPassword : "",
            userName : "",
            idCulture:  moment.locale()
    	};

    	$scope.createAccount = function()
        {
            AccountService.CreateAccount.save(  $scope.account )
            .$promise 
                .then(function (data)  
                {
                    toastr.success(data.message, 'Confirmação!');
                    $state.go('status', {message : {
                        statusType : 1,
                        header: "Código de confirmação enviado!",
                        description : 'Para que seu cadastro seja confirmado acesse o seu e-mail "' + $scope.account.email +'".'
                    }});
                },
                function (reason) 
                {
                    $rootScope.error = reason;
                });                     
    	};
    }])
    .controller('ActivateAccountController', ['$scope', '$rootScope', 'toastr', '$state', 'AccountService', '$stateParams', function ($scope, $rootScope, toastr, $state, AccountService, $stateParams) 
    {                   
        AccountService.ActivateAccount.execute(  { code: $stateParams.code} )
        .$promise 
            .then(function (data)  
            {                  
                toastr.success(data.message, 'Confirmação!');
                $state.go('/');
            },
            function (reason) {
                $rootScope.error = reason;
                $state.go('create-account');
            });                     
    }])
    .controller('LoginAccountController', ['$scope', 'localStorageService', '$rootScope', 'toastr', '$state', '$stateParams', 'AccountService', function ($scope, localStorageService, $rootScope, toastr, $state, $stateParams, AccountService) 
    {                   
        $scope.account =        
        {  
            password : "",
            userName : "",
            grant_type:'password'
        };

        $scope.doLogin = function()
        {
            AccountService.Login( $scope.account )
                .then(function (response) 
                {
                    if($stateParams.redirectTo != '')     
                        $state.go($stateParams.redirectTo) 
                    else
                        $state.go('profile') 
                },
                function (err, status) 
                {
                    $rootScope.error = 
                    { 
                        statusText : err.error,
                        data:  { message: err.error_description } 
                    };
                });
        }
    }])
    .controller('ChangePasswordAccountController', ['$scope', '$rootScope', 'toastr', '$stateParams', '$state', 'AccountService', function ($scope, $rootScope, toastr, $stateParams, $state, AccountService) 
    {    
        $scope.password = 
        {
             oldPassword:"",
             newPassword :"",
             confirmPassword:"",
             activateCode: $stateParams.code
        };

        $scope.changePassword = function()
        {         
            AccountService.ChangePassword.execute(  $scope.password )
            .$promise 
                .then(function (data)  
                {
                    toastr.success(data.message, 'Confirmação!');
                    AccountService.logOut();
                    $state.go('sign-in');
                },
                function (reason) 
                {
                    $rootScope.error = reason;
                });                     
        };        
    }])
    .controller('LogoutAccountController', ['$scope', '$state', 'AccountService', function ($scope, $state, AccountService) 
    {    
        console.log('LogoutAccountController called');
        AccountService.logOut();
        $state.go('sign-in');        
    }])
    .controller('RecoveryAccountController', ['$scope', '$rootScope', 'toastr', '$state', '$stateParams', 'AccountService', function ($scope, $rootScope, toastr, $state, $stateParams, AccountService) 
    {              
        $scope.data = 
        {        
            userName : "",
            recoveryName : "",
            email : "",
            recovery : $stateParams.recoveryType,
            idCulture:  moment.locale() 
        };        

        var _message = 
        {
            statusType : 1,
            header: "",
            description : ""
        };

        if( $stateParams.recoveryType == 1 )
        {
            $scope.data.recoveryName = "Senha";
            _message.header = "Recuperação de senha efetuda com sucesso";
            _message.description = "Um e-mail com as instruções de recuperação de senha foi enviado a sua caixa postal";
        }
        else if( $stateParams.recoveryType == 2 )
        {
            $scope.data.recoveryName = "Usuário";
            _message.header = "Recuperação de usuário efetuda com sucesso";
            _message.description = "Um e-mail contendo o seu usuário foi enviado a sua caixa postal"            
        }

        $scope.requestRecovery = function()
        {
            AccountService.Recover.execute(  $scope.data )
            .$promise 
                .then(function (data)  
                {
                    //toastr.success(data.message, 'Confirmação!');
                    $state.go('status', {message : _message});
                },
                function (reason) 
                {
                    $rootScope.error = reason;
                });     

            //$state.go('status', {});
        }

    }]);    