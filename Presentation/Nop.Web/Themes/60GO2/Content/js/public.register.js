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

    InviteCodeOK: "邀请码填写正确",
    InviteCodeWrong: "您输入的邀请码无效",
    InviteCodeRequired: "邀请码不能为空哦",
    InviteCodeHasBeUsed: "很遗憾，该邀请码已被使用了"
};

var Register = {
    usernameId: false,
    authenticodeId: false,
    authenticodeEnabled: false,//是否启用验证码
    passwordId: false,
    confirmPasswordId: false,
    inviteCodeId: false,
    inviteCodeEnabled: false,//是否启用邀请码
    checkMobileAvailabilityUrl: false,
    checkEmailAvailabilityUrl: false,
    checkRegisterAuthenticodeUrl: false,
    validateInviteCodeUrl: false,

    init: function (usernameId,
        authenticodeId,
        authenticodeEnabled,
        passwordId,
        confirmPasswordId,
        inviteCodeId,
        inviteCodeEnabled,
        checkMobileAvailabilityUrl,
        checkEmailAvailabilityUrl,
        checkRegisterAuthenticodeUrl,
        validateInviteCodeUrl) {

        this.usernameId = usernameId;
        this.authenticodeId = authenticodeId;
        this.authenticodeEnabled = authenticodeEnabled;
        this.passwordId = passwordId;
        this.confirmPasswordId = confirmPasswordId;
        this.inviteCodeId = inviteCodeId;
        this.inviteCodeEnabled = inviteCodeEnabled;
        this.checkMobileAvailabilityUrl = checkMobileAvailabilityUrl;
        this.checkEmailAvailabilityUrl = checkEmailAvailabilityUrl;
        this.checkRegisterAuthenticodeUrl = checkRegisterAuthenticodeUrl;
        this.validateInviteCodeUrl = validateInviteCodeUrl;

        $("#" + this.usernameId).focus();

        $("#" + this.usernameId).focusout(function () {
            if ($.trim($("#" + Register.usernameId).val()) != "")
                Register.checkUserName();
            else
                $("#username_ok").hide();
        });

        if (this.authenticodeEnabled) {
            $("#" + this.authenticodeId).focusout(function () {
                if ($.trim($("#" + Register.authenticodeId).val()) != "")
                    Register.checkAuthenticode();
                else
                    $("#authenticode_ok").hide();
            });
        }

        $("#" + this.passwordId).focusout(function () {
            if ($.trim($("#" + Register.passwordId).val()) != "")
                Register.checkPassword();
            else
                $("#password_ok").hide();
        });

        $("#" + this.confirmPasswordId).focusout(function () {
            if ($.trim($("#" + Register.confirmPasswordId).val()) != "")
                Register.checkConfirmPassword();
            else
                $("#confirmpassword_ok").hide();
        });

        if (this.inviteCodeEnabled) {
            $("#" + this.inviteCodeId).focusout(function () {
                if ($.trim($("#" + Register.inviteCodeId).val()) != "")
                    Register.checkInviteCode();
                else
                    $("#invitecode_ok").hide();
            });

            $("#" + this.inviteCodeId).keyup(Register.checkSubmit);
        }
        else {
            $("#" + this.confirmPasswordId).keyup(Register.checkSubmit);
        }

        $("#get_authenticode_btn").click(function () {
            if (Register.checkUserName()) {
                var userName = $.trim($("#" + Register.usernameId).val());
                Authenticode.getAuthenticode({ "userName": userName });
            }
        });

        $("#cb_license").click(function () {
            if (Register.checkLicense())
                $("#register_btn").removeAttr("disabled");
            else
                $("#register_btn").attr("disabled", "disabled");
        });

        $("#register_btn").click(Register.checkForm);
    },

    checkSubmit: function (e) {
        var ev = window.event || e;
        if (ev.keyCode == 13) {
            Register.checkForm();
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
        if (!Register.checkLicense())
            return false;

        var result = false;
        result = Register.checkUserName()
                 && Register.checkAuthenticode()
                 && Register.checkPassword()
                 && Register.checkConfirmPassword()
                 && Register.checkInviteCode();

        if (result) {
            $("#reg-form").attr("onsubmit", "return true;");
            //$("#reg-form").submit();
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
                    url: Register.checkMobileAvailabilityUrl,
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
                    url: Register.checkEmailAvailabilityUrl,
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
        if (!Register.authenticodeEnabled)
            return true;//如果没有启用验证码，忽略验证验证码

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
                url: Register.checkRegisterAuthenticodeUrl,
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

    checkInviteCode: function () {
        if (!Register.inviteCodeEnabled)
            return true;//如果没有启用邀请码，忽略验证邀请码

        var inviteCode = $.trim($("#" + this.inviteCodeId).val());
        if (inviteCode == '') {
            this.showError(FormMsg.InviteCodeRequired);
            return false;
        }
        else if (!/^\S{6}$/.test(inviteCode)) {
            this.showError(FormMsg.InviteCodeWrong);
            return false;
        }
        else {
            var valid = false;
            var isUsed = false;
            $.ajax({
                type: 'POST',
                url: Register.validateInviteCodeUrl,
                data: { "inviteCode": inviteCode },
                async: false,
                success: function (result) {
                    valid = result.Valid;
                    isUsed = result.IsUsed;
                }
            });

            if (!valid) {
                if (isUsed)
                    this.showError(FormMsg.InviteCodeHasBeUsed);
                else
                    this.showError(FormMsg.InviteCodeWrong);
                return false;
            }

            $("#invitecode_ok").show();
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


