"use strict";
angular.module('app.hierarchy')
    .factory('PurposeService', ['$resource', 'APP_CONFIG',
        function ($resource, APP_CONFIG) 
        {
            return {
                List: $resource(APP_CONFIG.apiUrl + "/apipurpose/List", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
				GetByID: $resource(APP_CONFIG.apiUrl + "/apipurpose/GetByID", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                              
                Save: $resource(APP_CONFIG.apiUrl + "/apipurpose/Save", {}, {
                    execute: { method: 'POST', params: {} }
                }),
                Deactivate: $resource(APP_CONFIG.apiUrl + "/apipurpose/Deactivate", {}, {
                    execute: { method: 'POST', params: {} }
                })              
            };
        }])
    .factory('HierarchyStructureService', ['$resource', 'APP_CONFIG',
        function ($resource, APP_CONFIG) 
        {
            return {
                List: $resource(APP_CONFIG.apiUrl + "/apihierarchystructure/List", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),
                ListCategory: $resource(APP_CONFIG.apiUrl + "/apihierarchystructure/ListCategory", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                
                ListType: $resource(APP_CONFIG.apiUrl + "/apihierarchystructure/ListType", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                
                GetByID: $resource(APP_CONFIG.apiUrl + "/apihierarchystructure/GetByID", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                              
                Save: $resource(APP_CONFIG.apiUrl + "/apihierarchystructure/Save", {}, {
                    execute: { method: 'POST', params: {} }
                }),
                Deactivate: $resource(APP_CONFIG.apiUrl + "/apihierarchystructure/Deactivate", {}, {
                    execute: { method: 'POST', params: {} }
                })              
            };
        }])    
