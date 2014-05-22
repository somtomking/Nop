/*
** nopCommerce one page checkout
*/


var Checkout = {
    loadWaiting: false,
    failureUrl: false,
    steps: new Array(),

    init: function (failureUrl) {
        this.loadWaiting = false;
        this.failureUrl = failureUrl;
        //this.steps = ['billing', 'shipping', 'shipping_method', 'payment_method', 'payment_info', 'confirm_order'];
        this.steps = ['shipping', 'shipping-method', 'payment-method', 'invoice'];

        //Accordion.disallowAccessToNextSections = true;
    },

    ajaxFailure: function () {
        location.href = Checkout.failureUrl;
    },

    _disableEnableAll: function (element, isDisabled) {
        var descendants = element.find('*');
        $(descendants).each(function () {
            if (isDisabled) {
                $(this).attr('disabled', 'disabled');
            } else {
                $(this).removeAttr('disabled');
            }
        });

        if (isDisabled) {
            element.attr('disabled', 'disabled');
        } else {
            element.removeAttr('disabled');
        }
    },

    setLoadWaiting: function (step, keepDisabled) {
        if (step) {
            if (this.loadWaiting) {
                this.setLoadWaiting(false);
            }
            var container = $('#' + step + '-buttons-container');
            container.addClass('disabled');
            container.css('opacity', '.5');
            this._disableEnableAll(container, true);
            $('#' + step + '-please-wait').show();
        } else {
            if (this.loadWaiting) {
                var container = $('#' + this.loadWaiting + '-buttons-container');
                var isDisabled = (keepDisabled ? true : false);
                if (!isDisabled) {
                    container.removeClass('disabled');
                    container.css('opacity', '1');
                }
                this._disableEnableAll(container, isDisabled);
                $('#' + this.loadWaiting + '-please-wait').hide();
            }
        }
        this.loadWaiting = step;
    },

    setPleaseConfirm: function (step, isVisible) {
        if (isVisible) {
            $('#' + step + '-please-confirm').show();
            document.location.href = "#" + step + "-point";
        } else
            $('#' + step + '-please-confirm').hide();
    },

    gotoSection: function (section) {
        if (section == "shipping-method")
            $("#opc-shipping-method-save").show();

        section = $('#opc-' + section);
        //section.addClass('allow');
        $(section).show();
        //Accordion.openSection(section);
    },

    closeSection: function (section) {
        if (section == "shipping-method")
            $("#opc-shipping-method-save").hide();

        section = $('#opc-' + section);
        //section.addClass('allow');
        $(section).hide();
        //Accordion.closeSection(section);
    },

    back: function () {
        if (this.loadWaiting) return;
        //Accordion.openPrevSection(true, true);
    },

    setStepResponse: function (response) {
        if (response.update_section) {
            $('#checkout-' + response.update_section.name + '-load').html(response.update_section.html);

            if (response.update_section.isedit) {
                //$('#checkout-' + response.update_section.name + '-colon').hide();
                $('#checkout-' + response.update_section.name + '-selection').hide();
                $('#checkout-' + response.update_section.name + '-edit').hide();
                Checkout.gotoSection(response.update_section.name);
            } else {
                //$('#checkout-' + response.update_section.name + '-colon').show();
                $('#checkout-' + response.update_section.name + '-selection').show();
                $('#checkout-' + response.update_section.name + '-edit').show();
                Checkout.closeSection(response.update_section.name);
                $('#checkout-' + response.update_section.name + '-selection').html(response.update_section.selection);
            }
        }

        if (response.shippingmethod_section) {
            $("#checkout-shipping-method-selection").html(response.shippingmethod_section.selection);
        }

        if (response.summary_html) {
            $("#checkout-order-summary").html(response.summary_html);
        }

        if (response.rewardpoints_html) {
            $("#checkout-rewardpoints").html(response.rewardpoints_html);
        }

        if (response.ordertotal_html) {
            $("#checkout-ordertotals").html(response.ordertotal_html);
        }

        if (response.goto_section) {
            $('#checkout-' + response.goto_section.name + '-load').html(response.goto_section.html);

            if (response.goto_section.isedit) {
                //$('#checkout-' + response.goto_section.name + '-colon').hide();
                //$('#checkout-' + response.goto_section.name + '-selection').hide();
                //$('#checkout-' + response.goto_section.name + '-edit').hide();
                Checkout.gotoSection(response.goto_section.name);
            } else {
                //$('#checkout-' + response.goto_section.name + '-colon').show();
                //$('#checkout-' + response.goto_section.name + '-selection').show();
                //$('#checkout-' + response.goto_section.name + '-edit').show();
                Checkout.closeSection(response.goto_section.name);
                $('#checkout-' + response.goto_section.name + '-selection').html(response.goto_section.selection);
            }
        }

        if (response.allow_sections) {
            response.allow_sections.each(function (e) {
                $('#opc-' + e).addClass('allow');
            });
        }

        //TODO move it to a new method
        if ($("#shipping-address-select").length > 0) {
            Shipping.newAddress(!$('#shipping-address-select').val());
        }

        if (response.redirect) {
            location.href = response.redirect;
            return true;
        }
        return false;
    }
};

//var ShippingDialog = {

//    show: function (isNew) {

//        if (isNew)
//            $("#lblSAD_title").html("新增收货地址");
//        else
//            $("#lblSAD_title").html("编辑收货地址");


//        $("#divShippingAddressDialog").lightbox_me({
//            centered: true,
//            onLoad: function () {
//                $("#divShippingAddressDialog").find("input:first").focus();
//            }
//        });
//    },

//    synchronous: function () {
//        $("#txtSAD_name").val($("#ShippingNewAddress_FirstName").val());
//        $("#txtSAD_phone").val($("#ShippingNewAddress_PhoneNumber").val());

//        $("#selSAD_province").html($("#ShippingNewAddress_StateProvinceId").html());
//        $("#selSAD_city").html($("#ShippingNewAddress_CityId").html());
//        $("#selSAD_district").html($("#ShippingNewAddress_DistrictId").html());

//        $("#txtSAD_address").val($("#ShippingNewAddress_Address1").val());
//        $("#txtSAD_zip").val($("#ShippingNewAddress_ZipPostalCode").val()); 
//    }
//};

var Shipping = {
    form: false,
    formUrl: false,
    saveUrl: false,
    getUrl: false,

    init: function (form, formUrl, saveUrl, getUrl) {
        this.form = form;
        this.formUrl = formUrl;
        this.saveUrl = saveUrl;
        this.getUrl = getUrl;
    },

    newAddress: function (isNew) {
        $("#pnlNewAddressFormTitle").show();
        $("#pnlEditAddressFormTitle").hide();

        $("#ShippingNewAddress_FirstName").val("");
        $("#ShippingNewAddress_ZipPostalCode").val("");
        $("#ShippingNewAddress_PhoneNumber").val("");
        $("#ShippingNewAddress_Address1").val("");
        $("#ShippingNewAddress_StateProvinceId option:nth-child(1)").attr("selected", "selected");
        $("#ShippingNewAddress_CityId option:nth-child(1)").attr("selected", "selected");
        $("#ShippingNewAddress_DistrictId option:nth-child(1)").attr("selected", "selected");

        $('#shipping-new-address-form-popup').lightbox_me({
            centered: true,
            //destroyOnClose: true,
            onLoad: function () {
                Shipping.clearErrors();
                $("#shipping_address_is_edit").val("false");
                $("#shipping_address_id").val("0");
            }
        });
    },

    editAddress: function (addressId) {
        $("#pnlNewAddressFormTitle").hide();
        $("#pnlEditAddressFormTitle").show();
        $('#shipping-new-address-form-popup').lightbox_me({
            centered: true,
            onLoad: function () {
                Shipping.clearErrors();
                Shipping.loadAddress(addressId);
                $("#shipping_address_is_edit").val("true");
                $("#shipping_address_id").val(addressId);
            }
        });
    },

    loadAddress: function (addressId) {
        $.ajax({
            cache: false,
            type: "GET",
            url: this.getUrl,
            data: { "addressId": addressId },
            dataType: "json",
            success: function (d) {
                if (d.Data.Success) {
                    //$("#ShippingNewAddressPopup_FirstName").val(d.Data.Model.FirstName);
                    //$("#ShippingNewAddressPopup_ZipPostalCode").val(d.Data.Model.ZipPostalCode);
                    //$("#ShippingNewAddressPopup_PhoneNumber").val(d.Data.Model.PhoneNumber);
                    //$("#ShippingNewAddressPopup_Address1").val(d.Data.Model.Address1);
                    //$("#ShippingNewAddressPopup_StateProvinceId").val(d.Data.Model.StateProvinceId);

                    $("#ShippingNewAddress_FirstName").val(d.Data.Model.FirstName);
                    $("#ShippingNewAddress_ZipPostalCode").val(d.Data.Model.ZipPostalCode);
                    $("#ShippingNewAddress_PhoneNumber").val(d.Data.Model.PhoneNumber);
                    $("#ShippingNewAddress_Address1").val(d.Data.Model.Address1);
                    $("#ShippingNewAddress_StateProvinceId").val(d.Data.Model.StateProvinceId);
                    LoadDistrictList(d.Data.Model.CityId, d.Data.Model.DistrictId);
                    LoadCityList(d.Data.Model.StateProvinceId, d.Data.Model.CityId);
                }
                else {
                    alert(d.Data.Message);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve states.');
            }
        });
    },

    clearErrors: function () {
        $(Shipping.form).find("span.field-validation-error").html("");
    },

    resetSelectedAddress: function () {
        var selectElement = $('#shipping-address-select');
        if (selectElement) {
            selectElement.val('');
        }
    },

    edit: function () {
        $.ajax({
            cache: false,
            url: this.formUrl,
            //data: $(this.form).serialize(),
            type: 'post',
            success: this.callback,
            //complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    selectAddress: function (addressId) {
        $("#shipping_address_is_edit").val("false");

        var items = $("#opc-shipping li");
        $(items).each(function () {
            $(this).removeClass("on");
            $(this).find(".ok").hide();
            if ($(this).attr("data-addressId") == addressId) {
                $(this).addClass("on");
                $(this).find(".ok").show();
                $("#shipping_address_id").val(addressId);
            }
        });
    },

    save: function (isNew, setToDefault) {
        if (Checkout.loadWaiting != false) return;

        if (!$(Shipping.form).valid())
            return;

        Checkout.setPleaseConfirm('shipping', false);
        Checkout.setLoadWaiting('shipping');

        if (setToDefault)
            $(Shipping.form).find(".settodefault").val("True");
        else
            $(Shipping.form).find(".settodefault").val("False");

        if (!$(Shipping.form).valid())
            return;

        $.ajax({
            cache: false,
            url: Shipping.saveUrl,
            data: $(Shipping.form).serialize(),
            type: 'post',
            success: this.callback,
            complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    openEditAddressDialog: function (addressid) {
        Shipping.editAddress(addressid);
        return false;
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    callback: function (response) {
        if (response.error) {
            if ((typeof response.message) == 'string') {
                alert(response.message);
            } else {
                alert(response.message.join("\n"));
            }
            return false;
        }

        if (response.address_invalid) {
            $("#shipping-address-content").html(response.update_section.html);
            return;
        }

        //确保 id=shipping-new-address-form-popup 的表单不会因为ligthbox_me插件功能
        //一直保留在页面，而在页面产生多个相同表单
        $("#shipping-new-address-form-popup").remove();
        $(".js_lb_overlay").remove();

        Checkout.setStepResponse(response);
    }
};


var ShippingMethod = {
    form: false,
    formUrl: false,
    saveUrl: false,

    init: function (form, formUrl, saveUrl) {
        this.form = form;
        this.formUrl = formUrl;
        this.saveUrl = saveUrl;
    },

    validate: function () {
        return true;
        var methods = document.getElementsByName('shippingoption');
        if (methods.length == 0) {
            alert('Your order cannot be completed at this time as there is no shipping methods available for it. Please make necessary changes in your shipping address.');
            return false;
        }

        for (var i = 0; i < methods.length; i++) {
            if (methods[i].checked) {
                return true;
            }
        }
        alert('Please specify shipping method.');
        return false;
    },

    edit: function () {
        if ($("#opc-shipping-method").hasClass("in")) {
            $("#opc-shipping-method").hide();
            $("#opc-shipping-method-save").hide();
            return;
        }

        $.ajax({
            cache: false,
            url: this.formUrl,
            //data: $(this.form).serialize(),
            type: 'post',
            success: this.callback,
            //complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;

        if (this.validate()) {
            Checkout.setPleaseConfirm('shipping-method', false);
            Checkout.setLoadWaiting('shipping-method');

            $.ajax({
                cache: false,
                url: this.saveUrl,
                data: $(this.form).serialize(),
                type: 'post',
                success: this.callback,
                complete: this.resetLoadWaiting,
                error: Checkout.ajaxFailure
            });
        }
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    callback: function (response) {
        if (response.error) {
            if ((typeof response.message) == 'string') {
                alert(response.message);
            } else {
                alert(response.message.join("\n"));
            }

            return false;
        }

        if (response.summary_html)
            $("#opc-order_summary").html(response.summary_html);
        Checkout.setStepResponse(response);
    }
};



var PaymentMethod = {
    form: false,
    formUrl: false,
    saveUrl: false,

    init: function (form, formUrl, saveUrl) {
        this.form = form;
        this.formUrl = formUrl;
        this.saveUrl = saveUrl;
    },

    validate: function () {
        var methods = document.getElementsByName('paymentmethod');
        if (methods.length == 0) {
            alert('Your order cannot be completed at this time as there is no payment methods available for it.');
            return false;
        }

        for (var i = 0; i < methods.length; i++) {
            if (methods[i].checked) {
                return true;
            }
        }
        alert('Please specify payment method.');
        return false;
    },

    edit: function () {
        $.ajax({
            cache: false,
            url: this.formUrl,
            //data: $(this.form).serialize(),
            type: 'post',
            success: this.callback,
            //complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;

        if (this.validate()) {
            Checkout.setPleaseConfirm('payment-method', false);
            Checkout.setLoadWaiting('payment-method');
            $.ajax({
                cache: false,
                url: this.saveUrl,
                data: $(this.form).serialize(),
                type: 'post',
                success: this.callback,
                complete: this.resetLoadWaiting,
                error: Checkout.ajaxFailure
            });
        }
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    callback: function (response) {
        if (response.error) {
            if ((typeof response.message) == 'string') {
                alert(response.message);
            } else {
                alert(response.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};


var Invoice = {
    form: false,
    formUrl: false,
    saveUrl: false,

    init: function (form, formUrl, saveUrl) {
        this.form = form;
        this.formUrl = formUrl;
        this.saveUrl = saveUrl;
    },

    validate: function () {
        return true;
        var methods = document.getElementsByName('invoice');
        if (methods.length == 0) {
            alert('Your order cannot be completed at this time as there is no invoice available for it.');
            return false;
        }

        for (var i = 0; i < methods.length; i++) {
            if (methods[i].checked) {
                return true;
            }
        }
        alert('Please specify invoice.');
        return false;
    },

    edit: function () {
        if ($("#opc-invoice").hasClass("in")) {
            $("#opc-invoice").hide();
            return;
        }

        $.ajax({
            cache: false,
            url: this.formUrl,
            //data: $(this.form).serialize(),
            type: 'post',
            success: this.callback,
            //complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    save: function (needInvoice) {
        if (Checkout.loadWaiting != false) return;

        $("#needInvoice").val(needInvoice);

        if (this.validate()) {
            Checkout.setPleaseConfirm('invoice', false);
            Checkout.setLoadWaiting('invoice');
            $.ajax({
                cache: false,
                url: this.saveUrl,
                data: $(this.form).serialize(),
                type: 'post',
                success: this.callback,
                complete: this.resetLoadWaiting,
                error: Checkout.ajaxFailure
            });
        }
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    callback: function (response) {
        if (response.error) {
            if ((typeof response.message) == 'string') {
                alert(response.message);
            } else {
                alert(response.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};


var RewardPoints = {
    form: false,
    formUrl: false,
    saveUrl: false,

    init: function (form, formUrl, saveUrl) {
        this.form = form;
        this.formUrl = formUrl;
        this.saveUrl = saveUrl;
    },

    validate: function () {
        return true;
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;

        if (!this.validate()) return;

        Checkout.setLoadWaiting('reward-points');
        $.ajax({
            cache: false,
            url: this.saveUrl,
            type: 'post',
            data: $(this.form).serialize(),
            success: this.nextStep,
            complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    resetLoadWaiting: function (transport) {
        Checkout.setLoadWaiting(false);
    },

    nextStep: function (response) {
        if (response.error) {
            if ((typeof response.message) == 'string') {
                alert(response.message);
            } else {
                alert(response.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};


var ConfirmOrder = {
    form: false,
    saveUrl: false,
    isSuccess: false,

    init: function (saveUrl, successUrl) {
        this.saveUrl = saveUrl;
        this.successUrl = successUrl;
    },

    validate: function () {
        if ($("#opc-shipping").hasClass("in") ||
            ($("#checkout-shipping-load").children().length > 0 && $("#opc-shipping").is(":visible"))) {
            //$("#shipping-please-confirm").show();
            Checkout.setPleaseConfirm('shipping', true);
            return false;
        }
        if (!$("#checkout-shipping-selection").html()) {
            //$("#shipping-please-confirm").show();
            Checkout.setPleaseConfirm('shipping', true);
            return false;
        }
        if ($("#opc-shipping-method").hasClass("in")) {
            //$("#shipping-method-please-confirm").show();
            Checkout.setPleaseConfirm('shipping-method', true);
            return false;
        }
        if ($("#opc-invoice").hasClass("in")) {
            Checkout.setPleaseConfirm('invoice', true);
            return false;
        }

        return true;
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;

        if (!this.validate()) return;

        Checkout.setLoadWaiting('confirm-order');
        $.ajax({
            cache: false,
            url: this.saveUrl,
            type: 'post',
            data: $("#co-payment-method-form").serialize(),
            success: this.nextStep,
            complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    resetLoadWaiting: function (transport) {
        Checkout.setLoadWaiting(false, ConfirmOrder.isSuccess);
    },

    nextStep: function (response) {
        if (response.error) {
            if ((typeof response.message) == 'string') {
                alert(response.message);
            } else {
                alert(response.message.join("\n"));
            }

            return false;
        }

        if (response.redirect) {
            ConfirmOrder.isSuccess = true;
            location.href = response.redirect;
            return;
        }
        if (response.success) {
            ConfirmOrder.isSuccess = true;
            window.location = ConfirmOrder.successUrl;
        }

        Checkout.setStepResponse(response);
    }
};


var Discount = {
    form: false,
    applyUrl: false,
    removeUrl: false,
    totalsContainer: false,
    infos: {},

    init: function (applyUrl, removeUrl, totalsContainer, infos) {
        this.applyUrl = applyUrl;
        this.removeUrl = removeUrl;
        this.totalsContainer = totalsContainer;
        this.infos = infos;

        $("#discount-couponcode").focus(function () {
            $("#input-code").attr("checked", "checked");
        });

        $("#add-discount").click(function () {
            var couponcode = $("input[name='discounttype']:checked").val();
            if (couponcode != "input-code") {
                $("#discount-couponcode").focus();
            }

            couponcode = $.trim($("#discount-couponcode").val());
            if (couponcode == "") {
                $("#apply-discount-message").html(Discount.infos.CouponCodeRequired);
                return;
            }

            Discount.apply(couponcode, true);
        });

        $("#remove-discount").click(function () {
            if (confirm(Discount.infos.Remove)) {
                Discount.remove();
            }
        });

        $("input[name='discounttype']").click(function () {
            var couponcode = $("input[name='discounttype']:checked").val();
            if (typeof (couponcode) == "undefined" || couponcode == "") return;

            if (couponcode != "input-code")
                Discount.apply(couponcode, false);
            else
                $("#apply-discount-message").html("");
        });
    },

    apply: function (couponcode, clearInputCouponcodeBox) {
        if (typeof (couponcode) == "undefined" || couponcode == "")
            return;

        $.ajax({
            cache: false,
            url: this.applyUrl,
            data: { discountcouponcode: couponcode },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    $("#pnlCurrentCodeMsg").html(response.currentCodeMessage);
                    $("#pnlCurrentCode").show();
                    //$('#' + Discount.totalsContainer).html(response.ordertotal_html);
                    $("#apply-discount-message").html("");

                    if (clearInputCouponcodeBox)
                        $("#discount-couponcode").val("");
                }
                else {
                    $("#apply-discount-message").html(response.message);
                }
                Checkout.setStepResponse(response);
            },
            complete: function () {
            },
            error: Checkout.ajaxFailure
        });
    },

    remove: function () {
        $.ajax({
            cache: false,
            url: this.removeUrl,
            data: { isRemove: true },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    $("#apply-discount-message").html("");
                    $("#pnlCurrentCodeMsg").html("");
                    $("#pnlCurrentCode").hide();
                    $("input[name='discounttype']").attr("checked", false);
                    //$('#' + Discount.totalsContainer).html(response.ordertotal_html);
                    Checkout.setStepResponse(response);
                }
            },
            complete: function () {
            },
            error: Checkout.ajaxFailure
        });
    }
};