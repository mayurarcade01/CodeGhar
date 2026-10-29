using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGhar.Models;

public class CodeVersion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CodeFileId { get; set; }

    public CodeFile? CodeFile { get; set; }

    public int Number { get; set; }

    public int Major { get; set; }

    public int Minor { get; set; }

    [NotMapped]
    public string Label => Minor == 0 ? $"v{Major}" : $"v{Major}.{Minor}";

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    [MaxLength(160, ErrorMessage = "Commit message cannot exceed 160 characters.")]
    [Display(Name = "Commit Message")]
    public string? Message { get; set; }

    [Required(ErrorMessage = "File content cannot be empty.")]
    public string Content { get; set; } = string.Empty;

    public int AddedLines { get; set; }

    public int DeletedLines { get; set; }
}
