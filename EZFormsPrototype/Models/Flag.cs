﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EZFormsPrototype.Models
{
    public class Flag
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(99)]
        [RegularExpression("[a-zA-Z0-9-]+")]
        public string Name { get; set; }
        public string Message { get; set; }
        //trigger expression is deprecated
        public string TriggerExpression { get; set; }
        public string Level { get; set; }
        public int FieldID { get; set; }
        public int FormID { get; set; }

        public List<ExpressionBlock> ExpressionBlocks { get; set; }

    }
}