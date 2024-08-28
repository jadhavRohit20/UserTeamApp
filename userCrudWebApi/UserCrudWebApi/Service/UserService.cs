using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserCrudWebApi.Context;
using UserCrudWebApi.Interface;
using UserCrudWebApi.Model;

namespace UserCrudWebApi.Service
{
    public class UserService : IUsers
    {
        private readonly UserContext _context;
        public IConfiguration _configuration;
        public UserService(IConfiguration configuration, UserContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        //Log In Token
        public async Task<IActionResult> Login(string Username, string Password, ControllerBase controllerBase)
        {
            var uName = await _context.Users.OrderBy(x => x.id).LastOrDefaultAsync(x => x.username == Username);
            if (uName != null)
            {
                return (new Response { Status = "Error", Message = "Username already exist" });
            }

            if (Username != null && Password != null)
            {
                //var userData = await GetUser(Username, Password);
                var userData = await _context.Users.Where(x=>x.username == Username && x.password== Password).ToListAsync();
                var jwt = _configuration.GetSection("jwt").Get<Jwt>();

                if (userData != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", Username)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                                      jwt.Issuer,
                                      jwt.Audience,
                                      claims,
                                      expires: DateTime.Now.AddMinutes(20),
                                      signingCredentials: signIn
                                      );
                    var Token = new JwtSecurityTokenHandler().WriteToken(token);
                    return controllerBase.Ok(new
                    {
                        Message = "Login successful",
                        token = Token
                    });
                }
                else
                {
                    return controllerBase.BadRequest(new
                    {
                        Message = "Invalid Credentials"
                    });
                }
            }
            else
            {
                return controllerBase.BadRequest(new
                {
                    Message = "Invalid Credentials"
                });

            }
        }

        //Geting User Details on username and password
        //public async Task<Users> GetUser(string username, string password)
        //{
        //    return await _context.Users.Where(x => x.username == username && x.password == password).ToListAsync();

        //}





        public async Task<IActionResult> UsersAdd(Users? user, ControllerBase controllerBase)

        {

            try
            {
                if (user.username == null || user.username == "" || user.password == null || user.password == "" || user.age == null)
                {
                    return controllerBase.BadRequest(new
                    {
                        Message = "Fill Mandatory Field"
                    });
                }
                //New User Data Add through Model
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return controllerBase.Ok(new
                {
                    Message = "Add User Successfully."
                });
            }
            catch (Exception ex)
            {
                return controllerBase.BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> UserUpdate(int id, Users updatedUser, ControllerBase controllerBase)
        {
            var user = await _context.Users.Where(x => x.id == id).ToListAsync();
            if (user == null || user.Count == 0)
            {
                return controllerBase.NotFound(new { Status = "Error", Message = "user Not found" });
            }

            //Update data
            user[0].username = updatedUser.username;
            user[0].password = updatedUser.password;
            user[0].age = updatedUser.age;
            user[0].isAdmin = updatedUser.isAdmin;
            user[0].hobbies = updatedUser.hobbies;
            _context.SaveChanges();

            return controllerBase.Ok(new
            {
                Message = "Update User Successfully."
            });

        }
        //Get All User
        public async Task<IActionResult> GetAllUser(ControllerBase controllerBase)
        {
            var user = await _context.Users.OrderByDescending(x => x.id).ToListAsync();
            if (user == null)
            {
                return controllerBase.NotFound(new { Status = "Error", Message = "User Not Found" });
            }
            return controllerBase.Ok(user);
        }

        //Get user on ID basis
        public async Task<IActionResult> GetUserById(int id, ControllerBase controllerBase)
        {
            try
            {
                //Fetch User By Id
                var user = await _context.Users.Where(x => x.id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return controllerBase.NotFound(new { Status = "Error", Message = "User Not found" });
                }
                return controllerBase.Ok(user);
            }
            catch (Exception ex)
            {
                return controllerBase.BadRequest(new { Message = ex.Message });
            }
        }

        public async Task<IActionResult> UserDelete(int id, ControllerBase controllerBase)
        {
            try
            {
                //Delete User by Id
                var user = await _context.Users.Where(x => x.id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return controllerBase.NotFound(new { Status = "Error", Message = "user Not found" });
                }


                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return controllerBase.Ok(new
                {
                    Message = "User deleted Successfully."
                });
            }
            catch (Exception ex)
            {
                return controllerBase.BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> UserSearch(string username, ControllerBase controllerBase)
        {
            try
            {
                var data = await _context.Users.Where(a => a.username == username).ToListAsync();
                //Fetch User By Id
                if (data == null)
                {
                    return controllerBase.NotFound(new { Status = "Error", Message = "User Not found" });
                }
                return controllerBase.Ok(data);
            }
            catch (Exception ex)
            {
                return controllerBase.BadRequest(new { Message = ex.Message });
            }

        }
    }
}
