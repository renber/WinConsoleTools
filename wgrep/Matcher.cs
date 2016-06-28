using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wgrep
{
    public class Matcher
    {
        Regex regex;
        SearchMode searchMode;
        WGrepArgs arguments;

        public Matcher(WGrepArgs arguments)
        {
            this.arguments = arguments;
            searchMode = arguments.SearchMode;

            RegexOptions options = new RegexOptions();
            if (arguments.IgnoreCase)
                options |= RegexOptions.IgnoreCase;

            switch(arguments.SearchMode)
            {
                case SearchMode.Regex:
                    regex = new Regex(arguments.SearchExpression, options);
                    break;
                case SearchMode.Contains:
                    regex = new Regex(Regex.Escape(arguments.SearchExpression), options);
                    break;
                case SearchMode.StartsWith:
                    regex = new Regex(@"^\s+(" + Regex.Escape(arguments.SearchExpression) + ")", options);
                    break;
                case SearchMode.EndsWith:
                    regex = null;
                    break;                  
            }
        }

        public IList<GrepMatch> Match(string text)
        {
            List<GrepMatch> matches = new List<GrepMatch>();

            if (searchMode == SearchMode.EndsWith)
            {
                string st = arguments.SearchExpression;
                string searchText = text.TrimEnd();
                if (arguments.IgnoreCase)
                {
                    searchText = searchText.ToLower();
                    st = st.ToLower();
                }

                if (searchText.EndsWith(st))
                {
                    GrepMatch gm = new GrepMatch();
                    gm.StartIndex = searchText.LastIndexOf(st);
                    gm.EndIndex = gm.StartIndex + st.Length - 1;
                    gm.MatchContent = text.Substring(gm.StartIndex, st.Length);
                    matches.Add(gm);
                }
            }
            else
            {                
                foreach (var m in regex.Matches(text).OfType<Match>())
                {
                    GrepMatch gm = new GrepMatch();
                    gm.StartIndex = m.Index;
                    gm.EndIndex = m.Index + m.Length - 1;
                    gm.MatchContent = m.Value;
                    matches.Add(gm);
                }                
            }

                return matches;
            }
    }
}
