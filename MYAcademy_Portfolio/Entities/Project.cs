using System.ComponentModel.DataAnnotations;

namespace MYAcademy_Portfolio.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Proje adı en fazla 150 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
        public string Description { get; set; }

        [StringLength(300)]
        public string? GitHubUrl { get; set; }

        public string? CoverImageUrl { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
