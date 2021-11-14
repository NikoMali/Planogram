using System.Text.Json.Serialization;

namespace Planograma.EmplUser.Application.Models.Users
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(int id, string Username, string jwtToken, string refreshToken)
        {
            this.Id = id;
            this.Username = Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}