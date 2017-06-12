angular.module('QCSpaCore', ['ngRoute', 'nvd3'])

.config(function ($routeProvider, $locationProvider) {

    $routeProvider
        .when('/Graph', {
            templateUrl: 'Home/GraphView/',
            controller: 'HomePageNavController'
        })
        .when('/Options', {
            templateUrl: 'Home/OptionsView/',
            controller: 'HomePageNavController'
        })

    $locationProvider.html5Mode(false).hashPrefix('!');
});