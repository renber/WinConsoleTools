using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls
{
    class DirEntry : LsEntry
    {
        string text;

        public DirEntry(string path)
        {
            text = path;
            int idx = text.LastIndexOf(Path.DirectorySeparatorChar);
            if (idx > -1)
                text = text.Substring(idx + 1);
        }

        public int Length
        {
            get { return text.Length; }
        }
        public string Text
        {
            get
            {
                return text;
            }
        }

        public ConsoleColor? Color
        {
            get { return ConsoleColor.Cyan; }
        }
    }
}
