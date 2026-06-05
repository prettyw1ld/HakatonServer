namespace HakatonServer.Dtos
{
    public class UserDto
    {
        public record UserCreate(string Email, string Password, string Name, string Lastname);
        public record UserLogin(string Email, string Password);
    }
}
