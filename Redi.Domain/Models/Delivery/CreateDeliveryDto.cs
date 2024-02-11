﻿using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.Models.Delivery
{
    public class CreateDeliveryDto
    {
        [Required]
        [RegularExpression(@"^R-\d{4}-\d{4}-\d{4}-\d{4}$")]
        public string TrackNumber { get; set; }

        [Required]
        public string OriginAddress { get; set; }

        [Required]
        public string OriginStateCountry { get; set; }

        [Required]
        public string OriginPhoneNumber { get; set; }
        public string OriginOthers { get; set; }

        [Required]
        public string DestinationAddress { get; set; }

        [Required]
        public string DestinationStateCountry { get; set; }

        [Required]
        public string DestinationPhoneNumber { get; set; }
        public string DestinationOthers { get; set; }

        [Required]
        public string PackageName { get; set; }

        [Required]
        public string PackageWeight { get; set; }

        [Required]
        public float WorthOfItems { get; set; }

        [Required]
        public bool IsInstantDelivery { get; set; }

        [Required]
        public float DeliveryCharges { get; set; }

        [Required]
        public float Taxes { get; set; }
    }
}
