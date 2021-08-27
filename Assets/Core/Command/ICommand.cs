namespace Core.Command
{
    public interface ICommand<in T>
    {
        void Execute(T commandData);
    }
}