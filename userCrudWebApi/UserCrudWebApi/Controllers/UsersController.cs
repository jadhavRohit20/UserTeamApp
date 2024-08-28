using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserCrudWebApi.Interface;
using UserCrudWebApi.Model;

namespace UserCrudWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsers _IUsers;
        public IConfiguration _configuration;
        public UsersController(IUsers iusers, IConfiguration configuration)
        {
            _IUsers = iusers;
            _configuration = configuration;

        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginSubmit(string Username, string Password)
        {

            return await _IUsers.Login(Username, Password, this);
        }



        [Authorize]
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(Users user)
        {
            return await _IUsers.UsersAdd(user, this);

        }


        [Authorize]
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id, Users updatedUser)
        {
            return await _IUsers.UserUpdate(id, updatedUser, this);
        }


        [Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _IUsers.GetAllUser(this);
            if (users == null)
            {
                return Ok("user Not found");
            }
            return Ok(users);
        }



        [Authorize]
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _IUsers.GetUserById(id, this);
            if (user == null)
            {
                return Ok("user Not found");
            }
            return Ok(user);
        }


        [Authorize]
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _IUsers.UserDelete(id, this);
        }

        [Authorize]
        [HttpPost]
        [Route("SearchByName")]
        public async Task<IActionResult> EarchUser(string username)
        {
            return await _IUsers.UserSearch(username, this);
        }
    }
}
