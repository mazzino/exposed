using Exposed.DI.Modules;
using Autofac;

namespace Exposed.DI
{
    public class ExposedApplicationDi
    {
        private static ExposedApplicationDi _instance;
        private ILifetimeScope _globalScope;
        public IContainer Container { get; private set; }

        public static ExposedApplicationDi Instance
        {
            get { return _instance ?? (_instance = new ExposedApplicationDi()); }
        }

        private ExposedApplicationDi()
        {
            RegisterAndBuildAllObjects();
        }

        public ILifetimeScope ApplicationScope { get; private set; }

        public void BeginApplicationScope()
        {
            ApplicationScope = Container.BeginLifetimeScope("application");
        }

        public void RegisterAndBuildAllObjects()
        {
            var builder = new ContainerBuilder();

            //exposed
            builder.RegisterModule(new ExposedApplicationModule());
            builder.RegisterModule(new ExposedCommonModule());

            Container = builder.Build();
        }

        public void EndApplicationScope()
        {
            ApplicationScope.Dispose();
        }

    }
}