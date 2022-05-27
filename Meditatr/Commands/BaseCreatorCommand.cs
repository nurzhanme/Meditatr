using CliFx.Attributes;
using Meditatr.Infrastructure;

namespace Meditatr.Commands
{
    public abstract class BaseCreatorCommand
    {
        [CommandOption("model", 'm', Description = "Model class", IsRequired = true)]
        public string Model { get; init; }

        [CommandOption("prj", 'p', Description = "Project name where Commands and Queries located")]
        public string ProjectName { get; init; } = IoHelper.GetOnlyCurrentDirectory();
    }

    public abstract class BaseCqCreatorCommand : BaseCreatorCommand
    {
        [CommandOption("action", 'a', Description = "Action name", IsRequired = true)]
        public string Action { get; init; }

        [CommandOption("return", 'r', Description = "Return type")]
        public string ReturnType { get; init; } = "Unit";

        [CommandOption("handlerprj", 'H', Description = "Project name where Handlers for Commands and Queries located")]
        public string HandlerProjectName { get; init; } = IoHelper.GetOnlyCurrentDirectory();
    }
}
