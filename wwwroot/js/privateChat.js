var routeURL = location.protocol + "//" + location.host;

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveNotification", function (message) {
    console.log(message);
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "15000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr["warning"](message);
});

connection.on("ReceivePrivateMessage", function (message) {
    //console.log(message);
    //<i class="bi bi-person"></i>     <img src="/img/vectors/person.png" alt=""></div>
    if (message.isSender) {
        var x = `<div class="single-text sent"> <div class= "profile-pic" > <img src="/img/vectors/person.png" alt=""></div>   <div class="text-content"><h5>${message.sender}</h5>${message.text}<span class="timestamp">${message.time}</span></div>    </div > `;
        $("#allMessages").append(x);
    } else {
        $("#allMessages").append(`<div  class="single-text"><div class="profile-pi">       <img src="/img/vectors/person.png" alt="">  </div>  <div class="text-content"><h5>${message.sender}</h5>${message.text}<span class="timestamp">${message.time}</span></div></div>`);

    }
    //$(`#${a}`).show();

    $("#allMessages").scrollTop($("#allMessages")[0].scrollHeight);

        //$("#allMessages").append(`<div class="single-text"><div class="profile-pic">        <i class="bi bi-person"></i>     <div class="text-content"><h5>${message.sender}</h5>${message.text}<span class="timestamp">${message.time}</span></div></div>`);
    //$('#allMessages').slideToggle();
    //$('#chat-texts').style.display = "block";
    //$("#chat-texts").css("display", "block");
    //document.getElementById('chat-texts').style.display = "block";
});


window.addEventListener("load", function () {
    var requestData = {
        ReceiverName: $('#receiverName').html()
    };
    console.log(requestData)
    $.ajax({
        url: routeURL + '/api/hubcontext/getPrivateMessges/' + $('#receiverName').html(),
        type: 'GET',

        contentType: 'application/json',
        success: function (response) {
            response.dataenum.forEach(function (message) {

                if (message.isSender) {
                    //document.getElementById('allMessages').innerHTML += `<div class="single-text sent"> <div class= "profile-pic" ><img src="/img/vectors/person.png" alt=""></div>    <div class="text-content"><h5>${message.sender}</h5>${message.text}<span class="timestamp">${message.time}</span></div>    </div > `;
                    $("#allMessages").append(`<div class="single-text sent"> <div class= "profile-pi" > <img src="/img/vectors/person.png" alt=""></div>    <div class="text-content"><h5>${message.sender}</h5>${message.text}<span class="timestamp">${message.time}</span></div>    </div > `);

                }
                else {
                    //document.getElementById('allMessages').innerHTML += `<div class="single-text sent"> <div class= "profile-pic" ><img src="/img/vectors/person.png" alt=""></div>    <div class="text-content"><h5>${message.sender}</h5>${message.text}<span class="timestamp">${message.time}</span></div>    </div > `;
                    $("#allMessages").append(`<div class="single-text"><div class="profile-pic">     <img src="/img/vectors/person.png" alt=""></div>    <div class="text-content"><h5>${message.sender+"asd"}</h5>${message.text}<span class="timestamp">${message.time}</span></div></div>`);

                }
                $("#allMessages").scrollTop($("#allMessages")[0].scrollHeight);
            });
        },
        error: function (xhr) {
            console.log("Error Occured", xhr);
        }
    });
});


connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});




function sendPrivateMessage() {
    if ($('#messageInput').val() != "") {
        var requestData = {
            Text: $('#messageInput').val(),
            ReceiverName: $('#receiverName').html()
        };
        //console.log(requestData);
        $.ajax({
            url: routeURL + '/api/hubcontext/sendPrivateMessage',
            type: 'POST',
            data: JSON.stringify(requestData),

            contentType: 'application/json',
            success: function (response) {
                //console.log("respnse is ", response);
            },
            error: function (xhr) {
                console.log("Error Occured", xhr);
            }
        });
    }
}

//const text = document.querySelectorAll(".text");
//let delay = 0;
//text.forEach(el => {
//    el.style.animation = "fade-in 1s ease forwards";
//    el.style.animationDelay = delay + "s";
//    delay += 0.33;
//});