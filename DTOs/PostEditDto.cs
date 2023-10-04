namespace API.DTOs;

public partial class PostEditDto {
    public int PostId {get; set;}
    public string PostTitle {get; set;}
    public string PostContent {get; set;}
    public PostEditDto() {
        if (PostTitle == null) {
            PostTitle = "";
        }
        if (PostContent == null) {
            PostContent = "";
        }
        
    }
}