using BookStore.Application.Account.Interface;
using BookStore.Application.Account.Model;
using BookStore.Application.Common.Models;
using MediatR;

namespace BookStore.Application.Account.Login.Command;

public class LoginCommand : IRequest<Result<AuthenticationResponse>>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthenticationResponse>>
{
    private IAccountService _accountService;

    public LoginCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
     return  await   _accountService.AuthenticateAsync(new AuthenticationRequest()
        {
            Email = request.Email,
            Password = request.Password
        });
    }
}