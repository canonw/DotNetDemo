namespace SharedLib.Managers
{
    public interface IHelloManager
    {
        string Say(string message);
    }

    public class HelloManager : IHelloManager
    {
        public string Say(string message)
        {
            return string.Format("SharedLib says {0}", message);
        }
    }
}
