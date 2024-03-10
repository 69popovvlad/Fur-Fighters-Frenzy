namespace Core.Ioc
{
    public static class Factory
    {
        public static IContainer CreateContainer()
        {
            return new Container();
        }
    }
}