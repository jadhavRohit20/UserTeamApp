using Microsoft.AspNetCore.Mvc;
using UserCrudWebApi.Model;

namespace UserCrudWebApi.Interface
{
    public interface IUsers
    {
        //public Task<Users> GetUser(string username, string password);
        public Task<IActionResult> Login(string Username, string Password, ControllerBase controllerBase);
        public Task<IActionResult> UsersAdd(Users user, ControllerBase controllerBase);
        public Task<IActionResult> UserUpdate(int id, Users updatedUser, ControllerBase controllerBase);
        public Task<IActionResult> GetAllUser(ControllerBase controllerBase);
        public Task<IActionResult> GetUserById(int id, ControllerBase controllerBase);
        public Task<IActionResult> UserDelete(int id, ControllerBase controllerBase);
        public Task<IActionResult> UserSearch(string username, ControllerBase controllerBase);
    }
}
