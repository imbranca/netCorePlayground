
using Microsoft.AspNetCore.Identity;

public interface IAccountService
{
  Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
}