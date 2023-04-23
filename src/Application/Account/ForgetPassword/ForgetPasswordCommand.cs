using BookStore.Application.Account.Interface;
using BookStore.Application.Common.Models;
using MediatR;

namespace BookStore.Application.Account.ForgetPassword;

public class ForgetPasswordCommand: IRequest<Result<string>>
{
    public string Email { get; set; } 
    
}

public class ForgetPasswordCommandHandler: IRequestHandler<ForgetPasswordCommand,Result<string>>
{
   
    private IAccountService _accountService;

    public ForgetPasswordCommandHandler( IAccountService accountService)
    {
     
        _accountService = accountService;
    }

    public Task<Result<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
      return   _accountService.SendForgetPasswordEmailOtp(request.Email, "123456");
    
    }
}
