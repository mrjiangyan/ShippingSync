"use strict";

define(["iPms.Application", "Common/Controllers/iPms.CommonController", "Report/Services/iPms.ReportService", "Report/Models/iPms.ReportModel"], function (application) {
    application.controller("iPms.Report.HotelListReportController", ["$scope", "$rootScope", "$routeParams", "$location", "$modal", "$filter", "iPms.CommonController", "iPms.Report.Service", "iPms.Report.Model", function ($scope, $rootScope, $routeParams, $location, $modal, $filter, commonController, reportService, reportModel) {
        var tmpCurrentDate = moment().add(-2, "days").format("YYYY-MM-DD");
        var tmpStartDate = moment().add(-2, "days").format("YYYY-MM-DD");
        $scope.searchModel = new reportModel.hotelListReportSearchModel();
        $scope.searchModel.endDate = tmpCurrentDate;
        $scope.dateFormat = "yyyy-MM-dd";
        $scope.calendarOpened = false;
        $scope.currentDate = tmpCurrentDate;
        $scope.maxDate = tmpCurrentDate;
        $scope.searchModel.PageNum = 1;
        $scope.searchModel.PageSize = 15;
        $scope.userName = $.cookie('loginInfo');

        $scope.dateRange = "yesterDay";
        $scope.source = "All";
        $scope.status = "All";
        $scope.searchModel.source = "";
        $scope.dateSel = { show: false };
        var source = $location.search()['source'];
        if (source != null) {
            $scope.source = source;
            $scope.searchModel.source = source;
        }
        var status = $location.search()['status'];
        if (status != null) {
            $scope.status = status;
            $scope.searchModel.status = status;
        }
        var city = $location.search()['city'];
        if (city != null) {
            $scope.searchModel.city = city;
        }
        var parEndDate = $location.search()['endDate'];
        if (parEndDate != null) {
            $scope.searchModel.endDate = parEndDate;
        }

        getHotelListReport();

      
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

        //按状态
        $scope.changeLevel = function (status, $event) {
            if (status == "All") {
                $scope.status = "All";
                $scope.searchModel.status = "";
            }
            else if (status == "Register") {
                $scope.status = "";
                $scope.searchModel.status = "Register";
            }
            else if (status == "DailyActive") {
                $scope.status = "";
                $scope.searchModel.status = "DailyActive";
            }
            else if (status == "Warning") {
                $scope.status = "";
                $scope.searchModel.status = "Warning";
            }
            $event.stopPropagation();//阻止冒泡到body
            $scope.searchModel.PageNum = 1;
        };
        //选择城市
        $scope.choiceCity = function (city) {

            $scope.searchModel.city = city;
            $scope.cityList = null;
           // $scope.searchModel.PageNum = 1;
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

        $scope.$watch("searchModel.PageNum", getHotelListReport);

        function getHotelListReport() {
            $scope.searchModel.PageIndex = $scope.searchModel.PageNum - 1;
            reportService.getHotelListReport($scope.searchModel, function (data, totalCount) {
                $scope.hotelList = data.list;
                $scope.totalInfo = data.totalInfo;
                $scope.dateSel.show = false;
                if (totalCount % $scope.searchModel.PageSize > 0) {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize) + 1;
                } else {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize);
                }
            });
        }
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
        $scope.searchHotelListReport = function () {
            if (!$scope.searchModel.endDate) {
                alert("请选择结束日期");
                return;
            }
            $scope.searchModel.endDate = moment($scope.searchModel.endDate).format("YYYY-MM-DD");
            $scope.searchModel.PageNum = 1;
            getHotelListReport();
        };

    }]);
});