using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Model
{
  public class UserDb
  {
    [Key]
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
  }
}
