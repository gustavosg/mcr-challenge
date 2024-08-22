namespace Application.Interfaces
{
    public interface IMessageAdapter<in T> where T : class
    {
        string GetMessageAsync();
    }
}
