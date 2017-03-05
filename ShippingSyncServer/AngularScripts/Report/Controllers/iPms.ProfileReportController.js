"use strict";

define(["iPms.Application", "Common/Controllers/iPms.CommonController", "Report/Services/iPms.ReportService", "Report/Models/iPms.ReportModel"], function (application) {
    application.controller("iPms.Report.ProfileReportController", ["$scope", "$rootScope", "$routeParams", "$location", "$modal", "$filter", "iPms.CommonController", "iPms.Report.Service", "iPms.Report.Model", function ($scope, $rootScope, $routeParams, $location, $modal, $filter, commonController, reportService, reportModel) {
        var tmpCurrentDate = moment().add(-2, "days").format("YYYY-MM-DD");
        var tmpStartDate = moment().add(-31, "days").format("YYYY-MM-DD");
        $scope.searchModel = new reportModel.hotelListReportSearchModel();
        $scope.searchModel.startDate = tmpStartDate;
        $scope.searchModel.endDate = tmpCurrentDate;
        $scope.dateFormat = "yyyy-MM-dd";
        $scope.calendarOpened = false;
        $scope.currentDate = tmpCurrentDate;
        $scope.minDateThreeMonths = moment().add(-3, "months").format("YYYY-MM-DD");
        $scope.minDate = $scope.searchModel.startDate;
        $scope.maxDate = tmpCurrentDate;
        $scope.searchModel.PageNum = 1;
        $scope.searchModel.PageSize = 15;
        $scope.userName = $.cookie('loginInfo');
        $scope.dateSel = { show: false };
        $scope.source = "IPMS";
        var source = $location.search()['source'];
        if (source != null) {
            $scope.source = source;
            $scope.searchModel.source = source;
        }
 

        //时间选择
        $scope.changeDateRange = function (range, $event) {
            $scope.searchModel.endDate = moment().add(-2, "days").format("YYYY/MM/DD");
            if (range == "yesterDay") {
                $scope.dateRange = "yesterDay";
                $scope.searchModel.startDate = moment().add(-3, "days").format("YYYY/MM/DD");
            }
            else if (range == "sevenDay") {
                $scope.dateRange = "sevenDay";
                $scope.searchModel.startDate = moment().add(-9, "days").format("YYYY/MM/DD");
            }
            else if (range == "thirtyDay") {
                $scope.dateRange = "thirtyDay";
                $scope.searchModel.startDate = moment().add(-32, "days").format("YYYY/MM/DD");
            }
            $event.stopPropagation();//阻止冒泡到body
        };

        getProfileReport();
        $scope.$watch("searchModel.PageNum", getProfileReport);

        function getProfileReport()
        {
            $scope.searchModel.PageIndex = $scope.searchModel.PageNum - 1;
            reportService.getProfile($scope.searchModel, function (data, totalCount) {
                $scope.profileList = data;
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
        $scope.startDateCalendarChanged = function ($event) {
            $scope.minDate = $scope.searchModel.startDate;
        };
        $scope.openCalendar = function ($event) {
            $scope.dateSel.show = !$scope.dateSel.show;
        };


        $scope.searchProfileReport = function () {
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
            $scope.searchModel.PageNum = 1;
            getProfileReport();
        };

        
    }]);
});