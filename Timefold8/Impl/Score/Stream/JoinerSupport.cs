namespace TimefoldSharp.Core.Impl.Score.Stream
{
    public sealed class JoinerSupport
    {
        private static volatile JoinerService INSTANCE;

        public static JoinerService GetJoinerService()
        {
            if (INSTANCE == null)
            {
                var type = typeof(JoinerService);

                var types = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(s => s.GetTypes())
                       .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract).GetEnumerator();
                //var servicesIterator = ServiceLoader.Load<JoinerService>().GetEnumerator();
                if (!types.MoveNext())
                {
                    throw new InvalidOperationException("Joiners not found.\n"
                            + "Maybe include ai.timefold.solver:timefold-solver-constraint-streams dependency in your project?\n"
                            + "Maybe ensure your uberjar bundles META-INF/services from included JAR files?");
                }
                else
                {
                    INSTANCE = (JoinerService)Activator.CreateInstance(types.Current);
                }

            }
            return INSTANCE;
        }
    }
}
