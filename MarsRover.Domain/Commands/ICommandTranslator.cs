using System.Collections.Generic;

namespace MarsRover.Domain.Commands
{
    public interface ICommandTranslator
    {
        List<IRoverCommand> TranslateCommands(string commands);
    }
}