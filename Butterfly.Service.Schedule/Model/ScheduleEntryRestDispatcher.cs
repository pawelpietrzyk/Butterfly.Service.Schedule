using butterfly.com.rest;

using butterfly.com.rest.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Butterfly.Service.Schedule.Model
{
    public class ScheduleEntryRestDispatcher : RestDispatcher
    {
        public override void DispatchRestMethod(RestMethod method)
        {
            throw new NotImplementedException();
        }

        public override void GET(HttpContext context)
        {            
            List<ScheduleEntry> entries = ScheduleDataService.GetEntries(DateTime.Now);
            Serializer.Serialize(entries, context.Response.OutputStream);
        }

        public override void POST(HttpContext context)
        {
            Entry entry = Serializer.Deserialize(context.Request.InputStream, typeof(Entry)) as Entry;
            if (entry != null)
            {
                BaseResult result = new BaseResult();
                ResultCodes code = ScheduleDataService.AddScheduleEntry(entry);
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
                        result.ResultMessage = "already exists";
                        context.Response.StatusCode = 202;
                        context.Response.Status = "202 Accepted";
                        break;
                }
                Serializer.Serialize(result, context.Response.OutputStream);
            }
        }

        public override void DELETE(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}