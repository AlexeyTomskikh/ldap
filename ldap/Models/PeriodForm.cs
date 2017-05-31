using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ldap.Models
{
    public class PeriodForm
    {
        #region properties

        [Required]
        [Display(Name = "От:")]
        public DateTime StartEvent { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "До:")]
        public DateTime EndEvent { get; set; }

        #endregion
    }
}