using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Butterfly.Service.Schedule.Model
{
    [DataContract]
    public class Entry
    {
        [DataMember]
        public string ParamId { get; set; }
        [DataMember]
        public int Hour { get; set; }
        [DataMember]
        public int Minute { get; set; }
    }
}