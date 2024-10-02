
using System.ComponentModel.DataAnnotations;


namespace BlogApps.Models
{
    public class RegisterViewvModel
    {
        
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }
        
        [Required]
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} alanı en az {2} en fazla {1} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

         [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parolanız eşleşmiyor.")]
        [Display(Name = "Parola Tekrarı")]
        public string? Password2 { get; set; }
    }
}