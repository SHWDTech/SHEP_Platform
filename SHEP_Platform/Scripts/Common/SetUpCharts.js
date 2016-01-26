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

var Echart_Tools = {
    getSeries: function () {
        var series = {
            itemStyle: {
                normal: {
                    color: '#1abc9c'
                }
            },
            name: '',
            type: 'bar',
            data: []
        };
        return series;
    },
    getGaugeOption: function () {
        var option = {
            title: {
                text: ''
            },
            tooltip: {
                formatter: "{a} <br/>{b} : {c}%"
            },
            toolbox: {
                show: true,
                feature: {
                    saveAsImage: {
                        type: 'png',
                        backgroundColor: 'auto',
                        excludeComponents: ['toolbox'],
                        show: true,
                        title: '保存为图片'
                    }
                }
            },
            series: [
                {
                    name: "",
                    type: "gauge",
                    min: 0,
                    max: 2,
                    title: {
                        show: true,
                        offsetCenter: [
                            0,
                            90
                        ],
                        textStyle: {
                            color: "#333",
                            fontSize: 16
                        }
                    },
                    detail: {
                        formatter: "{value}",
                        offsetCenter: [0, '70%'],
                        height: 40
                    },
                    data: [
                        {
                            name: "",
                            value: ""
                        }
                    ]
                }]
        }

        return option;
    }
};