// using System.Text.Json;
// using API.Data;
// using Microsoft.AspNetCore.Mvc;

// namespace API.Controllers;


// [ApiController]
// [Route("[controller]")]
// public class DB : ControllerBase
// {
//     DataContextDapper _dapper;
//     public DB(IConfiguration config) {
//          _dapper = new DataContextDapper(config);
//     }
    
//     // IConfiguration cnfg = new ConfigurationBuilder()
//     // .AddJsonFile("appsettings.json")
//     // .Build();
//     // Console.WriteLine(config.GetConnectionString("DefaultConnection"));
//     // DataContextDapper uploadDapper = new DataContextDapper(cnfg);
//     // [HttpGet("upload")]
//     // public void UploadData() {
//     //     string usersJson = System.IO.File.ReadAllText("UsersData/Users.json");
//     //     // Console.WriteLine(usersJson);
//     //     IEnumerable<Users> UsersData = JsonSerializer.Deserialize<IEnumerable<Users>>(usersJson);
//     //     foreach(Users user in UsersData) {
//     //         // Console.WriteLine(user.FirstName);
//     //     }
//     //     string sqlInsert = @"SELECT GETDATE()";
//     //     _dapper.ExecuteWithRowCount(sqlInsert);
//     // }

//     [HttpGet("Getdate")]
//     public DateTime GetDate() {
//         // DateTime Date = _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
//         return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
//     }

    
//     [HttpGet("ConvertCelsius/{celsiusTemperature}")]
//     public string Get(int celsiusTemperature)
//     {
//         return ((celsiusTemperature * 9/5) + 32).ToString() + " F";
//     }

// }
