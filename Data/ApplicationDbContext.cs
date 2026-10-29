using CodeGhar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeGhar.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<CodeFile> CodeFiles => Set<CodeFile>();
    public DbSet<CodeVersion> CodeVersions => Set<CodeVersion>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<CodeFile>().HasIndex(x => new { x.Name, x.IsDeleted });
        b.Entity<CodeVersion>().HasIndex(x => new { x.CodeFileId, x.Number }).IsUnique();
        b.Entity<CodeFile>().HasMany(x => x.Versions).WithOne(x => x.CodeFile).HasForeignKey(x => x.CodeFileId).OnDelete(DeleteBehavior.Cascade);
    }
}
