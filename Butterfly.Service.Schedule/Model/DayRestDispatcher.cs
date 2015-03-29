using butterfly.com.rest;
using butterfly.com.rest.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Butterfly.Service.Schedule.Model
{
    public class DayRestDispatcher : RestDispatcher
    {

        public override void DispatchRestMethod(RestMethod method)
        {
            throw new NotImplementedException();
        }

        public override void GET(System.Web.HttpContext context)
        {
            List<Day> days = ScheduleDataService.GetDays(String.Empty);
            if (days != null)
            {
                this.ResultOK(context);
                Serializer.Serialize(days, context.Response.OutputStream);
            }            
        }

        public override void POST(System.Web.HttpContext context)
        {
            Day day = Serializer.Deserialize(context.Request.InputStream, typeof(Day)) as Day;
            if (day != null)
            {
                BaseResult result = new BaseResult();
                ResultCodes code = ScheduleDataService.AddUpdateDay(day) ;
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
                        result.ResultMessage = "DayId already exists";
                        context.Response.StatusCode = 202;
                        context.Response.Status = "202 Accepted";
                        break;
                }                
                Serializer.Serialize(result, context.Response.OutputStream);
            }
        }

        public override void DELETE(System.Web.HttpContext context)
        {
            Day day = Serializer.Deserialize(context.Request.InputStream, typeof(Day)) as Day;
            if (day != null)
            {
                BaseResult result = new BaseResult();
                ResultCodes code = ScheduleDataService.DeleteDay(day.DayId);
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
