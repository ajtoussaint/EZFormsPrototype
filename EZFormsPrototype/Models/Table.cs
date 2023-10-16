using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZFormsPrototype.Models
{
    public class Table : FormItem
    {
        public virtual ICollection<TableField> Fields { get; set; }
    }
}