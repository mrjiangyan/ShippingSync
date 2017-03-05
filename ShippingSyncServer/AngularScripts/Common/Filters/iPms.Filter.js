"use strict";

define(function () {
    var iPmsFilters = angular.module('iPms.Filters', []);

    iPmsFilters.filter('enumValue', function ($rootScope) {
        return function (enumValue, enumType) {
            if (enumValue) {
                var tmp = $rootScope.businessEnum[enumType].filter(function (e) {
                    if (e.Value == enumValue) {
                        return true;
                    } else {
                        return false;
                    }
                });
                if (tmp && tmp[0]) {
                    return tmp[0].Description;
                }
                return "";
            } else {
                return "";
            }
        }
    });
    iPmsFilters.filter('enumString', function ($rootScope) {
        return function (enumString, enumType) {
            if (enumString) {
                var tmp = $rootScope.businessEnum[enumType].filter(function (e) {
                    if (e.EnumString == enumString) {
                        return true;
                    } else {
                        return false;
                    }
                });
                if (tmp && tmp[0]) {
                    return tmp[0].Description;
                }
                return "";
            } else {
                return "";
            }
        }
    });
});