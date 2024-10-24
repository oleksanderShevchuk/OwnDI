using OwnDI.Placeholders.Interfaces;

namespace OwnDI.Placeholders
{
    public class AnotherServiceInstance : IAnotherService
    {
        private AnotherServiceInstance() { }
        public static AnotherServiceInstance Instance = new();
    }
}
