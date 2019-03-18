using System.ComponentModel.DataAnnotations;

namespace Web
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ýsim boþ býrakýlamaz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mail adresi boþ býrakýlamaz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir mail adresi giriniz.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Þifre boþ býrakýlamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Þifre Tekrarý boþ býrakýlamaz.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }        
    }
}