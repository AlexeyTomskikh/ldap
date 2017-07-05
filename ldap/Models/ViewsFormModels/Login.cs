namespace ldap.Models.ViewsFormModels
{
    using System.ComponentModel.DataAnnotations;

    public class Login
    {
        #region properties

        [Required]
        [Display(Name = "Логин:")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }

        #endregion
    }
}