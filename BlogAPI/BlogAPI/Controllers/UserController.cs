using Azure;
using BlogAPI.Helper;
using BlogAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JWTHelper _jWTHelper;
        private readonly DataContext _dataContext;
        //private readonly UserManager<ApplicationUser> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;
        //, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> roleManager
        public UserController(IConfiguration config, JWTHelper jWTHelper, DataContext dataContext)
        {
            _config = config;
            _jWTHelper = jWTHelper;
            _dataContext = dataContext;
            //this.roleManager = roleManager;
            //this.userManager = _userManager;
        }
         
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            try
            {
                if (userLogin is null)
                {
                    return BadRequest("Invalid user request!!!");
                }
                UserLogin user = Authenticate(userLogin);
                if (user != null)
                {
                    var token = _jWTHelper.GenerateToken(user);
                    return Ok(new CustomResponseLogin { Status = "Success", Message = token, UserID = user.UserID });
                }
                else
                {
                    return Ok(new CustomResponse { Status = "Error", Message = "Invalid user Details" });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }
        }
          
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserLogin userLogin)
        {
            try {
                if (userLogin is null)
                {
                    return BadRequest("Invalid user request!!!");
                }
                UserLogin user = _dataContext.UserLogin.FirstOrDefault(u => u.Email.Equals(userLogin.Email));
                if (user != null)
                {
                    return Ok(new CustomResponse { Status = "Error", Message = "User already exists!" });
                }
                else
                {
                    var SignUpData = new UserLogin()
                    {
                        UserID = Guid.NewGuid().ToString(),
                        FirstName = userLogin.FirstName,
                        LastName = userLogin.LastName,
                        Email = userLogin.Email,
                        MobileNumber = userLogin.MobileNumber,
                        Geneder = userLogin.Geneder,
                        Password = userLogin.Password,
                        IsActive = true,
                        ProfileImg = "Uploads\\EmptyPlaceHolder.jpg"
                    };
                    await _dataContext.UserLogin.AddAsync(SignUpData);
                    await _dataContext.SaveChangesAsync();
                    return Ok(new CustomResponse { Status = "Success", Message = "User created successfully!" });
                }
                 
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }
        }

        private UserLogin Authenticate(UserLogin userLogin)
        {
            //return _dataContext.UserLogin.FirstOrDefault(u => u.Email.Equals(userLogin.Email) || u.Password.Equals(userLogin.Password));
            var log = _dataContext.UserLogin.Where(x => x.Email.Equals(userLogin.Email) && x.Password.Equals(userLogin.Password)).FirstOrDefault();

            if (log == null)
            {

                return null;
            }
            else
               // return new Response { Status = "Success", Message = "Login Successfully" };
               return log;

        }

        [HttpGet]
        [Route("GetProfileDetail")]
        public async Task<IActionResult> GetProfileDetail(string UserID)
        {
            try
            {
                var contact = await _dataContext.UserLogin.Where(u => u.UserID.Equals(UserID)).ToListAsync();
                if (contact == null)
                {
                    return NotFound();
                }
                foreach (var item in contact)
                {
                    if (string.IsNullOrEmpty(item.ProfileImg))
                    {
                        item.ProfileImg = "Uploads\\EmptyPlaceHolder.jpg";
                    }
                    var byteArrImg = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), item.ProfileImg));
                    var base64Img = Convert.ToBase64String(byteArrImg);
                    item.ProfileImg = base64Img;
                }
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }

        }

    }
}
