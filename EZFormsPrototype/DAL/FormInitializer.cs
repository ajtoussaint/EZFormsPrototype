using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ContosoUniversity.Models;

namespace EZFormsPrototype.DAL
{
    public class FormInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<FormContext>
    {
        protected override void Seed(FormContext context)
        {
            context.SaveChanges();
        }
    }
}