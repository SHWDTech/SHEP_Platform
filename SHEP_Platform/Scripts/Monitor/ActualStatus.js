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
//校零指令列表
var tasks = [];
//重复查询结果任务
var requestId = null;

$(function () {
    if (BaseInfo.IsMobileDevice) {
        $('#tpGauge').width(window.screen.width).height(window.screen.width);
        $('#dbGauge').width(window.screen.width).height(window.screen.width);
    }

    tpGauge = echarts.init(document.getElementById('tpGauge'));

    dbGauge = echarts.init(document.getElementById('dbGauge'));

    if (!BaseInfo.IsMobileDevice) {
        mainChart = echarts.init(document.getElementById('divBar'));
    }
});

var setDeviceMin = function(id) {
    var param = {
        'fun': 'setDeviceMin',
        'statid': id
    }

    $('#setMin').prop('disabled', 'disabled');
    $('#setMin').val('校零（执行中）');
    $.post(ajaxUrl, param, function(ret) {
        if (ret.taskAdd === true) {
            tasks = ret.tasks;
            questTaskResult(tasks, id);
        }
    });
}

var questTaskResult = function(tasks, statId) {
    var param = {
        'fun': 'questTaskResult',
        'tasks': JSON.stringify(tasks)
    }

    $('#setMin').val('校零（等待结果）');
    $.post(ajaxUrl, param, function(ret) {
        if (ret.success === false) {
            requestId = setTimeout(function() { questTaskResult(tasks) }, 5000);
        } else {
            clearTimeout(requestId);
            startProof(statId);
        }
    });
}

var startProof = function(statId) {
    var param = {
        'fun': 'startProof',
        'statid': statId
    }

    $.post(ajaxUrl, param, function(ret) {
        if (ret.success === true) {
            $('#setMin').val('校零（完成）');
            $('#setMin').prop('disabled', '');
        } else {
            setTimeout(function () { startProof(statId); }, 5000);
        }
    });
}

var load = function (id, name) {
    if (id === -1 || name === 'null') return;
    if (!BaseInfo.IsMobileDevice) {
        mainChart.showLoading();
    }
    tpGauge.showLoading();
    dbGauge.showLoading();
    var param = {
        'fun': 'getStatsActualData',
        'statId': id
    }

    $.post(ajaxUrl, param, function (ret) {
        if (ret) {
            setChart(ret, name, id);
            if (!BaseInfo.IsMobileDevice) {
                mainChart.hideLoading();
            }
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
    option.legend.data = ['颗粒物', '噪音值', 'PM2.5', 'PM10'];
    option.yAxis = [
    {
         type: 'value', name: 'tp', axisLabel: { formatter: '{value}' }
    }, {
         type: 'value', name: 'db', axisLabel: { formatter: '{value}' }
    }];
    var seriesTp = Echart_Tools.getSeries('颗粒物', 'line', PollutantColor.PM);
    var seriesDb = Echart_Tools.getSeries('噪音值', 'line', PollutantColor.Noise);
    var seriesPm25 = Echart_Tools.getSeries('PM2.5', 'line', PollutantColor.PM25);
    var seriesPm100 = Echart_Tools.getSeries('PM10', 'line', PollutantColor.PM100);
    seriesTp.yAxisIndex = 1;
    seriesPm25.yAxisIndex = 1;
    seriesPm100.yAxisIndex = 1;

    var xAxisData = [];
    var seriesTpData = [];
    var seriesDbData = [];
    var seriesPm25Data = [];
    var seriesPm100Data = [];
    $.each(list, function () {
        xAxisData.push($(this)[0].UpdateTime);
        seriesTpData.push($(this)[0].TP);
        seriesDbData.push($(this)[0].DB);
        seriesPm25Data.push($(this)[0].PM25);
        seriesPm100Data.push($(this)[0].PM100);
        option.xAxis.data = xAxisData;
        seriesTp.data = seriesTpData;
        seriesDb.data = seriesDbData;
        seriesPm25.data = seriesPm25Data;
        seriesPm100.data = seriesPm100Data;
    });
    option.series.push(seriesTp);
    option.series.push(seriesDb);
    option.series.push(seriesPm25);
    option.series.push(seriesPm100);

    if (!BaseInfo.IsMobileDevice) {
        mainChart.setOption(option);
    }

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
    $('#itemTitle').find('#chartTitle').html(name + '实时数据');

    clearTimeout(timeoutId);
    timeoutId = setTimeout(function () { load(id, name); }, 60000);
};