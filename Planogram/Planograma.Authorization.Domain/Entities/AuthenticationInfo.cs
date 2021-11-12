using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.Authorization.Domain.Entities
{
    public class AuthenticationInfo
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }        
        public Employee Employee { get; set; }
        public DateTime ConnectedTime { get; set; }
        public string ConnectedIP { get; set; }
        public bool IsSuccesLoggedIn { get; set; }
        public bool IsBlockActive { get; set; }
        public bool IsBlockIPAdressActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
