﻿using Microsoft.Extensions.DependencyInjection;
using SwapIt.Data.Entities.Common;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class ServiceRequest : IDeletedEntity, IAuditEntity
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestState { get; set; }
        public float ExecutionTime { get; set; }
        public float PaidFund { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; } 
        [ForeignKey("CustomerId")]
        public ApplicationUser Customer { get; set; }
    }
}
