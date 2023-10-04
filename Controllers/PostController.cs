using System.Data;
using API.Data;
using API.Helpers;
using API.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers ;
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase {
        private readonly DataContextDapper _dapper;
        private readonly PostHelper _postHelper;
        public PostController(IConfiguration config){
            _dapper = new DataContextDapper(config);
            _postHelper = new PostHelper();
        }
        
        [HttpGet("Posts/{PostId}/{UserId}/{SearchQuery}")]
        public IEnumerable<Post> GetPosts(int PostId = 0, int UserId = 0, string SearchQuery = "None") {
            string sqlGet = "EXEC UserData.spPost_Get ";
            DynamicParameters parameters = new DynamicParameters();
            string parametersString = "";
            if(PostId != 0) {
                parametersString += ",@PostId = @PostIdParameter"; 
                parameters.Add("@PostIdParameter" , PostId , DbType.Int32);
            }
            if(UserId != 0) {
                parametersString += ",@UserId = @UserIdParameter";
                parameters.Add("@UserIdParameter" , UserId , DbType.Int32);
            }
            if(SearchQuery != "None") {
                parametersString += ",@SearchQuery = @SearchQueryParameter";
                parameters.Add("@SearchQueryParameter" , SearchQuery , DbType.String);
            }
            if(parametersString.Length > 0) {
                sqlGet += parametersString.Substring(1);
            }
            return _dapper.LoadDataWithParams<Post>(sqlGet , parameters);
        } 

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetUserPosts() {
            string sqlGetSingle = $@"EXEC UserData.spPost_Get @UserId = @UserIdParameter";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(User.FindFirst("UserId")?.Value);
            return _dapper.LoadDataWithParams<Post>(sqlGetSingle , parameters);
        }  

        [HttpPut("UpsertPost")]
        public IActionResult UpsertPost(Post newPost) {
            string sqlNewPost = $@"EXEC UserData.spPost_Upsert 
                    @UserId = @UserIdParameter ,
                    @PostTitle = @PostTitleParameter ,
                    @PostContent = @PostContentParameter
            ";
            string optionalParam = $", @PostId = @PostIdParameter";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserIdParameter" , this.User.FindFirst("userId")?.Value , DbType.Int32);
            parameters.Add("@PostTitleParameter" , _postHelper.SQLSingleEscape(newPost.PostTitle) , DbType.String);
            parameters.Add("@PostContentParameter" , _postHelper.SQLSingleEscape(newPost.PostContent) , DbType.String);
            Console.WriteLine(sqlNewPost);
            if (newPost.PostId > 0) {
                parameters.Add("@PostIdParameter" , newPost.PostId , DbType.Int32);
                sqlNewPost += optionalParam;
            }
            Console.WriteLine(sqlNewPost);

            if(_dapper.ExecuteWithParams(sqlNewPost , parameters)) {
                return Ok();
            }
            throw new Exception("Failed To Add/Edit Post! :( ");
        } 

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId) {
            string sqlDeletePost = $"UserData.spPost_Delete @PostId = @PostIdParameter AND @UserId = @UserIdParameter";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PostIdParameter" , postId , DbType.Int32);
            parameters.Add("@UserId" , this.User.FindFirst("UserId")?.Value , DbType.Int32);
            if(_dapper.ExecuteWithParams(sqlDeletePost , parameters)) {
                return Ok();
            }
            throw new Exception("Failed To Delete Post! :( ");
        } 
    }
