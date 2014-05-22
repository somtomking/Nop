var FormMsg = {
    MobileOK: "手机号码或邮箱填写正确",
    UserNameRequired: "邮箱/手机号码不能为空哦",
    UserNameFormat: "您输入的邮箱/手机号码格式有误",//您输入的邮箱格式有误，您可以选择手机号或邮箱作为账号
    MobileExsits: "很遗憾，该手机号码已被使用了",
    EmaiExsits: "很遗憾，该邮箱已被使用了",

    AuthenticodeOK: "验证码填写正确",
    AuthenticodeWrong: "您输入的验证码无效",
    AuthenticodeFormat: "您输入的验证码格式错误",
    AuthenticodeRequired: "验证码不能为空哦",

    PasswordOK: "密码填写正确",
    PasswordShort: "您输入的密码有点短，请输入4-16位字符哦",
    PasswordLong: "您输入的密码好长呀，请输入4-16位字符哦",
    PasswordRequired: "密码不能为空哦",
    PasswordFormat: '密码内不能含有"空格"哦',

    ConfirmPasswordOK: "确认密码填写正确",
    ConfirmPasswordNotEqualPassword: "两次密码输入不一致",
    ConfirmPasswordRequired: "确认密码不能为空哦",
};

var ExternalAuthBind = {
    usernameId: false,
    authenticodeId: false,
    passwordId: false,
    confirmPasswordId: false,
    checkMobileAvailabilityUrl: false,
    checkEmailAvailabilityUrl: false,
    checkExternalAuthBindAuthenticodeUrl: false,

    init: function (usernameId,
        authenticodeId,
        passwordId,
        confirmPasswordId,
        checkMobileAvailabilityUrl,
        checkEmailAvailabilityUrl,
        checkExternalAuthBindAuthenticodeUrl) {

        this.usernameId = usernameId;
        this.authenticodeId = authenticodeId;
        this.passwordId = passwordId;
        this.confirmPasswordId = confirmPasswordId;
        this.checkMobileAvailabilityUrl = checkMobileAvailabilityUrl;
        this.checkEmailAvailabilityUrl = checkEmailAvailabilityUrl;
        this.checkExternalAuthBindAuthenticodeUrl = checkExternalAuthBindAuthenticodeUrl;

        $("#" + this.usernameId).focus();

        $("#" + this.usernameId).focusout(function () {
            if ($.trim($("#" + ExternalAuthBind.usernameId).val()) != "")
                ExternalAuthBind.checkUserName();
            else
                $("#username_ok").hide();
        });

        $("#" + this.authenticodeId).focusout(function () {
            if ($.trim($("#" + ExternalAuthBind.authenticodeId).val()) != "")
                ExternalAuthBind.checkAuthenticode();
            else
                $("#authenticode_ok").hide();
        });

        $("#" + this.passwordId).focusout(function () {
            if ($.trim($("#" + ExternalAuthBind.passwordId).val()) != "")
                ExternalAuthBind.checkPassword();
            else
                $("#password_ok").hide();
        });

        $("#" + this.confirmPasswordId).focusout(function () {
            if ($.trim($("#" + ExternalAuthBind.confirmPasswordId).val()) != "")
                ExternalAuthBind.checkConfirmPassword();
            else
                $("#confirmpassword_ok").hide();
        });

        $("#get_authenticode_btn").click(function () {
            if (ExternalAuthBind.checkUserName()) {
                var userName = $.trim($("#" + ExternalAuthBind.usernameId).val());
                Authenticode.getAuthenticode({ "userName": userName });
            }
        });

        $("#cb_license").click(function () {
            if (ExternalAuthBind.checkLicense())
                $("#bind_btn").removeAttr("disabled");
            else
                $("#bind_btn").attr("disabled", "disabled");
        });

        $("#bind_btn").click(ExternalAuthBind.checkForm);
    },

    checkSubmit: function (e) {
        var ev = window.event || e;
        if (ev.keyCode == 13) {
            ExternalAuthBind.checkForm();
        }
    },

    showError: function (msg) {
        $("#errorMsgBox").show();
        $("#errorMsgBox").html(msg);
    },

    hidError: function () {
        $("#errorMsgBox").hide();
        $("#errorMsgBox").html("");
    },

    checkForm: function () {
        if (!ExternalAuthBind.checkLicense())
            return false;

        var result = false;
        result = ExternalAuthBind.checkUserName()
                 && ExternalAuthBind.checkAuthenticode()
                 && ExternalAuthBind.checkPassword()
                 && ExternalAuthBind.checkConfirmPassword();

        if (result) {
            $("#externalauthbind-form").attr("onsubmit", "return true;");
            //$("#externalauthbind-form").submit();
            $("#submit_btn").click();
        }
    },

    checkUserName: function () {
        $("#username_ok").hide();
        var username = $.trim($("#" + this.usernameId).val());
        if (username == '') {
            this.showError(FormMsg.UserNameRequired);
            //Authenticode.disableButton();
            return false;
        }
        else if (username.length > 30 ||
            (!/^\w+((\.\w+)|(-\w+))*\@[a-zA-Z0-9]+((\.|-)[a-zA-Z0-9]+)*\.[a-zA-Z0-9]+$/.test(username)
            && !/^1\d{10}$/.test(username)
            )) {
            this.showError(FormMsg.UserNameFormat);
            //Authenticode.disableButton();
            return false;
        }
        else {
            var available = false;
            var wrongMsg = "";
            if (/^1\d{10}$/.test(username)) {
                $.ajax({
                    type: 'POST',
                    url: ExternalAuthBind.checkMobileAvailabilityUrl,
                    data: { "mobile": username },
                    async: false,
                    success: function (result) {
                        available = result.Available;
                        if (!available) {
                            wrongMsg = FormMsg.MobileExsits;
                        }
                    }
                });
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: ExternalAuthBind.checkEmailAvailabilityUrl,
                    data: { "email": username },
                    async: false,
                    success: function (result) {
                        available = result.Available;
                        if (!available) {
                            wrongMsg = FormMsg.EmaiExsits;
                        }
                    }
                });
            }

            if (!available) {
                this.showError(wrongMsg);
                //Authenticode.disableButton();
                return false;
            }

            $("#username_ok").show();
            this.hidError();
            //Authenticode.enableButton();
            return true;
        }
    },

    checkAuthenticode: function () {
        $("#authenticode_ok").hide();
        var authenticode = $.trim($("#" + this.authenticodeId).val());
        if (authenticode == '') {
            this.showError(FormMsg.AuthenticodeRequired);
            return false;
        }
        else if (!/^\d{6}$/.test(authenticode)) {
            this.showError(FormMsg.AuthenticodeWrong);
            return false;
        }
        else {
            var username = $.trim($("#" + this.usernameId).val());
            var valid = false;
            $.ajax({
                type: 'POST',
                url: ExternalAuthBind.checkExternalAuthBindAuthenticodeUrl,
                data: { "authenticode": authenticode, "username": username },
                async: false,
                success: function (result) {
                    valid = result.Valid;
                }
            });

            if (!valid) {
                this.showError(FormMsg.AuthenticodeWrong);
                return false;
            }

            $("#authenticode_ok").show();
            this.hidError();
            return true;
        }
    },

    checkPassword: function () {
        $("#password_ok").hide();
        var password = $("#" + this.passwordId).val();
        if (password == '') {
            this.showError(FormMsg.PasswordRequired);
            return false;
        }
        else if (!/^\S{1,16}$/.test(password)) {
            this.showError(FormMsg.PasswordFormat);
            return false;
        }
        else if (password.length < 4) {
            this.showError(FormMsg.PasswordShort);
            return false;
        }
        else if (password.length > 16) {
            this.showError(FormMsg.PasswordLong);
            return false;
        }
        else {
            $("#password_ok").show();
            this.hidError();
            return true;
        }
    },

    checkConfirmPassword: function () {
        $("#confirmpassword_ok").hide();
        var password = $("#" + this.passwordId).val();
        var confirmPassword = $("#" + this.confirmPasswordId).val();
        if (confirmPassword == '') {
            this.showError(FormMsg.ConfirmPasswordRequired);
            return false;
        }
        else if (confirmPassword != password) {
            this.showError(FormMsg.ConfirmPasswordNotEqualPassword);
            return false;
        }
        else {
            $("#confirmpassword_ok").show();
            this.hidError();
            return true;
        }
    },

    checkLicense: function () {
        if (document.getElementById("cb_license").checked)
            return true;
        else
            return false;
    }
};


