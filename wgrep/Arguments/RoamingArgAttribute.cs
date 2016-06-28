using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wgrep.Arguments
{
    /// <summary>
    /// Console argument which is evaluated last
    /// and catches the last remaining command line parameter
    /// </summary>
    class RoamingArgAttribute : ConsoleArgAttribute
    {
        public override int Priority
        {
            get { return 0; }
        }       
 
        public RoamingArgAttribute(String name, String description)
            : base(name, description)
        {

        }

        public override void PrintHelp(System.Reflection.PropertyInfo property)
        {
            Console.WriteLine("[" + Name + "]\t" + Description);            
        }

        public override void TryToApply(object targetInstance, System.Reflection.PropertyInfo property, IList<string> argv)
        {
            if (argv.Where(x => !x.StartsWith("-")).Count() == 1)
            {
                var val = argv.First(x => !x.StartsWith("-"));
                property.SetValue(targetInstance, val);
                argv.Remove(val);
            }
            else
                throw new ArgumentException("Argument " + Name + " missing or given multiple times.");
        }
    }
}
