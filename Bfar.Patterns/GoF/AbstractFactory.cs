namespace Bfar.Patterns.Gof
{
    public abstract class AbstractFactory<T> where T : class, new()
    {
        public static T Factory { get { return new T(); } }
    }
}
