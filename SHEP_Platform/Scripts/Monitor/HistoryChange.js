//颗粒物物柱形图
var tpChart = null;
//噪音值柱形图
var dbChart = null;
//Ajax请求地址
var ajaxUrl = '/Ajax/Access';
//当前展示工地ID
var curId = null;

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
            load(curId);
        }
    });

    $('#customQuery').on('click', function () {
        if (IsNullOrEmpty($('.daterange').val())) return;
        load(curId);
    });

    if (BaseInfo.IsMobileDevice) {
        $('.date').width(window.screen.width);
        $('#tpChart').width(window.screen.width);
        $('#dbChart').width(window.screen.width);
    }

    tpChart = echarts.init(document.getElementById('tpChart'));
    dbChart = echarts.init(document.getElementById('dbChart'));

    load(curId);
});

var load = function (statId) {
    if (curId === -1) return;
    curId = statId;
    tpChart.showLoading();
    dbChart.showLoading();
    if (IsNullOrEmpty($('.daterange').val())) return;

    var param = {
        'fun': 'load',
        'id': statId,
        'queryDateRange': $('.daterange').val(),
        'datePickerValue': getDatePickerString()
    }

    $.post(ajaxUrl, param, function (ret) {
        if (ret) {
            reSetChart(ret);
            tpChart.hideLoading();
            dbChart.hideLoading();
        }
    });
};

var getDatePickerString = function () {
    return $('#startDate').find('input').val() + ',' + $('#endDate').find('input').val();
};

var reSetChart = function (obj) {
    if (obj === '请求失败！' || obj.data.length === 0) {
        msg.warning('暂无最新数据！');
        return;
    }

    var tpOption = Echart_Tools.getOption();
    var dbOption = Echart_Tools.getOption();

    tpOption.title.text = '颗粒物历史数据';
    tpOption.legend.data = ['颗粒物'];
    tpOption.series[0].name = '颗粒物';
    tpOption.series[0].itemStyle.normal.color = '#5d4bc1';

    dbOption.title.text = '噪音值历史数据';
    dbOption.legend.data = ['噪音值'];
    dbOption.series[0].name = '噪音值';
    dbOption.series[0].itemStyle.normal.color = '#de366d';

    var xAxisData = [];
    var seriesTpData = [];
    var seriesDbData = [];
    $.each(obj.data, function () {
        xAxisData.push($(this)[0].UpdateTime);
        seriesTpData.push($(this)[0].TP);
        seriesDbData.push($(this)[0].DB);
    });

    tpOption.xAxis.data = xAxisData;
    dbOption.xAxis.data = xAxisData;

    tpOption.series[0].data = seriesTpData;
    dbOption.series[0].data = seriesDbData;

    tpChart.setOption(tpOption);
    dbChart.setOption(dbOption);
};