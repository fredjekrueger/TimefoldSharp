namespace TimefoldSharp.Core.Config.Solver
{
    public interface AbstractConfig<Config> where Config : AbstractConfig<Config>
    {
        Config Inherit(Config inheritedConfig);

        Config CopyConfig();

        void VisitReferencedClasses(Action<Type> classVisitor);
    }
}