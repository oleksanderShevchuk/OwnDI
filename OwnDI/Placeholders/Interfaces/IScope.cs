namespace OwnDI.Placeholders.Interfaces
{
    public interface IScope
    {
        object Resolve(Type service);
    }
}
