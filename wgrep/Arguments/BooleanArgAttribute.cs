using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wgrep.Arguments;

namespace wgrep.Arguments
{
    class BooleanArgAttribute : ConsoleArgAttribute
    {
        public override int Priority
        {
            get { return 128; }
        }

        public string OnOption { get; set; }
        public string OffOption { get; set; }
        
        public bool DefaultValue { get; set; }

        public BooleanArgAttribute(string onOption, string offOption, bool defaultValue, String name, String description)
            : base(name, description)
        {
            OnOption = onOption;
            OffOption = offOption;            
            DefaultValue = defaultValue;
        }

        public override void PrintHelp(System.Reflection.PropertyInfo property)
        {
            if (!String.IsNullOrEmpty(OnOption))
                Console.Write(OnOption + "\t" + Description);            
            else
            if (!String.IsNullOrEmpty(OffOption))
                Console.Write(OffOption + "\t" + Description);

            Console.WriteLine(" (default " + (DefaultValue ? "on" : "off") + ")");
        }

        public override void TryToApply(object targetInstance, PropertyInfo property, IList<String> argv)
        {
            bool wasSet = false;

            if (!String.IsNullOrEmpty(OnOption))
            {
                if (argv.Contains(OnOption))
                {
                    wasSet = true;
                    property.SetValue(targetInstance, true);

                    argv.Remove(OnOption);
                }
            }
            if (!String.IsNullOrEmpty(OffOption))
            {
                if (argv.Contains(OffOption))
                {
                    if (wasSet)
                        throw new ArgumentException("Contradicting command line switches: " + OnOption + " and " + OffOption);

                    property.SetValue(targetInstance, false);
                    wasSet = true;

                    argv.Remove(OffOption);
                }                    
            }

            if (!wasSet)
                property.SetValue(targetInstance, DefaultValue);
        }
    }
}
