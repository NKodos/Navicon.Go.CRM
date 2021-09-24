using Microsoft.Xrm.Sdk;

namespace Navicon.Plugins.Interfaces
{
    public interface IService<in T> where T : Entity
    {
        void Execute(T entity);
    }
}