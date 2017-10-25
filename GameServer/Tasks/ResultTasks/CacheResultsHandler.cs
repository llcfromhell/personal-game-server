using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace GameServer.Tasks.ResultTasks
{
    public class CacheResultsHandler
    {
        private Result[] newResults;

        public static async Task Store(Result[] newResults)
        {
            await Task.Run(() => CacheResults(newResults));
        }

        private static void CacheResults(Result[] newResults)
        {

            Debug.WriteLine(DateTime.Now + " | Caching results.");

            ObjectCache cache = MemoryCache.Default;
            
            Result[] results = new Result[0];

            lock (cache)
            {

                try
                {

                    var cachedGameResults = cache.Get("gameResults") as Result[];

                    if (cachedGameResults == null)
                    {

                        results = new Result[newResults.Length];
                        newResults.CopyTo(results, 0);

                    }
                    else
                    {

                        results = new Result[(cachedGameResults.Length + newResults.Length)];
                        newResults.CopyTo(results, 0);
                        cachedGameResults.CopyTo(results, newResults.Length);

                    }

                }
                catch (Exception e) {
                    Debug.WriteLine(DateTime.Now + " | Error while chaching: " + e.Message);
                }
                finally
                {
                    cache.Remove("gameResults");
                    cache.Add("gameResults", results, MemoryCache.InfiniteAbsoluteExpiration);

                }

            }

            Debug.WriteLine(DateTime.Now + " | Finished caching.");

        }
    }
}