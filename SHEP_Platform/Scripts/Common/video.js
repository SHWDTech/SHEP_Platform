// For version detection, set to min. required Flash Player version, or 0 (or 0.0.0), for no version detection. 
var swfVersionStr = "11.1.0";
// To use express install, set to playerProductInstall.swf, otherwise the empty string. 
var xiSwfUrlStr = "playerProductInstall.swf";
var flashvars = {};
var params = {};
params.quality = "high";
params.bgcolor = "#ffffff";
params.allowscriptaccess = "always";
params.allowfullscreen = "true";
var attributes = {};
attributes.id = "WebAPI2";
attributes.name = "WebAPI2";
attributes.align = "middle";
swfobject.embedSWF(
	"http://121.42.34.0:2600/webclient_api/WebAPI2.swf", "flashContent",
	"100%", "600px", 
	swfVersionStr, xiSwfUrlStr, 
	flashvars, params, attributes);
// JavaScript enabled so display the flashContent div in case it is not replaced with a swf object.
swfobject.createCSS("#flashContent", "display:block;text-align:left;");


//获取视频
function getvideo(){
    var ip = "rtmp://121.42.34.0/myapp";
	var did = document.getElementById("selDevs").value;
	document.getElementById("WebAPI2").getvideo(ip,did);
}
//获取一个设备的视频
function getonedevvideo(did){
	closevideo();
	alert("已请求视频，请稍候！");
	var ip = document.getElementById("ip").value;
	document.getElementById("WebAPI2").getvideo(ip,did);
}

//关闭视频
function closevideo(){
	document.getElementById("WebAPI2").closevideo();
}

//获取设备列表
function getdevlist(){
	var serverip = document.getElementById("serverip").value;
	var user = document.getElementById("username").value;
	var pwd = document.getElementById("pwd").value;
	
	var urlxx = "http://"+serverip+":2600/bin/getUserAccessDevicesimpleforwebapi?uname="+user+"&pwd="+pwd;
	
	$.ajax({
		type : "get",
		url : urlxx,
		dataType : "jsonp",
		jsonpCallback:"success_jsonpCallback",//自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
		success : function(json){
			var r = json[0].resultxx;
			if (r == "ok")
			{
				buildtree(json[0].data);
			}
			else{
				alert(json[0].data);
			}
			//alert(json[0].did);
			//alert(json[0].dname);
			
		},
		error:function(){
			alert('fail');
		}
	});
	//document.getElementById("WebAPI2").initservice(serverip);
}
function delect_tabletrx(table){//清除表
	var U= table.rows.length;
	for(var i=1;i<U;i++){
		table.deleteRow(1);
	}
}
function buildtree(data){
	var device_table = document.getElementById("devlisttb");
	delect_tabletrx(device_table);
	for (var i=0;i<data.length ;i++ ){
		var did = data[i]["did"];
		var dname = data[i]["dname"];
		
		//添加表
		var r = device_table.insertRow(i+1);
		var onecell = r.insertCell(0);
		onecell.width = 40;
		onecell.innerHTML= did;
		var onecell1 = r.insertCell(1);
		onecell1.innerHTML= "<span class='btncursor' onclick='getonedevvideo(\""+ did+"\")'>"+dname+"</span>";
	}
}

function jsfun(){//这个是flex调用js的方法
	alert("myJavaScriptFunction invoked  zwj ok");
}



//////////////////////////云台
function left(){
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(20,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function right(){
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(21,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function up(){
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(18,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function down(){
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(19,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function far()
{
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(2,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function near()
{
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(1,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function focus_far()
{
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(5,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function focus_near()
{
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(4,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function aperture_out()
{
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(9,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function aperture_in()
{
	try {
	    var did = $("#selDevs").val();
		document.getElementById("WebAPI2").PTZcontrol(10,did);
	} 
	catch(error) 
	{
		alert("请先点击连接");
	}
}
function connect()
{
    var serverip = "121.42.34.0";
    document.getElementById("WebAPI2").initservice(serverip);
}
function getphotolist()
{
	var serverip = "121.42.34.0";
	var did = document.getElementById("selDevspic").value;
	var urlxx = "http://"+serverip+":2600/bin/getOneDeviceImages_api?did="+did;
	
	$.ajax({
		type : "get",
		url : urlxx,
		dataType : "jsonp",
		jsonpCallback:"success_jsonpCallback",//自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
		success : function(json){
			var r = json[0].resultxx;
			if (r == "ok")
			{
				var datas = json[0].data[1];
				setuppic(datas);
			}
			else{
				alert(json[0].data);
			}
		},
		error:function(){
			alert('fail');
		}
	});
}

function setuppic(data) {
    var serverip = "121.42.34.0";
    var picCantiner = $("#picCantiner");
    for (var i = 0; i < data.length ; i++) {
        var time = data[i][1];
        var name = data[i][0];
        var path = "http://" + serverip + ":2600/imgdown/" + name;
        picCantiner.append('<img src="' + path + '" style="height:240px;"/>');
    };
};
function getpicture(name)
{
	var serverip = "121.42.34.0";
	var urlxx = "http://"+serverip+":2600/imgdown/"+name;
	
	document.getElementById("image").src=urlxx;
}
function buildtree2(data){
	var photos_table = document.getElementById("photostb");
	delect_tabletrx(photos_table);
	for (var i=0;i<data.length ;i++ ){
		var time = data[i][1];
		var name = data[i][0];
		//添加表
		var r = photos_table.insertRow(i+1);
		var onecell = r.insertCell(0);
		onecell.width = 200;
		onecell.innerHTML= time;
		var onecell1 = r.insertCell(1);
		var arrs=name.split("|");
		onecell1.innerHTML= "<span class='btncursor' onclick='getpicture(\""+ name+"\")'>"+arrs[arrs.length-1]+"</span>";
	}
}
function takepicture()
{
    var serverip = "121.42.34.0";

	var check1 = document.getElementById("check1");
	var check2 = document.getElementById("check2");
	var check3 = document.getElementById("check3");
	var check4 = document.getElementById("check4");
	var cameras = '';
	if (check1.checked){cameras += "1,";}
	if (check2.checked){cameras += "2,";}
	if (check3.checked){cameras += "3,";}
	if (check4.checked){cameras += "4,";}
	cameras = cameras.substring(0,cameras.length-1);


	//var myselect=document.getElementById("cameras");
	//var index=myselect.selectedIndex ;
	//var cameras=myselect.options[index].value;
	
	var myselect2=document.getElementById("qua");
	var index2=myselect2.selectedIndex ;
	var qua=myselect2.options[index2].value;
	
	var myselect3=document.getElementById("res");
	var index3=myselect3.selectedIndex ;
	var res=myselect3.options[index3].value;

	var did = document.getElementById("selDevs").value;
	var urlxx = "http://"+serverip+":2600/bin/OneDevicePhone_api?did="+did+"&cameras="+cameras+"&qua="+qua+"&res="+res;
	
	$.ajax({
		type : "get",
		url : urlxx,
		dataType : "jsonp",
		jsonpCallback:"success_jsonpCallback_pic",//自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
		success : function(json){
			var r = json[0].resultxx;
			if (r == "ok"){
				alert("ok");
			}else{
				alert("error");
			}
		},
		error:function(){
			//alert('fail');
		}
	});
}
