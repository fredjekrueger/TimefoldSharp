namespace TimefoldSharp.Core.Config.Solver
{
    //[XmlAccessorType(XmlAccessType.FIELD)] JDEF
    public interface AbstractConfig<Config> where Config : AbstractConfig<Config>
    {
        Config Inherit(Config inheritedConfig);

        Config CopyConfig();

        void VisitReferencedClasses(Action<Type> classVisitor);
    }
}