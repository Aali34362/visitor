namespace Visitor.Module.IAM.Application.ResponseDtos;

public class TokenResponse
{
    public string Access_Token { get; set; }
    public short Expires_In { get; set; }
    public string Refresh_Token { get; set; }
    public short Refresh_Expires_In { get; set; }
    public string Token_Type { get; set; }
}
