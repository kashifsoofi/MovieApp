!function(){"use strict";angular.module("moviesApp",["moviesServices"])}(),function(){"use strict";function a(a,b){a.movies=b.query()}angular.module("moviesApp").controller("moviesController",a),a.$inject=["$scope","Movies"]}(),function(){"use strict";function a(a){function b(){}var c={getData:b};return c}var b=angular.module("moviesServices",["ngResource"]);b.factory("Movies",["$resource",function(a){return a("/api/movies",{},{query:{method:"GET",params:{},isArray:!0}})}]),angular.module("app").factory("moviesService",a),a.$inject=["$http"]}();