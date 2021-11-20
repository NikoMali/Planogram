using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Planograma.EmplUser.API.Helpers
{
    public class AcceptLanguageHttpHeader
    {
        private readonly RequestDelegate _next;

        public AcceptLanguageHttpHeader(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var userLangs = httpContext.Request.Headers["Accept-Language"].ToString();
            var firstLang = userLangs.Split(',').FirstOrDefault();

            var lang = string.IsNullOrEmpty(firstLang) ? "ka-GE" : firstLang;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            //httpContext.Items["ClientLang"] = lang;
            //httpContext.Items["ClientCulture"] = Thread.CurrentThread.CurrentUICulture.Name;

            

            return _next(httpContext);
        }
    }

    
}
