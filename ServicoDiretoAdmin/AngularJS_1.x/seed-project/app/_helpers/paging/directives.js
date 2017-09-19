"use strict";

angular.module('SmartAdmin.UI')   
  .directive("paging", function () {
        return {
          link: function(scope, el, attrs, ctrl) 
          {     
            /*
            //
            scope.paging = 
            {
                page:1, //indice da pagina atual   
                maximumRows:20,  //numero de registros a serem exibido por página
                maximumRange:10, //total de itens a serem exibidos na paginação
                recordCount:0, //numero de registros retornados
                orderBy:"", //ordenar por 
                totalPages:0, //numero de páginas 
                display:[] //coleção de items a serem exibidos na paginação
            }            
            //
            scope.config.maximumRows  = scope.maximumRows;
            scope.config.maximumRange = scope.maximumRange;
            scope.config.recordCount  = scope.recordCount;
            scope.config.orderBy      = scope.orderBy;
            scope.config.page         = scope.page;
            scope.config.totalPages   = scope.totalPages; 
            */       

            scope.$on("$searchCompleted", function (event, args) 
            {
                if(scope.config.createDisplay || scope.config.display == 0)
                  scope.createPaging(scope.config.page);
            });

            /*  
            scope.$watchCollection('config', function(newVal, oldVal){
              if ((newVal.page != oldVal.page && newVal.createDisplay) || newVal.display == 0) {
                scope.createPaging(newVal.page);
              }                              
            });*/

          
          },          
          templateUrl: './app/_helpers/paging/template.html',
          restrict: "E",          
          // Remove all existing content of the directive.
          transclude: true,          
          scope: 
          {
            config:'='
          },
          controller:function ($scope, $rootScope)
          {
            $rootScope.$broadcast("$doSearch");
            //
            $scope.createPaging = function(actualPage)
            {
                //criando os itens a serem exibido na paginação
                this.config.page = actualPage;
                this.config.display = [];
                var nextPages = (this.config.page + this.config.maximumRange);
                if(nextPages > this.config.totalPages)
                    nextPages = this.config.totalPages;

                //se a pagina atual for menor que o maximumRange
                if(actualPage > this.config.maximumRange)
                {
                    this.config.display.push
                    (
                        { 
                            page:actualPage - this.config.maximumRange,
                            first: true                    
                        }
                    );
                }
                //
                for (var i = this.config.page; i < nextPages; i++) 
                {
                    this.config.display.push
                    (
                        { 
                            page : i,
                            text : '[' + i + ']',
                            first: false,
                            last :false
                        }
                    );
                };                                
                //
                if(actualPage < this.config.totalPages && (actualPage + this.config.maximumRange) < this.config.totalPages)
                {
                    this.config.display.push
                    (
                        { 
                            page:actualPage + this.config.maximumRange,
                            last: true                    
                        }
                    );
                }
            }    
            //
            $scope.changePage = function(page, createDisplay)
            {
              this.config.page = page;
              $rootScope.$broadcast("$doSearch", createDisplay);
            }  
          },
          controllerAs: 'ctrl'
        };
    });    
