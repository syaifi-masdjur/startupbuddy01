﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace startupbuddy01.Models
{
    public class ResponseModel
    {
        public string Messages { get; set; }
        public Guid id { get; set; }
        public bool isSuccess { get; set; }
    }
}