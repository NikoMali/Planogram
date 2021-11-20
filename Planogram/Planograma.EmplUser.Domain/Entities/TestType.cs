using Planograma.EmplUser.Domain.Enums;
using Planograma.EmplUser.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Domain.Entities
{
    public class TestType: IEnumModel<TestType, byte, TestTypeEnum>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte Id { get; set; }
        [StringLength(50)]
        public TestTypeEnum Name { get; set; }
        public bool IsActive { get; set; }
    }
}
