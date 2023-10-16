using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EZFormsPrototype.Models
{
    public class FormItem
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int FormID { get; set; }
        [Range(0, int.MaxValue)]
        public int FormOrder {  get; set; }
    }
}