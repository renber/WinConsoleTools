using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wgrep.Arguments;

namespace wgrep
{
    /// <summary>
    /// Some kind of grep for windows
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    // show help
                    Console.WriteLine("command line: [switches] [expr]");
                    CommandLineParser.PrintCommandLineHelp<WGrepArgs>();
                }
                else
                {
                    var wargs = WGrepArgs.FromCommandLine(args);                    

                    foreach (var filename in Directory.EnumerateFiles(wargs.SearchPath, wargs.FileMask, wargs.SearchRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                    {
                        if (wargs.SearchAllFiles || GuessIfTextfile(filename))
                        {
                            var r = WGrep.InspectFile(filename, wargs);

                            String basePath = Path.GetFullPath(wargs.SearchPath);

                            if (r.Results.Count > 0)
                            {
                                ConsoleUtils.PushAndSetForegroundColor(ConsoleColor.DarkMagenta);
                                string fPath = Path.GetFullPath(r.Filename);
                                if (fPath.StartsWith(basePath))
                                    fPath = fPath.Remove(0, basePath.Length).Trim(Path.DirectorySeparatorChar);                                                                    
                                Console.WriteLine(fPath);

                                ConsoleUtils.PopForegroundColor();

                                foreach (var l in r.Results)
                                {
                                    if (l.Matches.Count == 1)
                                    {
                                        var m = l.Matches.First();                                        
                                        Console.Write(" @" + l.LineNo.ToString() + "," + m.StartIndex.ToString() + ": ");
                                        PrintGrepMatch(wargs, l, m);
                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        foreach (var m in l.Matches)
                                        {
                                            Console.Write(" @" + l.LineNo.ToString() + "," + m.StartIndex.ToString() + ": ");
                                            PrintGrepMatch(wargs, l, m);
                                            Console.WriteLine();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

#if DEBUG
            Console.ReadLine();
#endif
        }

        /// <summary>
        /// Tries to guess if the given file is a text file
        /// May not be accurate all the time
        /// </summary>        
        static bool GuessIfTextfile(string filename)
        {
            try
            {
                using (FileStream fStream = new FileStream(filename, FileMode.Open))
                {
                    int b = fStream.ReadByte();
                    while (b != -1)
                    {
                        b = fStream.ReadByte();
                        if (b == 0) // ASCII and UTF-8 files should not contain null bytes
                            return false;
                    }
                }
            } catch (UnauthorizedAccessException ex)
            {
                return false; // skip the file
            }

            return true;
        }

        static void PrintGrepMatch(WGrepArgs args, GrepLineResult line, GrepMatch match)
        {
            int outlook = 10;            

            int sIdx = 0;
            if (!args.PrintFullLines)
                sIdx = Math.Max(0, match.StartIndex - outlook);

            if (sIdx < match.StartIndex)
            {
                string before = line.LineContent.Substring(sIdx, match.StartIndex - sIdx);
                before = before.TrimStart();
                if (sIdx > 0)
                    Console.Write("...");
                Console.Write(before);
            }

            ConsoleUtils.PushAndSetForegroundColor(ConsoleColor.Yellow);
            if (args.PrintFullLines)
                Console.Write(match.MatchContent);
            else
                Console.Write(match.MatchContent.Trim());
            ConsoleUtils.PopForegroundColor();

            int eIdx = line.LineContent.Length - 1;
            if (!args.PrintFullLines)            
                eIdx = Math.Min(line.LineContent.Length - 1, match.EndIndex + outlook);

            if (eIdx > match.EndIndex)
            {
                string after = line.LineContent.Substring(match.EndIndex + 1, eIdx - match.EndIndex);
                after = after.TrimEnd();
                Console.Write(after);
                if (eIdx < line.LineContent.Length - 1)
                    Console.Write("...");                
            }            
        }
    }
}
