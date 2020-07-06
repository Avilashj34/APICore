using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APICoreExample.Model
{
    public class Customer:BaseEntity
    {
        //The Id is inherited form the BaseEntity
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage ="Age is required")]
        public int Age { get; set; }
        // one to many relation Customer => WorkInformation
        public IList<WorkInformation> WorkInformation { get; set; }
    }
}
