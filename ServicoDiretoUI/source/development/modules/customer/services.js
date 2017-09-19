
angular
    .module('CustomerApp')
    .factory('CustomerService', ['$resource', 'config', 'Upload',
        function ($resource, config, Upload){
            'use strict';
            return {
                GetCustomer: $resource(config.apiUrl + "/apicustomer/GetCustomerAsync", {}, {
                    query: { method: 'GET', params: {} }
                }),
                SaveCustomer: $resource(config.apiUrl + "/apicustomer/SaveCustomer", {}, {
                    execute: { method: 'POST', params: {} },
                }),
                ListProperties: $resource(config.apiUrl + "/apicustomer/ListProperties", {}, {
                    query: { method: 'POST', params: {} },
                }),
                SendEmailRequestInfo: $resource(config.apiUrl + "/apicustomer/SendEmailRequestInfo", {}, {
                    execute: { method: 'POST', params: {} },
                }),
                SendEmailContact: $resource(config.apiUrl + "/apicustomer/SendEmailContact", {}, {
                    execute: { method: 'POST', params: {} },
                })
            };
        }]);

