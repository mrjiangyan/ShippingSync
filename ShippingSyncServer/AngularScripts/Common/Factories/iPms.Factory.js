"use strict";

define(function () {
    var iPmsFactories = angular.module('iPms.Factories', []);

    iPmsFactories.factory('ngValidateFactory', function () {
        var ngValidate = {};

        //options
        ngValidate.options = {
            validateOnEvent: true,
            validateOnBlur: false,
            errorMessageCSSClass: 'hasError'
        };

        //generic function example
        ngValidate.allowedChars = function () {
            return true;
        };

        ngValidate.rangeLength = function (testVal, expectedVal) {
            var tmpArr = expectedVal.split("|");
            if (tmpArr.length == 2) {
                return testVal.length >= tmpArr[0] && testVal.length <= tmpArr[1];
            } else
                return true;
        };

        ngValidate.textLength = function (testVal, expectedVal) {
            return testVal.length == expectedVal;
        };

        ngValidate.minLength = function (testVal, expectedVal) {
            return testVal.length >= expectedVal;
        };

        ngValidate.maxLength = function (testVal, expectedVal) {
            return testVal.length <= expectedVal;
        };

        ngValidate.required = function (testVal) {
            return !!testVal;
        };

        //下拉框必选
        ngValidate.selectRequired = function (testVal) {
            return testVal > 0;
        };

        ngValidate.pattern = function (testVal, patternval) {
            return patternval.test(testVal);
        };

        ngValidate.regexPattern = function (testVal, patternval) {
            return ngValidate.pattern(testVal, eval(patternval));
        };

        ngValidate.emailPattern = function (testVal) {
            return ngValidate.pattern(testVal, /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
        };

        ngValidate.mobilePattern = function (testVal) {
            return ngValidate.pattern(testVal, /^1[3-8]\d{9}$/);
        };

        ngValidate.identPattern = function (testVal) {
            return ngValidate.pattern(testVal, /[\d]{6}(19|20)*[\d]{2}((0[1-9])|(11|12))([012][\d]|(30|31))[\d]{3}[xX\d]*/);
        };

        ngValidate.identityPattern = function (testVal) {
            return ngValidate.pattern(testVal, /[\d]{6}(19|20)*[\d]{2}((0[1-9])|(11|12))([012][\d]|(30|31))[\d]{3}[xX\d]*/);
        };

        ngValidate.numericalityPattern = function (testVal) {
            return ngValidate.pattern(testVal, /^(-|\+)?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d*)?$/);
        };

        ngValidate.integerPattern = function (testVal) {
            return ngValidate.pattern(testVal, /^[+\-]?\d+$/);
        };

        ngValidate.equalTo = function (testVal, expectedVal) {
            return testVal === expectedVal;
        };

        ngValidate.strategies = {};

        //Default Strategies
        ngValidate.strategies.required = [{
            value: ngValidate.required,
            message: '不能为空'
        }];

        ngValidate.strategies.selectRequired = [{
            value: ngValidate.selectRequired,
            message: ''
        }];

        ngValidate.strategies.rangeLength = [{
            value: [ngValidate.rangeLength, true],
            message: ''
        }];

        ngValidate.strategies.textLength = [{
            value: [ngValidate.textLength, true],
            message: '只能输入{textLength}个字符'
        }];

        ngValidate.strategies.minLength = [{
            value: [ngValidate.minLength, true],
            message: '至少输入{minLength}个字符'
        }];

        ngValidate.strategies.maxLength = [{
            value: [ngValidate.maxLength, true],
            message: '最多输入{maxLength}个字符'
        }];

        ngValidate.strategies.regexPattern = [{
            value: [ngValidate.regexPattern, true],
            message: ''
        }];

        ngValidate.strategies.email = [{
            value: ngValidate.emailPattern,
            message: 'Emall地址不正确'
        }];

        ngValidate.strategies.mobile = [{
            value: ngValidate.mobilePattern,
            message: '手机号码格式不正确'
        }];

        ngValidate.strategies.ident = [{
            value: ngValidate.identPattern,
            message: '身份证号码格式不正确'
        }];

        ngValidate.strategies.identity = [{
            value: ngValidate.identityPattern,
            message: '身份证号码格式不正确'
        }];

        ngValidate.strategies.numericality = [{
            value: ngValidate.numericalityPattern,
            message: '请输入数字'
        }];

        ngValidate.strategies.integer = [{
            value: ngValidate.integerPattern,
            message: '请输入整数'
        }];

        ngValidate.strategies.equalTo = [{
            value: [ngValidate.equalTo, true],
            message: '两次输入不一致'
        }];

        return ngValidate;
    });
});