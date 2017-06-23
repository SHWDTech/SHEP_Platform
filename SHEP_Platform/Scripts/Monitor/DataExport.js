//Ajax请求地址
var ajaxUrl = '/Ajax/Access';
$(function () {
    $('#startDate').datetimepicker({
        locale: 'zh-cn',
        format: 'YYYY-MM-DD'
    });

    $('#endDate').datetimepicker({
        locale: 'zh-cn',
        format: 'YYYY-MM-DD'
    });

    disableDatePicker();

    $('#daterange').on('change', function () {
        if (IsNullOrEmpty($('.daterange').val())) return;
        var startDateCtrl = $('#startDate').find('input');
        var endDateCtrl = $('#endDate').find('input');
        var startDate = new Date();
        var endDate = new Date();
        disableDatePicker();
        if ($('.daterange').val() === QueryDateRange.Today) {
            endDate.setDate(startDate.getDate() + 1);
            startDateCtrl.val(startDate.Format('yyyy-MM-dd'));
            endDateCtrl.val(endDate.Format('yyyy-MM-dd'));
        }else if ($('.daterange').val() === QueryDateRange.LastDay) {
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

function getSuccess(data) {
    if (data.result === "failed") {
        if (data.message === "unAuthorized") {
            alert("登录超时，请重新登陆！");
        }
        return false;
    }
    $('#export').show();
    var stats = $('#stat').val();
    var devs = $('#devs').val();

    var target = '/Monitor/ExportHistoryDataSheet?stat=';
    if (stats != null) {
        target += stats;
    }

    target += '&devs=';
    if (devs != null) {
        target += devs;
    }

    target += '&startDate=' + $('#startDate').find('input').val()
        + '&endDate=' + $('#endDate').find('input').val();
    $('#export').attr('href', target);
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

function dateFormatter(value) {
    var date = new Date(parseInt(value.slice(6, -2)));
    return date.Format("yyyy-MM-dd hh:mm:ss");
};