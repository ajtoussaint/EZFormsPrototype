﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.Models
{
    public class FormField : Field
    {
        public virtual List<TableField> TableFields { get; set; }
    }
}