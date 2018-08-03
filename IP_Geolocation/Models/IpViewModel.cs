using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP_Geolocation.Models
{
    public class IpViewModel
    {
        [Required]
        [RegularExpression(@"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}\b", ErrorMessage = "Please insert a valid Ip address.")]
        public string IP { get; set; }

        public string Country { get; set; }

        public string TwoLettersCode { get; set; }

        public string ProviderOrCacheUsed { get; set; }

        public DateTime DateInserted { get; set; }

    }

}