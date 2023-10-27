using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EZFormsPrototype.Models;

namespace EZFormsPrototype.ViewModels
{
    public class FieldWithFlags
    {
        public Field Field { get; set; }
        public List<TableField> TableFields{ get; set; }
        public List<Flag> Flags { get; set; }
    }
}