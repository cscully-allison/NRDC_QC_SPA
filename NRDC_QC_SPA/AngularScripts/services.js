angular.module('QCSpaCore')

//HTTP/API calls service 
.service('dataService', function ($http) {

    //given an array of returned rows from an entity
    //get the primary key using the name provided
    //and type of entity
    this.getIdFromName = function (rows, entity, selectedName){
        console.log(rows[selectedName]);
        return rows[selectedName][entity + 'ID'];
    }

    //constructs and fetches basic get from api
    this.getFromAPI = function (api, dbName, callbackFunct, errorCallBack) {
        console.log('/api/' + api + '/' + dbName);
        $http.get('/api/' + api + '/' + dbName)
        .then(
            //success callback
            function (data) {
                callbackFunct(data);
            },
            //error callback
            function (error) {
                errorCallBack()
            });
    }

    //get data from an api with a dbName
    // and an id associated with the "parent"
    // entity
    this.getFromAPIWithParentID = function (api, dbName, id, callback, errorCallback) {
        $http.get('/api/' + api + '/' + dbName + '/' + id)
        .then(
            function (returned) {
                callback(returned);
            },
            function (error) {
                errorCallback();
            }
        )
    }

    this.getFromAPIWithOptions = function (api, dbName, options, callback, errorCallBack) {
        //this will be used principally for
        // calls to database data
    }



})

.service('NavService', function () {
    this.baseLevel = 4;
    this.topLevel = 0;
    this.currentLevel = 0;

    //safe incrementation function
    this.incrementLevel = function () {
        if (this.currentLevel < this.baseLevel) {
            this.currentLevel++;
        }
    }

    //safe fetch child level function
    this.getChildLevel = function () {
        if ((this.currentLevel + 1) < this.baseLevel) {
            return (this.currentLevel + 1);
        }
    }
});