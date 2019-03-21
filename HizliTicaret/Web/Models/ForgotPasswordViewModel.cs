using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Mail adresi bo� b�rak�lamaz.")]
        [EmailAddress(ErrorMessage = "L�tfen ge�erli bir mail adresi giriniz.")]
        public string Mail { get; set; }
    }
}