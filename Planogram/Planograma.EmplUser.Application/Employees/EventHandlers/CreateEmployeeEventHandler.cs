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
    public class CreateEmployeeEventHandler : INotificationHandler<DomainEventNotification<EmployeeCreatedEvent>>
    {
        

        public CreateEmployeeEventHandler()
        {
            
        }

        public Task Handle(DomainEventNotification<EmployeeCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            Log.Information("Employee Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
