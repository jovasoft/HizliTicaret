using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Mail adresi bo� b�rak�lamaz.")]
        [EmailAddress(ErrorMessage = "L�tfen ge�erli bir mail adresi giriniz.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "�ifre bo� b�rak�lamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}