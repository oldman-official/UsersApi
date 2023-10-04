namespace API.Models ;
public partial class Functions {
    public string SQLSingleEscape(string str) {
        return str.Replace("'" , "''");
    }
}