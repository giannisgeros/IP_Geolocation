using IP_Geolocation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Geolocation.Interfaces
{
    interface IService
    {
        Task<IpViewModel> TryGetIpViewModel(string ip);
    }
}
