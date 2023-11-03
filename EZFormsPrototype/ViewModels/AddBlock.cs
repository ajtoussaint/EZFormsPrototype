using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.ViewModels
{
    public class AddBlock
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string TriggerExpression { get; set; }
        public string Level { get; set; }
        public int FieldID { get; set; }
        public int FormID { get; set; }
        public int Order { get; set; }
        public int DependantFieldID1 { get; set; }
        public int DependantFieldID2 { get; set; }
        public string CodeExpression { get; set; }
        public string ViewExpression { get; set; }
    }
}