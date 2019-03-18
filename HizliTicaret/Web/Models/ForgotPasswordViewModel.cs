using System.ComponentModel.DataAnnotations;

namespace Web
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Mail adresi boþ býrakýlamaz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir mail adresi giriniz.")]
        public string Mail { get; set; }
    }
}