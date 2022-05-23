using Microsoft.Build.Construction;

namespace Meditatr.Infrastructure
{
    public static class IoHelper
    {
        public static string CreateDirectory(string[] pathList)
        {
            var path = Path.Combine(pathList);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static string CreateFile(string[] pathList, string data)
        {
            var path = Path.Combine(pathList);
            if (File.Exists(path))
            {
                path += '2';
            }

            using var streamWriter = new StreamWriter($"{path}.cs");
            streamWriter.Write(data);

            return path;
        }

        public static string GetProjectAbsolutePath(string projectName)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var solutionFileUri = Directory.GetFiles(currentDir, "*.sln").FirstOrDefault();
            if (string.IsNullOrWhiteSpace(solutionFileUri))
            {
                solutionFileUri = Directory.GetFiles(Directory.GetParent(currentDir).ToString(), "*.sln").FirstOrDefault();
            }
            var projectList = SolutionFile.Parse(solutionFileUri).ProjectsInOrder;

            var project =
                projectList.FirstOrDefault(x => x.ProjectName.ToLower().Equals(projectName.ToLower()));

            if (project == null)
                throw new Exception("Project not found");

            return RemoveLastItemInPath(project.AbsolutePath);
        }

        private static string RemoveLastItemInPath(string path)
        {
            var index = path.LastIndexOf("\\", StringComparison.Ordinal);
            return path.Substring(0, index);
        }
    }
}
