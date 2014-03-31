﻿using System;

namespace Procent.DependencyInjection.app
{
    public class UsersController
    {
        public void RegisterUser(string email)
        {
            // check if email is valid
            if (new EmailValidator().Validate(email) == false)
            {
                throw new ArgumentException("Invalid email address");
            }

            // check if email is not taken
            if (UsersDatabase.IsEmailTaken(email))
            {
                throw new InvalidOperationException("Email already taken");
            }

            // create new user
            var newUser = new User
            {
                Email = email,
                RegistrationToken = Guid.NewGuid().ToString(),
            };

            // insert user
            UsersDatabase.InsertUser(newUser);

            // generate activation link
            string registrationLink = string.Format(
                "http://myapp.com/confirm?email={0}&token={1}"
                , newUser.Email, newUser.RegistrationToken
            );

            EmailService.RegistrationEmail(newUser.Email, registrationLink);
        }
    }
}