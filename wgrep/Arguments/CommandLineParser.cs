using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace wgrep.Arguments
{
    public static class CommandLineParser
    {

        public static void ParseTo(Object commandLineSettingsObject, string[] argv)
        {
            var attr = GetConsoleArgAttributes(commandLineSettingsObject.GetType());

            IList<String> args = new List<String>();
            foreach (var arg in argv)
                args.Add(arg);

            foreach(var a in attr)
                a.Value.TryToApply(commandLineSettingsObject, a.Key, args);            

            if (args.Count > 0)
            {                
                string s = String.Join(", ", args);
                if (args.Count == 1)
                    throw new ArgumentException("Unrecognized command line option: " + s);
                else
                    throw new ArgumentException("Unrecognized command line options: " + s);
            }
        }

        public static void PrintCommandLineHelp<T>()
        {            
            var attr = GetConsoleArgAttributes(typeof(T));

            foreach(var a in attr)
            {
                a.Value.PrintHelp(a.Key);
            }
        }

        private static IList<KeyValuePair<PropertyInfo, ConsoleArgAttribute>> GetConsoleArgAttributes(Type forType)
        {
            List<KeyValuePair<PropertyInfo, ConsoleArgAttribute>> attributes = new List<KeyValuePair<PropertyInfo, ConsoleArgAttribute>>();

            var properties = forType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in properties)
            {
                var attr = p.GetCustomAttributes<ConsoleArgAttribute>();

                foreach (var a in attr)
                    attributes.Add(new KeyValuePair<PropertyInfo, ConsoleArgAttribute>(p, a));
            }

            return attributes.OrderByDescending(x => x.Value.Priority).ToList();            
        }

    }
}
