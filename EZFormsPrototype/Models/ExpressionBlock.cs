using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.Models
{
    public class ExpressionBlock
    {
        public int ID { get; set; }
        public int FlagID { get; set; }
        public int Order {  get; set; }
        public int DependantFieldID1 { get; set; }
        public int DependantFieldID2 { get; set; }
        //For the fillable view it is more useful to simply have the operator than the entire expression
        //this may change when tabel functionality is added
        public string CodeExpression { get; set; }
        public string ViewExpression { get; set; }
    }
}