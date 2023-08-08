namespace Wheel.Entities.Common
{
    public interface IEntity : IEntity<long>
    {

    }
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
    public class Entity<TKey> : IEntity<TKey>
    {
        public virtual TKey Id { get; set; } = default!;
    }
    public class Entity : IEntity
    {
        public virtual long Id { get; set; }
    }
}
