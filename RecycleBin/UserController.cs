// using API.Data;
// using API.DTOs;
// using API.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace API.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class UserController : ControllerBase
// {
//     DataContextDapper _dapper ; 
//     public UserController(IConfiguration config) {
//         _dapper = new DataContextDapper(config);
//     }
//     [HttpGet("AllUsers")]
//     public IEnumerable<User> GetUsers() {
//         string sql = @"
//         SELECT [UserId],
//             [FirstName],
//             [LastName],
//             [Email],
//             [Gender],
//             [Active] 
//         FROM UserData.Users
//         ";
//         return _dapper.LoadData<User>(sql);
//     }
//     [HttpGet("User/{ID}")]

//     public User GetSingleUser(int ID) {
//         string sql = $@"
//         SELECT [UserId],
//             [FirstName],
//             [LastName],
//             [Email],
//             [Gender],
//             [Active] 
//         FROM UserData.Users 
//             WHERE UserId = {ID}
//         ";
//         return _dapper.LoadDataSingle<User>(sql);
//     }
//     [HttpPost("AddUser")]
//     public IActionResult AddUser(UserDTO user) {
//         string sql = $@"
//             INSERT INTO UserData.Users(
//                 [FirstName],
//                 [LastName],
//                 [Email],
//                 [Gender],
//                 [Active])
//             VALUES ('{user.FirstName}' , '{user.LastName}' , '{user.Email}' , '{user.Gender}' , '{user.Active}')
//         ";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Something Went Wrong While Adding User");
//     }


//     [HttpPut("EditUser")]
//     public IActionResult EditUser(User user) {
//         string sql = $@"
//             UPDATE UserData.Users
//                 SET FirstName = '{user.FirstName}' , LastName = '{user.LastName}' , Email = '{user.Email}' , Gender = '{user.Gender}' , Active = '{user.Active}'
//                 WHERE UserID = {user.UserID};
//         ";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Something Went Wrong While Editing User");
//     }

//     [HttpDelete("DeleteUser/{userID}")]
//     public IActionResult DeleteUser(int userID) {
//         string sql = $"DELETE FROM UserData.Users WHERE UserId = {userID}";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         } 
//         throw new Exception("Failed To Delete User, You May Be Deleting A Non-Existing Row");
//     }
// }
