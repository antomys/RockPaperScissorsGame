using System;

namespace Services
{
    public abstract class ItemWithId<T> where T: class
    {
        public Guid Id { get; set; }
    }
}