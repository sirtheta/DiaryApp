﻿using System.ComponentModel.DataAnnotations;

namespace DiaryApp
{
  public class UserDb
  {
    [Key]
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }

    public string FullName
    {
      get
      {
        return $"{FirstName} {LastName}";
      }
    }
  }
}
