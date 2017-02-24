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
    tpOption.legend.data = ['颗粒物','PM2.5', 'PM10'];
    tpOption.series = [];
    var seriesTp = Echart_Tools.getSeries('颗粒物', 'bar', PollutantColor.PM);
    var seriesPm25 = Echart_Tools.getSeries('PM2.5', 'bar', PollutantColor.PM25);
    var seriesPm100 = Echart_Tools.getSeries('PM10', 'bar', PollutantColor.PM100);

    dbOption.title.text = '噪音值历史数据';
    dbOption.legend.data = ['噪音值'];
    dbOption.series[0].name = '噪音值';
    dbOption.series[0].itemStyle.normal.color = PollutantColor.Noise;

    var xAxisData = [];
    var seriesTpData = [];
    var seriesDbData = [];
    var seriesPm25Data = [];
    var seriesPm100Data = [];
    $.each(obj.data, function () {
        xAxisData.push($(this)[0].UpdateTime);
        seriesTpData.push($(this)[0].TP);
        seriesDbData.push($(this)[0].DB);
        seriesPm25Data.push($(this)[0].PM25);
        seriesPm100Data.push($(this)[0].PM100);
    });

    tpOption.xAxis.data = xAxisData;
    dbOption.xAxis.data = xAxisData;

    seriesTp.data = seriesTpData;
    seriesPm25.data = seriesPm25Data;
    seriesPm100.data = seriesPm100Data;
    dbOption.series[0].data = seriesDbData;

    if ($('.daterange').val() === QueryDateRange.LastHour) {
        seriesTp.type = 'line';
        seriesPm25.type = 'line';
        seriesPm100.type = 'line';
        dbOption.series[0].type = 'line';
    }

    tpOption.series.push(seriesTp);
    tpOption.series.push(seriesPm25);
    tpOption.series.push(seriesPm100);

    tpChart.setOption(tpOption);
    dbChart.setOption(dbOption);

    $('#chartTitle').html(obj.statName);

    reSetTable(obj);
};

function reSetTable(obj) {
    if (obj.average.length === 0) {
        return;
    }

    var targets = $('tr[statid=' + obj.statId + ']');
    targets.find('.pm').html(obj.average.TP);
    targets.find('.db').html(obj.average.DB);
    targets.find('.pm25').html(obj.average.PM25);
    targets.find('.pm100').html(obj.average.PM100);

    $('span[statid=' + obj.statId + ']').html('（' + obj.dttypename +'）');
};