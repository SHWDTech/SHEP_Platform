//ECharts图表option对象
var Echart_option = null;

$(function () {
    Echart_option = {
        title: {
            text: ''
        },
        tooltip: {},
        legend: {
            data: ['']
        },
        xAxis: {
            data: []
        },
        yAxis: {},
        toolbox: {
            show: true,
            feature: {
                saveAsImage: {
                    type: 'png',
                    backgroundColor: 'auto',
                    excludeComponents: ['toolbox'],
                    show: true,
                    title: '保存为图片'
                },
                magicType: {
                    show: true,
                    type: ['line', 'bar'],
                    title: {
                        line: '切换为折线图',
                        bar: '切换为柱状图'
                    }
                }
            }
        },
        series: [{
            itemStyle: {
                normal: {
                    color: '#1abc9c'
                }
            },
            name: '',
            type: 'bar',
            data: []
        }]
    };
});