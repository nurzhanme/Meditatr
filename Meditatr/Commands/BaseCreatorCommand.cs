using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CliFx.Attributes;

namespace Meditatr.Commands
{
    public abstract class BaseCreatorCommand
    {
        [CommandOption("model", 'm', Description = "Model class", IsRequired = true)]
        public string Model { get; init; }

        [CommandOption("action", 'a', Description = "Action name", IsRequired = true)]
        public string Action { get; init; }

        [CommandOption("return", 'r', Description = "Return type (default value: Unit)")]
        public string ReturnType { get; init; } = "Unit";

        [CommandOption("prj", 'p', Description = "Project name where Commands and Queries located (default value: current project)")]
        public string ProjectName { get; init; } = Assembly.GetCallingAssembly().GetName().Name;

        [CommandOption("handlerprj", 'h', Description = "Project name where Handlers for Commands and Queries located (default value: current project)")]
        public string HandlerProjectName { get; init; } = Assembly.GetCallingAssembly().GetName().Name;
    }
}
