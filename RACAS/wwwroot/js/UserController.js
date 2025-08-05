
var objCommon = new Common();
function LoginUser() {

    var data = $('#formLogin').serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});

    if (data.Email == "" || data.Password == "") {
        objCommon.ShowMessage("Email/Password required.", "error");
        return;
    }

    objCommon.AjaxCall("../userlogin/LoginWeb", JSON.stringify(data), "POST", true, function (d) {
        console.log(d);
        sessionStorage.setItem("Object", JSON.stringify(d));
        if (d != null) {
            sessionStorage.setItem("token", d.token);
            window.location.href = objCommon.baseUrl + "Admin/ReconciliationReport";
        }
    });


}

$(function () {

    var hash = location.hash.substr(1);

    if (hash != null && hash != undefined && hash != "") {

        var sURLVariables = hash.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == "access_token") {
                hash=sParameterName[1];
            }
        }

        objCommon.AjaxCall("../userlogin/AfterLoginAd", JSON.stringify({ AccessToken: hash }), "POST", true, function (d) {
            console.log(d);
            sessionStorage.setItem("Object", JSON.stringify(d));
            if (d != null) {
                sessionStorage.setItem("token", d.token);
                window.location.href = objCommon.baseUrl +"Admin/ReconciliationReport";
            }
        });
    }

  

})

