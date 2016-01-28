//Echart工具
var Echart_Tools = {};

Echart_Tools.getSeries = function () {
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
        series: [
            {
                itemStyle: {
                    normal: {
                        color: '#1abc9c'
                    }
                },
                name: '',
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
                saveAsImage: {}
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