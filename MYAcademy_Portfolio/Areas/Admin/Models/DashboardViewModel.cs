namespace MYAcademy_Portfolio.Areas.Admin.Models;

public class DashboardViewModel
{
    // Sayısal İstatistikler
    public int TotalProjectCount { get; set; }
    public int ActiveProjectCount { get; set; }
    public int TotalSkillCount { get; set; }
    public int TotalMessageCount { get; set; }
    public int UnreadMessageCount { get; set; }
    public int ReadMessageCount { get; set; }
    public int CategoryCount { get; set; }
    public int ExperienceCount { get; set; }
    public int EducationCount { get; set; }

    // Listeler
    public List<MYAcademy_Portfolio.Entities.Message> LatestMessages { get; set; } = new();
    public List<MYAcademy_Portfolio.Entities.Project> LatestProjects { get; set; } = new();
    public Dictionary<string, int> CategoryProjectCounts { get; set; } = new();
}
