using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Models
{
    public class Theme
    {
        public string new_theme_PackURI { get; set; }
        public string old_theme_name { get; set; }
        public string base_dictionary_name { get; set; }
        public object sender { get; set; }
        public Theme() { }
    }
}
