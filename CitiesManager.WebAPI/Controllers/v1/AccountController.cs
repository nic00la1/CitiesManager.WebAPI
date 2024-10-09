using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1;

[AllowAnonymous]
[ApiVersion("1.0")]
public class AccountController : CustomControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             RoleManager<ApplicationRole> roleManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost]
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

            return Ok(user);
        } else
        {
            string errorMessage = string.Join(" | ", result.Errors
                .Select(e => e.Description)); // error1 | error2 | error3

            return Problem(errorMessage);
        }
    }

    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);

        if (user == null) return Ok(true);

        return Ok($"Email {email} is already in use");
    }
}
