"use strict";

define(["iPms.Application", "Common/Services/iPms.CommonService"], function (application) {
    application.factory("iPms.Report.Service", ["$resource", "iPms.Common.Service", "$rootScope", function ($resource, commonService, $rootScope) {
        //获取汇总
        var getProfile = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/Profile/", searchModel, callback);
        };
        //获取新开店数
        var getNewHotel = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/NewHotel/" + searchModel.source + "/", null, callback);
        };
        //获取商家阶段
        var getStage = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/Stage/" + searchModel.source + "/", null, callback);
        };
        //状态统计表
        var getStatusStatisticsReport = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/StatusStatistics/" + searchModel.startDate + "/" + searchModel.endDate + "/", searchModel, callback);
        };
        //门店列表
        var getHotelListReport = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/HotelList/", searchModel, callback);
        };
        //门店状态列表
        var getHotelLivenessListReport = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/HotelLivenessList/" + searchModel.endDate + "/", searchModel, callback);
        };
        //状但店营业统计
        var getHotelStatisticsReport = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/HotelStatistics/" + searchModel.startDate + "/" + searchModel.endDate + "/", searchModel, callback);
        };
        //得到酒店ID
        var getGetHotelIdReport = function (hotelName, callback) {
            commonService.ajaxGet("/api/Report/HotelId/" + hotelName + "/", null, callback);
        };
        //得到城市名称
        var getGetCityReport = function (city, callback) {
            commonService.ajaxGet("/api/Report/City/" + city + "/", null, callback);
        };
        //城市统计
        var getCityStatisticsReport = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/CityStatistics/" + searchModel.endDate + "/", searchModel, callback);
        };
        //EB预订单分析
        var getOrderingAnalysisReport = function (searchModel, callback) {
            commonService.ajaxGet("/api/Report/OrderingAnalysis/", searchModel, callback);
        };
        return {
            getProfile: getProfile,
            getNewHotel: getNewHotel,
            getStage:getStage,
            getStatusStatisticsReport: getStatusStatisticsReport,
            getHotelListReport: getHotelListReport,
            getHotelLivenessListReport:getHotelLivenessListReport,
            getHotelStatisticsReport: getHotelStatisticsReport,
            getGetHotelIdReport: getGetHotelIdReport,
            getGetCityReport: getGetCityReport,
            getCityStatisticsReport: getCityStatisticsReport,
            getOrderingAnalysisReport: getOrderingAnalysisReport
        };
    }]);
});