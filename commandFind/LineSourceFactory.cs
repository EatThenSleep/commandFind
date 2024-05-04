using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commandFind
{
    internal class LineSourceFactory
    {
        public static ILineSource[] CreateInstance(string path, bool skipOfflineFiles)
        {
            if (string.IsNullOrEmpty(path))
            {
                return [new ConsolineSource()];
            }
            // Path.PathSeparator == "//"
            string pattern;
            int idx = path.LastIndexOf(Path.PathSeparator);
            if (idx < 0)
            {
                pattern = path;
                path = ".";
            }
            else
            {
                // substring from idx + 1 to end
                pattern = path[(idx + 1)..];
                // substring from 0 to idx
                path = path[..idx];
            }
            var dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                var files = dir.GetFiles(pattern);
                if (skipOfflineFiles)
                {
                    files = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Offline)).ToArray();
                }
                return files.Select(f => new FileLineSource(f.FullName, f.Name)).ToArray();
            }
            return [];
        }
    }
}
