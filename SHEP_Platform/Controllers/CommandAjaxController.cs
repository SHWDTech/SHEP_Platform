using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SHEP_Platform.Process;

namespace SHEP_Platform.Controllers
{
    public class CommandAjaxController : AjaxControllerBase
    {
        public JsonResult Access() => ParseRequest();

        // GET: Ajax
        protected override JsonResult ExecuteFun(string functionName)
        {
            switch (functionName)
            {
                case "addCoordinateTask":
                    return AddCoordinateTask();
                case "adjustingCoordinate":
                    return AdjustingCoordinate();
            }

            return null;
        }

        private JsonResult AddCoordinateTask()
        {
            var statId = Request["statId"];

            var dev = DbContext.T_Devs.FirstOrDefault(obj => obj.StatId == statId);

            if (dev == null)
            {
                return Json(new { success = false });
            }

            var task = new T_Tasks();
            var cmd = new DevCtrlCmd();
            cmd.EncodeGpsInfoCmd();

            cmd.GetTaskModel(dev.Id, ref task);

            DbContext.T_Tasks.Add(task);

            DbContext.SaveChanges();

            return Json(new { success = true, taskId = task.TaskId }, JsonRequestBehavior.AllowGet);
        }

        private JsonResult AdjustingCoordinate()
        {
            var taskId = Request["taskId"];

            var taskNotice = DbContext.T_TaskNotice.FirstOrDefault(obj => obj.TaskId.ToString() == taskId);
            if (taskNotice == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            var cmd = new DevCtrlResponseCmd();
            if (taskNotice.Length == null)
            {
                return Json(new {success = false }, JsonRequestBehavior.AllowGet);
            }

            cmd.DecodeFrame(taskNotice.Data, taskNotice.Length.Value);

            if (cmd.CmdByte != 0x2F)
            {
                return Json(new {success = false}, JsonRequestBehavior.AllowGet);
            }

            if (cmd.Data[0] == 0xFF)
            {
                return Json(new {success = true, coordinate = new { result = "failed"} }, JsonRequestBehavior.AllowGet);
            }

            var data = new byte[24];
            for (var i = 0; i < 24; i++)
            {
                if (cmd.Data[i] == 0x00)
                {
                    data[i] = 0x30;
                }
                else
                {
                    data[i] = cmd.Data[i];
                }
            }

            var sourceCoordinate = Encoding.ASCII.GetString(data, 0, 24).Insert(12, ",");
            var url = $"http://api.map.baidu.com/geoconv/v1/?coords={sourceCoordinate}&from=1&to=5&ak=0DpSiAEhexZzZR7c7pkYFq7E";

            var request = (HttpWebRequest) WebRequest.Create(url);
            var response = request.GetResponse();
            // ReSharper disable once AssignNullToNotNullAttribute
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var jsonSerializer = new JavaScriptSerializer();
            var result = (Dictionary<string, object>)((object[])((Dictionary<string, object>)jsonSerializer.DeserializeObject(responseString))["result"])[0];
            var longitude = double.Parse(result["x"].ToString());
            var latitude = double.Parse(result["y"].ToString());
            var coordinate = new {longitude = longitude.ToString("F6"), latitude = latitude.ToString("F6"), result = "success"};

            return Json(new {success = true, coordinate }, JsonRequestBehavior.AllowGet);
        }
    }
}