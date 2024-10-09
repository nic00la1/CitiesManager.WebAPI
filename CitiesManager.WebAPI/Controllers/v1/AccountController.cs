using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CitiesManager.WebAPI.Controllers.v1;

/// <summary>
/// 
/// </summary>
[AllowAnonymous]
[ApiVersion("1.0")]
public class AccountController : CustomControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtService _jwtService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="roleManager"></param>
    public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             RoleManager<ApplicationRole> roleManager,
                             IJwtService jwtService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<ApplicationUser>> PostRegister(
        RegisterDTO registerDto
    )
    {
        // Validation
        if (ModelState.IsValid == false)
        {
            string errorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return Problem(errorMessage);
        }

        // Create User
        ApplicationUser user = new()
        {
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            UserName = registerDto.Email,
            PersonName = registerDto.PersonName
        };

        IdentityResult result =
            await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            // Sign-in 
            await _signInManager.SignInAsync(user, false);

            AuthenticationResponse authenticationResponse =
                _jwtService.GenerateJwtToken(user);

            return Ok(authenticationResponse);
        } else
        {
            string errorMessage = string.Join(" | ", result.Errors
                .Select(e => e.Description)); // error1 | error2 | error3

            return Problem(errorMessage);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);

        if (user == null) return Ok(true);

        return Ok($"Email {email} is already in use");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<ApplicationUser>> PostLogin(
        LoginDTO loginDto
    )
    {
        // Validation
        if (ModelState.IsValid == false)
        {
            string errorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return Problem(errorMessage);
        }

        SignInResult result = await _signInManager.PasswordSignInAsync(
            loginDto.Email,
            loginDto.Password, false, false);

        if (result.Succeeded)
        {
            ApplicationUser? user =
                await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return NotFound();


            // Sign-in 
            await _signInManager.SignInAsync(user, false);

            AuthenticationResponse authenticationResponse =
                _jwtService.GenerateJwtToken(user);

            return Ok(authenticationResponse);
        }

        return Problem("Invalid email or password.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("logout")]
    public async Task<ActionResult<ApplicationUser>> GetLogout()
    {
        await _signInManager.SignOutAsync();

        return NoContent();
    }
}
