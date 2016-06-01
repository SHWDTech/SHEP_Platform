$(function () {
    $('#startDate').datetimepicker({
        locale: 'zh-cn',
        format: 'L'
    });

    $('#endDate').datetimepicker({
        locale: 'zh-cn',
        format: 'L'
    });

    $('.daterange').on('change', function () {
        if ($(this).val() === QueryDateRange.Customer) {
            $('#dateQuery').css('display', 'inline-table');
        } else {
            $('#dateQuery').hide();
        }
    });

    $('#customQuery').on('click', function () {
        if (IsNullOrEmpty($('.daterange').val())) return;
        load(curId);
    });
});