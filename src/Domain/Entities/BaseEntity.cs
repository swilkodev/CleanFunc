using System;

namespace CleanFunc.Domain.Entities
{
    public abstract class BaseEntity<TId>
    {

        public TId Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get;set;}

    }
}