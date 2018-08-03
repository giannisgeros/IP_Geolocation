using IP_Geolocation.Cache;
using IP_Geolocation.Exceptions;
using IP_Geolocation.Interfaces;
using IP_Geolocation.Models;
using IP_Geolocation.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IP_Geolocation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Get(IpViewModel inputModel)
        {

            if (ModelState.IsValid)
            {
                IpCache cache = new IpCache();

                IpViewModel ipViewModel;

                if (cache.TryGetFromCache(inputModel.IP, out IpViewModel model))
                {
                    ipViewModel = model;

                    return View("Index", ipViewModel);
                }


                //try ip2c Service
                int tries = 0;

                while(tries < 3)
                {
                    try
                    {
                        IService ip2cService = new Ip2cService();

                        ipViewModel = await ip2cService.TryGetIpViewModel(inputModel.IP);
                        ipViewModel.ProviderOrCacheUsed = "Ip2c";

                        return View("Index", ipViewModel);
                    }
                    catch (TaskCanceledException)
                    {
                        tries += 1;
                    }
                    catch (HttpStatusCodeNotOKException)
                    {
                        tries += 1;
                    }
                    catch (IpIsValidButDoesNotCorrespondToAnyCountryException)
                    {
                        ViewData["ErrorMessage"] = "This Ip is valid but does not correspond to any country";
                        return View("Index");
                    }
                    catch (Exception)
                    {
                        tries += 1;
                    }
                }

                // try ipStack Service
                tries = 0;

                while(tries < 3)
                {
                    try
                    {
                        IService ipStackService = new IpStackService();

                        ipViewModel = await ipStackService.TryGetIpViewModel(inputModel.IP);
                        ipViewModel.ProviderOrCacheUsed = "IpStack";

                        return View("Index", ipViewModel);
                    }
                    catch (TaskCanceledException)
                    {
                        tries += 1;
                    }
                    catch (HttpStatusCodeNotOKException)
                    {
                        tries += 1;
                    }
                    catch (IpIsValidButDoesNotCorrespondToAnyCountryException)
                    {
                        ViewData["ErrorMessage"] = "This Ip is valid but does not correspond to any country";
                        return View("Index");
                    }
                    catch (Exception)
                    {
                        tries += 1;
                    }
                }

                // this far all have failed so return null model
                ViewData["ErrorMessage"] = "Services Failed";
                return View("Index");
            }
            else
            {
                return View("Index");
            }

        }

        [HttpPost]
        public async Task<JsonResult> GetJson(IpViewModel inputModel)
        {

            if (ModelState.IsValid)
            {
                IpCache cache = new IpCache();

                IpViewModel ipViewModel;

                if (cache.TryGetFromCache(inputModel.IP, out IpViewModel model))
                {
                    ipViewModel = model;

                    return Json(ipViewModel);
                }


                //try ip2c Service
                int tries = 0;

                while (tries < 3)
                {
                    try
                    {
                        IService ip2cService = new Ip2cService();

                        ipViewModel = await ip2cService.TryGetIpViewModel(inputModel.IP);
                        ipViewModel.ProviderOrCacheUsed = "Ip2c";

                        return Json(ipViewModel);
                    }
                    catch (TaskCanceledException)
                    {
                        tries += 1;
                    }
                    catch (HttpStatusCodeNotOKException)
                    {
                        tries += 1;
                    }
                    catch (IpIsValidButDoesNotCorrespondToAnyCountryException)
                    {
                        return Json("This Ip is valid but does not correspond to any country");
                    }
                    catch (Exception)
                    {
                        tries += 1;
                    }
                }

                // try ipStack Service
                tries = 0;

                while (tries < 3)
                {
                    try
                    {
                        IService ipStackService = new IpStackService();

                        ipViewModel = await ipStackService.TryGetIpViewModel(inputModel.IP);
                        ipViewModel.ProviderOrCacheUsed = "IpStack";

                        return Json(ipViewModel);
                    }
                    catch (TaskCanceledException)
                    {
                        tries += 1;
                    }
                    catch (HttpStatusCodeNotOKException)
                    {
                        tries += 1;
                    }
                    catch (IpIsValidButDoesNotCorrespondToAnyCountryException)
                    {
                        return Json("This Ip is valid but does not correspond to any country");
                    }
                    catch (Exception)
                    {
                        tries += 1;
                    }
                }

                // this far all have failed so return null model
                return Json("Services Failed");
            }
            else
            {
                return Json("Invalid Ip");
            }

        }
    }
}