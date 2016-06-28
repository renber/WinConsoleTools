using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wgrep.Arguments
{
    public class EnumArgAttribute : ConsoleArgAttribute
    {
        public override int Priority
        {
            get { return 128; }
        }

        Type _enumType;
        object _defaultValue;        

        /// <summary>
        /// Use EnumSwitch Attributes to declare the enum switches for this argument
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="defaultValue"></param>
        public EnumArgAttribute(Type enumType, object defaultValue, string name, string description)
            : base(name, description)
        {
            _enumType = enumType;
            _defaultValue = defaultValue;            
        }

        public override void PrintHelp(System.Reflection.PropertyInfo property)
        {
            Console.WriteLine(Name + "\t" + Description);
            Console.WriteLine("[ mutually exclusive");

            var switches = property.GetCustomAttributes(typeof(EnumSwitchAttribute), true).OfType<EnumSwitchAttribute>();
            foreach(var s in switches)
            {
                Console.WriteLine(" " + s.Switch + "\t" + s.Description);
            }

            Console.WriteLine("]");
        }

        public override void TryToApply(object targetInstance, System.Reflection.PropertyInfo property, IList<String> argv)
        {
            // get the switches            
            var switches = property.GetCustomAttributes(typeof(EnumSwitchAttribute), true).OfType<EnumSwitchAttribute>();

            // find the set switches
            HashSet<EnumSwitchAttribute> hashset = new HashSet<EnumSwitchAttribute>();
            foreach (var s in switches)
            {
                if (argv.Contains(s.Switch))
                    hashset.Add(s);
            }

            if (hashset.Count <= 1)
            {
                if (hashset.Count == 1)
                {
                    var v = hashset.First();
                    property.SetValue(targetInstance, v.EnumValue);
                    argv.Remove(v.Switch);
                } else
                    property.SetValue(targetInstance, _defaultValue);
            }
            else
            {
                String cs = String.Join(", ", hashset);                
                throw new ArgumentException("Contradicting command line options: " + cs.TrimEnd(','));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EnumSwitchAttribute : Attribute
    {
        public String Switch { get; private set; }
        public String Description { get; private set; }
        public Object EnumValue { get; private set; }

        public EnumSwitchAttribute(Object _enumValue, string _switch, string _description)
        {
            EnumValue = _enumValue;
            Switch = _switch;
            Description = _description;
        }
    }
}
