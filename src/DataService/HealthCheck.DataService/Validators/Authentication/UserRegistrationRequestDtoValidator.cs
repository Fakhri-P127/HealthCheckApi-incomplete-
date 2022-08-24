using FluentValidation;
using HealthCheck.Authentication.Models.DTOs.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.DataService.Validators.Authentication
{
    public class UserRegistrationRequestDtoValidator:AbstractValidator<UserRegistrationRequestDto>
    {
        public UserRegistrationRequestDtoValidator()
        {
            RuleFor(x=>x.Firstname).NotEmpty().MaximumLength(40);
            RuleFor(x=>x.Lastname).NotEmpty().MaximumLength(40);
            RuleFor(x=>x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
           
        }
    }
}
