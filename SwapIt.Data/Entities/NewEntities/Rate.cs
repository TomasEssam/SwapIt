using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities.NewEntities
{
    public class Rate
    {
        public int Id { get; set; }
        public byte RateValue { get; set; }
        public DateTime RateDate { get; set; }
        public string Feedback { get; set; }
        // ??????
        [ForeignKey("ApplicationUser")]
        public int ServiceProviderId { get; set; }
        public ApplicationUser ServiceProvider { get; set; }

        [ForeignKey("ApplicationUser")]
        public int CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
    }
}
