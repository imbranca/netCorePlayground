
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiScratch.API.Models;

namespace MyApp.Namespace
{

  [Route("api/[controller]")]
  [ApiController]
  public class AuthController: ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;


    public AuthController(AppDbContext productsContext, UserManager<User> userManager, SignInManager<User> signInManager)
    {
      _context = productsContext;
      _userManager = userManager;
      _signInManager = signInManager;
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
    
      if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password)){
        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new
        {
          id = user.Id,
          email = user.Email,
          roles = roles
        });
      }
      else
      {
          return NotFound();
      }
    }


    [HttpGet]
    public async Task<IActionResult> List()
    {
      try
      {
        return StatusCode(400, new {error = "Error"});
      }
      catch (Exception e)
      {
        return StatusCode(400, e.Message);
      }
    }
  }
}