using System.Collections.Generic;

namespace EntityGrabber
{
    /// <summary>
    /// Interface for ModelRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="P"></typeparam>
    public interface IModelRepository<T, P>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        P Create(IDataModel<P> model);
        T Read(P id);
        List<T> ReadAll();
        bool Update(IDataModel<P> model);
        bool Delete(P id);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
