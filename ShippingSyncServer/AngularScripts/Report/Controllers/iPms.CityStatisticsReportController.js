"use strict";

define(["iPms.Application", "Common/Controllers/iPms.CommonController", "Report/Services/iPms.ReportService", "Report/Models/iPms.ReportModel"], function (application) {
    application.controller("iPms.Report.CityStatisticsReportController", ["$scope", "$rootScope", "$routeParams", "$location", "$modal", "$filter", "iPms.CommonController", "iPms.Report.Service", "iPms.Report.Model", function ($scope, $rootScope, $routeParams, $location, $modal, $filter, commonController, reportService, reportModel) {
        var tmpCurrentDate = moment().add(-2, "days").format("YYYY-MM-DD");
        var tmpStartDate = moment().add(-2, "days").format("YYYY-MM-DD");
        $scope.searchModel = new reportModel.cityStatisticsReportSearchModel();
        $scope.searchModel.endDate = tmpCurrentDate;
        $scope.dateFormat = "yyyy-MM-dd";
        $scope.calendarOpened = false;
        $scope.currentDate = tmpCurrentDate;
        $scope.minDate = $scope.searchModel.startDate;
        $scope.maxDate = tmpCurrentDate;
        $scope.searchModel.PageNum = 1;
        $scope.searchModel.PageSize = 15;
        $scope.userName = $.cookie('loginInfo');

        $scope.dateRange = "yesterDay";
        $scope.source = "All";
        $scope.dateSel = { show: false };
        var source = $location.search()['source'];
        if (source != null) {
            $scope.source = source;
            $scope.searchModel.source = source;
        }
        var city = $location.search()['city'];
        if (city != null) {
            $scope.searchModel.city = city;
        }
        var endDatePar = $location.search()['endDate'];
        if (endDatePar != null) {
            if ($location.search()['endDate'] != null) {
                $scope.searchModel.endDate = $location.search()['endDate'];
            }
        }

        getCityStatisticsReport();

        //按系统
        $scope.changeSource = function (source, $event) {
            if (source == "All") {
                $scope.source = "All";
                $scope.searchModel.source = "";
            }
            else if (source == "IPMS") {
                $scope.source = "";
                $scope.searchModel.source = "IPMS";
            }
            else if (source == "SPMS") {
                $scope.source = "";
                $scope.searchModel.source = "SPMS";
            }
            $event.stopPropagation();//阻止冒泡到body
            $scope.searchModel.PageNum = 1;
        };
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
            getHotelListReport();
        };

        $scope.hideCity = function ($event) {
            $scope.cityList = null;
        };

        $scope.$watch("searchModel.PageNum", getCityStatisticsReport);

        function getCityStatisticsReport() {
            $scope.searchModel.PageIndex = $scope.searchModel.PageNum - 1;
            reportService.getCityStatisticsReport($scope.searchModel, function (data, totalCount) {
                $scope.citySummariesList = data.list;
                $scope.totalInfo = data.totalInfo;
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
        $scope.searchCityStatisticsReport = function () {
            if (!$scope.searchModel.endDate) {
                alert("请选择结束日期");
                return;
            }
            $scope.searchModel.endDate = moment($scope.searchModel.endDate).format("YYYY-MM-DD");
            $scope.searchModel.PageNum = 1;
            getCityStatisticsReport();
        };

    }]);
});