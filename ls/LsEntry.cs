using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls
{
    interface LsEntry
    {        
        string Text { get; }
        int Length { get; }
        ConsoleColor? Color { get; }
    }
}
