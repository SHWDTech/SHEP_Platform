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
//每分钟更新一次数据
var timeoutId = null;

$(function () {
    tpGauge = echarts.init(document.getElementById('tpGauge'));

    dbGauge = echarts.init(document.getElementById('dbGauge'));

    mainChart = echarts.init(document.getElementById('divBar'));
});

var load = function (id, name) {
    mainChart.showLoading();
    tpGauge.showLoading();
    dbGauge.showLoading();
    var param = {
        'fun': 'getStatsActualData',
        'statId': id
    }

    $.post(ajaxUrl, param, function (ret) {
        if (ret) {
            setChart(ret, name, id);
            mainChart.hideLoading();
            tpGauge.hideLoading();
            dbGauge.hideLoading();
        }
    });
};

var setChart = function (list, name, id) {
    if (list.length === 0) {
        msg.warning('暂无最新数据！');
        return;
    }

    var option = Echart_Tools.getOption();

    option.series = [];
    option.legend.data = ['颗粒物', '噪音值'];
    option.yAxis = [
    {
         type: 'value', name: 'tp', axisLabel: { formatter: '{value}' }
    }, {
         type: 'value', name: 'db', axisLabel: { formatter: '{value}' }
    }];
    var seriesTp = Echart_Tools.getSeries();
    var seriesDb = Echart_Tools.getSeries();
    seriesTp.name = '颗粒物';
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
        option.xAxis.data = xAxisData;
        seriesTp.data = seriesTpData;
        seriesDb.data = seriesDbData;
    });
    option.series.push(seriesTp);
    option.series.push(seriesDb);

    mainChart.setOption(option);

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

    clearTimeout(timeoutId);
    timeoutId = setTimeout(function () { load(id, name); }, 60000);
};