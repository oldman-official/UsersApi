// using API.Data;
// using API.DTOs;
// using API.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace API.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class UserSalaryController : ControllerBase
// {
//     DataContextDapper _dapper ; 
//     public UserSalaryController(IConfiguration config) {
//         _dapper = new DataContextDapper(config);
//     }

//     [HttpGet("AllUsersJobInfo")]
//     public IEnumerable<UserSalary> GetUsers() {
//         string sql = @"
//             SELECT 
//                 [UserId],
//                 [Salary]
//             FROM UserData.UserSalary
//         ";
//         return _dapper.LoadData<UserSalary>(sql);
//     }


//     [HttpGet("UserSalary/{ID}")]

//     public UserSalary GetSingleUser(int ID) {
//         string sql = $@"
//             SELECT 
//                 [UserId],
//                 [Salary]
//             FROM UserData.UserSalary 
//                 WHERE UserId = {ID}
//         ";
//         return _dapper.LoadDataSingle<UserSalary>(sql);
//     }


//     [HttpPost("AddUserJobInfo")]
//     public IActionResult AddUser(UserSalaryDTO user) {
//         string sql = $@"
//             INSERT INTO UserData.UserSalary(
//                 [Salary]
//                 )
//             VALUES ('{user.Salary}')
//         ";
//         Console.WriteLine(sql);
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Something Went Wrong While Adding User's Salary");
//     }


//     [HttpPut("EditUserJobInfo")]
//     public IActionResult EditUser(UserSalary user) {
//         string sql = $@"
//             UPDATE UserData.UserSalary
//                 SET Salary = '{user.Salary}'
//                 WHERE UserId = {user.UserID};
//         ";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Something Went Wrong While Editing User's Salary");
//     }

//     [HttpDelete("DeleteUserJobInfo/{userID}")]
//     public IActionResult DeleteUser(int userID) {
//         string sql = $"DELETE FROM UserData.UserSalary WHERE UserId = {userID}";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         } 
//         throw new Exception("Failed To Delete User's Salary, You May Be Deleting A Non-Existing Row");
//     }
// }
