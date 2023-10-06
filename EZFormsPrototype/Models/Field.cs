using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.Models
{
    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual Form ParentForm { get; set; }
    }
}