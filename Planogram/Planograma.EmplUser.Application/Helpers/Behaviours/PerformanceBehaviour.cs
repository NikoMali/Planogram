using MediatR;
using Microsoft.Extensions.Logging;
using Planograma.EmplUser.Application.Interfaces;
using Serilog;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Helpers.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ICurrentUserService _currentUserService;
        

        public PerformanceBehaviour(
            ICurrentUserService currentUserService)
        {
            _timer = new Stopwatch();
            _currentUserService = currentUserService;
            
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var userId = _currentUserService.UserId ?? string.Empty;
                var userName = string.Empty;

                if (!string.IsNullOrEmpty(userId))
                {
                    userName = "admin";
                }

                Log.Warning("CleanArchitecture Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userId, userName, request);
            }

            return response;
        }
    }
}
