using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace asp_webapi.Middleware
{
  public class Message
  {
    private RequestDelegate _next;

    public Message(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      String correlationId = context.Request.Headers["correlationId"];
      log4net.ThreadContext.Properties["correlationId"] = correlationId;
      log4net.GlobalContext.Properties["service"] = "BusinessInteraction";

      await _next.Invoke(context);
    }
  }
}
