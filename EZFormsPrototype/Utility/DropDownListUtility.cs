using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EZFormsPrototype.Utility
{
    public static class DropDownListUtility
    {
        public static IEnumerable<SelectListItem> GetFieldTypeDropdown(string selectedValue)
        {
            //TODO: throw exception if the input is not a useful string?
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Number", Value = "number", Selected = ("number" == selectedValue)},
                new SelectListItem { Text = "Text", Value = "text", Selected = ("text" == selectedValue)},
                new SelectListItem { Text = "Date", Value = "date", Selected = ("date" == selectedValue)},
                new SelectListItem { Text = "Time", Value = "time", Selected = ("time" == selectedValue)},
                new SelectListItem { Text = "Date Time", Value = "dateTime", Selected = ("dateTime" == selectedValue)},
                new SelectListItem { Text = "Dollar Amount", Value = "dollarAmount", Selected = ("dollarAmount" == selectedValue)},
            };
        }
    }
}