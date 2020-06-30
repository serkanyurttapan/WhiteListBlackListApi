using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace CoreSecurity.Web.MiddleWares
{
    public class IPSafeMiddleWare
    {
        private readonly RequestDelegate _next;

        private readonly IPList _list;


        public IPSafeMiddleWare(RequestDelegate next, IOptions<IPList> list)
        {
            _next = next;
            _list = list.Value;

        }
        public async Task Invoke(HttpContext context)
        {

            IPAddress requestAdress = context.Connection.RemoteIpAddress;
            bool isWhiteList = _list.WhiteList.Where(x => IPAddress.Parse(x).Equals(requestAdress)).Any();
            if (!isWhiteList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next(context);
        }

    }
}
