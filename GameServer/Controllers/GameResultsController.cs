using GameServer.Models;
using GameServer.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using GameServer.Tasks.DBTasks;
using GameServer.Tasks.ResultTasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using GameServer.Helpers;

namespace GameServer.Controllers
{
    public class GameResultsController : ApiController
    {

        IDataValidator<Result> validator = new ResultValidator();

        // GET: GameResults
        public HttpResponseMessage Post([FromBody] ResultWrapper data)
        {

            if (IsNotValid(data))
            {
                Debug.WriteLine(DateTime.Now + " | Invalid data.");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Task.Run(() => CacheResultsHandler.Store(data.gameResults));

            Debug.WriteLine(DateTime.Now + " | Results sent to cache.");

            return Request.CreateResponse(HttpStatusCode.OK);

        }

        private bool IsNotValid(ResultWrapper data)
        {
            if (data == null)
            {
                return true;
            }

            if (data.gameResults == null)
            {
                return true;
            }

            if (data.gameResults.Length <= 0)
            {
                return true;
            }

            foreach (Result result in data.gameResults)
            {
                if (!validator.IsValid(result))
                {
                    return true;
                }
            };

            return false;
        }

        public class ResultWrapper {
            public Result[] gameResults { get; set; }
        }
    }
}