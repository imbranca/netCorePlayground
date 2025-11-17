using Microsoft.AspNetCore.Authorization;

public class Permission : AuthorizeAttribute
{
    public Permission(string role)
    {
        Policy = $"Role:{role}";
    }
}