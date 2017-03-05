"use strict";

define(["iPms.Application"], function (application) {
    application.factory("iPms.Base.Model", function () {
        var basePageModel = function () {
            return {
                PageSize: 15,
                PageIndex: 0,
                SortField: null,
                IsDesc: false
            };
        };      

        return {
            basePageModel: basePageModel
        };
    });
});