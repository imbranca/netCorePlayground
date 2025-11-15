using System.ComponentModel.DataAnnotations;
using System.Composition;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

public class LoginUserDTO
{
  [Required]
  public string Email {get;set;}

  [Required]
  public string Password {get; set;}
}