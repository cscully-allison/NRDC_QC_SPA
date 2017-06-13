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

.controller('lineGraphController', ['dataService', '$scope',

    function (dataService, $scope) {
        var vm = this;

        vm.options = {
            chart: {
                type: 'lineChart',
                //height: 450,
                //margin: {
                //    top: 20,
                //    right: 20,
                //    bottom: 40,
                //    left: 55
                //},
                x: function (d) { return d.x; },
                y: function (d) { return d.y; },
                useInteractiveGuideline: true,
                dispatch: {
                    stateChange: function (e) { console.log("stateChange"); },
                    changeState: function (e) { console.log("changeState"); },
                    tooltipShow: function (e) { console.log("tooltipShow"); },
                    tooltipHide: function (e) { console.log("tooltipHide"); }
                },
                xAxis: {
                    axisLabel: 'Time (ms)'
                },
                yAxis: {
                    axisLabel: 'Voltage (v)',
                    tickFormat: function (d) {
                        return d3.format('.02f')(d);
                    },
                    axisLabelDistance: -10
                },
                callback: function (chart) {
                    console.log("!!! lineChart callback !!!");
                }
            },
            title: {
                enable: true,
                text: 'Title for Line Chart'
            }
            /*
            subtitle: {
                enable: true,
                text: 'Subtitle for simple line chart. Lorem ipsum dolor sit amet, at eam blandit sadipscing, vim adhuc sanctus disputando ex, cu usu affert alienum urbanitas.',
                css: {
                    'text-align': 'center',
                    'margin': '10px 13px 0px 7px'
                }
            },
            caption: {
                enable: true,
                html: '<b>Figure 1.</b> Lorem ipsum dolor sit amet, at eam blandit sadipscing, <span style="text-decoration: underline;">vim adhuc sanctus disputando ex</span>, cu usu affert alienum urbanitas. <i>Cum in purto erat, mea ne nominavi persecuti reformidans.</i> Docendi blandit abhorreant ea has, minim tantas alterum pro eu. <span style="color: darkred;">Exerci graeci ad vix, elit tacimates ea duo</span>. Id mel eruditi fuisset. Stet vidit patrioque in pro, eum ex veri verterem abhorreant, id unum oportere intellegam nec<sup>[1, <a href="https://github.com/krispo/angular-nvd3" target="_blank">2</a>, 3]</sup>.',
                css: {
                    'text-align': 'justify',
                    'margin': '10px 13px 0px 7px'
                }
            }
            */

        };


        vm.data = genData();
       

        function genData() {
            sinWave = [];

            for (var i = 0; i < 100; i++) {
                sinWave.push({ x: i, y: Math.sin(i / 10) });
            }

            return [
                {
                    values: sinWave,
                    key: "Sin Wave",
                    color: '#c0ffee'
                }
            ]

        }
    }
])
