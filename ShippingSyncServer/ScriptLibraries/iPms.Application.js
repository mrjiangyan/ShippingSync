"use strict";

define(["Common/Routes/iPms.Route", "Common/Resolvers/iPms.DependencyResolver", "Common/Directives/iPms.Directive", "Common/Filters/iPms.Filter", "Common/Services/iPms.CommonService"],
    function (commonRoutes, dependencyResolver, commomDirectives, commonFilters) {
        var application = angular.module("iPms.Application", ["tmh.dynamicLocale", "ngRoute", "ngResource", "ngCookies", "ngSanitize", "ngStorage", "ui.select", "ui.bootstrap.tpls", "ui.bootstrap", "iPms.Directives", "iPms.Filters", "iPms.CommonService"]);
        application.factory("handleResponse", ["$q", "$rootScope", "$timeout", "$cookieStore", function ($q, $rootScope, $timeout, $cookieStore) {
            $rootScope.isLogin = true;

            $rootScope.showMessage = function (message) {
                alert(message);
            };

            $rootScope.signalRHandle = {
                init: function () {
                }
            };

            var delayTime = 1000;//延迟时间，小于此延迟时间不显示loading
            var activeRequests = 0;
            var started = function () {
                $timeout(function () {
                    if (activeRequests == 0) {
                        $rootScope.$broadcast("loadingStatusActive");
                    };
                    activeRequests++;
                }, delayTime);
            };
            var ended = function () {
                activeRequests--;
                if (activeRequests == 0) {
                    $rootScope.$broadcast("loadingStatusInactive");
                };
            };
            return {
                'request': function (config) {//发起请求时
                    started();
                    return config;
                }, 'response': function (response) {//返回请求
                    ended();
                    if (!response.data.Code || response.data.Code != 0) {

                    };
                    return response;
                }, 'responseError': function (rejection) {//返回请求异常时
                    ended();
                    return $q.reject(rejection);
                }
            };
        }]);

        application.config([
            'tmhDynamicLocaleProvider',
            '$routeProvider',
            '$locationProvider',
            '$controllerProvider',
            '$compileProvider',
            '$filterProvider',
            '$provide',
            "$httpProvider",
            function (tmhDynamicLocaleProvider, $routeProvider, $locationProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, $httpProvider) {
                application.controller = $controllerProvider.register;
                application.directive = $compileProvider.directive;
                application.filter = $filterProvider.register;
                application.factory = $provide.factory;
                application.service = $provide.service;
                $httpProvider.interceptors.push("handleResponse");

                tmhDynamicLocaleProvider.localeLocationPattern('/ScriptLibraries/Angular.Locales/Angular.Locale.{{locale}}.js');

                if (commonRoutes.routes !== undefined) {
                    angular.forEach(commonRoutes.routes, function (route, path) {
                        $routeProvider.when(path, { templateUrl: route.templateUrl, resolve: dependencyResolver(route.dependencies) });
                    });
                }
                if (commonRoutes.defaultRoutePath !== undefined) {
                    $routeProvider.otherwise({ redirectTo: commonRoutes.defaultRoutePath });
                }
            }
        ]);

        application.run(["$resource", "$rootScope", "tmhDynamicLocale", function ($resource, $rootScope, tmhDynamicLocale) {
            tmhDynamicLocale.set("ZH-CN");

            var tmpBusinessEnumJs = "/ScriptLibraries/BusinessEnumDescriptions.js?" + CurrentConfig.Version;
            $resource(tmpBusinessEnumJs).get(function (data) {
                //加载枚举文档
                $rootScope.businessEnum = data;
            });
        }]);

        application.controller("iPms.Common.SecurityController", ["$scope", "$rootScope", "$cookieStore", "$route", "$location", "$resource", "$modal", function ($scope, $rootScope, $cookieStore, $route, $location, $resource, $modal) {
            $rootScope.userModel = null;
            var protocol = location.protocol;
            $rootScope.reportUrl = protocol + "//" + location.hostname + ":8090";
            $rootScope.$on("$locationChangeSuccess", function (eve, to, from) {
                checkIsLogin();

                var url = $route.current.originalPath;
                var params = $route.current.params;
                url = url ? url.split('/')[1] : "";
                if (url) {
                    $scope.currentPage = url;
                } else {
                    $scope.currentPage = "RealTimeRoomStatus";
                };
            });

            $scope.showLogoutModal = function () {
                require(["Template!Security/Templates/Logout.html", "Security/Controllers/iPms.LogoutController"], function (logoutTemplate) {
                    var tmpModalInstance = $modal.open({
                        backdropClass: "graylayer",
                        template: logoutTemplate,
                        size: "sm"
                    });

                    $rootScope.modalInstance = tmpModalInstance;
                });
            };

            $rootScope.signalRHandle.init();

            function checkIsLogin() {
                var locationHash = location.hash;
                if (locationHash != "#/Register" && locationHash != "#/Login" && locationHash != "#/ForgetPassword") {
                    var tmpSessionId = $cookieStore.get("SessionId");

                    if (tmpSessionId) {
                        $rootScope.userModel = $cookieStore.get("UserModel");

                        $rootScope.isLogin = true;
                    } else {
                        $rootScope.isLogin = false;

                        if (!$scope.userModel) {
                            $location.path("/Login");
                        }
                    }
                } else {
                    $rootScope.isLogin = false;
                }
            };

            //轮询未读消息
            $scope.poolingMessage = function () {
                $.ajax({
                    url: "/api/MessageRecord/Pooling",
                    headers: { AccessKeyId: CurrentConfig.AccessKeyId, AppId: CurrentConfig.AppId },
                    success: function (data, textStatus, jqXHR) {
                        if (textStatus == "success") {
                            if (data.data && data.data > 0) {
                                $scope.hasMessage = true;
                            } else {
                                $scope.hasMessage = false;
                            }
                        }
                    }
                });
            };

            function pollingMessage() {
                checkIsLogin();
                if ($rootScope.isLogin) {
                    if (!$rootScope.pendingOtaOrder) {
                        $.ajax({
                            url: "/ota/binding/Pooling",
                            headers: { AccessKeyId: CurrentConfig.AccessKeyId, AppId: CurrentConfig.AppId },
                            success: function (data, textStatus, jqXHR) {
                                if (textStatus == "success") {
                                    if (data.data && data.data.OrderStatus != undefined && (data.data.OrderStatus == "UnAccept" || data.data.OrderStatus == "Canceled" || (data.data.OrderStatus == "Accepted" && data.data.Channel == "TAOBAO")) && !$rootScope.pendingOtaOrder) {
                                        require(["Template!Distribution/Templates/OtaOrderTip.html", "Distribution/Controllers/iPms.OtaOrderTipController"], function (otaOrderTipTemplate) {
                                            var tmpModalInstance = $modal.open({
                                                backdrop: "static",
                                                backdropClass: "graylayer",
                                                template: otaOrderTipTemplate,
                                                controller: "iPms.Distribution.OtaOrderTipController"
                                            });
                                            $rootScope.pendingOtaOrder = data.data;
                                            $rootScope.otaOrderTipmodalInstance = tmpModalInstance;
                                        });
                                    }
                                }
                            }
                        });
                    }

                    $scope.poolingMessage();
                }
                setTimeout(function () {
                    console.log(new Date());
                    pollingMessage();
                }, 300000);
            };

            if (!$cookieStore.get("IncludeApp")) {
                pollingMessage();
            }

            //输入旧域名，跳转到新域名
            function redirectToNewDomainName() {
                var tmpOldDomainName = "ipms.beyondh.com";
                var tmpNewDomainName = "www.51pms.net";

                if ($location.$$host == tmpOldDomainName) {
                    window.location.href = $location.$$absUrl.replace(tmpOldDomainName, tmpNewDomainName) + "?showBanjia=true";
                }
                if ($location.search()['showBanjia'] == "true") {
                    require(["Template!Msg/Templates/DomainNameChange.html", "Msg/Controllers/iPms.DomainNameChangeController"], function (otaOrderTipTemplate) {
                        var tmpModalInstance = $modal.open({
                            backdrop: "static",
                            backdropClass: "graylayer",
                            template: otaOrderTipTemplate,
                            controller: "iPms.Msg.DomainNameChangeController"
                        });
                        $rootScope.domainNameChangeModalInstance = tmpModalInstance;
                    });
                }
            };

            redirectToNewDomainName();
        }]);

        return application;
    });