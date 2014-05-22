
(function (a) {
    a.fn.imgScroll = function (b) {
        return this.each(function () {
            function o() {
                x || p.css("float", "left"), q.css({
                    "position": "absolute",
                    "left": 0,
                    "top": 0
                }), l.css({
                    "position": m,
                    //"width": e.direction == "x" ? c * h : p.outerWidth(),
                    "height": e.direction == "x" ? d : c * d,
                    "overflow": "hidden"
                }), r.addClass(e.disableClass + w);
                if (!e.loop && (u - c) % j !== 0 && u > c) {
                    var A = j - (u - c) % j;
                    q.append(p.slice(0, A).clone()), u = q.find("li").length, i = parseInt((u - c) / j);
                }
                k ? q.css("width", u * h) : q.css("height", u * d), !e.showControl && u <= c ? (s.hide(), r.hide()) : (s.show(), r.show()), u <= c ? (s.addClass(e.disableClass), r.addClass(e.disableClass)) : (r.addClass(e.disableClass), s.removeClass(e.disableClass));
                if (e.showNavItem) {
                    for (var y = 0; y <= i; y++) {
                        var z = y == 0 ? e.navItemCurrent : "";
                        t.append('<em class="' + z + '">' + (y + 1) + "</em>");
                    }
                    l.after(t), t.find("em").bind(e.navItemEvtType, function () {
                        var B = parseInt(this.innerHTML);
                        g((B - 1) * j);
                    });
                }
                e.showStatus && l.after('<span class="scroll-status">1/' + (i + 1) + "</span>");
            }
            function g(y) {
                if (q.is(":animated")) return !1;
                if (y < 0) return r.addClass(e.disableClass + w), !1;
                if (y + c > u) return s.addClass(e.disableClass), !1;
                v = y, q.animate(e.direction == "x" ? {
                    "left": -(y * h)
                } : {
                    "top": -(v * d)
                }, e.speed, function () {
                    y > 0 ? r.removeClass(e.disableClass + w) : r.addClass(e.disableClass + w), y + c < u ? s.removeClass(e.disableClass) : s.addClass(e.disableClass), n(y);
                });
            }
            function n(y) {
                t.find("em").removeClass(e.navItemCurrent).eq(y / j).addClass(e.navItemCurrent), e.showStatus && a(".scroll-status").html(y / j + 1 + "/" + (i + 1));
            }
            var e = a.extend({
                "evtType": "click",
                "visible": 1,
                "showControl": !0,
                "showNavItem": !1,
                "navItemEvtType": "click",
                "navItemCurrent": "current",
                "showStatus": !1,
                "direction": "x",
                "next": ".next",
                "prev": ".prev",
                "disableClass": "disabled",
                "speed": 300,
                "loop": !1,
                "step": 1,
                "ie6DisableClass": "disableIE6"
            }, b), l = a(this), q = l.find("ul"), p = q.find("li"), h = e.width || p.outerWidth(), d = e.height || p.outerHeight(), u = p.length, c = e.visible, j = e.step, i = parseInt((u - c) / j), v = 0, m = l.css("position") == "absolute" ? "absolute" : "relative", x = p.css("float") !== "none", t = a('<div class="scroll-nav-wrap"></div>'), f = e.direction == "x" ? "left" : "top", k = e.direction == "x", r = typeof e.prev == "string" ? a(e.prev) : e.prev, s = typeof e.next == "string" ? a(e.next) : e.next, w = a.browser.isIE6 ? e.ie6DisableClass : "";
            e.loop = !1, o(), u > c && (s.click(function () {
                g(v + j);
            }), r.click(function () {
                g(v - j);
            }));
        });
    };
})(jQuery);

(function (e) {
    e.fn.jqueryzoom = function (t) {
        var i = {
            "xzoom": 200,
            "yzoom": 200,
            "offset": 10,
            "position": "right",
            "lens": 1,
            "preload": 1
        };
        t && e.extend(i, t);
        var s = "";
        e(this).hover(function () {
            function t(e) {
                this.x = e.pageX, this.y = e.pageY;
            }
            var a = e(this).offset().left, n = e(this).offset().top, o = e(this).find("img").get(0).offsetWidth, r = e(this).find("img").get(0).offsetHeight;
            s = e(this).find("img").attr("alt");
            var c = e(this).find("img").attr("jqimg");
            e(this).find("img").attr("alt", ""), 0 == e("div.zoomdiv").get().length && (e(this).after("<div class='zoomdiv'><img class='bigimg' src='" + c + "'/></div>"), e(this).append("<div class='jqZoomPup'>&nbsp;</div>")), e("div.zoomdiv").width(i.xzoom), e("div.zoomdiv").height(i.yzoom), e("div.zoomdiv").show(), i.lens || e(this).css("cursor", "crosshair"), e(document.body).mousemove(function (s) {
                mouse = new t(s);
                var c = e(".bigimg").get(0).offsetWidth, d = e(".bigimg").get(0).offsetHeight, l = "x", p = "y";
                if (isNaN(p) | isNaN(l)) {
                    var p = c / o, l = d / r;
                    
                    e("div.jqZoomPup").width(i.xzoom / (1 * p)), e("div.jqZoomPup").height(i.yzoom / (1 * l)), i.lens && e("div.jqZoomPup").css("visibility", "visible");
                }
                xpos = mouse.x - e("div.jqZoomPup").width() / 2 - a, ypos = mouse.y - e("div.jqZoomPup").height() / 2 - n, i.lens && (xpos = a > mouse.x - e("div.jqZoomPup").width() / 2 ? 0 : mouse.x + e("div.jqZoomPup").width() / 2 > o + a ? o - e("div.jqZoomPup").width() - 2 : xpos, ypos = n > mouse.y - e("div.jqZoomPup").height() / 2 ? 0 : mouse.y + e("div.jqZoomPup").height() / 2 > r + n ? r - e("div.jqZoomPup").height() - 2 : ypos), i.lens && e("div.jqZoomPup").css({
                    "top": ypos,
                    "left": xpos
                }), scrolly = ypos, e("div.zoomdiv").get(0).scrollTop = scrolly * l, scrollx = xpos, e("div.zoomdiv").get(0).scrollLeft = scrollx * p;
            });
        }, function () {
            e(this).children("img").attr("alt", s), e(document.body).unbind("mousemove"), i.lens && e("div.jqZoomPup").remove(), e("div.zoomdiv").remove();
        }), count = 0, i.preload && (e("body").append("<div style='display:none;' class='jqPreload" + count + "'>360buy</div>"), e(this).each(function () {
            var t = e(this).children("img").attr("jqimg"), i = jQuery("div.jqPreload" + count).html();
            jQuery("div.jqPreload" + count).html(i + '<img src="' + t + '">');
        }));
    };
})(jQuery);