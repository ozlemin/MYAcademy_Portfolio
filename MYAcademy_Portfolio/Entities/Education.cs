using System.ComponentModel.DataAnnotations;

namespace MYAcademy_Portfolio.Entities
{
    public class Education
    {
        public int EducationId { get; set; }

        [Required(ErrorMessage = "Okul / Kurum adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Bölüm / Program adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Alt başlık en fazla 150 karakter olabilir.")]
        public string SubTitle { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tarih aralığı zorunludur.")]
        [StringLength(100, ErrorMessage = "Süre en fazla 100 karakter olabilir.")]
        public string Duration { get; set; }

        public bool IsActive { get; set; }
    }
}
