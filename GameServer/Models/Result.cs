using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.Models
{
    public class Result
    {

        public int Id { get; set; }

        public int playerId { get; set; }        

        public int gameId { get; set; }

        public int win { get; set; }

        public String timestamp { get; set; }

    }
}