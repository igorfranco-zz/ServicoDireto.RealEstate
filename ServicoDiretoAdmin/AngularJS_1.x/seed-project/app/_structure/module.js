
"use strict";

angular.module('app.structure', ['ui.router'])
    .config(function ($stateProvider) {
        $stateProvider
            .state('app.structure', { }) //default
            .state('app.structure.icon-list', {
                url: '/structure/icon-list',
                data: {
                    title: 'Listagem de Ícones'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_structure/views/icon/list.html",
                        controller: 'IconListController'
                    }
                }                    
            })        
            .state('app.structure.icon-create', {
                url: '/structure/icon-create/:idIcon',
                data: {
                    title: 'Gereciamento de Ícones'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_structure/views/icon/create.html",
                        controller: 'IconCreateController'
                    }
                }
            })
            .state('app.structure.culture-list', {
                url: '/structure/culture-list',
                data: {
                    title: 'Listagem de Idiomas'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_structure/views/culture/list.html",
                        controller: 'CultureListController'
                    }
                }                    
            })        
            .state('app.structure.culture-create', {
                url: '/structure/culture-create/:idCulture',
                data: {
                    title: 'Gereciamento de Idiomas'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_structure/views/culture/create.html",
                        controller: 'CultureCreateController'
                    }
                }
            })

        });

