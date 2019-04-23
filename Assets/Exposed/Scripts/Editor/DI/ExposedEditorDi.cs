using Exposed.DI.Modules;
using Exposed.Editor.DI.Modules;
using Autofac;

namespace Exposed.Editor.DI
{
    public class ExposedEditorDi
    {
        private static ExposedEditorDi _instance;
        public IContainer Container { get; private set; }

        public static ExposedEditorDi Instance
        {
            get { return _instance ?? (_instance = new ExposedEditorDi()); }
        }

        private ExposedEditorDi()
        {
            var builder = new ContainerBuilder();

            //exposed
            builder.RegisterModule(new ExposedEditorModule());
            builder.RegisterModule(new ExposedCommonModule());

            Container = builder.Build();
        }

    }
}