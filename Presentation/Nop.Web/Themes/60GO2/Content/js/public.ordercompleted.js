var OrderCompleted = {
    orderTotal: 0.00,
    cashBalance: 0.00,
    cashCanBeUsed: 0.00,
    cashToUsed: 0.00,
    currencySymbol: "¥",
    selectingPaymentMethod: true,

    init: function (orderTotal, cashCanBeUsed, cashBalance, currencySymbol) {
        this.orderTotal = OrderCompleted.formatDecimal(orderTotal);
        this.cashBalance = OrderCompleted.formatDecimal(cashBalance);
        this.cashCanBeUsed = OrderCompleted.formatDecimal(cashCanBeUsed);
        this.currencySymbol = currencySymbol;

        $(document).ready(function () {
            $("#txtCashToUsed").val(cashCanBeUsed);
            $("#lblCashToUsed").html(OrderCompleted.currencySymbol + OrderCompleted.formatDecimal(cashCanBeUsed));
            $("#spnPaymentMethod").show();
            $("#lblCashToUsed").show();
            $("#txtCashToUsed").hide();
            $("#updateCashToUsed").hide();
            $("#saveCashToUsed").hide();
            $("#cancelUpdateCashToUsed").hide();
            //$("#spnRemainPaymentTotal").hide();
            $("#CheckoutCompletedForm").attr("target", "_blank");
            $("#useCash").removeAttr("checked");
            $("input:radio[name='PaymentMethod']").removeAttr("checked");
            $("#confirmpaymentmethod").show();
            $("#selectotherpaymentmethod").hide();
            $("#paymentsubmitbtn").hide();
            OrderCompleted.resetPageContent();

            $("#showOrderSummary").click(function () {
                $("#pnlOrderSummary").show();
                $("#pnlOrderDetails").hide();
            });

            $("#showOrderDetails").click(function () {
                $("#pnlOrderDetails").show();
                $("#pnlOrderSummary").hide();
            });

            $("#useCash").click(function () {
                if ($("#useCash").attr("checked")) {
                    $("#updateCashToUsed").show();
                    $("#saveCashToUsed").hide();
                    //$("#spnRemainPaymentTotal").show();

                    var inputCashToUsed = OrderCompleted.getInputCashToUsed();
                    OrderCompleted.cashToUsed = inputCashToUsed;

                    if (parseFloat(OrderCompleted.cashToUsed) == parseFloat(OrderCompleted.orderTotal)) {
                        $("#spnPaymentMethod").hide();
                        $("#CheckoutCompletedForm").removeAttr("target");
                    }
                    else {
                        OrderCompleted.calculateRemainPaymentTotal();
                        $("#spnPaymentMethod").show();
                        $("#CheckoutCompletedForm").attr("target", "_blank");
                    }
                }
                else {
                    OrderCompleted.cashToUsed = 0.00;
                    $("#spnPaymentMethod").show();
                    $("#lblCashToUsed").show();
                    $("#txtCashToUsed").hide();
                    $("#updateCashToUsed").hide();
                    $("#saveCashToUsed").hide();
                    $("#cancelUpdateCashToUsed").hide();
                    //$("#spnRemainPaymentTotal").hide();
                }

                OrderCompleted.calculateRemainPaymentTotal();
                OrderCompleted.resetPageContent();
            });

            $("#updateCashToUsed").click(function () {
                $("#lblCashToUsed").hide();
                $("#updateCashToUsed").hide();
                $("#txtCashToUsed").show();
                $("#saveCashToUsed").show();
                $("#cancelUpdateCashToUsed").show();

                OrderCompleted.resetPageContent();
            });

            $("#cancelUpdateCashToUsed").click(function () {
                $("#txtCashToUsed").val(OrderCompleted.cashToUsed);

                $("#lblCashToUsed").show();
                $("#txtCashToUsed").hide();

                $("#updateCashToUsed").show();
                $("#saveCashToUsed").hide();
                $("#cancelUpdateCashToUsed").hide();

                OrderCompleted.resetPageContent();
            });

            $("#saveCashToUsed").click(function () {
                var inputCashToUsed = OrderCompleted.getInputCashToUsed();
                if (parseFloat(inputCashToUsed) > parseFloat(OrderCompleted.orderTotal)) {
                    alert("现金支付余额不能大于订单总金额");
                    return;
                }
                if (parseFloat(inputCashToUsed) > parseFloat(OrderCompleted.cashCanBeUsed)) {
                    alert("您的现金余额不足");
                    return;
                }

                OrderCompleted.cashToUsed = inputCashToUsed;

                $("#txtCashToUsed").val(inputCashToUsed);
                $("#lblCashToUsed").html(OrderCompleted.currencySymbol + inputCashToUsed);
                $("#lblCashToUsed").show();
                $("#updateCashToUsed").show();
                $("#txtCashToUsed").hide();
                $("#saveCashToUsed").hide();
                $("#cancelUpdateCashToUsed").hide();

                if (parseFloat(OrderCompleted.cashToUsed) == parseFloat(OrderCompleted.orderTotal)) {
                    $("#spnPaymentMethod").hide();
                    $("#CheckoutCompletedForm").removeAttr("target");
                }
                else {
                    OrderCompleted.calculateRemainPaymentTotal();
                    $("#spnPaymentMethod").show();
                    $("#CheckoutCompletedForm").attr("target", "_blank");
                }

                OrderCompleted.resetPageContent();
            });

            $(".payment_method_list li").click(function () {
                $(".payment_method_list li").removeAttr("class");
                $(".payment_method_list li").find(":radio").attr("checked", false);
                $(this).attr("class", "on");
                $(this).find(":radio").attr("checked", true);

                OrderCompleted.resetPageContent();
            });

            $("#confirmpaymentmethod").click(function () {
                OrderCompleted.selectingPaymentMethod = false;

                //复制一份图片Logo显示
                var selectedPaymentMethodLogo = $("input:radio[name='PaymentMethod']:checked").parent("li").find("img").clone(false);
                if (selectedPaymentMethodLogo.attr("data-paymenttype") == "chinapay")
                    $("#paymentMethodTypeName").html("网上银行：");
                else
                    $("#paymentMethodTypeName").html("支付平台：");
                $("#confirmPaymentMethodLogoContainer").html(selectedPaymentMethodLogo);

                OrderCompleted.calculateRemainPaymentTotal();
                OrderCompleted.resetPageContent();
            });

            $("#selectotherpaymentmethod").click(function () {
                OrderCompleted.selectingPaymentMethod = true;
                OrderCompleted.resetPageContent();
            });
        });
    },

    doPayment: function () {
        if (OrderCompleted.canDoPayment()) {
            if (OrderCompleted.useCash() && parseFloat(OrderCompleted.cashToUsed) == parseFloat(OrderCompleted.orderTotal)) {
                OrderCompleted.disablePayment();
            }
            else {
                $("#dialog").lightbox_me({ centered: true });
            }
            $("#paymentsubmit").removeAttr("disabled");
            $("#paymentsubmit").click();
        }
    },

    calculateRemainPaymentTotal: function () {
        var remainPaymentTotal = OrderCompleted.formatDecimal(OrderCompleted.orderTotal - OrderCompleted.cashToUsed);
        $("#remainPaymentTotal").html(OrderCompleted.currencySymbol + OrderCompleted.formatDecimal(remainPaymentTotal));
    },

    useCash: function () {
        if ($("#useCash").attr("checked"))
            return true;
        else
            return false;
    },

    resetPaymentSubmitButton: function () {
        if (OrderCompleted.canDoPayment())
            OrderCompleted.enablePayment();
        else
            OrderCompleted.disablePayment();
    },

    resetPageContent: function () {
        if (OrderCompleted.selectingPaymentMethod) {
            $("#confirmpaymentmethod").show();
            $("#selectotherpaymentmethod").hide();
            $("#paymentsubmitbtn").hide();
        }
        else {
            $("#confirmpaymentmethod").hide();
            $("#selectotherpaymentmethod").show();
            $("#paymentsubmitbtn").show();
        }

        if (OrderCompleted.canDoPayment()) {
            OrderCompleted.enablePayment();
            $("#confirmpaymentmethod").removeAttr("disabled");

            if (OrderCompleted.useCash()
                && parseFloat(OrderCompleted.cashToUsed) == parseFloat(OrderCompleted.orderTotal)) {
                $("#spnPaymentList").hide();
                $("#spnConfirmPaymentMethod").hide();

                $("#confirmpaymentmethod").hide();
                $("#selectotherpaymentmethod").hide();
                $("#paymentsubmitbtn").show();
            }

            if (!OrderCompleted.selectingPaymentMethod) {
                $("#spnPaymentList").hide();
                $("#spnConfirmPaymentMethod").show();
            }
            else {
                $("#spnPaymentList").show();
                $("#spnConfirmPaymentMethod").hide();
            }
        }
        else {
            OrderCompleted.disablePayment();
            $("#confirmpaymentmethod").attr("disabled", "disabled");

            if (OrderCompleted.selectingPaymentMethod) {
                $("#spnPaymentList").show();
                $("#spnConfirmPaymentMethod").hide();
            }
            else {
                $("#spnPaymentList").hide();
                $("#spnConfirmPaymentMethod").show();
            }
        }
    },

    canDoPayment: function () {
        if (OrderCompleted.useCash()) {
            if ($("#lblCashToUsed").is(":hidden"))
                return false;

            if (parseFloat(OrderCompleted.cashToUsed) == parseFloat(OrderCompleted.orderTotal))
                return true;
        }

        var selectedPaymentMethod = $("input:radio[name='PaymentMethod']:checked").val() != null;
        return selectedPaymentMethod;
    },

    getInputCashToUsed: function () {
        var cashToUsed = $.trim($("#txtCashToUsed").val());
        return OrderCompleted.formatDecimal(cashToUsed);
    },

    disablePayment: function () {
        $("#paymentsubmit").attr("disabled", "disabled");
        $("#paymentsubmitbtn").attr("disabled", "disabled");
    },

    enablePayment: function () {
        $("#paymentsubmit").removeAttr("disabled");
        $("#paymentsubmitbtn").removeAttr("disabled");
    },

    formatDecimal: function (num) {
        //num = parseFloat(num);
        //return num.toFixed(2);
        return (num * 100 / 100).toFixed(2);
    }
};