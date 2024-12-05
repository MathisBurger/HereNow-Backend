namespace PresenceBackend.Repository;

/// <summary>
/// Generic repo interface
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T>
{
    Task<T?> FindOneById(Guid id);
}