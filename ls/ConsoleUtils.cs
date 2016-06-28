using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls
{
    public static class ConsoleUtils
    {

        private static Stack<ConsoleColor> ForegroundColors = new Stack<ConsoleColor>();

        public static void PushForegroundColor()
        {
            ForegroundColors.Push(Console.ForegroundColor);
        }

        public static void PushAndSetForegroundColor(ConsoleColor newColor)
        {
            PushForegroundColor();
            Console.ForegroundColor = newColor;
        }

        public static void PopForegroundColor()
        {
            Console.ForegroundColor = ForegroundColors.Pop();
        }

    }
}
