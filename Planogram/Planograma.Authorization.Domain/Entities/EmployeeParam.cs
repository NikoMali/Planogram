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
        public EmployeeParams() {
            RefreshTokens = new List<RefreshToken>();
        }
        public void addRefreshTokens(RefreshToken refreshToken) { RefreshTokens.Add(refreshToken); }
        [Key]
        public int EmployeeId { get; set; }
        public string Username { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [NotMapped]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
