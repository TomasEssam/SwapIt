using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        #endregion
    }
}
