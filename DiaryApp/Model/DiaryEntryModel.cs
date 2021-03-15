using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

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
        StringBuilder sb = new StringBuilder();
        if (TagFamily)
        {
          sb.Append("Family");
        }
        if (TagFriends)
        {
          if (sb.Length != 0)
          {
            sb.Append(", ");
          }
          sb.Append("Friends");
        }
        if (TagBirthday)
        {
          if (sb.Length != 0)
          {
            sb.Append(", ");
          }
          sb.Append("Birthday");
        }
        return sb.ToString();
      }
    }
  }
}
