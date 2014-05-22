var AddToCartTools = {
    init: function (infos) {
        $("a.minus[data-input]").click(function () {
            try {
                var inputId = $(this).attr("data-input");
                var $inputBox = $("#" + inputId);
                var quantity = $inputBox.val();
                if (quantity == "" || quantity == "0")
                    $inputBox.val("1");
                else if (quantity <= 1)
                    return;
                else
                    $inputBox.val(parseInt(quantity) - 1);
            } catch (e) { };
        });

        $("a.plus[data-input]").click(function () {
            try {
                var inputId = $(this).attr("data-input");
                var $inputBox = $("#" + inputId);
                var quantity = $inputBox.val();
                if (quantity == "")
                    $inputBox.val("1");
                else if (quantity == "99")
                    return;
                else
                    $inputBox.val(parseInt(quantity) + 1);
            } catch (e) { };
        });

        $("a.add-to-cart[data-input]").click(function () {
            try {
                var pid = $(this).attr("data-pid");
                var inputId = $(this).attr("data-input");
                var quantity = $("#" + inputId).val();
                var picUrl = $(this).attr("data-picurl");
                quantity = 1;
                if (quantity == "")
                    alert(infos.QuantityRequired);
                else if (quantity <= 0)
                    alert(infos.QuantityGreaterThanZero);
                else {
                    AjaxCart.addproducttocart_catalog("/addproducttocart/catalog/" + pid + "/1/" + quantity);
                    ShowCart(this, picUrl);
                }
            } catch (e) { };
        });
    }
}