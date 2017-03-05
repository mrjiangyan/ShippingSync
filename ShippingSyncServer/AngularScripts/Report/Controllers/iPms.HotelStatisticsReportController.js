"use strict";

define(["iPms.Application", "Common/Controllers/iPms.CommonController", "Report/Services/iPms.ReportService", "Report/Models/iPms.ReportModel"], function (application) {
    application.controller("iPms.Report.HotelStatisticsReportController", ["$scope", "$rootScope", "$routeParams", "$location", "$modal", "$filter", "iPms.CommonController", "iPms.Report.Service", "iPms.Report.Model", function ($scope, $rootScope, $routeParams, $location, $modal, $filter, commonController, reportService, reportModel) {
        var tmpCurrentDate = moment().add(-2, "days").format("YYYY-MM-DD");
        var tmpStartDate = moment().add(-32, "days").format("YYYY-MM-DD");
        $scope.searchModel = new reportModel.hotelStatisticsReportSearchModel();
        $scope.searchModel.startDate = tmpStartDate;
        $scope.searchModel.endDate = tmpCurrentDate;
        $scope.dateFormat = "yyyy-MM-dd";
        $scope.calendarOpened = false;
        $scope.currentDate = tmpCurrentDate;
        $scope.minDate = $scope.searchModel.startDate;
        $scope.maxDate = tmpCurrentDate;
        $scope.searchModel.PageNum = 1;
        $scope.searchModel.PageSize = 10;
        $scope.userName = $.cookie('loginInfo');

        //$scope.dateRange = "yesterDay";
        $scope.dateSel = { show: false }
        
        var hotelName = $location.search()['hotelName'];
        if (hotelName != null) {
            $scope.hotelName = hotelName;
        }
        var OrgId = $location.search()['OrgId'];
        if (OrgId != null) {
            $scope.searchModel.OrgId = OrgId;
        }
        var OwnerId = $location.search()['OwnerId'];
        if (OwnerId != null) {
            $scope.searchModel.OwnerId = OwnerId;
        }
        var source = $location.search()['source'];
        if (source != null) {
            $scope.searchModel.source = source;
        }
        var parEndDate = $location.search()['endDate'];
        var parStartDate = $location.search()['startDate'];
        if (parEndDate != null) {
            $scope.searchModel.startDate = parEndDate;
            if (parStartDate != null && parStartDate != "") {
                $scope.minDate = parStartDate;
                $scope.searchModel.startDate = parStartDate;
            }
            $scope.maxDate = parEndDate;
            $scope.minDate = parEndDate;
            $scope.searchModel.endDate = parEndDate;
        }

        getHotelStatisticsReport();

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
        //选择城市
        $scope.choiceHotel = function (ownerId, orgId, hotelName, source) {
            $scope.hotelName = hotelName;
            $scope.searchModel.OrgId = orgId;
            $scope.searchModel.OwnerId = ownerId;
            $scope.searchModel.source = source;
            $scope.hotelList = null;
            $scope.searchModel.PageNum = 1;
        };


        //加载城市搜索框
        $scope.changeHotel = function () {

            reportService.getGetHotelIdReport($scope.hotelName, function (data) {
                $scope.hotelList = data;
            });
            if ($scope.hotelName == "" || $scope.hotelName == null) {
                $scope.hotelList = null;
            }
        };

        $scope.$watch("searchModel.PageNum", getHotelStatisticsReport);

        function getHotelStatisticsReport() {
            //if ($scope.searchModel.PageNum != 1) {
            //    $scope.searchModel.PageNum = 1;
            //}
            $scope.searchModel.PageIndex = $scope.searchModel.PageNum - 1;
            reportService.getHotelStatisticsReport($scope.searchModel, function (data, totalCount) {
                $scope.hotelStatisticsList = data.list;
                $scope.hotelInfo = data.hotelInfo;
                $scope.dateSel.show = false;
                if (totalCount % $scope.searchModel.PageSize > 0) {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize) + 1;
                } else {
                    $scope.totalPageNum = parseInt(totalCount / $scope.searchModel.PageSize);
                }
            });
        }
     

        $scope.hideHotel = function ($event) {
            $scope.hotelList = null;
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
        $scope.searchHotelStatisticsReport = function () {
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
            getHotelStatisticsReport();
        };

    }]);
});