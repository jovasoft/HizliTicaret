using System.ComponentModel.DataAnnotations;

namespace Web
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "�sim bo� b�rak�lamaz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mail adresi bo� b�rak�lamaz.")]
        [EmailAddress(ErrorMessage = "L�tfen ge�erli bir mail adresi giriniz.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "�ifre bo� b�rak�lamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "�ifre Tekrar� bo� b�rak�lamaz.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }        
    }
}