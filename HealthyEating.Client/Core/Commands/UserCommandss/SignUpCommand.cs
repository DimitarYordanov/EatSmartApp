﻿using Bytes2you.Validation;
using HealthyEating.Client.Core.Contracts;
using HealthyEating.Client.Data;
using System.Collections.Generic;

namespace HealthyEating.Client.Core.Commands
{
    public class SignUp : Command, ICommand
    {
        private readonly IDatabase database;
        private readonly IUserManager userManager;
        private readonly IPasswordHasher passwordHasher;

        public SignUp( IUserManager userManager, IReader reader, IWriter writer)
            : base(reader, writer)
        {
            Guard.WhenArgument(userManager, "userManager").IsNull().Throw();
     
            this.userManager = userManager;           
        }

        public override string Execute(IList<string> commandLine)
        {
            var parameters = TakeInput();
            var username = parameters[0];
            var password = parameters[1];

            return this.userManager.SignUp(username, password);
        }

        private IList<string> TakeInput()
        {
            var username = ReadOneLine("Username: ");
            var password = ReadOneLine("Password: ");
            var confirmedPassword = this.ReadOneLine("Confirm Password: ");
            return new List<string>() { username, password };
        }
    }
}
