using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wgrep.Arguments
{
    /// <summary>
    /// Extracts a parameter of the form [-indicator] [value]
    /// </summary>
    public class ValueParamArg : ConsoleArgAttribute
    {
        public override int Priority
        {
            get { return 255; }
        }

        public string Indicator { get; private set; }

        public Type ValueType { get; private set; }

        public Object DefaultValue { get; private set; }

        public ValueParamArg(string indicator, Type valueType, Object defaultValue, string name, string description)
            : base(name, description)
        {
            Indicator = indicator;
            ValueType = valueType;
            DefaultValue = defaultValue;
        }

        public override void TryToApply(object targetInstance, System.Reflection.PropertyInfo property, IList<string> argv)
        {
            int idx = argv.IndexOf(Indicator);

            if (idx >= 0)
            {
                if (idx < argv.Count - 1)
                {
                    try
                    {
                        property.SetValue(targetInstance, Convert.ChangeType(argv[idx + 1], ValueType));
                        argv.RemoveAt(idx + 1);
                        argv.RemoveAt(idx);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("Invalid parameter value for " + Indicator + ".");
                    }
                }
                else
                    throw new ArgumentException("Missing argument for parameter " + Indicator + ".");
            }
            else
                property.SetValue(targetInstance, DefaultValue);
        }

        public override void PrintHelp(System.Reflection.PropertyInfo property)
        {
            Console.WriteLine(Indicator + " [" + Name + "]\t" + Description);
        }
    }
}
