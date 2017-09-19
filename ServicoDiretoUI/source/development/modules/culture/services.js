//http://stackoverflow.com/questions/17160771/angularjs-a-service-that-serves-multiple-resource-urls-data-sources

angular
    .module('CultureApp')
    .factory("CultureService", function ($resource) {
    return $resource(
        CONFIG.SERVICE_URL + "/culture/:Id",
        { Id: "@Id" },
        {
            'save': { method: 'POST' },
            "update": { method: "PUT" }, //custom
            "reviews": { 'method': 'GET', 'params': { 'reviews_only': "true" }, isArray: true } //custom
        }
    );
});