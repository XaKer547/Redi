using System.Text.Json.Serialization;

namespace Redi.Domain.Models.Delivery
{
    public class DeliveryStatus
    {
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }

        [JsonIgnore]
        public bool IsCompleted => CreatedDate.HasValue;
    }
}
