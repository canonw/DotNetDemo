namespace MultiTenantWebApi.Managers
{
    public interface IHelloManager
    {
        string Say(string message);
    }

    public class HelloManager : IHelloManager
    {
        public string Say(string message)
        {
            return string.Format("HelloManager says {0}", message);
        }
    }

    public class HellloManagerA : IHelloManager
    {
        public string Say(string message)
        {
            return string.Format("A says {0}", message);
        }
    }

    public class HellloManagerB : IHelloManager
    {
        public string Say(string message)
        {
            return string.Format("B says {0}", message);
        }
    }
}