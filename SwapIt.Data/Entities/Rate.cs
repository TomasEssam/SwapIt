﻿using Microsoft.Extensions.DependencyInjection;
using SwapIt.Data.Entities.Common;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class Rate : IDeletedEntity, IAuditEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RateValue { get; set; }
        [Required]
        public DateTime RateDate { get; set; }
        [MaxLength(1000)]
        public string? Feedback { get; set; }
        public int ServiceId { get; set; }
        public int CustomerId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
        [ForeignKey("CustomerId")]
        public ApplicationUser Customer { get; set; } = null!;
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
    }
}
