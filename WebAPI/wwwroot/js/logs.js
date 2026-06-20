let logsUrlBase = "http://localhost:5200/api/Log/get/";
let token = localStorage.getItem("JWT");

$(function () {
    loadLogs();

    $(".action-button").on("click", function () {
        loadLogs();
    });

    $(".logout-button").on("click", function (){
        jwtLogout();
    })
})

function loadLogs(){
    let n = $("#log-count").val();
    let logsUrl = logsUrlBase + n;

    $.ajax({
        url: logsUrl,
        headers: { "Authorization": `Bearer ${token}` }
    }).done(function (data) {
        $("#log-list").empty();

        $.each(data, function (i, log) {
            $("#log-list").append(`<tr><td>${log.timestamp}</td><td>${log.logLevelTitle}</td><td>${log.message}</td></tr>`);
        });
    }).fail(function (err){
        alert("Unauthorized to see the logs");
        localStorage.removeItem("JWT");
    });
}

function jwtLogout() {
    localStorage.removeItem("JWT");
    window.location.href = "login.html";
}