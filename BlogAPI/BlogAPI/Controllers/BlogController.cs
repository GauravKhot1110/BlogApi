using BlogAPI.Helper;
using BlogAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using static System.Reflection.Metadata.BlobBuilder;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JWTHelper _jWTHelper;
        private readonly DataContext _dataContext;
       
        public BlogController(IConfiguration config, JWTHelper jWTHelper, DataContext dataContext 
           
            )
        {
            _config = config;
            _jWTHelper = jWTHelper;
            _dataContext = dataContext;
            
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();

                string title = formCollection["title"], Description = formCollection["Description"], CreateBy = formCollection["CreateBy"];

                string dbPath = string.Empty;

                var files = formCollection.Files;
                var folderName = Path.Combine("Uploads", CreateBy);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                foreach (var file in files)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                var BlogData = new BlogMaster()
                {
                    UserID = Guid.NewGuid().ToString(),
                    Title = title,
                    Description = Description,
                    Filename = dbPath,
                    CountLikes = 0,
                    CountUnLikes = 0,
                    CountViews = 0,
                    CountComments = 0,
                    CreateBy = CreateBy,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                };
                await _dataContext.BlogMaster.AddAsync(BlogData);
                await _dataContext.SaveChangesAsync();
                return Ok(new CustomResponse { Status = "Success", Message = "Blog created successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }

        }




        [HttpGet]
        [Route("GetAllBlog")]
        public async Task<IActionResult> GetAllBlog()
        {
            try
            {
                var contact = await _dataContext.BlogMaster.OrderByDescending(x => x.CreateDate).ToListAsync();
                if (contact == null)
                {
                    return NotFound();
                }
                foreach (var item in contact)
                {
                    if (string.IsNullOrEmpty(item.Filename) || !BlogFileExist(item.Filename))
                    {
                        item.Filename = "Uploads\\EmptyPlaceHolder.jpg";
                    }
                    var byteArrImg = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), item.Filename));
                    var base64Img = Convert.ToBase64String(byteArrImg);
                    item.Filename = base64Img;
                }
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetMyBlog")]
        public async Task<IActionResult> GetMyBlog(string CreateBy)
        {
            try
            {
                var contact = await _dataContext.BlogMaster.Where(u => u.CreateBy.Equals(CreateBy)).OrderByDescending(x => x.CreateDate).ToListAsync();
                if (contact == null)
                {
                    return NotFound();
                }
                foreach (var item in contact)
                {
                    if (string.IsNullOrEmpty(item.Filename) || !BlogFileExist(item.Filename))
                    {
                        item.Filename = "Uploads\\EmptyPlaceHolder.jpg";
                    }
                    var byteArrImg = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), item.Filename));
                    var base64Img = Convert.ToBase64String(byteArrImg);
                    item.Filename = base64Img;
                }
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }

        }

        [HttpGet]
        [Route("getMyBlogDetails")]
        public async Task<IActionResult> getMyBlogDetails(string UserID)
        {
            try
            {
                var contact = await _dataContext.BlogMaster.Where(u => u.UserID.Equals(UserID)).ToListAsync();
                if (contact == null)
                {
                    return NotFound();
                }
                foreach (var item in contact)
                {
                    if (string.IsNullOrEmpty(item.Filename) || !BlogFileExist(item.Filename))
                    {
                        item.Filename = "Uploads\\EmptyPlaceHolder.jpg";
                    }
                    var byteArrImg = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), item.Filename));
                    var base64Img = Convert.ToBase64String(byteArrImg);
                    item.Filename = base64Img;
                }
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }

        }

        [HttpPost]
        [Route("CreateAction")]
        public async Task<IActionResult> CreateAction([FromBody] ActionDetails actionDetails)
        {
            try
            {
                if (actionDetails is null)
                {
                    return BadRequest("Invalid user request!!!");
                }
                ActionDetails user = _dataContext.ActionDetails.FirstOrDefault(u => u.ActionUserID.Equals(actionDetails.ActionUserID));
                if (user != null)
                {
                    user.ActionFor = actionDetails.ActionFor;
                    user.ActionBy = actionDetails.ActionBy;
                    user.Comment = actionDetails.Comment;
                    user.IsLikeUnlikeComment = actionDetails.IsLikeUnlikeComment;
                    await _dataContext.SaveChangesAsync();
                    return Ok(new CustomResponse { Status = "Success", Message = "Records updated successfully!" });
                }
                else
                {
                    var ActionData = new ActionDetails()
                    {
                        ActionUserID = Guid.NewGuid().ToString(),
                        ActionBy = actionDetails.ActionBy,
                        ActionFor = actionDetails.ActionFor,
                        IsLikeUnlikeComment = actionDetails.IsLikeUnlikeComment,
                        Comment = actionDetails.Comment,
                        IsActive = true,
                    };
                    await _dataContext.ActionDetails.AddAsync(ActionData);
                    await _dataContext.SaveChangesAsync();
                    return Ok(new CustomResponse { Status = "Success", Message = "Records created successfully!" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("getActionDetails")]
        public async Task<IActionResult> getActionDetails(string ActionFor)
        {
            try
            { // var contact = await _dataContext.ActionDetails.Where(u => u.ActionFor.Equals(ActionFor)).OrderByDescending(x => x.CreateDate).ToListAsync();

                var query = from a in _dataContext.ActionDetails
                            join b in _dataContext.UserLogin on a.ActionBy equals b.UserID into Action_details
                            where a.IsLikeUnlikeComment.Equals(3)
                            orderby a.CreateDate descending
                            from left in Action_details.DefaultIfEmpty()
                            select new ActionViewDetails()
                            {
                                FirstName = left.FirstName,
                                IsLikeUnlikeComment = a.IsLikeUnlikeComment,
                                Comment = a.Comment,
                                ProfileImg = left.ProfileImg,
                                CreateDate = a.CreateDate
                            };

                var contact = query.ToList();
                foreach (var item in contact)
                {
                    if (string.IsNullOrEmpty(item.ProfileImg) || !BlogFileExist(item.ProfileImg))
                    {
                        item.ProfileImg = "Uploads\\EmptyPlaceHolder.jpg";
                    }
                    var byteArrImg = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), item.ProfileImg));
                    var base64Img = Convert.ToBase64String(byteArrImg);
                    item.ProfileImg = base64Img;
                }
                if (contact == null)
                {
                    return NotFound();
                }
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return Ok(new CustomResponse { Status = "Error", Message = ex.Message });
            }
        }

        private bool BlogFileExist(string filePath)
        {
            try
            {
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                if (System.IO.File.Exists(pathToSave)) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

}
