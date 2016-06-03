//Ajax请求地址
var ajaxUrl = '/Ajax/Access';
$(function () {
    $('#startDate').datetimepicker({
        locale: 'zh-cn',
        format: 'L'
    });

    $('#endDate').datetimepicker({
        locale: 'zh-cn',
        format: 'L'
    });

    disableDatePicker();

    $('#daterange').on('change', function () {
        if (IsNullOrEmpty($('.daterange').val())) return;
        var startDateCtrl = $('#startDate').find('input');
        var endDateCtrl = $('#endDate').find('input');
        var startDate = new Date();
        var endDate = new Date();
        disableDatePicker();
        if ($('.daterange').val() === QueryDateRange.LastDay) {
            startDate.setDate(endDate.getDate() - 1);
            startDateCtrl.val(startDate.Format('yyyy-MM-dd'));
            endDateCtrl.val(endDate.Format('yyyy-MM-dd'));
        } else if ($('.daterange').val() === QueryDateRange.LastWeek) {
            startDate.setDate(endDate.getDate() - 7);
            startDateCtrl.val(startDate.Format('yyyy-MM-dd'));
            endDateCtrl.val(endDate.Format('yyyy-MM-dd'));
        } else if ($('.daterange').val() === QueryDateRange.LastMonth) {
            startDate.setMonth(endDate.getMonth() - 1);
            startDateCtrl.val(startDate.Format('yyyy-MM-dd'));
            endDateCtrl.val(endDate.Format('yyyy-MM-dd'));
        } else if ($('.daterange').val() === QueryDateRange.LastYear) {
            startDate.setYear(endDate.getFullYear() - 1);
            startDateCtrl.val(startDate.Format('yyyy-MM-dd'));
            endDateCtrl.val(endDate.Format('yyyy-MM-dd'));
        } else if ($('.daterange').val() === QueryDateRange.Customer) {
            enableDatePicker();
        }
    });

    $('#stat').select2({
        placeholder: "---选择工地---"
    });

    $('#devs').select2({
        placeholder: "---选择设备---"
    });
});

function checkDate() {
    if (IsNullOrEmpty($('#startDate').find('input').val()) || IsNullOrEmpty($('#endDate').find('input').val())) {
        alert("请指定明确的查询时间范围！");
        return false;
    }
    if (IsNullOrEmpty($('#stat').val()) && IsNullOrEmpty($('#devs').val())) {
        alert("请至少选择一个工地或设备！");
        return false;
    }
    return true;
}

function disableDatePicker() {
    $('#startDate').find('input').attr('readonly', true);
    $('#endDate').find('input').attr('readonly', true);
};

function enableDatePicker() {
    $('#startDate').find('input').attr('readonly', false);
    $('#endDate').find('input').attr('readonly', false);
}