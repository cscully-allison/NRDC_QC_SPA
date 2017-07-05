angular.module('QCSpaCore', ['ngRoute'])

.config(function ($routeProvider, $locationProvider) {

    //refactor into state provider for subviews when navigating flag lists
    // flags should go in order of:
    // deployments with flags -> flagSubtypes associated with depoyment (eg. out of bounds or repeated value) -> list of specific flags associated with this subtype
    $routeProvider
        .when('/Home', {
            templateUrl: '/Home/Dashboard/'
        })
        .when('/Graph/:StreamId/:TimeStamp', {

            templateUrl: function (params) {
                 
                var constructedUrl = '/ChartView/Index/?';

                for (param in params) {
                    if (params.hasOwnProperty(param)) {
                       constructedUrl += (param + '=' + params[param] + '&');
                    }
                }

                
                return constructedUrl;
            },
            controller: 'chartViewController',
            controllerAs: 'CVC'
        })

    $locationProvider.html5Mode(false).hashPrefix('!');
});