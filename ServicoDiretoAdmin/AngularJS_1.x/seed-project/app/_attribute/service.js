"use strict";
angular.module('app.attribute')
    .factory('AttributeTypeService', ['$resource', 'APP_CONFIG',
        function ($resource, APP_CONFIG) 
        {
            return {
                List: $resource(APP_CONFIG.apiUrl + "/apiattributetype/List", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
				GetByID: $resource(APP_CONFIG.apiUrl + "/apiattributetype/GetByID", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                              
                Save: $resource(APP_CONFIG.apiUrl + "/apiattributetype/Save", {}, {
                    execute: { method: 'POST', params: {} }
                }),
                Deactivate: $resource(APP_CONFIG.apiUrl + "/apiattributetype/Deactivate", {}, {
                    execute: { method: 'POST', params: {} }
                })              
            };
        }])
    .factory('AttributeService', ['$resource', 'APP_CONFIG',
        function ($resource, APP_CONFIG) 
        {
            return {
                List: $resource(APP_CONFIG.apiUrl + "/apiattribute/List", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),
                GetByID: $resource(APP_CONFIG.apiUrl + "/apiattribute/GetByID", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                              
                Save: $resource(APP_CONFIG.apiUrl + "/apiattribute/Save", {}, {
                    execute: { method: 'POST', params: {} }
                }),
                Deactivate: $resource(APP_CONFIG.apiUrl + "/apiattribute/Deactivate", {}, {
                    execute: { method: 'POST', params: {} }
                })              
            };
        }]);

