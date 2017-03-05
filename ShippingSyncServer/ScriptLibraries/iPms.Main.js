(function (root) {
    "use strict";

    var tmpRequireJS = "/ScriptLibraries/Require.Config.Min.js?" + CurrentConfig.Version;

    require([tmpRequireJS], function (requireConfig) {
        requireConfig.urlArgs = CurrentConfig.Version;
        require.config(requireConfig);
        require(["iPms.Application"], function (application) {
            angular.bootstrap(document, ["iPms.Application"]);
        });
    });
})(this);