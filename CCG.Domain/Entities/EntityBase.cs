using CCG.Domain.Contracts;

namespace CCG.Domain.Entities
{
    public abstract class EntityBase : IEntity<string>
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }
}