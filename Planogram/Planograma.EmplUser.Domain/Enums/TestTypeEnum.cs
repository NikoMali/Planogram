using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Domain.Enums
{
    public enum TestTypeEnum : byte
    {
        [Display(Name = "Test")]
        Test = 1,
        [Display(Name = "Test1")]
        Test2 = 2,
        [Display(Name = "Test2")]
        Test3 = 3,
        Test4 = 4
    }
}
