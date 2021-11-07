using MediatR;
using Microsoft.Extensions.Logging;
using Planograma.EmplUser.Application.Models;
using Planograma.EmplUser.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.EventHandlers
{
    class CompletedEmployeeEventHandler : INotificationHandler<DomainEventNotification<EmployeeCompletedEvent>>
    {
        private readonly ILogger<CompletedEmployeeEventHandler> _logger;

        public CompletedEmployeeEventHandler(ILogger<CompletedEmployeeEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<EmployeeCompletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Employee Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
