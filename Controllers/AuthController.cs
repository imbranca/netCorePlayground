
using System.Security.Claims;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiScratch.API.Models;

namespace MyApp.Namespace
{

  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    private readonly TokenService _tokenService;


    public AuthController(AppDbContext productsContext,
     TokenService tokenService,
     UserManager<User> userManager,
     SignInManager<User> signInManager)
    {
      _context = productsContext;
      _userManager = userManager;
      _signInManager = signInManager;
      _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser)
    {
      //Validate DTO type
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      //Call DB and return user
      // var tempUser = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.RoleId).First(x=>x.Email == loginUser.Email);
      // if(tempUser == null) { return NotFound(); }
      // var response = new UserDTO
      // {
      //     Email = tempUser.Email,
      //     Name = tempUser.UserName,
      //     Role = tempUser.
      //     // add more fields as needed
      // };
      // return Ok(tempUser);
      var user = await _userManager.FindByEmailAsync(loginUser.Email);
      // var loggeds = _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);
      // var logged = await _signInManager.PasswordSignInAsync(user.UserName, loginUser.Password, false, false);

      if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
      {
        // var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.CreateToken(user);

        return Ok(new
        {
          id = user.Id,
          email = user.Email,
          // roles = roles,
          token = accessToken
        });
      }
      else
      {
        return NotFound();
      }
    }


    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List()
    {
      try
      {
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(400, e.Message);
      }
    }

    [HttpGet("profile")]
    [Authorize] // Ensures only authenticated users can access
    public IActionResult GetUserProfile()
    {
      // Get the user's ID
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      // Get the user's email
      var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

      // Get a custom claim (if present in your token)
      // var customClaimValue = User.FindFirst("your_custom_claim_type")?.Value;

      if (userId == null)
      {
        return Unauthorized("User ID not found in token.");
      }

      // You can now use userId or userEmail to fetch more detailed user data from your database
      // e.g., var user = _userService.GetUserById(userId);

      return Ok(new {  Email = userEmail, UserId = userId });
    }

    // [AllowAnonymous]
    // [HttpGet("succsess")]
    // public async Task<IActionResult> Success(HttpContext context)
    // {
    //   var userTwo = context.User;
    //   Console.WriteLine("email ", userTwo);

    //   //   if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
    //   //       return Unauthorized("Not authenticated.");
    //         var result = await HttpContext.AuthenticateAsync("External");

    //         if (!result.Succeeded || result.Principal == null)
    //             return Unauthorized("External authentication failed");

    //         var user = result.Principal;

    //     // ✔️ Read basic profile
    //     var email = user.FindFirst(ClaimTypes.Email)?.Value;
    //     var name = user.FindFirst(ClaimTypes.Name)?.Value;
    //     var picture = user.FindFirst("picture")?.Value;
    //   Console.WriteLine("email ", result.Principal);
    //   return Ok();
    // }

    [AllowAnonymous]
    [HttpGet("success")]
    public async Task<IActionResult> GoogleLoginSuccess()
    {
        // Read the identity from the EXTERNAL cookie
        // var result = await HttpContext.AuthenticateAsync("External");
        var result = await HttpContext.AuthenticateAsync("External");

        if (!result.Succeeded || result.Principal == null)
            return Unauthorized("External login failed");

        var user = result.Principal;

        // Read Google claims
        var email = user.FindFirst(ClaimTypes.Email)?.Value;
        var name = user.FindFirst(ClaimTypes.Name)?.Value;
        var googleId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var picture = user.FindFirst("picture")?.Value;

        // Read tokens (Google access + id_token)
        var accessToken = result.Properties.GetTokenValue("access_token");
        var idToken = result.Properties.GetTokenValue("id_token");
        var refreshToken = result.Properties.GetTokenValue("refresh_token");

        // Debug log
        Console.WriteLine("GOOGLE EMAIL: " + email);
        Console.WriteLine("GOOGLE NAME: " + name);

        // TODO: Save user to DB or generate JWT here

        return Ok(new
        {
            Email = email,
            Name = name,
            GoogleId = googleId,
            Picture = picture,
            AccessToken = accessToken,
            IdToken = idToken,
            RefreshToken = refreshToken
        });
    }
  }

}