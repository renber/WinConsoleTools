using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/**
Run the command given in an elevated shell 
*/

namespace elevate
{
    class Program
    {        
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Parameter(s) missing");
                return;
            }            

            string workDir = Directory.GetCurrentDirectory();            
            bool elevateCmdItself = args[0].ToLower() == "cmd";

            string command;

            if (elevateCmdItself)
            {
                // keep the elevated command prompt open
                if (args.Length > 1)
                    command = "/K cd /d \"" + workDir + "\" && " + String.Join(" ", args.Skip(1));
                else
                    command = "/K cd /d \"" + workDir + "\"";                
            } else
                command = "/C cd /d \"" + workDir + "\" && " + String.Join(" ", args);

            // start cmd, switch to the current directory and execute the given command
            ProcessStartInfo pInfo = new ProcessStartInfo("cmd", command);
            pInfo.UseShellExecute = true;
            pInfo.Verb = "runas";

            if (!elevateCmdItself)
            {
                pInfo.CreateNoWindow = true;
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }            

            try
            {
                Process.Start(pInfo);
            } catch (Exception e)
            {
                Console.WriteLine("Elevation failed.");
            }
        }
    }
}
