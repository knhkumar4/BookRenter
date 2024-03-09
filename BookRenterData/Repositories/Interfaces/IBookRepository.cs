

using BookRenterData.Entities;
using BookRenterData.Repositories.Base.Interfaces;

namespace BookRenterData.Repositories.Interfaces
{
    /// <summary>
    /// User Repository Interface.
    /// </summary>
    public interface IBookRepository : IBaseRepository<Book>
    {

    }
}