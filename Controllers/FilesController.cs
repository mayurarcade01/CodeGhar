using CodeGhar.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeGhar.Controllers;

public class FilesController(IFileService files, IDiffService diff) : Controller
{
    // READ: List all files with optional search (State Management: Session stores last search query)
    public async Task<IActionResult> Index(string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            HttpContext.Session.SetString("LastSearchQuery", search);
        }
        return View(await files.ListAsync(search));
    }

    // READ: View / Edit file (State Management: Session stores last viewed file & increments visit count)
    public async Task<IActionResult> Edit(Guid id)
    {
        var f = await files.GetAsync(id);
        if (f is null) return NotFound();

        // State Management via Session
        HttpContext.Session.SetString("LastViewedFile", f.Name);
        var viewCount = (HttpContext.Session.GetInt32("FilesViewedCount") ?? 0) + 1;
        HttpContext.Session.SetInt32("FilesViewedCount", viewCount);

        return View(f);
    }

    // CREATE: Form handling with Model Validation & State Management
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string name, string content, string? message)
    {
        // Explicit Model Validation
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Error"] = "Validation Error: File name cannot be empty.";
            return RedirectToAction(nameof(Index));
        }

        if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            TempData["Error"] = "Validation Error: File name contains invalid characters.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            await files.CreateAsync(name.Trim(), content ?? string.Empty, message);

            // State Management: TempData notification + Session action tracker
            TempData["Notice"] = $"File '{name}' created successfully.";
            HttpContext.Session.SetString("LastAction", $"Created '{name}' at {DateTime.Now:HH:mm:ss}");

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Error"] = e.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // CREATE: File Upload with Form Handling and Validation
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile? file, string? message)
    {
        if (file is null || file.Length == 0)
        {
            TempData["Error"] = "Validation Error: Please select a valid file to upload.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            await files.UploadAsync(file.FileName, file.OpenReadStream(), message);
            TempData["Notice"] = $"{Path.GetFileName(file.FileName)} uploaded as v1.";
            HttpContext.Session.SetString("LastAction", $"Uploaded '{file.FileName}' at {DateTime.Now:HH:mm:ss}");
        }
        catch (Exception e)
        {
            TempData["Error"] = e.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // UPDATE: Save new version with Model Validation
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(Guid id, string content, string? message)
    {
        if (content is null)
        {
            TempData["Error"] = "Validation Error: Content cannot be null.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        try
        {
            var v = await files.SaveAsync(id, content, message);
            if (v is null)
            {
                TempData["Notice"] = "No changes detected to save.";
            }
            else
            {
                TempData["Notice"] = $"Version saved as {v.Label}.";
                HttpContext.Session.SetString("LastAction", $"Saved {v.Label} at {DateTime.Now:HH:mm:ss}");
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = e.Message;
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // UPDATE: Rename file with Validation
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Rename(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Error"] = "Validation Error: New file name cannot be empty.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var success = await files.RenameAsync(id, name.Trim());
        TempData["Notice"] = success ? "File renamed successfully." : "Rename failed.";
        if (success)
        {
            HttpContext.Session.SetString("LastViewedFile", name.Trim());
            HttpContext.Session.SetString("LastAction", $"Renamed to '{name}' at {DateTime.Now:HH:mm:ss}");
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // DELETE: Remove file
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await files.DeleteAsync(id);
        TempData["Notice"] = "File moved out of the workspace.";
        HttpContext.Session.SetString("LastAction", $"Deleted a file at {DateTime.Now:HH:mm:ss}");
        return RedirectToAction(nameof(Index));
    }

    // READ: Download raw file content
    public async Task<IActionResult> Download(Guid id)
    {
        var f = await files.GetAsync(id);
        if (f is null) return NotFound();
        var v = f.Versions.OrderByDescending(x => x.Number).First();
        return File(System.Text.Encoding.UTF8.GetBytes(v.Content), "text/plain", f.Name);
    }

    // READ: Version history & Diff inspection
    public async Task<IActionResult> History(Guid id, int? compare)
    {
        var f = await files.GetAsync(id);
        if (f is null) return NotFound();

        var versions = f.Versions.OrderByDescending(x => x.Number).ToList();
        var target = versions.First();
        var baseV = versions.Skip(1).FirstOrDefault();

        if (compare.HasValue)
        {
            target = versions.FirstOrDefault(x => x.Number == compare) ?? target;
        }

        baseV = versions.Where(x => x.Number < target.Number).OrderByDescending(x => x.Number).FirstOrDefault();

        ViewBag.Target = target;
        ViewBag.Base = baseV;
        ViewBag.Diff = diff.Compare(baseV?.Content ?? "", target.Content);

        // State Management
        HttpContext.Session.SetString("LastViewedFile", f.Name);

        return View(f);
    }

    // UPDATE / RESTORE: Restore previous version
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(Guid id, int number)
    {
        var v = await files.RestoreAsync(id, number);
        TempData["Notice"] = v is null
            ? "That version is already current or unavailable."
            : $"Restored as {v.Label}.";

        if (v is not null)
        {
            HttpContext.Session.SetString("LastAction", $"Restored {v.Label} at {DateTime.Now:HH:mm:ss}");
        }

        return RedirectToAction(nameof(History), new { id });
    }
}
