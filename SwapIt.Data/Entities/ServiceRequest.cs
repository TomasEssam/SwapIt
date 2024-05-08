using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        // ???????
        public string RequestState { get; set; }
        public float ExecutionTime { get; set; }
        public float PaidFund { get; set; }
        // how to make it act as a customer
        [ForeignKey("ApplicationUser")]
        public int CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
