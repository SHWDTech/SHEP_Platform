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

    $('#customQuery').on('click', function () {
        if (IsNullOrEmpty($('.daterange').val())) return;
        LoadExportData();
    });

    $('#stat').select2({
        placeholder: "---选择工地---"
    });

    $('#devs').select2({
        placeholder: "---选择设备---"
    });
});

function LoadExportData() {
    var param = {
        'fun': "loadExportData",
        'queryDateRange': $('.daterange').val(),
        'datePickerValue': getDatePickerString(),
        'statList': $('#stat').val(),
        'deviceList': $('#devs').val()
    }

    $.post(ajaxUrl, param, function (ret) {
        if (ret.success) {
            
        }
    });
};

var getDatePickerString = function () {
    return $('#startDate').find('input').val() + ',' + $('#endDate').find('input').val();
};