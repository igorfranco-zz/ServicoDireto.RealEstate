angular.module("AccountApp")
  .directive('matchPassword', [ "$timeout", function($timeout) {
          return {
              restrict: 'A',
              require: 'ngModel',
              link: function(scope, elm, attrs, ctrl, ngModel) 
              {
                return $timeout(function() 
                {
                  var validateFn = function (viewValue) {
                    if(ctrl.$isEmpty(viewValue) || scope.password != viewValue)
                    {
                       ctrl.$setValidity('matchPassword', false);                                    
                       return viewValue;
                    }
                    else
                    {
                      ctrl.$setValidity('matchPassword', true);
                      return viewValue;
                    }                              
                  };
                  ctrl.$parsers.push(validateFn);
                  ctrl.$formatters.push(validateFn);
                });
              },
              scope: 
              {
                password: '=matchPassword',
                confirmPassword: '=ngModel'
              }  
          }
        }]);
      