using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wgrep
{
    static class WGrep
    {

        /// <summary>
        /// Finds all line sin the given file matching the regular expression and reports them
        /// using the given callback method
        /// </summary>        
        public static GrepResult InspectFile(string filename, WGrepArgs arguments)
        {
            GrepResult r = new GrepResult();
            r.Filename = filename;

            Matcher matcher = new Matcher(arguments);

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    int lineIdx = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var matches = InspectText(line, matcher);
                        if (matches.Count > 0)
                        {
                            GrepLineResult gr = new GrepLineResult();
                            gr.LineContent = line;
                            gr.LineNo = lineIdx + 1;
                            foreach (var m in matches)
                                gr.Matches.Add(m);
                            r.Results.Add(gr);
                        }

                        lineIdx++;
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return new GrepResult() { Filename = filename }; // skip the file
            }

            return r;
        }

        public static IList<GrepMatch> InspectText(String text, Matcher matcher)
        {
            return matcher.Match(text);
        }
    }

    public class GrepResult
    {
        public string Filename { get; set; }

        public IList<GrepLineResult> Results {get; private set;}
        
        public GrepResult()
        {
            Results = new List<GrepLineResult>();
        }
    }

    /// <summary>
    /// Enthält Informationen über eine Zeile einer Datei und die darin gefundenen Treffer
    /// </summary>
    public class GrepLineResult
    {        
        public int LineNo { get; set; }
        public String LineContent { get; set; }        
        public IList<GrepMatch> Matches {get; private set;}

        public GrepLineResult()
        {
            Matches = new List<GrepMatch>();
        }
    }

    public class GrepMatch
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string MatchContent { get; set; }
    }
}
