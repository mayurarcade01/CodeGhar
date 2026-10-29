using CodeGhar.Data;
using CodeGhar.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeGhar.Services;
public class FileService(ApplicationDbContext db) : IFileService
{
    public async Task<List<CodeFile>> ListAsync(string? search = null) => await db.CodeFiles.AsNoTracking().Include(x => x.Versions).Where(x => !x.IsDeleted && (string.IsNullOrWhiteSpace(search) || x.Name.Contains(search))).OrderByDescending(x => x.UpdatedUtc).ToListAsync();
    public Task<CodeFile?> GetAsync(Guid id) => db.CodeFiles.Include(x => x.Versions).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    public async Task<CodeFile> CreateAsync(string name, string content, string? message = null)
    {
        Validate(name, content);
        if (await db.CodeFiles.AnyAsync(x => !x.IsDeleted && x.Name == name)) throw new InvalidOperationException("A file with that name already exists.");
        var file = new CodeFile { Name = name, Extension = Extension(name), CurrentVersionNumber = 1 };
        file.Versions.Add(new CodeVersion { CodeFileId = file.Id, Number = 1, Major = 1, Minor = 0, Content = content, Message = message ?? "Initial version" });
        db.Add(file); await db.SaveChangesAsync(); return file;
    }
    public async Task<CodeFile> UploadAsync(string name, Stream content, string? message = null)
    {
        if (content.Length > 2_000_000) throw new InvalidOperationException("Files must be 2 MB or smaller.");
        using var reader = new StreamReader(content, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: false);
        var text = await reader.ReadToEndAsync();
        return await CreateAsync(Path.GetFileName(name), text, message ?? "Uploaded from local storage");
    }
    public async Task<CodeVersion?> SaveAsync(Guid id, string content, string? message = null, bool major = false)
    {
        ValidateContent(content); var file = await GetAsync(id) ?? throw new KeyNotFoundException();
        var current = file.Versions.OrderByDescending(x => x.Number).First();
        if (current.Content == content) return null;
        var version = new CodeVersion { CodeFileId = id, Number = current.Number + 1, Major = major ? current.Major + 1 : current.Major, Minor = major ? 0 : current.Minor + 1, Content = content, Message = message };
        file.CurrentVersionNumber = version.Number; file.UpdatedUtc = DateTime.UtcNow; db.Add(version); await db.SaveChangesAsync(); return version;
    }
    public async Task<bool> RenameAsync(Guid id, string name) { ValidateName(name); var f = await db.CodeFiles.FindAsync(id); if (f is null || f.IsDeleted || await db.CodeFiles.AnyAsync(x => x.Id != id && !x.IsDeleted && x.Name == name)) return false; f.Name=name; f.Extension=Extension(name); f.UpdatedUtc=DateTime.UtcNow; await db.SaveChangesAsync(); return true; }
    public async Task<bool> DeleteAsync(Guid id) { var f=await db.CodeFiles.FindAsync(id); if(f is null)return false; f.IsDeleted=true; await db.SaveChangesAsync(); return true; }
    public async Task<CodeVersion?> RestoreAsync(Guid id,int number) { var f=await GetAsync(id); var v=f?.Versions.FirstOrDefault(x=>x.Number==number); return v is null ? null : await SaveAsync(id,v.Content,$"Restored {v.Label}", major: true); }
    static void Validate(string name,string content){ValidateName(name);ValidateContent(content);}
    static void ValidateName(string name){ if(string.IsNullOrWhiteSpace(name)||name.Contains('/')||name.Contains('\\')||name.Contains("..")||name.Length>180||!Allowed(name)) throw new InvalidOperationException("Use a supported, safe source filename."); }
    static void ValidateContent(string content){ if(content.Length>2_000_000) throw new InvalidOperationException("Files must be 2 MB or smaller."); if(content.Contains('\0')) throw new InvalidOperationException("Binary files are not supported."); }
    static bool Allowed(string n){var e=Extension(n); return new[]{".txt",".md",".markdown",".html",".htm",".css",".js",".ts",".jsx",".tsx",".json",".xml",".yaml",".yml",".cs",".java",".py",".c",".h",".cpp",".hpp",".sql",".php",".go",".rs",".sh",".bat",".ps1",".vue",".svelte",".gitignore",".editorconfig"}.Contains(e,StringComparer.OrdinalIgnoreCase);}
    static string Extension(string n){ if(n.Equals(".gitignore",StringComparison.OrdinalIgnoreCase))return ".gitignore"; if(n.Equals(".editorconfig",StringComparison.OrdinalIgnoreCase))return ".editorconfig"; return Path.GetExtension(n).ToLowerInvariant(); }
}
