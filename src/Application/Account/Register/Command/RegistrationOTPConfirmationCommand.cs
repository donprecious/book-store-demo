using BookStore.Application.Account.Interface;
using BookStore.Application.Common.Models;
using MediatR;

namespace BookStore.Application.Account.Register.Command;

public class RegistrationConfirmationCommand: IRequest<Result>
{
    public string  Otp { get; set; }
    public string  email { get; set; }
}

public class RegistrationConfirmationCommandHandler : IRequestHandler< RegistrationConfirmationCommand,Result>
{
    private IAccountService _accountService;

    public RegistrationConfirmationCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result> Handle(RegistrationConfirmationCommand request, CancellationToken cancellationToken)
    {
        var result = await _accountService.ConfirmEmailOtpAsync(request.email, request.Otp); 
        return result;
    }
}