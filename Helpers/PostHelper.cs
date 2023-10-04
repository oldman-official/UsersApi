namespace API.Helpers ;

public partial class PostHelper {
    public string SQLSingleEscape(string str) {
        return str.Replace("'" , "''");
    }
}