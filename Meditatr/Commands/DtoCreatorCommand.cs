using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Meditatr.Services;

namespace Meditatr.Commands
{
    [Command("Dto", Description = "creates dto from given model")]
    public class DtoCreatorCommand : BaseCreatorCommand, ICommand
    {
        private readonly ClassService _classService;

        public DtoCreatorCommand(ClassService classService)
        {
            _classService = classService ?? throw new ArgumentNullException(nameof(classService));
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _classService.CreateDto(ProjectName, Model);

            // Return default task if the command is not asynchronous
            return default;
        }
    }
}
