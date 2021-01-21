﻿using System.Security.Claims;
using System.Web;
using MessagePack;
using ReCode.Cocoon.Legacy.Auth;

namespace ReCode.Cocoon.Legacy.Auth
{
    public class AuthApiHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.User is ClaimsPrincipal principal)
            {
                if (principal.Identity?.IsAuthenticated == false)
                {
                    context.Response.StatusCode = 401;
                    context.Response.Flush();
                    context.Response.End();
                    return;
                }

                var messagePrincipal = MessagePrincipal.FromClaimsPrincipal(principal);
                var bytes = MessagePackSerializer.Serialize(messagePrincipal);
                context.Response.BinaryWrite(bytes);
                context.Response.StatusCode = 200;
                context.Response.Flush();
                context.Response.End();
                return;
            }

            context.Response.StatusCode = 401;
            context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable => true;
    }
}