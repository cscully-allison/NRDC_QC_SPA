angular.module('QCSpaCore')

.controller('HomePageNavController', ['dataService', 'NavService', '$scope',

    function (dataService, NavService, $scope, $route, $routeParams, $location) {

        /**********************
        Function Declarations
        **********************/

        var vm = this;

        vm.navData = [];
        vm.selectedDB;
        vm.fetchNextDataSet = [fetchSiteNetworks, fetchSites, fetchSystems, fetchDeployments, fetchDataStreams];
        vm.liveNavService = NavService;

        $scope.test = "Yep this is bound.";




        /************************
        Function Definitions
        *************************/

        vm.test = function () {
            console.log("Does the controller come over");
            console.log(vm.liveNavService.currentLevel);
        }



        /*
         *  Name: Fetch Site Networks
         *  Asynchronously fetches list of site networks for user to select from
         *  Requires DB to be selected by user
         */

        function fetchSiteNetworks() {
            console.log(vm.liveNavService.level);
            dataService.getFromAPI('SiteNetworks', vm.selectedDB,
                function (response) {   //success callback
                    vm.navData.push(response.data);
                    console.log(vm.navData[0]);
                },
                function () {   //failure callback
                    vm.navData.push(null);
                    console.log("Database Connection Error");
                })
        };

        /*
         *  Name: Fetch Sites with context of a site network
         *  Asynchronously fetches list of sites for a given network
         *  Requires DB to be selected by user
         */
        function fetchSites(siteNetwork) {
            console.log(vm.liveNavService.level);
            //look up site network from previous level and get associated
            //id for api call
            var id = dataService.getIdFromName(vm.navData[0], 'Network', siteNetwork);

            //use getFromApiWithParentID service to populate list of
            //sites
            dataService.getFromAPIWithParentID('Sites', vm.selectedDB, id,
                function (response) {
                    vm.navData.push(response.data);
                },
                function () {
                    vm.navData.push(null);
                    console.log("Database Connection Error");
                });
        }

        function fetchSystems(site) {

            //look up site network and get associated id
            var id = dataService.getIdFromName(vm.navData[1], 'Site', site);


            //use getFromApiWithParentID service to populate list of
            //sites
            dataService.getFromAPIWithParentID('Systems', vm.selectedDB, id,
                function (response) {
                    vm.navData.push(response.data);
                },
                function () {
                    vm.navData.push(null);
                    console.log("Database Connection Error");
                });

        }

        /**
          * Asynchrously fetches deployments assocaited with a certian system
          *
          */

        function fetchDeployments(system) {

            //look up site network and get associated id
            var id = dataService.getIdFromName(vm.navData[2], 'System', system);

            console.log(id);

            //use getFromApiWithParentID service to populate list of
            //sites
            dataService.getFromAPIWithParentID('Deployments', vm.selectedDB, id,
                function (response) {
                    vm.navData.push(response.data);
                },
                function () {
                    vm.navData.push(null);
                    console.log("Database Connection Error");
                });

        }

        /**
          * Asynchrously fetches data streams assocaited with a certian deployment
          *
          */

        function fetchDataStreams(deployment) {

            //look up site network and get associated id
            var id = dataService.getIdFromName(vm.navData[3], 'Deployment', deployment);

            //use getFromApiWithParentID service to populate list of
            //sites
            dataService.getFromAPIWithParentID('DataStreams', vm.selectedDB, id,
                function (response) {
                    vm.navData.push(response.data);
                },
                function () {
                    vm.navData.push(null);
                    console.log("Database Connection Error");
                });

            $location.path('#!/Graph/');
        }
    }


])

.controller('routeController', ['$location', '$scope',

    function ($location, $scope) {

        $scope.setPath = function (StreamID, TimeStamp) {
            $location.path('Graph/' + StreamID + '/' + TimeStamp);

        }

        var initPath = function () {
            $location.path('Home/');
        }

        initPath();
    }
])


.controller('flagListController', ['dataService', function (dataService) {
    var vm = this;

    //options for querying for flags
    var options = {}
    vm.flags = []



    dataService.getFromAPIWithOptions('flags', 'protoNRDC', options,
            function (response) {
                vm.flags = response.data;
            },
            function (e) { console.log(e); }
        );

}])

.controller('chartViewController', [ '$scope', 'dataService', function ($scope, dataService) {
    var vm = this;
    vm.dataContext = {
    };
    var options = {
        DataStreamId: '54',
        StartTime: '2014-06-15 04:00:00',
        EndTime: '2014-06-22 05:00:00'
    };

    vm.queriedData = { data: [] };


    vm.injectDataContext = function (DataStreamId, StartTime, EndTime) {
        vm.dataContext.DataStreamId = DataStreamId;
        vm.dataContext.StartTime = StartTime;
        vm.dataContext.EndTime = EndTime;

        console.log(vm.queriedData);

        //get data using the current context
        dataService.getFromAPIWithOptions('measurements', 'protoNRDC', vm.dataContext,
            function (response) {
                vm.queriedData.data = response.data;
            },
            function (e) {
                console.log(e);
            }
        );

    }

}])