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
        public List<FlagResult> FlagResults { get; set; }
    }

    public class FlagResult
    {
        public string FlagTitle { get; set; }
        public string FlagResponse { get; set; }
    }

    public class TableResult : Result
    {
        public List<string> TableFields { get; set; }
        public List<List<string>> FieldValues { get; set; }
    }

    public class FinalResult
    {
        

        public List<Result> Results { get; set; }
        public String FormTitle { get; set; }
        public String FormDescription { get; set; }

        //TODO: add flags parameter
        public void addResult(Result res)
        {
            Results.Add(res);
        }

        public FinalResult()
        {
            Results = new List<Result>();
        }
        
    }
}