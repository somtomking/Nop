$(document).ready(function () {

    var elm = $('.fixedTopShow');
    var startPos = $(elm).offset().top;
    $.event.add(window, "scroll", function () {
        var p = $(window).scrollTop();
        if ((p) > startPos) {
            $('.fixedTop').css('display', 'block');
            $('.fixedTop').css('position', 'fixed');
            $('.fixedTop').css('width', '100%');
            $('.fixedTop').css('top', '0px');
        }
        else {
            $('.fixedTop').css('display', 'none');
            $('.fixedTop').css('position', '');
            $('.fixedTop').css('width', 'auto');
        }
    });

    $('.fixedTop .container').on('mouseenter', '#topFixedCategory', function () {
        var links = $("#topFixedCategory .mainClass a");
        if (!$(links).hasClass("on"))
            $(links[0]).addClass("on");
        $("#fixed-flyout-categories").show();
    });

    $('.fixedTop .container').on('mouseleave', '#topFixedCategory', function () {
        $("#fixed-flyout-categories").hide();
    });

    $('.fixedTop .container').on('mouseenter', '#fixed-flyout-categories', function () {
    });

    $('.fixedTop .container').on('mouseleave', '#fixed-flyout-categories', function () {
        var links = $("#topFixedCategory .mainClass a");
        if (!$(links).hasClass("on"))
            $(links[0]).addClass("on");
    });

    $('.fixedTop .dropdown-menu .mainClass li').each(function () {
        $(this).on('mouseenter', function () {
            $('.fixedTop .dropdown-menu .mainClass li a').removeClass("on");
            $(this).addClass("on");
            var index = ($(this).attr("index"));
            $(".fixedTop .dropdown-menu .subClass").each(function () {
                if ($(this).attr("index") == index)
                    $(this).show();
                else
                    $(this).hide();
            });
        });
    });

    $('.fixedTop .dropdown-menu .subClass').each(function () {
        $(this).on("mouseenter", function () {
            var index = ($(this).attr("index"));
            $('.fixedTop .dropdown-menu .mainClass li').each(function () {
                if (index == $(this).attr("index"))
                    $(this).find("a").addClass("on");
            });
        });
    });

    $("#fixed-search-button").click(function () {

    });
});
