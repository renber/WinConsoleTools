using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls
{
    class FileEntry : LsEntry
    {
        string text;

        public FileEntry(String filename)
        {
            text = Path.GetFileName(filename);
        }

        public string Text
        {
            get
            {
                return text;
            }
        }

        public int Length
        {
            get { return text.Length; }
        }


        public ConsoleColor? Color
        {
            get { return null; }
        }
    }
}
