using Exposed.Editor.Config;
using Autofac;
using Module = Autofac.Module;

namespace Exposed.Editor.DI.Modules
{
    public class ExposedEditorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<FieldsRenderer>().As<IFieldsRenderer>().SingleInstance();
            builder.RegisterType<ConfigFieldRenderer>().As<IConfigFieldRenderer>().SingleInstance();
        }
         
    }
}