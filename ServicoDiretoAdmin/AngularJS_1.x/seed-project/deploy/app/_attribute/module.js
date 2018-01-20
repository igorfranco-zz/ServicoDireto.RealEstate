
"use strict";

angular.module('app.attribute', ['ui.router'])
    .config(function ($stateProvider) {
        $stateProvider
            .state('app.attribute', { }) //default
            .state('app.attribute.type-list', {
                url: '/hierarchy/type-list',
                data: {
                    title: 'Listagem de tipos de atributos'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_attribute/views/type/list.html",
                        controller: 'AttributeTypeListController'
                    }
                }                    
            })        
            .state('app.attribute.type-create', {
                url: '/hierarchy/type-create/:idAttributeType',
                data: {
                    title: 'Gerenciamento de tipos de atributos'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_attribute/views/type/create.html",
                        controller: 'AttributeTypeCreateController'
                    }
                },
                resolve: {
                        srcipts: function(lazyScript){
                            return lazyScript.register([
                                "build/vendor.ui.js"
                            ])

                        }
                    }                

            })
            .state('app.attribute.attribute-list', {
                url: '/hierarchy/attribute-list',
                data: {
                    title: 'Listagem de atributos'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_attribute/views/attribute/list.html",
                        controller: 'AttributeListController'
                    }
                }                    
            })        
            .state('app.attribute.attribute-create', {
                url: '/hierarchy/attribute-create/:idAttribute',
                data: {
                    title: 'Gerenciamento de atributos'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_attribute/views/attribute/create.html",
                        controller: 'AttributeCreateController'
                    }
                }
            })            
        });
