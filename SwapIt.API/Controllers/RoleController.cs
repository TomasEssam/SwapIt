using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices.Identity;

namespace SwapIt.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region Fields

        private readonly IRoleService _roleService;

        #endregion

        #region Ctor

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion

        #region  Actions

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            return Ok(_roleService.GetRoles());
        }
        [HttpPost]
        [Route("AddUserRole")]
        public async Task<IActionResult> AddUserRolesRow([FromBody] UserRolesDto userRole)
        {
            return Ok(_roleService.AddUserRoleRow(userRole));
        }
        #endregion
    }
}
