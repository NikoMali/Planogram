using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planograma.Authorization.Domain.Entities
{
    public class EmployeeRole
    {
        [Key]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        //referencee
        public int RoleId { get; set; }
        public Role Roles { get; set; }

        public string OtherPermissions { get; set; }
    }
}