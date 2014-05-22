var Login = {
    usernameId: false,
    passwordId: false,
    infos: {},

    init: function (usernameId, passwordId, infos) {
        this.usernameId = usernameId;
        this.passwordId = passwordId;
        this.infos = infos;

        $("#" + this.usernameId).focus();
        $("#" + this.passwordId).keyup(Login.checkSubmit);
        $("#register_btn").click(Login.checkForm);
    },

    checkUserName: function () {
        var username = $.trim($("#" + this.usernameId).val());
        if (username == "") {
            Login.showErrorMsg(Login.infos.EmailRequired);
            return false;
        }
        return true;
    },

    checkPassword: function () {
        var password = $.trim($("#" + this.passwordId).val());
        if (password == "") {
            Login.showErrorMsg(Login.infos.PasswordRequired);
            return false;
        }
        return true;
    },

    checkForm: function () {
        if (Login.checkUserName() && Login.checkPassword()) {
            $("#login-form").attr("onsubmit", "return true;");
            //$("#login-form").submit();
            $("#submit_btn").click();
            return false;
        }
    },

    checkSubmit: function (e) {
        var ev = window.event || e;
        if (ev.keyCode == 13) {
            Login.checkForm();
        }
    },

    showErrorMsg: function (msg) {
        $("#errorMsg").show();
        $("#errorMsg").html(msg);
    },

    hidErrorMsg: function () {
        $("#errorMsg").hide();
    }
};
