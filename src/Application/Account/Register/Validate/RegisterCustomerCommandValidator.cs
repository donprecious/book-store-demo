using BookStore.Application.Account.Register.Command;
using FluentValidation;

namespace BookStore.Application.Account.Register.Validate;

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        
    }
}