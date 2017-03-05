/*
 AngularJS v1.4.6
 (c) 2010-2015 Google, Inc. http://angularjs.org
 License: MIT
*/
(function (I, f, C) {
    'use strict'; function D(t, e) { e = e || {}; f.forEach(e, function (f, k) { delete e[k] }); for (var k in t) !t.hasOwnProperty(k) || "$" === k.charAt(0) && "$" === k.charAt(1) || (e[k] = t[k]); return e } var y = f.$$minErr("$resource"), B = /^(\.[a-zA-Z_$@][0-9a-zA-Z_$@]*)+$/; f.module("ngResource", ["ng"]).provider("$resource", function () {
        var t = /^https?:\/\/[^\/]*/, e = this; this.defaults = { stripTrailingSlashes: !0, actions: { get: { method: "GET" }, save: { method: "POST" }, query: { method: "GET", isArray: !0 }, remove: { method: "DELETE" }, "delete": { method: "DELETE" } } };
        this.$get = ["$http", "$q", function (k, F) {
            function w(f, g) { this.template = f; this.defaults = r({}, e.defaults, g); this.urlParams = {} } function z(l, g, s, h) {
                function c(b, q) { var c = {}; q = r({}, g, q); u(q, function (a, q) { x(a) && (a = a()); var m; if (a && a.charAt && "@" == a.charAt(0)) { m = b; var d = a.substr(1); if (null == d || "" === d || "hasOwnProperty" === d || !B.test("." + d)) throw y("badmember", d); for (var d = d.split("."), n = 0, g = d.length; n < g && f.isDefined(m) ; n++) { var e = d[n]; m = null !== m ? m[e] : C } } else m = a; c[q] = m }); return c } function G(b) { return b.resource }
                function d(b) { D(b || {}, this) } var t = new w(l, h); s = r({}, e.defaults.actions, s); d.prototype.toJSON = function () { var b = r({}, this); delete b.$promise; delete b.$resolved; return b }; u(s, function (b, q) {
                    var g = /^(POST|PUT|PATCH)$/i.test(b.method); d[q] = function (a, A, m, e) {
                        var n = {}, h, l, s; switch (arguments.length) { case 4: s = e, l = m; case 3: case 2: if (x(A)) { if (x(a)) { l = a; s = A; break } l = A; s = m } else { n = a; h = A; l = m; break } case 1: x(a) ? l = a : g ? h = a : n = a; break; case 0: break; default: throw y("badargs", arguments.length); } var w = this instanceof d, p = w ?
                            h : b.isArray ? [] : new d(h), v = {}, z = b.interceptor && b.interceptor.response || G, B = b.interceptor && b.interceptor.responseError || C; u(b, function (b, a) { "params" != a && "isArray" != a && "interceptor" != a && (v[a] = H(b)) }); g && (v.data = h); t.setUrlParams(v, r({}, c(h, b.params || {}), n), b.url); n = k(v).then(function (a) {
                                var c = a.data, m = p.$promise; if (c) {
                                    if (f.isArray(c) !== !!b.isArray) throw y("badcfg", q, b.isArray ? "array" : "object", f.isArray(c) ? "array" : "object", v.method, v.url); b.isArray ? (p.length = 0, u(c, function (a) {
                                        "object" === typeof a ? p.push(new d(a)) :
                                        p.push(a)
                                    })) : (D(c, p), p.$promise = m)
                                } p.$resolved = !0; a.resource = p; return a
                            }, function (a) { p.$resolved = !0; (s || E)(a); return F.reject(a) }); n = n.then(function (a) { var b = z(a); (l || E)(b, a.headers); return b }, B); return w ? n : (p.$promise = n, p.$resolved = !1, p)
                    }; d.prototype["$" + q] = function (a, b, c) { x(a) && (c = b, b = a, a = {}); a = d[q].call(this, a, this, b, c); return a.$promise || a }
                }); d.bind = function (b) { return z(l, r({}, g, b), s) }; return d
            } var E = f.noop, u = f.forEach, r = f.extend, H = f.copy, x = f.isFunction; w.prototype = {
                setUrlParams: function (l, g,
                e) {
                    var h = this, c = e || h.template, k, d, r = "", b = h.urlParams = {}; u(c.split(/\W/), function (d) { if ("hasOwnProperty" === d) throw y("badname"); !/^\d+$/.test(d) && d && (new RegExp("(^|[^\\\\]):" + d + "(\\W|$)")).test(c) && (b[d] = !0) }); c = c.replace(/\\:/g, ":"); c = c.replace(t, function (b) { r = b; return "" }); g = g || {}; u(h.urlParams, function (b, e) {
                        k = g.hasOwnProperty(e) ? g[e] : h.defaults[e]; f.isDefined(k) && null !== k ? (d = encodeURIComponent(k).replace(/%40/gi, "@").replace(/%3A/gi, ":").replace(/%24/g, "$").replace(/%2C/gi, ",").replace(/%20/g, "%20").replace(/%26/gi,
                        "&").replace(/%3D/gi, "=").replace(/%2B/gi, "+"), c = c.replace(new RegExp(":" + e + "(\\W|$)", "g"), function (a, b) { return d + b })) : c = c.replace(new RegExp("(/?):" + e + "(\\W|$)", "g"), function (a, b, c) { return "/" == c.charAt(0) ? c : b + c })
                    }); h.defaults.stripTrailingSlashes && (c = c.replace(/\/+$/, "") || "/"); c = c.replace(/\/\.(?=\w+($|\?))/, "."); l.url = r + c.replace(/\/\\\./, "/."); u(g, function (b, c) { h.urlParams[c] || (l.params = l.params || {}, l.params[c] = b) })
                }
            }; return z
        }]
    })
})(window, window.angular);
//# sourceMappingURL=angular-resource.min.js.map