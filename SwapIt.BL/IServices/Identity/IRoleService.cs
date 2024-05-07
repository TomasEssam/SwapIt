using SwapIt.BL.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices.Identity
{
    public interface IRoleService
    {
        List<string> GetRoles();
        Task<bool> AddUserRoleRow(UserRolesDto userRole);
    }
}
