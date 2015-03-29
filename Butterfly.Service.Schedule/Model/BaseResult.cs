﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Butterfly.Service.Schedule.Model
{
    [DataContract]
    public class BaseResult
    {
        [DataMember]
        public ResultCodes ResultCode { get; set; }
        [DataMember]
        public string ResultMessage { get; set; }
    }
}