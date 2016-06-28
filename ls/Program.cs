using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls
{
    /// <summary>
    /// Unix's ls for windows (kind of)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string path = @".";

            List<LsEntry> list = new List<LsEntry>();

            foreach (var dir in Directory.GetDirectories(path).OrderBy(x => x))
                list.Add(new DirEntry(dir));

            foreach (var fn in Directory.GetFiles(path).OrderBy(x => x))
                list.Add(new FileEntry(fn));

            int colCount = 4;
            int[] colWidths = new int[colCount];

            for (int i = 0; i < list.Count; i++)
            {
                int col = i % colCount;
                int eLen = list[i].Length;
                if (eLen > colWidths[col])
                    colWidths[col] = eLen;
            }                

            for (int i = 0; i < list.Count; i++)
            {
                int col = i % colCount;

                if (list[i].Color.HasValue)                
                    ConsoleUtils.PushAndSetForegroundColor(list[i].Color.Value);                

                Console.Write(list[i].Text.PadRight(colWidths[col]));

                if (list[i].Color.HasValue)
                    ConsoleUtils.PopForegroundColor();

                if (col == colCount-1)
                    Console.WriteLine(" ");
                else
                    Console.Write(" ");
            }

#if DEBUG
                Console.ReadLine();
#endif
        }
    }
}
