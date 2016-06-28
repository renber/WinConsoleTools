using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace wgrep.Arguments
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ConsoleArgAttribute : Attribute
    {
        public String Name { get; private set; }
        public String Description { get; set; }

        public ConsoleArgAttribute(String name, String description)
        {
            Name = name;
            Description = description;
        }

        public abstract int Priority { get; }

        public abstract void TryToApply(object targetInstance, PropertyInfo property, IList<String> argv);

        public abstract void PrintHelp(System.Reflection.PropertyInfo property);
    }
}
