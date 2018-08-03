using IP_Geolocation.Exceptions;
using IP_Geolocation.Interfaces;
using IP_Geolocation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace IP_Geolocation.Services
{
    public class IpStackService : IService
    {
        public async Task<IpViewModel> TryGetIpViewModel(string ip)
        {

            IpViewModel model;

            // get the url and api key from the web config file
            string firstPartUrl = System.Configuration.ConfigurationManager.AppSettings["ipStackUrl"];
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["ipStackApiKey"];

            // create the url
            string finalUrl = firstPartUrl + ip + "?access_key=" + apiKey;

            // get the result from the ipStack api
            string jsonResult;

            using (var client = new HttpClient())
            {
                Uri uri = new Uri(finalUrl);
                client.Timeout = TimeSpan.FromMilliseconds(2000);

                HttpResponseMessage response = await client.GetAsync(uri);                      
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpStatusCodeNotOKException();
                }

                jsonResult = await response.Content.ReadAsStringAsync();

                // convert json object to c# annonymous object
                dynamic data = Json.Decode(jsonResult);
                if (data.country_name == null || data.country_name == "UNKOWN" || data.country_name == "Reserved")
                {
                    throw new IpIsValidButDoesNotCorrespondToAnyCountryException();
                }

                // crete the model to return
                model = new IpViewModel
                {
                    IP = ip,
                    TwoLettersCode = data.country_code,
                    Country = data.country_name
                };

                return model;

            }
            
            
        }
    }
}