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

    /**
     * This function will be the primary interface between angular controller
     * and meaasurements api. Should contruct a http string using db name, api name
     * and options 
     * @param {string} api - the name of the api being called eg. 'meausrements'
     * @param {string} dbName - the name of the database being queried
     * @param {object} options - jsonObject containing additional data to add to the query 
     * @param {function} callback - callbackFunction evoked to return the data from this function  
     * @param {function} errorCallback - callback function evoked under circumstances of an error
     *
     */
    this.getFromAPIWithOptions = function (api, dbName, options, callback, errorCallBack) {
        var httpString = '/api/' + api + '/' + dbName + '/?';

        for (var option in options) {
            if(options.hasOwnProperty(option)){
                httpString += (option + '=' + options[option] + '&');
            }
        }

        $http.get(httpString)
        .then(
          function (returned) {
              callback(returned);
          },
          function (error) {
              errorCallBack(error);
          }
      )

        

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