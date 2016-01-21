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