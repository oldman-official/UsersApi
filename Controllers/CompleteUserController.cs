using System.Data;
using API.Data;
using API.Helpers;
using API.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CompleteUserController : ControllerBase
{
    private readonly DataContextDapper _dapper ; 
    private readonly ReusableSql _reusableSql;
    public CompleteUserController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
        _reusableSql = new ReusableSql(config);
    }
    [HttpGet("GetUser/{UserId}/{isActive}")]
    public IEnumerable<CompleteUserInfo> GetUsers(int UserId , bool isActive) {
        string sql = @"EXEC UserData.spGetAllUsers";
        string paramsString = "";
        DynamicParameters parameters = new DynamicParameters(); 
        if (UserId != 0) {
            paramsString += ", @UserId = @UserIdParam";
            parameters.Add("@UserIdParam" , UserId , DbType.Int32);
        }
        if (isActive) {
            paramsString += ", @Active = @ActiveParam";
            parameters.Add("@ActiveParam" , isActive , DbType.Boolean);

        }
        if (paramsString.Length > 1) {
            sql += paramsString.Substring(1);
        }
        //? Should we change the way false works? User 17 with active = false will return results
        Console.WriteLine(sql);
        return _dapper.LoadDataWithParams<CompleteUserInfo>(sql , parameters);
    }
    

    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(CompleteUserInfo user) {
        if(_reusableSql.UpsertUser(user)) {
            return Ok();
        }
        throw new Exception("Something Went Wrong While Adding/Editing User");
    }

    [HttpDelete("DeleteUser/{userID}")]
    public IActionResult DeleteUser(int userID) {
        string sql = $"EXEC UserData.spUser_Delete @UserId = @UserIdParam";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@UserIdParam" , userID , DbType.Int32);
        if(_dapper.ExecuteWithParams(sql , parameters)) {
            return Ok();
        } 
        throw new Exception("Failed To Delete User, You May Be Deleting A Non-Existing Row");
    }

}
