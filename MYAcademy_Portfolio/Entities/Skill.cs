using System.ComponentModel.DataAnnotations;

namespace MYAcademy_Portfolio.Entities
{
    public class Skill
    {
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Yetenek adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Yetenek adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Seviye zorunludur.")]
        [Range(1, 100, ErrorMessage = "Seviye 1 ile 100 arasında olmalıdır.")]
        public int Percentage { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public bool IsActive { get; set; }
    }
}
