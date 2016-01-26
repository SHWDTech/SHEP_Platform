//颗粒物仪表板
var tpGauge = null;
//噪音仪表板
var dbGauge = null;
//一小时数值变化表
var mainChart = null;
//Ajax请求地址
var ajaxUrl = '/Ajax/Access';
//表格主体
var tbody = $('#divTable').find('tbody');

$(function () {
    tpGauge = echarts.init(document.getElementById('tpGauge'));

    dbGauge = echarts.init(document.getElementById('dbGauge'));

    mainChart = echarts.init(document.getElementById('divBar'));
});

var load = function (id, name) {
    var param = {
        'fun': 'getStatsActualData',
        'statId': id
    }

    $.post(ajaxUrl, param, function (ret) {
        if (ret) {
            setChart(ret, name);
        }
    });
};

var setChart = function (list, name) {
    if (list.length === 0) {
        alert('暂无数据');
        return;
    }
    Echart_option.series = [];
    Echart_option.legend.data = ['颗粒物', '噪音值'];
    Echart_option.yAxis = [
    {
         type: 'value', name: 'tp', axisLabel: { formatter: '{value}' }
    }, {
         type: 'value', name: 'db', axisLabel: { formatter: '{value}' }
    }];
    var seriesTp = Echart_Tools.getSeries();
    var seriesDb = Echart_Tools.getSeries();
    seriesTp.name = '颗粒物';
    seriesTp.type = 'line';
    seriesTp.yAxisIndex = 1;
    seriesTp.itemStyle.normal.color = '#5d4bc1';
    seriesDb.name = '噪音值';
    seriesDb.itemStyle.normal.color = '#de366d';

    var xAxisData = [];
    var seriesTpData = [];
    var seriesDbData = [];
    $.each(list, function () {
        xAxisData.push($(this)[0].UpdateTime);
        seriesTpData.push($(this)[0].TP);
        seriesDbData.push($(this)[0].DB);
        Echart_option.xAxis.data = xAxisData;
        seriesTp.data = seriesTpData;
        seriesDb.data = seriesDbData;
    });
    Echart_option.series.push(seriesTp);
    Echart_option.series.push(seriesDb);

    mainChart.setOption(Echart_option);

    var tpGaugeOption = Echart_Tools.getGaugeOption();
    var dbGaugeOption = Echart_Tools.getGaugeOption();

    tpGaugeOption.title.text = '颗粒物';
    tpGaugeOption.series[0].data = { name: '颗粒物', value: $(list).last()[0].TP };
    dbGaugeOption.title.text = '噪音值';
    dbGaugeOption.series[0].min = 30;
    dbGaugeOption.series[0].max = 90;
    dbGaugeOption.series[0].data = { name: '噪音值', value: $(list).last()[0].DB };
    tpGauge.setOption(tpGaugeOption);
    dbGauge.setOption(dbGaugeOption);
    $('#itemTitle').find('h1').html(name + '实时数据');
};