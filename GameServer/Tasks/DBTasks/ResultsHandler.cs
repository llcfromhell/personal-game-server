using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace GameServer.Tasks.DBTasks
{
    public class ResultsHandler
    {
        public async Task DoWork()
        {
            await Task.Run(() => SaveResults());
        }

        private void SaveResults()
        {

            Debug.WriteLine(DateTime.Now + " | Starting to persist data on DB.");

            var context = new Context.ServerContext();          

            ObjectCache cache = MemoryCache.Default;

            var gameResults = new Result[0];

            lock (cache)
            {

                gameResults = cache.Get("gameResults") as Result[];
                cache.Remove("gameResults");

            }

            if (gameResults == null || gameResults.Length < 1)
            {
                Debug.WriteLine(DateTime.Now + " | No data to persist.");
                return;
            }

            gameResults.ToList().ForEach(result =>
            {
                context.Results.Add(result);
            });

            try
            {
                context.SaveChanges();

                Debug.WriteLine(DateTime.Now + " | Finished persisting data on DB.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " | Error while persisting: " + e.Message);
            }

            

        }
    }
}