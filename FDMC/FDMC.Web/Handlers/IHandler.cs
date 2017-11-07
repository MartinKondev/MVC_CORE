using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDMC.Web.Handlers
{
    public interface IHandler
    {
        int Order { get; }

        Func<HttpContext, bool> Condition { get; }

        RequestDelegate RequestHandler { get; }
    }
}
