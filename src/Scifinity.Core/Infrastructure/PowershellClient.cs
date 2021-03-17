using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace Scifinity.Core.Infrastructure
{
    public class PowershellClient
    {       
        public void RunCommand(string commandText)
        {
            var currentShell = PowerShell.Create(RunspaceMode.NewRunspace);

            currentShell.AddScript(commandText);
            Collection<PSObject> results = currentShell.Invoke();
            if (currentShell.HadErrors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var error in currentShell.Streams.Error)
                {
                    Console.WriteLine("{0}", error);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var result in results)
            {
                Console.WriteLine("{0}", result);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
