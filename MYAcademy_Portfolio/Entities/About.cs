using System.ComponentModel.DataAnnotations;

namespace MYAcademy_Portfolio.Entities
{
    public class About
    {
        public int AboutId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Unvan zorunludur.")]
        [StringLength(100, ErrorMessage = "Unvan en fazla 100 karakter olabilir.")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Konum zorunludur.")]
        [StringLength(100, ErrorMessage = "Konum en fazla 100 karakter olabilir.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [StringLength(150, ErrorMessage = "Uzmanlık en fazla 150 karakter olabilir.")]
        public string Expert { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        [StringLength(150, ErrorMessage = "E-posta en fazla 150 karakter olabilir.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
        public string Description { get; set; }

        [StringLength(50)]
        public string OpenToWork { get; set; }
    }
}
