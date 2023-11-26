using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.ViewModels
{
    public class FieldEditViewModel
    {
        public int ID { get; set; }
        public int FormID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int FormOrder { get; set; }
        public virtual List<string> TableFieldNames { get; set; }
        public virtual List<string> TableFieldTypes { get; set; }
        public string Redirect {  get; set; }
    }
}