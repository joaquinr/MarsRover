using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    /// <summary>
    /// Translates from a string to a list of commands
    /// </summary>
    public class CommandTranslator : ICommandTranslator
    {
        public List<IRoverCommand> TranslateCommands(string commands)
        {
            var ret = new List<IRoverCommand>();

            foreach (var commandCharacter in commands)
            {
                switch (commandCharacter)
                {
                    case MoveForwardCommand.CommandLetter:
                        ret.Add(new MoveForwardCommand());
                        break;
                    case MoveBackwardCommand.CommandLetter:
                        ret.Add(new MoveBackwardCommand());
                        break;
                    case TurnLeftCommand.CommandLetter:
                        ret.Add(new TurnLeftCommand());
                        break;
                    case TurnRightCommand.CommandLetter:
                        ret.Add(new TurnRightCommand());
                        break;
                    case TakeSampleCommand.CommandLetter:
                        ret.Add(new TakeSampleCommand());
                        break;
                    case ExtendSolarPanelsCommand.CommandLetter:
                        ret.Add(new ExtendSolarPanelsCommand());
                        break;
                    default:
                        break;
                }
            }

            return ret;
        }
    }
}
