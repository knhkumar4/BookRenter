using BookRenterData.Context;
using BookRenterData.Repositories;
using BookRenterData.Repositories.Interfaces;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using JC.Samples.AsyncRepository.Repository;
using System;
using System.Threading.Tasks;

namespace BookRenterData.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BookRenterContext _dbContext;
        private IBookRepository _bookRepository;
        private ICartBookRepository _cartBookRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(BookRenterContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IBookRepository BookRepository
        {
            get
            {
                if (_bookRepository == null)
                {
                    _bookRepository = new BookRepository(_dbContext);
                }
                return _bookRepository;
            }
        }

        public ICartBookRepository CartBookRepository
        {
            get
            {
                if (_cartBookRepository == null)
                {
                    _cartBookRepository = new CartBookRepository(_dbContext);
                }
                return _cartBookRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_dbContext);
                }
                return _userRepository;
            }
        }
        

        public async Task CompleteAsync() => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    await _dbContext.DisposeAsync();
                }
            }
        }

        // Finalizer
        ~UnitOfWork()
        {
            DisposeAsync(false).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
