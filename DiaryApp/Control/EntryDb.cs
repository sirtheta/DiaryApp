using System.ComponentModel.DataAnnotations;

namespace DiaryApp
{
  public class DiaryEntryDb
  {
    [Key]
    public int EntryId { get; set; }
    public string Text { get; set; }
    public string Tag { get; set; }
    public string Date { get; set; }

    //public int UserId { get; set; }
    //public virtual User User { get; set; }
  }

  public class TagDb
  {
    [Key]
    public int TagID { get; set; }
    public string TagText { get; set; }
  }
}
