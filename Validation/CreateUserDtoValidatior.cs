using FluentValidation;
using System.Text.Json.Serialization;
using UdemyCoreLayer.Dtos;

namespace UdemyAuthServer.API.Validation
{
    public class CreateUserDtoValidatior:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidatior() 
        { 
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email boş olamaz").EmailAddress().WithMessage("Geçerli bir email adresi giriniz");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola boş olamaz").MinimumLength(4).WithMessage("Parola en az 4 karakter olmalıdır");



        }

        
    }
}
