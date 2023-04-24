using API.ViewModels;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<CustomerController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<string>> AuthenticateUserAccount([FromBody] AuthenticationViewModel authenticationViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.Log(LogLevel.Information, message: $"Authenticating UserName and Password");
                Message message = await _authenticationService.AuthenticateUser(authenticationViewModel.UserName, authenticationViewModel.Password);
                if (message.Result)
                {
                    return Ok(message.Data);
                }
                else
                {
                    _logger.Log(LogLevel.Error, message: $"Unauthorized");
                    return Unauthorized($"Unauthorized.,Reason: {message.ResultMessage}");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Authentication Failed");
                return Unauthorized("Unauthorized");
            }
        }
    }
}
