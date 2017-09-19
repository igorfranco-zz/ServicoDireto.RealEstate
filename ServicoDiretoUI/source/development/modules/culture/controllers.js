
angular.module('CultureApp')
    .controller('CultureController', ['$scope', 'CultureService', function ($scope, CultureService) {
        $scope.cultures = []
        $scope.culture = null;
        $scope.error = null
        //Alternar templates
        $scope.templates =
            [{ name: 'create', url: '/app/modules/culture/views/create.html' },
             { name: 'list', url: '/app/modules/culture/views/list.html' }];

        $scope.getTemplateUrl = function (name) {
            for (var i = 0; i < $scope.templates.length; i++) {
                if ($scope.templates[i].name == name)
                    return $scope.templates[i].url;
            }
        };

        $scope.basedate = new Date(2010, 11, 28, 14, 57);

        //$scope.template = $scope.templates[0];        

        $scope.delete = function () {
            $id = $scope.culture.IDCulture;
            CultureService
                .delete({ id: $id })
                .$promise
                    .then(function () {
                        if ($scope.cultures != []) {
                            $scope.cultures.pop($scope.culture);
                            $scope.culture = null;
                        }
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;
                    });
        };

        $scope.save = function () {
            $scope.culture
                .$save()
                    .then(function () {
                        $scope.cultures.push($scope.culture);
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;
                    });
        };

        $scope.update = function () {
            $id = $scope.culture.IDCulture;
            CultureService
                .update({ id: $id }, $scope.culture)
                .$promise
                    .then(function () {
                        $scope.listCulture();
                    },
                    function (reason) //Error
                    {
                        $scope.error = reason;

                    });
        };

        $scope.listCulture = function ()
        {
            CultureService.query()
             .$promise
                    .then(function (item) {
                        $scope.cultures = item;
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };

        $scope.getCulture = function (_id) {
            CultureService.get({}, { 'Id': _id })
                .$promise
                    .then(function (item) {
                        //item.CreateDate =  new Date(item.CreateDate);
                        $scope.culture = item;
                    },
                    function (reason) {
                        $scope.error = reason;
                    });
        };
    }]);
