using butterfly.com.rest;
using butterfly.com.rest.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Butterfly.Service.Schedule.Model
{
    public class DayGroupRestDispatcher : RestDispatcher
    {

        public override void DispatchRestMethod(RestMethod method)
        {
            throw new NotImplementedException();
        }

        public override void GET(HttpContext context)
        {
            List<DayGroup> days = ScheduleDataService.GetDayGroups(String.Empty);
            if (days != null)
            {
                this.ResultOK(context);
                Serializer.Serialize(days, context.Response.OutputStream);
            }           
        }

        public override void POST(HttpContext context)
        {
            DayGroup day = Serializer.Deserialize(context.Request.InputStream, typeof(DayGroup)) as DayGroup;
            if (day != null)
            {
                BaseResult result = new BaseResult();
                ResultCodes code = ScheduleDataService.AddUpdateDayGroup(day);
                result.ResultCode = code;
                switch (code)
                {
                    case ResultCodes.Created:
                        result.ResultMessage = "Succesfully created";
                        context.Response.StatusCode = 200;
                        context.Response.Status = "200 OK";
                        break;
                    case ResultCodes.Updated:
                        result.ResultMessage = "Succesfully updated";
                        context.Response.StatusCode = 200;
                        context.Response.Status = "200 OK";
                        break;
                    default:
                        result.ResultMessage = "Already exists";
                        context.Response.StatusCode = 202;
                        context.Response.Status = "202 Accepted";
                        break;
                }
                Serializer.Serialize(result, context.Response.OutputStream);
            }
        }

        public override void DELETE(HttpContext context)
        {
            DayGroup day = Serializer.Deserialize(context.Request.InputStream, typeof(DayGroup)) as DayGroup;
            if (day != null)
            {
                BaseResult result = new BaseResult();
                ResultCodes code = ScheduleDataService.DeleteDayGroup(day.DayGroupId);
                result.ResultCode = code;
                switch (code)
                {
                    case ResultCodes.Deleted:
                        result.ResultMessage = "Succesfully deleted";
                        context.Response.StatusCode = 200;
                        context.Response.Status = "200 OK";
                        break;
                }
                Serializer.Serialize(result, context.Response.OutputStream);
            }
        }
    }
}