using GameServer.Tasks;
using GameServer.Tasks.DBTasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Configuration;

namespace GameServer.App_Start
{
    public class TasksConfig
    {

        public static void initTasks() {

            var persistDataIntervalInSeconds = ConfigurationManager.AppSettings["PersistDataIntervalInSeconds"];

            int secondsForInterval = 0;

            int.TryParse(persistDataIntervalInSeconds, out secondsForInterval);

            double defaultInterval = TimeSpan.FromSeconds(10).TotalMilliseconds;

            double interval = secondsForInterval > 0 ? TimeSpan.FromSeconds(secondsForInterval).TotalMilliseconds : defaultInterval;

            Debug.WriteLine(DateTime.Now + " | Configured data persist interval for => " + interval + " milliseconds.");

            Timer t = new Timer(interval)
            {
                AutoReset = true
            };
            t.Elapsed += new System.Timers.ElapsedEventHandler(callSaveChanges);
            t.Start();

        }

        private static void callSaveChanges(object sender, ElapsedEventArgs e)
        {
            LeaderboardHandler leaderboardHandler = new LeaderboardHandler();
            ResultsHandler resultsHandler = new ResultsHandler();

            resultsHandler
                .DoWork()
                .ContinueWith(async (prevTask) => { await leaderboardHandler.DoWork(); });
        }

    }
}