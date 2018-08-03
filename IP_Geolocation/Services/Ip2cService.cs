using IP_Geolocation.Cache;
using IP_Geolocation.Exceptions;
using IP_Geolocation.Interfaces;
using IP_Geolocation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IP_Geolocation.Services
{
    public class Ip2cService : IService
    {
        public async Task<IpViewModel> TryGetIpViewModel(string ip)
        {
            IpViewModel model;

            // get the ip2c url from web config file
            string firstPartUrl = System.Configuration.ConfigurationManager.AppSettings["ip2cUrl"];

            // create the url
            string finalUrl = firstPartUrl + ip;

            // get the result from ip2c api
            string textResult;

            using (var client = new HttpClient())
            {
                Uri uri = new Uri(finalUrl);
                client.Timeout = TimeSpan.FromMilliseconds(1000);

                HttpResponseMessage response = await client.GetAsync(uri);        
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpStatusCodeNotOKException();
                }

                textResult = await response.Content.ReadAsStringAsync();
                if (textResult.Split(';')[0] == "0")
                {
                    throw new IpIsValidButDoesNotCorrespondToAnyCountryException();
                }
            }

            // split it to get the actual data
            string[] splitTextResult = textResult.Split(';');

            // create the model to return
            model = new IpViewModel
            {
                IP = ip,
                Country = splitTextResult[3],
                TwoLettersCode = splitTextResult[1]
            };

            return model;

        }
    }
}