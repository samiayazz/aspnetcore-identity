using AspNetIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspNetIdentity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    UserManager<AppUser> _userManager;
    SignInManager<AppUser> _signInManager;
    RoleManager<AppRole> _roleManager;
    IConfiguration _config;
    public IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _config = config;

        if (_roleManager.Roles.Count() == 0)
        {
            _roleManager.CreateAsync(new AppRole
            {
                Name = "Admin"
            });
            _roleManager.CreateAsync(new AppRole
            {
                Name = "User"
            });
        }
    }

    [HttpPost("/[action]", Name = "Register")]
    public IActionResult Register(AppUser user, string password, string roleName)
    {
        // Create user
        var userResult = _userManager.CreateAsync(user, password).Result;
        if (userResult.Succeeded)
            // Add role to user
            _userManager.AddToRoleAsync(user, roleName).Wait();

        return Ok(new
        {
            Status = 200,
            Result = $"A new user has been successfully created with the username \"{user.UserName}\""
        });
    }

    [HttpGet("/[action]", Name = "login")]
    public IActionResult Login(string username, string password)
    {
        var user = _userManager.FindByNameAsync(username).Result;
        if (user == null)
            return NotFound(new
            {
                Status = 404,
                Result = "The user not found!"
            });

        var signInResult = _signInManager.CheckPasswordSignInAsync(user, password, false).Result;
        if (!signInResult.Succeeded)
            return BadRequest(new
            {
                Status = 400,
                Result = "The password isn't correct!"
            });

        string userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, userRole)
        };

        string token = CreateJwtToken(claims);

        return Ok(new
        {
            Status = 200,
            Result = token
        });
    }

    private string CreateJwtToken(List<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:SecurityKey"]));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var lifeTime = DateTime.Now.AddMinutes(Convert.ToInt32(_config["JwtToken:LifeTimeMinute"]));

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _config["JwtToken:Issuer"],
            audience: _config["JwtToken:Audience"],
            signingCredentials: signingCredentials,
            expires: lifeTime);

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        return jwtSecurityTokenHandler.WriteToken(token);
    }
}
