var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

connection.on("NewMessage",
    function (message) {
        if (message.text.trim() !== '') {
            var chatInfo = `
<br />
<div class="incoming_msg">
<div class="incoming_msg_img">
    <img class="rounded-circle" src="/uploads/${message.userImagePath}" alt="" />
</div>
<div class="received_msg">
    <div class="received_withd_msg">
        <p>${message.text}</p>
       <span style="color: red">${message.userUserName}</span>:${moment().format('llll')}
    </div>
</div>
</div>`;
            $("#messagesList").append(chatInfo);
            $(".messages").animate({ scrollTop: "99999" }, "fast");
        }
    });

$("#sendButton").click(function () {
    var message = $("#messageInput").val();
    connection.invoke("Send", message);
    $("#messageInput").val("");
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}


//Aplication logic HERE

$(".messages").animate({ scrollTop: "99999" }, "fast");

$("#profile-img").click(function () {
    $("#status-options").toggleClass("active");
});

$(".expand-button").click(function () {
    $("#profile").toggleClass("expanded");
    $("#contacts").toggleClass("expanded");
});

$("#status-options ul li").click(function () {
    $("#profile-img").removeClass();
    $("#status-online").removeClass("active");
    $("#status-away").removeClass("active");
    $("#status-busy").removeClass("active");
    $("#status-offline").removeClass("active");
    $(this).addClass("active");

    if ($("#status-online").hasClass("active")) {
        $("#profile-img").addClass("online");
    } else if ($("#status-away").hasClass("active")) {
        $("#profile-img").addClass("away");
    } else if ($("#status-busy").hasClass("active")) {
        $("#profile-img").addClass("busy");
    } else if ($("#status-offline").hasClass("active")) {
        $("#profile-img").addClass("offline");
    } else {
        $("#profile-img").removeClass();
    };

    $("#status-options").removeClass("active");
});

function newMessage() {
    message = $(".message-input input").val();
    if ($.trim(message) == '') {
        return false;
    }
    $('<li class="sent"><img src="http://emilcarlsson.se/assets/mikeross.png" alt="" /><p>' + message + '</p></li>').appendTo($('.messages ul'));
    $('.message-input input').val(null);
    $('.contact.active .preview').html('<span>You: </span>' + message);
    $(".messages").animate({ scrollTop: $(document).height() }, "fast");
};

$('.submit').click(function () {
    newMessage();
});

$(window).on('keydown',
    function (e) {
        if (e.which == 13) {
            newMessage();
            return false;
        }
    });