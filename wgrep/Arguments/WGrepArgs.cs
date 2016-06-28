using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wgrep.Arguments;

namespace wgrep
{
    public class WGrepArgs
    {
        /// <summary>
        /// The directory to search in
        /// </summary>
        [ValueParamArg("-p", typeof(string), ".", "path", "The path to search in")]
        public string SearchPath { get; set; }

        /// <summary>
        /// The file mask to use
        /// </summary>
        [ValueParamArg("-f", typeof(string), "*", "mask", "The filename mask to use")]
        public string FileMask { get; set; }

        /// <summary>
        /// The expression to search
        /// </summary>        
        [RoamingArgAttribute("expr", "The expression to search with (depending on the search mode)")]
        public string SearchExpression { get; set; }
        
        [BooleanArg("-r", "", false, "SearchRecursive", "Search recursively in sub directories")]
        public bool SearchRecursive { get; set; }
        
        [BooleanArg("-a", "", false, "SearchAllFiles", "Search all files instead of only text files")]
        public bool SearchAllFiles { get; set; }

        /// <summary>
        /// The SearchMode to use
        /// </summary>        
        [EnumArg(typeof(SearchMode), SearchMode.Regex, "SearchMode", "How to search")]
        [EnumSwitch(SearchMode.StartsWith, "-s", "File lines begin with the given search term")]
        [EnumSwitch(SearchMode.EndsWith, "-e", "File lines end with the given search term")]
        [EnumSwitch(SearchMode.Contains, "-c", "File lines contain the given search term")]
        [EnumSwitch(SearchMode.StartsWith, "-x", "The search term is a regular expression")]
        public SearchMode SearchMode { get; set; }

        /// <summary>
        /// Ignore the case of characters
        /// </summary>        
        [BooleanArg("-ic", "", false, "IgnoreCase", "Ignore character casing")]
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Print the matched lines fully
        /// </summary>
        [BooleanArg("-full", "", false, "PrintFullLines", "Prints whole matched lines instead of an excerpt around the match")]
        public bool PrintFullLines { get; set; }

        public static WGrepArgs FromCommandLine(string[] argv)
        {
            WGrepArgs wg = new WGrepArgs();
            CommandLineParser.ParseTo(wg, argv);
            return wg;
        }
    }

    public enum SearchMode
    {
        StartsWith, 
        EndsWith,
        Contains,
        Regex
    }
}
