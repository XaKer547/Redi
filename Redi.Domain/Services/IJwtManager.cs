namespace Redi.Domain.Services
{
    public interface IJwtManager
    {
        Task<string> GenerateTokenAsync();
    }
}
