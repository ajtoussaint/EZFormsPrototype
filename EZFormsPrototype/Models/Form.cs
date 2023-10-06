using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZFormsPrototype.Models
{
    public class Form
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FormID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Field> Fields { get; set; }

    }
}