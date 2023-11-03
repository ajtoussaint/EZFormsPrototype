using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.ViewModels
{
    public class RemoveBlock
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string TriggerExpression { get; set; }
        public string Level { get; set; }
        public int FieldID { get; set; }
        public int FormID { get; set; }

        public int BlockToRemove {  get; set; }
    }
}