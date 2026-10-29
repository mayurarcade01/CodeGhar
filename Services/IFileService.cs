using CodeGhar.Models;

namespace CodeGhar.Services;
public interface IFileService
{
    Task<List<CodeFile>> ListAsync(string? search = null);
    Task<CodeFile?> GetAsync(Guid id);
    Task<CodeFile> CreateAsync(string name, string content, string? message = null);
    Task<CodeFile> UploadAsync(string name, Stream content, string? message = null);
    Task<CodeVersion?> SaveAsync(Guid id, string content, string? message = null, bool major = false);
    Task<bool> RenameAsync(Guid id, string name);
    Task<bool> DeleteAsync(Guid id);
    Task<CodeVersion?> RestoreAsync(Guid id, int number);
}
