namespace Redi.Api.Models
{
    public class RequestInfo
    {
        public string Ip { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location => $"{Country}, {City}";
    }
}
