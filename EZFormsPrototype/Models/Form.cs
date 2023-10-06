using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.Models
{
    public class Form
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Field> Fields { get; set; }

    }
}