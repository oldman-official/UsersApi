// using API.Data;
// using API.DTOs;
// using API.Helpers;
// using API.Models;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace API.Controllers ;
//     [Authorize]
//     [ApiController]
//     [Route("[controller]")]
//     public class PostController : ControllerBase {
//         private readonly DataContextDapper _dapper;
//         private readonly PostHelper _postHelper;
//         public PostController(IConfiguration config){
//             _dapper = new DataContextDapper(config);
//             _postHelper = new PostHelper();
//         }
//         [HttpGet("Posts/{PostId}/{UserId}/{SearchQuery}")]
//         public IEnumerable<Post> GetPosts(int PostId = 0, int UserId = 0, string SearchQuery = "None") {
//             string sqlGet = "EXEC UserData.spPost_Get ";
//             string parameters = "";
//             if(PostId != 0) {
//                 parameters += ",@PostId = " + PostId.ToString();
//             }
//             if(UserId != 0) {
//                 parameters += ",@UserId = " + UserId.ToString();
//             }
//             if(SearchQuery != "None") {
//                 parameters += ",@SearchQuery = " + SearchQuery ;
//             }
//             if(parameters.Length > 0) {
//                 sqlGet += parameters.Substring(1);
//             }
//             return _dapper.LoadData<Post>(sqlGet);
//         } 
//         // [HttpGet("PostSingle/{postId}")]
//         // public Post GetPostSingle(int postId) {
//         //     string sqlGetSingle = $@"
//         //         SELECT [PostId],
//         //             [UserId],
//         //             [PostTitle],
//         //             [PostContent],
//         //             [PostCreated],
//         //             [PostUpdated] 
//         //         FROM UserData.Posts WHERE PostId = {postId}
//         //     ";
//         //     return _dapper.LoadDataSingle<Post>(sqlGetSingle);
//         // }  
//         // [HttpGet("PostsByUser/{userId}")]
//         // public IEnumerable<Post> GetUsersPosts(int userId) {
//         //     string sqlUsersPosts = $@"
//         //         SELECT [PostId],
//         //             [UserId],
//         //             [PostTitle],
//         //             [PostContent],
//         //             [PostCreated],
//         //             [PostUpdated] 
//         //         FROM UserData.Posts WHERE UserId = {userId}
//         //     ";
//         //     return _dapper.LoadData<Post>(sqlUsersPosts);
//         // }  

//         [HttpGet("MyPosts")]
//         public IEnumerable<Post> GetUserPosts() {
//             string sqlGetSingle = $@"EXEC UserData.spPost_Get @UserId = {User.FindFirst("UserId")?.Value}";
//             return _dapper.LoadData<Post>(sqlGetSingle);
//         }  
//         // [HttpGet("SearchPosts/{searchQuery}")]
//         // public IEnumerable<Post> SearchPosts(string searchQuery) {
//         //     string sqlSearchPosts = $"SELECT * FROM UserData.Posts WHERE PostTitle LIKE '%{_postHelper.SQLSingleEscape(searchQuery)}%' OR PostContent LIKE '%{_postHelper.SQLSingleEscape(searchQuery)}%'";
//         //     return _dapper.LoadData<Post>(sqlSearchPosts);
//         // }

//         [HttpPut("UpsertPost")]
//         public IActionResult UpsertPost(Post newPost) {
//             string sqlNewPost = $@"EXEC UserData.spPost_Upsert @UserId = {this.User.FindFirst("userId")?.Value} ,
//                     @PostTitle = '{_postHelper.SQLSingleEscape(newPost.PostTitle)}' ,
//                     @PostContent = '{_postHelper.SQLSingleEscape(newPost.PostContent)}'
//             ";
//             string optionalParam = $", @PostId = {newPost.PostId}";
//             Console.WriteLine(sqlNewPost);
//             if (newPost.PostId > 0) {
//                 sqlNewPost += optionalParam;
//             }
//             Console.WriteLine(sqlNewPost);

//             if(_dapper.ExecuteBool(sqlNewPost)) {
//                 return Ok();
//             }
//             throw new Exception("Failed To Add/Edit Post! :( ");
//         } 

//         [HttpPut("Post")]
//         public IActionResult EditPost(PostEditDto editPost) {
//             string sqlEditPost = $@"
//                 UPDATE UserData.Posts SET 
//                     PostTitle = '{_postHelper.SQLSingleEscape(editPost.PostTitle)}' ,
//                     PostContent = '{_postHelper.SQLSingleEscape(editPost.PostContent)}' ,
//                     PostUpdated = GETDATE() 
//                 WHERE PostId = {editPost.PostId} AND 
//                 UserId = {this.User.FindFirst("UserId")?.Value}
//             ";

//             if(_dapper.ExecuteBool(sqlEditPost)) {
//                 return Ok();
//             }
//             throw new Exception("Failed To edit Post! :( ");
//         }

//         [HttpDelete("Post/{postId}")]
//         public IActionResult DeletePost(int postId) {
//             string sqlDeletePost = $"UserData.spPost_Delete @PostId = {postId} AND @UserId = {this.User.FindFirst("UserId")?.Value}";

//             if(_dapper.ExecuteBool(sqlDeletePost)) {
//                 return Ok();
//             }
//             throw new Exception("Failed To Delete Post! :( ");
//         } 






//     }
