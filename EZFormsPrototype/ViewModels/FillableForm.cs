using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EZFormsPrototype.Models;

namespace EZFormsPrototype.ViewModels
{
    public class FillableForm
    {
        public Form Form { get; set; }
        public List<FormField> Fields { get; set; }
        public List<TableField> TableFields { get; set; }
        public List<Flag> Flags { get; set; }
    }
}