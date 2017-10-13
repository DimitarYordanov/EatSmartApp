﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyEating.Client.Core.Contracts
{
    public interface ICommandProcessor
    {
        ICollection<ICommand> Commands { get; }

        void Add(ICommand command);

        void ProcessSingleCommand(ICommand command, string commandLine);
    }
}
