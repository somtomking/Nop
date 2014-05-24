(function ($) {
    var EasySlider = function (element, options) {
        var speed = 5000;
        var current = 0;

        var items = $(element).find("." + options.itemClass);
        items.hide();
        items.eq(0).show();

        var totalpic = items.size() - 1;

        var pagination = $(element).find('.' + options.paginationClass).find("a");
        pagination.removeClass("on");
        pagination.eq(0).addClass("on");
        pagination.on('click', function () {
            var changenow = $(this).index();
            //console.log(changenow);

            items.eq(current).hide();
            items.eq(changenow).show();
            pagination.eq(changenow).addClass('on').siblings('a').removeClass('on');
            items.eq(current).fadeOut(400, function () {
                items.eq(changenow).fadeIn(500);
            });
            current = changenow;
        });

        var timer = setTimeout(function () {
            play();
        }, speed);

        var play = function () {
            var NN = current + 1;

            if (current < totalpic) {
                items.eq(current).hide();
                items.eq(NN).show();
                pagination.eq(NN).addClass('on').siblings('a').removeClass('on');
                items.eq(current).fadeOut(400, function () {
                    items.eq(NN).fadeIn(500);
                });
                current += 1;
            } else {
                NN = 0;
                items.eq(current).hide();
                items.eq(NN).stop(true, true).show();
                items.eq(current).fadeOut(400, function () {
                    items.eq(0).fadeIn(500);
                });
                pagination.eq(NN).addClass('on').siblings('a').removeClass('on');
                current = 0;
            }

            timer = setTimeout(play, speed);
        };
    };

    var defaults = {
        itemClass: "item",
        paginationClass: 'slideNum',
    };

    $.fn.easySlider = function (options) {
        var options = $.extend(defaults, options);
        //console.log(options.paginationClass);
        //console.log(options.itemClass);
        return this.each(function (key, value) {
            var element = $(this);
            // Return early if this element already has a plugin instance
            if (element.data('easyslider')) { return element.data('easyslider'); }
            // Pass options to plugin constructor
            var easyslider = new EasySlider(this, options);
            // Store plugin object in this element's data
            element.data('easyslider', easyslider);
        });
    };
})(jQuery);
$(function() {
    //var jslide1 = new JSlide();
    //var jslide2 = new JSlide();
    //jslide1.init("mrysBox", "adBox_02", "slideNum");
    //jslide2.init("flashSale", "con", "slideNum");
    if ($(".mrysBox .adBox_02").size() > 1)
        $(".mrysBox").easySlider({
            itemClass: "adBox_02",
            paginationClass: "slideNum"
        });
    if ($(".flashSale .con").size() > 1)
        $(".flashSale").easySlider({
            itemClass: "con",
            paginationClass: "slideNum"
        });
});



