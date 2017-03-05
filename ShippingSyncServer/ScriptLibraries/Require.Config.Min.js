"use strict";

define({
    baseUrl: "/AngularScripts",
    paths: {
        "Angular": "/ScriptLibraries/Angular.Min",
        "Angular.Dynamic.Locale": "/ScriptLibraries/Angular.Dynamic.Locale.Min",
        "Angular.Route": "/ScriptLibraries/Angular.Route.Min",
        "Angular.Resource": "/ScriptLibraries/Angular.Resource.Min",
        "Angular.Sanitize": "/ScriptLibraries/Angular.Sanitize.Min",
        "Angular.Animate": "/ScriptLibraries/Angular.Animate.Min",
        "Angular.Cookies": "/ScriptLibraries/Angular.Cookies.Min",
        "Angular.Storage": "/ScriptLibraries/Angular.Storage.Min",
        "Angular.Select": "/ScriptLibraries/Angular.Select.Min",
        "Angular.Bootstrap": "/ScriptLibraries/Angular.Bootstrap.Min",
        "Angular.BootstrapTemplate": "/ScriptLibraries/Angular.BootstrapTemplate.Min",

        "Moment": "/ScriptLibraries/Moment.Min",

        "Template": "/ScriptLibraries/Require.Text.Min",

        "iPms.Application": "/ScriptLibraries/iPms.Application",
    },
    shim: {
        "iPms.Application": {
            deps: ["Angular", "Angular.Dynamic.Locale", "Angular.Route", "Angular.Resource", "Angular.Animate", "Angular.Cookies", "Angular.Sanitize", "Angular.Storage", "Angular.BootstrapTemplate", "Angular.Bootstrap", "Angular.Select", "Moment"]
        },
        "Angular.Dynamic.Locale": {
            deps: ["Angular"]
        },
        "Angular.Animate": {
            deps: ["Angular"]
        },
        "Angular.Route": {
            deps: ["Angular"]
        },
        "Angular.Resource": {
            deps: ["Angular"]
        },
        "Angular.Cookies": {
            deps: ["Angular"]
        },
        "Angular.Sanitize": {
            deps: ["Angular"]
        },
        "Angular.Storage": {
            deps: ["Angular"]
        },
        "Angular.Select": {
            deps: ["Angular"]
        },
        "Angular.Bootstrap": {
            deps: ["Angular"]
        },
        "Angular.BootstrapTemplate": {
            deps: ["Angular"]
        }
    }
});