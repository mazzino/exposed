using System.Collections.Generic;
using Exposed.Config;
using Exposed.Config.Processors.Core;
using Exposed.Config.Processors.Query;
using Exposed.Utils;
using Autofac;
using Module = Autofac.Module;

namespace Exposed.DI.Modules
{
    public class ExposedCommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ComponentObtainer>().As<IComponentObtainer>().SingleInstance();
            builder.RegisterType<FieldsObtainer>().As<IFieldsObtainer>().SingleInstance();
            builder.RegisterType<FieldsProcessor>().As<IFieldsProcessor>().SingleInstance();

            builder.Register(c => new PropertyProcessorManager(c.ResolveNamed<IEnumerable<IPropertyProcessor>>("core"),
                c.ResolveNamed<IEnumerable<IPropertyProcessor>>("custom"), 
                c.ResolveNamed<IPropertyProcessor>("query"),
                c.Resolve<IArrayTypeUtils>()))
                .As<IPropertyProcessorManager>().SingleInstance();

            //processors - autowiring
            builder.RegisterAssemblyTypes(typeof (ExposedApplicationDi).Assembly)
                .Where(t => t.Name.EndsWith("CustomPropertyProcessor") && typeof(PropertyProcessor).IsAssignableFrom(t))
                .Named<IPropertyProcessor>("custom")
                .SingleInstance();

            //core processors
            builder.RegisterType<ObjectOfTypePropertyProcessor>().Named<IPropertyProcessor>("core").SingleInstance();
            builder.RegisterType<TagPropertyProcessor>().Named<IPropertyProcessor>("core").SingleInstance();
            builder.RegisterType<ParentPropertyProcessor>().Named<IPropertyProcessor>("core").SingleInstance();
            builder.RegisterType<ChildPropertyProcessor>().Named<IPropertyProcessor>("core").SingleInstance();
            builder.RegisterType<SameObjectPropertyProcessor>().Named<IPropertyProcessor>("core").SingleInstance();

            //query processor
            builder.RegisterType<QueryPropertyProcessor>().Named<IPropertyProcessor>("query").SingleInstance();

            //utils
            builder.RegisterType<ArrayTypeUtils>().As<IArrayTypeUtils>().SingleInstance();
        }

    }
}