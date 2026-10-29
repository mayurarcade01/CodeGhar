using CodeGhar.Models;
namespace CodeGhar.Services;
public class DiffService : IDiffService
{
 public IReadOnlyList<DiffLine> Compare(string oldText,string newText){var a=Lines(oldText);var b=Lines(newText);var l=new int[a.Length+1,b.Length+1];for(int i=a.Length-1;i>=0;i--)for(int j=b.Length-1;j>=0;j--)l[i,j]=a[i]==b[j]?l[i+1,j+1]+1:Math.Max(l[i+1,j],l[i,j+1]);var d=new List<DiffLine>();int x=0,y=0;while(x<a.Length||y<b.Length){if(x<a.Length&&y<b.Length&&a[x]==b[y]){d.Add(new("same",x+1,y+1,a[x++]));y++;}else if(y<b.Length&&(x==a.Length||l[x,y+1]>=l[x+1,y]))d.Add(new("added",0,++y,b[y-1]));else d.Add(new("removed",++x,0,a[x-1]));}return d;}
 static string[] Lines(string value)=>value.Replace("\r\n","\n").Replace('\r','\n').Split('\n');
}
