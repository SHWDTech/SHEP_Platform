//地图容器名称
var containerName = null;
//初始定位地址
var centerPosition = null;
//初始放大倍数
var zoom = null;
//地图对象
var map = null;

function map_init() {
    map = new BMap.Map(containerName);
    map.centerAndZoom(centerPosition, zoom);
    map.enableScrollWheelZoom();    //启用滚轮放大缩小，默认禁用
    map.enableContinuousZoom();    //启用地图惯性拖拽，默认禁用
};

var add_MapPoint = function(point, markType) {
    var icon = new BMap.Icon('pin.png', new BMap.Size(20, 32), {//是引用图标的名字以及大小，注意大小要一样
        anchor: new BMap.Size(10, 30)//这句表示图片相对于所加的点的位置
    });
    var mkr = new BMap.Marker(new BMap.Point(point.longitude, point.latitude), {
        icon: icon
    });

    map.addOverlay(mkr);
};