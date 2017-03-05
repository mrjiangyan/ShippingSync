"use strict";

define(["iPms.Application", "Common/Controllers/iPms.CommonController", "Report/Services/iPms.ReportService", "Report/Models/iPms.ReportModel"], function (application) {
    application.controller("iPms.Report.StatusStatisticsReportController", ["$scope", "$rootScope", "$routeParams", "$location", "$modal", "$filter", "iPms.CommonController", "iPms.Report.Service", "iPms.Report.Model", function ($scope, $rootScope, $routeParams, $location, $modal, $filter, commonController, reportService, reportModel) {
        var tmpCurrentDate = moment().format("YYYY-MM-DD");
        var tmpStartDate = moment().add(-1, "months").format("YYYY-MM-DD");
        $scope.searchModel = new reportModel.statusStatisticsReportSearchModel();
        $scope.searchModel.startDate = tmpStartDate;
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
        $scope.dateSel = { show: false }

        getStatusStatisticsReport();

        //时间选择
        $scope.changeDateRange = function (range, $event) {
            $scope.searchModel.endDate = moment().format("YYYY/MM/DD");
            if (range == "yesterDay") {
                $scope.dateRange = "yesterDay";
                $scope.searchModel.startDate = moment().add(-1, "days").format("YYYY/MM/DD");
            }
            else if (range == "sevenDay") {
                $scope.dateRange = "sevenDay";
                $scope.searchModel.startDate = moment().add(-7, "days").format("YYYY/MM/DD");
            }
            else if (range == "thirtyDay") {
                $scope.dateRange = "thirtyDay";
                $scope.searchModel.startDate = moment().add(-30, "days").format("YYYY/MM/DD");
            }
            $event.stopPropagation();//阻止冒泡到body
        };
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
            getStatusStatisticsReport();
        };
        //选择城市
        $scope.choiceCity = function (city) {
            $scope.searchModel.PageNum = 1;
            $scope.searchModel.city = city;
            $scope.cityList = null;
            getStatusStatisticsReport();
        };

        
        //加载城市搜索框
        $scope.changeCity = function () {
            
            reportService.getGetCityReport($scope.searchModel.city, function (data) {
                $scope.cityList = data;
            });
            if ($scope.searchModel.city == "" || $scope.searchModel.city==null) {
                $scope.cityList = null;
            }
            getStatusStatisticsReport();
        };

      


        $scope.$watch("searchModel.PageNum", getStatusStatisticsReport);

        function getStatusStatisticsReport() {
            
            //if ($scope.searchModel.PageNum != 1) {
            //    $scope.searchModel.PageNum = 1;
            //}
            $scope.searchModel.PageIndex = $scope.searchModel.PageNum - 1;
            reportService.getStatusStatisticsReport($scope.searchModel, function (data, totalCount) {
                $scope.statusStatisticsList = data;
                $scope.dateSel.show = false;
                if (totalCount % $scope.searchModel.PageSize > 0) {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize) + 1;
                } else {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize);
                }
            });
        }

        //$("body").click(function () {
            
        //    $scope.cityList = null;
        //});
        $scope.hideCity = function ($event) {
            $scope.cityList = null;
        };

        $scope.openCalendar = function ($event) {
            $scope.dateSel.show = !$scope.dateSel.show;
        };

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
        $scope.startDateCalendarChanged = function ($event) {
            $scope.minDate = $scope.searchModel.startDate;
        };
        $scope.searchStatusStatisticsReport = function () {
            if (!$scope.searchModel.startDate) {
                alert("请选择开始日期");
                return;
            }
            if (!$scope.searchModel.endDate) {
                alert("请选择结束日期");
                return;
            }
            $scope.searchModel.startDate = moment($scope.searchModel.startDate).format("YYYY-MM-DD");
            $scope.searchModel.endDate = moment($scope.searchModel.endDate).format("YYYY-MM-DD");
            getStatusStatisticsReport();
        };

    }]);
});
