using BookStore.Application.Account.ForgetPassword;
using BookStore.Application.Account.Interface;
using BookStore.Application.Account.Login.Command;
using BookStore.Application.Account.Model;
using BookStore.Application.Account.Register.Command;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

public class AccountController : ApiControllerBase
{

    private IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
/// <summary>
/// endpoint to register 
/// </summary>
/// <param name="command"></param>
/// <returns></returns>
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
    
    /// <summary>
    /// endpoint to verify account 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// endpoint to resend registration token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    
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
    
    /// <summary>
    /// endpoint to login and get jwt token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
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
    /// <summary>
    /// endpoint to forget password
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
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
    /// <summary>
    /// endpoint to confirm password
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
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
    /// <summary>
    ///  Endpoint to refresh token 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    [SwaggerOperation("Refresh token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result =await _accountService.RefreshTokenAsync(request);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}