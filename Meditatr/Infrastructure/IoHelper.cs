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

        public static string ReadFile(string path)
        {
            using var streamReader = new StreamReader(path);

            return streamReader.ReadToEnd();
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
            var solutionFileUri = string.Empty;
            while (string.IsNullOrWhiteSpace(solutionFileUri) && !string.IsNullOrWhiteSpace(currentDir))
            {
                solutionFileUri = Directory.GetFiles(currentDir, "*.sln").FirstOrDefault();
                currentDir = Directory.GetParent(currentDir)?.ToString();
            }

            if (string.IsNullOrWhiteSpace(solutionFileUri))
            {
                throw new FileNotFoundException("Solution file not found");
            }

            var projectList = SolutionFile.Parse(solutionFileUri).ProjectsInOrder;

            var project =
                projectList.FirstOrDefault(x => x.ProjectName.ToLower().Equals(projectName.ToLower()));

            if (project == null)
                throw new Exception("Project not found");

            return RemoveLastItemInPath(project.AbsolutePath);
        }

        public static string GetOnlyCurrentDirectory()
        {
            return Directory.GetCurrentDirectory().Substring(Directory.GetCurrentDirectory().LastIndexOf("\\") + 1);
        }

        private static string RemoveLastItemInPath(string path)
        {
            var index = path.LastIndexOf("\\", StringComparison.Ordinal);
            return path.Substring(0, index);
        }
    }
}
