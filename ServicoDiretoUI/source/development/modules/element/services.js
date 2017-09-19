
angular
    .module('ElementApp')
    .factory('ElementService', ['$resource', 'config',
        function ($resource, config) 
        {
            return {
                SetDefaultImage: $resource(config.apiUrl + "/apielement/SetDefaultImage", {}, {
                    execute: { method: 'GET', params: {} }
                }),                                                
                DeleteElementImage: $resource(config.apiUrl + "/apielement/DeleteElementImage", {}, {
                    execute: { method: 'GET', params: {} }
                }),                                
                ListImages: $resource(config.apiUrl + "/apielement/ListImages", {}, {
                    execute: { method: 'GET', params: {} }
                }),                    
                InsertUpdateElement: $resource(config.apiUrl + "/apielement/InsertUpdateElement", {}, {
                    save: { method: 'POST', params: {} }
                }),                
                ListElement: $resource(config.apiUrl + "/apielement/ListElement", {}, {
                    query: { method: 'POST', params: {}, isArray: false }
                }),
                ListFeatured: $resource(config.apiUrl + "/apielement/listfeatured", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListTopViewed: $resource(config.apiUrl + "/apielement/ListTopViewed", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListSimilar: $resource(config.apiUrl + "/apielement/ListSimilar", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListBasicAttributes: $resource(config.apiUrl + "/apielement/ListBasicAttributes", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListInfrastructureAttributes: $resource(config.apiUrl + "/apielement/ListInfrastructureAttributes", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                                
                GetElement: $resource(config.apiUrl + "/apielement/getelement", {}, {
                    query: { method: 'GET', params: {} }
                }),                
                InactivateElement: $resource(config.apiUrl + "/apielement/InactivateElement", {}, {
                    execute: { method: 'GET', params: {} }
                }),                                
                AddAsFavorite: $resource(config.apiUrl + "/apielement/AddAsFavorite", {}, {
                    execute: { method: 'GET', params: {} }
                }),
                RemoveAsFavorite: $resource(config.apiUrl + "/apielement/RemoveAsFavorite", {}, {
                    execute: { method: 'GET', params: {} }
                }),
                ListBookmarked: $resource(config.apiUrl + "/apielement/ListBookmarked", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                })               
            };
        }]);
