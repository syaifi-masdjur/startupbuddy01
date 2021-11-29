using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace startupbuddy01.Data
{
    public class AdminUserModel
    {
        public int id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is Mandatory")]
        public string name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Display(Name = "Profile")]
        public string photo { get; set; }
        [Display(Name = "Gender")]
        public string  gender { get; set; }
        [Display(Name = "Age")]
        [Range(20,80,ErrorMessage ="Invalid Age")]
        public int age { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Position")]
        public string position { get; set; }
    }
}