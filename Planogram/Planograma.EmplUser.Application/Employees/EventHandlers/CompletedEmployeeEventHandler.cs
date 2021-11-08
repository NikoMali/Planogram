using MediatR;
using Microsoft.Extensions.Logging;
using Planograma.EmplUser.Application.Models;
using Planograma.EmplUser.Domain.Events;
using Serilog;
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
        

        public CompletedEmployeeEventHandler()
        {
            
        }

        public Task Handle(DomainEventNotification<EmployeeCompletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            Log.Information("Employee Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
