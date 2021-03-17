using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Linq;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace DiaryApp
{
  internal class DiaryEntryModel
  {
    [Key]
    public int EntryId { get; set; }
    public string Text { get; set; }
    public bool TagFamily { get; set; }
    public bool TagFriends { get; set; }
    public bool TagBirthday { get; set; }
    public DateTime Date { get; set; }
    public byte[] ByteImage { get; set; }

    //Add foreign key from UserDb to Entry
    public int UserId { get; set; }

    //generate the tagtext according to selected bool
    public string TagText
    {
      get
      {
        string[] vs = new string[] { TagFamily ? "Family" : null, TagFriends ? "Friends" : null, TagBirthday ? "Birthday" : null };
        vs = vs.Where(s => !string.IsNullOrEmpty(s)).ToArray();
        return vs.Length > 0 ? vs.Aggregate((a, b) => $"{a}, {b}") : "";
      }
    }
  }
}
