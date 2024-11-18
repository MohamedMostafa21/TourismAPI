using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TourismAPI.DTO;
using TourismAPI.Models;

namespace TourismAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration config;


        public AuthenticationController (UserManager<User> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto) //Post api/Authentication/Register
        {
            // Handle file uploads
           
            if (ModelState.IsValid)
            {
                string profilePhotoPath = null;
                string cvDocumentPath = null;
                if (registerDto.ProfilePhoto != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "photos");
                    Directory.CreateDirectory(uploadsFolder);

                    profilePhotoPath = Path.Combine(uploadsFolder, registerDto.ProfilePhoto.FileName);

                    using (var stream = new FileStream(profilePhotoPath, FileMode.Create))
                    {
                        await registerDto.ProfilePhoto.CopyToAsync(stream);
                    }
                }

                if (registerDto.CvDocument != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cvs");
                    Directory.CreateDirectory(uploadsFolder);

                    cvDocumentPath = Path.Combine(uploadsFolder, registerDto.CvDocument.FileName);

                    using (var stream = new FileStream(cvDocumentPath, FileMode.Create))
                    {
                        await registerDto.CvDocument.CopyToAsync(stream);
                    }
                }

                User user = new User();
                user.UserName = registerDto.UserName;
                user.Email = registerDto.Email;
                user.FirstName = registerDto.FirstName;
                user.LastName = registerDto.LastName;
                user.PhoneNumber = registerDto.PhoneNumber;
                user.Gender  = registerDto.Gender.ToString();
                user.Age = registerDto.Age;
                user.Role = registerDto.Role;
                user.Country = registerDto.Country;
                user.PhotoUrl = profilePhotoPath;
                user.CvUrl = cvDocumentPath;
                IdentityResult result = await userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    return Ok("Create");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }

            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto LoggedUser) //Post api/Authentication/Login
        {
            if (ModelState.IsValid)
            {
                //check
                User userFromDb =
                    await userManager.FindByEmailAsync(LoggedUser.Email);
                if (userFromDb != null)
                {
                    bool found =
                        await userManager.CheckPasswordAsync(userFromDb, LoggedUser.Password); ;
                    if (found == true)
                    {
                        //generate token<==

                        List<Claim> UserClaims = new List<Claim>();

                        //Token Genrated id change (JWT Predefind Claims )
                        UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id));
                        UserClaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));

                        var UserRoles = await userManager.GetRolesAsync(userFromDb);

                        foreach (var roleNAme in UserRoles)
                        {
                            UserClaims.Add(new Claim(ClaimTypes.Role, roleNAme));
                        }

                        var SignInKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                config["JWT:SecritKey"]));

                        SigningCredentials signingCred =
                            new SigningCredentials
                            (SignInKey, SecurityAlgorithms.HmacSha256);

                        //design token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            audience: config["JWT:AudienceIP"],
                            issuer: config["JWT:IssuerIP"],
                            expires: DateTime.Now.AddHours(1),
                            claims: UserClaims,
                            signingCredentials: signingCred

                            );
                        //generate token response

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = DateTime.Now.AddHours(1)//mytoken.ValidTo
                            //
                        });
                    }
                }
                ModelState.AddModelError("Email", "Email OR Password  Invalid");
            }
            return BadRequest(ModelState);
        }

    }
}
