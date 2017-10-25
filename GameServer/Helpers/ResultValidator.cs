using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GameServer.Helpers
{
    public class ResultValidator : IDataValidator<Result>
    {
        bool IDataValidator<Result>.IsValid(Result result)
        {
            if (result.gameId <= 0)
            {
                return false;
            }

            if (result.playerId <= 0)
            {
                return false;
            }

            try
            {
                DateTime.ParseExact(result.timestamp, "yyyy-MM-dd'T'HH:mm:ss'Z'", null);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (FormatException e)
            {
                return false;
            }

            return true;
        }
    }
}