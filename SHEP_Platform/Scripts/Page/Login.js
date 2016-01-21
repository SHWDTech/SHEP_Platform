$(function() {
    $('input[type=text]').on('focus', function () {
        $(this).parents('.textbox-wrap').addClass("focuesd");
    });

    $('input[type=text]').on('blur', function () {
        $(this).parents('.textbox-wrap').removeClass("focuesd");
    });
})