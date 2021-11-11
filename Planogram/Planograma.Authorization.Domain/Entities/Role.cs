using Planograma.Authorization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planograma.Authorization.Domain.Entities
{ 
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }
        public bool IsActive { get; set; }
        public List<EmployeeRole> EmployeeRoles { get; set; }
    }
}