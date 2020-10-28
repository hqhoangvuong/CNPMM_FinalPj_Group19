namespace HRM.Core.Models
{
    public interface BaseEntity<T>
    {
        public T Id { get; set; }
    }

    public interface BaseEntity : BaseEntity<int>
    {

    }
}
