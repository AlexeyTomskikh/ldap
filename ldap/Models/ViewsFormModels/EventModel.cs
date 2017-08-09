namespace ldap.Models.ViewsFormModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    public class EventModel
    {
        #region properties

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Начало:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        public bool AllDay { get; set; }

        [Required]
        [Display(Name = "Конец:")]
        [DataType(DataType.Text)]
        public DateTime EndTime { get; set; }


        public string Description { get; set; }

        public string Color { get; set; }

        public int RoomId { get; set; }

        public int[] MemberList { get; set; }
    
        #endregion
    }
}