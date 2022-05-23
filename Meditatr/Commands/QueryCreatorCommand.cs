using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Meditatr.Enums;
using Meditatr.Services;

namespace Meditatr.Commands
{
    [Command("Query", Description = "creates query and its handler")]
    public class QueryCreatorCommand : BaseCreatorCommand, ICommand
    {
        private readonly ClassService _classService;

        public QueryCreatorCommand(ClassService classService)
        {
            _classService = classService ?? throw new ArgumentNullException(nameof(classService));
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _classService.Create(ProjectName, HandlerProjectName, Model, Action, OperationType.Query, ReturnType);

            // Return default task if the command is not asynchronous
            return default;
        }
    }
}
