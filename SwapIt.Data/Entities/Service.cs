using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int TimeToExecute { get; set; }
        // how to handel it 
        public byte[] Previous_work_Images { get; set; }

        //how to make it only for service Provider Role
        [ForeignKey("ApplicationUser")]
        public int ServiceProviderId { get; set; }
        public ApplicationUser ServiceProvider { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
