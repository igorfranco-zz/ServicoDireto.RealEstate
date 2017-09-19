//http://stackoverflow.com/questions/12371159/how-to-get-evaluated-attributes-inside-a-custom-directive
//http://stackoverflow.com/questions/25212859/using-angular-directive-to-watch-for-change-values-with-bootstrap-select

angular.module("MainApp")
    .directive("bootselectpicker", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
            scope.$watchCollection('array', function(newVal) {
              el.selectpicker('refresh');
            });
          },          
          restrict: "A",
          replace : true,
          scope: 
          {
            array: '=', //usar o mesmo do contexto
            class: '=' 
          }          
        };
    }])
    .directive("zCheckbox", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
              el.iCheck();
              console.log('zCheckbox called');

              scope.$watchCollection('array', function(newVal) {
                el.iCheck('refresh');
                console.log('zCheckbox called update');
              });              
          },          
          restrict: "A",
          replace : true,
        };
    }])
    .directive('icheck', ['$timeout', '$parse', function($timeout, $parse) {
      return {
              require: 'ngModel',
              link: function($scope, element, $attrs, ngModel) {
                  return $timeout(function() {
                      var value;
                      value = $attrs['value'];

                      $scope.$watch($attrs['ngModel'], function(newValue){
                          $(element).iCheck('update');
                      })

                      return $(element).iCheck(
                      {
                          //checkboxClass: 'icheckbox_flat-aero',
                          //radioClass: 'iradio_flat-aero'
                      }).on('ifChanged', function(event) {
                          if ($(element).attr('type') === 'checkbox' && $attrs['ngModel']) {
                              $scope.$apply(function() {
                                  return ngModel.$setViewValue(event.target.checked);
                              });
                          }
                          if ($(element).attr('type') === 'radio' && $attrs['ngModel']) {
                              return $scope.$apply(function() {
                                  return ngModel.$setViewValue(value);
                              });
                          }
                      });
                  });
              }
          };      
    }])    
    .directive("zFileInput", ["$timeout", "config", function ($timeout, config) {
        return {
          link: function(scope, el, attrs, ngModel){
            scope.$watch('element', function(newValue){
              console.log('zFileInput called');
              //if(scope.element != null) //presenca do atributo
              {                  
                  var initialPreviewImages = [];                
                  var btnCust = '<button type="button" class="btn btn-default" title="Imagem principal" ' + 
                      'onclick="alert(\'Call your custom code here.\')">' +
                      '<i class="glyphicon glyphicon-tag"></i>' +
                      '</button>'; 

                  if(scope.element != null && scope.element.Images != null) 
                  {   
                    for (var i = 0; i < scope.element.Images.length; i++) 
                    {
                      var image = scope.element.Images[i];
                      initialPreviewImages.push("<img alt='' title='' class='file-preview-image' src='" + config.adminSite + '/uploads/' + scope.element.IDCustomer + '/' + scope.element.IDElement + '/thumb/' + image + "' >");
                    };
                  }

                  el.fileinput({
                      initialPreview:  initialPreviewImages,
                      language: "pt-BR",
                      overwriteInitial: true,
                      uploadUrl: "/____LINK_UPLOAD/2",
                      allowedFileExtensions: ["jpg", "png", "gif"],
                      browseClass: "btn btn-default",
                      browseIcon: "<i class=\"glyphicon glyphicon-picture\"></i> ",
                      removeClass: "btn btn-danger",
                      removeIcon: "<i class=\"glyphicon glyphicon-trash\"></i> ",
                      removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
                      showUpload : false,
                      maxImageWidth: 200,
                      maxFileCount: 5,
                      resizeImage: true,
                      maxFileSize: 500,
                      layoutTemplates: 
                      {  
                        actions: btnCust + ' {delete}'//,
                      }});                  
                }
            });
          },          
          restrict: "A",
          require: 'ngModel',
          replace : true,
          scope: 
          {
            element: '=ngModel'
          }            
        };
    }])
    .directive("zFileInputAvatar", ["$timeout", "config", function ($timeout, config) {
        return {
          link: function(scope, el, attrs) {
              scope.$watch('logo', function(newValue){
                console.log('zFileInputAvatar called');
                  var initialPreviewImages = [];
                  if(newValue != null && newValue != undefined)  
                  {
                    initialPreviewImages.push('<img src="' + config.adminSite + newValue + '" alt="Foto">') ;
                    el.fileinput('refresh', {initialPreview:  initialPreviewImages});
                  }
                  else
                  {
                    el.fileinput({
                        maxFileCount: 1,
                        overwriteInitial: true,
                        maxFileSize: 1500,
                        showClose: false,
                        showCaption: false,
                        showUpload : false,
                        language: "pt-BR",
                        browseLabel: '',
                        removeLabel: '',
                        browseIcon: '<i class="glyphicon glyphicon-folder-open"></i>',
                        removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
                        removeTitle: 'Cancel or reset changes',
                        elErrorContainer: '#kv-avatar-errors',
                        msgErrorClass: 'alert alert-block alert-danger',
                        defaultPreviewContent: '<img src="assets/img/agent-01.jpg" alt="Foto" >',
                        allowedFileExtensions: ["jpg", "png", "gif"] });
                  }
              })
          },          
          restrict: "A",
          replace : true,
          scope: 
          {
            logo: '=ngModel'
          }                      
        };
    }])
    .directive("zTooltip", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
            console.log('zTooltip called');
            el.tooltip();
          },          
          restrict: "A",
          replace : true,
        };
    }])  
    .directive("meioMask", ["$timeout", function ($timeout) {
        return {
          link: function(scope, el, attrs) 
          {
            console.log('meioMask called');
            el.setMask(attrs['mask']);
          },          
          restrict: "A",
          replace : true,
        };
    }]);      
