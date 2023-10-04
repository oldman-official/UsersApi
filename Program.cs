using System.Text;
using API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddPolicy("DevCors" , (corsBuilder) => {
        corsBuilder.WithOrigins("http://localhost:8000" ,"http://localhost:4200" ,"http://localhost:3000")
        .AllowAnyHeader() 
        .AllowAnyMethod()
        .AllowCredentials();
    });
    options.AddPolicy("ProdCors" , (corsBuilder) => {
        corsBuilder.WithOrigins("https://prowebsite.ir")
        .AllowAnyHeader() 
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

IConfiguration cnfg = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
    Console.WriteLine(cnfg.GetConnectionString("DefaultConnection"));
    DataContextDapper uploadDapper = new DataContextDapper(cnfg);
        // Functions _func = new();
        // string usersJson = File.ReadAllText("UsersData/Users.json");
        // Console.WriteLine(usersJson);
        // IEnumerable<User> UsersData = JsonSerializer.Deserialize<IEnumerable<User>>(usersJson);
        // foreach(User user in UsersData) {
            // Console.WriteLine();
        //     string sqlInsert = @"INSERT INTO UserData.Users VALUES(" + 
        //     "'" + _func.SQLSingleEscape(user.FirstName) + "','" + 
        //     _func.SQLSingleEscape(user.LastName) + "','" + 
        //     _func.SQLSingleEscape(user.Email) + "','" + 
        //     _func.SQLSingleEscape(user.Gender) + "','" + 
        //     user.Active + "')";
        //     uploadDapper.ExecuteBool(sqlInsert);
            // Console.WriteLine(ReplaceSingleQuote("Sa'ds"));
        // }

        // string salaryJson = File.ReadAllText("UsersData/UserSalary.json");
        // IEnumerable<UserSalary> UsersSalary = JsonSerializer.Deserialize<IEnumerable<UserSalary>>(salaryJson);
        // foreach(UserSalary user in UsersSalary) {
            // Console.WriteLine();
        //     string sqlInsert = @"INSERT INTO UserData.UserSalary VALUES(" + 
        //     "'" + user.UserId + "','" + 
        //     user.Salary + "')";
        //     uploadDapper.ExecuteBool(sqlInsert);
            // Console.WriteLine(ReplaceSingleQuote("Sa'ds"));
        // }
        
        // string jobJson = File.ReadAllText("UsersData/UserJobInfo.json");
        // IEnumerable<UserJobInfo> UserJobJson = JsonSerializer.Deserialize<IEnumerable<UserJobInfo>>(jobJson);
        // foreach(UserJobInfo user in UserJobJson) {
            // Console.WriteLine();
        //     string sqlInsert = @"INSERT INTO UserData.UserJobInfo VALUES(" + 
        //     "'" + user.UserId + "','" + 
        //     ReplaceSingleQuote(user.JobTitle) + "','" + 
        //     ReplaceSingleQuote(user.Department) + "')";
        //     uploadDapper.ExecuteBool(sqlInsert);
            // Console.WriteLine(ReplaceSingleQuote("Sa'ds"));
        // }

// string ReplaceSingleQuote(string str) {
//     return str.Replace("'" , "''");
// }
    

// JWT Validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters() {
            ValidateIssuerSigningKey = true ,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:symKey").Value)),
            ValidateIssuer = false ,
            ValidateAudience = false
        };
    });


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCors");
} else {
    app.UseHttpsRedirection();
    app.UseCors("ProdCors");
}



// Order Of Authes IS IMPORTANT 
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
