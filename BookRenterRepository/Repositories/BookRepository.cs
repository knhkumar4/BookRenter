using BookRenterData.Context;
using BookRenterData.Entities;
using BookRenterData.Repositories.Base;
using BookRenterData.Repositories.Interfaces;


namespace JC.Samples.AsyncRepository.Repository
{
    /// <summary>
    /// User Repository.
    /// </summary>
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dbContext">The Database Context</param>
        public BookRepository(BookRenterContext dbContext) : base(dbContext)
        {
        }

        #endregion
    }
}