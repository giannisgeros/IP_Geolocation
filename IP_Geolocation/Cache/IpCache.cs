using IP_Geolocation.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IP_Geolocation.Cache
{
    public sealed class IpCache
    {
        private static Lazy<ConcurrentDictionary<string, IpViewModel>> _Cache = new Lazy<ConcurrentDictionary<string, IpViewModel>>(() => new ConcurrentDictionary<string, IpViewModel>());
        private static Lazy<System.Timers.Timer> _MyTimer = new Lazy<System.Timers.Timer>(() => new System.Timers.Timer());

        public IpCache()
        {
            if (_MyTimer.Value.Enabled != true)
            {
                TimerInitializer();
            }     
        }
        public bool TryGetFromCache(string ip, out IpViewModel model)
        {
            model = null;

            if (_Cache.Value.TryGetValue(ip, out IpViewModel cacheModel))
            {
                model = cacheModel;
                model.ProviderOrCacheUsed = "Cache";
                return true;
            }

            return false;

        }

        public void AddToCache(IpViewModel model)
        {
            model.DateInserted = DateTime.Now;

            _Cache.Value.TryAdd(model.IP, model);
        }

        // Timer
        private static void TimerInitializer()
        {          
            _MyTimer.Value.Interval = 3000;
            _MyTimer.Value.Elapsed += ClearCache;
            _MyTimer.Value.AutoReset = true;
            _MyTimer.Value.Enabled = true;
        }

        private static void ClearCache(Object source, System.Timers.ElapsedEventArgs e)
        {
            // remove the ip from cache after 3 secs      
            if (_Cache != null)
            {
                string[] keys = _Cache.Value.Keys.ToArray();
                IpViewModel[] values = _Cache.Value.Values.ToArray();
                for (int i = 0; i < values.Length; i++)
                {
                    TimeSpan difference = DateTime.Now - values[i].DateInserted;
                    if (difference.Seconds > 3)
                    {
                        _Cache.Value.TryRemove(keys[i], out IpViewModel value);
                    }
                }
            }
        }
    }
}