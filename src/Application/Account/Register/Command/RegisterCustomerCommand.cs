using BookStore.Application.Account.Interface;
using BookStore.Application.Account.Model;
using BookStore.Application.Common.Models;
using MediatR;

namespace BookStore.Application.Account.Register.Command;

public class RegisterCustomerCommand : IRequest<Result<string>>
{
    // public string FirstName { get; set; }
    // public string LastName { get; set; }

    public string Email { get; set; }
    // public string PhoneNumber { get; set; } 
    public string Password { get; set; }
}

public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Result<string>>
{
    private IAccountService _accountService;

    public RegisterCustomerCommandHandler( IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result<string>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
     var result = await   _accountService.RegisterAsync(new RegisterRequest()
        {
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.Password,
            FirstName = "",
            LastName = "",
            UserName = request.Email,
            PhoneNumber = ""
        });

     return result;
    }
}

