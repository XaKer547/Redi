using System.ComponentModel.DataAnnotations;

namespace Redi.DataAccess.Data
{
    public enum DeliveryStatuses
    {
        [Display(Name = "Courier requested")]
        Requested,

        [Display(Name = "Package ready for delivery")]
        ReadyForDelivery,

        [Display(Name = "Package in transit")]
        InTransit,

        [Display(Name = "Package delivered")]
        Delivered
    }
}
