namespace CodeGhar.Models;
public record DiffLine(string Kind, int OldLine, int NewLine, string Text);
