    $(document).ready(function () {
        'use strict';
        xLoadFun();
    });

var data = {
    'FullName': "",
    'Phone': "",
    'Email': "",
    'Message': "",
    'Subject': "",
    'Response': ""
}
var widgetId1;
initReCaptcha: function xLoadFun() {
        document.getElementById("fullname").value = "";
    document.getElementById("phone").value = "";
    document.getElementById("email").value = "";
    document.getElementById("message").value = "";
    setTimeout(function () {
        if (typeof grecaptcha === 'undefined' || typeof grecaptcha.render === 'undefined') {
            this.initReCaptcha();
        } else {
            grecaptcha.render('captcha2', {
                sitekey: document.getElementById("apiKey").value,
                callback: this.submit,
                'expired-callback': this.expired
            });
        }
    }.bind(this), 100);
}
//function xLoadFun() {

//    document.getElementById("fullname").value = "";
//    document.getElementById("phone").value = "";
//    document.getElementById("email").value = "";
//    document.getElementById("message").value = "";
//    document.getElementById("type").value = "";
//    if (document.getElementById("apiKey").value.length >5) {
//        widgetId1 = grecaptcha.render('captcha2', {
//            'sitekey': document.getElementById("apiKey").value,
//            'theme': 'light'
//        });
//    }


//}



$("#submitmessage").on("click", function (e) {
    data.FullName = document.getElementById("fullname").value;
    data.Phone = document.getElementById("phone").value;
    data.Email = document.getElementById("email").value;
    data.Message = document.getElementById("message").value;
    data.Response = grecaptcha.getResponse();
    $.ajax({
        url: '/api/GetVerify',
        method: 'Post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (data) {
            console.log(data);
            if (data.state) {
                window.alert("Success");
            }
            else {
                window.alert("Fail")
            }
        },
        error: function (x) {
            console.log(x);
            window.alert("Fail");
            
        },
        async: false
    });
});