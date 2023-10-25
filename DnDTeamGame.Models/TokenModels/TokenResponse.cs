namespace DnDTeamGame.Models.TokenModels;

public class TokenResponse
{
    public in UserId {get; set;}
    public string Token { get; set; } = null!;
    public DateTime IssuedAt { get; set; }
    public DateTime Expires { get; set; }
}
