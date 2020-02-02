using System;

namespace EntityGrabber
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IDataModel<T>
    {
        T Id { get; set; }
        Type GetIdType();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
