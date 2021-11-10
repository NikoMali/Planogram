using Planograma.Authorization.Domain.Entities;
using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Planograma.Authorization.Domain.Entities
{
    public class EmployeeParams
    {
        [Key]
        public int EmployeeId { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [NotMapped]
        public Employee Employee { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
