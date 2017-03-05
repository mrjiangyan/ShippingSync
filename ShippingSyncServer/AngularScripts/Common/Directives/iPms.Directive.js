"use strict";

define(["Common/Factories/iPms.Factory"], function () {
    var iPmsDirectives = angular.module('iPms.Directives', ["iPms.Factories"]);

    iPmsDirectives.directive("ngValidate", function ($compile, ngValidateFactory) {
        var validationFunction = function (scope, element, attrs, ctrl, fromBroadcast) {
            //Validate invisible elements as Valid
            //check controller visibility-Jquery style (visible if width>0 & height>0, visibility hidden considered visible)
            if (!(element.prop('offsetWidth') > 0 && element.prop('offsetHeight') > 0)) {
                ctrl.$setValidity("notVisibleValidation", true);
                return;
            }

            //Validate disabled elements as Valid
            if (!!attrs.disabled) {
                ctrl.$setValidity("inputDisabledValidation", true);
                return;
            }

            var strategy = attrs.ngValidate;
            if (attrs.ngIdent && attrs.ngIdent == "C01") {
                strategy = "ident";
            }
            if (attrs.ngIdent && attrs.ngIdent != "C01") {
                ctrl.$setValidity("ident-0", true);
            }
            var validationCase = ngValidateFactory.strategies[strategy];

            //TODO case:arg syntax for minlength:length, maxlength:length, pattern:regex

            if (!validationCase || !angular.isArray(validationCase) || validationCase.length == 0) {
                ctrl.$setValidity("emptyValidation", true);
                return;
            }

            var isValid, errorMessage, callFunction;
            for (var i = 0, l = validationCase.length; i < l; ++i) {
                var isComplex = false,
                    target,
                    targetValue;

                if (angular.isFunction(validationCase[i].value)) {
                    isValid = validationCase[i].value(element.val());
                } else if (angular.isArray(validationCase[i].value)) {
                    var argumentsArray = validationCase[i].value.slice(1);

                    isComplex = argumentsArray[0];
                    if (isComplex) {
                        target = attrs["target"];
                        if (target == "html") {
                            targetValue = attrs["targetvalue"];
                        } else if (target == "scope") {
                            var tmpTargetValue = attrs["targetvalue"];
                            targetValue = eval("scope.__proto__." + tmpTargetValue);
                        }
                        argumentsArray[0] = targetValue;
                    }
                    argumentsArray.unshift(element.val());

                    isValid = validationCase[i].value[0].apply(null, argumentsArray);
                }

                //Validate optional elements with zero length as Valid
                if (typeof attrs.optional != 'undefined' && attrs.optional != 'false' && element.val().length == 0) {
                    isValid = true;
                }

                ctrl.$setValidity(strategy + "-" + i, isValid);
                if (!isValid) {
                    var tmpErrorMessage = attrs["message"];
                    if (tmpErrorMessage) {
                        errorMessage = tmpErrorMessage;
                    } else {
                        errorMessage = validationCase[i].message.replace("{" + strategy + "}", targetValue) || "";
                    }
                    break;
                }
            }
            scope.errorStatus = !isValid;
            scope.errorMessage = errorMessage;
            if (!fromBroadcast) {
                scope.$apply();
            }
        };

        return {
            require: 'ngModel',
            scope: true,
            link: function (scope, element, attrs, ctrl) {
                element.after($compile(angular.element('<span ng-show="errorStatus" ng-bind="errorMessage" class="' + ngValidateFactory.options.errorMessageCSSClass + '"></span>'))(scope));

                if (ngValidateFactory.options.validateOnEvent == true) {
                    scope.$on('ng-validate', function () { validationFunction(scope, element, attrs, ctrl, true) });
                }

                if (ngValidateFactory.options.validateOnBlur == true) {
                    element.on('blur', function () {
                        validationFunction(scope, element, attrs, ctrl, false);
                    });
                }
            }
        };
    });

    iPmsDirectives.directive('ngDaterangepicker', ['$document', function ($document) {
        return {
            restrict: "EA",
            require: '?ngModel',
            scope: true,
            link: function (scope, element, attrs, ngModel) {
                scope.format = attrs.format || 'YYYY-MM-DD';
                scope.locale = attrs.locale || 'zh-cn';

                scope.opened = false;
                scope.dayNames = [];
                scope.viewValue = null;
                scope.dateFromValue = null;
                scope.dateToValue = null;

                scope.daterange = '';

                scope.selectedDateFrom = null;
                scope.selectedDateTo = null;

                moment.locale(scope.locale);

                var dateFrom = moment();
                var dateTo = moment();

                var generateCalendar = function (date, to) {
                    var lastDayOfMonthFrom = dateFrom.endOf('month').date(),
                        monthFrom = dateFrom.month(),
                        yearFrom = dateFrom.year(),
                        nFrom = 1,
                        i;

                    var lastDayOfMonthTo = dateTo.endOf('month').date(),
                        monthTo = dateTo.month(),
                        yearTo = dateTo.year(),
                        nTo = 1;

                    var firstWeekDayFrom = scope.firstWeekDaySunday === true ? dateFrom.set('date', 2).day() : dateFrom.set('date', 1).day();
                    if (firstWeekDayFrom !== 1) {
                        nFrom -= firstWeekDayFrom - 1;
                    }

                    var firstWeekDayTo = scope.firstWeekDaySunday === true ? dateTo.set('date', 2).day() : dateTo.set('date', 1).day();
                    if (firstWeekDayTo !== 1) {
                        nTo -= firstWeekDayTo - 1;
                    }

                    if (to !== undefined) {
                        scope.daysTo = [];
                        scope.dateToValue = dateTo.format('MMMM YYYY');
                        for (i = nTo; i <= lastDayOfMonthTo; i += 1) {
                            if (i > 0) {
                                scope.daysTo.push({ day: i, month: monthTo + 1, year: yearTo, enabled: true });
                            } else {
                                scope.daysTo.push({ day: null, month: null, year: null, enabled: false });
                            }
                        }
                    } else {
                        scope.daysFrom = [];
                        scope.dateFromValue = dateFrom.format('MMMM YYYY');
                        for (i = nFrom; i <= lastDayOfMonthFrom; i += 1) {
                            if (i > 0) {
                                scope.daysFrom.push({ day: i, month: monthFrom + 1, year: yearFrom, enabled: true });
                            } else {
                                scope.daysFrom.push({ day: null, month: null, year: null, enabled: false });
                            }
                        }
                    }
                };

                var generateDayNames = function () {
                    var date = scope.firstWeekDaySunday === true ? moment('2015-06-07') : moment('2015-06-01');
                    for (var i = 0; i < 7; i += 1) {
                        scope.dayNames.push(date.format('ddd'));
                        date.add('1', 'd');
                    }
                };

                generateDayNames();
                generateCalendar(dateFrom);
                generateCalendar(dateTo, true);

                scope.open = function () {
                    scope.opened = true;
                };

                scope.close = function () {
                    scope.opened = false;
                };

                scope.prevYear = function (to) {
                    if (to !== undefined) {
                        dateTo.subtract(1, 'Y');
                        generateCalendar(dateTo, true);
                    } else {
                        dateFrom.subtract(1, 'Y');
                        generateCalendar(dateFrom);
                    }
                };

                scope.prevMonth = function (to) {
                    if (to !== undefined) {
                        dateTo.subtract(1, 'M');
                        generateCalendar(dateTo, true);
                    } else {
                        dateFrom.subtract(1, 'M');
                        generateCalendar(dateFrom);
                    }
                };

                scope.nextMonth = function (to) {
                    if (to !== undefined) {
                        dateTo.add(1, 'M');
                        generateCalendar(dateTo, true);
                    } else {
                        dateFrom.add(1, 'M');
                        generateCalendar(dateFrom);
                    }
                };

                scope.nextYear = function (to) {
                    if (to !== undefined) {
                        dateTo.add(1, 'Y');
                        generateCalendar(dateTo, true);
                    } else {
                        dateFrom.add(1, 'Y');
                        generateCalendar(dateFrom);
                    }
                };

                scope.selectDate = function (e, d, to) {
                    e.preventDefault();

                    if (to) {
                        scope.selectedDateTo = moment(d.year + ' ' + d.month + ' ' + d.day, scope.format);
                    } else {
                        scope.selectedDateFrom = moment(d.year + ' ' + d.month + ' ' + d.day, scope.format);
                    }

                    scope.daterange = '';

                    if (scope.selectedDateFrom !== null && scope.selectedDateTo !== null) {
                        scope.daterange = scope.selectedDateFrom.format(scope.format) + ' - ' + scope.selectedDateTo.format(scope.format);
                        ngModel.$setViewValue(scope.daterange);
                    }

                    if (scope.selectedDateFrom !== null && scope.selectedDateTo !== null) {
                        scope.close();
                    }

                };

                scope.markDate = function (d, to) {
                    var currentHoverDate,
                        currentNDate;

                    if (to) {
                        if (scope.selectedDateFrom === null) {
                            return;
                        }

                        scope.daysTo.forEach(function (i, k) {
                            i.active = false;
                        });

                        currentHoverDate = moment(d.year + ' ' + d.month + ' ' + d.day, scope.format);
                        currentNDate = null;

                        scope.daysTo.forEach(function (i, k) {
                            currentNDate = moment(i.year + ' ' + i.month + ' ' + i.day, scope.format);
                            if (currentNDate <= currentHoverDate && currentNDate >= scope.selectedDateFrom) {
                                i.active = true;
                            }
                        });

                        // mark from dates
                        scope.daysFrom.forEach(function (i, k) {
                            currentNDate = moment(i.year + ' ' + i.month + ' ' + i.day, scope.format);
                            i.active = false;

                            if (currentNDate <= currentHoverDate && currentNDate >= scope.selectedDateFrom) {
                                i.active = true;
                            }
                        });


                    } else {
                        if (scope.selectedDateTo === null) {
                            return;
                        }

                        scope.daysFrom.forEach(function (i, k) {
                            i.active = false;
                        });

                        currentHoverDate = moment(d.year + ' ' + d.month + ' ' + d.day, scope.format);
                        currentNDate = null;

                        scope.daysFrom.forEach(function (i, k) {
                            currentNDate = moment(i.year + ' ' + i.month + ' ' + i.day, scope.format);
                            if (currentNDate >= currentHoverDate && currentNDate <= scope.selectedDateTo) {
                                i.active = true;
                            }
                        });

                        // mark to dates
                        scope.daysTo.forEach(function (i, k) {
                            currentNDate = moment(i.year + ' ' + i.month + ' ' + i.day, scope.format);
                            i.active = false;

                            if (currentNDate >= currentHoverDate && currentNDate <= scope.selectedDateTo) {
                                i.active = true;
                            }
                        });
                    }
                };

                // if clicked outside of calendar
                var classList = ['ng-daterangepicker', 'ng-daterangepicker-container'];
                if (attrs.id !== undefined) classList.push(attrs.id);
                $document.on('click', function (e) {
                    if (!scope.opened) return;

                    var i = 0,
                        element;

                    if (!e.target) return;

                    for (element = e.target; element; element = element.parentNode) {
                        var id = element.id;
                        var classNames = element.className;

                        if (id !== undefined) {
                            for (i = 0; i < classList.length; i += 1) {
                                if (id.indexOf(classList[i]) > -1 || classNames.indexOf(classList[i]) > -1) {
                                    return;
                                }
                            }
                        }
                    }

                    scope.close();
                    scope.$apply();
                });

                ngModel.$render = function () {
                    var newValue = ngModel.$viewValue;
                    if (newValue !== undefined) {
                        scope.daterange = newValue;
                    }
                };

            },
            template:
            '<div class="ng-daterangepicker-container">' +
            '  <input class="date" type="text" ng-model="daterange" ng-focus="open()">' +
            '  <div class="ng-daterangepicker" ng-show="opened">' +
            '    <div class="ng-calendar-container">' +
            '      <div class="controls">' +
            '        <div class="left">' +
            '          <i class="fa fa-backward prev-year-btn" ng-click="prevYear()"></i>' +
            '          <i class="fa fa-angle-left prev-month-btn" ng-click="prevMonth()"></i>' +
            '        </div>' +
            '        <span class="date">{{ dateFromValue }}</span>' +
            '        <div class="right">' +
            '          <i class="fa fa-angle-right next-month-btn" ng-click="nextMonth()"></i>' +
            '          <i class="fa fa-forward next-year-btn" ng-click="nextYear()"></i>' +
            '        </div>' +
            '      </div>' +
            '      <div class="day-names">' +
            '        <span ng-repeat="dn in dayNames track by $index">' +
            '          <span>{{ dn }}</span>' +
            '        </span>' +
            '      </div>' +
            '      <div class="cal">' +
            '        <span ng-repeat="d in daysFrom track by $index">' +
            '          <span class="day" ng-click="selectDate($event, d, false)" ng-class="{disabled: !d.enabled, active: d.active && d.enabled}" ng-mouseover="markDate(d, false)">{{ d.day }}</span>' +
            '        </span>' +
            '      </div>' +
            '    </div>' +
            '    <div class="ng-calendar-container">' +
            '      <div class="controls">' +
            '        <div class="left">' +
            '          <i class="fa fa-backward prev-year-btn" ng-click="prevYear(true)"></i>' +
            '          <i class="fa fa-angle-left prev-month-btn" ng-click="prevMonth(true)"></i>' +
            '        </div>' +
            '        <span class="date">{{ dateToValue }}</span>' +
            '        <div class="right">' +
            '          <i class="fa fa-angle-right next-month-btn" ng-click="nextMonth(true)"></i>' +
            '          <i class="fa fa-forward next-year-btn" ng-click="nextYear(true)"></i>' +
            '        </div>' +
            '      </div>' +
            '      <div class="day-names">' +
            '        <span ng-repeat="dn in dayNames track by $index">' +
            '          <span>{{ dn }}</span>' +
            '        </span>' +
            '      </div>' +
            '      <div class="cal">' +
            '        <span ng-repeat="d in daysTo track by $index">' +
            '          <span class="day" ng-click="selectDate($event, d, true)" ng-class="{disabled: !d.enabled, active: d.active && d.enabled}" ng-mouseover="markDate(d, true)">{{ d.day }}</span>' +
            '        </span>' +
            '      </div>' +
            '    </div>' +
            '  </div>' +
            '</div>'
        };
    }]);

    iPmsDirectives.directive("ngEnum", function () {
        return {
            restrict: "E",
            replace: false,
            template: '<span>{{key}}</span>',
            scope: {
                value: "=",
                type: "@"
            },
            controller: function (scope) {
                scope.refresh = function () {
                    var type = $scope.type,
                        dict = app.enum[type],
                        item = dict.first(function (e) {
                            return e.Value == scope.value;
                        });
                    scope.key = item.Description;
                };
                scope.$watch('value', function () {
                    scope.refresh();
                });
            }
        };
    });

    iPmsDirectives.directive("ngDict", function () {
        return {
            //        restrict: "E",
            //        replace: true,
            //        template: '<span>{{key}}</span>',
            //        scope: {
            //            value: "@", 
            //            dict: "@",
            //        },
            //        controller: function ($scope) {
            //            $scope.refresh = function () {
            //                var type = $scope.type;
            //                var item = null;
            //                var enums = $scope.$root.businessEnum[$scope.dict];
            //                angular.forEach(enums, function (d, i) {
            //                    if (d.EnumString == $scope.value) {
            //                        item = d;
            //                    }
            //                });
            //                if (item != null) {
            //                    $scope.key = item.Description;
            //                } else {
            //                    $scope.key = "";
            //                }
            //            };
            //            $scope.$watch('value', function () {
            //                $scope.refresh();
            //            });
            //        }
        };
    });

    iPmsDirectives.directive("ngModalDialog", function () {
        return {
            //        restrict: "E",
            //        scope: {
            //            show: "="
            //        },
            //        replace: true,
            //        transclude: true,
            //        link: function (scope, element, attrs) {
            //            scope.dialogStyle = {};
            //            if (attrs.width)
            //                scope.dialogStyle.width = attrs.width;
            //            if (attrs.height)
            //                scope.dialogStyle.height = attrs.height;
            //            scope.hideModal = function () {
            //                scope.show = false;
            //            };
            //        },
            //        template: "<div class='ng-modal' ng-show='show'><div class='ng-modal-overlay' ng-click='hideModal()'></div><div class='ng-modal-dialog' ng-style='dialogStyle'><div class='ng-modal-close' ng-click='hideModal()'>X</div><div class='ng-modal-dialog-content' ng-transclude></div></div></div>"
        };
    });

    iPmsDirectives.directive("ngScroll", function ($window, $document) {
        return {
            restrict: "A",
            link: function (scope, element, attrs) {
                var raw = element[0];
                angular.element($window, $document).bind('scroll', function () {
                    var scrollTop = $window.pageYOffset || (($document[0].documentElement || $document[0].body.parentNode || $document[0].body).scrollTop);
                    var scrollHeight = ($document[0].body.scrollHeight || $document[0].documentElement.scrollHeight) - $window.innerHeight;
                    if (scrollTop >= scrollHeight) {
                        scope.$apply(attrs.ngScroll);
                    }
                });

                element.bind('scroll', function () {
                    if (raw.scrollTop + raw.offsetHeight >= raw.scrollHeight) {
                        scope.$apply(attrs.ngScroll);
                    }
                });
            }
        };
    });

    iPmsDirectives.directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });

    iPmsDirectives.directive('loadingStatus', function () {
        return {
            link: function (scope, element, attrs) {
                var show = function () {
                    element.addClass("show");
                };
                var hide = function () {
                    element.removeClass("show");
                };
                scope.$on('loadingStatusActive', show);
                scope.$on('loadingStatusInactive', hide);
                hide();
            }
        };
    });

    iPmsDirectives.directive("ipmsPagination", function () {
        return {
            restrict: 'E',
            replace: true,
            link: function (scope, element, attrs) {
                scope.changeCurrentPageNum = function (pageNum) {
                    if (pageNum.isNum) {
                        scope.currentPageNum = pageNum.num;
                    }
                };

                scope.selectPrexPage = function () {
                    if (scope.currentPageNum > 1) {
                        scope.currentPageNum--;
                    }
                };

                scope.selectNextPage = function () {
                    if (scope.currentPageNum < scope.totalPageNum) {
                        scope.currentPageNum++;
                    }
                };

                function getPageNums(currentPageNum, totalPageNum) {
                    if (!totalPageNum) return;

                    var tmpPageNums = [];

                    if (currentPageNum <= 5) {
                        for (var i = 1; i <= currentPageNum; i++) {
                            tmpPageNums.push({ num: i, isNum: true });
                        }
                    } else {
                        tmpPageNums.push({ num: 1, isNum: true });
                        tmpPageNums.push({ num: -1, isNum: false });
                        tmpPageNums.push({ num: currentPageNum - 3, isNum: true });
                        tmpPageNums.push({ num: currentPageNum - 2, isNum: true });
                        tmpPageNums.push({ num: currentPageNum - 1, isNum: true });
                        tmpPageNums.push({ num: currentPageNum, isNum: true });
                    }

                    if (totalPageNum - currentPageNum <= 3) {
                        for (var i = currentPageNum + 1; i <= totalPageNum; i++) {
                            tmpPageNums.push({ num: i, isNum: true });
                        }
                    } else {
                        tmpPageNums.push({ num: currentPageNum + 1, isNum: true });
                        tmpPageNums.push({ num: currentPageNum + 2, isNum: true });
                        tmpPageNums.push({ num: -1, isNum: false });
                        tmpPageNums.push({ num: totalPageNum, isNum: true });
                    }

                    return tmpPageNums;
                };

                scope.$watch("currentPageNum", function () { scope.pageNums = getPageNums(scope.currentPageNum, scope.totalPageNum); });
                scope.$watch("totalPageNum", function () { scope.pageNums = getPageNums(scope.currentPageNum, scope.totalPageNum); });
            },
            template:
                '<div class="page text-center">' +
                    '<nav>' +
                        '<ul class="pagination">' +
                            '<li ng-click="selectPrexPage()" ng-class="{1:\'disabled\'}[currentPageNum]"><a href="javascript:;">«</a></li>' +
                            '<li ng-repeat="pageNum in pageNums" ng-click="changeCurrentPageNum(pageNum)" ng-class="{\'active\': currentPageNum==pageNum.num, \'disabled ellipsis\': pageNum.isNum == false}"><a href="javascript:;">{{ pageNum.isNum ? pageNum.num : "..." }}</a></li>' +
                            '<li ng-click="selectNextPage()" ng-class="{true:\'disabled\'}[currentPageNum==totalPageNum]"><a href="javascript:;">»</a></li>' +
                        '</ul>' +
                    '</nav>' +
                '</div>',
            scope: {
                totalPageNum: "=",
                currentPageNum: "="
            }
        };
    });
});