using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.Utility
{
    //deprecated but may be useful later
    public class FlagExpression
    {
        private Dictionary<string, string> comparisonFunctions = new Dictionary<string, string>()
        {
            {">","greaterThan"},
            {"<","lessThan"},
            {">=","greaterThanOrEqualTo" },
            {"<=","lessThanOrEqualTo" },
            {"=", "equalTo" },
            {"!=","notEqualTo" }
        };
        public String formatExpression(String exp) {
            foreach (var key in comparisonFunctions.Keys)
            {
                
            }
            return "";
        } 
    }
}