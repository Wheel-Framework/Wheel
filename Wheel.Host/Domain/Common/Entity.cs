namespace Wheel.Domain.Common
{
    public interface IEntity : IEntity<long>
    {

    }
    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
    public class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; } = default!;
    }
    public class Entity : IEntity
    {
        public virtual long Id { get; set; }
    }
}
