namespace PresenceBackend.Repository;

public interface IRepository<T>
{
    Task<T?> FindOneById(Guid id);
}