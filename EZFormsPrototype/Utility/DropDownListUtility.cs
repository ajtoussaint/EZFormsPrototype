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
                //new SelectListItem { Text = "Date", Value = "date", Selected = ("date" == selectedValue)},
                //new SelectListItem { Text = "Time", Value = "time", Selected = ("time" == selectedValue)},
                //new SelectListItem { Text = "Date Time", Value = "dateTime", Selected = ("dateTime" == selectedValue)},
                //new SelectListItem { Text = "Dollar Amount", Value = "dollarAmount", Selected = ("dollarAmount" == selectedValue)},
                new SelectListItem { Text = "Table", Value = "table", Selected = ("table" == selectedValue)},
            };
        }
        public static IEnumerable<SelectListItem> GetFlagLevelDropdown(string selectedValue)
        {
            return new List<SelectListItem>
            {
            new SelectListItem { Text = "Warning", Value = "warning", Selected = ("warning" == selectedValue)},
            new SelectListItem { Text = "Danger", Value = "danger", Selected = ("danger" == selectedValue) },
            };
        }

        public static IEnumerable<SelectListItem> GetFlagTriggerDropdown(bool selectedValue)
        {
            return new List<SelectListItem>
            {
            new SelectListItem { Text = "Flag displays as soon as user input data meets the condition", Value = "false", Selected = (!selectedValue) },
            new SelectListItem { Text = "Flag only displays if condition is met when user attempts to submit form.", Value = "true", Selected = (selectedValue)},
            };
        }
    }
}