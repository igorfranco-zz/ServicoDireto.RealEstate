"use strict";
angular.module('app.structure')
    .factory('IconService', ['$resource', 'APP_CONFIG',
        function ($resource, APP_CONFIG) 
        {
            return {
                List: $resource(APP_CONFIG.apiUrl + "/apiicon/List", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),
                ListActive: $resource(APP_CONFIG.apiUrl + "/apiicon/ListActive", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
				GetByID: $resource(APP_CONFIG.apiUrl + "/apiicon/GetByID", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                              
                Save: $resource(APP_CONFIG.apiUrl + "/apiicon/Save", {}, {
                    execute: { method: 'POST', params: {} }
                }),
                Deactivate: $resource(APP_CONFIG.apiUrl + "/apiicon/Deactivate", {}, {
                    execute: { method: 'POST', params: {} }
                })              
            };
        }])
    .factory('CultureService', ['$resource', 'APP_CONFIG',
        function ($resource, APP_CONFIG) 
        {
            return {
                List: $resource(APP_CONFIG.apiUrl + "/apiculture/List", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),
                ListActive: $resource(APP_CONFIG.apiUrl + "/apiicon/ListActive", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                GetByID: $resource(APP_CONFIG.apiUrl + "/apiculture/GetByID", {}, {
                    query: { method: 'GET', params: {}, isArray: false }
                }),                                              
                Save: $resource(APP_CONFIG.apiUrl + "/apiculture/Save", {}, {
                    execute: { method: 'POST', params: {} }
                }),
                Deactivate: $resource(APP_CONFIG.apiUrl + "/apiculture/Deactivate", {}, {
                    execute: { method: 'POST', params: {} }
                })              
            };
        }])
