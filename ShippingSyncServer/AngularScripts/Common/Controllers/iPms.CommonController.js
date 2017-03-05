"use strict";

define(["iPms.Application", "Common/Services/iPms.CommonService"], function (application) {
    application.factory("iPms.CommonController", ["$rootScope", "iPms.Common.Service", function ($rootScope, commonService) {
        $rootScope.closeModal = function () {
            if ($rootScope.modalInstance) {
                $rootScope.modalInstance.close();
            }
        };

        var formValidate = function (scope, formElement) {
            scope.$broadcast('ng-validate');

            return scope[formElement].$valid;
        };

        var getChannels = function (callback) {
            commonService.getChannels(callback);
        };

        var showMessage = function (message) {
            commonService.showMessage(message);
        };

        var showMessages = function (messages) {
            commonService.showMessages(messages);
        };

        var printReport = function (templateDir, reportName, postDatas, target) {
            var self = this,
                reportUrl = $rootScope.reportUrl + "/Report/Print";

            if (target && $("#" + target).length == 0) {
                $('<iframe id="' + target + '" name="' + target + '" style="width:0;height:0;border:0px solid #fff;"></iframe>').appendTo('body');
            }

            target = target || "_blank";
            var printType = "PrintPdf";
            if (templateDir == 'receipt') {
                printType = /msie/.test(navigator.userAgent.toLowerCase()) ? "PrintHtml" : "PrintPdf";
                target = '_blank';
            }

            var form = $('<form method="POST" target="' + target + '" />').attr('action', reportUrl);
            form.append('<input type="hidden" name="templatedir" value="' + templateDir + '" />');
            form.append('<input type="hidden" name="reportname" value="' + reportName + '" />');
            form.append('<input type="hidden" name="printaction" value="' + printType + '" />');

            for (var postData in postDatas) {
                form.append('<input type="hidden" name="' + postData + '" value="' + postDatas[postData] + '" />');
            }
            form.appendTo('body');
            form.submit().remove();
        };

        return {
            formValidate: formValidate,

            getChannels: getChannels,

            showMessage: showMessage,
            showMessages: showMessages,

            printReport: printReport
        }
    }]);
});