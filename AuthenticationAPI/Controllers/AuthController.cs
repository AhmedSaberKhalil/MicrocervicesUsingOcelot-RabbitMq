using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IOptions<JwtSettings> _options;
        private readonly IConfiguration configuration;
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> options)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._options = options;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterationDto registerationDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = registerationDto.Username;
                user.Email = registerationDto.Email;
                IdentityResult result = await userManager.CreateAsync(user, registerationDto.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false); // flase =>  session coockie عشان لو نسي الباسورد يرجع يسجل تاني
                    return Ok(true);
                }
                foreach (var item in result.Errors)
                {
                    return BadRequest(item.Description);
                }

            }

            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                // check user name
                ApplicationUser user = await userManager.FindByNameAsync(loginDto.Username);
                if (user != null)
                {
                    // check password
                    bool found = await userManager.CheckPasswordAsync(user, loginDto.Password);
                    if (found)
                    {
                        // Add Claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        // Add Role
                        // Role تبع انهي Token عشان اعرف ال

                        var role = await userManager.GetRolesAsync(user);
                        foreach (var itemRole in role)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, itemRole));
                        }

                        // Key To Pass To signingCredentials
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Secret)); // configuration["JWT:Secret"]
                                                                                                                           // signingCredentials 
                        SigningCredentials signincred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        // create Token  in 2 steps

                        // 1- represent Token
                        // Json الداتا هنا بتتبعت
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: _options.Value.ValidIssuer,//configuration["JWT:ValidIssuer"],     // url web api (Provider)
                            audience: _options.Value.ValiedAudiance,//configuration["JWT:ValiedAudiance"],  // url consumer angular
                            claims: claims,
                            expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: signincred

                            ); ; ;
                        // Generate and set password reset token
                        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                        await userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "ResetPasswordToken", resetToken);
                        // save user info to AspNetUserLogins
                        await userManager.AddLoginAsync(user, new UserLoginInfo("Identity", user.Id, user.UserName));


                        // 2- Create Token
                        return Ok(new
                        {

                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration = myToken.ValidTo
                        });
                    }

                }
                return Unauthorized();

            }
            return Unauthorized();
        }

        // all external login
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            GoogleLoginDto model = new GoogleLoginDto
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return Ok(model);
        }

    }
}
