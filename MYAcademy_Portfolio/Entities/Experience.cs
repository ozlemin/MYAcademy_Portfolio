using System.ComponentModel.DataAnnotations;

namespace MYAcademy_Portfolio.Entities
{
    public class Experience
    {
        public int ExperienceId { get; set; }

        [Required(ErrorMessage = "Pozisyon / Ünvan zorunludur.")]
        [StringLength(150, ErrorMessage = "Pozisyon en fazla 150 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Şirket adı en fazla 150 karakter olabilir.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
        public string Description { get; set; }

        [StringLength(100)]
        public string SideTitle { get; set; }

        [Required(ErrorMessage = "Tarih aralığı zorunludur.")]
        [StringLength(100, ErrorMessage = "Süre en fazla 100 karakter olabilir.")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Çalışma tipi zorunludur.")]
        [StringLength(50)]
        public string WorkType { get; set; }

        public bool IsActive { get; set; }
    }
}
