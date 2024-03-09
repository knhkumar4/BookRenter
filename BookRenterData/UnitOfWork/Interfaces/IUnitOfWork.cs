using BookRenterData.Repositories.Interfaces;
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
        #region Properties

        Task CompleteAsync();

        #endregion
    }
}