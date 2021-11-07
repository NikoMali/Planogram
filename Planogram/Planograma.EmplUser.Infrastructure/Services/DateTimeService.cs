
using Planograma.EmplUser.Application.Interfaces;
using System;

namespace Planograma.EmplUser.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
