using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.ViewModels
{
    public class Result
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public List<string> Flags { get; set; }
    }
    public class FinalResult
    {
        

        public List<Result> Results { get; set; }
        public String FormTitle { get; set; }
        public String FormDescription { get; set; }

        //TODO: add flags parameter
        public void addResult(string Title, string Value)
        {
            Result res = new Result { Title = Title, Value = Value};
            Results.Add(res);
        }

        public FinalResult()
        {
            Results = new List<Result>();
        }
        
    }
}