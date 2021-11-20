using Microsoft.EntityFrameworkCore;
using Planograma.EmplUser.Domain.Entities;
using Planograma.EmplUser.Domain.Enums;
using Planograma.EmplUser.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Infrastructure.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestType>().HasData(EnumHelpers.GetModelFromEnum<TestType, TestTypeEnum>());
        }
    }
}
