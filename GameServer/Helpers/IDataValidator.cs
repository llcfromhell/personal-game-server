namespace GameServer.Helpers
{
    public interface IDataValidator<T>
    {
        bool IsValid(T data);
    }
}