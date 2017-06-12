angular.module('QCSpaCore')

.directive('barChart', barChart);


function barChart($parse) {
    var directive =  {
        restrict: 'E',
        replace: 'false',
        link: link,
        controller: 'barGraphController',
        controllerAs: 'BGC',
        bindToController: true
    };

    
    return directive;


    /**Link Implementation**/
    function link (scope, element, attrs) {
        d3.select(element[0])
          .selectAll("div")
          .data(scope.BGC.data)
          .enter()
          .append("div")
          .text(function (dp) { return "I'm datapoint: " + dp })
    }
    
}