using System.ComponentModel.DataAnnotations;
using System.Composition;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

public class UserDTO
{
  public string Name {get;set;}
  public string Email {get;set;}

  public string Role {get;set;}
}