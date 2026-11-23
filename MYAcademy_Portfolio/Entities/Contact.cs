using System.ComponentModel.DataAnnotations;

namespace MYAcademy_Portfolio.Entities
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(300)]
        public string Linkedin { get; set; }

        [Required(ErrorMessage = "Konum zorunludur.")]
        [StringLength(150)]
        public string Location { get; set; }

        [StringLength(300)]
        public string Github { get; set; }

        [StringLength(500)]
        public string SubDescription { get; set; }
    }
}
