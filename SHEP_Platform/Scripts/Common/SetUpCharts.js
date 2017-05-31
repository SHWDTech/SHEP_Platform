﻿//Echart工具
var Echart_Tools = {};

Echart_Tools.getSeries = function (name, type, color, data, stack) {
    var series = {
        itemStyle: {
            normal: {
                color: '#1abc9c'
            }
        },
        name: '',
        type: 'bar',
        data: [],
        stack: '',
        markPoint: {
            data: []
        },
        markLine: {
            data: []
        }
    };

    if (name) {
        series.name = name;
    }
    if (type) {
        series.type = type;
    }
    if (color) {
        series.itemStyle.normal.color = color;
    }
    if (data) {
        series.data = data;
    }
    if (stack) {
        series.stack = stack;
    }

    return series;
};

Echart_Tools.getGaugeOption = function () {
    var option = {
        title: {
            text: ''
        },
        tooltip: {
            formatter: "{a} <br/>{b} : {c}"
        },
        toolbox: {
            show: true,
            feature: {
                saveAsImage: {
                    type: 'png',
                    backgroundColor: '#FFFFFF',
                    excludeComponents: ['toolbox'],
                    show: true,
                    title: '保存为图片'
                }
            }
        },
        backgroundColor: '#FFFFFF',
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
            }
        ]
    }

    return option;
};

Echart_Tools.getOption = function () {
    var option = {
        title: {
            text: ''
        },
        tooltip: {},
        legend: {
            data: ['']
        },
        xAxis: {
            selected: {
                '颗粒物': false,
                '噪音值': false,
                'PM2.5': false,
                'PM10': true
            },
            data: []
        },
        yAxis: {},
        toolbox: {
            show: true,
            feature: {
                saveAsImage: {
                    type: 'png',
                    backgroundColor: '#FFFFFF',
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
        backgroundColor: '#FFFFFF',
        series: [
            {
                itemStyle: {
                    normal: {
                        color: '#1abc9c'
                    }
                },
                name: '',
                min: 0,
                type: 'bar',
                data: [],
                markPoint: {
                    data: []
                },
                markLine: {
                    data: []
                }
            }
        ]
    };

    return option;
};

Echart_Tools.getStackLineOption = function () {
    var option = {
        title: {
            text: ''
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: []
        },
        toolbox: {
            feature: {
                type: 'png',
                backgroundColor: '#FFFFFF',
                excludeComponents: ['toolbox'],
                show: true,
                title: '保存为图片'
            }
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                boundaryGap: false,
                data: []
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: []
    };

    return option;
};

Echart_Tools.ResetData = function (charts) {
    charts.forEach(function (chart) {
        chart.clear();
    });
};