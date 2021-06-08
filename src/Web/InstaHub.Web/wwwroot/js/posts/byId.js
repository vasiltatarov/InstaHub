function addInSaved(postId) {
    var json = { postId: postId };
    $.ajax({
        type: 'GET',
        url: "/api/userSavePost/save/" + postId,
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.message === 'It is already saved!') {
                alert('You have already Saved it');
            }
            const btn = document.getElementById('savePostBtn');
            btn.textContent = 'Saved';
        }
    });
}

function showCommentForm(parentId) {
    $("#AddCommentForm input[name='ParentId']").val(parentId);
    $("#AddCommentForm").toggle();
    var commentBtnText = document.getElementById('commentBtn').textContent.trim();
    if (commentBtnText === 'Comment') {
        document.getElementById('commentBtn').textContent = 'Close Comment form';
    } else {
        document.getElementById('commentBtn').textContent = 'Comment';
    }
    $('html, body').animate({
        scrollTop: $("#AddCommentForm").offset().top
    },
        1000);
}

function sendVote(postId, isUpVote) {
    var token = $("#votesForm input[name=__RequestVerificationToken]").val();
    var json = { postId: postId, isUpVote: isUpVote };
    $.ajax({
        url: "/api/votes",
        type: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            $("#votesCount").html(data.votesCount);
        }
    });
}