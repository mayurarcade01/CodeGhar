using CodeGhar.Models;
namespace CodeGhar.Services;
public interface IDiffService { IReadOnlyList<DiffLine> Compare(string oldText,string newText); }
