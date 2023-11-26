using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZFormsPrototype.ViewModels
{
    public class FormEditViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Redirect {  get; set; }
    }
}