(function () {
    'use strict';

    angular
        .module('moviesServices', ['ngResource'])
        .factory('Movies', Movies);
        
    Movies.$inject = ['$resource'];

    function Movies($resource) {
        return $resource('/api/movies/:id');
    }
})();