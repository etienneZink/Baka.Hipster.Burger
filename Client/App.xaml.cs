using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Baka.Hipster.Burger.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClass && (t.Namespace.Contains("Controllers") || t.Namespace.Contains("ViewModels") || t.Namespace.Contains("Views")));
            containerBuilder.RegisterInstance(this);
            //ToDo
            //containerBuilder.RegisterType<CustomerServiceClient>();
            Container = containerBuilder.Build();

            //Container.Resolve<MainWindowController>().Initialize();
        }
    }
}
