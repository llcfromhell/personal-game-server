using GameServer.Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace GameServer.Tasks.DBTasks
{
    public class LeaderboardHandler
    {
        public async Task DoWork()
        {
            await Task.Run(() => UpdateLeaderboard());
        }

        private void UpdateLeaderboard()
        {
            try
            {

                Debug.WriteLine(DateTime.Now + " | Starting updating the leaderboard");

                var context = new Context.ServerContext();

                var playerResults = context.Results.GroupBy(grp => grp.playerId).Select(p => new { player = p.Key, results = p });

                int[] players = context.Results.Select(p => p.playerId).Distinct().ToArray();

                var leaderboard = playerResults.Select(p => new
                {
                    player = p.player,
                    lastUpdate = p.results.OrderByDescending(r => r.timestamp).Select(r => r.timestamp).FirstOrDefault(),
                    balance = p.results.OrderBy(r => r.timestamp).Sum(r => r.win)
                })
                .OrderByDescending(p => p.balance)
                .Take(100)
                .ToArray();

                ObjectCache cache = MemoryCache.Default;
                cache.Remove("leaderboard");
                cache.Add("leaderboard", leaderboard, MemoryCache.InfiniteAbsoluteExpiration);

                Debug.WriteLine(DateTime.Now + " | Finshed updating the leaderboard");
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " | Error while updating Leaderboard: " + e.Message);
            }
        }
    }
}