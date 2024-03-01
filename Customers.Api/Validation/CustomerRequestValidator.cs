using System.Text.RegularExpressions;
using Customers.Api.Contracts.Requests;
using FluentValidation;

namespace Customers.Api.Validation;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    private static readonly Regex FullNameRegex =
        new("^[a-z ,.'-]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex EmailRegex =
        new("^[\\w!#$%&’*+/=?`{|}~^-]+(?:\\.[\\w!#$%&’*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public CustomerRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().Matches(FullNameRegex);
        RuleFor(x => x.Email).NotEmpty().Matches(EmailRegex);
        RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).NotEmpty();
    }
}
