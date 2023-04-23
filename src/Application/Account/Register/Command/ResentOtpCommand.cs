using BookStore.Application.Account.Interface;
using BookStore.Application.Common.Models;
using MediatR;

namespace BookStore.Application.Account.Register.Command;

public class ResentOtpCommand: IRequest<Result>
{
    public string  Email { get; set; }
}

public class ResentOtpCommandHandler : IRequestHandler< ResentOtpCommand,Result>
{
    private IAccountService _accountService;

    public ResentOtpCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result> Handle(ResentOtpCommand request, CancellationToken cancellationToken)
    {
        var result = await _accountService.SendEmailConfirmationOtp(request.Email, "123456"); 
        return Result.Success();
    }
}