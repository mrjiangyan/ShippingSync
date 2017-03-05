"use strict";

define(["iPms.Application", "Common/Controllers/iPms.CommonController", "Report/Services/iPms.ReportService", "Report/Models/iPms.ReportModel"], function (application) {
    application.controller("iPms.Report.OrderingAnalysisReportController", ["$scope", "$rootScope", "$routeParams", "$location", "$modal", "$filter", "iPms.CommonController", "iPms.Report.Service", "iPms.Report.Model", function ($scope, $rootScope, $routeParams, $location, $modal, $filter, commonController, reportService, reportModel) {
        var tmpCurrentDate = moment().add(-2, "days").format("YYYY-MM-DD");
        var tmpStartDate = moment(tmpCurrentDate).add(-9, "days").format("YYYY-MM-DD");
        $scope.searchModel = new reportModel.orderingAnalysisReportSearchModel();
        $scope.searchModel.startDate = tmpStartDate;
        $scope.searchModel.endDate = tmpCurrentDate;
        $scope.dateFormat = "yyyy-MM-dd";
        $scope.calendarOpened = false;
        $scope.currentDate = tmpCurrentDate;
        //$scope.minDate = $scope.searchModel.startDate;
        $scope.maxDate = tmpCurrentDate;
        $scope.searchModel.PageNum = 1;
        $scope.searchModel.PageSize = 15;
        $scope.userName = $.cookie('loginInfo');

        $scope.dateSel = { show: false };
        var city = $location.search()['city'];
        if (city != null) {
            $scope.searchModel.city = city;
        }
        if ($location.search()['endDate'] != null) {
            $scope.searchModel.endDate = $location.search()['endDate'];
        }
        if ($location.search()['ReUrl'] == "Profile") {
            $scope.searchModel.source = "Profile"
        }
        

        getOrderingAnalysisReport();

        //选择城市
        $scope.choiceCity = function (city) {

            $scope.searchModel.city = city;
            $scope.cityList = null;
            $scope.searchModel.PageNum = 1;
        };


        //加载城市搜索框
        $scope.changeCity = function () {
            reportService.getGetCityReport($scope.searchModel.city, function (data) {
                $scope.cityList = data;
            });
            if ($scope.searchModel.city == "" || $scope.searchModel.city == null) {
                $scope.cityList = null;
            }
        };

        $scope.hideCity = function ($event) {
            $scope.cityList = null;
        };

        $scope.$watch("searchModel.PageNum", getOrderingAnalysisReport);

        function getOrderingAnalysisReport() {
            
            $scope.searchModel.PageIndex = $scope.searchModel.PageNum - 1;
            reportService.getOrderingAnalysisReport($scope.searchModel, function (data, totalCount) {
                $scope.list = data.list;
                $scope.orderInfo = data.orderInfo;
                $scope.dateSel.show = false;
                if (totalCount % $scope.searchModel.PageSize > 0) {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize) + 1;
                } else {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize);
                }
            });
        }

        $scope.openStartDateCalendar = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.startDateCalendarOpened = true;
            $scope.endDateCalendarOpened = false;
        };

        $scope.openEndDateCalendar = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.startDateCalendarOpened = false;
            $scope.endDateCalendarOpened = true;
        };
        $scope.endDateCalendarChanged = function ($event) {
            $scope.maxDate = $scope.searchModel.endDate;
        };
        $scope.searchOrderingAnalysisReport = function () {
            if (!$scope.searchModel.endDate) {
                alert("请选择结束日期");
                return;
            }
            $scope.searchModel.source = "";
            $scope.searchModel.endDate = moment($scope.searchModel.endDate).format("YYYY-MM-DD");
            $scope.searchModel.startDate = moment($scope.searchModel.endDate).add(-9, "days").format("YYYY-MM-DD");
            $scope.searchModel.PageNum = 1;
            getOrderingAnalysisReport();
        };

    }]);
});