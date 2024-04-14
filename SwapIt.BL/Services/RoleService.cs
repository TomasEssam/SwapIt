using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SwapIt.BL.Helpers;
using SwapIt.BL.IServices.Identity;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.IPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class RoleService :  IRoleService 
    {

        public RoleService() 
        {
          
        }

        public List<string> GetRoles()
        {
            return new List<string>() { RolesNames.Admin, RolesNames.SuperAdmin,
                RolesNames.ServiceProvider , RolesNames.ServiceConsumer};
        }

      
    }
}
