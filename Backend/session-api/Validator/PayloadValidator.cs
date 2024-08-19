using FluentValidation;
using session_api.Model;

namespace session_api.Validator
{
    public class PayloadValidator : AbstractValidator<Payload>
    {
        public PayloadValidator()
        {
            RuleFor(x => x.mail)
                .NotEmpty().When(x => x != null).WithMessage("Email cannot be empty if provided.")
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.mail)).WithMessage("Invalid email format.")
                .Length(3, 254).When(x => !string.IsNullOrEmpty(x.mail)).WithMessage("Email length must be between 3 and 254 characters.");

            RuleFor(x => x.pageUrl)
                .Must(_IsValidUrl).WithMessage("The pageUrl is not valid as either Base64 URL or regular URL.");

            RuleFor(x => x.pictureUrl)
                .Must(_IsValidUrl).WithMessage("The pictureUrl is not valid as either Base64 URL or regular URL.");
        }

        private static bool _IsValidUrl(string url)
        {
            return !IsValidBase64Url.Test(url)
                ? IsValidUrl.Test(url)
                    ? true
                    : false
                : true;
        }
    }
}

