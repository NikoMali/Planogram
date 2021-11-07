using Planograma.EmplUser.Domain.Common;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
