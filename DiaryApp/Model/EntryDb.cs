using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace DiaryApp.Model
{
  public class DiaryEntryDb
  {
    [Key]
    public int EntryId { get; set; }
    public string Text { get; set; }
    public bool TagFamily { get; set; }
    public bool TagFriends { get; set; }
    public bool TagBirthday { get; set; }
    public string Date { get; set; }
    public byte[] ByteImage { get; set; }

    public string TagText
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        if (TagFamily == true)
        {
          sb.Append("Family");
        }
        if (TagFriends == true)
        {
          if (sb.Length != 0)
          {
            sb.Append(", ");
          }
          sb.Append("Friends");
        }
        if (TagBirthday == true)
        {
          if (sb.Length != 0)
          {
            sb.Append(", ");
          }
          sb.Append("Birthday");
        }
        return Regex.Replace(sb.ToString(), "[^A-Za-z0-9, ]", "");        
      }
    }

    //public int UserId { get; set; }
    //public virtual User User { get; set; }
  }
}
