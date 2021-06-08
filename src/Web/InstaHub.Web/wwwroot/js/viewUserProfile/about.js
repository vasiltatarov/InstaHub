function addDescription() {
    const content = document.getElementById('descriptionText').value;
    var json = { content: content };

    $.ajax({
        type: 'GET',
        url: "/api/userSettings/changeDescription/" + content,
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
        }
    });
}