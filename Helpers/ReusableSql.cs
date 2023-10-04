using System.Data;
using API.Data;
using API.Models;
using Dapper;

namespace API.Helpers;
public class ReusableSql {
    private readonly DataContextDapper _dapper;
    public ReusableSql(IConfiguration config) {
        _dapper = new DataContextDapper(config);
    }
    public bool UpsertUser(CompleteUserInfo user) {

        string sql = $@"
            EXEC UserData.spUser_Upsert
                @FirstName = @FirstNameParam , @LastName = @LastNameParam , @Email = @EmailParam , @Gender = @GenderParam , @Active = @ActiveParam
                , @JobTitle = @JobTitleParam , @Department = @DepartmentParam , @Salary = @SalaryParam 
        ";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@FirstNameParam" , user.FirstName , DbType.String);
        parameters.Add("@LastNameParam" , user.LastName , DbType.String);
        parameters.Add("@EmailParam" , user.Email , DbType.String);
        parameters.Add("@GenderParam" , user.Gender , DbType.String);
        parameters.Add("@ActiveParam" , user.Active , DbType.Boolean);
        parameters.Add("@JobTitleParam" , user.JobTitle , DbType.String);
        parameters.Add("@DepartmentParam" , user.Department , DbType.String);
        parameters.Add("@SalaryParam" , user.Salary , DbType.Int32);

        if (user.UserId > 0) {
            sql += ", @UserId = @UserIdParam";
            parameters.Add("@UserIdParam" , user.UserId , DbType.String);
        }
        return _dapper.ExecuteWithParams(sql , parameters);
    }
    
}