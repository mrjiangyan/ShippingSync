"use strict";

define(function () {
    return {
        defaultRoutePath: "/",
        routes: {
            "/": {
                templateUrl: "/AngularScripts/Report/Templates/ProfileReport.html",
                dependencies: [
                    "Report/Controllers/iPms.ProfileReportController"
                ]
            },
            "/ProfileReport": {
                templateUrl: "/AngularScripts/Report/Templates/ProfileReport.html",
                dependencies: [
                    "Report/Controllers/iPms.ProfileReportController"
                ]
            },
            "/StatusStatisticsReport": {
                templateUrl: "/AngularScripts/Report/Templates/StatusStatisticsReport.html",
                dependencies: [
                    "Report/Controllers/iPms.StatusStatisticsReportController"
                ]
            },
            "/HotelListReport": {
                templateUrl: "/AngularScripts/Report/Templates/HotelListReport.html",
                dependencies: [
                    "Report/Controllers/iPms.HotelListReportController"
                ]
            },
            "/HotelLivenessListReport": {
                templateUrl: "/AngularScripts/Report/Templates/HotelLivenessListReport.html",
                dependencies: [
                    "Report/Controllers/iPms.HotelLivenessListReportController"
                ]
            },
            "/HotelStatisticsReport": {
                templateUrl: "/AngularScripts/Report/Templates/HotelStatisticsReport.html",
                dependencies: [
                    "Report/Controllers/iPms.HotelStatisticsReportController"
                ]
            },
            "/CityStatisticsReport": {
                templateUrl: "/AngularScripts/Report/Templates/CityStatisticsReport.html",
                dependencies: [
                    "Report/Controllers/iPms.CityStatisticsReportController"
                ]
            },
            "/OrderingAnalysisReport": {
                templateUrl: "/AngularScripts/Report/Templates/OrderingAnalysisReport.html",
                dependencies: [
                    "Report/Controllers/iPms.OrderingAnalysisReportController"
                ]
            }
        }
    };
});