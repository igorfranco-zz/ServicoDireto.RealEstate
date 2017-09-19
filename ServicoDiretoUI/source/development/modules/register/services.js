//http://stackoverflow.com/questions/17160771/angularjs-a-service-that-serves-multiple-resource-urls-data-sources

angular
    .module('CultureApp')
    .factory("CultureService", function ($resource) {
    return $resource(
        "http://localhost:9090/api/culture/:Id",
        { Id: "@Id" },
        {
            'save': { method: 'POST' },
            "update": { method: "PUT" }, //custom
            "reviews": { 'method': 'GET', 'params': { }, isArray: true } //custom
        }
    );
});