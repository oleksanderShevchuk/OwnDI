using OwnDI.Placeholders.Interfaces;

namespace OwnDI.Interfaces
{
    public interface IContainer
    {
        IScope CreateScope();
    }
}
