
using BookRenterData.Repositories.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace BookRenterData.UnitOfWork.Interfaces
{
    /// <summary>
    /// Unit of Work Interface.
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        IBookRepository BookRepository { get; }
        ICartBookRepository CartBookRepository { get; }
        IUserRepository UserRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        #region Properties

        Task CompleteAsync();

        #endregion
    }
}