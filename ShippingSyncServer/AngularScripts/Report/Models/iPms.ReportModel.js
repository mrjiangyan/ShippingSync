"use strict";

define(["iPms.Application", "Common/Models/iPms.CommonModel"], function (application) {
    application.factory("iPms.Report.Model", ["iPms.Base.Model", function (baseModel) {
        //概况报表
        var profileReportSearchModel = function () {
            return {
                source: "",
                day:""
            };
        };
        //状态统计报表
        var statusStatisticsReportSearchModel = function () {
            return angular.extend(new baseModel.basePageModel(), {
                startDate: "",
                endDate: "",
                city: "",
                source: ""
            });
        };
        //门店列表
        var hotelListReportSearchModel = function () {
            return angular.extend(new baseModel.basePageModel(), {
                startDate: "",
                endDate: "",
                city: "",
                source: "",
                status: ""
            });
        };
        //单店营业统计
        var hotelStatisticsReportSearchModel = function () {
            return angular.extend(new baseModel.basePageModel(), {
                startDate: "",
                endDate: "",
                OwnerId: "",
                OrgId: "",
                source: "",
            });
        };
        //得到酒店ID
        var hotelGetHotelIdSearchModel = function () {
            return {
                hotelName:""
            };
        };
        //得到城市名称
        var hotelGetCitySearchModel = function () {
            return {
                cityName:""
            };
        };
        //城市统计
        var cityStatisticsReportSearchModel = function () {
            return angular.extend(new baseModel.basePageModel(), {
                endDate: "",
                city: "",
                source: ""
            });
        };
        //EB预订单分析统计
        var orderingAnalysisReportSearchModel = function () {
            return angular.extend(new baseModel.basePageModel(), {
                endDate: "",
                city: "",
                source:""
            });
        };
        return {
            profileReportSearchModel:profileReportSearchModel,
            statusStatisticsReportSearchModel: statusStatisticsReportSearchModel,
            hotelListReportSearchModel: hotelListReportSearchModel,
            hotelStatisticsReportSearchModel: hotelStatisticsReportSearchModel,
            hotelGetHotelIdSearchModel: hotelGetHotelIdSearchModel,
            hotelGetCitySearchModel: hotelGetCitySearchModel,
            cityStatisticsReportSearchModel: cityStatisticsReportSearchModel,
            orderingAnalysisReportSearchModel: orderingAnalysisReportSearchModel
        };
    }]);
});