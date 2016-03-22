(function () {
    'use strict';

    angular
        .module('moviesApp')
        .controller('MoviesListController', MoviesListController)
        .controller('MoviesAddController', MoviesAddController)
        .controller('MoviesEditController', MoviesEditController)
        .controller('MoviesDeleteController', MoviesDeleteController);

    /* Movies List Controller  */
    MoviesListController.$inject = ['$scope', 'Movies']; 

    function MoviesListController($scope, Movies) {
        $scope.movies = Movies.query();
    }
    
    /* Movies Create Controller */
    MoviesAddController.$inject = ['$scope', '$location', 'Movies'];
    
    function MoviesAddController($scope, $location, Movies) {
        $scope.movie = new Movie();
        $scope.add = function () {
            $scope.movie.$save(function() {
                $location.path('/');
            });
        }
    }
    
    /* Movies Edit Controller */
    MoviesEditController.$inject = ['$scope', '$routeParams', '$location', 'Movies'];
    
    function MoviesEditController($scope, $routeParams, $location, Movies) {
        $scope.movie = Movies.get({ id: $routeParams.id });
        $scope.edit = function () {
            $scope.movie.$save(function () {
                $location.path('/');
            });
        };
    }
    
    /* Movies Delete Controller  */
    MoviesDeleteController.$inject = ['$scope', '$routeParams', '$location', 'Movies'];
 
    function MoviesDeleteController($scope, $routeParams, $location, Movies) {
        $scope.movie = Movies.get({ id: $routeParams.id });
        $scope.remove = function () {
            $scope.movie.$remove({id:$scope.movie.Id}, function () {
                $location.path('/');
            });
        };
    }
    
})();
