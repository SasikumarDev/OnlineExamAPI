using System;
using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using OnlineExamAPI.common;
using OnlineExamAPI.OnlineExamModels;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace OnlineExamAPI.Controllers
{
    [ApiController]
    public class AdminUsers : ControllerBase
    {
        #region "Constructor"
        private readonly IWebHostEnvironment _EnvFilePaths;
        private readonly IConfiguration _configuration;
        private OnlineExamContext _OnlineContext;
        private ILog _logger;
        public AdminUsers(IWebHostEnvironment _webHostEnv, IConfiguration configuration, ILog Log_)
        {
            this._EnvFilePaths = _webHostEnv;
            _configuration = configuration;
            this._logger = Log_;

        }
        #endregion
        #region "User Logon & Maintanence"
        [AllowAnonymous]
        [HttpPost]
        [Route("[controller]/LoginAdmin")]
        public IActionResult LoginAdmin(AdminUser user)
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                user.Apassword = PasswordDecrpt.DecryptPassword(user.Apassword);
                var _user = _OnlineContext.AdminUsers.FirstOrDefault(us => us.EmailId == user.EmailId && us.Apassword == user.Apassword);
                if (_user != null)
                {
                    var token = GenerateJwtToken(_user);
                    return new OkObjectResult(new { Token = token, Message = "Login Successfully", Status = HttpStatusCode.OK });
                }
                else
                {
                    return new OkObjectResult(new { Message = "Invalid Username or Password", Status = HttpStatusCode.Unauthorized });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/ValidateUser")]
        public bool ValidateUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return false;
            }
            else
            {
                var name = identity.Claims.Cast<Claim>().Where(p => p.Type == "UserID").FirstOrDefault()?.Value;
                if (name == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/CurrentUser")]
        public IActionResult CurrentUser()
        {
            try
            {
                string filepath = _EnvFilePaths.WebRootPath + "/AdminImages/";
                string urlpath = Request.Scheme + "://" + Request.Host.Value;
                var identity = User.Identity as ClaimsIdentity;
                var name = identity.Claims.Cast<Claim>().Where(p => p.Type == "UserID").FirstOrDefault()?.Value;
                _OnlineContext = new OnlineExamContext();
                var _usrs = (from Usrs in _OnlineContext.AdminUsers
                             where Usrs.AdminUsId.ToString() == name.ToString()
                             select new
                             {
                                 Name = Usrs.Firstname + " " + Usrs.Lastname,
                                 EmailID = Usrs.EmailId,
                                 ProfPic = urlpath + Url.Content($"/AdminImages/{Usrs.ImageUrl}")
                             }).ToList();
                _logger.Information($"Current Logged User : ${_usrs[0].EmailID}");
                return new OkObjectResult(new { Users = _usrs, Status = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        #endregion
        #region "Listing"
        [Authorize]
        [HttpGet]
        [Route("[controller]/AdminUserListing")]
        public async Task<IActionResult> AdminUserListing()
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                var Users = await (from Aduser in _OnlineContext.AdminUsers
                                   select new
                                   {
                                       AdID = Aduser.AdminUsId,
                                       FirstName = Aduser.Firstname,
                                       LastName = Aduser.Lastname,
                                       EmailID = Aduser.EmailId,
                                       DOB = Aduser.DateofBirth.ToString("dd-MMM-yyyy hh:mm tt"),
                                       CreatedOn = Aduser.CreatedOn.ToString("dd-MMM-yyyy hh:mm tt"),
                                       UpdatedOn = Aduser.UpdateOn.ToString("dd-MMM-yyyy hh:mm tt")
                                   }).OrderBy(x => x.FirstName).ToListAsync();
                return new OkObjectResult(new { Users = Users, Status = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
      /*  [Authorize]
        [HttpGet]
        [Route("[controller]/QuestionListing")]
        public async Task<IActionResult> QuestionListing()
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                var Questions = await (from Qst in _OnlineContext.Questions
                                       join Chs in _OnlineContext.Choices on Qst.Qcid equals Chs.Qid
                                       select new
                                       {
                                           Qid = Qst.Qid,
                                           Qstion = Qst.Qtext
                                       }).ToListAsync();
                return new OkObjectResult(new { Question = Questions, Status = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        } */
        [Authorize]
        [HttpGet]
        [Route("[controller]/CategoryListing")]
        public async Task<IActionResult> CategoryListing()
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                var _category = await (from ct in _OnlineContext.Categories
                                       select new
                                       {
                                           CID = ct.Cid,
                                           Desc = ct.Cdesc,
                                           Type = ct.CqstType == 1 ? "Verbal" : "Non-Verbal",
                                           Noofqst = ct.CnoOfQst,
                                           DefaultScore = ct.CdefaultScoreorQst,
                                           totalscore = ct.CnoOfQst * ct.CdefaultScoreorQst
                                       }).OrderBy(x => x.Desc).ToListAsync();
                return new OkObjectResult(new { Category = _category, Status = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("[controller]/QuestionListing")]
        public async Task<IActionResult> QuestionListing()
        {
            try
            {
                _logger.Information("Question Listin");
                _OnlineContext = new OnlineExamContext();
                var Question = await _OnlineContext.Questions.Include(xc => xc.Choices).ToListAsync();
                return new OkObjectResult(new { Data = Question, Status = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        #endregion
        #region "Non Methods"
        [NonAction]
        public async Task<AdminUser> GetCrntUser(HttpContext httpContext)
        {
            _OnlineContext = new OnlineExamContext();
            var identity = User.Identity as ClaimsIdentity;
            var name = identity.Claims.Cast<Claim>().Where(p => p.Type == "UserID").FirstOrDefault()?.Value;
            if (name != null)
            {
                return await _OnlineContext.AdminUsers.FirstOrDefaultAsync(us => us.AdminUsId.ToString() == name.ToString());
            }
            else
            {
                return null;
            }
        }
        [NonAction]
        public AdminUser GetCrntUserNonayc(HttpContext httpContext)
        {
            _OnlineContext = new OnlineExamContext();
            var identity = User.Identity as ClaimsIdentity;
            var name = identity.Claims.Cast<Claim>().Where(p => p.Type == "UserID").FirstOrDefault()?.Value;
            if (name != null)
            {
                return _OnlineContext.AdminUsers.FirstOrDefault(x => x.AdminUsId.ToString() == name.ToString());
            }
            else
            {
                return null;
            }
        }

        [NonAction]
        private object GenerateJwtToken(AdminUser usercred)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, usercred.AdminUsId.ToString()),
                        new Claim("UserMail", usercred.EmailId),
                        new Claim("UserID", usercred.AdminUsId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
        #region "Category"
        [HttpPost]
        [Authorize]
        [Route("[controller]/SaveCategory")]
        public async Task<IActionResult> SaveCategory([FromBody] Category _category)
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                var category = await _OnlineContext.Categories.FirstOrDefaultAsync(xc => xc.Cid.Equals(_category.Cid));
                if (category == null)
                {
                    _category.CqstType = 1;
                    _category.CtotalQst = _category.CnoOfQst;
                    await _OnlineContext.Categories.AddAsync(_category);
                    await _OnlineContext.SaveChangesAsync();
                    return new OkObjectResult(new { Message = "Saved Successfully", Status = HttpStatusCode.OK });
                }
                else
                {
                    category.Cdesc = _category.Cdesc;
                    category.CdefaultScoreorQst = _category.CdefaultScoreorQst;
                    category.CnoOfQst = _category.CnoOfQst;
                    category.CqstType = _category.CqstType;
                    category.Cstatus = _category.Cstatus;
                    category.CqstType = 1;
                    category.CtotalQst = _category.CnoOfQst;
                    await _OnlineContext.SaveChangesAsync();
                    return new OkObjectResult(new { Message = "Updated Successfully", Status = HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("[controller]/GetCategory/{ID}")]
        public async Task<IActionResult> GetCategory(string ID)
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                var isexists = await _OnlineContext.Categories.AnyAsync(xc => xc.Cid.ToString() == ID);
                if (isexists)
                {
                    var rslt = await _OnlineContext.Categories.FirstOrDefaultAsync(xc => xc.Cid.ToString() == ID);
                    return new OkObjectResult(new { Data = rslt, Status = HttpStatusCode.OK });
                }
                else
                {
                    return new OkObjectResult(new { Message = "No Data Found", Status = HttpStatusCode.NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("[controller]/DeleteCategory/{ID}")]
        public async Task<IActionResult> DeleteCategory(string ID)
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                if (_OnlineContext.Categories.Any(xc => xc.Cid.ToString() == ID))
                {
                    var _category = await _OnlineContext.Categories.FirstOrDefaultAsync(xc => xc.Cid.ToString() == ID);
                    _OnlineContext.Categories.Attach(_category);
                    _OnlineContext.Categories.Remove(_category);
                    await _OnlineContext.SaveChangesAsync();
                    _logger.Information($"Deleted Category ID is {ID}");
                    return new OkObjectResult(new { Message = "Deleted", Status = HttpStatusCode.OK });
                }
                else
                {
                    _logger.Debug($"Delete Request For Category ID : {ID}");
                    return new OkObjectResult(new { Message = "No Data Found", Status = HttpStatusCode.NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        #endregion
        #region "Admin User Creation"
        [Authorize]
        [HttpPost, DisableRequestSizeLimit]
        [Route("[controller]/NewAdmins")]
        public IActionResult NewAdmins()
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                string _id = Request.Form["AdminUsId"];
                if (_id == null)
                {
                    AdminUser adminUser = new AdminUser();
                    adminUser.Firstname = Request.Form["Firstname"];
                    adminUser.Lastname = Request.Form["Lastname"];
                    adminUser.EmailId = Request.Form["EmailId"];
                    adminUser.Apassword = PasswordDecrpt.DecryptPassword(Request.Form["Apassword"]);
                    adminUser.DateofBirth = Convert.ToDateTime(Request.Form["DateofBirth"]);
                    if (Request.Form?.Files.Count() > 0)
                    {
                        var file = Request.Form.Files[0];
                        adminUser.ImageUrl = adminUser.EmailId + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                        string filepath = _EnvFilePaths.WebRootPath + "/AdminImages/" + adminUser.ImageUrl;
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    else
                    {
                        adminUser.ImageUrl = "DefaultImage.png";
                    }
                    _OnlineContext.AdminUsers.Add(adminUser);
                    _OnlineContext.SaveChanges();
                    return new OkObjectResult(new { Message = "User Created Successfully", Status = HttpStatusCode.OK });
                }
                else
                {
                    var _EditAdminUser = _OnlineContext.AdminUsers.FirstOrDefault(xus => xus.AdminUsId.ToString() == _id);
                    _EditAdminUser.Firstname = Request.Form["Firstname"];
                    _EditAdminUser.Lastname = Request.Form["Lastname"];
                    _EditAdminUser.EmailId = Request.Form["EmailId"];
                   // _EditAdminUser.DateofBirth = Convert.ToDateTime(Request.Form["DateofBirth"]);
                    if (Request.Form?.Files.Count() > 0)
                    {
                        var file = Request.Form.Files[0];
                        _EditAdminUser.ImageUrl = _EditAdminUser.EmailId + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                        string filepath = _EnvFilePaths.WebRootPath + "/AdminImages/" + _EditAdminUser.ImageUrl;
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    _OnlineContext.SaveChanges();
                    return new OkObjectResult(new { Message = "Updated Successfully", Status = HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString()+Request.Form.Files);
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        [HttpGet]
        [Authorize]
        [Route("[controller]/GetAdminUser/{ID}")]
        public async Task<IActionResult> GetAdminUser(string ID)
        {
            try
            {
                _OnlineContext = new OnlineExamContext();
                var userexistence = await _OnlineContext.AdminUsers.AnyAsync(xau => xau.AdminUsId.ToString().Equals(ID));
                if (userexistence)
                {
                    string filepath = _EnvFilePaths.WebRootPath + "/AdminImages/";
                    string urlpath = Request.Scheme + "://" + Request.Host.Value;
                    var users = await (from usr in _OnlineContext.AdminUsers
                                       where usr.AdminUsId.ToString().Equals(ID)
                                       select new AdminUser
                                       {
                                           Firstname = usr.Firstname,
                                           Lastname = usr.Lastname,
                                           AdminUsId = usr.AdminUsId,
                                           EmailId = usr.EmailId,
                                           DateofBirth = usr.DateofBirth,
                                           ImageUrl = urlpath + Url.Content($"/AdminImages/{usr.ImageUrl}")
                                       }).ToListAsync();
                    return new OkObjectResult(new { Data = users, Status = HttpStatusCode.OK });
                }
                else
                {
                    return new OkObjectResult(new { Message = "Not Found", Status = HttpStatusCode.NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new OkObjectResult(new { Message = ex.Message.ToString(), Status = HttpStatusCode.InternalServerError });
            }
        }
        #endregion
    }
}
