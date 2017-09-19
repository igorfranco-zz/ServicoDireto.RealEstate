using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SpongeSolutions.Core.Model
{
    public class CustomSelectListItem : System.Web.Mvc.SelectListItem
    {
        public MultiSelectList Children { get; set; }       
    }
}
