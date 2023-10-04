using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers {
    public class AuthHelper {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        public AuthHelper(IConfiguration config) {
            _config = config;
            _dapper = new DataContextDapper(config);
        }
        

        public byte[] GetHashedPassword(string password , byte[] salt) {
        // SALT SHOULD BE CONVERTED 
        string saltAndStr = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(salt);
        return KeyDerivation.Pbkdf2(
            password ,
            Encoding.ASCII.GetBytes(saltAndStr) ,
            KeyDerivationPrf.HMACSHA256 ,
            1000000,
            256/8
        );

        }
        public string CreateToken(int UserId) {
            Claim[] claims = new Claim[] {
                new Claim("UserId" , UserId.ToString())
            };
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:symKey").Value)
            );
            SigningCredentials credentials = new SigningCredentials(
                symmetricKey ,
                SecurityAlgorithms.HmacSha512Signature
            );
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials ,
                Expires = DateTime.Now.AddDays(1)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(securityToken);        
        }
        public bool SetPassword(UserLoginDTO UserLoginInfo , string type = "register") {
            byte[] passwordSalt = new byte[128 / 8];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
                    rng.GetNonZeroBytes(passwordSalt);
                }
                byte[] passwordHashed = GetHashedPassword(UserLoginInfo.Password , passwordSalt);
                string sqlAddAuth = "";
                if (type == "update") {
                    sqlAddAuth = @$"EXEC UserData.spAuth_UpadtePass
                                        @Email = @EmailParam, 
                                        @PasswordHash = @PasswordHashParam, 
                                        @PasswordSalt = @PasswordSaltParam";
                } else { 
                    sqlAddAuth = @$"EXEC UserData.spAuth_Upsert 
                                        @Email = @EmailParam, 
                                        @PasswordHash = @PasswordHashParam, 
                                        @PasswordSalt = @PasswordSaltParam";
                }

                    // List<SqlParameter> sqlParamsList = new List<SqlParameter>();

                    // SqlParameter emailParam = new SqlParameter("@EmailParam" , SqlDbType.VarChar);
                    // emailParam.Value = UserLoginInfo.Email;
                    
                    // SqlParameter passParam = new SqlParameter("@PasswordHashParam" , SqlDbType.VarBinary);
                    // passParam.Value = passwordHashed;

                    // SqlParameter saltParam = new SqlParameter("@PasswordSaltParam" , SqlDbType.VarBinary);
                    // saltParam.Value = passwordSalt;
                    
                    // sqlParamsList.Add(emailParam);
                    // sqlParamsList.Add(passParam);
                    // sqlParamsList.Add(saltParam);

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmailParam" , UserLoginInfo.Email , DbType.String);
                    parameters.Add("@PasswordHashParam" , passwordHashed , DbType.Binary);
                    parameters.Add("@PasswordSaltParam" , passwordSalt , DbType.Binary);
                    return _dapper.ExecuteWithParams(sqlAddAuth , parameters);
        }
    }
}