using GameServer.Models;
using GameServer.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace GameServer.Controllers
{
    public class LeaderboardController : ApiController
    {
        public Object[] Get() {
            
            ObjectCache cache = MemoryCache.Default;

            var leaderboard = cache.Get("leaderboard") as Object[];

            if (leaderboard != null)
            {
                return leaderboard;
            }
            else
            {
                return new Object[0];
            }
                        
        }
    }
}