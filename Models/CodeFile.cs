using System.ComponentModel.DataAnnotations;

namespace CodeGhar.Models;

public class CodeFile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "File name is required.")]
    [StringLength(180, MinimumLength = 1, ErrorMessage = "File name must be between 1 and 180 characters.")]
    [RegularExpression(@"^[^\\/:*?""<>|]+$", ErrorMessage = "File name contains invalid characters.")]
    [Display(Name = "File Name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Extension { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    public int CurrentVersionNumber { get; set; }

    public List<CodeVersion> Versions { get; set; } = [];
}
