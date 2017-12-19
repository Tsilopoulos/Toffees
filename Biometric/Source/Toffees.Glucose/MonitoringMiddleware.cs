using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibOwin;

namespace Toffees.Glucose
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MonitoringMiddleware
    {
        private readonly AppFunc _next;
        private readonly Func<Task<bool>> _healthCheck;

        private static readonly PathString MonitorPath = new PathString("/_monitor");
        private static readonly PathString MonitorShallowPath = new PathString("/_monitor/shallow");
        private static readonly PathString MonitorDeepPath = new PathString("/_monitor/deep");

        public MonitoringMiddleware(AppFunc next, Func<Task<bool>> healthCheck)
        {
            _next = next;
            _healthCheck = healthCheck;
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);
            return context.Request.Path.StartsWithSegments(MonitorPath)
                ? HandleMonitorEndpoint(context) : _next(env);
        }

        private Task HandleMonitorEndpoint(IOwinContext context)
        {
            if (context.Request.Path.StartsWithSegments(MonitorShallowPath))
                return ShallowEndpoint(context);
            return context.Request.Path.StartsWithSegments(MonitorDeepPath)
                ? DeepEndpoint(context) : Task.FromResult(0);
        }

        private async Task DeepEndpoint(IOwinContext context)
        {
            if (await _healthCheck())
                context.Response.StatusCode = 204;
            else
                context.Response.StatusCode = 503;
        }

        private Task ShallowEndpoint(IOwinContext context)
        {
            context.Response.StatusCode = 204;
            return Task.FromResult(0);
        }
    }
}
