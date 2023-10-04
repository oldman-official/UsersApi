using System.Data;
using API.Data;
using API.DTOs;
using API.Helpers;
using API.Models;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers ;
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase {

        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        private readonly ReusableSql _reusableSql;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration config) {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
            _reusableSql = new ReusableSql(config);
            _mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.CreateMap<UserRegisterationDTO , CompleteUserInfo>();
            }));
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult RegisterUser(UserRegisterationDTO UserInfo) {
            if (UserInfo.Password == UserInfo.PasswordConfirm){
                    string sqlEmailCheck = $"SELECT Email FROM UserData.Users WHERE Email = '{UserInfo.Email}'";
                    IEnumerable<string> ReturnedEmails = _dapper.LoadData<string>(sqlEmailCheck);
                    if(ReturnedEmails.Count() == 0) {
                            UserLoginDTO UserEmailPass = new UserLoginDTO() {
                                Email = UserInfo.Email ,
                                Password = UserInfo.Password
                            };
                            if(_authHelper.SetPassword(UserEmailPass)) {
                                CompleteUserInfo completeUserInfo = _mapper.Map<UserRegisterationDTO , CompleteUserInfo>(UserInfo);
                                completeUserInfo.Active = true;
                                if(_reusableSql.UpsertUser(completeUserInfo)) {
                                    return Ok();
                                }
                                throw new Exception("Failed to add user.");
                            }
                            throw new Exception("Failed to Register user.");
                    }
                throw new Exception("This Email Already Exists!");
            }
            throw new Exception("Your Passwords Don't Match!");
        }

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserLoginDTO UserToResetPassword) {
            string sqlGetEmailsId = $"SELECT UserId FROM UserData.Users WHERE Email ='{UserToResetPassword.Email}'";
            int JwtUserId = _dapper.LoadDataSingle<int>(sqlGetEmailsId);
            if(Convert.ToInt32(User.FindFirst("UserId")?.Value) == JwtUserId) {
                if(_authHelper.SetPassword(UserToResetPassword , "update")) {
                    return Ok();
                } 
            }
            throw new Exception("Failed To Reset Password");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginUser(UserLoginDTO userLoginInfo) {

            string sqlUserAuth = $"EXEC UserData.spAuth_Get @Email = @EmailParam";
            DynamicParameters sqlParams = new();
            sqlParams.Add("@EmailParam" , userLoginInfo.Email , DbType.String);
            UserLoginConfirmationDto userDbInfo = _dapper.LoadDataSingleWithParams<UserLoginConfirmationDto>(sqlUserAuth , sqlParams);

            string sqlUserId = $"SELECT [UserId] From UserData.Users WHERE Email = '{userLoginInfo.Email}'";
            int UserId = _dapper.LoadDataSingle<int>(sqlUserId);

            byte[] inputedPass = _authHelper.GetHashedPassword(userLoginInfo.Password , userDbInfo.PasswordSalt);
            for(int index = 0 ; index < inputedPass.Length ; index++) {
                if(inputedPass[index] != userDbInfo.PasswordHash[index]) {
                    return StatusCode(401 , "Incorrect Password!");
                }
            }
            return Ok(
                new Dictionary<string , string> {
                    {"Token" , _authHelper.CreateToken(UserId)}
                }
            );
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken() {
            string sqlGetId = $"SELECT [UserId] From UserData.Users WHERE UserId = '{User.FindFirst("UserId")?.Value}'";
            int UserId = _dapper.LoadDataSingle<int>(sqlGetId);
            return _authHelper.CreateToken(UserId);
        }
        

    
} 
