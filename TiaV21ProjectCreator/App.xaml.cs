using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TiaV21ProjectCreator
{
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var requestedName = new AssemblyName(args.Name);
            if (!requestedName.Name.StartsWith("Siemens.Engineering", StringComparison.OrdinalIgnoreCase))
                return null;

            string net48Dir = @"C:\Program Files\Siemens\Automation\Portal V21\PublicAPI\V21\net48";
            string candidatePath = Path.Combine(net48Dir, requestedName.Name + ".dll");

            if (File.Exists(candidatePath))
                return Assembly.LoadFrom(candidatePath);

            return null;
        }
    }
}