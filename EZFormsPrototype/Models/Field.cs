using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZFormsPrototype.Models
{
    public class Field
    {
        public int FieldID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual Form ParentForm { get; set; }
    }
}