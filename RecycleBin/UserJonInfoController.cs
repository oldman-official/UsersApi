// using API.Data;
// using API.DTOs;
// using API.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace API.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class UserJobInfoController : ControllerBase
// {
//     Functions _func = new();
//     DataContextDapper _dapper ; 
//     public UserJobInfoController(IConfiguration config) {
//         _dapper = new DataContextDapper(config);
//     }

//     [HttpGet("AllUsersJobInfo")]
//     public IEnumerable<UserJobInfo> GetUsers() {
//         string sql = @"
//             SELECT 
//                 [UserId],
//                 [JobTitle],
//                 [Department]
//             FROM UserData.UserJobInfo
//         ";
//         return _dapper.LoadData<UserJobInfo>(sql);
//     }


//     [HttpGet("UserJobInfo/{ID}")]

//     public UserJobInfo GetSingleUser(int ID) {
//         string sql = $@"
//             SELECT 
//                 [UserId],
//                 [JobTitle],
//                 [Department]
//             FROM UserData.UserJobInfo 
//                 WHERE UserId = {ID}
//         ";
//         return _dapper.LoadDataSingle<UserJobInfo>(sql);
//     }


//     [HttpPost("AddUserJobInfo")]
//     public IActionResult AddUser(UserJobInfoDTO user) {
//         string sql = $@"
//             INSERT INTO UserData.UserJobInfo(
//                 [JobTitle],
//                 [Department])
//             VALUES ('{_func.SQLSingleEscape(user.JobTitle)}' ,'{_func.SQLSingleEscape(user.Department)}')
//         ";
//         Console.WriteLine(sql);
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Something Went Wrong While Adding User's Job Info");
//     }


//     [HttpPut("EditUserJobInfo")]
//     public IActionResult EditUser(UserJobInfo user) {
//         string sql = $@"
//             UPDATE UserData.UserJobInfo
//                 SET JobTitle = '{_func.SQLSingleEscape(user.JobTitle)}' , Department = '{_func.SQLSingleEscape(user.Department)}' 
//                 WHERE UserId = {user.UserID};
//         ";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Something Went Wrong While Editing User's Job Info");
//     }

//     [HttpDelete("DeleteUserJobInfo/{userID}")]
//     public IActionResult DeleteUser(int userID) {
//         string sql = $"DELETE FROM UserData.UserJobInfo WHERE UserId = {userID}";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         } 
//         throw new Exception("Failed To Delete User's Job Info, You May Be Deleting A Non-Existing Row");
//     }
// }
