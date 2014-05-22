/*
** nopCommerce ajax cart implementation
*/


var AjaxCart = {
    loadWaiting: false,
    usepopupnotifications: false,
    topcartselector1: '',
    topcartselector2: '',
    fixedcartselector: '',
    flyoutcartselector: '',
    fixedflyoutcartselector: '',
    topwishlistselector: '',
    fixedwishlistselector: '',
    fixedflyoutwishlistselector: '',

    init: function (usepopupnotifications,
            topcartselector1, topcartselector2, fixedcartselector, flyoutcartselector, fixedflyoutcartselector,
            topwishlistselector, fixedwishlistselector, fixedflyoutwishlistselector) {
        this.loadWaiting = false;
        this.usepopupnotifications = usepopupnotifications;
        this.topcartselector1 = topcartselector1;
        this.topcartselector2 = topcartselector2;
        this.fixedcartselector = fixedcartselector;
        this.flyoutcartselector = flyoutcartselector;
        this.fixedflyoutcartselector = fixedflyoutcartselector;
        this.topwishlistselector = topwishlistselector;
        this.fixedwishlistselector = fixedwishlistselector;
        this.fixedflyoutwishlistselector = fixedflyoutwishlistselector;
    },

    setLoadWaiting: function (display) {
        displayAjaxLoading(display);
        this.loadWaiting = display;
    },

    //add a product to the cart/wishlist from the catalog pages
    addproducttocart_catalog: function (urladd, quantity) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);
        if (quantity != undefined)
            urladd = this.urlRegReplace(urladd, quantity);
        $.ajax({
            cache: false,
            url: urladd,
            type: 'post',
            success: this.success_desktop,
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    //add a product to the cart/wishlist from the product details page (desktop version)
    addproducttocart_details: function (urladd, formselector, SuccessHandler, obj, imgUrl) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);
        var returnUrl = location.pathname;

        var successEvent = this.success_desktop;
        if (SuccessHandler != null && typeof (SuccessHandler) != "undefined")
            successEvent = SuccessHandler;

        $.ajax({
            cache: false,
            url: urladd,
            data: $(formselector).serialize() + "&returnUrl=" + returnUrl,
            type: 'post',
            success: function (response) {
                successEvent(response, obj, imgUrl);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    //add a product to the cart/wishlist from the product details page (mobile devices version)
    addproducttocart_details_mobile: function (urladd, formselector, successredirecturl) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        $.ajax({
            cache: false,
            url: urladd,
            data: $(formselector).serialize(),
            type: 'post',
            success: function (response) {
                //if (response.updatetopcartsectionhtml) {
                //    $(AjaxCart.topcartselector).html(response.updatetopcartsectionhtml);
                //}
                //if (response.updatetopwishlistsectionhtml) {
                //    $(AjaxCart.topwishlistselector).html(response.updatetopwishlistsectionhtml);
                //}
                if (response.message) {
                    //display notification
                    if (response.success == true) {
                        //we do not display success message in mobile devices mode
                        //just redirect a user to the cart/wishlist
                        location.href = successredirecturl;
                    }
                    else {
                        //error
                        displayStandardAlertNotification(response.message);
                    }
                    return false;
                }
                if (response.redirect) {
                    location.href = response.redirect;
                    return true;
                }
                return false;
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    success_desktop: function (response) {

        AjaxCart.update_cart_info(response);

        if (response.message) {
            //display notification
            if (response.success == true) {
                //success
                //if (AjaxCart.usepopupnotifications == true) {
                //    displayPopupNotification(response.message, 'success', true);
                //}
                //else {
                //    //specify timeout for success messages
                //    displayBarNotification(response.message, 'success', 3500);
                //}
                //displayAddtoCartDialog(response.message);
            }
            else {
                //error
                if (AjaxCart.usepopupnotifications == true) {
                    displayPopupNotification(response.message, 'error', true);
                }
                else {
                    //no timeout for errors
                    displayBarNotification(response.message, 'error', 0);
                }

            }
            return false;
        }
        if (response.redirect) {
            location.href = response.redirect;
            return true;
        }
        return false;
    },

    update_cart_info: function (response) {
        if (response.updatetopcartsectionhtml) {
            if (AjaxCart.topcartselector1)
                $(AjaxCart.topcartselector1).html(response.updatetopcartsectionhtml);
            if (AjaxCart.topcartselector2)
                $(AjaxCart.topcartselector2).html(response.updatetopcartsectionhtml);
        }
        if (response.updatefixedcartsectionhtml) {
            if (AjaxCart.fixedcartselector)
                $(AjaxCart.fixedcartselector).html(response.updatefixedcartsectionhtml);
        }
        if (response.updateflyoutcartsectionhtml) {
            if (AjaxCart.flyoutcartselector)
                $(AjaxCart.flyoutcartselector).replaceWith(response.updateflyoutcartsectionhtml);
        }
        if (response.updatefixedflyoutcartsectionhtml) {
            if (AjaxCart.fixedflyoutcartselector) {
                $("#fixedShoppingCart .numChat").show();
                $(AjaxCart.fixedflyoutcartselector).replaceWith(response.updatefixedflyoutcartsectionhtml);
            }
        }
        if (response.updatetopwishlistsectionhtml) {
            if (AjaxCart.topwishlistselector)
                $(AjaxCart.topwishlistselector).html(response.updatetopwishlistsectionhtml);
        }
        if (response.updatefixedwishlistsectionhtml) {
            if (AjaxCart.fixedwishlistselector) {
                $("#fixedWishlist .numChat").show();
                $(AjaxCart.fixedwishlistselector).html(response.updatefixedwishlistsectionhtml);
            }
        }
        if (response.updatefixedflyoutwishlistsectionhtml) {
            if (AjaxCart.fixedflyoutwishlistselector)
                $(AjaxCart.fixedflyoutwishlistselector).replaceWith(response.updatefixedflyoutwishlistsectionhtml);
        }

        if (response.removeSciIds) {
            var sciIds = response.removeSciIds.split(',');
            var items = $(".cartMain .item-content");
            $(items).each(function () {
                for (var i = 0; i < sciIds.length; i++) {
                    if ($(this).attr("sciId") == sciIds[i]) {
                        $(this).remove();
                    }
                }
            });
        }

        if (response.updateSciId) {
            var items = $(".cartMain .item-content");
            $(items).each(function () {
                if ($(this).attr("sciId") == response.updateSciId) {
                    if (response.updateSciDiscount)
                        $(this).find(".td-info").html(response.updateSciDiscount);
                    if (response.updateSciUnitPrice)
                        $(this).find(".td-price").html(response.updateSciUnitPrice);
                    if (response.updateSciQuantity)
                        $(this).find(".num_textinput").val(response.updateSciQuantity);
                    if (response.updateSciSubTotal)
                        $(this).find(".td-sum").html(response.updateSciSubTotal);
                    return;
                }
            });
        }

        if (response.updateordertotalhtml) {
            $("#sci-ordertotal").html(response.updateordertotalhtml);
        }

        if (response.ordersummaryhtml) {
            $("#ordersummary").html(response.ordersummaryhtml);
        }

        if (response.crosssellproductshtml) {
            $("#crosssellproducts").html(response.crosssellproductshtml);
        }
    },

    resetLoadWaiting: function () {
        AjaxCart.setLoadWaiting(false);
    },

    ajaxFailure: function () {
        //alert('Failed to add the product to the cart. Please refresh the page and try one more time.');
    },

    selectAllItems: function (urlselect, obj) {
        var checkboxs = $(".cartMain").find("input[type=checkbox]");
        //var checked = $("#selectAllItems").attr("checked") || $("#selectAllItems1").attr("checked");
        var checked = false;
        if ($(obj).attr("checked"))
            checked = true;
        $(checkboxs).each(function () {
            if ($(this).attr('id') != "selectAllItems" && $(this).attr('id') != "selectAllItems1") {
                $(this).attr("checked", checked);
            }
        });
        if ($(obj).attr("id") == "selectAllItems") {
            $("#selectAllItems1").attr("checked", checked);
        }
        else if ($(obj).attr("id") == "selectAllItems1") {
            $("#selectAllItems").attr("checked", checked);
        }
        //$("#selectAllItems").attr("checked", checked);
        //$("#selectAllItems1").attr("checked", checked);

        $.ajax({
            cache: false,
            url: urlselect,
            data: { selected: checked },
            type: 'post',
            success: function (response) {
                $("#sci-ordertotal").html(response.updateordertotalhtml);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    selectItemsByStore: function (obj) {
        var checkboxs = $(".cartMain").find("input[type=checkbox]");
        var checked = $(obj).attr("checked");
        var storeId = $(obj).val();
        $(checkboxs).each(function () {
            if ($(this).attr('storeId') == storeId) {
                if (checked)
                    $(this).attr("checked", true);
                else
                    $(this).attr("checked", false);
            }
        });
    },

    //remove a cart item
    deletecartitem: function (sciId, urldelete) {
        //$("input[name=removefromcart-" + sciId + "]").click();
        //return;
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        $.ajax({
            cache: false,
            url: urldelete,
            type: 'post',
            success: function (response) {
                if (response.redirect) {
                    location.href = response.redirect;
                } else {
                    $("#items-" + sciId + "").remove();
                    //$("#sci-ordertotal").html(response.updateordertotalhtml);
                    //$("#HeaderLinkCartItems").html("(" + response.updateOrderTotalProducts + ")");
                    AjaxCart.success_desktop(response);
                }
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    //remove cart items
    deletecartitems: function (urldelete, formselector) {
        var checkboxs = $(".cartMain").find("input[type=checkbox]");
        var isSelect = false;
        $(checkboxs).each(function () {
            if ($(this).attr('id') != "selectAllItems" && $(this).attr('id') != "selectAllItems1") {
                if ($(this).attr("checked"))
                    isSelect = true;
            }
        });
        if (isSelect) {
            if (this.loadWaiting != false) {
                return;
            }
            this.setLoadWaiting(true);
            $.ajax({
                cache: false,
                url: urldelete,
                data: $(formselector).serialize(),
                type: 'post',
                success: function (response) {
                    if (response.redirect) {
                        location.href = response.redirect;
                    } else {
                        var removes = response.updateRemoveSciIds.split(",");
                        for (var i = 0; i < removes.length; i++) {
                            $("#" + removes[i] + "").remove();
                        }
                        //$("#sci-ordertotal").html(response.updateordertotalhtml);
                        //$("#HeaderLinkCartItems").html("(" + response.updateOrderTotalProducts + ")");
                        AjaxCart.success_desktop(response);
                    }
                },
                complete: this.resetLoadWaiting,
                error: this.ajaxFailure
            });
        } else {
            alert("请选择要删除的商品！");
        }
    },

    updatecartitems: function (urlupdate, formselector) {
        $("input[name=updatecart]").click();
        return;
    },

    plusQuantity: function (urlupdate, obj, sciId,successHandler) {
        //if (this.loadWaiting != false) {
        //    return;
        //}
        //this.setLoadWaiting(true);

        if (!successHandler) {
            successHandler = this.success_desktop;
        }

        var input = $(obj).parent().find(".num-text").find("input");
        var qty = parseInt($(input).val());
        if (isNaN(qty))
            qty = 1;
        qty++;
        if (qty > 99)
            return;
        $(input).val(qty);
        //return;

        $.ajax({
            cache: false,
            url: urlupdate,
            data: { newQuantity: qty },
            type: 'post',
            success: function (response) {
                $("#subTotal" + sciId + "").html(response.updateSciSubTotal);
                //$("#sci-ordertotal").html(response.updateordertotalhtml);
                //$("#HeaderLinkCartItems").html("(" + response.updateOrderTotalProducts + ")");
                successHandler(response, sciId);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    minusQuantity: function (urlupdate, obj, sciId, successHandler) {
        //if (this.loadWaiting != false) {
        //    return;
        //}
        //this.setLoadWaiting(true);

        if (!successHandler) {
            successHandler = this.success_desktop;
        }

        var input = $(obj).parent().find(".num-text").find("input");
        var qty = parseInt($(input).val());
        if (isNaN(qty))
            qty = 1;
        qty--;
        if (qty <= 0)
            qty = 1;
        $(input).val(qty);
        //return;

        $.ajax({
            cache: false,
            url: urlupdate,
            data: { newQuantity: qty },
            type: 'post',
            success: function (response) {
                $("#subTotal" + sciId + "").html(response.updateSciSubTotal);
                //$("#sci-ordertotal").html(response.updateordertotalhtml);
                //$("#HeaderLinkCartItems").html("(" + response.updateOrderTotalProducts + ")");
                successHandler(response, sciId);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    changedQuantity: function (urlupdate, obj, sciId, successHandler) {
        if (!successHandler) {
            successHandler = this.success_desktop;
        }

        var input = $(obj);
        var qty = parseInt($(input).val());
        if (isNaN(qty))
            qty = 1;
        if (qty <= 0)
            qty = 1;
        $(input).val(qty);

        $.ajax({
            cache: false,
            url: urlupdate,
            data: { newQuantity: qty },
            type: 'post',
            success: function (response) {
                if (response.redirect) {
                    location.href = response.redirect;
                } else {
                    if (qty == 0) {
                        $("#" + response.updateSciId + "").remove();
                    } else {
                        $("#subTotal" + sciId + "").html(response.updateSciSubTotal);
                    }
                    //$("#HeaderLinkCartItems").html("(" + response.updateOrderTotalProducts + ")");
                    //$("#sci-ordertotal").html(response.updateordertotalhtml);
                    successHandler(response, sciId);
                }
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    selectItem: function (urlselect, obj, sciId) {
        var checked = false;
        if ($(obj).attr("checked"))
            checked = true;

        var checkboxs = $(".cartMain").find("input[type=checkbox]");
        var allChecked = true;
        for (var i = 0; i < checkboxs.length; i++) {
            if ($(checkboxs[i]).attr("id") == "selectAllItems"
                || $(checkboxs[i]).attr("id") == "selectAllItems1")
                continue;

            if (!$(checkboxs[i]).attr("checked")) {
                allChecked = false;
                break;
            }
        }
        $("#selectAllItems").attr("checked", allChecked);
        $("#selectAllItems1").attr("checked", allChecked);

        $.ajax({
            cache: false,
            url: urlselect,
            data: { selected: checked },
            type: 'post',
            success: function (response) {
                $("#sci-ordertotal").html(response.updateordertotalhtml);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    addproducttowishlistfromcart: function (sciId, urladd) {
        //$("input[name=addtowishlist-" + sciId + "]").click();
        //return;
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        $.ajax({
            cache: false,
            url: urladd,
            type: 'post',
            success: function (response) {
                if (response.redirect) {
                    location.href = response.redirect;
                    return true;
                }
                $("#items-" + sciId + "").remove();
                //$("#sci-ordertotal").html(response.updateordertotalhtml);
                //$("#HeaderLinkCartItems").html("(" + response.updateOrderTotalProducts + ")");
                AjaxCart.success_desktop(response);
            },
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },
    //remove cart items
    addproductstowishlistfromcart: function (urladd, formselector) {
        var checkboxs = $(".cartMain").find("input[type=checkbox]");
        var isSelect = false;
        $(checkboxs).each(function () {
            if ($(this).attr('id') != "selectAllItems" && $(this).attr('id') != "selectAllItems1") {
                if ($(this).attr("checked")) {
                    isSelect = true;
                }
            }
        });
        if (isSelect) {
            if (this.loadWaiting != false) {
                return;
            }
            this.setLoadWaiting(true);

            $.ajax({
                cache: false,
                url: urladd,
                data: $(formselector).serialize(),
                type: 'post',
                success: this.success_desktop,
                complete: this.resetLoadWaiting,
                error: this.ajaxFailure
            });
        } else {
            alert("请选择要移入收藏夹的商品！");
        }
    },
    //remove cart items
    clearShoppingCart: function (urladd, formselector) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        $.ajax({
            cache: false,
            url: urladd,
            data: $(formselector).serialize(),
            type: 'post',
            success: this.success_desktop,
            complete: this.resetLoadWaiting,
            error: this.ajaxFailure
        });
    },

    urlRegReplace: function (url, quantity) {
        var sreg = new RegExp("(/[A-za-z]+)*/", "gmi");
        var sr = sreg.exec(url);
        var reg = new RegExp("(" + sr[0] + ")(\\d+)/(\\d+)/(\\d+)(/.*)?", "gmi");

        var result = url.replace(reg, function () {
            var args = arguments;
            return args[5] != undefined ? args[1] + args[2] + "/" + args[3] + "/" + quantity + args[5] : args[1] + args[2] + "/" + args[3] + "/" + quantity
        });
        return result;
    }
};