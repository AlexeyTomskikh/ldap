namespace ldap.Models.ViewsFormModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EventModels
    {
        #region properties

        [Required]
        [Display(Name = "Название события")]
        public string NameEvent { get; set; }

        [Required]
        [Display(Name = "Начало:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartEvent { get; set; }

        [Required]
        [Display(Name = "Конец:")]
        [DataType(DataType.Text)]
        public DateTime EndEvent { get; set; }

        #endregion
    }
}