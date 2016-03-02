//污染物平均浓度柱形图
var avgChart = null;
//Ajax请求地址
var ajaxUrl = '/Ajax/Access';
//表格主体
var tbody = $('#avgTable').find('tbody');

$(function () {
    $('#startDate').datetimepicker({
        locale: 'zh-cn',
        format: 'L'
    });

    $('#endDate').datetimepicker({
        locale: 'zh-cn',
        format: 'L'
    });

    $('.PollutantType').on('change', function() {
        if (IsNullOrEmpty($('.daterange').val())) return;

        getStatAvgReport($(this).val(), $('.daterange').val(), getDatePickerString());
    });

    $('.daterange').on('change', function () {
        if (IsNullOrEmpty($('.PollutantType').val())) return;

        if ($(this).val() === QueryDateRange.Customer) {
            $('#dateQuery').css('display', 'inline-table');
        } else {
            $('#dateQuery').hide();
            getStatAvgReport($('.PollutantType').val(), $(this).val(), getDatePickerString());
        }
    });

    $('#customQuery').on('click', function () {
        if (IsNullOrEmpty($('.PollutantType').val())) return;
        if (IsNullOrEmpty($('.daterange').val())) return;
        getStatAvgReport($('.PollutantType').val(), $('.daterange').val(), getDatePickerString());
    });

    if (BaseInfo.IsMobileDevice) {
        $('.date').width(window.screen.width);
        $('#avgChart').width(window.screen.width);
        $('.daterange').css('width', '100%');
    }

    avgChart = echarts.init(document.getElementById('avgChart'));
    getStatAvgReport($('.PollutantType').val(), $('.daterange').val(), getDatePickerString());
});

var getStatAvgReport = function (pollutantType, queryDateRange, datePickerValue) {
    avgChart.showLoading();
    var param = {
        'fun': 'getStatAvgReport',
        'pollutantType': pollutantType,
        'queryDateRange': queryDateRange,
        'datePickerValue': datePickerValue
    }

    $.post(ajaxUrl, param, function (ret) {
        if (ret) {
            RefreashPage(ret);
            avgChart.hideLoading();
        }
    });
};

var RefreashPage = function (ret) {
    if (ret === '请求失败！') return;
    var polluteType;
    var color;
    if ($('.PollutantType').val() === PollutantType.ParticulateMatter) {
        polluteType = '颗粒物';
        color = '#5d4bc1';
    } else {
        polluteType = '噪音值';
        color = '#de366d';
    }

    var option = Echart_Tools.getOption();
    option.title.text = '污染物均值';
    option.legend.data = [polluteType];
    option.series[0].name = polluteType;
    option.series[0].itemStyle.normal.color = color;

    var xAxisData = [];
    var seriesData = [];
    $.each(ret, function() {
        xAxisData.push($(this)[0].Name);
        seriesData.push($(this)[0].AvgVal);
    });
    option.xAxis.data = xAxisData;
    option.series[0].data = seriesData;

    avgChart.clear();
    avgChart.setOption(option);

    tbody.html('');
    $.each(ret, function () {
        var tr = $('<tr></tr>');

        var td = $('<td></td>');
        td.html($(this)[0].Name);
        tr.append(td);

        td = $('<td></td>');
        td.html($(this)[0].AvgVal);
        tr.append(td);

        td = $('<td></td>');
        td.html($(this)[0].MaxVal);
        tr.append(td);

        td = $('<td></td>');
        td.html($(this)[0].MinVal);
        tr.append(td);

        td = $('<td></td>');
        td.html($(this)[0].ValidNum);
        tr.append(td);

        tbody.append(tr);
    });
};

var getDatePickerString = function() {
    return $('#startDate').find('input').val() + ',' + $('#endDate').find('input').val();
};