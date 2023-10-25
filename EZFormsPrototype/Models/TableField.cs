using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZFormsPrototype.Models
{
    public class TableField : Field
    {
        public int TableID {  get; set; }

    }
}