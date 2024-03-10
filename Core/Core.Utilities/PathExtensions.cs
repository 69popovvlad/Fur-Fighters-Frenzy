using System;
using System.IO;

namespace Core.Utilities
{
    public static class PathExtensions
    {
        public const string ResourcesPath = "Assets\\Resources\\";
        
        public static string GetResourcePathForLoading(this string fullPath)
        {
            return Path.ChangeExtension(GetRelativePath(fullPath, ResourcesPath), null);
        }
        
        public static string GetRelativePath(this string fullPath, string rootPath)
        {
            rootPath = NormalizeFilepath(rootPath);
            fullPath = NormalizeFilepath(fullPath);

            if (!fullPath.StartsWith(rootPath))
            {
                throw new Exception("Could not find rootPath in fullPath when calculating relative path.");
            }

            return fullPath.Substring(rootPath.Length);
        }
        
        private static string NormalizeFilepath(string filepath)
        {
            var result = Path.GetFullPath(filepath);
            result = result.TrimEnd(new [] { Path.PathSeparator });
            return result;
        }
    }
}