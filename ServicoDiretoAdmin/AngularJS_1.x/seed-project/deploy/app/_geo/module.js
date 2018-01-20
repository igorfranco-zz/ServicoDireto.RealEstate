
//O primeiro estado tem ter o mesmo nome do modulo
"use strict";

//angular.module('app.forms', ['ui.router'])

angular.module('app.geo', ['ui.router'])
    .config(function ($stateProvider) {
        $stateProvider
            .state('app.geo', { }) //default
            .state('app.geo.country-list', {
                    url: '/geo/country-list',
                    data: {
                        title: 'Listagem de Países'
                    },
                    views: {
                        "content@app": {
                            templateUrl: "app/_geo/views/country/list.html",
                            controller: 'CountryListController'
                        }
                    }                    

            })        
            .state('app.geo.country-create', {
                url: '/geo/country-create/:idCountry',
                data: {
                    title: 'Gereciamento de País'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_geo/views/country/create.html",
                        controller: 'CountryCreateController'
                    }
                }
            })
            .state('app.geo.stateprovince-list', {
                    url: '/geo/stateprovince-list',
                    data: {
                        title: 'Listagem de Estados e Províncias'
                    },
                    views: {
                        "content@app": {
                            templateUrl: "app/_geo/views/stateprovince/list.html",
                            controller: 'StateProvinceListController'
                        }
                    }                    

            })        
            .state('app.geo.stateprovince-create', {
                url: '/geo/stateprovince-create/:idStateProvince',
                data: {
                    title: 'Gereciamento Estados e Províncias'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_geo/views/stateprovince/create.html",
                        controller: 'StateProvinceCreateController'
                    }
                }
            })            
            .state('app.geo.city-list', {
                    url: '/geo/city-list',
                    data: {
                        title: 'Listagem de Cidades'
                    },
                    views: {
                        "content@app": {
                            templateUrl: "app/_geo/views/city/list.html",
                            controller: 'CityListController'
                        }
                    }                    

            })        
            .state('app.geo.city-create', {
                url: '/geo/city-create/:idCity',
                data: {
                    title: 'Gereciamento de Cidades'
                },
                views: {
                    "content@app": {
                        templateUrl: "app/_geo/views/city/create.html",
                        controller: 'CityCreateController'
                    }
                }
            })                        
    });