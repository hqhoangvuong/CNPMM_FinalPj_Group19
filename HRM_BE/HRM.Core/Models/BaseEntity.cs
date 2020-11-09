using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Core.Models
{
    public class BaseEntity<T>
    {
        [Key, Column(Order = 0)]
        public T Id { get; set; }
    }

    public class BaseEntity : BaseEntity<int>
    {

    }
}
