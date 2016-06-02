using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EasyLabWPF.ViewModel;

namespace EasyLabWPF.Common
{
    public class ViewModelModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MainWindowVM()).AsSelf().SingleInstance();
            builder.Register(c => new StartVM()).AsSelf();
            builder.Register(c => new ProductsVM()).AsSelf();
        }
    }
}
