using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Test.Api.Models;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("id")]
        public IActionResult GetUserInfo(int id)
        {
            logger.LogInformation("Info Metod");
            var user = new UserLoginResponseModel()
            {
                Success = true,
                UserMail = "onurumutluoglu@gmail.com"

            };
           
            return Ok(user);
        }
        [HttpPost]
        [Route("LoginOnly")]
        public IActionResult LoginOnly([FromBody] UserLoginRequestModel model)
        {
            logger.LogInformation("Info Metod");

            return Ok();

        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] UserLoginRequestModel model)
        {
            logger.LogInformation("Info Metod");

            var user = new UserLoginResponseModel()
            {
                Success = true,
                UserMail = "onurumutluoglu@gmail.com"

            };
            return Ok(user);

        }
    }
}
