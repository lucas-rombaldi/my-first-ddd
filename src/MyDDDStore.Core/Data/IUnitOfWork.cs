using System.Threading.Tasks;

namespace MyDDDStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
