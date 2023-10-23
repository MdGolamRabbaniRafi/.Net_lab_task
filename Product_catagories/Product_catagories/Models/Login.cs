using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Product_catagories.Models
{
    public class Login
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public string Password { get; set; }
    }
}