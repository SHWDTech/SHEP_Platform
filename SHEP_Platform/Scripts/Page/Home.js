//信息面板
var broad = null;


$(function () {
    //先加载地图控件
    containerName = 'mapContainer';

    //如果是移动设备，重新设置地图的高宽
    if (BaseInfo.IsMobileDevice) {
        $('.infoWrap').height(window.screen.height - 160);
        $('#' + containerName).width(window.screen.width);
        $('#' + containerName).height(window.screen.height - 120);
        $('#' + containerName).css('position', 'absolute');
        $('#' + containerName).css('top', 60);
        $('#' + containerName).hide();
        $('#mapSwitcher').on('click', function() { switchMap() });
    }
    centerPosition = '上海';
    zoom = 12;
    var load = document.createElement("script");
    load.src = "http://api.map.baidu.com/api?v=1.4&callback=map_init(updateStats)";
    document.body.appendChild(load);
});

var updateStats = function() {
    //加载实时信息
    broad = $('#infobroad');

    $(statusInfo).each(function () {
        var ul = $('<ul></ul>');

        $(ul).append('<li class="title"><a href="/Monitor/ActualStatus/' + $(this)[0].Id + '">' + $(this)[0].Name + '</a></li>');
        $(ul).append('<li><span class="text pm">颗粒物</span><span class="num safe">' + $(this)[0].AvgTp + '(mg/m³)</span></li>');
        $(ul).append('<li><span class="text db">噪音</span><span class="num safe">' + $(this)[0].AvgDb + '(dB)</span></li>');
        $(ul).append('<li><span class="date">' + $(this)[0].UpdateTime + '</span></li>');
        broad.append(ul);

        var point = { 'longitude': $(this)[0].Longitude, 'latitude': $(this)[0].Latitude }
        add_MapPoint(point, $(this)[0].PolluteType, $(this)[0]);
    });
}

var switchMap = function () {
    map.centerAndZoom(centerPosition, zoom);
    if (IsShow(containerName)) {
        $('#' + containerName).fadeIn();
    } else {
        $('#' + containerName).fadeOut();
    }
}