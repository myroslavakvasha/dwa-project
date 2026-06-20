let loginUrl = "http://localhost:5200/api/Auth/Login";

$(function () {
    $("#login-form").on("submit", function(e) {
        e.preventDefault();
        jwtLogin();
    });
});

function jwtLogin() {
    let loginData = {
        "username": $("#username").val(),
        "password": $("#password").val()
    }

    $.ajax({
        method: "POST",
        url: loginUrl,
        data: JSON.stringify(loginData),
        contentType: 'application/json'
    }).done(function (tokenData) {
        console.log(tokenData);
        localStorage.setItem("JWT", tokenData);
        window.location.href = "/html/logs.html";
    }).fail(function (err) {
        window.alert("Unauthorized to see the logs");
        localStorage.removeItem("JWT");
    });
}