using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZFormsPrototype.Models
{
    public class Form
    {
        [Key]
        public int ID { get; set; }
        [RegularExpression("[a-zA-Z0-9-]+")]
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<FormField> Fields { get; set; }

    }
}