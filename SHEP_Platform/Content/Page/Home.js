//信息面板
var broad = null;


$(function () {
    //先加载地图控件
    window.containerName = 'mapContainer';
    window.centerPosition = '上海';
    window.zoom = 12;
    var load = document.createElement("script");
    load.src = "http://api.map.baidu.com/api?v=1.4&callback=map_init";
    document.body.appendChild(load);

    //加载实时信息
    broad = $('#infobroad');
});