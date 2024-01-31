namespace Redi.Api.Infrastructure.Interfaces
{
    public interface IJwtGenerator
    {
        public string CreateToken(string userId, params string[] roles);
    }
}
