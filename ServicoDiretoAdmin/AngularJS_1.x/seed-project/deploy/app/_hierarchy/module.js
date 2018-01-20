
"use strict";

angular.module('app.hierarchy', ['ui.router'])
    .config(function ($stateProvider) {
        $stateProvider
            .state('app.hierarchy', { }) //default
            .state('app.hierarchy.purpose-list', {
                url: '/hierarchy/purpose-list',
                data: {
                    title: 'Listagem de Propósitos'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_hierarchy/views/purpose/list.html",
                        controller: 'PurposeListController'
                    }
                }                    
            })        
            .state('app.hierarchy.purpose-create', {
                url: '/hierarchy/purpose-create/:idCulture/:idPurpose',
                params: { 
                    idCulture: 'pt-BR', // default value                     
                    idPurpose: '0',     // default value 
                },               
                data: {
                    title: 'Gerenciamento de Propósitos'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_hierarchy/views/purpose/create.html",
                        controller: 'PurposeCreateController'
                    }
                }
            })
            .state('app.hierarchy.hierarchy-structure-list', {
                url: '/hierarchy/hierarchy-structure-list',
                data: {
                    title: 'Listagem de Estruturas'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_hierarchy/views/hierarchy-structure/list.html",
                        controller: 'HierarchyStructureListController'
                    }
                }                    
            })        
            .state('app.hierarchy.hierarchy-structure-create', {
                url: '/hierarchy/hierarchy-structure-create/:idHierarchyStructure',
                data: {
                    title: 'Gerenciamento de Estruturas'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_hierarchy/views/hierarchy-structure/create.html",
                        controller: 'HierarchyStructureCreateController'
                    }
                }
            })            
        });

