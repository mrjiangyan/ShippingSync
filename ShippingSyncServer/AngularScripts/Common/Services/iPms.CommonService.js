"use strict";

define(function () {
    var iPmsGlobalService = angular.module("iPms.CommonService", []);
    iPmsGlobalService.factory("iPms.Common.Service", ["$resource", "$rootScope", "$document", "$compile", "$timeout", "$interval", "$cookieStore", function ($resource, $rootScope, $document, $compile, $timeout, $interval, $cookieStore) {
        var commonAjaxHandle = function (returnResult, callback) {
            var tmpResult = null;
            if (returnResult) {
                switch (returnResult.resultCode) {
                    case 200:
                    case 202:                        
                        tmpResult = returnResult.data;

                        if (callback && angular.isFunction(callback)) {
                            callback(tmpResult, returnResult.TotalCount);
                        };
                        break;
                    case 401:
                        showMessage("请登录");
                        $rootScope.isLogin = false;
                        $cookieStore.remove("SessionId");
                        $cookieStore.remove("UserModel");
                        window.location.href = "/#/Login";
                        break;
                    default:
                        showMessage(returnResult.Message);
                        break;
                }
            }

            setTimeout(function () {
                $("body").find("[data-ajaxDisabled='true']").prop("disabled", false).removeAttr("dark-but").removeClass("dark-but");
            }, 300);
        };

        var commonAjaxErrorHandle = function (returnResult) {
            if (returnResult && returnResult.data) {
                var tmpReturnResult = returnResult.data;

                switch (tmpReturnResult.resultCode) {
                    case 401:
                        showMessage("请登录");
                        $rootScope.isLogin = false;
                        $cookieStore.remove("SessionId");
                        $cookieStore.remove("UserModel");
                        window.location.href = "/#/Login";
                        break;
                    case 409:
                        if (tmpReturnResult && tmpReturnResult.Message) {
                            showMessage(tmpReturnResult.Message);
                        }
                        break;
                    default:
                        break;
                }
            }

            setTimeout(function () {
                $("body").find("[data-ajaxDisabled='true']").prop("disabled", false).removeAttr("dark-but").removeClass("dark-but");
            }, 300);
        };

        var ajaxGet = function (getUrl, getParam, callback) {
            $("body").find("[data-ajaxDisabled='true']").prop("disabled", true).attr("disabled", true);

            var getResource = $resource(getUrl, {}, { "get": { method: "GET", headers: { AccessKeyId: CurrentConfig.AccessKeyId, AppId: CurrentConfig.AppId } } });
            getResource.get(getParam, function (returnResult) {
                commonAjaxHandle(returnResult, callback);
            }, function (returnResult) {
                commonAjaxErrorHandle(returnResult);
            });
        };

        var ajaxQuery = function (queryUrl, queryParam, callback) {
            $("body").find("[data-ajaxDisabled='true']").prop("disabled", true).attr("disabled", true);

            var queryResource = $resource(queryUrl, {}, { "query": { method: "GET", headers: { AccessKeyId: CurrentConfig.AccessKeyId, AppId: CurrentConfig.AppId } } });

            queryResource.query(queryParam, function (returnResult) {
                commonAjaxHandle(returnResult, callback);
            }, function (returnResult) {
                commonAjaxErrorHandle(returnResult);
            });
        };

        var ajaxSave = function (saveUrl, saveParam, callback) {
            $("body").find("[data-ajaxDisabled='true']").prop("disabled", true).attr("disabled", true);

            var saveResource = $resource(saveUrl, {}, { "save": { method: "POST", headers: { AccessKeyId: CurrentConfig.AccessKeyId, AppId: CurrentConfig.AppId } } });
            saveResource.save(saveParam, function (returnResult) {
                commonAjaxHandle(returnResult, callback);
            }, function (returnResult) {
                commonAjaxErrorHandle(returnResult);
            });
        };

        var ajaxPut = function (putUrl, putParam, callback) {
            $("body").find("[data-ajaxDisabled='true']").prop("disabled", true).attr("disabled", true);

            var putResource = $resource(putUrl, {}, { put: { method: "PUT", headers: { AccessKeyId: CurrentConfig.AccessKeyId, AppId: CurrentConfig.AppId } } });
            putResource.put(putParam, function (returnResult) {
                commonAjaxHandle(returnResult, callback);
            }, function (returnResult) {
                commonAjaxErrorHandle(returnResult);
            });
        };

        var ajaxDelete = function (deleteUrl, deleteParam, callback) {
            $("body").find("[data-ajaxDisabled='true']").prop("disabled", true).attr("disabled", true);

            var deleteResource = $resource(deleteUrl, {}, { "delete": { method: "DELETE", headers: { AccessKeyId: CurrentConfig.AccessKeyId, AppId: CurrentConfig.AppId } } });
            deleteResource.delete(deleteParam, function (returnResult) {
                commonAjaxHandle(returnResult, callback);
            }, function (returnResult) {
                commonAjaxErrorHandle(returnResult);
            });
        };

        var getChannels = function (callback) {
            return ajaxGet("/API/iPmsCommon/Channels", null, callback);
        };

        var showMessage = function (message) {
            $rootScope.showMessage(message);
        };

        var showMessages = function (messages) {
            $rootScope.showMessage(messages);
        };

        return {
            ajaxGet: ajaxGet,
            ajaxQuery: ajaxQuery,
            ajaxSave: ajaxSave,
            ajaxPut: ajaxPut,
            ajaxDelete: ajaxDelete,

            getChannels: getChannels,

            showMessage: showMessage,
            showMessages: showMessages
        };
    }]);
});