angular
    .module('LocationApp')
    .factory('LocationService', ['$resource', 'config',
        function ($resource, config) 
        {
            return {
                ListCountry: $resource(config.apiUrl + "/apilocation/listcountry", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),
                ListStateProvince: $resource(config.apiUrl + "/apilocation/liststateprovince", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                }),                
                ListCity: $resource(config.apiUrl + "/apilocation/listCity", {}, {
                    query: { method: 'GET', params: {}, isArray: true }
                })
            };
        }]);
