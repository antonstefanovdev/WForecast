public interface IRepository<T> : IDisposable where T : class
{
    IQueryable<T> DbSet { get; }
    Task<T?> FindByIdAsync(object id);
    Task AddAsync(T item, bool autoSaveChanges = true);
    Task AddAsync(IEnumerable<T> items, bool autoSaveChanges = true);
    Task UpdateAsync(T item, bool autoSaveChanges = true);
    Task UpdateAsync(IEnumerable<T> items, bool autoSaveChanges = true);
    Task DeleteAsync(T item, bool autoSaveChanges = true);
    Task DeleteAsync(IEnumerable<T> items, bool autoSaveChanges = true);
    Task SaveAsync();
}
