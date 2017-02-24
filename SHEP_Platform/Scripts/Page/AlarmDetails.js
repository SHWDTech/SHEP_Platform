$(function () {
    $('.alarmTitle').on('click', function () {
        var detail = $(this).next('div');
        var title = $(this);
        if (!detail.hasClass('view')) {
            detail.addClass('view');
        } else {
            detail.removeClass('view');
        }

        if (title.hasClass('notreaded')) {
            $.post("/Ajax/Access", { 'fun': 'alarmReaded', id: title.attr('id') }, function (result) {
                if (result === "success") {
                    title.removeClass('notreaded');
                }
            });
        }
    });
})