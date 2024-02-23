namespace Redi.Domain.Services.Response
{
    public class ServiceResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool Success => !Errors.Any();
    }
}
