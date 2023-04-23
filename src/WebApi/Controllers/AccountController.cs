using BookStore.Application.Account.ForgetPassword;
using BookStore.Application.Account.Login.Command;
using BookStore.Application.Account.Register.Command;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

public class AccountController : ApiControllerBase
{

    [HttpPost("register")]
    [SwaggerOperation("register a new account")]
    public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    
    [HttpPost("verify")]
    [SwaggerOperation("validate otp of a phone number")]
    public async Task<IActionResult> ValidateOtpOfPhoneNumber([FromBody] RegistrationConfirmationCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [HttpPost("resend-registration-otp")]
    [SwaggerOperation("resend registration otp")]
    public async Task<IActionResult> ResendRegistrationOtp([FromBody] ResentOtpCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    [HttpPost("login")]
    [SwaggerOperation("login with an account")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    [HttpPost("forget-password")]
    [SwaggerOperation("request passowrd reset")]
    public async Task<IActionResult> PassworReset([FromBody] ForgetPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [HttpPost("confirm-password-reset")]
    [SwaggerOperation("request passowrd reset")]
    public async Task<IActionResult> ConfirmPasswordReset([FromBody] ConfirmForgetPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
}