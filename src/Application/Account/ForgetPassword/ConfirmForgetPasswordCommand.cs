using BookStore.Application.Account.Interface;
using BookStore.Application.Common.Models;
using MediatR;

namespace BookStore.Application.Account.ForgetPassword;

public class ConfirmForgetPasswordCommand : IRequest<Result>
{
    public string Email { get; set; }
    
    public string NewPassword { get; set; } 
    public string Otp { get; set; }
}

public class ConfirmForgetPasswordCommandHandler : IRequestHandler<ConfirmForgetPasswordCommand, Result>
{
    private IAccountService _accountService;

    public ConfirmForgetPasswordCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result> Handle(ConfirmForgetPasswordCommand request, CancellationToken cancellationToken)
    {
       return await _accountService.ConfirmPasswordOtpByEmailAsync(request.Email, request.NewPassword, request.Otp); 
    }
}
