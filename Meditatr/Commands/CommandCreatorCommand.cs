using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Meditatr.Enums;
using Meditatr.Services;

namespace Meditatr.Commands
{
    [Command("Command", Description = "creates command and its handler")]
    public class CommandCreatorCommand : BaseCreatorCommand, ICommand
    {
        private readonly ClassService _classService;

        public CommandCreatorCommand(ClassService classService)
        {
            _classService = classService ?? throw new ArgumentNullException(nameof(classService));
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _classService.Create(ProjectName, HandlerProjectName, Model, Action, OperationType.Command, ReturnType);

            // Return default task if the command is not asynchronous
            return default;
        }
    }
}
