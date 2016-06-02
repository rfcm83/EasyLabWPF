using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using EasyLabWPF.ViewModel;

namespace EasyLabWPF.Common
{
    public class ViewModelLocator
    {
        private static readonly IContainer Container;

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ViewModelModule>();
            Container = builder.Build();
        }

        public MainWindowVM Main => Container.Resolve<MainWindowVM>();
        public StartVM Start => Container.Resolve<StartVM>();
        public ProductsVM Products => Container.Resolve<ProductsVM>();

        public static T Get<T>() where T : class
        {
            return Container.Resolve<T>();
        }

        public static T Get<T>(params Parameter[] parameters) where T : class
        {
            return Container.Resolve<T>(parameters);
        }

        public static T Get<T>(IEnumerable<Parameter> parameters) where T : class
        {
            return Container.Resolve<T>(parameters);
        }
    }
}