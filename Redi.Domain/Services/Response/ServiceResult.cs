namespace Redi.Domain.Services.Response
{
    public class ServiceResult
    {
        public List<string> Errors { get; set; }
        public bool Success => !Errors.Any();
    }
}
