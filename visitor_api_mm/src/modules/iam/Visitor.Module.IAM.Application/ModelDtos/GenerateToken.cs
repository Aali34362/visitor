namespace Visitor.Module.IAM.Application.ModelDtos;

public class GenerateTokenCommand
{
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
    public string GrantType { get; init; } = null!; // "client_credentials", "password", "refresh_token"
    public string Username { get; init; }
    public string Password { get; init; }
    public string RefreshToken { get; init; }    
}
