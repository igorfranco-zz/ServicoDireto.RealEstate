"use strict";

angular.module('app.geo')
    .factory('CountryService', ['$resource', 'APP_CONFIG',
        function ($resource, APP_CONFIG) 
        {
            return {
                List: $resource(APP_CONFIG.apiUrl + "/apicountry/List", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),
                ListActive: $resource(APP_CONFIG.apiUrl + "/apicountry/ListActive", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
				GetByID: $resource(APP_CONFIG.apiUrl + "/apicountry/GetByID", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                              
                Save: $resource(APP_CONFIG.apiUrl + "/apicountry/Save", {}, {
                    execute: { method: 'POST', params: {} }
                }),
                Deactivate: $resource(APP_CONFIG.apiUrl + "/apicountry/Deactivate", {}, {
                    execute: { method: 'POST', params: {} }
                })              
            };
        }])
    .factory('StateProvinceService', ['$resource', 'APP_CONFIG',
            function ($resource, APP_CONFIG) 
            {
                return {
                    List: $resource(APP_CONFIG.apiUrl + "/apistateprovince/List", {}, {
                        query: { method: 'GET', params: {}, isArray: false }
                    }),
                    ListActive: $resource(APP_CONFIG.apiUrl + "/apistateprovince/ListActive", {}, {
                            query: { method: 'GET', params: {}, isArray: true }
                    }),                      
                    ListByCountry : $resource(APP_CONFIG.apiUrl + "/apistateprovince/ListByCountry", {}, {
                            query: { method: 'GET', params: {}, isArray: true }
                    }),
                    GetByID: $resource(APP_CONFIG.apiUrl + "/apistateprovince/GetByID", {}, {
                        query: { method: 'GET', params: {}, isArray: false }
                    }),                                              
                    Save: $resource(APP_CONFIG.apiUrl + "/apistateprovince/Save", {}, {
                        execute: { method: 'POST', params: {} }
                    }),
                    Deactivate: $resource(APP_CONFIG.apiUrl + "/apistateprovince/Deactivate", {}, {
                        execute: { method: 'POST', params: {} }
                    })              
                };
            }])    
    .factory('CityService', ['$resource', 'APP_CONFIG',
                function ($resource, APP_CONFIG) 
                {
                    return {
                        List: $resource(APP_CONFIG.apiUrl + "/apicity/List", {}, {
                            query: { method: 'GET', params: {}, isArray: false }
                        }),
                        ListActive: $resource(APP_CONFIG.apiUrl + "/apicity/ListActive", {}, {
                                query: { method: 'GET', params: {}, isArray: true }
                        }),                                    
                        GetByID: $resource(APP_CONFIG.apiUrl + "/apicity/GetByID", {}, {
                            query: { method: 'GET', params: {}, isArray: false }
                        }),                                              
                        Save: $resource(APP_CONFIG.apiUrl + "/apicity/Save", {}, {
                            execute: { method: 'POST', params: {} }
                        }),
                        Deactivate: $resource(APP_CONFIG.apiUrl + "/apicity/Deactivate", {}, {
                            execute: { method: 'POST', params: {} }
                        })              
                    };
                }])    
