angular
    .module('StructureApp')
    .factory('StructureService', ['$resource', 'config',
        function ($resource, config) 
        {
            return {
                ListPurpose: $resource(config.apiUrl + "/apistructure/listpurpose", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListCategory: $resource(config.apiUrl + "/apistructure/listcategory", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListType: $resource(config.apiUrl + "/apistructure/listtype", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
            };
        }]);
