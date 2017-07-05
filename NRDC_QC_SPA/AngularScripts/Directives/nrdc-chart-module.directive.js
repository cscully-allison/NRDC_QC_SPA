angular.module('QCSpaCore')

//creates an enitity that retrieves the size of the div outside of it
// makes a custom fitted chart
.directive('nrdcChartModule',  nrdcChartModule);

function nrdcChartModule() {
    var directive = {
        link: link,
        restrict: 'E',
        templateUrl: '/Content/Templates/nrdc-chart-module.directive.html',
        controller: 'lineGraphController',
        controllerAs: 'NCM',
        scope:true,
        bindToController: true
    };

    return directive;

    function link(scope, element, attrs, NCM) {
        //fetches height of parent and sets our graph to that height
        NCM.options.chart.height = '' + element.parent()[0].offsetHeight;
    }
}