using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Services.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Entities.Token;
using EmailingSystem.Core.Enums;
using EmailingSystem.Services;
using EmailingSystemAPI.DTOs.User;
using EmailingSystemAPI.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public AuthController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<int>> roleManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AuthDto>> Register(RegisterDto registerDto)
        {

            #region Some Validations

            var AdminEmail = User.FindFirstValue(ClaimTypes.Email);
            var Admin = await userManager.FindByEmailAsync(AdminEmail);
            var AdminRole = (await userManager.GetRolesAsync(Admin)).ToString();

            if(registerDto.Role == UserRole.Admin) 
            {
                return Unauthorized(new APIErrorResponse(401, "Can't create user with Role Admin."));
            }
            else if(registerDto.Role == UserRole.CollegeAdmin)
            {
                if (registerDto.CollegeId is null || registerDto.DepartmentId.HasValue) 
                    return BadRequest(new APIErrorResponse(400,"User with Role College Admin must be in a college and can't be in a department."));
            }
            else if(registerDto.Role == UserRole.Presedient)
            {
                if (registerDto.CollegeId.HasValue || registerDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400, "Can't create user with Role Presedient with department and college."));
            }
            else if(registerDto.Role == UserRole.VicePresedient)
            {
                //Some Code
            }
            else if (registerDto.Role == UserRole.Dean)
            {
                if (registerDto.CollegeId is null || registerDto.DepartmentId.HasValue)
                    return BadRequest("User with Role Dean must have a college and can't be in a department.");
            }
            else if (registerDto.Role == UserRole.ViceDean)
            {
                //Some Code
            }
            else if (registerDto.Role == UserRole.Secretary)
            {
                if (registerDto.CollegeId is null || registerDto.DepartmentId.HasValue) 
                    return BadRequest(new APIErrorResponse(400, "User with Role Secertary must be in a college and can't be in  a department."));
            }
            else if (registerDto.Role == UserRole.NormalUser)
            {
                if (!registerDto.CollegeId.HasValue || !registerDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400,"User with Role Normal User must be in a college and in a depertment."));
            }

            // Check if college admin and user are in the same college 
            if (AdminRole == UserRole.CollegeAdmin.ToString())
            {
                if(Admin.CollegeId != registerDto.CollegeId)
                {
                    return Unauthorized(new APIErrorResponse(401, "You aren't authorized do add a user to this college."));
                }
            }

            //Check if the selected department in the selected college
            if (registerDto.DepartmentId is not null && registerDto.CollegeId is not null)
            {
                var College = await unitOfWork.Repository<College>().GetByIdAsync<int>(registerDto.CollegeId);
                var Department = await unitOfWork.Repository<Department>().GetByIdAsync<int>(registerDto.DepartmentId);

                if (Department is null) return NotFound(new APIErrorResponse(404, "Department Not Found."));
                if (College is null) return NotFound(new APIErrorResponse(404, "College Not Found."));

                if (!College.Departments.Any(D => D.Id == registerDto.DepartmentId))
                    return BadRequest(new APIErrorResponse(400,"This College doesn't contain such a department"));
            }


            var EmailExists = await userManager.FindByEmailAsync(registerDto.Email);
            if (EmailExists is not null) return BadRequest(new APIErrorResponse(400, "Email Already Exists."));
            #endregion

            using var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                var AppUser = mapper.Map<ApplicationUser>(registerDto);
                AppUser.UserName = registerDto.Email.Substring(0, registerDto.Email.IndexOf("@"));
                AppUser.NormalizedName = AppUser.Name.Trim().ToUpper();
                    

                if (registerDto.Picture is not null)
                {
                    AppUser.PicturePath = await FileHandler.SaveFile(registerDto.Picture.FileName, "ProfileImages", registerDto.Picture);
                }

                if (registerDto.SignatureFile is not null)
                {
                    var Signature = new Signature()
                    {
                        FileName = registerDto.SignatureFile.FileName,
                        FilePath = await FileHandler.SaveFile(registerDto.SignatureFile.FileName, "Signatures", registerDto.SignatureFile)
                    };

                    await unitOfWork.Repository<Signature>().AddAsync(Signature);
                    await unitOfWork.CompleteAsync();

                    AppUser.SignatureId = Signature.Id;
                }

                var Result = await userManager.CreateAsync(AppUser, registerDto.Password);
                
                if (!Result.Succeeded) throw new Exception();


                var role = await roleManager.FindByNameAsync(registerDto.Role.ToString());

                if (role is null)
                {
                    await roleManager.CreateAsync(new IdentityRole<int>() { Name = registerDto.Role.ToString() });
                }

                await userManager.AddToRoleAsync(AppUser,registerDto.Role.ToString());


                var refreshToken = GenerateRefreshToken();
                AppUser?.RefreshTokens?.Add(refreshToken);

                SetRefreshTokenInCookie(refreshToken.Token, refreshToken.ExpiresOn);

                await unitOfWork.CompleteAsync();
                await transaction.CommitAsync();

                var userDto = mapper.Map<AuthDto>(AppUser);
                userDto.Role = (await userManager.GetRolesAsync(AppUser)).ToString();
                userDto.AccessToken = await tokenService.CreateTokenAsync(AppUser, userManager);
                userDto.RefreshTokenExpirationTime = refreshToken.ExpiresOn;

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new APIErrorResponse(500, "An error occurred. Please try again later."));
            }
        }

        [HttpGet("LogIn")]
        public async Task<ActionResult<AuthDto>> Login([FromQuery] LogInDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) return BadRequest(new APIErrorResponse(400,"Invalid Email Or Password."));

            var Result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if(!Result.Succeeded) return BadRequest(new APIErrorResponse(400, "Invalid Email Or Password."));

            var userRefreshToken = user.RefreshTokens?.FirstOrDefault(T => T.IsActive);

            if (string.IsNullOrEmpty(userRefreshToken?.Token))
            {
                var refreshToken = GenerateRefreshToken();

                user?.RefreshTokens?.Add(refreshToken);
                await userManager.UpdateAsync(user);

                SetRefreshTokenInCookie(refreshToken.Token, refreshToken.ExpiresOn);
            }
            else
            {
                SetRefreshTokenInCookie(userRefreshToken.Token, userRefreshToken.ExpiresOn);
            }

            var userDto = mapper.Map<AuthDto>(user);
            userDto.Role = (await userManager.GetRolesAsync(user)).ToString();
            userDto.AccessToken = await tokenService.CreateTokenAsync(user,userManager);
            userDto.RefreshTokenExpirationTime = user.RefreshTokens.FirstOrDefault(T => T.IsActive).ExpiresOn;

            return Ok(userDto);
        }

        [HttpGet("LogOut")]
        public async Task<ActionResult> LogOut()
        {
            var refreshToken = Request?.Cookies["RefreshToken"];

            if (refreshToken is null) return NotFound(new APIErrorResponse(404,"User Isn't Logged in."));

            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            var TokenToDelete = user.RefreshTokens.FirstOrDefault(T => T.Token == refreshToken);

            var Result = user.RefreshTokens.Remove(TokenToDelete);
            await userManager.UpdateAsync(user);

            if(!Result) return BadRequest(new APIErrorResponse(400,"An error ocurred while logging out."));

            Response.Cookies.Delete("RefreshToken");

            return Ok();
        }

        [HttpGet("Refresh")]
        public async Task<ActionResult<AuthDto>> Refresh()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if(refreshToken.IsNullOrEmpty())
            {
                return NotFound(new APIErrorResponse(404,"Log in and try again."));
            }

            var user = userManager.Users.FirstOrDefault(U => U.RefreshTokens.Any(T => T.IsActive && T.Token == refreshToken));

            if (user is null)
            {
                return NotFound(new APIErrorResponse(404, "Log in and try again."));
            }

            var AuthDto = mapper.Map<AuthDto>(user);
            AuthDto.Role = (await userManager.GetRolesAsync(user)).ToString();
            AuthDto.AccessToken = await tokenService.CreateTokenAsync(user, userManager);
            AuthDto.RefreshTokenExpirationTime = user.RefreshTokens?.FirstOrDefault(T => T.IsActive)?.ExpiresOn;

            return Ok(AuthDto);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var token = Guid.NewGuid().ToString();

            return new RefreshToken
            {
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddDays(double.Parse(configuration["JWT:DurationInDays"] ?? "30")),
                CreatedOn = DateTime.UtcNow
            };
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime ExpiresOn)
        {
            var cookiewOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = ExpiresOn.ToLocalTime()
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookiewOptions);
        }
    
    }
}
