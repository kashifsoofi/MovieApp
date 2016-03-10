(function () {
    'use strict';

    var moviesServices = angular.module('moviesServices', ['ngResource']);

    moviesServices.factory('Movies', ['$resource',
        function ($resource) {
            return $resource('/api/movies', {}, {
                query: { method: 'GET', params: {}, isArray: true }
            });
        }]);

    angular
        .module('app')
        .factory('moviesService', moviesService);

    moviesService.$inject = ['$http'];

    function moviesService($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData() { }
    }
})();