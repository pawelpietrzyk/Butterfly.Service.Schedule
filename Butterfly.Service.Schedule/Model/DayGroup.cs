using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Butterfly.Service.Schedule.Model
{
    [DataContract]
    public class DayGroup
    {
        [DataMember]
        public string DayGroupId { get; set; }
        [DataMember]
        public string Description { get; set; }

    }
}