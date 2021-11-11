
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Planograma.EmplUser.Application.Interfaces;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Helpers.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
       
        private readonly ICurrentUserService _currentUserService;
        

        public LoggingBehaviour(ICurrentUserService currentUserService)
        {
          
            _currentUserService = currentUserService;
            
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            string userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = "Admin";
            }

            Log.Information("CleanArchitecture Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
            return Task.CompletedTask;
        }
    }
}
