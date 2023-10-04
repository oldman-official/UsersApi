// using API.Data;
// using API.DTOs;
// using API.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace API.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class CompleteUserController : ControllerBase
// {
//     DataContextDapper _dapper ; 
//     Functions _func = new Functions();
//     public CompleteUserController(IConfiguration config) {
//         _dapper = new DataContextDapper(config);
//     }
//     [HttpGet("GetUser/{UserId}/{isActive}")]
//     public IEnumerable<CompleteUserInfo> GetUsers(int UserId , bool isActive) {
//         string sql = @"EXEC UserData.spGetAllUsers";
//         string parameters = "";
//         if (UserId != 0) {
//             parameters += $", @UserId = {UserId}";
//         }
//         if (isActive) {
//             parameters += $", @Active = {isActive}";
//         }
//         if (parameters.Length > 1) {
//             Console.WriteLine(parameters);
//             sql += parameters.Substring(1);
//         }
//         //? Should we change the way false works? User 17 with active = false will return results
//         Console.WriteLine(sql);
//         return _dapper.LoadData<CompleteUserInfo>(sql);
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
//     public IActionResult EditUser(CompleteUserInfo user) {
//         string sql = $@"
//             EXEC UserData.spUser_Upsert
//                 @FirstName = '{user.FirstName}' , @LastName = '{user.LastName}' , @Email = '{user.Email}' , @Gender = '{user.Gender}' , @Active = '{user.Active}'
//                 , @JobTitle = '{user.JobTitle}' , @Department = '{user.Department}' , @Salary = '{user.Salary}' , @UserId ={user.UserId}
//         ";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Something Went Wrong While Adding/Editing User");
//     }

//     [HttpDelete("DeleteUser/{userID}")]
//     public IActionResult DeleteUser(int userID) {
//         string sql = $"EXEC UserData.spUser_Delete @UserId = {userID}";
//         if(_dapper.ExecuteBool(sql)) {
//             return Ok();
//         } 
//         throw new Exception("Failed To Delete User, You May Be Deleting A Non-Existing Row");
//     }

//     // USER JOB INFO CONTROLLER 
//     // [HttpGet("AllUsersJobInfo")]
//     // public IEnumerable<UserJobInfo> AllUsersJobInfo() {
//     //     string sql = @"
//     //         SELECT 
//     //             [UserId],
//     //             [JobTitle],
//     //             [Department]
//     //         FROM UserData.UserJobInfo
//     //     ";
//     //     return _dapper.LoadData<UserJobInfo>(sql);
//     // }


//     // [HttpGet("UserJobInfoSingle/{ID}")]

//     // public UserJobInfo UserJobInfoSingle(int ID) {
//     //     string sql = $@"
//     //         SELECT 
//     //             [UserId],
//     //             [JobTitle],
//     //             [Department]
//     //         FROM UserData.UserJobInfo 
//     //             WHERE UserId = {ID}
//     //     ";
//     //     return _dapper.LoadDataSingle<UserJobInfo>(sql);
//     // }


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


//     // [HttpPut("EditUserJobInfo")]
//     // public IActionResult EditUser(UserJobInfo user) {
//     //     string sql = $@"
//     //         UPDATE UserData.UserJobInfo
//     //             SET JobTitle = '{_func.SQLSingleEscape(user.JobTitle)}' , Department = '{_func.SQLSingleEscape(user.Department)}' 
//     //             WHERE UserId = {user.UserID};
//     //     ";
//     //     if(_dapper.ExecuteBool(sql)) {
//     //         return Ok();
//     //     }
//     //     throw new Exception("Something Went Wrong While Editing User's Job Info");
//     // }

//     // [HttpDelete("DeleteUserJobInfo/{userID}")]
//     // public IActionResult DeleteUserJobInfo(int userID) {
//     //     string sql = $"DELETE FROM UserData.UserJobInfo WHERE UserId = {userID}";
//     //     if(_dapper.ExecuteBool(sql)) {
//     //         return Ok();
//     //     } 
//     //     throw new Exception("Failed To Delete User's Job Info, You May Be Deleting A Non-Existing Row");
//     // }

//     // User SALARY CONTROLLER 
//     // [HttpGet("AllUsersSalary")]
//     // public IEnumerable<UserSalary> AllUsersSalary() {
//     //     string sql = @"
//     //         SELECT 
//     //             [UserId],
//     //             [Salary]
//     //         FROM UserData.UserSalary
//     //     ";
//     //     return _dapper.LoadData<UserSalary>(sql);
//     // }


//     // [HttpGet("UserSalary/{ID}")]

//     // public UserSalary UserSalary(int ID) {
//     //     string sql = $@"
//     //         SELECT 
//     //             [UserId],
//     //             [Salary]
//     //         FROM UserData.UserSalary 
//     //             WHERE UserId = {ID}
//     //     ";
//     //     return _dapper.LoadDataSingle<UserSalary>(sql);
//     // }


//     [HttpPost("AddUserSalary")]
//     public IActionResult AddUserSalary(UserSalaryDTO user) {
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


//     // [HttpPut("EditUserSalary")]
//     // public IActionResult EditUserSalary(UserSalary user) {
//     //     string sql = $@"
//     //         UPDATE UserData.UserSalary
//     //             SET Salary = '{user.Salary}'
//     //             WHERE UserId = {user.UserID};
//     //     ";
//     //     if(_dapper.ExecuteBool(sql)) {
//     //         return Ok();
//     //     }
//     //     throw new Exception("Something Went Wrong While Editing User's Salary");
//     // }

//     // [HttpDelete("DeleteUserSalary/{userID}")]
//     // public IActionResult DeleteUserSalary(int userID) {
//     //     string sql = $"DELETE FROM UserData.UserSalary WHERE UserId = {userID}";
//     //     if(_dapper.ExecuteBool(sql)) {
//     //         return Ok();
//     //     } 
//     //     throw new Exception("Failed To Delete User's Salary, You May Be Deleting A Non-Existing Row");
//     // }

// }
