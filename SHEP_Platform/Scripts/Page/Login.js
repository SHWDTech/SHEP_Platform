$(function() {
    $('.form-control').on('focus', function () {
        $(this).parents('.textbox-wrap').addClass("focuesd");
    });

    $('.form-control').on('blur', function () {
        $(this).parents('.textbox-wrap').removeClass("focuesd");
    });
})