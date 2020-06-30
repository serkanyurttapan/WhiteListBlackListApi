using CoreSecurity.Web.MiddleWares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;

namespace CoreSecurity.Web.Filters
{
    public class CheckWhiteList : ActionFilterAttribute
    {
        private readonly IPList _ipList;

        public CheckWhiteList(IOptions<IPList> ipList)
        {
            _ipList = ipList.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var requestIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var isWhiteList = this._ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(requestIpAddress)).Any();

            if (!isWhiteList)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
