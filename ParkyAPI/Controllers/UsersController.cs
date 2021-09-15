using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {

        private readonly IUserRepository _repository;

        public UsersController(IUserRepository userRepo)
        {
            _repository = userRepo;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User model)
        {
            var user = _repository.Authenticate(model.UserName, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect!" });
            }

            return Ok(user);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
