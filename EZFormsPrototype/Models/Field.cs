using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZFormsPrototype.Models
{
    public class Field { 
        [Key]
        public int ID { get; set; }
        [MaxLength(99)]
        [RegularExpression("[a-zA-Z0-9-]+")]
        public string Name { get; set; }
        public int FormID { get; set; }
        [Range(0, int.MaxValue)]
        public int FormOrder { get; set; }
        public string Type { get; set; }

        public virtual List<string> TableFieldNames {  get; set; }
        public virtual List<string> TableFieldTypes { get; set; }

        public virtual List<int> DependentFlagIDs { get; set; }
    }
}