using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EZFormsPrototype.Models
{
    public class Flag
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(99)]
        [RegularExpression("[a-zA-Z0-9-]+")]
        public string Name { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public int FieldID { get; set; }
        public int FormID { get; set; }
        public string userID { get; set; }
        public bool appearsOnSubmit { get; set; }
        public virtual List<int> DependantFieldIDs { get; set; }
        public virtual List<string> CodeExpressions { get; set; }

    }
}