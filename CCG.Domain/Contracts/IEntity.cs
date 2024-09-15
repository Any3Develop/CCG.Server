namespace CCG.Domain.Contracts
{
    public interface IEntity<out T>
    {
        public T Id { get; }
        public DateTime Created { get; }
        public DateTime Updated { get; set; }
    }
}